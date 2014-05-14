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

public partial class ReporteAltasBajasLegajos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DateTime fechaInicial = DateTime.Now;

            for (int i = 0; i < 25; i++)
            {
                DateTime fechaActual = fechaInicial.AddMonths(-1 * i);
                string FechaFormat = string.Format("{0:MM/yyyy}", fechaActual);
                cboPeriodos.Items.Add(new ListItem(FechaFormat, FechaFormat));
            }


        }
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        EntidadesConosud dc = new EntidadesConosud();

        DateTime FechaInicial = Convert.ToDateTime("01/" + cboPeriodos.Text);
        DateTime FechaFinal = Convert.ToDateTime("01/" + cboPeriodos.Text).AddMonths(1);

        DateTime FechaInicioPA = FechaInicial.AddMonths(-1);
        DateTime FechaFinalPA = FechaFinal.AddMonths(-1);


        var empActuales = (from emp in dc.ContEmpLegajos
                           where ((emp.CabeceraHojasDeRuta.Periodo >= FechaInicial &&
                           emp.CabeceraHojasDeRuta.Periodo < FechaFinal) || (emp.FechaTramiteBaja >= FechaInicial && emp.FechaTramiteBaja < FechaFinal))
                           select new
                           {
                               idCab = emp.CabeceraHojasDeRuta.IdCabeceraHojasDeRuta,
                               idLeg = emp.Legajos.IdLegajos,
                               idConEmp = emp.ContratoEmpresas.IdContratoEmpresas,
                               idCon = emp.ContratoEmpresas.Contrato.IdContrato,
                               NombreCompleto = emp.Legajos.Apellido + ", " + emp.Legajos.Nombre,
                               Nrodoc = emp.Legajos.NroDoc,
                               CodigoContrato = emp.ContratoEmpresas.Contrato.Codigo,
                               NombreEmpresa = emp.ContratoEmpresas.Empresa.RazonSocial,
                               Encuadre = emp.Legajos.objConvenio,
                               FechaBaja = emp.CabeceraHojasDeRuta.Periodo,
                               FechaTramite = emp.FechaTramiteBaja,
                               EsContratista = emp.ContratoEmpresas.EsContratista,
                               VencimientoContrato = emp.ContratoEmpresas.Contrato.Prorroga.HasValue ? emp.ContratoEmpresas.Contrato.Prorroga.Value : emp.ContratoEmpresas.Contrato.FechaVencimiento.Value

                           }).ToList();


        var empPA = (from emp in dc.ContEmpLegajos
                     where emp.CabeceraHojasDeRuta.Periodo >= FechaInicioPA &&
                     emp.CabeceraHojasDeRuta.Periodo < FechaFinalPA &&
                       (
                            (emp.ContratoEmpresas.Contrato.FechaVencimiento > FechaFinalPA)
                            ||
                            (emp.ContratoEmpresas.Contrato.Prorroga.HasValue && emp.ContratoEmpresas.Contrato.Prorroga > FechaFinalPA)
                       )
                     select new
                     {
                         idCab = emp.CabeceraHojasDeRuta.IdCabeceraHojasDeRuta,
                         idLeg = emp.Legajos.IdLegajos,
                         idConEmp = emp.ContratoEmpresas.IdContratoEmpresas,
                         idCon = emp.ContratoEmpresas.Contrato.IdContrato,
                         NombreCompleto = emp.Legajos.Apellido + ", " + emp.Legajos.Nombre,
                         Nrodoc = emp.Legajos.NroDoc,
                         CodigoContrato = emp.ContratoEmpresas.Contrato.Codigo,
                         NombreEmpresa = emp.ContratoEmpresas.Empresa.RazonSocial,
                         Encuadre = emp.Legajos.objConvenio,
                         FechaBaja = emp.CabeceraHojasDeRuta.Periodo,
                         FechaTramite = emp.FechaTramiteBaja,
                         EsContratista = emp.ContratoEmpresas.EsContratista

                     }).ToList();

        List<AltasBajasLegajosTemp> LegajosEncontrados = new List<AltasBajasLegajosTemp>();

        foreach (var item in empActuales.Where(emp => (emp.FechaTramite >= FechaInicial && emp.FechaTramite < FechaFinal)))
        {

            AltasBajasLegajosTemp LegajoEliminado = new AltasBajasLegajosTemp();
            LegajoEliminado.Accion = "Baja";
            LegajoEliminado.CodigoContrato = item.CodigoContrato;
            LegajoEliminado.NombreCompleto = Capitalize(item.NombreCompleto.ToLower());
            LegajoEliminado.NombreEmpresaContratista = Capitalize(item.NombreEmpresa.ToLower());
            LegajoEliminado.Nrodoc = item.Nrodoc;


            if (item.Encuadre != null)
                LegajoEliminado.Encuadre = item.Encuadre.Descripcion;
            else
                LegajoEliminado.Encuadre = "";

            if (item.FechaTramite.HasValue)
            {
                LegajoEliminado.FechaTramite = item.FechaTramite.Value.ToShortDateString();
                LegajoEliminado.FechaBaja = string.Format("{0:MMM-yy}", item.FechaBaja);
            }
            else
            {
                LegajoEliminado.FechaTramite = "";
                LegajoEliminado.FechaBaja = "";
            }

            if (!item.EsContratista.HasValue || !item.EsContratista.Value)
            {
                LegajoEliminado.NombreEmpresaSubContratista = Capitalize(item.NombreEmpresa.ToLower());
                LegajoEliminado.NombreEmpresaContratista = Capitalize((from c in dc.ContratoEmpresas
                                                                       where c.Contrato.IdContrato == item.idCon
                                                                       && c.EsContratista.Value
                                                                       select new
                                                                       {
                                                                           Contratista = c.Empresa.RazonSocial
                                                                       }).First().Contratista.ToLower());
            }

            LegajosEncontrados.Add(LegajoEliminado);

        }



        /// Solo se verifican los legajos que no han sido dado de baja ya que nunca
        /// se pueden dar de baja y de alta en el mismo periodo y contrato.
        foreach (var item in empActuales.Where(w => w.FechaTramite == null))
        {

            /// Si el legajo del periodo actual no existe en el periodo anterior 
            /// se infiere que el mismo fue dado de alta en el perio actual.
            int existe = (from emp in empPA
                          where emp.idConEmp == item.idConEmp
                          && emp.idLeg == item.idLeg
                          select emp).Count();

            if (existe == 0)
            {
                AltasBajasLegajosTemp LegajoEliminado = new AltasBajasLegajosTemp();
                LegajoEliminado.Accion = "Alta";
                LegajoEliminado.CodigoContrato = item.CodigoContrato;
                LegajoEliminado.NombreCompleto = Capitalize(item.NombreCompleto.ToLower());
                LegajoEliminado.NombreEmpresaContratista = Capitalize(item.NombreEmpresa.ToLower());
                LegajoEliminado.Nrodoc = item.Nrodoc;
                LegajoEliminado.FechaTramite = "";
                LegajoEliminado.FechaBaja = "";

                if (item.Encuadre != null)
                    LegajoEliminado.Encuadre = item.Encuadre.Descripcion;
                else
                    LegajoEliminado.Encuadre = "";


                if (!item.EsContratista.HasValue || !item.EsContratista.Value)
                {
                    LegajoEliminado.NombreEmpresaSubContratista = Capitalize(item.NombreEmpresa.ToLower());
                    LegajoEliminado.NombreEmpresaContratista = Capitalize((from c in dc.ContratoEmpresas
                                                                           where c.Contrato.IdContrato == item.idCon
                                                                           && c.EsContratista.Value
                                                                           select new
                                                                           {
                                                                               Contratista = c.Empresa.RazonSocial
                                                                           }).First().Contratista.ToLower());
                }

                LegajosEncontrados.Add(LegajoEliminado);
            }
        }


        if (LegajosEncontrados.Count > 0)
        {
            InformeAltaBajaLegajo rep = new InformeAltaBajaLegajo();
            rep.InitReport(LegajosEncontrados, FechaInicial);
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

    public static string Capitalize(string value)
    {
        return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);
    }
}
