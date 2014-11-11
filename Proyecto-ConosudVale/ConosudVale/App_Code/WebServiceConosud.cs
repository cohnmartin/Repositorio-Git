using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services;
using Entidades;

/// <summary>
/// Summary description for WebServiceConosud
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WebServiceConosud : System.Web.Services.WebService {

    public WebServiceConosud () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod(Description = "Web metodo del sistema de Conosud que devuelve los legajos con las credenciales habilitadas.")]
    public List<WSLegajos> GetAllLegajos(string Usuario, string Clave)
    {
        List<WSLegajos> colLeg = null;
        try
        {
            if (Usuario == "Admin" && Clave == "admin123admin456")
            {
                ///grabar una tabla de auditoria de los usuarios que ingresaron.

                EntidadesConosud Contexto = new EntidadesConosud();
                colLeg = (from L in Contexto.Legajos
                          where L.CredVencimiento.HasValue && DateTime.Now < L.CredVencimiento
                          select new WSLegajos
                          {
                              Apellido = L.Apellido,
                              Nombre = L.Nombre,
                              NroDoc = L.NroDoc,
                              PermitidoAcceso = false
                          }).ToList();
            }
            else
            {
                colLeg = new List<WSLegajos>();
                WSLegajos legErr = new WSLegajos();
                legErr.Apellido = "Usuario o Clave Incorrectos";
                legErr.Nombre = "No se puede cargar la informacion";
                colLeg.Add(legErr);
            }

        }
        catch (Exception ex)
        {
            colLeg = new List<WSLegajos>();
            WSLegajos legErr = new WSLegajos();
            legErr.Apellido = "Error al consultar";
            legErr.Nombre = ex.Message;
            colLeg.Add(legErr);
        }

        return colLeg;
    }

 
}
