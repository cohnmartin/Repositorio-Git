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

public partial class ReporteVehiculosAfectados : System.Web.UI.Page
{
    private EntidadesConosud _Contexto;

    public EntidadesConosud Contexto
    {
        //get
        //{
        //    if (Session["ContextoConsultas"] != null)
        //        return (EntidadesConosud)Session["ContextoConsultas"];
        //    else
        //    {
        //        Session.Add("ContextoConsultas", new EntidadesConosud());
        //        return (EntidadesConosud)Session["ContextoConsultas"];
        //    }
        //}

        get
        {

            if (_Contexto == null)
            {
                _Contexto = new EntidadesConosud();
                return _Contexto;
            }
            else
                return _Contexto;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        GridRecursosAfectados.ExportToExcel += new ControlsAjaxNotti.ClickEventHandler(GridRecursosAfectados_ExportToExcel);

        if (!IsPostBack)
        {
            DateTime fechaInicial = DateTime.Now;

            for (int i = 0; i < 25; i++)
            {
                DateTime fechaActual = fechaInicial.AddMonths(-1 * i);
                string FechaFormat = string.Format("{0:MM/yyyy}", fechaActual);
                cboPeriodos.Items.Add(new ListItem(FechaFormat, FechaFormat));
            }

            cboPeriodos.Items[0].Selected = true;

            DateTime FechaInicial = Convert.ToDateTime("01/" + cboPeriodos.Text);
            DateTime FechaFinal = Convert.ToDateTime("01/" + cboPeriodos.Text).AddMonths(1);


            int CantRecursos = (from vehicu in Contexto.VahiculosyEquipos
                                where vehicu.Tipo == "Vehículo" && vehicu.objEmpresa != null
                                select vehicu).Count();

            GridRecursosAfectados.VirtualCount = CantRecursos;
            GridRecursosAfectados.DataSource = (GetData(cboPeriodos.Text, 0, GridRecursosAfectados.PageSize) as IList);

        }
    }

    void GridRecursosAfectados_ExportToExcel(object sender)
    {

        List<dynamic> datosExportar = GetData(cboPeriodos.Text, 0, 5000);


        List<string> camposExcluir = new List<string>(); ;
        Dictionary<string, string> alias = new Dictionary<string, string>() {
            {"FechaConsulta" ,"Fecha Consulta" },
            {"Patente" ,"Patente"},
            {"Marca"  ,"Marca" },
            {"Modelo"  ,"Modelo"},
            {"AltaEmpresa","Alta Empresa"},
            {"BajaEmpresa","Baja Empresa"},
            {"CapacidadCarga" ,"Capacidad Carga"},
            {"CompañiaSeguro","Compañia Seguro"},
            {"DescripcionSeguro" ,"Des Seguro"},
            {"EquipamientoAgregado","Equipamiento Agregado"},
            {"EsPropio" ,"Es Propio"},
            {"FechaFabricacion","Fecha Fabricacion"},
            {"FechaHabilitacion","Fecha Habilitacion"},
            {"FechaInicialSeguro","Fecha Inicial Seguro"},
            {"FechaUltimoPagoSeguro","Fecha Ult Pago Seguro"},
            {"FechaVencimientoHabilitacion","Fecha Venc Habilitacion"},
            {"FechaVencimientoSeguro","Fecha Venc Seguro"},
            {"HabilitarCredencial","HabilitarCredencial"},
            {"NombreTitular","Nombre Titular"},
            {"NroChasis" ,"Nro Chasis"},
            {"NroHabilitacion" ,"Nro Habilitacion"},
            {"NroMotor" ,"Nro Motor"},
            {"NroPolizaSeguro","Nro Poliza Seguro"},
            {"TipoUnidad" ,"Tipo Unidad"},
            {"VencimientoCredencial" ,"Vencimiento Credencial"},
            {"Codigo","Codigo Contrato"},
            {"FechaInicioContrato","Fecha Inicio Contrato"},
            {"FechaVencimientoContrato","Fecha Vencimiento Contrato"},
            {"Prorroga"   ,"Prorroga"},
            {"FiscalNombre","Fiscal Nombre"},
            {"FiscalEmail","Fiscal Email"},
            {"GestorEmail","Gestor Email"},
            {"GestorNombre"  ,"Gestor Nombre"},
            {"DescCategoria","Categoria"},
            {"DescContratadoPor","Contratado Por" },
            {"DescTipoContrato","Tipo Contrato"},
            {"Servicio" ,"Servicio" },
            {"DescArea"  ,"Area" },
            {"CUITEmpresa","CUIT Empresa"},
            {"RazonSocial","Razon Social Emp"}};


        camposExcluir.Add("IdVehiculo");
        camposExcluir.Add("IdLegajos");
        camposExcluir.Add("FechaConsulta");




        List<string> DatosReporte = new List<string>();
        DatosReporte.Add("Listado de Veh&iacute;culos afectados por Empresa");
        DatosReporte.Add("Fecha y Hora emisi&oacute;n:" + DateTime.Now.ToString());
        DatosReporte.Add("Per&iacute;odo de Consulta:" + cboPeriodos.Text);
        DatosReporte.Add("Incluye a todos los veh&iacute;culos afectados a contratos vigentes del per&iacute;odo de consulta");


        GridView gv = Helpers.GenerarExportExcel(datosExportar.ToList<dynamic>(), alias, camposExcluir, DatosReporte);

        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        gv.RenderControl(htmlWrite);

        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=VehiculosAfectados" + "_" + DateTime.Now.ToString("M_dd_yyyy_H_M_s") + ".xls");
        HttpContext.Current.Response.ContentType = "application/xls";
        HttpContext.Current.Response.Write(stringWrite.ToString());
        HttpContext.Current.Response.End();


        //GridRecursosAfectados.ExportToExcelFunction("Recursos", (GetData(cboPeriodos.Text, 0, 5000) as IList));

    }

    [WebMethod(EnableSession = true)]
    public static List<dynamic> GetData(string periodo, int start, int take)
    {

        #region Recupero los Datos
        /// Calculo el utlimo día del mes de consulta.
        DateTime fechaConsulta = DateTime.Parse("01/" + periodo).AddMonths(1).AddDays(-1);

        using (EntidadesConosud Contexto = new EntidadesConosud())
        {
            //EntidadesConosud Contexto = (HttpContext.Current.Session["ContextoConsultas"] as EntidadesConosud);





            int CantRecursos = (from vehicu in Contexto.VahiculosyEquipos
                                where vehicu.Tipo == "Vehículo" && vehicu.objEmpresa != null
                                select vehicu).Count();

            DateTime fechaTempota = DateTime.Parse("01/01/3000");

            /// Consulta para determinar el contrato y periodod de baja del legajo si es que posee
            var vehicuActuales = (from vehicu in Contexto.VahiculosyEquipos
                                  where vehicu.Tipo == "Vehículo" && vehicu.objEmpresa != null
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

                                      /// Datos Propios del Contrato
                                      vehicu.objContrato.Codigo,
                                      FechaInicioContrato = vehicu.objContrato.FechaInicio,
                                      FechaVencimientoContrato = vehicu.objContrato.FechaVencimiento,
                                      vehicu.objContrato.Prorroga,

                                      FechaVencimientoContratoEfectiva = vehicu.objContrato == null ? fechaTempota : vehicu.objContrato.Prorroga.HasValue && vehicu.objContrato.Prorroga.Value > vehicu.objContrato.FechaVencimiento ? vehicu.objContrato.Prorroga.Value : vehicu.objContrato.FechaVencimiento.Value,

                                      vehicu.objContrato.FiscalNombre,
                                      vehicu.objContrato.FiscalEmail,
                                      vehicu.objContrato.GestorEmail,
                                      vehicu.objContrato.GestorNombre,
                                      DescCategoria = vehicu.objContrato.objCategoria.Descripcion,
                                      DescContratadoPor = vehicu.objContrato.objContratadopor.Descripcion,
                                      DescTipoContrato = vehicu.objContrato.objTipoContrato.Descripcion,
                                      DescArea = vehicu.objContrato.objArea != null ? vehicu.objContrato.objArea.Descripcion : "",
                                      vehicu.objContrato.Servicio,

                                      ///// Datos Propios de la Empresa Asignada
                                      CUITEmpresa = vehicu.objEmpresa.CUIT,
                                      vehicu.objEmpresa.RazonSocial,

                                  }).Skip(start).Take(take).ToList();


            var DatosFormateados = (from l in vehicuActuales
                                    where fechaConsulta <= l.FechaVencimientoContratoEfectiva
                                    select new
                                    {
                                        l.Patente,
                                        l.Marca,
                                        l.Modelo,
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
                                        l.NombreTitular,
                                        l.NroChasis,
                                        l.NroHabilitacion,
                                        l.NroMotor,
                                        l.NroPolizaSeguro,
                                        l.TipoUnidad,
                                        l.VencimientoCredencial,
                                        l.Codigo,
                                        l.FechaInicioContrato,
                                        l.FechaVencimientoContrato,
                                        l.Prorroga,
                                        l.FiscalNombre,
                                        l.FiscalEmail,
                                        l.GestorEmail,
                                        l.GestorNombre,
                                        l.DescCategoria,
                                        l.DescContratadoPor,
                                        l.DescTipoContrato,
                                        l.Servicio,
                                        l.DescArea,
                                        l.CUITEmpresa,
                                        l.RazonSocial,
                                        IdLegajos = l.IdVehiculo,
                                        FechaConsulta = DateTime.Now.ToShortDateString(),
                                    }).Distinct().ToList();

            return DatosFormateados.ToList<dynamic>();

        }
        #endregion
    }

}



