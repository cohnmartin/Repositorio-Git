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
using System.Collections.Generic;

public partial class PaginasDefinicion_ABMUsuarios : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //((Label)this.Master.FindControl("lblTituloPrincipal")).Text = "Gestión de Usuarios";
        }
    }

    protected void btnVolver_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }

    protected void GridView1_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
        this.GridView1.DataBind();

    }
    protected void GridView1_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        this.GridView1.DataBind();

    }
    protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        this.GridView1.DataBind();
        upGrillaPrincipal.Update();

        DetailsView1.Fields[0].Visible = false;
        DetailsView1.Fields[1].Visible = false;
        DetailsView1.Fields[2].Visible = false;
        upDetail.Update();

    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        if (this.CboRoles.SelectedValue != null && this.GridView1.SelectedValue != null)
        {
            ConosudDataContext dc = new ConosudDataContext();

            if ((from ur in dc.SegUsuarioRols
                 where ur.Rol == long.Parse(this.CboRoles.SelectedValue) &&
                 ur.Usuario == long.Parse(this.GridView1.SelectedValue.ToString())
                 select ur).Count() == 0)
            {
                SegRol rol = (from u in dc.SegRols
                              where u.IdSegRol == long.Parse(this.CboRoles.SelectedValue)
                              select u).First();

                SegUsuario usu = (from u in dc.SegUsuarios
                                  where u.IdSegUsuario == long.Parse(this.GridView1.SelectedValue.ToString())
                                  select u).First();

                SegUsuarioRol ConfRol = new SegUsuarioRol();
                ConfRol.ObjSegRol = rol;
                ConfRol.ObjSegUsuario = usu;

                dc.SegUsuarioRols.InsertOnSubmit(ConfRol);
                dc.SubmitChanges();

                this.GridView2.DataBind();
            }
            else
            {
                string alert = "alert('Ya existe este rol para este usuario');";
                System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", alert, true);
            }
        }
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void LinqDataSource2_Selecting(object sender, LinqDataSourceSelectEventArgs e) 
    {
        foreach (System.Collections.Generic.KeyValuePair<string,object> kvp in e.WhereParameters)
        {
            if (kvp.Value == null)
            { e.Cancel = true; return; }
        } 
    }
    protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "CANCEL")
        {
            DetailsView1.Fields[0].Visible = false;
            DetailsView1.Fields[1].Visible = false;
            DetailsView1.Fields[2].Visible = false;
            upDetail.Update();
        }
        if (e.CommandName.ToUpper() == "NEW")
        {
            DetailsView1.Fields[0].Visible = true;
            DetailsView1.Fields[1].Visible = true;
            DetailsView1.Fields[2].Visible = true;
            upDetail.Update();
        }
    }
}
