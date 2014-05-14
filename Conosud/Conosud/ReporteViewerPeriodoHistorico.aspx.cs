using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Reporting;
using Entidades;


public partial class ReporteViewerPeriodoHistorico : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        long id = long.Parse(Request.QueryString["Id"].ToString());
        bool EsHistorico = bool.Parse(Request.QueryString["EsHistorico"].ToString());
        EntidadesConosud dc = new EntidadesConosud();

        Entidades.CabeceraHojasDeRuta cabecera = (from C in dc.CabeceraHojasDeRuta
                                        .Include("ContratoEmpresas.Contrato")
                                        .Include("ContratoEmpresas")
                                        .Include("ContratoEmpresas.Empresa")
                                                  where C.IdCabeceraHojasDeRuta == id
                                                  select C).First<Entidades.CabeceraHojasDeRuta>();




        ///// Cabecera hoja de ruta donde no tiene documentacion 
        ///// presentada para ninguno de sus items (Contratista y sub contratista)
        //List<Entidades.CabeceraHojasDeRuta> CabSinDocumentacion = (from C in dc.CabeceraHojasDeRuta
        //                                       .Include("ContratoEmpresas.Contrato")
        //                                       .Include("ContratoEmpresas")
        //                                       .Include("ContratoEmpresas.Empresa")
        //                                                   where C.ContratoEmpresas.Contrato.IdContrato == cabecera.ContratoEmpresas.Contrato.IdContrato
        //                                                   && C.Periodo <= cabecera.Periodo
        //                                                   && C.FechaAprobacion == null 
        //                                                   && C.HojasDeRuta.Count(h => h.DocFechaEntrega != null) == 0
        //                                                   select C).ToList<Entidades.CabeceraHojasDeRuta>();


        ///// Cabecera hoja de ruta donde TIENE documentacion 
        ///// presentada para alguno de sus items pero aun no se han auditado (Contratista y sub contratista)
        ///// es decir no hay items aprobados ni comentarios de los mismos.
        //List<Entidades.CabeceraHojasDeRuta> CabSinAuditoria = (from C in dc.CabeceraHojasDeRuta
        //                                       .Include("ContratoEmpresas.Contrato")
        //                                       .Include("ContratoEmpresas")
        //                                       .Include("ContratoEmpresas.Empresa")
        //                                                           where C.ContratoEmpresas.Contrato.IdContrato == cabecera.ContratoEmpresas.Contrato.IdContrato
        //                                                           && C.Periodo <= cabecera.Periodo
        //                                                           && C.FechaAprobacion == null
        //                                                           && C.HojasDeRuta.Count(h => h.DocFechaEntrega != null) > 0
        //                                                           && C.HojasDeRuta.Count(h => h.HojaFechaAprobacion != null) == 0
        //                                                           && C.HojasDeRuta.Count(h => h.HojaComentario != "") == 0
        //                                                           select C).ToList<Entidades.CabeceraHojasDeRuta>();


        List<Entidades.HojasDeRuta> HRConPendientes = new List<Entidades.HojasDeRuta>();
        if (EsHistorico)
        {
            /// items de las cabeceras que tiene ingresado
            /// algún comentario y no estan aprobadas (Contratista y sub contratista)
            /// Para todos los periodos


            if (Session["TipoUsuario"] != null && Session["TipoUsuario"] == "Cliente")
            {
                /// Para este tipo de cliente solo se muestra el historico de las
                /// hojas de ruta que estan publicadas, ademas de las otras condiciones.

                HRConPendientes = (from I in dc.HojasDeRuta
                                                                          .Include("CabeceraHojasDeRuta.ContratoEmpresas.Contrato")
                                                                          .Include("CabeceraHojasDeRuta.ContratoEmpresas")
                                                                          .Include("CabeceraHojasDeRuta")
                                                                          .Include("CabeceraHojasDeRuta.ContratoEmpresas.Empresa")
                                                                          .Include("Plantilla")
                                   where I.CabeceraHojasDeRuta.ContratoEmpresas.Contrato.IdContrato == cabecera.ContratoEmpresas.Contrato.IdContrato
                                   && I.CabeceraHojasDeRuta.Periodo <= cabecera.Periodo
                                   && I.CabeceraHojasDeRuta.FechaAprobacion == null
                                   && I.HojaComentario != ""
                                   && I.CabeceraHojasDeRuta.Publicar == true
                                   orderby I.CabeceraHojasDeRuta.Periodo descending, I.Plantilla.Codigo
                                   select I).ToList<Entidades.HojasDeRuta>();
            }
            else
            {
                HRConPendientes = (from I in dc.HojasDeRuta
                                                                          .Include("CabeceraHojasDeRuta.ContratoEmpresas.Contrato")
                                                                          .Include("CabeceraHojasDeRuta.ContratoEmpresas")
                                                                          .Include("CabeceraHojasDeRuta")
                                                                          .Include("CabeceraHojasDeRuta.ContratoEmpresas.Empresa")
                                                                          .Include("Plantilla")
                                   where I.CabeceraHojasDeRuta.ContratoEmpresas.Contrato.IdContrato == cabecera.ContratoEmpresas.Contrato.IdContrato
                                   && I.CabeceraHojasDeRuta.Periodo <= cabecera.Periodo
                                   && I.CabeceraHojasDeRuta.FechaAprobacion == null
                                   && I.HojaComentario != ""
                                   orderby I.CabeceraHojasDeRuta.Periodo descending, I.Plantilla.Codigo
                                   select I).ToList<Entidades.HojasDeRuta>();
            }



        }
        else
        {

            /// items de las cabeceras que tiene ingresado
            /// algún comentario y no estan aprobadas (Contratista y sub contratista)
            /// Solo para el periodo seleccionado
            HRConPendientes = (from I in dc.HojasDeRuta
                                                                      .Include("CabeceraHojasDeRuta.ContratoEmpresas.Contrato")
                                                                      .Include("CabeceraHojasDeRuta.ContratoEmpresas")
                                                                      .Include("CabeceraHojasDeRuta")
                                                                      .Include("CabeceraHojasDeRuta.ContratoEmpresas.Empresa")
                                                                      .Include("Plantilla")
                               where I.CabeceraHojasDeRuta.ContratoEmpresas.Contrato.IdContrato == cabecera.ContratoEmpresas.Contrato.IdContrato
                               && I.CabeceraHojasDeRuta.Periodo.Month == cabecera.Periodo.Month && I.CabeceraHojasDeRuta.Periodo.Year == cabecera.Periodo.Year
                               && I.CabeceraHojasDeRuta.FechaAprobacion == null
                               && I.HojaComentario != ""
                               orderby I.CabeceraHojasDeRuta.Periodo descending, I.Plantilla.Codigo
                               select I).ToList<Entidades.HojasDeRuta>();


        }

        List<InformexPeriodoTemp> Resultado = new List<InformexPeriodoTemp>();
        foreach (Entidades.HojasDeRuta hoja in HRConPendientes)
        {
            Entidades.InformexPeriodoTemp I = new Entidades.InformexPeriodoTemp();
            I.RazonSocial = hoja.CabeceraHojasDeRuta.ContratoEmpresas.Empresa.RazonSocial;
            I.Periodo = hoja.CabeceraHojasDeRuta.Periodo;
            I.PeriodoDesc = Capitalize(string.Format("{0:Y}", hoja.CabeceraHojasDeRuta.Periodo));
            I.EsContratista = hoja.CabeceraHojasDeRuta.ContratoEmpresas.EsContratista.Value;
            I.Comentario = hoja.HojaComentario;
            I.Tipo = "Hojas Con Pendientes";

            I.OrdenDetalle = int.Parse(hoja.CabeceraHojasDeRuta.Periodo.Year.ToString() + string.Format("{0:00}", hoja.CabeceraHojasDeRuta.Periodo.Month));
            if (I.EsContratista)
                I.OrdenEmpresas = 0;
            else
                I.OrdenEmpresas = 1;

            Resultado.Add(I);
        }

        /// Solo vamos a mostrar las hojas que tienen pendientes (21/10/2010)
        //foreach (Entidades.CabeceraHojasDeRuta itemC in CabSinDocumentacion)
        //{
        //    Entidades.InformexPeriodoTemp I = new Entidades.InformexPeriodoTemp();
        //    I.RazonSocial = itemC.ContratoEmpresas.Empresa.RazonSocial;
        //    I.Periodo = itemC.Periodo;
        //    I.PeriodoDesc = string.Format("{0:Y}", itemC.Periodo);
        //    I.EsContratista = itemC.ContratoEmpresas.EsContratista.Value;
        //    I.Comentario = "";
        //    I.Tipo = "Hojas Sin Documentación Presentada";
        //    I.OrdenDetalle = int.Parse(itemC.Periodo.Year.ToString() + string.Format("{0:00}", itemC.Periodo.Month));
        //    if (I.EsContratista)
        //        I.OrdenEmpresas = 0;
        //    else
        //        I.OrdenEmpresas = 1;

        //    Resultado.Add(I);
        //}


        /// Solo vamos a mostrar las hojas que tienen pendientes (21/10/2010)
        //foreach (Entidades.CabeceraHojasDeRuta itemC in CabSinAuditoria)
        //{
        //    Entidades.InformexPeriodoTemp I = new Entidades.InformexPeriodoTemp();
        //    I.RazonSocial = itemC.ContratoEmpresas.Empresa.RazonSocial;
        //    I.Periodo = itemC.Periodo;
        //    I.PeriodoDesc = string.Format("{0:Y}", itemC.Periodo);
        //    I.EsContratista = itemC.ContratoEmpresas.EsContratista.Value;
        //    I.Comentario = "";
        //    I.Tipo = "Hojas en Proceso de Auditoria";
        //    I.OrdenDetalle = int.Parse(itemC.Periodo.Year.ToString() + string.Format("{0:00}", itemC.Periodo.Month));
        //    if (I.EsContratista)
        //        I.OrdenEmpresas = 0;
        //    else
        //        I.OrdenEmpresas = 1;

        //    Resultado.Add(I);
        //}


        if (Resultado.Count > 0)
        {
            InformexPeriodoHistorico rep = new InformexPeriodoHistorico();
            rep.InitReport(Resultado, cabecera, EsHistorico);
            this.ReportViewer1.Report = rep;
            (ReportViewer1.FindControl("ReportToolbar").FindControl("ExportGr").Controls[0].Controls[0] as DropDownList).ClearSelection();
            (ReportViewer1.FindControl("ReportToolbar").FindControl("ExportGr").Controls[0].Controls[0] as DropDownList).Items.FindByText("Excel").Selected = true;
        }
        else
        {
            divcontent.Visible = false;
            lblSinResultados.Visible = true;
        }


    }

    public static string Capitalize(string value)
    {
        return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);
    }
}
