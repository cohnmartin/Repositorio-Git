using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class AprobacionHojaSinLegajos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnAprobar.Attributes.Add("onmouseover", "this.src='images/HojaAprobadaUp.gif'");
        btnAprobar.Attributes.Add("onmouseout", "this.src='images/HojaAprobada4.gif'");
    }
    protected void cboPriodosAjax_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cboPriodosAjax.SelectedItem.Text != "")
        {
            lblContratista.Text = cboContratistasAjax.SelectedItem.Text;
            lblContrato.Text = cboContratosAjax.SelectedItem.Text;
            lblPeriodo.Text = cboPriodosAjax.SelectedItem.Text;
            lblFechaAprobacion.Text = System.DateTime.Now.ToShortDateString();
        }
    }
    protected void btnAprobar_Click(object sender, ImageClickEventArgs e)
    {
        if (lblFechaAprobacion.Text != "")
        {
            DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter DACabecera = new DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter();
            DSConosud.CabeceraHojasDeRutaDataTable DTCabecera = DACabecera.GetById(Convert.ToInt32(cboPriodosAjax.SelectedItem.Value));

            ((DSConosud.CabeceraHojasDeRutaRow)DTCabecera.Rows[0]).FechaAprobacionSinLegajos = System.DateTime.Now.Date;
            ((DSConosud.CabeceraHojasDeRutaRow)DTCabecera.Rows[0]).IdEstado = (long)Helpers.EstadosHoja.Aprobada;

            DACabecera.Update(DTCabecera.Rows[0]);

            lblContratista.Text = "";
            lblContrato.Text = "";
            lblPeriodo.Text = "";
            lblFechaAprobacion.Text = "";

            string alert = "alert('La hoja de ruta fue aprobada con existo'); window.document.forms[0].submit();";
            System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", alert, true);
        }
        else
        {
            string alert = "alert('Debe seleccionar un periodo para aprobar')";
            System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", alert, true);

        }




    }
}
