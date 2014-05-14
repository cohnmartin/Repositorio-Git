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

public partial class ReporteEquiposAfectados : System.Web.UI.Page
{
    public EntidadesConosud Contexto
    {
        get
        {
            if (Session["ContextoConsultas"] != null)
                return (EntidadesConosud)Session["ContextoConsultas"];
            else
            {
                Session.Add("ContextoConsultas", new EntidadesConosud());
                return (EntidadesConosud)Session["ContextoConsultas"];
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        GridRecursosAfectados.ExportToExcel += new ControlsAjaxNotti.ClickEventHandler(GridRecursosAfectados_ExportToExcel);

        if (!IsPostBack)
        {
            int CantRecursos = (from vehicu in Contexto.VahiculosyEquipos
                                where vehicu.Tipo == "Equipo" && vehicu.objEmpresa != null
                                select vehicu).Count();

            GridRecursosAfectados.VirtualCount = CantRecursos;
            GridRecursosAfectados.DataSource = (GetData(0, GridRecursosAfectados.PageSize) as IList);

        }
    }

    void GridRecursosAfectados_ExportToExcel(object sender)
    {

        GridRecursosAfectados.ExportToExcelFunction("EquiposAfectados", (GetData(0, 5000) as IList));

    }

    [WebMethod(EnableSession = true)]
    public static object GetData(int start, int take)
    {

        #region Recupero los Datos
        EntidadesConosud Contexto = (HttpContext.Current.Session["ContextoConsultas"] as EntidadesConosud);


        int CantRecursos = (from vehicu in Contexto.VahiculosyEquipos
                            where vehicu.Tipo == "Equipo" && vehicu.objEmpresa != null
                            select vehicu).Count();


        /// Consulta para determinar el contrato y periodod de baja del legajo si es que posee
        var vehicuActuales = (from vehicu in Contexto.VahiculosyEquipos
                              where vehicu.Tipo == "Equipo" && vehicu.objEmpresa != null
                              orderby vehicu.Patente
                              select new
                              {
                                  IdVehiculo = vehicu.IdVehiculoEquipo,

                                  /// Datos Propios del Vehiculo
                                  vehicu.AltaEmpresa,
                                  vehicu.BajaEmpresa,
                                  vehicu.CapacidadCarga,
                                  CompañiaSeguro = vehicu.objCompañiaSeguro.Descripcion,
                                  vehicu.DescripcionSeguro,
                                  vehicu.EquipamientoAgregado,
                                  EsPropio = vehicu.EsPropio.HasValue && vehicu.EsPropio == true ? "Si" : "No",
                                  vehicu.FechaFabricacion,
                                  vehicu.FechaHabilitacion,
                                  vehicu.FechaInicialSeguro,
                                  vehicu.FechaUltimoPagoSeguro,
                                  vehicu.FechaVencimientoHabilitacion,
                                  vehicu.FechaVencimientoSeguro,
                                  HabilitarCredencial = (!vehicu.HabilitarCredencial.HasValue || vehicu.HabilitarCredencial == false) ? "No" : (vehicu.VencimientoCredencial < DateTime.Now) ? "No" : "SÍ",
                                  vehicu.Marca,
                                  vehicu.Modelo,
                                  vehicu.NombreTitular,
                                  vehicu.NroChasis,
                                  vehicu.NroHabilitacion,
                                  vehicu.NroMotor,
                                  vehicu.NroPolizaSeguro,
                                  vehicu.Patente,
                                  TipoUnidad = vehicu.objTipoUnidad.Descripcion,
                                  vehicu.VencimientoCredencial,
                                  vehicu.FechaUltimaActualizacion,
                                  vehicu.FechaCreacion,

                                  /// Datos Propios del Contrato
                                  vehicu.objContrato.Codigo,
                                  FechaInicioContrato = vehicu.objContrato.FechaInicio,
                                  FechaVencimientoContrato = vehicu.objContrato.FechaVencimiento,
                                  vehicu.objContrato.Prorroga,
                                  DescCategoria = vehicu.objContrato.objCategoria.Descripcion,
                                  DescContratadoPor = vehicu.objContrato.Contratadopor.Descripcion,
                                  DescTipoContrato = vehicu.objContrato.TipoContrato.Descripcion,
                                  vehicu.objContrato.Servicio,

                                  ///// Datos Propios de la Empresa Asignada
                                  CUITEmpresa = vehicu.objEmpresa.CUIT,
                                  vehicu.objEmpresa.RazonSocial,

                              }).Skip(start).Take(take).ToList();


        var DatosFormateados = (from l in vehicuActuales
                                select new
                                {
                                    IdLegajos = l.IdVehiculo,
                                    FechaConsulta = DateTime.Now.ToShortDateString(),
                                    l.AltaEmpresa,
                                    l.BajaEmpresa,
                                    l.CapacidadCarga,
                                    l.CompañiaSeguro,
                                    l.DescripcionSeguro,
                                    l.EquipamientoAgregado,
                                    l.EsPropio,
                                    l.FechaFabricacion,
                                    l.FechaHabilitacion,
                                    l.FechaInicialSeguro,
                                    l.FechaUltimoPagoSeguro,
                                    l.FechaVencimientoHabilitacion,
                                    l.FechaVencimientoSeguro,
                                    l.HabilitarCredencial,
                                    l.Marca,
                                    l.Modelo,
                                    l.NombreTitular,
                                    l.NroChasis,
                                    l.NroHabilitacion,
                                    l.NroMotor,
                                    l.NroPolizaSeguro,
                                    l.Patente,
                                    l.TipoUnidad,
                                    l.VencimientoCredencial,
                                    l.FechaUltimaActualizacion,
                                    l.FechaCreacion,

                                    l.Codigo,
                                    l.FechaInicioContrato,
                                    l.FechaVencimientoContrato,
                                    l.Prorroga,
                                    l.DescCategoria,
                                    l.DescContratadoPor,
                                    l.DescTipoContrato,
                                    l.Servicio,

                                    l.CUITEmpresa,
                                    l.RazonSocial,
                                }).Distinct().ToList();

        return DatosFormateados;
        #endregion
    }

}