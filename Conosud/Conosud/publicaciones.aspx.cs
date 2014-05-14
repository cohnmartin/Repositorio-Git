using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Linq;

public partial class publicaciones : System.Web.UI.Page
{
    private static ConosudDataContext db = new ConosudDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        TxPeriodo.Attributes.Add("onkeydown", "NoSubmit();");
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (GridView1.Rows.Count > 0)
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                long IdCabecera = long.Parse((row.FindControl("lblId") as Label).Text);
                bool isCheck = (row.FindControl("chkPublicar") as CheckBox).Checked;

                CabeceraHojasDeRuta Cabecera = (from C in db.CabeceraHojasDeRutas
                                           where C.IdCabeceraHojasDeRuta == IdCabecera
                                           select C).Single<CabeceraHojasDeRuta>();

                Cabecera.Publicar = isCheck;
            }

            db.SubmitChanges();

            GridView1.DataSource = null;
            GridView1.DataBind();
            TxPeriodo.Text = "";
        }

    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        if (TxPeriodo.Text != "")
        {
            DateTime Fecha = Convert.ToDateTime("01/" + TxPeriodo.Text.Substring(5, 2) + "/" + TxPeriodo.Text.Substring(0, 4));

            List<CabeceraHojasDeRuta>  CabecerasSeleccionadas = (from C in db.CabeceraHojasDeRutas
                                       where C.Periodo == Fecha
                                       orderby C.ObjContratoEmpresa.ObjEmpresa.RazonSocial
                                       select C).ToList<CabeceraHojasDeRuta>();

            GridView1.DataSource = CabecerasSeleccionadas;
            GridView1.DataBind();
        }
    }
}
