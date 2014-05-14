<%@ WebHandler Language="C#" Class="BusquedaDatosIngresosEventuales" %>

using System;
using System.Web;
using Entidades;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Web.SessionState;

public class BusquedaDatosIngresosEventuales : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        var textIngresado = "";
        var TipoBusqueda = context.Request.QueryString["Tipo"];
        var clearSession = false;

        if (context.Request.QueryString["clearSession"] != null)
            clearSession = bool.Parse(context.Request.QueryString["clearSession"]);

        List<MaestroGenerico> datosResult = new List<MaestroGenerico>();

        if (context.Session["DatosIngresos"] != null && clearSession)
        {
            context.Session["DatosIngresos"] = null;
            context.Response.Write("{query:'" + textIngresado + "',  suggestions:[],  data:[]}");
            return;
        }

        if (context.Session["DatosIngresos"] != null)
            datosResult = (context.Session["DatosIngresos"] as List<MaestroGenerico>);

        if (context.Session["DatosIngresos"] == null || !(context.Session["DatosIngresos"] as List<MaestroGenerico>).Any(w => w.Tipo == TipoBusqueda))
        {
            EntidadesConosud a = new EntidadesConosud();

            switch (TipoBusqueda)
            {
                case "Act":
                    datosResult.AddRange((from e in a.IngresosEventuales
                                          select new MaestroGenerico
                                          {
                                              Denominacion = e.Actividad,
                                              Tipo = TipoBusqueda
                                          }).Distinct().ToList());
                    break;
                case "Cit":
                    datosResult.AddRange((from e in a.IngresosEventuales
                                          select new MaestroGenerico
                                          {
                                              Denominacion = e.Citadopor,
                                              Tipo = TipoBusqueda
                                          }).Distinct().ToList());
                    break;
                case "Ape":
                    datosResult.AddRange((from e in a.IngresosEventuales
                                          select new MaestroGenerico
                                          {
                                              Denominacion = e.ApellidoNombre,
                                              Tipo = TipoBusqueda
                                          }).Distinct().ToList());
                    break;
                case "DNI":
                    datosResult.AddRange((from e in a.IngresosEventuales
                                          select new MaestroGenerico
                                          {
                                              Denominacion = e.DNI,
                                              Tipo = TipoBusqueda
                                          }).Distinct().ToList());
                    break;
            }

            context.Session.Add("DatosIngresos", datosResult);
        }


        List<MaestroGenerico> datosFiltrados = new List<MaestroGenerico>();

        textIngresado = context.Request.QueryString["query"];
        datosFiltrados = (from e in datosResult
                          where e.Denominacion.ToUpper().StartsWith(textIngresado.ToUpper())
                          && e.Tipo == TipoBusqueda
                          select e).Distinct().ToList();



        string cadenaDatos = "";
        foreach (MaestroGenerico item in datosFiltrados)
        {
            if (cadenaDatos.IndexOf(item.Denominacion) < 0)
                cadenaDatos += "'" + item.Denominacion + "',";

        }

        if (cadenaDatos.Length > 0)
            cadenaDatos = cadenaDatos.Substring(0, cadenaDatos.Length - 1);

        context.Response.Write("{query:'" + textIngresado + "',  suggestions:[" + cadenaDatos + "],  data:[]}");

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}