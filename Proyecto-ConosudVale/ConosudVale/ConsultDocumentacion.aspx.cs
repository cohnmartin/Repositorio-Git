using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;
using Entidades;

public partial class ConsultDocumentacion : System.Web.UI.Page
{
    private EntidadesConosud _dc = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadEmpresas();
            CargarGrilla();
        }
    }

    private void LoadEmpresas()
    {

        this.cboEmpresas.DataTextField = "RazonSocial";
        this.cboEmpresas.DataValueField = "IdEmpresa";
        this.cboEmpresas.DataSource = Helpers.GetEmpresasContratistas(long.Parse(Session["idusu"].ToString()));
        this.cboEmpresas.DataBind();

        this.cboEmpresas.Items.Insert(0, new RadComboBoxItem("- Seleccione una Empresa -"));

    }

    private void LoadContratos(int id)
    {

        this.cboContratos.DataTextField = "Codigo";
        this.cboContratos.DataValueField = "IdContrato";
        this.cboContratos.DataSource = (from c in Helpers.GetContratos(id, long.Parse(Session["idusu"].ToString()))
                                        select new
                                        {
                                            IdContrato = c.IdContrato,
                                            Codigo = c.Codigo + " (" + string.Format("{0:MM/yyyy}", c.FechaInicio.Value) + " - " + string.Format("{0:MM/yyyy}", (c.Prorroga.HasValue ? c.Prorroga.Value : c.FechaVencimiento.Value)) + ")"
                                        }).ToList();
        this.cboContratos.DataBind();

        this.cboContratos.Items.Insert(0, new RadComboBoxItem("- Seleccione un Contrato -"));

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
        this.cboPeriodos.DataTextFormatString = "{0:yyyy/MM}";
        this.cboPeriodos.DataTextField = "Periodo";
        this.cboPeriodos.DataValueField = "IdCabeceraHojasDeRuta";
        this.cboPeriodos.DataSource = Helpers.GetPeriodos(id, long.Parse(this.Session["idusu"].ToString()));
        this.cboPeriodos.DataBind();

        this.cboPeriodos.Items.Insert(0, new RadComboBoxItem("- Seleccione un Periodo -"));
    }

    protected void cboEmpresas_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        LoadEmpresas();
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

    protected void btnAplicar_Click(object sender, EventArgs arg)
    {
        _dc = new EntidadesConosud();
        foreach (GridItem item in gvItemHoja.Items)
        {

            if ((item.FindControl("chkPresento") as CheckBox).Checked)
            {
                long id = long.Parse(gvItemHoja.Items[item.DataSetIndex].GetDataKeyValue("IdHojaDeRuta").ToString());

                Entidades.HojasDeRuta itemsHoja = (from H in _dc.HojasDeRuta
                                                   where H.IdHojaDeRuta == id
                                                   select H).First<Entidades.HojasDeRuta>();

                itemsHoja.DocFechaEntrega = DateTime.Now;
                itemsHoja.DocComentario = "Sin Comentarios";
                (item.FindControl("chkPresento") as CheckBox).Checked = false;

                /// al presnetar documentación para una hoja de ruta que esta publicada
                /// se des-publica automaticamente.
                itemsHoja.CabeceraHojasDeRutaReference.Load();
                itemsHoja.CabeceraHojasDeRuta.Publicar = false;
                itemsHoja.CabeceraHojasDeRuta.EsFueraTermino = chkFueraTermino.Checked;

                itemsHoja.CabeceraHojasDeRuta.Publicar = false;
            }
        }
        _dc.SaveChanges();
        CargarGrilla();
        upGrilla.Update();

    }

    protected void gvItemHoja_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.EditFormItem)
        {
            ////Carga de combos
        }
    }

    protected void gvItemHoja_ItemCommand(object source, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.PerformInsertCommandName || e.CommandName == RadGrid.UpdateCommandName)
        {
            if (e.Item is GridEditFormItem)
            {
                GridEditableItem editedItem = e.Item as GridEditableItem;
                UpdateDoc(editedItem);
                this.gvItemHoja.MasterTableView.ClearEditItems();
            }
        }
        else if (e.CommandName == RadGrid.InitInsertCommandName)
        {
            this.gvItemHoja.MasterTableView.ClearEditItems();
        }
        else if (e.CommandName == RadGrid.EditCommandName || e.CommandName == RadGrid.CancelCommandName)
        {
            CargarGrilla();
            upGrilla.Update();
        }


    }

    protected void gvItemHoja_DataBound(object sender, EventArgs e)
    {
        long idUsuario = long.Parse(Session["idusu"].ToString());
        Entidades.SegRolMenu PermisosPagina = Helpers.GetPermisosAcciones(Helpers.Constantes.PaginaMenu_.Documentacion, idUsuario);

        if (PermisosPagina.Lectura && !PermisosPagina.Creacion && !PermisosPagina.Modificacion)
        {
            gvItemHoja.MasterTableView.GetColumn("imgEdit").Visible = false;
            gvItemHoja.MasterTableView.GetColumn("chkPresentoColumn").Visible = false;

        }
    }

    private void CargarGrilla()
    {
        if (this.cboPeriodos.SelectedValue.Length > 0)
        {
            _dc = new EntidadesConosud();

            int Id = int.Parse(this.cboPeriodos.SelectedValue);

            var cabecera = (from C in _dc.CabeceraHojasDeRuta
                            where C.IdCabeceraHojasDeRuta == Id
                            select C).FirstOrDefault();

            if (cabecera.EsFueraTermino.HasValue)
                chkFueraTermino.Checked = cabecera.EsFueraTermino.Value;

            upFueraTermino.Update();

            this.gvItemHoja.DataSource = (from S in _dc.HojasDeRuta.Include("Plantilla").Include("CabeceraHojasDeRuta")
                                          where S.CabeceraHojasDeRuta.IdCabeceraHojasDeRuta == Id
                                          orderby S.Plantilla.Codigo
                                          select S).ToList();
            this.gvItemHoja.DataBind();
        }
        else
        {
            this.gvItemHoja.DataSource = new List<HojasDeRuta>();
            this.gvItemHoja.DataBind();
        }

    }

    public void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument == "ActualizarGrilla")
        {
            CargarGrilla();
            upGrilla.Update();
        }
    }

    private void UpdateDoc(GridEditableItem editedItem)
    {

        _dc = new EntidadesConosud();
        GridEditManager editMan = editedItem.EditManager;

        string id = (editMan.GetColumnEditor("Id") as GridTextBoxColumnEditor).Text;
        long idHR = Convert.ToInt64(id);

        HojasDeRuta _hojaRuta = (from c in _dc.HojasDeRuta
                                 where c.IdHojaDeRuta == idHR
                                 select c).FirstOrDefault();

        if (_hojaRuta != null)
        {

            #region Recupero los  Datos Ingresado por el usuario
            DateTime? DocFechaEntrega = (editedItem.FindControl("TextBoxFechaEntrega") as RadDatePicker).SelectedDate;
            string TextBoxDocComentario = (editedItem.FindControl("TextBoxDocComentario") as TextBox).Text;

            #endregion

            try
            {
                _hojaRuta.DocFechaEntrega = DocFechaEntrega;
                _hojaRuta.DocComentario = TextBoxDocComentario;

                _dc.SaveChanges();
                CargarGrilla();
                upGrilla.Update();
            }
            catch (Exception e)
            {
                ScriptManager.RegisterStartupScript(this.upGrilla, typeof(UpdatePanel), "Error Grabacion", "alert(" + e.Message + ")", true);

            }
        }
    }

}
