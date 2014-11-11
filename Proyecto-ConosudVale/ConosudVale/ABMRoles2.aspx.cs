using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Entidades;
using System.Collections;

public partial class ABMRoles2 : System.Web.UI.Page
{
    private EntidadesConosud _Contexto;

    public EntidadesConosud Contexto
    {
        get
        {

            if (_Contexto == null)
            {
                _Contexto = new EntidadesConosud();
                return _Contexto;
            }
            else
                return _Contexto;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void grillaRoles_DataBound(object sender, EventArgs e)
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
    protected void grillaRoles_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        this.grillaRoles.DataSource = (from c in Contexto.SegRol select c).ToList();
    }
    protected void grillaRoles_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridItem)
        {
            if ((e.Item.FindControl("ApellidoLabel") as Label) != null && 
                (
                
                (e.Item.FindControl("ApellidoLabel") as Label).Text.ToUpper() == "ADMINISTRADOR" ||
                (e.Item.FindControl("ApellidoLabel") as Label).Text.ToUpper() == "PUBLICADOR" ||
                (e.Item.FindControl("ApellidoLabel") as Label).Text.ToUpper() == "APROBADOR" 
                
                ))
            {
                (e.Item.FindControl("ApellidoLabel") as Label).ForeColor = System.Drawing.Color.Red;
            }
        }
    }
    protected void grillaRoles_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["idsel"] = this.grillaRoles.SelectedValue;
    }
    protected void grillaRoles_ItemCommand(object source, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.PerformInsertCommandName
            || e.CommandName == RadGrid.UpdateCommandName)
        {
            if (e.Item is GridEditFormInsertItem)
            {
                GridEditFormInsertItem editedItem = e.Item as GridEditFormInsertItem;
                InsertRoles(editedItem);
            }

            if (e.Item is GridEditFormItem)
            {
                GridEditFormItem editedItem = e.Item as GridEditFormItem;
                UpdateRoles(editedItem);
            }

        }

        else if (e.CommandName == "DeleteSelected")
        {
            DeleteRoles();
        }

    }

    protected void InsertRoles(GridEditableItem editedItem)
    {
        long idPlanilla = Convert.ToInt64(ViewState["idsel"]);
        GridEditManager editMan = editedItem.EditManager;
        string Login = (editedItem.FindControl("LoginTextBox") as TextBox).Text;
           
        try
        {
            SegRol _Rol = new SegRol();
            _Rol.Descripcion = Login;

            Contexto.AddToSegRol(_Rol);
            Contexto.SaveChanges();
            this.grillaRoles.Rebind();
        }
        catch (Exception e)
        {
            ScriptManager.RegisterStartupScript(this.UpdPnlGrilla, typeof(UpdatePanel), "Error Grabacion", "alert(" + e.Message + ")", true);
        }

    }
    protected void UpdateRoles(GridEditableItem editedItem)
    {
        long idRol = Convert.ToInt64(ViewState["idsel"]);
        SegRol _Rol = (from c in Contexto.SegRol where c.IdSegRol == idRol select c).FirstOrDefault();

        if (_Rol != null)
        {
            GridEditManager editMan = editedItem.EditManager;
            string Login = (editedItem.FindControl("LoginTextBox") as TextBox).Text;

            try
            {
                _Rol.Descripcion = Login;
                Contexto.SaveChanges();
                this.grillaRoles.Rebind();
            }
            catch (Exception e)
            {
                ScriptManager.RegisterStartupScript(UpdPnlGrilla, typeof(UpdatePanel), "Error Grabacion", "alert(" + e.Message + ")", true);
            }
        }
    }
    protected void DeleteRoles()
    {
        long idRol = Convert.ToInt64(ViewState["idsel"]);
        SegRol _Rol = (from c in Contexto.SegRol where c.IdSegRol == idRol select c).FirstOrDefault();

        if (_Rol != null)
        {

            try
            {
                Contexto.DeleteObject(_Rol);
                Contexto.SaveChanges();
                this.grillaRoles.Rebind();
            }
            catch (Exception e)
            {
                ScriptManager.RegisterStartupScript(UpdPnlGrilla, typeof(UpdatePanel), "Error Grabacion", "alert(" + e.Message + ")", true);
            }
        }
    }

}
