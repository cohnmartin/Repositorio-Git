using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;
using Telerik.Web.UI;
using Entidades;
using System.Linq;
using System.Data.Linq;

public partial class CambioDeEncuadre : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            long idLeg = long.Parse(Request.QueryString["IdLegajo"]);
            EntidadesConosud dc = new EntidadesConosud();
            
            Entidades.Clasificacion convenio = (from L in dc.Legajos
                                      where L.IdLegajos == idLeg
                                      select L.objConvenio).FirstOrDefault();

            if (convenio != null)
            {
                lblEncuadreActual.Text = convenio.Descripcion;
            }
            else
            {
                lblEncuadreActual.Text = "F/Convenio";
            }

        }



    }
    protected void btnCambiar_Click(object sender, EventArgs e)
    {
        if (cboEncuadre.SelectedValue != "0")
        {
            long idLeg = long.Parse(Request.QueryString["IdLegajo"]);
            long idConv = long.Parse(cboEncuadre.SelectedValue);


            EntidadesConosud dc = new EntidadesConosud();
            Entidades.Legajos CurrentLegajo = (from L in dc.Legajos
                                               where L.IdLegajos == idLeg
                                               select L).First();


            CurrentLegajo.objConvenio = (from c in dc.Clasificacion
                                         where c.IdClasificacion == idConv
                                         select c).First();

            dc.SaveChanges();
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "ocultar", "CloseWindows();", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "ocultarss", "alert('Debe seleccionar un encuadre')", true);
        }
    }
}
