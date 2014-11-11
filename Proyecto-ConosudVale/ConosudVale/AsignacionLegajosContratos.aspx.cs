using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;
using Telerik.Web.UI;
using Entidades;
using System.Linq;
using System.Data.Linq;

public partial class AsignacionLegajosContratos : System.Web.UI.Page
{
    public List<ContEmpLegajos> DatosLegajosAsignados
    {
        get
        {
            if (Session["DatosLegajosAsignados"] != null)
                return (List<ContEmpLegajos>)Session["DatosLegajosAsignados"];
            else
            {
                Session["DatosLegajosAsignados"] = new List<Legajos>();
                return (List<ContEmpLegajos>)Session["DatosLegajosAsignados"];
            }
        }
        set
        {
            Session["DatosLegajosAsignados"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadEmpresas();
            gvLegajosAsociados.DataSource = new List<ContEmpLegajos>();
            gvLegajosAsociados.DataBind();
            SetSeguridad();

            if (this.Session["TipoUsuario"].ToString() == "Cliente")
                this.lblTipoGestion.Text = "LISTADO PERSONAL ASIGNADOS";

        }
        gvLegajosAsociados.DataBound += new EventHandler(gvLegajosAsociados_DataBound);
        gvLegajosAsociados.ItemDataBound += new GridItemEventHandler(gvLegajosAsociados_ItemDataBound);

    }

    void gvLegajosAsociados_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            Entidades.ContEmpLegajos contratoemp = EntityDataSourceExtensions.GetItemObject<Entidades.ContEmpLegajos>(e.Item.DataItem);

            if (contratoemp.FechaTramiteBaja.HasValue)
            {
                e.Item.Style.Add(HtmlTextWriterStyle.Color, "Red");
                e.Item.ToolTip = "Fecha Baja: " + contratoemp.FechaTramiteBaja.Value.ToShortDateString();
            }

        }

    }

    void gvLegajosAsociados_DataBound(object sender, EventArgs e)
    {
        SetSeguridad();
    }

    protected void gvLegajosAsociados_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == "ExportLegajos")
        {
            this.gvLegajosAsociados.DataSource = DatosLegajosAsignados;
            this.gvLegajosAsociados.DataBind();

            ConfigureExportAndExport();
        }
    }

    private void SetSeguridad()
    {
        long idUsuario = long.Parse(Session["idusu"].ToString());
        Entidades.SegRolMenu PermisosPagina = Helpers.GetPermisosAcciones(Helpers.Constantes.PaginaMenu_.AsignacionLegajos, idUsuario);

        LinkButton btnAccionE = (LinkButton)gvLegajosAsociados.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnEliminar");
        btnAccionE.Visible = PermisosPagina.Modificacion;

        LinkButton btnAccionA = (LinkButton)gvLegajosAsociados.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnAsignar");
        btnAccionA.Visible = PermisosPagina.Creacion;

        LinkButton btnAccionEnc = (LinkButton)gvLegajosAsociados.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnCambiarEncuadre");
        btnAccionEnc.Visible = PermisosPagina.Modificacion;

        if (cboPeriodos.Text != "")
        {
            btnCopiado.Visible = PermisosPagina.Creacion;
            upAsignar.Update();
        }

        if (PermisosPagina.Creacion)
        {
            ///Si administrador puede asignar y desasiganar en el mesa actual y hacia atras.
            ///Si no es administrador puede desasignar en el mes actual y hacia atras, 
            ///PERO NO puede asignar hacia atras solo en el mes actual.
            EntidadesConosud db = new EntidadesConosud();
            string DescRol = Helpers.RolesEspeciales.Administrador.ToString();
            int RolesAdministrador = (from U in db.SegUsuario
                                      from UR in U.SegUsuarioRol
                                      where U.IdSegUsuario == idUsuario
                                      && UR.SegRol.Descripcion == DescRol
                                      select UR).Count();


            if (RolesAdministrador == 0 && cboPeriodos.Text != "" && cboPeriodos.Text != string.Format("{0:yyyy/MM}", DateTime.Now))
            {
                btnAccionE.Visible = false;
                btnAccionA.Visible = false;
            }
        }

    }

    private void LoadEmpresas()
    {

        cboEmpresas.DataTextField = "RazonSocial";
        cboEmpresas.DataValueField = "IdEmpresa";
        cboEmpresas.DataSource = Helpers.GetEmpresas(long.Parse(Session["idusu"].ToString()));
        cboEmpresas.DataBind();

        cboEmpresas.Items.Insert(0, new RadComboBoxItem("- Seleccione una Empresa -"));

    }

    private void LoadContratos(int id)
    {

        cboContratos.DataTextField = "Codigo";
        cboContratos.DataValueField = "IdContrato";
        cboContratos.DataSource = (from c in Helpers.GetContratos(id, long.Parse(Session["idusu"].ToString()))
                                   select new
                                   {
                                       IdContrato = c.IdContrato,
                                       Codigo = c.Codigo + " (" + string.Format("{0:MM/yyyy}", c.FechaInicio.Value) + " - " + string.Format("{0:MM/yyyy}", (c.Prorroga.HasValue ? c.Prorroga.Value : c.FechaVencimiento.Value)) + ")"
                                   }).ToList();
        cboContratos.DataBind();

        cboContratos.Items.Insert(0, new RadComboBoxItem("- Seleccione un Contrato -"));

    }

    private void LoadContratistas(int id)
    {
        using (EntidadesConosud db = new EntidadesConosud())
        {


            this.cboContratistas.DataTextField = "RazonSocial";
            this.cboContratistas.DataValueField = "IdContratoEmpresas";
            this.cboContratistas.DataSource = (from C in db.ContratoEmpresas
                                               where C.Contrato.IdContrato == id
                                               && C.CabeceraHojasDeRuta.Count() > 0
                                               select new { C.IdContratoEmpresas, C.Empresa.RazonSocial, cabeceras = C.CabeceraHojasDeRuta.Count }).ToList();// Helpers.GetContratistas(id);
            this.cboContratistas.DataBind();

            this.cboContratistas.Items.Insert(0, new RadComboBoxItem("- Seleccione un Contratista -"));
        }
    }

    private void LoadPeriodos(int id)
    {
        cboPeriodos.DataTextFormatString = "{0:yyyy/MM}";
        cboPeriodos.DataTextField = "Periodo";
        cboPeriodos.DataValueField = "IdCabeceraHojasDeRuta";
        cboPeriodos.DataSource = Helpers.GetPeriodosAsignacionLegajos(id, long.Parse(this.Session["idusu"].ToString()));
        cboPeriodos.DataBind();
        cboPeriodos.SelectedIndex = 0;

    }

    protected void cboContratos_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        LoadContratos(int.Parse(e.Text));
    }

    protected void cboContratistas_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        LoadContratistas(int.Parse(e.Text));
    }

    protected void cboPriodos_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        LoadPeriodos(int.Parse(e.Text));
    }

    public void ConfigureExportAndExport()
    {
        foreach (Telerik.Web.UI.GridColumn column in gvLegajosAsociados.MasterTableView.Columns)
        {
            if ((!column.Visible || !column.Display) && column.HeaderText != "")
            {
                column.Visible = true;
                column.Display = true;
            }
            else if (column.HeaderText == "")
            {
                column.Visible = false;
                column.Display = false;
            }
        }

        gvLegajosAsociados.ExportSettings.ExportOnlyData = true;
        gvLegajosAsociados.ExportSettings.IgnorePaging = true;
        gvLegajosAsociados.ExportSettings.FileName = "LegajosAsignados_" + cboPeriodos.Text;
        gvLegajosAsociados.MasterTableView.ExportToExcel();



    }

    public void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument == "ActualizarGrilla")
        {
            CargarGrilla();

        }
        else if (e.Argument == "DeleteSelected")
        {

            DateTime periodo = DateTime.Parse(cboPeriodos.Text + "/01");
            long idContratoEmpresa = Convert.ToInt32(cboContratistas.SelectedValue);
            long idLegajo = long.Parse(gvLegajosAsociados.Items[gvLegajosAsociados.SelectedItems[0].DataSetIndex].GetDataKeyValue("Legajos.IdLegajos").ToString());
            long idContempLeg = long.Parse(gvLegajosAsociados.Items[gvLegajosAsociados.SelectedItems[0].DataSetIndex].GetDataKeyValue("IdContEmpLegajos").ToString());

            EntidadesConosud dc = new EntidadesConosud();


            var contEmpLegajos = from C in dc.ContEmpLegajos
                                 where C.ContratoEmpresas.IdContratoEmpresas == idContratoEmpresa
                                 && C.Legajos.IdLegajos == idLegajo
                                 && C.CabeceraHojasDeRuta.Periodo >= periodo
                                 select C;

            foreach (Entidades.ContEmpLegajos cont in contEmpLegajos)
            {
                if (idContempLeg == cont.IdContEmpLegajos)
                {
                    cont.FechaTramiteBaja = DateTime.Now;
                }
                else
                {
                    dc.DeleteObject(cont);
                }
            }


            /// Limpio la fecha de vencimiento de la credencial.
            var legajo = (from L in dc.Legajos
                          where L.IdLegajos == idLegajo
                          select L).FirstOrDefault();

            legajo.CredVencimiento = null;


            dc.SaveChanges();
            CargarGrilla();
            ScriptManager.RegisterStartupScript(upGrilla, typeof(UpdatePanel), "acr", "ActualizarXML();", true);

        }
        else if (e.Argument == "CargaGrilla")
        {
            CargarGrilla();
        }
        else if (e.Argument != "undefined")
        {
            DateTime periodo = DateTime.Parse(cboPeriodos.Text + "/01");
            long idContratoEmpresa = Convert.ToInt32(cboContratistas.SelectedValue);
            //long idEstado = 15;
            long idLegajo = long.Parse(e.Argument);
            long idEmpresa = long.Parse(cboEmpresas.SelectedValue);

            EntidadesConosud dcLocal = new EntidadesConosud();

            var cabeceras = (from C in dcLocal.CabeceraHojasDeRuta.Include("ContratoEmpresas")
                            where C.Periodo >= periodo
                            && C.ContratoEmpresas.IdContratoEmpresas == idContratoEmpresa
                            //&& C.Estado.IdClasificacion == idEstado
                            select C).ToList();

            Entidades.Legajos legAsociado = (from L in dcLocal.Legajos
                                             where L.IdLegajos == idLegajo
                                             select L).First<Entidades.Legajos>();

            Contrato contratoAsignado = cabeceras.FirstOrDefault().ContratoEmpresas.Contrato;

            foreach (Entidades.CabeceraHojasDeRuta cab in cabeceras)
            {
                List<ContEmpLegajos> legajosExistentes = cab.ContEmpLegajos.ToList();
                ContEmpLegajos legAsigndoExistente = legajosExistentes.Where(w => w.IdLegajos == legAsociado.IdLegajos).FirstOrDefault();
                if (legAsigndoExistente== null)
                {
                    Entidades.ContEmpLegajos ContEmpLeg = new Entidades.ContEmpLegajos();
                    ContEmpLeg.Legajos = legAsociado;
                    ContEmpLeg.ContratoEmpresas = cab.ContratoEmpresas;
                    ContEmpLeg.CabeceraHojasDeRuta = cab;
                    dcLocal.AddToContEmpLegajos(ContEmpLeg);
                }
                else
                {
                    legAsigndoExistente.FechaTramiteBaja = null;
                }
            }


            /// 91: Categoria del contrato Auditable al ingreso, se debe colocar como 
            /// vencimiento de la credencial la fecha de asignación mas un mes, caso contrario
            /// la fecha de vencimiento del contrato.
            if (contratoAsignado.objCategoria.IdClasificacion != 91)
            {
                if (contratoAsignado.Prorroga.HasValue)
                {
                    legAsociado.CredVencimiento = contratoAsignado.Prorroga;
                }
                else
                {
                    legAsociado.CredVencimiento = contratoAsignado.FechaVencimiento;
                }
            }
            else
                legAsociado.CredVencimiento = DateTime.Now.AddMonths(1);




            dcLocal.SaveChanges();
            txtNroDocToolTip.Text = "";
            CargarGrilla();

        }



    }

    private void CargarGrilla()
    {
        EntidadesConosud dcLocal = new EntidadesConosud();
        long Id = long.Parse(cboPeriodos.SelectedValue);
        long IdContratoEmpresa = long.Parse(cboContratistas.SelectedValue);
        long idEmpresa = long.Parse(cboEmpresas.SelectedValue);

        List<ContEmpLegajos> conEmpLeg = (from it in dcLocal.ContEmpLegajos
                                          where it.CabeceraHojasDeRuta.IdCabeceraHojasDeRuta == Id
                                          && it.ContratoEmpresas.IdContratoEmpresas == IdContratoEmpresa
                                          && it.Legajos != null
                                          orderby it.Legajos.Apellido
                                          select it).ToList();

        DatosLegajosAsignados = conEmpLeg;
        gvLegajosAsociados.DataSource = DatosLegajosAsignados;
        gvLegajosAsociados.DataBind();
        upGrilla.Update();
        Helpers.GeneracionXmlLegajos(Server.MapPath(""), idEmpresa);
    }

}
