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

public partial class ConsultaHojaRuta : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnBuscar.Attributes.Add("onmouseover", "this.src='images/BuscarUp11.gif'");
        btnBuscar.Attributes.Add("onmouseout", "this.src='images/Buscar6.gif'");
        btnEditar.Attributes.Add("onmouseover", "this.src='images/EditarHojaUp1.gif'");
        btnEditar.Attributes.Add("onmouseout", "this.src='images/EditarHoja1.gif'");

        if (!IsPostBack)
        {
            txtInicial.Text = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            txtFinal.Text = DateTime.Now.ToShortDateString();
        }

    }
    protected void btnEditar_Click(object sender, EventArgs e)
    {
        if (gdHojasRuta.SelectedValue != null)
        {
            string idContratoEmpresa = gdHojasRuta.SelectedValue.ToString();
            Response.Redirect("CargarHojaRuta.aspx?id=" + idContratoEmpresa);
        }
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            gdHojasRuta.DataBind();
        }
        catch (Exception ex)
        {
            if (ex.Message == "String was not recognized as a valid DateTime.")
            {
                string alert = "alert('Fechas Invalidas')";
                System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", alert, true);
            }
        }
    }
    protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        
       lblIdCabecera.Text =  ((Label)((GridViewRow)((Button)sender).Parent.Parent).Cells[7].Controls[1]).Text;

       AEAuditores.TargetControlID = ((Button)sender).UniqueID;
       ((Button)sender).OnClientClick = "return false;";


    }
    protected void gdHojasRuta_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }
    protected void gdHojasRuta_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#CCCCCC';this.style.cursor='hand';");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#F1DCDC'");
           // e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference((Control)sender, "Select$" + e.Row.RowIndex.ToString()));
        }

        


    }
    protected void gdHojasRuta_SelectedIndexChanged(object sender, EventArgs e)
    {
        gdHojasRuta.SelectedRow.Attributes.Clear();
        Control ctr = gdHojasRuta.SelectedRow.FindControl("btnAuditores");
        AEAuditores.TargetControlID = ctr.UniqueID;

        //foreach (GridViewRow  row in gdHojasRuta.Rows)
        //{
        //    if (row != gdHojasRuta.SelectedRow)
        //    {
        //        row.Attributes.Add("onmouseover", "this.style.backgroundColor='#CCCCCC';this.style.cursor='hand';");
        //        row.Attributes.Add("onmouseout", "this.style.backgroundColor='#F1DCDC'");
        //        row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference((Control)sender, "Select$" + row.RowIndex.ToString()));
        //    }
        //    else
        //    {
        //        gdHojasRuta.SelectedRow.Attributes.Clear();
        //        Control ctr = gdHojasRuta.SelectedRow.FindControl("btnAuditores");
        //        AEAuditores.TargetControlID = ctr.UniqueID;

        //        //foreach (Control ctr in gdHojasRuta.SelectedRow.Cells[8].Controls)
        //        //{
        //        //    if (ctr.ID == "btnAuditores")
        //        //    {
                        
        //        //        break;
        //        //    }
        //        //}
            
        //    }
        //}

        //if (gdHojasRuta.SelectedRow != null)
        //{
        //    foreach (Control ctr in gdHojasRuta.SelectedRow.Cells[8].Controls)
        //    {
        //        if (ctr.ID == "btnAuditores")
        //        {
        //            AEAuditores.TargetControlID = ctr.UniqueID;
        //            break;
        //        }
        //    }
        //}


    }
    protected void SqlDataSource1_Selecting1(object sender, SqlDataSourceSelectingEventArgs e)
    {
       
    }
    protected void btnAuditores_Click(object sender, EventArgs e)
    {
        LinkButton btnEdit = sender as LinkButton;
        GridViewRow row = (GridViewRow)btnEdit.NamingContainer;
        string ass = (row.FindControl("lblId") as Label).Text;
        Response.Redirect("CargarHojaRuta.aspx?id=" + ass);

        //gdHojasRuta.SelectedIndex = row.RowIndex;
        //DataSourceSelectArguments SS = new DataSourceSelectArguments();
        //SS.StartRowIndex = 0;
        //SS.MaximumRows = 10;
        //SDSAudirores.DataSourceMode = SqlDataSourceMode.DataSet;

        //

        //object datos = SDSAudirores.Select(SS);

        //GridView1.DataSource = datos;
        //GridView1.DataBind();


    }
    protected void igbExcel_Click(object sender, ImageClickEventArgs e)
    {
        DSConosudTableAdapters.ContratoEmpresasTableAdapter TAEmp = new DSConosudTableAdapters.ContratoEmpresasTableAdapter();
        DSConosud.ContratoEmpresasDataTable dtEmp = TAEmp.GetHojasVisor(Convert.ToDateTime(txtInicial.Text), Convert.ToDateTime(txtFinal.Text));
        Helpers.GenExcell c = new Helpers.GenExcell();
        c.DoExcell(Server.MapPath(Request.ApplicationPath) + @"\ReporteExcel.xls", dtEmp, this.gdHojasRuta.Columns);

        System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", "window.open('ReporteExcel.xls')", true);
    }
    protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {

    }
}
