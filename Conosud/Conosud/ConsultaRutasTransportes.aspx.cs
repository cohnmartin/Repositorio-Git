using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Telerik.Web.UI;
using System.Xml.Linq;
using System.Xml;

public partial class ConsultaRutasTransportes : System.Web.UI.Page
{
    private static Double deg2rad(Double deg)
    {
        return (deg * Math.PI / 180.0);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }


    [WebMethod(EnableSession = true)]
    public static object CargarKML()
    {

        using (EntidadesConosud dc = new EntidadesConosud())
        {
            CabeceraRutasTransportes cab = new CabeceraRutasTransportes();
            cab.Empresa = "ANDESMAR S.A.";
            cab.HorariosSalida = "06.15  – 18.15 hs ";
            cab.HorariosLlegada = ": 7.15 – 19,15 hs";
            cab.TipoUnidad = "Minibus";
            cab.Turno = "1 y 2 (TURNO)";
            cab.Linea = "LINEA N° 4 (LUZURIAGA - MAIPU CENTRO - PERDRIEL)";



            string docName = @"C:\Desarrollo\Repositorio\Infolegacy\Conosud\Analisis\Transportes\LINEA 4 TURNO.xml";
            Dictionary<string, object> datos = new Dictionary<string, object>();
            XmlDocument doc = new XmlDocument();
            doc.Load(docName);
            var listaCoordenadas = doc.ChildNodes[1].ChildNodes[0].ChildNodes[11].ChildNodes[4].ChildNodes[2].ChildNodes;
            int contador = 0;
            List<object> dd = new List<object>();
            foreach (var item in listaCoordenadas)
            {
                int r;
                if ((item as XmlElement).LocalName == "coord")
                {
                    Math.DivRem(contador, 1, out r);
                    if (r == 0)
                    {
                        RutasTransportes ruta = new RutasTransportes();
                        ruta.Departamento = "MAIPU";
                        ruta.Latitud = (item as XmlElement).InnerXml.Split(' ')[1].ToString().Replace(".", ",");
                        ruta.Longitud = (item as XmlElement).InnerXml.Split(' ')[0].ToString().Replace(".", ",");
                        ruta.objCabecera = cab;
                        //dd.Add(new object[] { (item as XmlElement).InnerXml.Split(' ')[0], (item as XmlElement).InnerXml.Split(' ')[1] });
                    }
                    contador++;

                }
            }

            dc.AddToCabeceraRutasTransportes(cab);
            dc.SaveChanges();
        }


        //string docName = @"C:\Desarrollo\Repositorio\Infolegacy\Conosud\Analisis\Transportes\LINEA 4 TURNO.xml";
        //Dictionary<string, object> datos = new Dictionary<string, object>();
        //XmlDocument doc = new XmlDocument();
        //doc.Load(docName);
        //var listaCoordenadas = doc.ChildNodes[1].ChildNodes[0].ChildNodes[11].ChildNodes[4].ChildNodes[2].ChildNodes;
        //int contador = 0;
        //List<object> dd = new List<object>();
        //foreach (var item in listaCoordenadas)
        //{
        //    int r;
        //    if ((item as XmlElement).LocalName == "coord")
        //    {
        //        Math.DivRem(contador,1, out r);
        //        if (r == 0)
        //        {
        //            dd.Add(new object[] { (item as XmlElement).InnerXml.Split(' ')[0], (item as XmlElement).InnerXml.Split(' ')[1] });

        //            //List<string> valores = new List<string>();
        //            //valores.Add((item as XmlElement).InnerXml.Split(' ')[0]);
        //            //valores.Add((item as XmlElement).InnerXml.Split(' ')[1]);
        //            //datos.Add(contador.ToString(), valores);

        //        }
        //        contador++;

        //    }
        //}

        return null;
        //return datos.Take(1000).ToList();

        
        

    }

    [WebMethod(EnableSession = true)]
    public static object BuscarLineaTransporte(Double lat, Double lon)
    {
        long earthRadius = 6371000;
        Double latFrom = deg2rad(lat);
        Double lonFrom = deg2rad(lon);
        Dictionary<string, object> valores = new Dictionary<string, object>();


        using (EntidadesConosud dc = new EntidadesConosud())
        {
            Double masCercano = 10000;
            CabeceraRutasTransportes cabecera = null;
            long idPunto = 0;

            
            var puntos = (from p in dc.RutasTransportes
                          select p).ToList();


            foreach (var item in puntos)
            {
                Double latTo = deg2rad(Convert.ToDouble(item.Latitud));
                Double lonTo = deg2rad(Convert.ToDouble(item.Longitud));

                Double latDelta = latTo - latFrom;
                Double lonDelta = lonTo - lonFrom;

                Double angle = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(latDelta / 2), 2) + Math.Cos(latFrom) * Math.Cos(latTo) * Math.Pow(Math.Sin(lonDelta / 2), 2)));
                Double distancia = (angle * earthRadius) / 1000;
                if (distancia < masCercano)
                {
                    masCercano = distancia;
                    cabecera = item.objCabecera;
                    idPunto = item.Id;
                }


            }

            /// 1. Deberia devolver las lista de puntos del recorrido mar cercano 
            /// 2. y los 15 puntos al rededor del punto mas cercano para RE calcular segun la api el punto mas cercano

            //cabecera= 3;
            ///1.
            var datos = (from p in puntos.Where(w => w.Cabecera == cabecera.Id).ToList()
                         select new
                         {
                             Key = Convert.ToDouble(p.Latitud),
                             Value = Convert.ToDouble(p.Longitud)
                         }).ToList();

            ///2.
            var ptos = (from p in puntos.Where(w => w.Cabecera == cabecera.Id && w.Id > idPunto - 2 && w.Id < idPunto + 2).ToList()
                        select new
                        {
                            Key = Convert.ToDouble(p.Latitud),
                            Value = Convert.ToDouble(p.Longitud)
                        }).ToList();


                 
            
            valores.Add("Ruta", datos.ToList());
            valores.Add("PuntosCercanos", ptos.ToList());
            valores.Add("Linea", cabecera.Linea);
            valores.Add("Empresa", cabecera.Empresa);
            valores.Add("Turno", cabecera.Turno);
            valores.Add("Horarios", cabecera.HorariosSalida + " - " + cabecera.HorariosLlegada);
            valores.Add("TipoUnidad", cabecera.TipoUnidad);

            return valores;

        }
    }

}