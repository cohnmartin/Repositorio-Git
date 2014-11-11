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
    private EntidadesConosud _Ds = new EntidadesConosud();
    private bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroupMae()
    {
        return this.gvContratos.MasterTableView.FilterExpression != "" ||
            (this.gvContratos.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            this.gvContratos.MasterTableView.SortExpressions.Count > 0;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadEmpresas();
        }
    }


    protected void gvContratos_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        long idEmp = 0;
        int count = 0;
        if (this.cboEmpresas.SelectedValue.Length > 0)
        {
            idEmp = Convert.ToInt64(this.cboEmpresas.SelectedValue);
            count = (from S in _Ds.ContratoEmpresas where S.Empresa.IdEmpresa == idEmp && S.EsContratista == true select S).Count();
        }
        else
        {
            count = (from S in _Ds.ContratoEmpresas where S.EsContratista == true select S).Count();
        }

        this.gvContratos.VirtualItemCount = count;

        int startRowIndex = (ShouldApplySortFilterOrGroupMae()) ?
            0 : this.gvContratos.CurrentPageIndex * this.gvContratos.PageSize;

        int maximumRows = (ShouldApplySortFilterOrGroupMae()) ?
            count : this.gvContratos.PageSize;

        this.gvContratos.AllowCustomPaging = !ShouldApplySortFilterOrGroupMae();
        if (this.cboEmpresas.SelectedValue.Length > 0)
        {
            idEmp = Convert.ToInt64(this.cboEmpresas.SelectedValue);
            this.gvContratos.DataSource = (from S in _Ds.ContratoEmpresas.Include("Contrato").Include("Empresa")
                                           where S.Empresa.IdEmpresa == idEmp && S.EsContratista == true
                                           orderby S.Empresa.RazonSocial
                                           select S).Skip(startRowIndex).Take(maximumRows).ToList();
        }
        else
        {
            this.gvContratos.DataSource = (from S in _Ds.ContratoEmpresas.Include("Contrato").Include("Empresa")
                                           where S.EsContratista == true
                                           orderby S.Empresa.RazonSocial
                                           select S).Skip(startRowIndex).Take(maximumRows).ToList();
        }

        upGrilla.Update();
    }

    private void LoadEmpresas()
    {

        cboEmpresas.DataTextField = "RazonSocial";
        cboEmpresas.DataValueField = "IdEmpresa";
        cboEmpresas.DataSource = Helpers.GetEmpresasContratistas(long.Parse(Session["idusu"].ToString()));
        cboEmpresas.DataBind();

        cboEmpresas.Items.Insert(0, new RadComboBoxItem("- Seleccione una Empresa -"));

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

    protected void cboEmpresas_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        gvContratos.Rebind();
        upGrilla.Update();
    }
}