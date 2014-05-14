using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;

public partial class ReporteViewerPeriodo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        long id = long.Parse(Request.QueryString["Id"].ToString());
        EntidadesConosud dc = new EntidadesConosud();

        Entidades.CabeceraHojasDeRuta cabecera = (from C in dc.CabeceraHojasDeRuta
                                                  .Include("HojasDeRuta")
                                                  where C.IdCabeceraHojasDeRuta == id
                                                  select C).First<Entidades.CabeceraHojasDeRuta>();

        InformexPeriodo rep = new InformexPeriodo();
        rep.InitReport(cabecera, false);
        this.ReportViewer1.Report = rep;
        (ReportViewer1.FindControl("ReportToolbar").FindControl("ExportGr").Controls[0].Controls[0] as DropDownList).ClearSelection();
        (ReportViewer1.FindControl("ReportToolbar").FindControl("ExportGr").Controls[0].Controls[0] as DropDownList).Items.FindByText("Excel").Selected = true;
       
    }
}
