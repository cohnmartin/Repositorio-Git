using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GestionAuditoriaHojaDeRuta : System.Web.UI.Page
{
    public long idCabeceraHojaRuta
    { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        idCabeceraHojaRuta = long.Parse(Request.QueryString["IdCab"].ToString());
    }
}