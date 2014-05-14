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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Helpers.GeneracionXmlLegajos(Server.MapPath(""));
            LoadEmpresas();
            
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
        long idUsuario = long.Parse(Session["idusu"].ToString());
        Entidades.SegRolMenu PermisosPagina = Helpers.GetPermisosAcciones(Helpers.Constantes.PaginaMenu_.AsignacionLegajos, idUsuario);

        LinkButton btnAccion = (LinkButton)gvLegajosAsociados.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnEliminar");
        btnAccion.Visible = PermisosPagina.Modificacion;

        btnAccion = (LinkButton)gvLegajosAsociados.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnAsignar");
        btnAccion.Visible = PermisosPagina.Creacion;

        if (cboPeriodos.Text != "")
        {
            btnCopiado.Enabled = true;
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
                btnAccion.Visible = false;
            
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
        cboContratos.DataSource = Helpers.GetContratos(id);
        cboContratos.DataBind();

        cboContratos.Items.Insert(0, new RadComboBoxItem("- Seleccione un Contrato -"));

    }

    private void LoadContratistas(int id)
    {
        cboContratistas.DataTextField = "RazonSocial";
        cboContratistas.DataValueField = "IdContratoEmpresas";
        cboContratistas.DataSource = Helpers.GetContratistas(id);
        cboContratistas.DataBind();

        cboContratistas.Items.Insert(0, new RadComboBoxItem("- Seleccione un Contratista -"));
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

    public void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument == "ActualizarGrilla")
        {
            gvLegajosAsociados.Rebind();
            upGrilla.Update();
        
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

            dc.SaveChanges();
            gvLegajosAsociados.Rebind();
            upGrilla.Update();

            //gvLegajosAsociados.Rebind();

            ScriptManager.RegisterStartupScript(upGrilla, typeof(UpdatePanel), "acr", "ActualizarXML();", true);
            Helpers.GeneracionXmlLegajos(Server.MapPath(""));

        }
        else if (e.Argument != "undefined")
        {
            DateTime periodo = DateTime.Parse(cboPeriodos.Text + "/01");
            long idContratoEmpresa = Convert.ToInt32(cboContratistas.SelectedValue);
            long idEstado = 15;
            long idLegajo = long.Parse(e.Argument);

            EntidadesConosud dcLocal = new EntidadesConosud();

            var cabeceras = from C in dcLocal.CabeceraHojasDeRuta.Include("ContratoEmpresas")
                            where C.Periodo >= periodo
                            && C.ContratoEmpresas.IdContratoEmpresas == idContratoEmpresa
                            && C.Estado.IdClasificacion == idEstado
                            select C;
           
            Entidades.Legajos legAsociado = (from L in dcLocal.Legajos
                                             where L.IdLegajos == idLegajo
                                             select L).First<Entidades.Legajos>();

            foreach (Entidades.CabeceraHojasDeRuta cab in cabeceras)
            {
              

                Entidades.ContEmpLegajos ContEmpLeg = new Entidades.ContEmpLegajos();
                ContEmpLeg.Legajos = legAsociado;
                ContEmpLeg.ContratoEmpresas = cab.ContratoEmpresas;
                ContEmpLeg.CabeceraHojasDeRuta = cab;
                dcLocal.AddToContEmpLegajos(ContEmpLeg);
            }

            dcLocal.SaveChanges();
            txtNroDocToolTip.Text = "";
            gvLegajosAsociados.Rebind();
            upGrilla.Update();
            Helpers.GeneracionXmlLegajos(Server.MapPath(""));
           
        }

    }
 
}
