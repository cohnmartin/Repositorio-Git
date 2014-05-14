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
using Subgurim.Controles;


public partial class prueba_upload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        FileUploaderAJAX1.CssClass = "fua";
        FileUploaderAJAX1.Style = @"
        body{font-size: smaller; 
             color: #F1DDDD;
             background-color: #F1DDDD;}
        a{color: black;font-weight:bold}"; 

        if (FileUploaderAJAX1.IsPosting)
            this.managePost();
    }

    private void managePost()
    {
        HttpPostedFileAJAX pf = FileUploaderAJAX1.PostedFile;


        if (pf.ContentType.Equals("image/gif") && pf.ContentLength <= 5 * 1024)
            FileUploaderAJAX1.SaveAs("~/images",pf.FileName);
    } 

}
