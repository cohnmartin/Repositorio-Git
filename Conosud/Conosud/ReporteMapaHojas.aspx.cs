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
        cboContratos.DataSource = Helpers.GetContratos(id);
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

    protected void btnBuscar_Click(object sender, EventArgs eventarg)
    {

        EntidadesConosud dc = new EntidadesConosud();
        List<InformeMapaHojasRutaTemp> datos = new List<InformeMapaHojasRutaTemp>();
        long idContrato = long.Parse(cboContratos.SelectedValue);
        List<Entidades.CabeceraHojasDeRuta> cabs = null;

        if (cboPeriodos.SelectedValue == "")
        {
            cabs = (from C in dc.CabeceraHojasDeRuta
                                                  .Include("HojasDeRuta").Include("HojasDeRuta.Plantilla").Include("HojasDeRuta.Plantilla.CategoriasItems").Include("ContratoEmpresas").Include("ContratoEmpresas.Contrato")
                                                  .Include("ContratoEmpresas.Empresa").Include("Estado")
                                                  .Include("ContratoEmpresas.CabeceraHojasDeRuta")
                    where (C.ContratoEmpresas.Contrato.IdContrato == idContrato)
                     &&
                        ((C.Periodo.Month < DateTime.Now.Month && C.Periodo.Year == DateTime.Now.Year) || (C.Periodo.Year < DateTime.Now.Year))
                    orderby C.Periodo ascending
                    select C).ToList<Entidades.CabeceraHojasDeRuta>();
        }
        else
        {
            int año = int.Parse(cboPeriodos.Text.Substring(3, 4));
            int mes = int.Parse(cboPeriodos.Text.Substring(0, 2));

            cabs = (from C in dc.CabeceraHojasDeRuta
                                  .Include("HojasDeRuta").Include("HojasDeRuta.Plantilla").Include("HojasDeRuta.Plantilla.CategoriasItems").Include("ContratoEmpresas").Include("ContratoEmpresas.Contrato")
                                  .Include("ContratoEmpresas.Empresa").Include("Estado")
                                  .Include("ContratoEmpresas.CabeceraHojasDeRuta")
                    where C.ContratoEmpresas.Contrato.IdContrato == idContrato
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

        }

        InformeMapaHojaDeRuta rep = new InformeMapaHojaDeRuta();
        rep.InitReport(datos.OrderBy(w => w.OrdenEmpresas).ToList());
        this.ReportViewer1.Report = rep;
        (ReportViewer1.FindControl("ReportToolbar").FindControl("ExportGr").Controls[0].Controls[0] as DropDownList).ClearSelection();
        (ReportViewer1.FindControl("ReportToolbar").FindControl("ExportGr").Controls[0].Controls[0] as DropDownList).Items.FindByText("Excel").Selected = true;
        trReporte.Visible = true;
        trResultadoVacio.Visible = false;


    }
}
