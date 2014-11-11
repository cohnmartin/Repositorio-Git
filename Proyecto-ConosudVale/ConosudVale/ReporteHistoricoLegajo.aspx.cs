using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Entidades;
using System.Web.Services;
using System.Collections;

public partial class ReporteHistoricoLegajo : System.Web.UI.Page
{

    //public EntidadesConosud Contexto
    //{
    //    get
    //    {
    //        if (Session["ContextoReporte"] != null)
    //            return (EntidadesConosud)Session["ContextoReporte"];
    //        else
    //        {
    //            Session.Add("ContextoReporte", new EntidadesConosud());
    //            return (EntidadesConosud)Session["ContextoReporte"];
    //        }
    //    }
    //}


    protected void Page_Load(object sender, EventArgs e)
    {
        GridHistoricoLegajos.ExportToExcel += new ControlsAjaxNotti.ClickEventHandler(GridHistoricoLegajos_ExportToExcel);

        if (!IsPostBack)
        {
            //var a = Contexto;
            GridHistoricoLegajos.DataSource = null;
        }
    }

    void GridHistoricoLegajos_ExportToExcel(object sender)
    {

        List<dynamic> datosExportar = GetData(txtNroDoc.Text);


        List<string> camposExcluir = new List<string>(); ;
        Dictionary<string, string> alias = new Dictionary<string, string>() {
        { "PeriodoInicial", "Fecha de Alta" } ,
        { "PeriodoFinal", "Fecha de Baja" } ,
        { "contrato", "Contrato" }};

        List<string> DatosReporte = new List<string>();
        DatosReporte.Add("Historial del legajo dentro de VALE");
        DatosReporte.Add("Fecha y Hora emisi&oacute;n:" + DateTime.Now.ToString());
        DatosReporte.Add("Incluye todos los contratos por lo que ha pasado el legajo consultado");


        camposExcluir.Add("Id");

        GridView gv = Helpers.GenerarExportExcel(datosExportar.ToList<dynamic>(), alias, camposExcluir, DatosReporte);

        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        gv.RenderControl(htmlWrite);

        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=HistorialLegajos" + "_" + DateTime.Now.ToString("M_dd_yyyy_H_M_s") + ".xls");
        HttpContext.Current.Response.ContentType = "application/xls";
        HttpContext.Current.Response.Write(stringWrite.ToString());
        HttpContext.Current.Response.End();

    }

    [WebMethod(EnableSession = true)]
    public static List<dynamic> GetData(string dni)
    {

        #region Recupero los Datos


        using (EntidadesConosud dc = new EntidadesConosud())
        {
            /// consulta historico legajos...
            var datos = (from emp in dc.ContEmpLegajos
                         where emp.Legajos.NroDoc == dni
                         group emp by emp.IdContratoEmpresas into g
                         select new
                         {
                             Id = g.Key,
                             Apellido = g.FirstOrDefault().Legajos.Apellido.ToUpper(),
                             Nombre = g.FirstOrDefault().Legajos.Nombre.ToUpper(),
                             contrato = g.FirstOrDefault().ContratoEmpresas.Contrato.Codigo,
                             PeriodoInicial = g.FirstOrDefault().CabeceraHojasDeRuta.Periodo,
                             Empresa = g.FirstOrDefault().ContratoEmpresas.Empresa.RazonSocial,
                             Periodos = g
                         }).ToList();


            var DatosFormateados = (from d in datos
                                    orderby d.PeriodoInicial
                                    select new
                                    {
                                        Apellido = d.Apellido,
                                        Nombre = d.Nombre,
                                        d.Empresa,
                                        d.contrato,
                                        d.PeriodoInicial,
                                        PeriodoFinal = d.Periodos.Where(w => w.FechaTramiteBaja != null).FirstOrDefault() != null ? d.Periodos.Where(w => w.FechaTramiteBaja != null).FirstOrDefault().CabeceraHojasDeRuta.Periodo.ToShortDateString() : "",
                                        d.Id
                                    }).ToList();

            return DatosFormateados.ToList<dynamic>();
        }

       
        #endregion
    }
}