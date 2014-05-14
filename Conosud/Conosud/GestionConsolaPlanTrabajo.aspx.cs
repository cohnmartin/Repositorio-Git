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
using System.Web.Services;
using System.Web.SessionState;
using System.Web.Script.Services;

public partial class GestionConsolaPlanTrabajo : System.Web.UI.Page
{
    private class InfoCalculo
    {
        public string Contrato{get;set;}
        public string Empresa{get;set;}
        public string UOCRA{get;set;}
        public string OTROS{get;set;}
        public string INDEPENDIENTES{get;set;}
        public string Resultado{get;set;}

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Do not display SelectedFilesCount progress indicator.
            RadProgressArea1.ProgressIndicators &= ~ProgressIndicators.SelectedFilesCount;

            for (int i = 0; i < 12; i++)
            {
                string fecha = string.Format("{0:MM/yyyy}", DateTime.Now.AddMonths(-1 * i));
                cboPeriodos.Items.Add(new Telerik.Web.UI.RadComboBoxItem(fecha, fecha));
            }
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
        StreamWriter _sw = null;
        _sw = new StreamWriter(Server.MapPath("") + "\\logInfoPlanes.txt", true);

        try
        {

            ArchivosSueldos newArchivo = new ArchivosSueldos();
            if (RadUpload1.UploadedFiles.Count > 0)
            {
                #region Lectura y Carga Archivo Excel
                string rutaArchivo = Server.MapPath("ArchivosPlanTrabajo") + "/" + RadUpload1.UploadedFiles[0].GetName();
                OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + rutaArchivo + ";Extended Properties=Excel 12.0;HDR=YES;");
                OleDbCommand command = new OleDbCommand("SELECT * FROM [Info$]", connection);
                OleDbDataReader dr;
                try
                {

                    connection.Open();
                    dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch
                {

                    connection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + rutaArchivo + ";Extended Properties=\"Excel 12.0;HDR=YES;\"");
                    command = new OleDbCommand("SELECT * FROM [Info$]", connection);
                    connection.Open();
                    dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                }

                DataTable excelData = new DataTable("ExcelData");
                excelData.Load(dr);

                var AlllegajosExcel = (from excel in excelData.AsEnumerable()
                                       select new
                                       {
                                           Contrato = excel.Field<object>("Contrato"),
                                           Contratista = excel.Field<object>("Contratista"),
                                           Sindicato = excel.Field<object>("Sindicato"),
                                           Aplicacion = excel.Field<object>("Aplicacion") == null ? 0 : Convert.ToDecimal(excel.Field<object>("Aplicacion"))
                                       }).ToList();

                //_sw.WriteLine("Total Registros:" + AlllegajosExcel.Count);
                //_sw.WriteLine("Total Contratos:" + AlllegajosExcel.Where(w => w.Contrato.ToString().Contains("UOCRA")).Count());
                //_sw.WriteLine("Total Sindicato:" + AlllegajosExcel.Where(w => w.Sindicato.ToString().Contains("UOCRA")).Count());

                var oucra = (from o in AlllegajosExcel
                             where o.Sindicato.ToString().Contains("UOCRA")
                             group o by new { o.Contrato, o.Contratista } into g
                             select new
                             {
                                 Total = g.Sum(w => w.Aplicacion),
                                 Contrato = g.Key.Contrato.ToString(),
                                 Contratista = g.Key.Contratista

                             }).ToList();

                var independientes = (from o in AlllegajosExcel
                                      where o.Sindicato.ToString().ToLower().Contains("(sin informacion)")
                                      group o by new { o.Contrato, o.Contratista } into g
                                      select new
                                      {
                                          Total = g.Sum(w => w.Aplicacion),
                                          Contrato = g.Key.Contrato.ToString(),
                                          Contratista = g.Key.Contratista

                                      }).ToList();

                var otros = (from o in AlllegajosExcel
                             where !o.Sindicato.ToString().Contains("UOCRA") && !o.Sindicato.ToString().ToLower().Contains("(sin informacion)")
                             group o by new { o.Contrato, o.Contratista } into g
                             select new
                             {
                                 Total = g.Sum(w => w.Aplicacion),
                                 Contrato = g.Key.Contrato.ToString(),
                                 Contratista = g.Key.Contratista

                             }).ToList();

                var contratosEmpresas = (from o in AlllegajosExcel

                                         select new
                                         {
                                             Contrato = o.Contrato.ToString(),
                                             Contratista = o.Contratista

                                         }).Distinct().ToList();




                //decimal UOCRA = Math.Round(AlllegajosExcel.Where(w => w.Sindicato.ToString().Contains("UOCRA")).Sum(w => w.Aplicacion));
                //decimal Independientes = Math.Round(AlllegajosExcel.Where(w => w.Sindicato.ToString().ToLower().Contains("(sin informacion)")).Sum(w => w.Aplicacion));
                //decimal Otros = Math.Round(AlllegajosExcel.Where(w => !w.Sindicato.ToString().Contains("UOCRA") && !w.Sindicato.ToString().ToLower().Contains("(sin informacion)")).Sum(w => w.Aplicacion));

                #endregion

                #region Actualizacion de info del plan de trabajo

                using (EntidadesConosud dc = new EntidadesConosud())
                {
                    List<InformacionPlanTrabajo> currents = (from i in dc.InformacionPlanTrabajo
                                                             where i.Periodo == cboPeriodos.SelectedValue
                                                             select i).ToList();

                    if (currents.Count > 0)
                    {
                        foreach (var item in currents)
                        {
                            dc.InformacionPlanTrabajo.DeleteObject(item);
                        }
                    }


                    foreach (var item in contratosEmpresas)
                    {
                        var T_UOCRA = oucra.Where(w => w.Contratista.ToString().Trim() == item.Contratista.ToString().Trim() && w.Contrato.ToString() == item.Contrato).FirstOrDefault();
                        var T_INDEP = independientes.Where(w => w.Contratista.ToString().Trim() == item.Contratista.ToString().Trim() && w.Contrato.ToString() == item.Contrato).FirstOrDefault();
                        var T_OTROS = otros.Where(w => w.Contratista.ToString().Trim() == item.Contratista.ToString().Trim() && w.Contrato.ToString() == item.Contrato).FirstOrDefault();

                        InformacionPlanTrabajo current = new InformacionPlanTrabajo();
                        current.Periodo = cboPeriodos.SelectedValue;
                        current.INDEPENDIENTES = T_INDEP != null ? Convert.ToInt32(T_INDEP.Total) : 0;
                        current.UOCRA = T_UOCRA != null ? Convert.ToInt32(T_UOCRA.Total) : 0;
                        current.OTROS = T_OTROS != null ? Convert.ToInt32(T_OTROS.Total) : 0;
                        current.Empresa = item.Contratista.ToString();
                        current.Contrato = item.Contrato;

                        dc.AddToInformacionPlanTrabajo(current);
                    }

                    dc.SaveChanges();

                    //if (current == null)
                    //{
                    //    current = new InformacionPlanTrabajo();
                    //    current.Periodo = cboPeriodos.SelectedValue;
                    //    current.INDEPENDIENTES = Convert.ToInt32(Independientes);
                    //    current.UOCRA = Convert.ToInt32(UOCRA);
                    //    current.OTROS = Convert.ToInt32(Otros);

                    //    dc.AddToInformacionPlanTrabajo(current);
                    //}
                    //else
                    //{
                    //    current.INDEPENDIENTES = Convert.ToInt32(Independientes);
                    //    current.UOCRA = Convert.ToInt32(UOCRA);
                    //    current.OTROS = Convert.ToInt32(Otros);
                    //}

                    //dc.SaveChanges();
                }

                #endregion

            }
        }
        catch (Exception ex)
        {
            _sw.WriteLine(ex.Message.ToString());
            _sw.WriteLine(ex.StackTrace);
            if (ex.InnerException != null)
            {
                _sw.WriteLine(ex.InnerException.Message.ToString());
            }

            string script = string.Format("alert('Error al procesar archivo!  {0}');", ex.Message.Replace("'", " "));
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", script, true);
        }

        _sw.Close();
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

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> datos = ( Dictionary<string, object>)GetDatosCalculo(hdf_Periodo.Value);

        List<dynamic> datosExportar = (datos["DatosCalculo"] as List<InfoCalculo>)
            .Select(w => w)
            .ToList<dynamic>();



        List<string> camposExcluir = new List<string>(); ;
        Dictionary<string, string> alias = new Dictionary<string, string>();



        List<string> DatosReporte = new List<string>();
        DatosReporte.Add("Planilla de Plan de Trabajo");
        DatosReporte.Add("Fecha y Hora emisi&oacute;n:" + DateTime.Now.ToString());
        DatosReporte.Add("Per&iacute;odo Calculado: " + hdf_Periodo.Value);
        DatosReporte.Add("Incluye todos los contratos que fueron importados para el Per&iacute;odo");


        GridView gv = Helpers.GenerarExportExcel(datosExportar.ToList<dynamic>(), alias, camposExcluir, DatosReporte);

        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        gv.RenderControl(htmlWrite);

        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=PlanTrabajo" + "_" + DateTime.Now.ToString("M_dd_yyyy_H_M_s") + ".xls");
        HttpContext.Current.Response.ContentType = "application/xls";
        HttpContext.Current.Response.Write(stringWrite.ToString());
        HttpContext.Current.Response.End();
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetDatos()
    {
        Dictionary<string, object> resultado = new Dictionary<string, object>();

        using (Entidades.EntidadesConosud dc = new EntidadesConosud())
        {
            var InformacionPlanTrabajo = (from d in dc.InformacionPlanTrabajo
                                          orderby d.Periodo
                                          group d by new {  d.Periodo } into g
                                          select new
                                          {
                                              Periodo = g.Key.Periodo,
                                              UOCRA = g.Sum(w => w.UOCRA),
                                              OTROS = g.Sum(w => w.OTROS),
                                              INDEPENDIENTES = g.Sum(w => w.INDEPENDIENTES)
                                          }).ToList();

            var Plantilla = (from d in dc.Plantilla
                             orderby d.Codigo
                             select new
                             {
                                 d.Codigo,
                                 d.Descripcion,
                                 FornulaPlanTrabajo = d.FornulaPlanTrabajo == null ? "" : d.FornulaPlanTrabajo

                             }).ToList();

            resultado.Add("Datos", InformacionPlanTrabajo);
            resultado.Add("Formulas", Plantilla);

            return resultado;
        }
    }


    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetDatosCalculo(string Periodo)
    {
        Dictionary<string, object> resultado = new Dictionary<string, object>();
        List<InfoCalculo> infoCalculos = new List<InfoCalculo>();

        using (Entidades.EntidadesConosud dc = new EntidadesConosud())
        {
            var InformacionPlanTrabajo = (from d in dc.InformacionPlanTrabajo
                                          where d.Periodo == Periodo
                                          select d).ToList();


            var Plantilla = (from d in dc.Plantilla
                             orderby d.Codigo
                             select new
                             {
                                 d.Codigo,
                                 d.Descripcion,
                                 FornulaPlanTrabajo = d.FornulaPlanTrabajo == null ? "" : d.FornulaPlanTrabajo

                             }).ToList();

            foreach (var hoja in InformacionPlanTrabajo)
            {
                InfoCalculo info = new InfoCalculo();
                decimal valorCalculado = 0;
                foreach (var pla in Plantilla)
                {

                    string formula = pla.FornulaPlanTrabajo;
                    formula = formula.Replace("A", hoja.UOCRA.ToString());
                    formula = formula.Replace("B", hoja.OTROS.ToString());
                    formula = formula.Replace("C", hoja.INDEPENDIENTES.ToString());
                    formula = formula.Replace("D", (hoja.UOCRA + hoja.OTROS+hoja.INDEPENDIENTES).ToString());
                    valorCalculado += decimal.Parse(ExpressionEvaluator.Eval(formula).Replace(".",","));
                }

                info.UOCRA = hoja.UOCRA.ToString();
                info.OTROS = hoja.OTROS.ToString();
                info.INDEPENDIENTES = hoja.INDEPENDIENTES.ToString();
                info.Empresa = hoja.Empresa.ToString();
                info.Contrato = hoja.Contrato.ToString();
                info.Resultado = string.Format("{0:0.0}", valorCalculado/60);

                infoCalculos.Add(info);
            }

            resultado.Add("DatosCalculo", infoCalculos);
            

            return resultado;
        }
    }
}