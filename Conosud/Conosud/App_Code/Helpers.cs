using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Entidades;
using System.Reflection;
using System.Data.Objects;
using System.ComponentModel;
using System.Linq.Expressions;

/// <summary>
/// Summary description for Helpers
/// </summary>

public static class EntityDataSourceExtensions
{

    public static TEntity GetItemObject<TEntity>(object dataItem) where TEntity : class
    {

        var entity = dataItem as TEntity;

        if (entity != null)
        {

            return entity;

        }

        var td = dataItem as ICustomTypeDescriptor;

        if (td != null)
        {
            try
            {
                return (TEntity)td.GetPropertyOwner(null);
            }
            catch
            {
                return null;
            }

        }

        return null;

    }

}

public class Helpers
{
    public static Hashtable _SubContratistas = null;
    public static Hashtable _Cabeceras = null;
    public static Hashtable _Contratos = null;
    public static string _rutaProyecto = "";
    public static long _UltimoPeriodo = 0;

    public abstract class Constantes
    {
        public const string ContextoVehiculosYPF = "Vehiculos YPF";

        public abstract class PaginaMenu_
        {
            public const string Empresas = "ABMEmpresas2";
            public const string Contratos = "ABMContratos";
            public const string Legajos = "ABMLegajos2";
            public const string Plantilla = "ABMItems";
            public const string Varios = "ABMClasificaciones";
            public const string AsignacionSubContratistas = "AsignacionEmpresasContratos";
            public const string AsignacionLegajos = "AsignacionLegajosContratos";
            public const string Documentacion = "ConsultDocumentacion";
            public const string Visor = "ConsultaHojadeRuta";
            public const string GestionPublicacion = "GestionPublicacion";
            public const string GestionUsuarios = "ABMUsuarios2";
            public const string GestionRoles = "ABMRoles2";
        }

    
    }

    public enum EstadosHoja
    { 
        Aprobada=14,
        NoAprobada=15
    }

    public enum RolesEspeciales
    {
        Aprobador,
        Publicador,
        Administrador
    }

    public Helpers()
    {
        //
        // TODO: Add constructor logic here
        //

    }

    public static string RutaProyecto
    {
        get { return _rutaProyecto; }
        set { _rutaProyecto = value; }
    }

    public static void GenerarHojadeRuta(ObjectContext pContext, DateTime FInicio, DateTime FFin, Entidades.ContratoEmpresas pContratoEmpresas)
    {
        string queryString = @"SELECT VALUE CabeceraHojasDeRuta FROM 
            EntidadesConosud.CabeceraHojasDeRuta AS CabeceraHojasDeRuta";
        ObjectQuery<Entidades.CabeceraHojasDeRuta> CabeceraHojasDeRutaQuery1 =
            new ObjectQuery<Entidades.CabeceraHojasDeRuta>(queryString, pContext);

        DateTime FechaInicio = FInicio;
        DateTime FechaFinal = FFin;

        int ultimonrocarpeta = 1;
        try
        {

            ultimonrocarpeta = CabeceraHojasDeRutaQuery1.Max(c => c.NroCarpeta);
            ultimonrocarpeta += 1;
        }
        catch { }


        long  id = Convert.ToInt64(Helpers.EstadosHoja.NoAprobada);
        IEnumerable<KeyValuePair<string, object>> entityKeyValues = new KeyValuePair<string, object>[] { 
                new KeyValuePair<string, object>("IdClasificacion", id) };
        EntityKey key = new EntityKey("EntidadesConosud.Clasificacion", entityKeyValues);
        Entidades.Clasificacion _est = (Entidades.Clasificacion)pContext.GetObjectByKey(key);

        /// Guardo la ultima cabecera antes de generar las nuevas para
        /// luego obtener los legados de la mism.

        Entidades.CabeceraHojasDeRuta UltimaCabecera = null;
        if (pContratoEmpresas.IdContratoEmpresas > 0)
        {
            if (!pContratoEmpresas.CabeceraHojasDeRuta.IsLoaded) { pContratoEmpresas.CabeceraHojasDeRuta.Load(); }
            UltimaCabecera = pContratoEmpresas.CabeceraHojasDeRuta.OrderBy(w => w.Periodo).Last();
        }


        while (GeneraxFecha(ref FechaInicio, ref FechaFinal))
        {
            if (UltimaCabecera == null || !(UltimaCabecera.Periodo.Month == FechaFinal.Month && UltimaCabecera.Periodo.Year == FechaFinal.Year))
            {
                /// control por las dudas que el primer periodo que se intenta crear ya existe.
                if (UltimaCabecera == null || string.Format("{0:MMyyyy}", FechaInicio) != string.Format("{0:MMyyyy}", UltimaCabecera.Periodo))
                {
                    /// controlo que el periodo que se esta intentando crear no exista ya.
                    if (!pContratoEmpresas.CabeceraHojasDeRuta.Any(w => string.Format("{0:MMyyyy}", w.Periodo) == string.Format("{0:MMyyyy}", FechaInicio)))
                    {

                        /// Genero la cabecera de hoja de ruta
                        Entidades.CabeceraHojasDeRuta _CabHojaRuta = new Entidades.CabeceraHojasDeRuta();
                        _CabHojaRuta.ContratoEmpresas = pContratoEmpresas;
                        _CabHojaRuta.Estado = _est;
                        _CabHojaRuta.Periodo = FechaInicio;
                        _CabHojaRuta.NroCarpeta = ultimonrocarpeta;
                        _CabHojaRuta.Estimacion = string.Empty;
                        _CabHojaRuta.EsFueraTermino = false;
                        pContext.AddObject("EntidadesConosud.CabeceraHojasDeRuta", _CabHojaRuta);

                        queryString = @"SELECT VALUE Plantilla FROM EntidadesConosud.Plantilla AS Plantilla";

                        ObjectQuery<Entidades.Plantilla> PlantillaQuery1 =
                            new ObjectQuery<Entidades.Plantilla>(queryString, pContext);

                        /// Genero los items de las hojas de ruta
                        foreach (Entidades.Plantilla plan in PlantillaQuery1.Select(p => p))
                        {
                            Entidades.HojasDeRuta _HojasDeRuta = new Entidades.HojasDeRuta();
                            _HojasDeRuta.Plantilla = plan;
                            _HojasDeRuta.HojaAprobado = false;
                            _HojasDeRuta.HojaComentario = string.Empty;
                            _HojasDeRuta.AuditadoPor = string.Empty;
                            _HojasDeRuta.DocComentario = string.Empty;
                            _CabHojaRuta.HojasDeRuta.Add(_HojasDeRuta);

                            pContext.AddObject("EntidadesConosud.HojasDeRuta", _HojasDeRuta);
                        }

                        /// Asocio  los legajos a la nueva cabecera

                        if (UltimaCabecera != null)
                        {
                            if (!UltimaCabecera.ContEmpLegajos.IsLoaded) { UltimaCabecera.ContEmpLegajos.Load(); }
                            foreach (Entidades.ContEmpLegajos itemContLeg in UltimaCabecera.ContEmpLegajos)
                            {
                                if (!itemContLeg.LegajosReference.IsLoaded) { itemContLeg.LegajosReference.Load(); }

                                Entidades.ContEmpLegajos newContLeg = new Entidades.ContEmpLegajos();
                                newContLeg.ContratoEmpresas = pContratoEmpresas;
                                newContLeg.Legajos = itemContLeg.Legajos;
                                _CabHojaRuta.ContEmpLegajos.Add(newContLeg);

                                pContext.AddObject("EntidadesConosud.ContEmpLegajos", newContLeg);
                            }
                        }
                    }
                }
            }

            FechaInicio = FechaInicio.AddMonths(1);
            ultimonrocarpeta += 1;
        }
    }

    public static void GenerarHojadeRuta(DSConosud Ds, DateTime FInicio, DateTime FFin, long pIdContratoEmpresas )
    {
        ////Comienzo Proceso de creacion de Hoja de Ruta y Documentacion
        DSConosudTableAdapters.CategoriasItemsTableAdapter TACatItems = new DSConosudTableAdapters.CategoriasItemsTableAdapter();
        TACatItems.Fill(Ds.CategoriasItems);

        DSConosudTableAdapters.PlantillaTableAdapter TAPlantilla = new DSConosudTableAdapters.PlantillaTableAdapter();
        TAPlantilla.Fill(Ds.Plantilla);
        //DSConosudTableAdapters.ContratoTableAdapter TAContrato = new DSConosudTableAdapters.ContratoTableAdapter();
        //TAContrato.Fill(Ds.Contrato);
        //DSConosudTableAdapters.PlantillaTableAdapter TAPlantilla = new DSConosudTableAdapters.PlantillaTableAdapter();
        //TAPlantilla.Fill(Ds.Plantilla);
        DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter TACabHojaRuta = new DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter();
        DSConosudTableAdapters.HojasDeRutaTableAdapter TAHojaRuta = new DSConosudTableAdapters.HojasDeRutaTableAdapter();

        DSConosud.CabeceraHojasDeRutaRow drCabHojaRuta;
        DSConosud.HojasDeRutaRow drHojaRuta;
        DateTime FechaInicio = new DateTime(FInicio.Year, FInicio.Month, 1);

        DSConosud dstemp = new DSConosud();
        dstemp.EnforceConstraints = false;
        dstemp.CabeceraHojasDeRuta.Clear();
        long ultimonrocarpeta = 0;
        try
        {
            TACabHojaRuta.FillUltimoNroCarpeta(dstemp.CabeceraHojasDeRuta);
            if (dstemp.CabeceraHojasDeRuta.Rows.Count != 0)
            {
                ultimonrocarpeta = Convert.ToInt64(dstemp.CabeceraHojasDeRuta[0]["UltimoNroCarpeta"]);
            }
        }
        catch (Exception)
        {
            
        }
        ultimonrocarpeta += 1;

        while (GeneraxFecha(ref FechaInicio, ref FFin))
        {
            drCabHojaRuta = Ds.CabeceraHojasDeRuta.NewCabeceraHojasDeRutaRow();
            drCabHojaRuta.IdContratoEmpresa = pIdContratoEmpresas;
            drCabHojaRuta.IdEstado = 15;
            drCabHojaRuta.Periodo = FechaInicio;
            drCabHojaRuta.NroCarpeta = ultimonrocarpeta;
            drCabHojaRuta.Estimacion = string.Empty;
            Ds.CabeceraHojasDeRuta.AddCabeceraHojasDeRutaRow(drCabHojaRuta);

            foreach (DSConosud.PlantillaRow rowplan in Ds.Plantilla)
            {
                drHojaRuta = Ds.HojasDeRuta.NewHojasDeRutaRow();
                drHojaRuta.IdPlanilla = rowplan.IdPlantilla;
                drHojaRuta.HojaAprobado = false;
                drHojaRuta.HojaComentario = string.Empty;
                drHojaRuta.AuditadoPor = string.Empty;
                drHojaRuta.DocComentario = string.Empty;
                drHojaRuta.IdCabeceraHojaDeRuta = drCabHojaRuta.IdCabeceraHojasDeRuta;
                Ds.HojasDeRuta.AddHojasDeRutaRow(drHojaRuta);
            }

            FechaInicio = FechaInicio.AddMonths(1);
            ultimonrocarpeta += 1;
        }
    }

    public static DateTime? DeterminarFinPeriodo(DateTime? FechaVenc, DateTime? FechaPro, ref DateTime? FechaFinAnt)
    {
        if (FechaPro != null)
        {
            if (FechaPro > FechaVenc)
            {
                FechaFinAnt = FechaPro;
            }
            else
            {
                FechaFinAnt = FechaVenc;
            }
        }
        else
        {
            FechaFinAnt = FechaVenc;
        }

        return FechaFinAnt;
    }

    private static bool GeneraxFecha(ref DateTime FInicio, ref DateTime FVencimiento)
    {
        if (FInicio.Year < FVencimiento.Year)
        {
            return true;
        }
        else
        {
            if ((FInicio.Year == FVencimiento.Year) && (FInicio.Month <= FVencimiento.Month))
            {
                return true;
            }
            return false;
        }
        return false;
    }

    public class GenExcell
    {
        StreamWriter w;
        DataControlFieldCollection colCampos = null;

        public int DoExcell(string ruta, DataTable dt)
        {
            FileStream fs = new FileStream(ruta, FileMode.Create,
            FileAccess.ReadWrite);
            w = new StreamWriter(fs);
            string bgColor = "", fontColor = "";
            bgColor = " bgcolor=\"Blue\" ";
            fontColor = " style=\"font-size: 12px;color: white;\" ";

            EscribeCabecera(dt.TableName.ToUpper());

            w.Write(@"<tr >");
            foreach (DataColumn dc in dt.Columns)
            {
                w.Write(@" <td {0} {1}> {2} </td>", bgColor, fontColor, dc.ColumnName);
            }
            w.Write(@" </tr >");

            foreach (DataRow dr in dt.Rows)
            {
                EscribeLineaTable(dr);
            }

            EscribePiePagina();
            w.Close();
            return 0;
        }

        public int DoExcell(string ruta, DataTable dt, DataControlFieldCollection col)
        {
            colCampos = col;
            FileStream fs = new FileStream(ruta, FileMode.Create,
            FileAccess.ReadWrite); 
            w = new StreamWriter(fs);
            string bgColor = "", fontColor = "";
            bgColor = " bgcolor=\"Blue\" ";
            fontColor = " style=\"font-size: 12px;color: white;\" ";


            EscribeCabecera(dt.TableName.ToUpper());

            w.Write(@"<tr >");
            foreach (DataControlField dcf in colCampos)
            {
                if (dcf is BoundField || dcf is TemplateField)
                {
                    w.Write(@" <td {0} {1}> {2} </td>", bgColor, fontColor, dcf.HeaderText);
                }
            }
            w.Write(@" </tr >");

            foreach (DataRow dr in dt.Rows)
            {
                EscribeLinea(dr);
            }

            EscribePiePagina();
            w.Close();
            return 0;
        }

        public void EscribeCabecera(string nombre)
        {
            StringBuilder html = new StringBuilder();
            html.Append("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">");
            html.Append("<html>");
            html.Append("  <head>");
            html.Append("<title>www.devjoker.com</title>");
            html.Append("<meta http-equiv=\"Content-Type\"content=\"text/html; charset=UTF-8\" />");
            html.Append("  </head>");
            html.Append("<body>");
            html.Append("<p>");
            html.Append("<table>");
            html.Append("<tr style=\"font-weight:bold;font-size: 16px;color: white;\">");
            html.Append("<td></td><td bgcolor=\"Red\">" + nombre + "</td>");
            html.Append("</tr>"); w.Write(html.ToString());
        }

        public void EscribeLinea(DataRow dr) 
        { 
            string bgColor = "", fontColor = "";
            fontColor = " style=\"font-size: 10px;\" "; 

            w.Write(@"<tr >");
            foreach (DataControlField dcf in colCampos)
            {
                if (dcf is BoundField )
                {
                    w.Write(@" <td {0} {1}> {2} </td>", bgColor, fontColor, dr[((BoundField)dcf).DataField].ToString());
                }
                else if (dcf is TemplateField)
                {
                    w.Write(@" <td {0} {1}> {2} </td>", bgColor, fontColor, dr[dcf.ToString()].ToString());
                }

            }
            w.Write(@" </tr >");
        }

        public void EscribeLineaTable(DataRow dr)
        {
            string bgColor = "", fontColor = "";
            fontColor = " style=\"font-size: 10px;\" ";

            w.Write(@"<tr >");
            foreach (DataColumn dc in dr.Table.Columns)
            {
                w.Write(@" <td {0} {1}> {2} </td>", bgColor, fontColor, dr[dc.ColumnName].ToString());
            }
            w.Write(@" </tr >");
        }    
        
        public void EscribePiePagina() 
        { 
            StringBuilder html = new StringBuilder();
            html.Append("  </table>"); html.Append("</p>");
            html.Append(" </body>"); html.Append("</html>");
            w.Write(html.ToString()); 
        }
    }
    
    public static List<Entidades.Empresa> GetEmpresas(long IdUsuario)
    {
    
        //long IdUsuario = long.Parse(["idusuario"].ToString());
        //long IdUsuario = 4;

        EntidadesConosud db = new EntidadesConosud();
        //.Include("Empresa")
        List<Entidades.Empresa> empresas = (from U in db.SegUsuario
                                    where U.IdSegUsuario == IdUsuario
                                    && U.Empresa != null
                                            select U.Empresa).ToList<Entidades.Empresa>();


        if (empresas.Count == 1)
        {
            return empresas;
        }
        else
        {
           empresas = (from E in db.Empresa
                           orderby E.RazonSocial
                        select E).ToList<Entidades.Empresa>();

            return empresas;
        }

    }

    public static List<Entidades.Empresa> GetEmpresasContratistas(long IdUsuario)
    {

        //long IdUsuario = long.Parse(Application["idusuario"].ToString());
        //long IdUsuario = 4;

        EntidadesConosud db = new EntidadesConosud();

        List<Entidades.Empresa> empresas = (from U in db.SegUsuario.Include("Empresa")
                                            where U.IdSegUsuario == IdUsuario
                                            && U.Empresa != null
                                            select U.Empresa).ToList<Entidades.Empresa>();


        if (empresas.Count == 1)
        {
            return empresas;
        }
        else
        {
            var varempresas = (from C in db.ContratoEmpresas.Include("Empresa")
                               where C.EsContratista == true
                               select C.Empresa).Distinct<Entidades.Empresa>();

            empresas = (from Emp in varempresas
                       orderby Emp.RazonSocial
                       select Emp).ToList<Entidades.Empresa>();

            return empresas;
        }

    }

    public static List<Entidades.Contrato> GetContratos(int id)
    {

        EntidadesConosud db = new EntidadesConosud();

        List<Entidades.Contrato> contratos = (from C in db.ContratoEmpresas
                                  where C.Empresa.IdEmpresa == id
                                  && C.EsContratista.Value 
                                   select C.Contrato).ToList<Entidades.Contrato>();


        return contratos;
    }

    public static object GetContratistas(int id)
    {
//        SELECT DISTINCT ContratoEmpresas.IdContratoEmpresas, ContratoEmpresas.IdContrato, Empresa.RazonSocial, ContratoEmpresas.EsContratista
//FROM         ContratoEmpresas INNER JOIN
//                      Empresa ON ContratoEmpresas.IdEmpresa = Empresa.IdEmpresa INNER JOIN
//                      CabeceraHojasDeRuta ON ContratoEmpresas.IdContratoEmpresas = CabeceraHojasDeRuta.IdContratoEmpresa
//WHERE     (ContratoEmpresas.IdContrato = @IdContrato)

        EntidadesConosud db = new EntidadesConosud();

        var contratosEmp = from C in db.ContratoEmpresas
                           where C.Contrato.IdContrato == id
                           select new {C.IdContratoEmpresas , C.Empresa.RazonSocial};


        return contratosEmp;
    }


    /// <summary>
    /// Devuelve los periodos del contrato sin tener en cuenta el mes actual
    /// </summary>
    /// <param name="id">Contrato en cuestion</param>
    /// <param name="IdUsuarioLogin">Usuario que esta logeado</param>
    /// <returns></returns>
    public static List<Entidades.CabeceraHojasDeRuta> GetPeriodos(int id, long IdUsuarioLogin)
    {
        EntidadesConosud db = new EntidadesConosud();
        string DescRol = Helpers.RolesEspeciales.Administrador.ToString();

        int RolesAdministrador = (from U in db.SegUsuario
                                   from UR in U.SegUsuarioRol
                                   where U.IdSegUsuario == IdUsuarioLogin
                                   && UR.SegRol.Descripcion == DescRol
                                   select UR).Count();


        DateTime Fecha = Convert.ToDateTime("01/"+ DateTime.Now.AddMonths(-1).Month.ToString() + "/" + DateTime.Now.AddMonths(-1).Year.ToString());
        
        if (RolesAdministrador > 0)
            Fecha = DateTime.Now.AddMonths(-100);
      


       
        long idEstado = 15;
        DateTime FechaHasta = DateTime.Now.AddMonths(-1);

        List<Entidades.CabeceraHojasDeRuta> periodos = (from C in db.CabeceraHojasDeRuta
                                                        where C.Estado.IdClasificacion == idEstado
                                              && C.ContratoEmpresas.IdContratoEmpresas == id
                                              && ((C.Periodo < FechaHasta) || (C.Periodo.Month == FechaHasta.Month && C.Periodo.Year == FechaHasta.Year))
                                              orderby C.Periodo
                                                        select C).ToList<Entidades.CabeceraHojasDeRuta>();


        return periodos;
    }

    /// <summary>
    /// Devuelve los periodos del contrato hasta el mes actual
    /// </summary>
    /// <param name="id">Contrato en cuestion</param>
    /// <param name="IdUsuarioLogin">Usuario que esta logeado</param>
    /// <returns></returns>
    public static List<Entidades.CabeceraHojasDeRuta> GetPeriodosAsignacionLegajos(int id, long IdUsuarioLogin)
    {
        EntidadesConosud db = new EntidadesConosud();
        DateTime Fecha = DateTime.Now.AddMonths(-100);

        /// Codigo comentado: antes si no era administrador no podia trabajar con
        /// meses anteriores, eso se cambio.
        //string DescRol = Helpers.RolesEspeciales.Administrador.ToString();
        //int RolesAdministrador = (from U in db.SegUsuario
        //                          from UR in U.SegUsuarioRol
        //                          where U.IdSegUsuario == IdUsuarioLogin
        //                          && UR.SegRol.Descripcion == DescRol
        //                          select UR).Count();
        //if (RolesAdministrador > 0)
        //    Fecha = DateTime.Now.AddMonths(-100);




        long idEstado = 15;
        DateTime FechaHasta = DateTime.Now;

        List<Entidades.CabeceraHojasDeRuta> periodos = (from C in db.CabeceraHojasDeRuta
                                                        where C.Estado.IdClasificacion == idEstado
                                              && C.ContratoEmpresas.IdContratoEmpresas == id
                                              && ((C.Periodo >= Fecha && C.Periodo < FechaHasta) || (C.Periodo.Month == FechaHasta.Month && C.Periodo.Year == FechaHasta.Year))
                                                        orderby C.Periodo
                                                        select C).ToList<Entidades.CabeceraHojasDeRuta>();


        return periodos;
    }

    public static object[] GetPeriodosByContrato(int id, long IdUsuarioLogin)
    {
        EntidadesConosud db = new EntidadesConosud();
        string DescRol = Helpers.RolesEspeciales.Administrador.ToString();

        int RolesAdministrador = (from U in db.SegUsuario
                                  from UR in U.SegUsuarioRol
                                  where U.IdSegUsuario == IdUsuarioLogin
                                  && UR.SegRol.Descripcion == DescRol
                                  select UR).Count();


        DateTime Fecha = DateTime.Now.AddMonths(-1);

        if (RolesAdministrador > 0)
            Fecha = DateTime.Now.AddMonths(-100);




        DateTime FechaHasta = DateTime.Now.AddMonths(-1);

        List<DateTime> periodos = (from C in db.CabeceraHojasDeRuta
                                                        where C.ContratoEmpresas.Contrato.IdContrato == id
                                                        && (C.Periodo >= Fecha && C.Periodo <= FechaHasta
                                                        || (C.Periodo.Month == FechaHasta.Month && C.Periodo.Year == FechaHasta.Year))
                                                        orderby C.Periodo
                                                        select C.Periodo
                                                       ).ToList<DateTime>();



        object[] PeriodosFormateados = (from p in periodos
                   select new 
                   {
                      Periodo =  string.Format("{0:MM/yyyy}",p)
                   }).Distinct().ToArray();


        return PeriodosFormateados;
    }

    public static void GeneracionXmlLegajos(string ruta)
    {
        EntidadesConosud dbLocal = new EntidadesConosud();


        List<Legajos> legajos = (from L in dbLocal.Legajos
                                 select L).ToList<Legajos>();
                                     //.Include("objContEmpLegajos")
                                     //.Include("objContEmpLegajos.CabeceraHojasDeRuta")
                                     //.Include("objContEmpLegajos.ContratoEmpresas")
                                     //.Include("objContEmpLegajos.ContratoEmpresas.Empresa")
                                     //.Include("objContEmpLegajos.ContratoEmpresas.Contrato")

        List<Entidades.Empresa> emps = (from L in dbLocal.Empresa
                              select L).ToList<Entidades.Empresa>();
        List<Entidades.Contrato> conts = (from L in dbLocal.Contrato
                                          select L).ToList<Entidades.Contrato>();
        List<Entidades.ContratoEmpresas> ContratoEmpresas = (from L in dbLocal.ContratoEmpresas
                                                             select L).ToList<Entidades.ContratoEmpresas>();
        List<Entidades.CabeceraHojasDeRuta> CabeceraHojasDeRuta = (from L in dbLocal.CabeceraHojasDeRuta
                                                                   select L).ToList<Entidades.CabeceraHojasDeRuta>();
        List<Entidades.ContEmpLegajos> ContEmpLegajos = (from L in dbLocal.ContEmpLegajos
                                                         select L).ToList<Entidades.ContEmpLegajos>();

        XDocument xDoc = new XDocument(
                new XElement("searchable_index", from pt in legajos select new XElement("item", pt.NroDoc)));


        IEnumerable<XElement> childList = from el in xDoc.Elements().Elements()
                                          select el;

        foreach (XElement item in childList)
        {
            Legajos current = legajos.Where(P => P.NroDoc == item.Value).FirstOrDefault<Legajos>();

            //Legajos  current = (from P in legajos
            //                    where P.NroDoc == item.Value
            //                    select P).FirstOrDefault<Legajos>();

            item.SetValue(current.Apellido + ", " + current.Nombre);

            if (current.objContEmpLegajos.Count > 0)
            {

                ContEmpLegajos ultimo = (from C in current.objContEmpLegajos
                                         where C.FechaTramiteBaja == null
                                         orderby C.CabeceraHojasDeRuta.Periodo
                                         select C).LastOrDefault<ContEmpLegajos>();

                if (ultimo == null)
                {
                    item.Add(new XAttribute("contrato", "Sin Contrato"));
                    item.Add(new XAttribute("UltimoPeriodo", ""));
                }
                else
                {
                    string UltiPeriodoAsignado = ultimo.CabeceraHojasDeRuta.Periodo.Year.ToString() + "/" + string.Format("{0:00}", ultimo.CabeceraHojasDeRuta.Periodo.Month);

                    //ContratoEmpresas currentContEmp = (from C in dbLocal.ContratoEmpresas.Include("Empresa").Include("Contrato")
                    //                                   where C.IdContratoEmpresas == idContEmpresa
                    //                                   select C).First<ContratoEmpresas>();

                    item.Add(new XAttribute("contrato", ultimo.ContratoEmpresas.Contrato.Codigo + " - " + ultimo.ContratoEmpresas.Empresa.RazonSocial));
                    item.Add(new XAttribute("UltimoPeriodo", UltiPeriodoAsignado));
                }
            }
            else
            {
                item.Add(new XAttribute("contrato", "Sin Contrato"));
                item.Add(new XAttribute("UltimoPeriodo",""));
            }

            item.Add(new XAttribute("NroDocumento", current.NroDoc));
            item.Add(new XAttribute("IdLegajo", current.IdLegajos));
            
        }


        xDoc.Save(ruta + @"\Legajos.xml");
    }

    public static DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
    {
        DataTable dtReturn = new DataTable();

        // column names 
        PropertyInfo[] oProps = null;

        if (varlist == null) return dtReturn;

        foreach (T rec in varlist)
        {
            // Use reflection to get property names, to create table, Only first time, others 
            //will follow 
            if (oProps == null)
            {
                oProps = ((Type)rec.GetType()).GetProperties();
                foreach (PropertyInfo pi in oProps)
                {
                    Type colType = pi.PropertyType;

                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }

                    dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                }
            }

            DataRow dr = dtReturn.NewRow();

            foreach (PropertyInfo pi in oProps)
            {
                dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                (rec, null);
            }

            dtReturn.Rows.Add(dr);
        }
        return dtReturn;
    }

    public class ComparerByContratoEmpresa : IEqualityComparer<Entidades.CabeceraHojasDeRuta>
    {
        public bool Equals(Entidades.CabeceraHojasDeRuta x, Entidades.CabeceraHojasDeRuta y)
        {
            return x.ContratoEmpresas.IdContratoEmpresas == y.ContratoEmpresas.IdContratoEmpresas;
        }
        public int GetHashCode(Entidades.CabeceraHojasDeRuta obj)
        {
            return obj.ContratoEmpresas.IdContratoEmpresas.GetHashCode();
        }
    }

    public static Expression<Func<TElement, bool>> NotContainsExpression<TElement, TValue>(Expression<Func<TElement, TValue>> valueSelector, IEnumerable<TValue> values)
    {

        if (null == valueSelector) { throw new ArgumentNullException("valueSelector"); }

        if (null == values) { throw new ArgumentNullException("values"); }

        ParameterExpression p = valueSelector.Parameters.Single();

        if (!values.Any())
        {
            return e => true;
        }

        var equals = values.Select(value => (Expression)Expression.NotEqual(valueSelector.Body, Expression.Constant(value, typeof(TValue))));

        var body = equals.Aggregate<Expression>((accumulate, equal) => Expression.And(accumulate, equal));

        return Expression.Lambda<Func<TElement, bool>>(body, p);

    }

    public static Expression<Func<TElement, bool>> ContainsExpression<TElement, TValue>(Expression<Func<TElement, TValue>> valueSelector, IEnumerable<TValue> values)
    {

        if (null == valueSelector) { throw new ArgumentNullException("valueSelector"); }

        if (null == values) { throw new ArgumentNullException("values"); }

        ParameterExpression p = valueSelector.Parameters.Single();

        if (!values.Any())
        {
            return e => false;
        }

        var equals = values.Select(value => (Expression)Expression.Equal(valueSelector.Body, Expression.Constant(value, typeof(TValue))));

        var body = equals.Aggregate<Expression>((accumulate, equal) => Expression.Or(accumulate, equal));

        return Expression.Lambda<Func<TElement, bool>>(body, p);

    }

    public static Entidades.SegRolMenu GetPermisosAcciones(string NombrePagina, long idUsuario)
    {
        EntidadesConosud dc = new EntidadesConosud();


        var rolesMenu = from U in dc.SegUsuario
                        from S in U.SegUsuarioRol
                        where U.IdSegUsuario == idUsuario
                        select S.SegRol.SegRolMenu.Where(w => w.SegMenu.Url.Contains(NombrePagina)).Distinct();

        /// Inicializo los permisos sobre la página
        /// en verdadero.
        Entidades.SegRolMenu segrolMenuTemp = new Entidades.SegRolMenu();
        segrolMenuTemp.Creacion = true;
        segrolMenuTemp.Modificacion = true;
        segrolMenuTemp.Lectura = true;

        /// Busco en cada uno de los permisos sobre la página
        /// para ver si hay superposición de los mismo, es decir que 
        /// por medio de distitos roles se hayan establecido permisos
        /// sobre una misma página.
        /// Si existe esta situación se aplicada el metodo PESIMISTA, es decir
        /// si para un rol el permiso de creación esta habilitado y en otro rol
        /// el permiso esta denegado, entonces se deja negado el permiso.
        foreach (List<Entidades.SegRolMenu> item in rolesMenu)
        {
            if (item.Count > 0)
            {
                Entidades.SegRolMenu ItemRolMenu = item.First();

                if (ItemRolMenu.Creacion == false)
                    segrolMenuTemp.Creacion = ItemRolMenu.Creacion;

                if (ItemRolMenu.Modificacion == false)
                    segrolMenuTemp.Modificacion = ItemRolMenu.Modificacion;

                if (ItemRolMenu.Lectura == false)
                    segrolMenuTemp.Lectura = ItemRolMenu.Lectura;
            }
        }

        /// Retorno los permisos en un RolMenu temporal
        /// que contiene el resultado de la combinación de lo
        /// rolesMenu.
        return segrolMenuTemp;
    }

    #region Metodos par la Exportacion a excel

    private static List<string> _datosReporte = new List<string>();

    private static void gvLegajos_RowCreated(object sender, GridViewRowEventArgs e)
    {
        /*Create header row above generated header row*/
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //create row    
            GridViewRow row = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
            int TotalColumnas = ((System.Web.UI.WebControls.GridView)(sender)).Columns.Count;
            int colSpan = TotalColumnas - 2 > 6 ? 6 : TotalColumnas - 1;

            #region Columna de Logo
            Image logo = new Image();
            int segmentos = HttpContext.Current.Request.Url.Segments.Count();
            string ruta = HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, HttpContext.Current.Request.Url.AbsoluteUri.IndexOf(HttpContext.Current.Request.Url.Segments[segmentos - 1].ToString()) - 1);
            logo.ImageUrl = ruta + "/images/LogoReportes.png"; ;
            logo.Width = Unit.Pixel(142);
            logo.Height = Unit.Pixel(92);

            TableCell left = new TableHeaderCell();
            left.ColumnSpan = 1;
            left.RowSpan = 4;
            left.Controls.Add(logo);
            left.Style.Add(HtmlTextWriterStyle.BackgroundColor, "White");
            left.Style.Add(HtmlTextWriterStyle.PaddingTop, "10px");
            left.Width = Convert.ToInt32(18 * decimal.Parse("8,46"));
            left.Style.Add("white-space", "nowrap");
            row.Cells.Add(left);
            #endregion

            #region  Columna de TITULO
            left = new TableHeaderCell();
            left.ColumnSpan = colSpan;
            left.Style.Add(HtmlTextWriterStyle.TextAlign, "left");
            left.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C5BE97");
            left.Style.Add(HtmlTextWriterStyle.Color, "Black");
            left.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
            left.Style.Add(HtmlTextWriterStyle.FontFamily, "Calibri");
            left.Style.Add(HtmlTextWriterStyle.FontSize, "11");
            left.Style.Add("white-space", "nowrap");
            left.Text = _datosReporte[0];

            row.Cells.Add(left);

            //Add the new row to the gridview as the master header row
            //A table is the only Control (index[0]) in a GridView
            ((Table)(sender as GridView).Controls[0]).Rows.AddAt(0, row);
            #endregion

            #region  Columna de FECHA
            row = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);

            //spanned cell that will span the columns I don't want to give the additional header 
            left = new TableHeaderCell();
            left.ColumnSpan = colSpan;
            left.Style.Add(HtmlTextWriterStyle.TextAlign, "left");
            left.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C5BE97");
            left.Style.Add(HtmlTextWriterStyle.Color, "Black");
            left.Style.Add(HtmlTextWriterStyle.FontFamily, "Calibri");
            left.Style.Add(HtmlTextWriterStyle.FontSize, "11");
            left.Style.Add(HtmlTextWriterStyle.FontWeight, "Normal");
            left.Style.Add("white-space", "nowrap");
            left.Text = _datosReporte[1];
            row.Cells.Add(left);

            //Add the new row to the gridview as the master header row
            //A table is the only Control (index[0]) in a GridView
            ((Table)(sender as GridView).Controls[0]).Rows.AddAt(1, row);
            #endregion

            #region  Columna Descripcion 1
            row = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);

            //spanned cell that will span the columns I don't want to give the additional header 
            left = new TableHeaderCell();
            left.ColumnSpan = colSpan;
            left.Style.Add(HtmlTextWriterStyle.TextAlign, "left");
            left.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C5BE97");
            left.Style.Add(HtmlTextWriterStyle.Color, "Black");
            left.Style.Add(HtmlTextWriterStyle.FontFamily, "Calibri");
            left.Style.Add(HtmlTextWriterStyle.FontSize, "11");
            left.Style.Add(HtmlTextWriterStyle.FontWeight, "Normal");
            left.Style.Add("white-space", "nowrap");
            left.Text = _datosReporte[2];

            row.Cells.Add(left);

            ((Table)(sender as GridView).Controls[0]).Rows.AddAt(2, row);
            #endregion

            #region  Columna de Descripcion 2

            row = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);

            //spanned cell that will span the columns I don't want to give the additional header 
            left = new TableHeaderCell();
            left.ColumnSpan = colSpan;
            left.Style.Add(HtmlTextWriterStyle.TextAlign, "left");
            left.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C5BE97");
            left.Style.Add(HtmlTextWriterStyle.Color, "Black");
            left.Style.Add(HtmlTextWriterStyle.FontFamily, "Calibri");
            left.Style.Add(HtmlTextWriterStyle.FontSize, "11");
            left.Style.Add(HtmlTextWriterStyle.FontWeight, "Normal");
            left.Style.Add("white-space", "nowrap");
            left.Text = _datosReporte.Count > 3 ? _datosReporte[3] : "";
            row.Cells.Add(left);

            ((Table)(sender as GridView).Controls[0]).Rows.AddAt(3, row);

            #endregion

            #region Columna de Separacion en Blanco

            row = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);

            left = new TableHeaderCell();
            left.ColumnSpan = TotalColumnas;
            left.Style.Add(HtmlTextWriterStyle.TextAlign, "left");
            left.Style.Add(HtmlTextWriterStyle.BackgroundColor, "White");
            left.Style.Add("white-space", "nowrap");
            left.Text = "&nbsp;";
            row.Cells.Add(left);

            ((Table)(sender as GridView).Controls[0]).Rows.AddAt(4, row);

            #endregion


            foreach (TableCell item in e.Row.Cells)
            {
                item.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#C5BE97");
                item.Style.Add(HtmlTextWriterStyle.Color, "Black");
                item.Style.Add(HtmlTextWriterStyle.FontFamily, "Calibri");
                item.Style.Add(HtmlTextWriterStyle.FontSize, "11");
                item.Style.Add(HtmlTextWriterStyle.FontWeight, "Normal");
            }


        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Style.Add(HtmlTextWriterStyle.Color, "Black");
            e.Row.Height = Unit.Pixel(18);
        }

    }


    public static GridView GenerarExportExcel(List<dynamic> datosExportar, Dictionary<string, string> alias, List<string> camposExcluir, List<string> DatosReporte)
    {
        _datosReporte = DatosReporte;
        GridView gv = new GridView();
        gv.RowCreated += new GridViewRowEventHandler(gvLegajos_RowCreated);
        gv.AutoGenerateColumns = false;

        foreach (System.Reflection.PropertyInfo item in datosExportar.First().GetType().GetProperties())
        {
            if (!camposExcluir.Contains(item.Name))
            {
                string alia = alias.ContainsKey(item.Name) ? alias[item.Name] : item.Name;
                BoundField boundField = new BoundField();
                boundField.DataField = item.Name;
                boundField.HeaderText = alia;
                boundField.NullDisplayText = "";
                boundField.ItemStyle.Wrap = false;
                boundField.HeaderStyle.Wrap = false;
                gv.Columns.Add(boundField);
            }

        }

        gv.DataSource = datosExportar;
        gv.DataBind();

        return gv;

    }

    #endregion
}

