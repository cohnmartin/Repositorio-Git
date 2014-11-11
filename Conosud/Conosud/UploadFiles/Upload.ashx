<%@ WebHandler Language="C#" Class="Upload" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.IO;
using System.Data.OleDb;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Xml.XPath;

public class Upload : IHttpHandler, IRequiresSessionState
{
    public class tempKml
    {
        public string Latitud { get; set; }
        public string Longitud { get; set; }
    }

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        context.Response.Expires = -1;
        try
        {
            HttpPostedFile postedFile = context.Request.Files["Filedata"];

            string savepath = "";
            string tempPath = context.Request.QueryString["folder"];

            //tempPath = System.Configuration.ConfigurationManager.AppSettings["FolderPath"];
            //savepath = context.Server.MapPath(tempPath);

            savepath = context.Server.MapPath(context.Request.QueryString["folder"]);

            string filename = postedFile.FileName;
            if (!Directory.Exists(savepath))
                Directory.CreateDirectory(savepath);


            postedFile.SaveAs(savepath + @"\" + filename);

            // Load the Image
            string ancho = ""; string alto = "";


            /// Logica aplicable solamente a los archivos de despacho
            if (filename.Contains(".kml"))
            {
                string docName = savepath + @"\" + filename;
                Dictionary<string, object> datos = new Dictionary<string, object>();
                string cadenaDatos = "";
                List<tempKml> kml = new List<tempKml>();



                //String data = "";
                //using (StreamReader reader = new StreamReader(docName))
                //{
                //    while (reader.Peek() != -1)
                //    {
                //        data += reader.ReadLine().Replace("&", "&amp;");
                //    }
                //}

                //XDocument xDoc = XDocument.Parse(data);


                //string InputFile = @"D:\Descargas-Web\ARCHIVOS KML\DIURNO\DIURNO Linea 7.kml";
                XDocument xDoc = XDocument.Load(docName);
                //string corr = "";
                var ns = new XmlNamespaceManager(new NameTable());
                ns.AddNamespace("kml", "http://www.opengis.net/kml/2.2");
                var namesa = xDoc.XPathSelectElements("//kml:Placemark/kml:LineString/kml:coordinates", ns);
                foreach (var item in namesa.First().Value.Replace("\t", "").Replace("\n", "").Split(' '))
                {
                    var aa = item.Split(',');
                    if (aa.Length > 1)
                        cadenaDatos += aa[1] + "," + aa[0] + "@";
                }
                
                
                //XNamespace ns = "http://www.opengis.net/kml/2.2";
                //XNamespace gx = "http://www.google.com/kml/ext/2.2";



                //var placemarksaaa = xDoc.Descendants(ns + "Placemark")
                //            .Where(p => p.Element(ns + "name").Value == "Path") 
                //           .Select(p => new
                //           {
                //               Name = p.Element(ns + "name").Value,
                //               point = p.Element(ns + "LineString").Element(ns + "coordinates").Value.Replace(",", "@").Replace("\t", "")
                //           }).FirstOrDefault();

                //if (placemarksaaa.point.Length > 0)
                //{
                //    cadenaDatos = placemarksaaa.point.Replace(" ", "");
                //    context.Response.Write(cadenaDatos);
                //    context.Response.StatusCode = 200;
                //    HttpContext.Current.Session.Add("Datoskml", cadenaDatos);
                //    return;
                //}
                
                
                //var placemarks = xDoc.Descendants(ns + "Placemark").Where(p => p.Attribute("id") != null)
                //            .Select(p => new
                //            {
                //                Name = p.Element(ns + "name").Value,
                //                id = p.Attribute("id").Value,
                //                point = p.Element(gx + "MultiTrack").Element(gx + "Track").Elements(gx + "coord").ToList()
                //            }).FirstOrDefault();

                //if (placemarks != null)
                //{
                //    foreach (System.Xml.Linq.XElement item in placemarks.point)
                //    {

                //        cadenaDatos += item.Value.Split(' ')[1].ToString() + "," + item.Value.Split(' ')[0].ToString() + "@";

                //    }
                //}

                //else
                //{

                //   ns = "http://earth.google.com/kml/2.0";
                //   ns = "http://www.opengis.net/kml/2.2";

                //   var placemarkBis = xDoc.Descendants(ns + "Placemark").Where(p => p.Element(ns + "name").Value != "Path")
                //    .Select(p => new
                //    {
                //        Name = p.Element(ns + "name").Value,
                //        lon=p.Element(ns + "LookAt").Element(ns + "longitude").Value,
                //        lat = p.Element(ns + "LookAt").Element(ns + "latitude").Value
                //    }).ToList();


                //    foreach (var item in placemarkBis)
                //    {

                //        cadenaDatos += item.lat.ToString() + "," + item.lon.ToString() + "@";

                //    }
                //}
                
                
                


                
               
                
                //try
                //{
                //    //var listaCoordenadas = doc.ChildNodes[1].ChildNodes[0].ChildNodes[11].ChildNodes[4].ChildNodes[2].ChildNodes;
                //    List<tempKml> kml = new List<tempKml>();
                //    foreach (System.Xml.Linq.XElement item in placemarks.point)
                //    {

                //        cadenaDatos += item.Value.Split(' ')[1].ToString() + "," + item.Value.Split(' ')[0].ToString() + "@";

                //    }

                //    //foreach (var item in placemarks.point)
                //    //{
                //    //    int r;
                //    //    if ((item as XmlElement).LocalName == "coord")
                //    //    {
                //    //        Math.DivRem(contador, 1, out r);
                //    //        if (r == 0)
                //    //        {
                //    //            cadenaDatos += (item as XmlElement).InnerXml.Split(' ')[1].ToString() + "," + (item as XmlElement).InnerXml.Split(' ')[0].ToString() + "@";

                //    //        }
                //    //        contador++;

                //    //    }
                //    //}

                //}
                //catch
                //{
                //    //var listaCoordenadas = doc.ChildNodes[1].ChildNodes[0].ChildNodes;
                //    //List<tempKml> kml = new List<tempKml>();


                //    //foreach (XmlNode item in listaCoordenadas)
                //    //{
                //    //    cadenaDatos += item.LastChild.InnerText.Split(',')[1].ToString() + "," + item.LastChild.InnerText.Split(',')[0].ToString() + "@";
                //    //}

                //}


                context.Response.Write(cadenaDatos);
                context.Response.StatusCode = 200;
                HttpContext.Current.Session.Add("Datoskml", cadenaDatos);

            }
            else
            {
                context.Response.Write(tempPath + "/" + filename + "|" + ancho + "|" + alto);
                context.Response.StatusCode = 200;
            }


        }
        catch (Exception ex)
        {
            context.Response.Write("Error: " + ex.Message);
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}