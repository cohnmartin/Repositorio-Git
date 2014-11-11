using System;
using System.Collections;
using System.Collections.Generic;
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
using Telerik.Web.UI;
using Entidades;
using System.Web.Services;
using System.Collections;
using System.IO;
using System.Dynamic;

public partial class ReporteMapaHojas : System.Web.UI.Page
{

    private EntidadesConosud _dc = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadEmpresas();
        }

    }

    private void LoadEmpresas()
    {

        cboEmpresas.DataTextField = "RazonSocial";
        cboEmpresas.DataValueField = "IdEmpresa";
        cboEmpresas.DataSource = Helpers.GetEmpresas(long.Parse(Session["idusu"].ToString()));
        cboEmpresas.DataBind();

        if (cboEmpresas.Items.Count > 1)
            cboEmpresas.Items.Insert(0, new RadComboBoxItem("- Seleccione una Empresa -"));
        else
            LoadContratos(int.Parse(cboEmpresas.Items[0].Value));

    }

    private void LoadContratos(int id)
    {

        cboContratos.DataTextField = "Codigo";
        cboContratos.DataValueField = "IdContrato";
        cboContratos.DataSource = (from c in Helpers.GetContratos(id, long.Parse(Session["idusu"].ToString()))
                                   select new
                                   {
                                       IdContrato = c.IdContrato,
                                       Codigo = c.Codigo + " (" + string.Format("{0:MM/yyyy}", c.FechaInicio.Value) + " - " + string.Format("{0:MM/yyyy}", (c.Prorroga.HasValue ? c.Prorroga.Value : c.FechaVencimiento.Value)) + ")"
                                   }).ToList();
        cboContratos.DataBind();

        cboContratos.Items.Insert(0, new RadComboBoxItem("- Seleccione un Contrato -"));

    }

    private void LoadPeriodos(int idContrato)
    {
        cboPeriodos.DataTextField = "Periodo";
        cboPeriodos.DataTextFormatString = "{0:MM/yyyy}";
        cboPeriodos.DataValueField = "Periodo";
        cboPeriodos.DataSource = Helpers.GetPeriodosByContrato(idContrato, long.Parse(Session["idusu"].ToString()));
        cboPeriodos.DataBind();

        cboPeriodos.Items.Insert(0, new RadComboBoxItem("- Todos los Períodos -"));
    }


    protected void cboEmpresas_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        LoadEmpresas();
    }

    protected void cboContratos_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        LoadContratos(int.Parse(e.Text));
    }

    protected void cboPeriodos_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        LoadPeriodos(int.Parse(e.Text));
    }

    [WebMethod(EnableSession = true)]
    public static string GetData(long IdContrato, string Periodo)
    {

        using (EntidadesConosud dc = new EntidadesConosud())
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            List<InformeMapaHojasRutaTemp> datos = new List<InformeMapaHojasRutaTemp>();
            List<Entidades.CabeceraHojasDeRuta> cabs = null;

            if (Periodo == "")
            {
                cabs = (from C in dc.CabeceraHojasDeRuta
                           .Include("ContratoEmpresas.Empresa")
                           .Include("ContratoEmpresas")
                           .Include("HojasDeRuta")
                        where (C.ContratoEmpresas.Contrato.IdContrato == IdContrato)
                         &&
                            ((C.Periodo.Month < DateTime.Now.Month && C.Periodo.Year == DateTime.Now.Year) || (C.Periodo.Year < DateTime.Now.Year))
                        orderby C.Periodo ascending
                        select C).ToList<Entidades.CabeceraHojasDeRuta>();
            }
            else
            {
                int año = int.Parse(Periodo.Substring(3, 4));
                int mes = int.Parse(Periodo.Substring(0, 2));

                cabs = (from C in dc.CabeceraHojasDeRuta
                                      .Include("ContratoEmpresas.Empresa")
                                      .Include("ContratoEmpresas")
                                      .Include("HojasDeRuta")
                        where C.ContratoEmpresas.Contrato.IdContrato == IdContrato
                        && C.Periodo.Month == mes && C.Periodo.Year == año
                        orderby C.Periodo ascending
                        select C).ToList<Entidades.CabeceraHojasDeRuta>();
            }

            foreach (var item in cabs)
            {
                InformeMapaHojasRutaTemp reg = new InformeMapaHojasRutaTemp();
                reg.IdCabecera = item.IdCabeceraHojasDeRuta.ToString();
                reg.Estado = item.Aprobada;
                reg.Empresa = item.ContratoEmpresas.Empresa.RazonSocial;
                reg.Periodo = string.Format("{0:MM/yyyy}", item.Periodo);

                if (reg.Estado == "Aprobada")
                    reg.EstadoValor = 0;
                else
                    reg.EstadoValor = 1;

                if (item.ContratoEmpresas.EsContratista.Value)
                    reg.OrdenEmpresas = 0;
                else
                    reg.OrdenEmpresas = 1;

                datos.Add(reg);

                string EstadoAdicional = "";
                if (item.FechaAprobacion == null)
                {

                    int SinDoc = (from H in item.HojasDeRuta
                                  where H.DocComentario != null
                                  && H.DocComentario.Trim() != ""
                                  select H).Count();

                    if (SinDoc == 0)
                    {
                        /// Si no tiene comentarios de pendientes y no tiene todos los items aprobados,
                        /// entonces no se ha aprobado porque alguna de las sub contratistas 
                        /// no esta aprobada y por lo tanto no puede aprobarce esta hoja.                    
                        int ItemsAprobados = item.HojasDeRuta.Where(w => w.HojaFechaAprobacion.HasValue).Count();
                        if (ItemsAprobados == 0)
                            EstadoAdicional = " (No Presentó Documentación)";
                        else
                        {
                            List<CabeceraHojasDeRuta> cabsPeriodo = cabs.Where(w => w.Periodo == item.Periodo).ToList();

                            if (cabsPeriodo.Any(c => c.HojasDeRuta.Any(w => w.HojaComentario != null && w.HojaComentario.Trim() != "")))
                                EstadoAdicional = " (Por pendientes de Subcontratista)";
                            else
                                EstadoAdicional = "";
                        }

                    }
                    else
                    {
                        if (item.ContratoEmpresas.EsContratista.Value)
                        {
                            List<CabeceraHojasDeRuta> cabsPeriodo = cabs.Where(w=>w.Periodo == item.Periodo).ToList();

                            if (cabsPeriodo.Any(c => c.IdCabeceraHojasDeRuta != item.IdCabeceraHojasDeRuta && c.HojasDeRuta.Any(w => w.HojaComentario != null && w.HojaComentario.Trim() != "")))
                                EstadoAdicional = " (Por pendientes de Subcontratista)";
                            else if (cabsPeriodo.Any(c => c.IdCabeceraHojasDeRuta != item.IdCabeceraHojasDeRuta && c.HojasDeRuta.Any(w => !w.HojaFechaAprobacion.HasValue)))
                                EstadoAdicional = " (Por Subcontratista)";
                           

                        }

                    }


                    int CantComentarios = (from H in item.HojasDeRuta
                                           where H.HojaComentario != null
                                           && H.HojaComentario.Trim() != ""
                                           select H).Count();


                    if (CantComentarios > 0)
                    {
                        EstadoAdicional = " (Con Pendientes)";
                    }
                }

                reg.Estado += EstadoAdicional;

            }


            var ratingTypes = datos.Select(i => new { i.OrdenEmpresas, i.Empresa }).Distinct().OrderBy(i => i.OrdenEmpresas).Select(w => w.Empresa).ToArray();

            var datosAgrupado = datos.GroupBy(i => i.Periodo)
                .Select(g => new { Periodo = g.Key, Estados = ratingTypes.GroupJoin(g, o => o, i => i.Empresa, (o, i) => i.Select(x => new { x.Estado, x.IdCabecera }).FirstOrDefault()).ToArray() })
                .ToArray();



            List<string> columnNames = new List<string>();
            columnNames.Add("Periodo");
            columnNames.AddRange(ratingTypes.ToArray());
            columnNames.Add("Reporte");



            HtmlTable tabla = new HtmlTable();
            tabla.Attributes.Add("class", "TSunset");
            tabla.Attributes.Add("cellpadding", "0");
            tabla.Attributes.Add("cellspacing", "0");
            tabla.Attributes.Add("width", "100%");

            HtmlTableRow rowHeader = new HtmlTableRow();

            // Header
            HtmlTableCell cellHeader = new HtmlTableCell();
            //cellHeader.InnerText = "Periodo";
            //rowHeader.Cells.Add(cellHeader);

            foreach (var item in columnNames)
            {
                cellHeader = new HtmlTableCell();
                cellHeader.InnerText = item;
                cellHeader.Attributes.Add("class", "Theader");
                rowHeader.Cells.Add(cellHeader);
            }

            tabla.Rows.Add(rowHeader);


            /// body
            foreach (var item in datosAgrupado)
            {
                rowHeader = new HtmlTableRow();

                /// Columna de Periodo
                cellHeader = new HtmlTableCell();
                cellHeader.InnerText = item.Periodo;
                cellHeader.Attributes.Add("class", "tdSimple");
                rowHeader.Cells.Add(cellHeader);

                // Columnas de Empresas
                string idCabPrincipal = "";
                for (int i = 0; i < ratingTypes.Count(); i++)
                {
                    string estado = " ";
                    string idCab = "";
                    if (item.Estados.Skip(i).Take(1).FirstOrDefault() != null)
                    {
                        estado = item.Estados.Skip(i).Take(1).FirstOrDefault().Estado;
                        idCab = item.Estados.Skip(i).Take(1).FirstOrDefault().IdCabecera;

                        if (i == 0)
                            idCabPrincipal = idCab;
                    }

                    HtmlAnchor a = new HtmlAnchor();
                    a.InnerText = estado;
                    a.HRef = "GestionHojadeRuta.aspx?IdCabecera=" + idCab;
                    a.Target = "_blank";
                    //HttpContext.Current.Request.ApplicationPath +

                    cellHeader = new HtmlTableCell();
                    cellHeader.Controls.Add(a);
                    cellHeader.Attributes.Add("class", "tdSimple");
                    rowHeader.Cells.Add(cellHeader);
                }

                // Columna de Reporte Historico
                HtmlAnchor linkReporteMensual = new HtmlAnchor();
                linkReporteMensual.InnerText = "Reporte Mensual";
                linkReporteMensual.HRef = "ReporteViewerPeriodoHistorico.aspx?Id=" + idCabPrincipal + "&EsHistorico=false";
                linkReporteMensual.Target = "_blank";


                cellHeader = new HtmlTableCell();
                cellHeader.Controls.Add(linkReporteMensual);
                cellHeader.Attributes.Add("class", "tdSimple");
                rowHeader.Cells.Add(cellHeader);


                tabla.Rows.Add(rowHeader);
            }


            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            tabla.RenderControl(htw);

            return htw.InnerWriter.ToString();

        }



    }
}








