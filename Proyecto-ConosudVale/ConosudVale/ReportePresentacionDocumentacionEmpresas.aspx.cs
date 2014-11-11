using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using System.Xml.Linq;
using System.Xml;
using System.Text;
using System.IO;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Data;

public partial class ReportePresentacionDocumentacionEmpresas : System.Web.UI.Page
{

    public class InformeMensualPreentacionTemp
    {
        public string DescContratista { get; set; }
        public string DescSubContratista { get; set; }
        public string Periodo { get; set; }
        public string CodigoContrato { get; set; }
        public Int32 Presento { get; set; }
        public Entidades.CabeceraHojasDeRuta Cab { get; set; }
        public Entidades.ContratoEmpresas ContratoEmpresa { get; set; }
        public string NroItem { get; set; }
        public HojasDeRuta ItemHoja { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public DateTime? FechaProrroga { get; set; }
        public string Categoria { get; set; }
        public string ContratadoPor { get; set; }
        public string Area { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        gvEstadoPresentacion.ExportToExcel += new ControlsAjaxNotti.ClickEventHandler(gvEstadoPresentacion_ExportToExcel);

        if (!IsPostBack)
        {
            gvEstadoPresentacion.DataSource = new List<object>();
        }
    }

    void gvEstadoPresentacion_ExportToExcel(object sender)
    {
        gvEstadoPresentacion.ExportToExcelFunction("EstadoPresentacionDoc", (List<DataRow>)GetDatos());
    }
    
    protected void imgBuscar_Click(object sender, ImageClickEventArgs e)
    {
        gvEstadoPresentacion.DataSource = (List<DataRow>)GetDatos();
    }

    private object GetDatos()
    {
        EntidadesConosud dc = new EntidadesConosud();
        List<InformeMensualEmpresaAuditarTemp> Result = new List<InformeMensualEmpresaAuditarTemp>();

        DateTime FI = txtInicial.SelectedDate.Value;
        DateTime FF = txtFinal.SelectedDate.Value.AddDays(1);


        var Result1 = (from C in dc.CabeceraHojasDeRuta
                       where ((C.Periodo >= FI && C.Periodo <= FF)
                       ||  C.HojasDeRuta.Where(h => h.DocFechaEntrega != null & h.DocFechaEntrega >= FI && h.DocFechaEntrega <= FF).Count() > 0
                       ||  C.HojasDeRuta.Where(h => h.HojaFechaControlado != null & h.HojaFechaControlado >= FI && h.HojaFechaControlado <= FF).Count() > 0)
                       //&& C.ContratoEmpresas.Contrato.Codigo == "4600002908" && C.Periodo.Month == 6
                       select C.IdCabeceraHojasDeRuta).Distinct().ToList();

        var ResultDetalle = (from h in dc.HojasDeRuta
                             where Result1.Contains(h.IdCabeceraHojaDeRuta)
                             select new InformeMensualPreentacionTemp
                             {
                                 ContratoEmpresa = h.CabeceraHojasDeRuta.ContratoEmpresas,
                                 NroItem = h.Plantilla.Codigo,
                                 DescContratista = "",
                                 DescSubContratista = "",
                                 Cab = h.CabeceraHojasDeRuta,
                                 CodigoContrato = h.CabeceraHojasDeRuta.ContratoEmpresas.Contrato.Codigo,
                                 ItemHoja = h,
                                 Categoria = h.CabeceraHojasDeRuta.ContratoEmpresas.Contrato.objCategoria.Descripcion,
                                 FechaInicio = h.CabeceraHojasDeRuta.ContratoEmpresas.Contrato.FechaInicio.Value,
                                 FechaVencimiento= h.CabeceraHojasDeRuta.ContratoEmpresas.Contrato.FechaVencimiento.Value,
                                 FechaProrroga = h.CabeceraHojasDeRuta.ContratoEmpresas.Contrato.Prorroga,
                                 ContratadoPor = h.CabeceraHojasDeRuta.ContratoEmpresas.Contrato.objContratadopor.Descripcion,
                                 Area = h.CabeceraHojasDeRuta.ContratoEmpresas.Contrato.objArea.Descripcion
                             }).ToList();



        foreach (InformeMensualPreentacionTemp item in ResultDetalle)
        {
            item.DescContratista = item.ContratoEmpresa.DescConstratista;
            item.DescSubContratista = item.ContratoEmpresa.DescSubConstratista.Trim() == "" ? "-" : item.ContratoEmpresa.DescSubConstratista;
            item.Periodo = string.Format("{0:MM/yyyy}", item.Cab.Periodo);
            item.NroItem = "Nro" + item.NroItem.Trim();

            if ((item.ItemHoja.DocFechaEntrega != null && item.ItemHoja.DocFechaEntrega >= FI && item.ItemHoja.DocFechaEntrega <= FF)
                       || (item.ItemHoja.HojaFechaControlado != null && item.ItemHoja.HojaFechaControlado >= FI && item.ItemHoja.HojaFechaControlado <= FF))
                item.Presento = 1;
            else
                item.Presento = 0;
        }



        
        XElement detalleAplanados = new XElement("result",
                ResultDetalle.OrderBy(w => w.DescContratista).ThenBy(w => w.Periodo)
                .ThenBy(w => w.CodigoContrato).GroupBy(i => new { i.DescContratista, i.DescSubContratista, i.Periodo, i.CodigoContrato })
                .Select(g =>
                    new XElement("Item",
                            new XElement("Contratista", g.Key.DescContratista),
                            new XElement("SubContratista", g.Key.DescSubContratista),
                            new XElement("Periodo", g.Key.Periodo),
                            new XElement("Contrato", g.Key.CodigoContrato),
                            new XElement("FechaInicio", g.Select(i => i.FechaInicio).FirstOrDefault().ToShortDateString()),
                            new XElement("FechaVencimiento", g.Select(i => i.FechaVencimiento).FirstOrDefault().ToShortDateString()),
                            new XElement("FechaProrroga", g.Select(i => i.FechaProrroga).FirstOrDefault().HasValue ? g.Select(i => i.FechaProrroga).FirstOrDefault().Value.ToShortDateString():""),
                            new XElement("Categoria", g.Select(i => i.Categoria).FirstOrDefault()),
                            new XElement("ContratadoPor", g.Select(i => i.ContratadoPor).FirstOrDefault()),
                            new XElement("Area", g.Select(i => i.Area).FirstOrDefault()),
                            g.Select(i => new XElement(i.NroItem, i.Presento))
                    )
                )
            );

        DataSet ds = new DataSet();
        ds.ReadXml(detalleAplanados.CreateReader());

        if (ds.Tables.Count > 0)
            return ds.Tables[0].AsEnumerable().ToList();
        else
            return null;
    }

   
}