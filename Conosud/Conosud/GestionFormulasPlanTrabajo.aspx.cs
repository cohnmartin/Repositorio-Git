using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.SessionState;
using System.Web.Script.Services;
using Entidades;

public partial class GestionFormulasPlanTrabajo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetDatos()
    {
        using (Entidades.EntidadesConosud dc = new EntidadesConosud())
        {
            var itemPlantilla = (from d in dc.Plantilla
                                 orderby d.Codigo
                                 select new
                                 {
                                     d.Codigo,
                                     d.Descripcion,
                                     FornulaPlanTrabajo  = d.FornulaPlanTrabajo == null ? "":d.FornulaPlanTrabajo 

                                 }).ToList();

            return itemPlantilla;
        }
    }
}