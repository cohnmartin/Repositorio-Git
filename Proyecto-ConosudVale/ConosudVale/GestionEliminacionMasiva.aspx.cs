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

public partial class GestionEliminacionMasiva : System.Web.UI.Page
{
    public class InfoEliminacion
    {
        public string NombreLegajo { get; set; }
        public string Dni { get; set; }
        public string Contrato { get; set; }
        public string Empresa { get; set; }
        public long Id { get; set; }
        public long IdLegajo { get; set; }


    }

    public List<InfoEliminacion> DatosProcesados
    {
        get
        {
            if (Session["DatosProcesados"] == null)
                return new List<InfoEliminacion>();
            else
                return (List<InfoEliminacion>)Session["DatosProcesados"];

        }
        set
        {

            Session["DatosProcesados"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Do not display SelectedFilesCount progress indicator.
            RadProgressArea1.ProgressIndicators &= ~ProgressIndicators.SelectedFilesCount;
            gvArchivos.DataSource = new List<InfoEliminacion>();
            DatosProcesados = new List<InfoEliminacion>();
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
            if (RadUpload1.UploadedFiles.Count > 0)
            {
                #region Lectura y Carga Archivo Excel
                string rutaArchivo = Server.MapPath("ArchivosEliminacionLegajos") + "/" + RadUpload1.UploadedFiles[0].GetName();
                OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + rutaArchivo + ";Extended Properties=Excel 8.0");
                OleDbCommand command = new OleDbCommand("SELECT * FROM [Eliminacion$]", connection);
                OleDbDataReader dr = null;
                try
                {

                    connection.Open();
                    dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch
                {

                    string scriptstring = "alert('El arcivo excel no posee una hoja con el nombre 'Eliminacion', por lo que no puede ser procesado');";
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript2", scriptstring, true);
                    return;
                }

                DataTable excelData = new DataTable("ExcelData");
                excelData.Load(dr);

                var AlllegajosExcel = (from excel in excelData.AsEnumerable()
                                       select new
                                       {
                                           dni = excel.Field<object>("DNI"),
                                           NroContrato = excel.Field<object>("CONTRATO")
                                       }).ToList();
                #endregion


                #region Actualizacion de info

                List<string> legajosDni = (from L in AlllegajosExcel
                                           where L.dni != null
                                           select L.dni.ToString()).ToList();

                List<Entidades.Legajos> LegajosEncontrados = dc.Legajos.Where(
                      Helpers.ContainsExpression<Entidades.Legajos, string>(l => l.NroDoc, legajosDni)).ToList<Entidades.Legajos>();

                List<InfoEliminacion> colLegajosEliminar = new List<InfoEliminacion>();
                /// Actualizo la información de los legajos encontrados
                foreach (Entidades.Legajos leg in LegajosEncontrados)
                {
                    if (!leg.objContEmpLegajos.IsLoaded) { leg.objContEmpLegajos.Load(); }

                    foreach (ContEmpLegajos item in leg.objContEmpLegajos)
                    {
                        if (!item.ContratoEmpresasReference.IsLoaded) { item.ContratoEmpresasReference.Load(); }
                        if (!item.ContratoEmpresas.ContratoReference.IsLoaded) { item.ContratoEmpresas.ContratoReference.Load(); }
                        if (!item.ContratoEmpresas.EmpresaReference.IsLoaded) { item.ContratoEmpresas.EmpresaReference.Load(); }

                        if (!colLegajosEliminar.Any(w => w.Contrato == item.ContratoEmpresas.Contrato.Codigo && w.Dni == leg.NroDoc))
                        {
                            InfoEliminacion legEliminar = new InfoEliminacion();
                            legEliminar.NombreLegajo = leg.Apellido + ", " + leg.Nombre;
                            legEliminar.Dni = leg.NroDoc;
                            legEliminar.Contrato = item.ContratoEmpresas.Contrato.Codigo;
                            legEliminar.Empresa = item.ContratoEmpresas.Empresa.RazonSocial;
                            legEliminar.Id = item.IdContEmpLegajos;
                            legEliminar.IdLegajo = leg.IdLegajos;
                            colLegajosEliminar.Add(legEliminar);
                        }
                    }


                }

                #endregion


                if (colLegajosEliminar.Count > 0)
                {
                    DatosProcesados = colLegajosEliminar;
                    gvArchivos.DataSource = colLegajosEliminar;
                    gvArchivos.DataBind();
                }
                else
                {
                    string scriptstring = "alert('No existe informacion en el archivo o los legajos no estan cargados en ninguno contrato');";
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript2", scriptstring, true);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            string script = string.Format("alert('Error al procesar archivo!  {0}');", ex.Message.Replace("'", " "));
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript", script, true);
        }
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

    protected void btnEliminar_Click(object sender, EventArgs e)
    {
        EntidadesConosud dc = new EntidadesConosud();
        if (DatosProcesados.Count > 0)
        {
            foreach (InfoEliminacion item in DatosProcesados.OrderBy(w => w.Empresa))
            {
                List<ContEmpLegajos> ContEmpLegajos = (from C in dc.ContEmpLegajos
                                                       where C.Legajos.IdLegajos == item.IdLegajo
                                                       select C).ToList();

                foreach (ContEmpLegajos legContEliminar in ContEmpLegajos)
                {
                    dc.DeleteObject(legContEliminar);
                }

            }

            dc.SaveChanges();
            gvArchivos.DataSource = new List<InfoEliminacion>();
            gvArchivos.DataBind();

            string scriptstring = "radalert('Los legajo se eliminaron correctamente de las hojas de ruta correspondientes.');";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript2", scriptstring, true);
            upGrilla.Update();
        }
        else
        {
            string scriptstring = "radalert('No hay legajos para eliminar de la hojas de ruta.');";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "myscript2", scriptstring, true);
            upGrilla.Update();
        }

    }
}
