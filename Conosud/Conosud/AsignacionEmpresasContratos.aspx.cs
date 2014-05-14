using System;
using System.Data;
using System.Configuration;
using System.Collections;
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


public partial class AsignacionEmpresasContratos : System.Web.UI.Page
{
    private static DSConosud.ContratoEmpresasDataTable _DTcontratoEmpresas;
    private EntidadesConosud _Ds = new EntidadesConosud();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadEmpresas();
        }
    }

    private void LoadEmpresas()
    {

        cboEmpresas.DataTextField = "RazonSocial";
        cboEmpresas.DataValueField = "IdEmpresa";
        cboEmpresas.DataSource = Helpers.GetEmpresasContratistas(long.Parse(Session["idusu"].ToString()));
        cboEmpresas.DataBind();

        cboEmpresas.Items.Insert(0, new RadComboBoxItem("- Seleccione una Empresa -"));

    }
    
    protected void ODSContratoEmpresa_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (((DSConosud.ContratoEmpresasDataTable)e.ReturnValue).Count > 0 )
            _DTcontratoEmpresas = (DSConosud.ContratoEmpresasDataTable)e.ReturnValue;
    }
  
    protected void gvContratos_DataBound(object sender, EventArgs e)
    {
        foreach (GridDataItem item in gvContratos.Items)
	    {
            long idContrato = long.Parse(item.GetDataKeyValue("Contrato.IdContrato").ToString());

            var subcotr = from S in _Ds.ContratoEmpresas.Include("Contrato").Include("Empresa").Include("CabeceraHojasDeRuta")
                          where S.Contrato.IdContrato == idContrato
                           && S.EsContratista == false
                          select S;
        

            string nombre = "";
            string cuit = "";
            foreach (ContratoEmpresas sub in subcotr)
            {

                if (sub.CabeceraHojasDeRuta.Count > 0)
                {
                    string DefinicionPeriodo = string.Format(" Desde: {0:MM/yyyy}", sub.CabeceraHojasDeRuta.OrderBy(w => w.Periodo).First().Periodo);
                    DefinicionPeriodo += " - " + string.Format(" Hasta: {0:MM/yyyy}", sub.CabeceraHojasDeRuta.OrderBy(w => w.Periodo).Last().Periodo);

                    cuit += sub.Empresa.CUIT + "|";
                    nombre += sub.Empresa.RazonSocial + DefinicionPeriodo + "|";
                }
                
            }

            (item.FindControl("imgSubContratistas") as ImageButton).Attributes.Add("CUIT-SubContratista", cuit);
            (item.FindControl("imgSubContratistas") as ImageButton).Attributes.Add("Rz-SubContratista", nombre);


	    }

        long idUsuario = long.Parse(Session["idusu"].ToString());
        Entidades.SegRolMenu PermisosPagina = Helpers.GetPermisosAcciones(Helpers.Constantes.PaginaMenu_.AsignacionSubContratistas, idUsuario);


        LinkButton btnAccion = (LinkButton)gvContratos.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnEliminar");
        btnAccion.Visible = PermisosPagina.Modificacion;

        btnAccion = (LinkButton)gvContratos.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnAsignar");
        btnAccion.Visible = PermisosPagina.Creacion;

    }

    public void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        gvContratos.Rebind();
        upGrilla.Update();
    }
}