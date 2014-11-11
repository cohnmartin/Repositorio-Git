<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" %>

<%@ Import Namespace="System.IO" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Export to Excel - GridView Sample</title>
</head>

<script language="C#" runat="server">
    
    // Get files from selected path    
    private void BindFiles() {
        DirectoryInfo di = new DirectoryInfo(tbPath.Text);
        gvFiles.DataSource = di.GetFiles();
        gvFiles.DataBind();
    }

    protected void btnExportToExcel_Click(object sender, EventArgs e) {
        ExportToExcel();
    }
    
    //Export to Excel from a GridView
    protected void ExportToExcel() {
        Response.Clear();
        Response.Buffer = true;
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("content-disposition", "attachment;filename=MyFiles.xls");
        Response.Charset = "";
        this.EnableViewState = false;

        System.IO.StringWriter sw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);

        gvFiles.RenderControl(htw);

        Response.Write(sw.ToString());
        Response.End();
    }

    protected void Page_Load(object sender, EventArgs e) {
        BindFiles();
    }

    public override void VerifyRenderingInServerForm(Control control) {
    }

</script>

<body>
    <form id="form1" runat="server">
    <div>
        <h1>
            My Files</h1>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 12%">
            <tr>
                <td>
                    Path:
                </td>
                <td>
                    <asp:TextBox ID="tbPath" runat="server" Width="600px" Text="C:/"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                     
                </td>
                <td>
                    <asp:Button ID="btnExportToExcel" runat="server" Text="ExportToExcel" 
                        onclick="btnExportToExcel_Click" />
                </td>
            </tr>
        </table>
    </div>
    <asp:GridView ID="gvFiles" runat="server">
    </asp:GridView>
    <br />
    </form>
</body>
</html> 