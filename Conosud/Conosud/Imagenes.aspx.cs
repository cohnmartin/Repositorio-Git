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

public partial class Imagenes : System.Web.UI.Page
{
    private static int index = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
        Helpers.RutaProyecto = Server.MapPath(Request.ApplicationPath);
        
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        //Random a = new Random();
        //if (index < 327)
        //{
        //    imgAnigrama.ImageUrl = Server.MapPath(Request.ApplicationPath) + "\\Documentos\\" + "angiogram-01.dcm-" + index.ToString() + ".jpg";
        //    imgAnigrama.Width = 200;
        //    imgAnigrama.Height = 200;
        //    index++;
        //}
        //else
        //    index = 1;
    }
    protected void Timer1_Tick1(object sender, EventArgs e)
    {
            if (index < 306)
            {
                Image1.ImageUrl =  "~\\Documentos\\" + "angiogram-01.dcm-" + index.ToString() + ".jpg";
               
                index++;
            }
            else
                index = 1;
       
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static AjaxControlToolkit.Slide[] GetSlides(string contextKey)
    {
        AjaxControlToolkit.Slide[] slides = new AjaxControlToolkit.Slide[306];

        for (int i = 1; i < 306; i++)
        {
            string nombre = "Documentos/" + "angiogram-01.dcm-" + i.ToString() + ".jpg";
            slides[i] = new AjaxControlToolkit.Slide(nombre, "", "");

        }
        return slides;
    }
}
