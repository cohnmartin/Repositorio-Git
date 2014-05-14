using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Telerik.Web.UI;
using System.Collections;

public partial class ABMUsuarios2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        grillaUsuarios.DataBound += new EventHandler(grillaUsuarios_DataBound);
    }

    void grillaUsuarios_DataBound(object sender, EventArgs e)
    {
        long idUsuario = long.Parse(Session["idusu"].ToString());
        Entidades.SegRolMenu PermisosPagina = Helpers.GetPermisosAcciones(Helpers.Constantes.PaginaMenu_.GestionUsuarios, idUsuario);


        LinkButton btnAccion = (LinkButton)grillaUsuarios.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnEdit");
        btnAccion.Visible = PermisosPagina.Modificacion;

        btnAccion = (LinkButton)grillaUsuarios.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnInsert");
        btnAccion.Visible = PermisosPagina.Creacion;

        btnAccion = (LinkButton)grillaUsuarios.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("btnDelete");
        btnAccion.Visible = PermisosPagina.Creacion;

        if (!PermisosPagina.Modificacion)
        {
            grillaUsuarios.MasterTableView.GetColumn("MenuColumn").Visible = false;
        }
        
    }
}
