using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Entidades;

/// <summary>
/// Summary description for AndroidSOAP
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class AndroidSOAP : System.Web.Services.WebService
{

    public AndroidSOAP()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hola Diego: Enviado desde Servidor Conosud";
    }

    [WebMethod]
    public string GrabarUbicacion(double Latitud, double Longitud, string Fecha)
    {
        try
        {
            using (EntidadesConosud dc = new EntidadesConosud())
            {
                Posicionamiento pos = new Posicionamiento();
                pos.Fecha = DateTime.Parse(Fecha);
                pos.Latitud = Latitud;
                pos.Longitud = Longitud;
                dc.AddToPosicionamiento(pos);

                dc.SaveChanges();

                return "Ok";
            }
        }
        catch
        {
            return "";
        }


    }

    [WebMethod]
    public List<Posicionamiento> ObtenrUbicacion(DateTime Fecha)
    {
        try
        {
            using (EntidadesConosud dc = new EntidadesConosud())
            {
                return (from p in dc.Posicionamiento
                        where p.Fecha == Fecha
                        select p).ToList();
            }
        }
        catch
        {
            return null;
        }


    }

}
