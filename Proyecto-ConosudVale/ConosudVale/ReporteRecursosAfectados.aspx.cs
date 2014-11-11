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

public partial class ReporteRecursosAfectados : System.Web.UI.Page
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

            int CantRecursos = (from emp in Contexto.ContEmpLegajos
                                where emp.CabeceraHojasDeRuta.Periodo >= FechaInicial && emp.CabeceraHojasDeRuta.Periodo < FechaFinal
                                select emp).Count();

            GridRecursosAfectados.VirtualCount = CantRecursos;
            GridRecursosAfectados.DataSource = (GetData(cboPeriodos.Text, 0, GridRecursosAfectados.PageSize) as IList);

        }
    }


    void GridRecursosAfectados_ExportToExcel(object sender)
    {

        List<dynamic> datosExportar = GetData(cboPeriodos.Text, 0, 5000);


        List<string> camposExcluir = new List<string>(); ;
        Dictionary<string, string> alias = new Dictionary<string, string>() {
            { "PeriodoConsulta","Periodo Consulta"},
            { "EmpContratista","Contratista" },
            { "EmpSubContratista","Sub Contratista" },
            { "RazonSocial","Emp Asignada" },
            { "CUITEmpresa","CUIT Empresa"},
            { "EsContratista","Es Contratista"},
            { "Codigo","Nro Contrato"},
            { "FechaInicioContrato","Inicio Contrato"},
            { "PeriodoAfectacion","Afectacion"},
            { "PeriodoBaja","Periodo Baja"},
            { "FechaTramiteBaja","Tramite Baja"},
            { "FechaVencimientoContrato","Vencimiento Contrato"},
            { "Prorroga","Prorroga Contrato"},
            { "FiscalNombre","Fiscal Nombre"},
            { "FiscalEmail" ,"Fiscal Email"},
            { "GestorEmail","Gestor Email"},
            { "GestorNombre","Gestor Nombre"},
            { "DescCategoria","Categoria"},
            { "DescContratadoPor" ,"Contratado Por"},
            { "DescTipoContrato","Tipo Contrato"},
            { "DescArea"  ,"Area"},
            { "DescTipoDocumento","Tipo Documento"},
            { "FechaNacimiento","Nacimiento"},
            { "DescEstadoCicil","Estado Civil"},
            { "DescNacionalida","Nacionalida"},
            { "CodigoPostal","Codigo Postal"},
            { "DescProvincia","Provincia"},
            { "TelefonoFijo","Telefono Fijo"},
            { "CorreoElectronico","Correo Electronico"},
            { "DescConvenio","Convenio"},
            { "FechaIngreos","Ingreso Empresa"},
            { "GrupoSangre","Grupo Sangre"},
            { "AutorizadoCond","Autorizado Conducir"},
            { "CredVencimiento","Cred. Vencimiento"},
            { "FechaUltimoExamen","Ultimo Examen Medico"},
            { "HabilitarCredencial","Habilitar Credencial"},
            { "DescEstadoVerificacion","Estado Verificacion"},
            { "FechaUltimaVerificacion","Ultima Verificacion"},
            { "Observacion","Observacion Auditoria"},
            { "FechaUltmaModificacion","Ultima Modificacion"},
            { "NroPoliza","Nro Poliza"},
            { "DescDescSeguro","Desc. Seguro"},
            { "DescCompañiaSeguro","Compañia Seguro"},
            { "FechaInicial" ,"Inicio Seguro"},
            { "FechaVencimiento" ,"Vencimiento Seguro"},
            { "FechaUltimoPago" ,"Ultimo Pago Seguro"}};


        camposExcluir.Add("IdLegajos");
        camposExcluir.Add("PeriodoConsulta");
        camposExcluir.Add("DescEstadoVerificacion");
        



        List<string> DatosReporte = new List<string>();
        DatosReporte.Add("Listado de Personal afectado por Empresa");
        DatosReporte.Add("Fecha y Hora emisi&oacute;n:" + DateTime.Now.ToString());
        DatosReporte.Add("Per&iacute;odo de Consulta:" + cboPeriodos.Text);
        DatosReporte.Add("Incluye a todo el personal afectado a contratos vigentes del per&iacute;odo de consulta");



        GridView gv = Helpers.GenerarExportExcel(datosExportar.ToList<dynamic>(), alias, camposExcluir, DatosReporte);

        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        gv.RenderControl(htmlWrite);

        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=PersonalAfectado" + "_" + DateTime.Now.ToString("M_dd_yyyy_s") + ".xls");
        HttpContext.Current.Response.ContentType = "application/xls";
        HttpContext.Current.Response.Write(stringWrite.ToString());
        HttpContext.Current.Response.End();


        //GridRecursosAfectados.ExportToExcelFunction("Recursos", (GetData(cboPeriodos.Text, 0, 5000) as IList));

    }


    [WebMethod(EnableSession = true)]
    public static List<dynamic> GetData(string periodo, int start, int take)
    {

        #region Recupero los Datos
        using (EntidadesConosud Contexto = new EntidadesConosud())
        {




            DateTime FechaInicial = Convert.ToDateTime("01/" + periodo);
            DateTime FechaFinal = Convert.ToDateTime("01/" + periodo).AddMonths(1);


            int CantRecursos = (from emp in Contexto.ContEmpLegajos
                                where emp.CabeceraHojasDeRuta.Periodo >= FechaInicial && emp.CabeceraHojasDeRuta.Periodo < FechaFinal
                                select emp).Count();


            var EmpresaContratistas = (from emp in Contexto.ContratoEmpresas
                                       select new
                                       {
                                           emp.IdContratoEmpresas,
                                           emp.Contrato.IdContrato,
                                           emp.Empresa.RazonSocial,
                                           EsContratista = emp.EsContratista.Value
                                       }).Distinct().ToList();



            /// Consulta para determinar el contrato y periodod de baja del legajo si es que posee
            var empActuales = (from emp in Contexto.ContEmpLegajos
                               where emp.CabeceraHojasDeRuta.Periodo >= FechaInicial && emp.CabeceraHojasDeRuta.Periodo < FechaFinal
                               && emp.Legajos.objEmpresaLegajo != null 
                               orderby emp.Legajos.Apellido
                               select new
                               {
                                   idLeg = emp.Legajos.IdLegajos,
                                   PeriodoConsulta = periodo,


                                   /// Datos Propios del Contrato
                                   emp.ContratoEmpresas.Contrato.Codigo,
                                   FechaInicioContrato = emp.ContratoEmpresas.Contrato.FechaInicio.Value,
                                   FechaVencimientoContrato = emp.ContratoEmpresas.Contrato.FechaVencimiento.Value,
                                   FechaVencimientoContratoEfectiva = emp.ContratoEmpresas.Contrato.Prorroga.HasValue && emp.ContratoEmpresas.Contrato.Prorroga.Value > emp.ContratoEmpresas.Contrato.FechaVencimiento ? emp.ContratoEmpresas.Contrato.Prorroga.Value : emp.ContratoEmpresas.Contrato.FechaVencimiento.Value,
                                   emp.ContratoEmpresas.Contrato.IdContrato,
                                   emp.ContratoEmpresas.Contrato.Prorroga,
                                   emp.ContratoEmpresas.Contrato.FiscalNombre,
                                   emp.ContratoEmpresas.Contrato.FiscalEmail,
                                   emp.ContratoEmpresas.Contrato.GestorEmail,
                                   emp.ContratoEmpresas.Contrato.GestorNombre,
                                   emp.ContratoEmpresas.IdContratoEmpresas,
                                   DescCategoria = emp.ContratoEmpresas.Contrato.objCategoria.Descripcion,
                                   DescContratadoPor = emp.ContratoEmpresas.Contrato.objContratadopor.Descripcion,
                                   DescTipoContrato = emp.ContratoEmpresas.Contrato.objTipoContrato.Descripcion,
                                   DescArea = emp.ContratoEmpresas.Contrato.objArea != null ? emp.ContratoEmpresas.Contrato.objArea.Descripcion : "",
                                   emp.ContratoEmpresas.Contrato.Servicio,
                                   EsContratista = emp.ContratoEmpresas.EsContratista,
                                   // Si hay fecha baja tramite es porque en este periodo se dio de baja
                                   PeriodoBaja = (!emp.FechaTramiteBaja.HasValue ? emp.FechaTramiteBaja : emp.CabeceraHojasDeRuta.Periodo),
                                   emp.FechaTramiteBaja,

                                   /// Datos Propios del Legajo
                                   emp.Legajos.Apellido,
                                   emp.Legajos.Nombre,
                                   DescTipoDocumento = emp.Legajos.objTipoDocumento.Descripcion,
                                   emp.Legajos.NroDoc,
                                   emp.Legajos.FechaNacimiento,
                                   emp.Legajos.Sexo,
                                   DescEstadoCicil = emp.Legajos.objEstadoCivil.Descripcion,
                                   emp.Legajos.CUIL,
                                   DescNacionalida = emp.Legajos.objNacionalidad.Descripcion,
                                   emp.Legajos.Direccion,
                                   emp.Legajos.CodigoPostal,
                                   DescProvincia = emp.Legajos.objProvincia.Descripcion,
                                   emp.Legajos.TelefonoFijo,
                                   emp.Legajos.CorreoElectronico,
                                   DescConvenio = emp.Legajos.objConvenio.Descripcion,
                                   emp.Legajos.FechaIngreos,
                                   emp.Legajos.GrupoSangre,
                                   emp.Legajos.AutorizadoCond,
                                   emp.Legajos.CredVencimiento,
                                   emp.Legajos.FechaUltimoExamen,
                                   DescEstadoVerificacion = emp.Legajos.objEstadoVerificacion.Descripcion,
                                   emp.Legajos.FechaUltimaVerificacion,
                                   emp.Legajos.Observacion,
                                   emp.Legajos.FechaUltmaModificacion,
                                   emp.Legajos.Funcion,
                                   emp.Legajos.NroPoliza,
                                   DescDescSeguro = emp.Legajos.Descripcion,
                                   DescCompañiaSeguro = emp.Legajos.objCompañiaSeguro.Descripcion,
                                   emp.Legajos.FechaInicial,
                                   emp.Legajos.FechaVencimiento,
                                   emp.Legajos.FechaUltimoPago,

                                   /// Datos Propios de la Empresa Asignada
                                   CUITEmpresa = emp.Legajos.objEmpresaLegajo.CUIT,
                                   emp.Legajos.objEmpresaLegajo.RazonSocial,

                                   /// Calculo del Perido de afectación del legajo
                                   //PeriodoAfectacion = emp.Legajos.objContEmpLegajos.OrderBy(p => p.CabeceraHojasDeRuta.Periodo).Select(p => p.CabeceraHojasDeRuta.Periodo).FirstOrDefault(),

                                   PeriodoAfectacion = emp.ContratoEmpresas.CabeceraHojasDeRuta.OrderBy(p => p.Periodo).Select(p => p.Periodo).FirstOrDefault(),


                               }).Skip(start).Take(take).ToList();


            var DatosFormateados = (from l in empActuales
                                    select new
                                    {
                                        /// Datos Propios de la Empresa Contratista
                                        EmpContratista = l.EsContratista.HasValue && l.EsContratista == true ? l.RazonSocial : EmpresaContratistas.Where(w => w.IdContrato == l.IdContrato && w.EsContratista).FirstOrDefault().RazonSocial,
                                        EmpSubContratista =  !l.EsContratista.HasValue || l.EsContratista == false  ? l.RazonSocial:"",
                                        l.RazonSocial,
                                        l.CUITEmpresa,
                                        l.Codigo,
                                        l.DescTipoContrato,
                                        l.Servicio,
                                        l.FechaInicioContrato,
                                        l.FechaVencimientoContrato,
                                        l.Prorroga,
                                        l.DescCategoria,
                                        l.DescArea,
                                        l.DescContratadoPor,

                                        Apellido = l.Apellido.ToUpper(),
                                        Nombre = l.Nombre.ToUpper(),
                                        l.DescTipoDocumento,
                                        l.NroDoc,

                                        l.CredVencimiento,
                                        HabilitarCredencial = l.CredVencimiento.HasValue && DateTime.Now < l.CredVencimiento.Value && l.CredVencimiento <= l.FechaVencimientoContratoEfectiva ? "SÍ" : "NO",
                                        l.DescEstadoVerificacion,
                                        l.FechaUltimaVerificacion,
                                        l.Observacion,
                                        l.FechaUltmaModificacion,

                                        PeriodoConsulta = l.PeriodoConsulta,
                                        IdLegajos = l.idLeg,
                                        l.FiscalNombre,
                                        l.FiscalEmail,
                                        l.GestorEmail,
                                        l.GestorNombre,
                                        EsContratista = l.EsContratista.HasValue && l.EsContratista == true ? "Si" : "No",
                                        PeriodoBaja = string.Format("{0:MM/yyyy}", l.PeriodoBaja),
                                        l.FechaTramiteBaja,
                                        l.FechaNacimiento,
                                        Sexo = l.Sexo.HasValue && l.Sexo == true ? "Masculino" : l.Sexo.HasValue && l.Sexo == false ? "Femenino" : "",
                                        l.DescEstadoCicil,
                                        l.CUIL,
                                        l.Direccion,
                                        l.DescProvincia,
                                        l.DescConvenio,
                                        l.FechaUltimoExamen,
                                        l.DescNacionalida,
                                        l.CodigoPostal,
                                        l.TelefonoFijo,
                                        l.CorreoElectronico,
                                        l.FechaIngreos,
                                        l.GrupoSangre,
                                        AutorizadoCond = l.AutorizadoCond == true ? "Si" : "No",
                                        
                                        l.Funcion,
                                        l.NroPoliza,
                                        l.DescDescSeguro,
                                        l.DescCompañiaSeguro,
                                        l.FechaInicial,
                                        l.FechaVencimiento,
                                        l.FechaUltimoPago,
                                        PeriodoAfectacion = string.Format("{0:MM/yyyy}", l.PeriodoAfectacion)
                                        



                                    }).OrderBy(w => w.EmpContratista).Distinct().ToList();

            return DatosFormateados.ToList<dynamic>();
        }
        #endregion
    }
}


