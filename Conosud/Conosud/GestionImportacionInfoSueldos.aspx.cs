using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Telerik.Web.UI;
using Telerik.Web.UI.Upload;
using System.IO;
using System.Data.OleDb;
using System.ComponentModel;

public partial class GestionImportacionInfoSueldos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Do not display SelectedFilesCount progress indicator.
            RadProgressArea1.ProgressIndicators &= ~ProgressIndicators.SelectedFilesCount;
            CargaGrilla(new EntidadesConosud());
        }
        RadProgressArea1.DisplayCancelButton = true;
        RadProgressArea1.Localization.Uploaded = "Total Progreso";
        RadProgressArea1.Localization.UploadedFiles = "Progreso";
        RadProgressArea1.Localization.TransferSpeed = "Velocidad: ";
        RadProgressArea1.Localization.EstimatedTime = "Tiempo estimado: ";
        RadProgressArea1.Localization.ElapsedTime = "Tiempo de enlace: ";
        RadProgressArea1.Localization.Cancel = "Cancelar: ";
    }

    private void BindResults()
    {
        try
        {
            EntidadesConosud dc = new EntidadesConosud();
            ArchivosSueldos newArchivo = new ArchivosSueldos();
            if (RadUpload1.UploadedFiles.Count > 0)
            {
                #region Lectura y Carga Archivo Excel
                string rutaArchivo = Server.MapPath("ArchivosSueldos") + "/" + RadUpload1.UploadedFiles[0].GetName();
                OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + rutaArchivo + ";Extended Properties=Excel 8.0");
                OleDbCommand command = new OleDbCommand("SELECT * FROM [Info$]", connection); 
                OleDbDataReader dr;
                try
                {

                    connection.Open();
                    dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch
                {

                    connection = new OleDbConnection(@"Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + rutaArchivo + ";Extended Properties=Excel 8.0");
                    command = new OleDbCommand("SELECT * FROM [FEBRERO$]", connection);
                    connection.Open();
                    dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                }

                DataTable excelData = new DataTable("ExcelData");
                excelData.Load(dr);

                var AlllegajosExcel = (from excel in excelData.AsEnumerable()
                                       select new
                                       {
                                           dni = excel.Field<object>("DNI"),
                                           fechaIngeso = excel.Field<DateTime?>("FECHA DE INGRESO"),
                                           Categoria = excel.Field<string>("CATEGORIA SEGÚN CCT"),
                                           Funcion = excel.Field<string>("FUNCION"),
                                           NombreCompleto = excel.Field<string>("APELLIDO Y NOMBRE"),
                                           FechaNacimiento = excel.Field<DateTime?>("FECHA DE NACIMIENTO"),
                                           CUIL = excel.Field<double?>("CUIL"),
                                           EncuadreGremial = excel.Field<string>("ENCUADRE GREMIAL"),

                                           Basico = excel.Field<decimal?>("BASICO/VALOR HORA SEGÚN CCT"),
                                           BasicoLiquidado = excel.Field<decimal?>("BASICO LIQUIDADO"),
                                           HorasExtras = excel.Field<decimal?>("HORAS EXTRAS"),
                                           AdicionalesRemunerativos = excel.Field<decimal?>("ADICIONALES REMUNERATIVOS (SIN HORAS EXTRAS)"),
                                           Vacaciones = excel.Field<decimal?>("VACACIONES"),
                                           SAC = excel.Field<decimal?>("SAC"),
                                           TotalBruto = excel.Field<decimal?>("TOTAL BRUTO"),
                                           AsigFliares = excel.Field<decimal?>("ASIGN FLIARES"),
                                           AdicionalesNoRemunerativos = excel.Field<decimal?>("ADICIONALES NO REMUNERATIVOS"),
                                           Descuentos = excel.Field<decimal?>("DESCUENTOS"),
                                           TotalNeto = excel.Field<decimal?>("TOTAL NETO"),
                                           CUITEmpresa = excel.Field<double?>("CUIT_(EMPRESA)"),
                                           Periodo = excel.Field<DateTime?>("MES DE LIQUIDACIÓN"),
                                           NroContrato = excel.Field<object>("CONTRATO N°"),
                                           RazonSocial = excel.Field<string>("RAZON SOCIAL")
                                       }).ToList();
                #endregion

                #region Valido informacion
                ///Valido que el periodo no exista en la base
                ///
                var claveArchivo = (from L in AlllegajosExcel
                                    where L.Periodo != null
                                    select new { L.Periodo, L.NroContrato, L.CUITEmpresa, L.RazonSocial });

                if (claveArchivo.Count() > 0)
                {
                    int mes = claveArchivo.First().Periodo.Value.Month;
                    int ano = claveArchivo.First().Periodo.Value.Year;
                    string cuitemp = claveArchivo.First().CUITEmpresa.ToString();
                    string nrocon = claveArchivo.First().NroContrato.ToString();

                    Entidades.Empresa emp = (from E in dc.Empresa
                                             where E.CUIT == cuitemp
                                             select E).FirstOrDefault();
                    if (emp == null)
                    {
                        string scriptstring = "alert('La empresa " + claveArchivo.First().RazonSocial + " no existe en la base de datos.');";
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript2", scriptstring, true);
                        return;
                    }


                    if ((from D in dc.DatosDeSueldos
                         where D.objContrato.Codigo == nrocon && D.objEmpresa.CUIT == cuitemp
                         && D.Periodo.Value.Year == ano && D.Periodo.Value.Month == mes
                         select D).Count() > 0)
                    {
                        string scriptstring = "alert('El periodo "
                            + mes + "/" + ano + " del contrato " + nrocon + " correspondiente a " + claveArchivo.First().RazonSocial
                            + " ya existe, debe eliminar el archivo del listado para completar la acción.');";

                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript1", scriptstring, true);
                        return;
                    }
                }
                else
                {
                    string scriptstring = "alert('El archivo no contiene informacion.');";
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript2", scriptstring, true);
                    return;
                }

                #endregion

                #region Actualizacion de info

                List<string> legajosDni = (from L in AlllegajosExcel
                                           where L.dni != null
                                           select L.dni.ToString()).ToList();

                List<Entidades.Legajos> LegajosEncontrados = dc.Legajos.Where(
                      Helpers.ContainsExpression<Entidades.Legajos, string>(l => l.NroDoc, legajosDni)).ToList<Entidades.Legajos>();

                /// Actualizo la información de los legajos encontrados
                foreach (Entidades.Legajos leg in LegajosEncontrados)
                {
                    var LegExcel = (from L in AlllegajosExcel
                                    where L.dni.ToString() == leg.NroDoc
                                    select L).First();

                    //leg.FuncionCCT = LegExcel.Funcion;
                    leg.FechaIngreos = LegExcel.fechaIngeso;
                    //leg.CategoriaCCT = LegExcel.Categoria;
                }

                /// Con la información de los legajos nuevos
                /// doy de alta a los nuevos legajos
                List<string> legajosDniBase = (from L in dc.Legajos
                                               select L.NroDoc).ToList();


                List<string> LegajosDniNevos = (from L in legajosDni
                                                where !legajosDniBase.Contains(L)
                                                select L).ToList();

                foreach (string dni in LegajosDniNevos)
                {
                    var LegExcel = (from L in AlllegajosExcel
                                    where L.dni == dni
                                    select L).First();

                    if (dni != null && dni != "" && dni.Length == 8)
                    {
                        Entidades.Legajos leg = new Entidades.Legajos();
                        //leg.FuncionCCT = LegExcel.Funcion;
                        leg.FechaIngreos = LegExcel.fechaIngeso;
                        //leg.CategoriaCCT = LegExcel.Categoria;
                        leg.NroDoc = dni;

                        if (LegExcel.NombreCompleto.IndexOf(",") > 0)
                        {
                            leg.Apellido = LegExcel.NombreCompleto.Substring(0, LegExcel.NombreCompleto.IndexOf(",")).Trim();
                            leg.Nombre = LegExcel.NombreCompleto.Substring(LegExcel.NombreCompleto.IndexOf(",") + 1).Replace(".", "").Trim();
                        }
                        else
                        {
                            leg.Apellido = LegExcel.NombreCompleto.Substring(0, LegExcel.NombreCompleto.IndexOf(" ")).Trim();
                            leg.Nombre = LegExcel.NombreCompleto.Substring(LegExcel.NombreCompleto.IndexOf(" ") + 1).Replace(".", "").Trim();
                        }

                        leg.FechaNacimiento = LegExcel.FechaNacimiento;

                        if (LegExcel.CUIL != null)
                            leg.CUIL = string.Format("{0:##-########-#}", long.Parse(LegExcel.CUIL.ToString()));

                        leg.objConvenio = (from Cla in dc.Clasificacion
                                           where Cla.Tipo == "Convenio" && Cla.Descripcion.Contains(LegExcel.EncuadreGremial)
                                           select Cla).FirstOrDefault();

                        dc.AddToLegajos(leg);
                    }
                }
                #endregion

                #region Carga registro de Archivo
                long idUsuario = long.Parse(this.Session["idusu"].ToString());
                newArchivo.Nombre = RadUpload1.UploadedFiles[0].GetName();
                newArchivo.SegUsuario = (from U in dc.SegUsuario
                                         where U.IdSegUsuario == idUsuario
                                         select U).First();

                newArchivo.FechaCreacion = DateTime.Now;
                dc.AddToArchivosSueldos(newArchivo);
                #endregion

                #region Generacion Información Sueldo Mensual
                List<Entidades.DatosDeSueldos> dtosSuel = new List<DatosDeSueldos>();
                foreach (var item in AlllegajosExcel)
                {
                    string StringDNI = Convert.ToInt32(item.dni).ToString();

                    Entidades.Legajos leg = (from L in dc.Legajos
                                             where L.NroDoc == StringDNI
                                             select L).FirstOrDefault();

                    string cuitemp = string.Empty;
                    if (item.CUITEmpresa.HasValue) { cuitemp = item.CUITEmpresa.Value.ToString(); }
                    Entidades.Empresa emp = (from E in dc.Empresa
                                             where E.CUIT == cuitemp
                                             select E).FirstOrDefault();

                    string nrocont = string.Empty;
                    if (item.NroContrato != null) { nrocont = item.NroContrato.ToString(); }
                    Entidades.Contrato cont = (from E in dc.Contrato
                                               where E.Codigo == nrocont
                                               select E).FirstOrDefault();

                    if (leg != null && emp != null && cont != null)
                    {
                        Entidades.DatosDeSueldos DatoSueldo = new DatosDeSueldos();
                        DatoSueldo.objLegajo = leg;
                        DatoSueldo.AdicionalesNORemunerativos = item.AdicionalesNoRemunerativos;
                        DatoSueldo.AdicionalesRemunerativos = item.AdicionalesRemunerativos;
                        DatoSueldo.AsignacionFamiliar = item.AsigFliares;
                        DatoSueldo.Basico_ValorHora = item.Basico;
                        DatoSueldo.BasicoLiquidado = item.BasicoLiquidado;
                        DatoSueldo.Descuentos = item.Descuentos;
                        DatoSueldo.HorasExtras = item.HorasExtras;
                        DatoSueldo.SAC = item.SAC;
                        DatoSueldo.TotalBruto = item.TotalBruto;
                        DatoSueldo.TotalNeto = item.TotalNeto;
                        DatoSueldo.Vacaciones = item.Vacaciones;
                        DatoSueldo.Periodo = item.Periodo;
                        DatoSueldo.objEmpresa = emp;
                        DatoSueldo.objContrato = cont;
                        dtosSuel.Add(DatoSueldo);
                    }
                }
                #endregion


                if (dtosSuel.Count > 0)
                {
                    foreach (Entidades.DatosDeSueldos ds in dtosSuel)
                    {
                        newArchivo.colDatosDeSueldos.Add(ds);
                    }

                    dc.SaveChanges();
                    gvArchivos.Rebind();
                }
                else
                {
                    string scriptstring = "alert('No existe informacion de Sueldos en el archivo o no estan cargado ninguno de los contratos o legagos en el sistema');";
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript2", scriptstring, true);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            string script = string.Format("alert('Error al procesar archivo!  {0}');", ex.Message.Replace("'"," "));
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", script, true);
        }
    }

    private void CargaGrilla(EntidadesConosud dc)
    {
        var archivos = from U in dc.ArchivosSueldos
                           .Include("SegUsuario")
                       select U;

        gvArchivos.DataSource = archivos;
        gvArchivos.DataBind();

        //string CadenaPeriodos = "";
        //DateTime?[] periodos = (from D in dc.DatosDeSueldos
        //                        select D.Periodo).Distinct().ToArray();

        //foreach (DateTime? item in periodos)
        //{
        //    if (item.HasValue)
        //    {
        //        CadenaPeriodos += item.Value.Year.ToString() + item.Value.Year.ToString() + "|";
        //    }
        //}

        //gvArchivos.Attributes.Add("Periodos", CadenaPeriodos);
    }

    protected void buttonSubmit_Click(object sender, System.EventArgs e)
    {

        UpdateProgressContext();

        BindResults();
    }

    private void UpdateProgressContext()
    {
        const int total = 100;

        RadProgressContext progress = RadProgressContext.Current;
        progress.Speed = "N/A";

        for (int i = 0; i < total; i++)
        {
            progress.PrimaryTotal = 1;
            progress.PrimaryValue = 1;
            progress.PrimaryPercent = 100;

            progress.SecondaryTotal = total;
            progress.SecondaryValue = i;
            progress.SecondaryPercent = i;

            progress.CurrentOperationText = "Step " + i.ToString();

            if (!Response.IsClientConnected)
            {
                //Cancel button was clicked or the browser was closed, so stop processing
                break;
            }

            progress.TimeEstimated = (total - i) * 100;
            //Stall the current thread for 0.1 seconds
            System.Threading.Thread.Sleep(100);
        }
    }

    protected void gvArchivos_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        EntidadesConosud dc = new EntidadesConosud();

        var archivos = from U in dc.ArchivosSueldos.Include("SegUsuario")
                       select U;

        gvArchivos.DataSource = archivos;

    }
   
    public void imgEliminar_Click(object sender, ImageClickEventArgs e)
    {
        long id = long.Parse(((Telerik.Web.UI.GridDataItem)(((System.Web.UI.WebControls.Image)(sender)).Parent.Parent)).GetDataKeyValue("IdArchivoSueldos").ToString());

        EntidadesConosud dc = new EntidadesConosud();

        Entidades.ArchivosSueldos CurrentArchivoSueldo = (from A in dc.ArchivosSueldos
                                                          where A.IdArchivoSueldos == id
                                                          select A).FirstOrDefault();
        dc.DeleteObject(CurrentArchivoSueldo);
        dc.SaveChanges();
        gvArchivos.Rebind();

        string CadenaPeriodos = "";
        DateTime?[] periodos = (from D in dc.DatosDeSueldos
                               select D.Periodo).Distinct().ToArray();

        foreach (DateTime item in periodos)
        {
            CadenaPeriodos += item.Year.ToString() + item.Year.ToString() + "|";
        }

        gvArchivos.Attributes.Add("Periodos", CadenaPeriodos);
    }
}
