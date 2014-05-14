using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class ABMRoles2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        grillaRoles.DataBound += new EventHandler(grillaRoles_DataBound);
    }

    void grillaRoles_DataBound(object sender, EventArgs e)
    {
        long idUsuario = long.Parse(Session["idusu"].ToString());
        Entidades.SegRolMenu PermisosPagina = Helpers.GetPermisosAcciones(Helpers.Constantes.PaginaMenu_.GestionRoles, idUsuario);


        LinkButton btnAccion = (LinkButton)grillaRoles.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnEdit");
        btnAccion.Visible = PermisosPagina.Modificacion;

        btnAccion = (LinkButton)grillaRoles.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnInsert");
        btnAccion.Visible = PermisosPagina.Creacion;

        btnAccion = (LinkButton)grillaRoles.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnDelete");
        btnAccion.Visible = PermisosPagina.Creacion;

        if (!PermisosPagina.Modificacion)
        {
            grillaRoles.MasterTableView.GetColumn("MenuColumn").Visible = false;
        }
    }
}
