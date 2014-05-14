using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Empresas : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (this.gvEmpresa.Rows.Count != 0)
            {
                this.gvEmpresa.SelectedIndex = 0;
            }
        }

        GridViewRow pagerRow = gvEmpresa.BottomPagerRow;
        ImageButton igbExcel = (ImageButton)pagerRow.FindControl("igbExcel");
        if (igbExcel != null)
        {
            igbExcel.Click += new ImageClickEventHandler(igbExcel_Click);
        }

    }

    void dvEmpresas_ItemCommand(object sender, DetailsViewCommandEventArgs e)
    {
    }

    void gvEmpresa_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    
    protected void DetailsView1_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
    {
        this.gvEmpresa.DataBind();
    }

    protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        this.gvEmpresa.DataBind(); 
    }

    protected void DetailsView1_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        this.gvEmpresa.DataBind();
    }

    protected void dvEmpresas_ItemDeleting(object sender, DetailsViewDeleteEventArgs e)
    {
        //MsgBox msg = new MsgBox();
        //msg.ShowConfirmation("Desea eliminar el registro", "Eliminacion", true, true);
        //e.Cancel = true;
    }

    protected void igbExcel_Click(object sender, ImageClickEventArgs e)
    {
        DSConosudTableAdapters.EmpresaTableAdapter TAEmp = new DSConosudTableAdapters.EmpresaTableAdapter();
        DSConosud.EmpresaDataTable dtEmp = TAEmp.GetData();
        Helpers.GenExcell c = new Helpers.GenExcell();
        c.DoExcell(Server.MapPath(Request.ApplicationPath) + @"\ReporteExcel.xls", dtEmp, this.gvEmpresa.Columns);

        System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", "window.open('ReporteExcel.xls')", true);
    }

    protected void dvEmpresas_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        DSConosudTableAdapters.EmpresaTableAdapter TAEmp = new DSConosudTableAdapters.EmpresaTableAdapter();
        if ((int)TAEmp.ExisteCUIT(e.Values["CUIT"].ToString()) > 0)
        {
            string alert = "alert('El Cuit que intenta ingresar ya existe')";
            System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", alert, true);
            e.Cancel = true;
        }

        if ((int)TAEmp.ExisteRazonSocial(e.Values["RazonSocial"].ToString()) > 0)
        {
            string alert = "alert('La razon que intenta ingresar ya existe')";
            System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", alert, true);
            e.Cancel = true;
        }
    }

    protected void dvEmpresas_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        DSConosudTableAdapters.EmpresaTableAdapter TAEmp = new DSConosudTableAdapters.EmpresaTableAdapter();

        if (e.OldValues["CUIT"].ToString() != e.NewValues["CUIT"].ToString())
        {
            if ((int)TAEmp.ExisteCUIT(e.NewValues["CUIT"].ToString()) > 0)
            {
                string alert = "alert('El Cuit que intenta ingresar ya existe')";
                System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", alert, true);
                e.Cancel = true;
            }
        }

        if (e.OldValues["RazonSocial"].ToString() != e.NewValues["RazonSocial"].ToString())
        {
            if ((int)TAEmp.ExisteRazonSocial(e.NewValues["RazonSocial"].ToString()) > 0)
            {
                string alert = "alert('La razon que intenta ingresar ya existe')";
                System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", alert, true);
                e.Cancel = true;
            }
        }
    }

    protected void ODSSelEmpresa_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        int A = 0;
    }
}


