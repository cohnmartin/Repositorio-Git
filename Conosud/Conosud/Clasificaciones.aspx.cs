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
using System.IO;

public partial class Clasificaciones : System.Web.UI.Page
{


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (this.GridView1.Rows.Count != 0)
            {
                this.GridView1.SelectedIndex = 0;
            }
        }

        GridViewRow pagerRow = GridView1.BottomPagerRow;
        ImageButton igbExcel = (ImageButton)pagerRow.FindControl("igbExcel");
        if (igbExcel != null)
        {
            igbExcel.Click += new ImageClickEventHandler(igbExcel_Click);
        }
    }

    protected void DetailsView1_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
    {
        if (e.Exception == null)
        {
            this.GridView1.DataBind();
        }
        else
        {
            string alert = "alert('El parametro esta siendo utilizado, no se podra eliminar')";
            System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", alert, true);

            e.ExceptionHandled = true;
        }
    }

    protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        this.GridView1.DataBind();
    }

    protected void DetailsView1_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        this.GridView1.DataBind();
    }

    protected void igbExcel_Click(object sender, ImageClickEventArgs e)
    {
        DSConosudTableAdapters.ClasificacionTableAdapter TACla = new DSConosudTableAdapters.ClasificacionTableAdapter();
        DSConosud.ClasificacionDataTable dtCla = TACla.GetData();
        Helpers.GenExcell c = new Helpers.GenExcell();
        c.DoExcell(Server.MapPath(Request.ApplicationPath) + @"\ReporteExcel.xls", dtCla, this.GridView1.Columns);

        System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", "window.open('ReporteExcel.xls')", true);
    }
    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {

    }
    protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        
    }
    protected void DetailsView1_ItemDeleting(object sender, DetailsViewDeleteEventArgs e)
    {
        
    }
}
