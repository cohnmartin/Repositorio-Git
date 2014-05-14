using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Entidades;

public partial class ABMClasificaciones : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RadGrid1.DataBound += new EventHandler(RadGrid1_DataBound);
    }

    void RadGrid1_DataBound(object sender, EventArgs e)
    {
        long idUsuario = long.Parse(Session["idusu"].ToString());
        Entidades.SegRolMenu PermisosPagina = Helpers.GetPermisosAcciones(Helpers.Constantes.PaginaMenu_.Varios, idUsuario);


        LinkButton btnAccion = (LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnEdit");
        btnAccion.Visible = PermisosPagina.Modificacion;

        btnAccion = (LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnInsert");
        btnAccion.Visible = PermisosPagina.Creacion;

        btnAccion = (LinkButton)RadGrid1.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnDelete");
        btnAccion.Visible = PermisosPagina.Creacion;
    }


    protected void RadGrid1_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["idsel"] = RadGrid1.SelectedValue;
    }
    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        ViewState["idsel"] = null;
    }
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        this.RadGrid1.Rebind();
        this.updpnlGrilla.Update();
    }

    protected void EntityDataSourceClasificaciones_Selecting(object sender, EntityDataSourceSelectingEventArgs e)
    {
        if (this.cboTipoParametro.Text == "")
        {
            e.DataSource.Where = "";
            e.DataSource.AutoGenerateWhereClause = true;
        }

    }
}
