using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Entidades;
using System.Collections;

public partial class ABMItems : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
         RadGrid1.DataBound += new EventHandler(RadGrid1_DataBound);
    }

    void RadGrid1_DataBound(object sender, EventArgs e)
    {
        long idUsuario = long.Parse(Session["idusu"].ToString());
        Entidades.SegRolMenu PermisosPagina = Helpers.GetPermisosAcciones(Helpers.Constantes.PaginaMenu_.Plantilla, idUsuario);


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

    protected void EntityDataSourceItems_Selecting(object sender, EntityDataSourceSelectingEventArgs e)
    {
        if (this.txtApellidoLegajo.Text == string.Empty)
        {
            e.DataSource.Where = "";
            e.DataSource.AutoGenerateWhereClause = true;
        }

    }
    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {

            Entidades.Plantilla hoja = EntityDataSourceExtensions.GetItemObject<Entidades.Plantilla>(e.Item.DataItem);
            hoja.RolesPlanilla.Load();
            if (hoja.RolesPlanilla.Count > 0)
            {
                hoja.RolesPlanilla.First().SegRolReference.Load();
                (e.Item.FindControl("lblRol") as Label).Text = hoja.RolesPlanilla.First().SegRol.Descripcion;
            
            }



        }
        else if (e.Item.ItemType == GridItemType.EditFormItem)
        {

            Entidades.Plantilla hoja = EntityDataSourceExtensions.GetItemObject<Entidades.Plantilla>(e.Item.DataItem);
            hoja.RolesPlanilla.Load();
            if (hoja.RolesPlanilla.Count > 0)
            {
                hoja.RolesPlanilla.First().SegRolReference.Load();
                if (e.Item.FindControl("cboRoles") != null)
                    (e.Item.FindControl("cboRoles") as RadComboBox).SelectedValue = hoja.RolesPlanilla.First().SegRol.IdSegRol.ToString();

            }
        }

    }
    protected void EntityDataSourceItems_Updating(object sender, EntityDataSourceChangingEventArgs e)
    {
        Entidades.Plantilla ItemPlantilla = (Entidades.Plantilla)e.Entity;
        
        long id = Convert.ToInt64(ViewState["IdRol"]);

        IEnumerable<KeyValuePair<string, object>> entityKeyValues =
            new KeyValuePair<string, object>[] { 
                new KeyValuePair<string, object>("IdSegRol", id) };

        System.Data.EntityKey key = new System.Data.EntityKey("EntidadesConosud.SegRol", entityKeyValues);
        Entidades.SegRol _rol = (Entidades.SegRol)e.Context.GetObjectByKey(key);
        ItemPlantilla.RolesPlanilla.Load();
        
        if (ItemPlantilla.RolesPlanilla.Count == 0)
        {
            Entidades.RolesPlanilla rolP = new Entidades.RolesPlanilla();
            rolP.Plantilla = ItemPlantilla;
            rolP.SegRol = _rol;
            e.Context.AddObject("EntidadesConosud.RolesPlanilla", rolP);
        }
        else
        {
            ItemPlantilla.RolesPlanilla.First().SegRol = _rol;
        }

        e.Context.SaveChanges();

    }
    protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.PerformInsertCommandName
            || e.CommandName == RadGrid.UpdateCommandName)
        {
            if (e.Item is GridEditFormInsertItem)
            {
                GridEditFormInsertItem editedItem = e.Item as GridEditFormInsertItem;
                RadComboBox Contra = (RadComboBox)editedItem.FindControl("cboRoles");
                ViewState["IdRol"] = Contra.SelectedValue;
            }

            if (e.Item is GridEditFormItem)
            {
                GridEditFormItem editedItem = e.Item as GridEditFormItem;
                RadComboBox Contra = (RadComboBox)editedItem.FindControl("cboRoles");
                ViewState["IdRol"] = Contra.SelectedValue;
            }

        }
    }
}
