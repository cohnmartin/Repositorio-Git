using System;
using System.Collections;
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

public partial class PaginasDefinicion_ABMRoles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //((Label)this.Master.FindControl("lblDescPagina")).Text = "Roles";
        }
    }

    protected void btnVolver_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }

    protected void GridView1_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        this.GridView1.DataBind();
    }
    protected void GridView1_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
        this.GridView1.DataBind();
    }
    protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        this.GridView1.DataBind();
    }

    protected void LinqDataSource2_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    {
        foreach (System.Collections.Generic.KeyValuePair<string, object> kvp in e.WhereParameters)
        {
            if (kvp.Value == null)
            { e.Cancel = true; return; }
        }
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        if (this.CboMenus.SelectedValue != null && this.GridView1.SelectedValue != null)
        {
            ConosudDataContext dc = new ConosudDataContext();

            if ((from rm in dc.SegRolMenus
                 where rm.Menu == long.Parse(this.CboMenus.SelectedValue)
                 && rm.Rol == long.Parse(this.GridView1.SelectedValue.ToString())
                 select rm).Count() == 0)
            {
                SegMenu menu = (from u in dc.SegMenus
                                where u.IdSegMenu == long.Parse(this.CboMenus.SelectedValue)
                                select u).First();

                SegRol rol = (from u in dc.SegRols
                              where u.IdSegRol == long.Parse(this.GridView1.SelectedValue.ToString())
                              select u).First();

                SegRolMenu Confseg = new SegRolMenu();
                Confseg.ObjSegRol = rol;
                Confseg.ObjSegMenu = menu;

                dc.SegRolMenus.InsertOnSubmit(Confseg);
                dc.SubmitChanges();

                this.GridView2.DataBind();
            }
            else
            {
                string alert = "alert('Ya existe este menu para este rol');";
                System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", alert, true);
            }
        }
    }
}
