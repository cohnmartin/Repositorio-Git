using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AsignacionSegContexto : System.Web.UI.Page
{
    public long idUsuario
    {
        get;
        set;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
         idUsuario = long.Parse(Request.QueryString["IdUsuario"].ToString());
    }
}