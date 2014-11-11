using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Telerik.Web.UI;
using Telerik.Web.UI.Upload;
using Entidades;
using Telerik.Web.UI;
using System.Xml.Linq;
using System.Xml;


public partial class GestionAltaRecorridos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //context.Session.Add("Datoskml
    }


    [WebMethod(EnableSession = true)]
    public static object GuardarAlta(string Empresa, string HorarioS,string HorarioL, string TipoUnidad, string Turno , string Linea, string TIpoRecorrido , string TipoTurno)
    {

        using (EntidadesConosud dc = new EntidadesConosud())
        {
            CabeceraRutasTransportes cab = new CabeceraRutasTransportes();
            cab.Empresa = Empresa;
            cab.HorariosSalida = HorarioS;
            cab.HorariosLlegada = HorarioL;
            cab.TipoUnidad = TipoUnidad;
            cab.Turno = Turno;
            cab.Linea = Linea;
            cab.TipoRecorrido = TIpoRecorrido;
            cab.TipoTurno = TipoTurno;
          

            foreach (var item in HttpContext.Current.Session["Datoskml"].ToString().Split('@'))
            {
                if (item != "")
                {
                    RutasTransportes ruta = new RutasTransportes();
                    ruta.Departamento = "MAIPU";
                    ruta.Latitud = item.Split(',')[0].ToString().Replace(".", ",");
                    ruta.Longitud = item.Split(',')[1].ToString().Replace(".", ",");
                    ruta.objCabecera = cab;
                }

            }

            dc.AddToCabeceraRutasTransportes(cab);
            dc.SaveChanges();
        }

        return null;

    }
}