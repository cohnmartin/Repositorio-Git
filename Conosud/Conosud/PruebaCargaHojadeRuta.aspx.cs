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

public partial class PruebaCargaHojadeRuta : System.Web.UI.Page
{
    public string _DescripcionItemSel = "";
    public string FechaActual = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ConosudDataContext dc = new ConosudDataContext();
            var itemsHojaRuta = from H in dc.HojasDeRutas
                                where H.IdCabeceraHojaDeRuta == 3501
                                select H;

            gdItemHoja.DataSource = itemsHojaRuta;
            gdItemHoja.DataBind();
        }
    }
    protected void btnEditComentDoc_Click(object sender, ImageClickEventArgs e)
    {
        mdlPopupFadeIn.Show();

        Control ctr = ((GridViewRow)((Control)sender).NamingContainer).FindControl("txtIdHojaDeRuta");
        string id = ((TextBox)ctr).Text;


        //Control ctr = ((GridViewRow)((Control)sender).NamingContainer).Cells[3].FindControl("lblComentDoc");
        //txtComentDoc.Text = "sdasdasdsa " + new Random().Next().ToString();
        //_DescripcionItemSel = ((GridViewRow)((Control)sender).NamingContainer).Cells[1].Text;
        ConosudDataContext dc = new ConosudDataContext();
        var itemsHojaRuta = from H in dc.HojasDeRutas
                            where H.IdHojaDeRuta == int.Parse(id) 
                            select H;

        DetailsView1.DataSource = itemsHojaRuta;
        DetailsView1.DataBind();

        upComentariosRecepcion.Update();
        
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {

    }
    protected void btnOtro_Click(object sender, EventArgs e)
    {
        mdlPopupFadeIn.Show();


        
        
    }
}
