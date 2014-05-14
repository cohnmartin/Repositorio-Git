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

public partial class Plantilla : System.Web.UI.Page
{
    private static DSConosud.RolesPlanillaDataTable _RolesPlanilla = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.gvItems.SelectedIndex < 0)
            tblRoles.Visible = false;
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        if (this.gvItems.SelectedValue != null)
        {
            if (NoExisteRolAsociado(long.Parse(this.cboRoles.SelectedValue)))
            {
                DSConosud Ds = new DSConosud();
                Ds.EnforceConstraints = false;

                DSConosud.RolesPlanillaRow drRolesPlanilla = Ds.RolesPlanilla.NewRolesPlanillaRow();
                drRolesPlanilla.IdPlanilla = Convert.ToInt64(this.gvItems.SelectedValue);
                drRolesPlanilla.IdRol = long.Parse(this.cboRoles.SelectedValue);
                Ds.RolesPlanilla.AddRolesPlanillaRow(drRolesPlanilla);

                DSConosudTableAdapters.RolesPlanillaTableAdapter TARolesPlanilla = new DSConosudTableAdapters.RolesPlanillaTableAdapter();
                TARolesPlanilla.Update(Ds);

                this.grRolesPlantilla.DataBind();
            }
            else
            {
                string alert = "alert('El Rol: " + this.cboRoles.SelectedItem.Text + ", que intenta ingresar ya existe. Por favor Seleccione uno diferente')";

                System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", alert, true);
            
            }
        }

    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        tblRoles.Visible = true;
    }
    protected void ODSRolesPlantilla_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        _RolesPlanilla = (DSConosud.RolesPlanillaDataTable)e.ReturnValue;
    }

    private bool NoExisteRolAsociado(long rolSeleccionado)
    {
        foreach (DSConosud.RolesPlanillaRow row in _RolesPlanilla)
        {
            if (row.IdRol == rolSeleccionado)
                return false;
        }

        return true;
    }
    protected void dvItems_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        this.dvItems.DataBind();
    }
    protected void dvItems_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        this.dvItems.DataBind();
    }
    protected void dvItems_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
    {
        this.dvItems.DataBind();
    }
}
