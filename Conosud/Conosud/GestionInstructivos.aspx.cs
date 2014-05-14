using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
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

public partial class GestionInstructivos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Do not display SelectedFilesCount progress indicator.
            RadProgressArea1.ProgressIndicators &= ~ProgressIndicators.SelectedFilesCount;
            CargaGrilla();
        }
        RadProgressArea1.DisplayCancelButton = true;
        RadProgressArea1.Localization.Uploaded = "Total Progreso";
        RadProgressArea1.Localization.UploadedFiles = "Progreso";
        RadProgressArea1.Localization.TransferSpeed = "Velocidad: ";
        RadProgressArea1.Localization.EstimatedTime = "Tiempo estimado: ";
        RadProgressArea1.Localization.ElapsedTime = "Tiempo de enlace: ";
        RadProgressArea1.Localization.Cancel = "Cancelar: ";
    }

    protected void btnAplicar_Click(object sender, System.EventArgs e)
    {
        if (RadUpload1.UploadedFiles.Count > 0)
        {
            UpdateProgressContext();
        }

        BindResults();

        CargaGrilla();
    }

    private void CargaGrilla()
    {

        GridInstructivos.DataSource = (GetData() as IList);
        UpArchivo.Update();

    }

    private void BindResults()
    {

        EntidadesConosud dc = new EntidadesConosud();
        Instructivos newArchivo = null;

        if (idInstructivo.Value == "")
        {
            newArchivo = new Instructivos();
            newArchivo.NombreFisico = RadUpload1.UploadedFiles[0].GetName();
            dc.AddToInstructivos(newArchivo);
        }
        else
        {
            long id = long.Parse(idInstructivo.Value);

            newArchivo = (from i in dc.Instructivos
                          where i.IdInstructivo == id
                          select i).FirstOrDefault();

            if (newArchivo!= null && RadUpload1.UploadedFiles.Count > 0)
                newArchivo.NombreFisico = RadUpload1.UploadedFiles[0].GetName();
        }

        if (newArchivo != null)
        {
            newArchivo.Fecha = DateTime.Now;
            newArchivo.NombreAlias = txtAlias.Text;
            newArchivo.Descripcion = txtDescricpcion.Text;
            dc.SaveChanges();
        }
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

    public static object GetData()
    {

        EntidadesConosud dc = new EntidadesConosud();
        var datos = (from i in dc.Instructivos
                     select i).ToList();

        return datos;
    }

    [WebMethod(EnableSession = true)]
    public static object EliminarRegistro(string Id, string Archivo)
    {
        long id = long.Parse(Id);
        EntidadesConosud dc = new EntidadesConosud();

        Instructivos objEliinar = (from v in dc.Instructivos
                                   where v.IdInstructivo == id
                                   select v).First();

        dc.DeleteObject(objEliinar);
        dc.SaveChanges();


        string rutaArchivo = HttpContext.Current.Server.MapPath("Instructivos") + "/" + Archivo;
        if (System.IO.File.Exists(rutaArchivo))
        {
            System.IO.File.Delete(rutaArchivo);
        }


        return GetData();
    }

}