using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Telerik.Web.UI;

public partial class ReporteEmpresasSinDocumentacion : System.Web.UI.Page
{
    public Hashtable EmpresasContratista = new Hashtable();
    public EntidadesConosud Contexto
    {
        get
        {
            if (Session["Contexto"] != null)
                return (EntidadesConosud)Session["Contexto"];
            else
            {
                Session["Contexto"] = new EntidadesConosud();
                return (EntidadesConosud)Session["Contexto"];
            }
        }
    }
    public List<EstadosRutaTemp> DetallesEstados
    {
        get
        {
            if (Session["DetallesEstados"] != null)
                return (List<EstadosRutaTemp>)Session["DetallesEstados"];
            else
            {
                Session["DetallesEstados"] = new List<EstadosRutaTemp>();
                return (List<EstadosRutaTemp>)Session["DetallesEstados"];
            }
        }
        set
        {
            Session["DetallesEstados"] = value;

        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ConsultaContratos();
        }
    }

    private string GetNombreContratista(bool? esContratista, string CodigoContrato, string NombreEmpresa)
    {
        if (esContratista.HasValue && esContratista.Value)
        {
            if (!EmpresasContratista.ContainsKey(CodigoContrato))
            {
                EmpresasContratista.Add(CodigoContrato, NombreEmpresa);
            }
            return NombreEmpresa;
        }
        else
        {
            if (EmpresasContratista[CodigoContrato] != null)
                return EmpresasContratista[CodigoContrato].ToString();
            else
                return "";
        }
    }

    public string GetNombreSubContratista(bool? esContratista, string CodigoContrato, string NombreEmpresa)
    {
        if (!esContratista.HasValue || !esContratista.Value)
        {
            return NombreEmpresa;
        }
        else
        {
            return "";
        }
    }

    public string GetFormatoFecha(DateTime? fecha)
    {

        if (fecha != null)
        {
            return fecha.Value.ToShortDateString();
        }
        else
            return "";
    }

    private void ConsultaContratos()
    {
        /// Busca: las hojas de ruta donde no se ha presentado documentacion     

        DetallesEstados = new List<EstadosRutaTemp>();
        int mes = DateTime.Now.Month;
        int año = DateTime.Now.Year;

        var a = from C in Contexto.CabeceraHojasDeRuta
                where !C.HojasDeRuta.Any(w => w.DocFechaEntrega.HasValue)
                && !C.HojasDeRuta.Any(w => w.HojaAprobado.HasValue && w.HojaAprobado.Value )
                && !C.HojasDeRuta.Any(w => w.HojaFechaControlado.HasValue)
                && C.Estado.Descripcion != "Aprobada"
                && (C.Periodo.Month < mes && C.Periodo.Year <= año)

                orderby C.ContratoEmpresas.EsContratista.Value descending, C.Periodo
                select new
                {
                    CodigoContrato = C.ContratoEmpresas.Contrato.Codigo,
                    Estado = C.Estado.Descripcion,
                    FechaFin = C.ContratoEmpresas.Contrato.FechaVencimiento,
                    FechaInicio = C.ContratoEmpresas.Contrato.FechaInicio,
                    FechaProrroga = C.ContratoEmpresas.Contrato.Prorroga,
                    NombreEmpresa = C.ContratoEmpresas.Empresa.RazonSocial.Trim(),
                    EsContratista = C.ContratoEmpresas.EsContratista,
                    Servicio = C.ContratoEmpresas.Contrato.Servicio,
                    Periodo = C.Periodo
                };


        foreach (var item in a)
        {
            string NombreContratista = GetNombreContratista(item.EsContratista, item.CodigoContrato, item.NombreEmpresa);
            string NombreSubContratista = GetNombreSubContratista(item.EsContratista, item.CodigoContrato, item.NombreEmpresa);

            if (NombreContratista.Trim() != "")
            {
                EstadosRutaTemp estadoRuta = new EstadosRutaTemp();
                estadoRuta.CodigoContrato = item.CodigoContrato;
                estadoRuta.Estado = item.Estado;
                estadoRuta.FechaFin = GetFormatoFecha(item.FechaFin);
                estadoRuta.FechaInicio = GetFormatoFecha(item.FechaInicio);
                estadoRuta.FechaProrroga = GetFormatoFecha(item.FechaProrroga);
                estadoRuta.NombreEmpresaContratista = NombreContratista;
                estadoRuta.NombreEmpresaSubContratista = NombreSubContratista;
                estadoRuta.Servicio = item.Servicio;
                estadoRuta.Periodo = string.Format("{0:MM/yyyy}", item.Periodo);

                DetallesEstados.Add(estadoRuta);
            }
        }

        gvEstadoContratos.Rebind();
    }
   
    protected void gvEstadoContratos_ItemCommand(object source, GridCommandEventArgs e)
    {
        if (e.CommandName == "ExportContratos")
        {
            gvEstadoContratos.DataSource = DetallesEstados.OrderBy(w => w.CodigoContrato).OrderBy(w => w.NombreEmpresaContratista).ToList();
            gvEstadoContratos.DataBind();
            ConfigureExportAndExport();
        }
    }


    protected void gvEstadoContratos_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        this.gvEstadoContratos.VirtualItemCount = DetallesEstados.Count;
        this.gvEstadoContratos.AllowCustomPaging = true;
        this.gvEstadoContratos.DataSource = DetallesEstados.OrderBy(w => w.CodigoContrato).OrderBy(w => w.NombreEmpresaContratista).Skip(gvEstadoContratos.CurrentPageIndex * gvEstadoContratos.PageSize).Take(gvEstadoContratos.PageSize).ToList();
    }

    public void ConfigureExportAndExport()
    {
        gvEstadoContratos.CurrentPageIndex = 0;
        foreach (Telerik.Web.UI.GridColumn column in gvEstadoContratos.MasterTableView.Columns)
        {
            if (!column.Visible || !column.Display)
            {
                column.Visible = true;
                column.Display = true;
            }
        }

        gvEstadoContratos.ExportSettings.ExportOnlyData = true;
        gvEstadoContratos.ExportSettings.IgnorePaging = true;
        gvEstadoContratos.ExportSettings.FileName = "ContratosSinDocumentacion";
        gvEstadoContratos.MasterTableView.ExportToExcel();



    }
}
