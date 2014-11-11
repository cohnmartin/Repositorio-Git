<%@ WebHandler Language="C#" Class="LoadLegajos" %>

using System;
using System.Web;
using Entidades;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Web.SessionState;

public class LoadLegajos : IHttpHandler, IRequiresSessionState
{
    public List<MaestroGenerico> DatosFiltrados
    {
        get
        {
            if (HttpContext.Current.Session["DatosLegajosAshx"] != null)

                return (List<MaestroGenerico>)Helper.DeSerializeObject(HttpContext.Current.Session["DatosLegajosAshx"], typeof(List<MaestroGenerico>));
            else
            {
                return null;
            }
        }
        set
        {
            if (value != null)
                HttpContext.Current.Session["DatosLegajosAshx"] = Helper.SerializeObject(value);
            else
                HttpContext.Current.Session["DatosLegajosAshx"] = value;
        }
    }
    
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string textIngresado = context.Request.QueryString["query"];

        List<MaestroGenerico> datosResult = new List<MaestroGenerico>();
        List<MaestroGenerico> datosFiltrados = new List<MaestroGenerico>();


        if (DatosFiltrados == null)
        {
            /// Cargo la variable de session que luego se utilizara 
            /// para la busquede de legajos por documento
            EntidadesConosud dc = new EntidadesConosud();
            datosResult = (from l in dc.Legajos
                           select new MaestroGenerico
                           {
                               DNI = l.NroDoc,
                               Denominacion = l.Apellido + ", " + l.Nombre,
                           }).ToList();

            datosResult = (from l in datosResult
                           select new MaestroGenerico
                           {
                               DNI = l.DNI,
                               Denominacion = Helper.ToCapitalize(l.Denominacion.ToLower())
                           }).ToList();
            
           DatosFiltrados = datosResult;
        }
        else
        {
            datosResult = (List<MaestroGenerico>)DatosFiltrados;
        }


        datosFiltrados = (from m in datosResult
                          where m.DNI.StartsWith(textIngresado)
                          select m).ToList();


        string cadenaDatos = "";
        string cadenaValues = "";
        foreach (MaestroGenerico item in datosFiltrados)
        {
            cadenaDatos += "'" + item.DNI + " - " + item.Denominacion + "',";
            cadenaValues += "'" + item.DNI + "',";
        }

        if (cadenaDatos.Length > 0)
            cadenaDatos = cadenaDatos.Substring(0, cadenaDatos.Length - 1);

        context.Response.Write("{query:'" + textIngresado + "',  suggestions:[" + cadenaDatos + "],  data:[" + cadenaValues + "]}");
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}