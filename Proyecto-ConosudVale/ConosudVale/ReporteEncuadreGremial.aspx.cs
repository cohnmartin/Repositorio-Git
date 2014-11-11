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

public partial class ReporteEncuadreGremial : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DateTime fechaInicial = DateTime.Now;

            for (int i = 0; i < 13; i++)
            {
                DateTime fechaActual = fechaInicial.AddMonths(-1 * i);
                string FechaFormat = string.Format("{0:MM/yyyy}", fechaActual);
                cboPeriodos.Items.Add(new ListItem(FechaFormat, FechaFormat));
            }


        }
    }
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        try
        {

            EntidadesConosud dc = new EntidadesConosud();

            DateTime FechaInicial = Convert.ToDateTime("01/" + cboPeriodos.Text);
            DateTime FechaFinal = Convert.ToDateTime("01/" + cboPeriodos.Text).AddMonths(1).AddDays(-1);


            /// Obtengo las cabeceras segun el rango de fechas
            var cabeceras1 = (from C in dc.CabeceraHojasDeRuta
                              where C.Periodo >= FechaInicial
                              && C.Periodo <= FechaFinal
                              && C.ContEmpLegajos.Count() > 0
                              orderby C.ContratoEmpresas.Contrato.IdContrato
                              select new
                              {
                                  Id = C.IdCabeceraHojasDeRuta,
                                  Cab = C,
                                  Constratista = C.ContratoEmpresas.Empresa.RazonSocial,
                                  IdContrato = C.ContratoEmpresas.Contrato.IdContrato,
                                  Codigo = C.ContratoEmpresas.Contrato.Codigo,
                                  C.ContratoEmpresas.EsContratista
                              }).ToList();



            /// Extraigo los ids de todas las cabeceras
            long[] idsCab = cabeceras1.Select(w => w.Id).ToArray();

            /// Recupero todos los legajos de cada una de las cabeceras
            var AllLegajosCabeceras = (from L in dc.ContEmpLegajos.Where(Helpers.ContainsExpression<Entidades.ContEmpLegajos, long>(L => L.CabeceraHojasDeRuta.IdCabeceraHojasDeRuta, idsCab))
                                       select new
                                       {
                                           idCab = L.CabeceraHojasDeRuta.IdCabeceraHojasDeRuta,
                                           Convenio = L.Legajos.objConvenio.Descripcion,
                                           Conratista = L.ContratoEmpresas.Empresa.RazonSocial,
                                           CondigoContrato = L.ContratoEmpresas.Contrato.Codigo
                                       }).ToList();


            if (idsCab.Length > 0)
            {

                List<Entidades.InformeEncuadreGremialTemp> AllEncuadre = new List<InformeEncuadreGremialTemp>();


                foreach (var ItemCabecera in cabeceras1)
                {

                    var LegAgrupados = from L in AllLegajosCabeceras
                                       where L.idCab == ItemCabecera.Id
                                       group L by L.Convenio into g
                                       select new
                                       {
                                           key = g.Key,
                                           cantidad = g.Count()
                                       };

                    foreach (var detalleLegAgrupado in LegAgrupados)
                    {
                        string contratista = "";

                        Entidades.InformeEncuadreGremialTemp enc = new InformeEncuadreGremialTemp();
                        enc.Cantidad = detalleLegAgrupado.cantidad;

                        if (ItemCabecera.EsContratista.Value)
                        {
                            enc.Empresa = Capitalize(ItemCabecera.Constratista.ToLower());
                            contratista = Capitalize(ItemCabecera.Constratista.ToLower());
                            enc.SubContratista = "";
                        }
                        else
                        {

                            var empContratista = (from c in cabeceras1
                                                  where c.IdContrato == ItemCabecera.IdContrato && c.EsContratista.Value
                                                  select new
                                                  {
                                                      Contratista = c.Constratista
                                                  }).FirstOrDefault();

                            if (empContratista != null)
                            {
                                enc.Empresa = Capitalize(empContratista.Contratista.ToLower());
                            }
                            else {

                                enc.Empresa = Capitalize((from c in dc.ContratoEmpresas
                                                          where c.Contrato.IdContrato == ItemCabecera.IdContrato && c.EsContratista.Value
                                                          select new
                                                          {
                                                              Contratista = c.Empresa.RazonSocial
                                                          }).First().Contratista.ToLower());
                            }

                            enc.SubContratista = Capitalize(ItemCabecera.Constratista.ToLower());
                        }

                        enc.Contrato = ItemCabecera.Codigo;


                        if (detalleLegAgrupado.key != null)
                            enc.Convenio = detalleLegAgrupado.key;
                        else
                            enc.Convenio = "F/Convenio";

                        AllEncuadre.Add(enc);
                    }
                }

                InformeEncuadreGremial rep = new InformeEncuadreGremial();
                rep.InitReport(AllEncuadre.OrderBy(w => w.Empresa).ToList(), cboPeriodos.Text);
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
        catch
        { 
        
        }
    }

    public static string Capitalize(string value)
    {
        return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);
    }

}
