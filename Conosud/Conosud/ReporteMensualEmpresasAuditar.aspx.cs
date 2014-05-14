using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;

public partial class ReporteMensualEmpresasAuditar : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtInicial.SelectedDate = DateTime.Parse("01/" + DateTime.Now.Month + "/" + DateTime.Now.Year);
            txtFinal.SelectedDate = DateTime.Now;
        }

    }

    public class ComparerByContratoEmpresaTemp : IEqualityComparer<InformeMensualEmpresaAuditarTemp>
    {
        public bool Equals(InformeMensualEmpresaAuditarTemp x, InformeMensualEmpresaAuditarTemp y)
        {
            return x.ContratoEmpresa.IdContratoEmpresas == y.ContratoEmpresa.IdContratoEmpresas;
        }
        public int GetHashCode(InformeMensualEmpresaAuditarTemp obj)
        {
            return obj.ContratoEmpresa.IdContratoEmpresas.GetHashCode();
        }
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        EntidadesConosud dc = new EntidadesConosud();
        List<InformeMensualEmpresaAuditarTemp> Result = new List<InformeMensualEmpresaAuditarTemp>();

        DateTime FI = txtInicial.SelectedDate.Value;
        DateTime FF = txtFinal.SelectedDate.Value.AddHours(23);

        //List<Entidades.CabeceraHojasDeRuta> cabs = (from C in dc.CabeceraHojasDeRuta
        //                                     .Include("HojasDeRuta").Include("HojasDeRuta.Plantilla").Include("HojasDeRuta.Plantilla.CategoriasItems").Include("ContratoEmpresas").Include("ContratoEmpresas.Contrato")
        //                                     .Include("ContratoEmpresas.Empresa")
        //                                     .Include("ContratoEmpresas.CabeceraHojasDeRuta")
        //                                            where
        //                                            (C.HojasDeRuta.Where(h => h.Plantilla.CategoriasItems.Nombre == Entidades.Emuneraciones.CATEGORIASUELDOS && h.DocFechaEntrega != null).Count() > 0
        //                                            || C.HojasDeRuta.Where(h => h.Plantilla.CategoriasItems.Nombre == Entidades.Emuneraciones.CATEGORIAPREVISIONAL && h.DocFechaEntrega != null).Count() > 0)

        //                                            && C.HojasDeRuta.Where(h => h.DocFechaEntrega != null & h.DocFechaEntrega >= FI && h.DocFechaEntrega <= FF).Count() > 0

        //                                            select C).ToList<Entidades.CabeceraHojasDeRuta>();



        Result = (from C in dc.CabeceraHojasDeRuta
                  where (C.HojasDeRuta.Where(h => h.Plantilla.CategoriasItems.Nombre == Entidades.Emuneraciones.CATEGORIASUELDOS && h.DocFechaEntrega != null).Count() > 0
                        || C.HojasDeRuta.Where(h => h.Plantilla.CategoriasItems.Nombre == Entidades.Emuneraciones.CATEGORIAPREVISIONAL && h.DocFechaEntrega != null).Count() > 0)
                          && C.HojasDeRuta.Where(h => h.DocFechaEntrega != null & h.DocFechaEntrega >= FI && h.DocFechaEntrega <= FF).Count() > 0
                  
                  select new InformeMensualEmpresaAuditarTemp
                  {
                      DescContratista = "",
                      DescSubContratista = "",
                      Periodo = C.Periodo,
                      CodigoContrato = C.ContratoEmpresas.Contrato.Codigo,
                      FechaUltimaPresentacionPrevisional = C.HojasDeRuta.Where(w => w.Plantilla.CategoriasItems.Nombre == Entidades.Emuneraciones.CATEGORIAPREVISIONAL && w.DocFechaEntrega != null).Select(w => w.DocFechaEntrega).Max(),
                      FechaUltimaPresentacionSueldos = C.HojasDeRuta.Where(w => w.Plantilla.CategoriasItems.Nombre == Entidades.Emuneraciones.CATEGORIASUELDOS && w.DocFechaEntrega != null).Select(w => w.DocFechaEntrega).Max(),
                      FechaUltimaAuditoria = C.HojasDeRuta.Any(w => w.HojaFechaControlado != null) ? (C.HojasDeRuta.Any(w => w.DocFechaEntrega > w.HojaFechaControlado) ? null : C.HojasDeRuta.Select(w => w.HojaFechaControlado).Max()) : null,
                      Aprobada = "",
                      Publicada = "",
                      Cab = C,
                      ContratoEmpresa = C.ContratoEmpresas
                  }).ToList<InformeMensualEmpresaAuditarTemp>();


        foreach (InformeMensualEmpresaAuditarTemp item in Result)
        {
            item.Aprobada = item.Cab.Aprobada;
            item.Publicada = item.Cab.Publicada;
            item.DescContratista = item.ContratoEmpresa.DescConstratista;
            item.DescSubContratista = item.ContratoEmpresa.DescSubConstratista.Trim() == "" ? "-" : item.ContratoEmpresa.DescSubConstratista;
        }

        //Result = Result.Distinct(new ComparerByContratoEmpresaTemp()).ToList();

        Result = Result.Distinct().ToList();

        

        if (Result.Count > 0)
        {
            InformeMensualEmpresasAuditar rep = new InformeMensualEmpresasAuditar();
            rep.InitReport(Result);
            this.ReportViewer1.Report = rep;
            (ReportViewer1.FindControl("ReportToolbar").FindControl("ExportGr").Controls[0].Controls[0] as DropDownList).ClearSelection();
            (ReportViewer1.FindControl("ReportToolbar").FindControl("ExportGr").Controls[0].Controls[0] as DropDownList).Items.FindByText("Excel").Selected = true;
            trReporte.Visible = true;
            trResultadoVacio.Visible = false;
        }
        else
        {
            trReporte.Visible = false;
            trResultadoVacio.Visible = true;
        }

        upResultado.Update();
    }


}
