using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Telerik.Web.UI;
using Telerik.Web.UI.Upload;
using System.IO;
using System.Data.OleDb;
using System.ComponentModel;
using System.Web.Services;

public partial class ConsultaInstructivos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargaGrilla();
        }
    }

    private void CargaGrilla()
    {

        GridInstructivos.DataSource = (GetData() as IList);

    }

    public static object GetData()
    {

        EntidadesConosud dc = new EntidadesConosud();
        var datos = (from i in dc.Instructivos
                     select i).ToList();

        return datos;
    }
}