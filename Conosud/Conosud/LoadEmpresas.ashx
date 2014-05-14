<%@ WebHandler Language="C#" Class="LoadEmpresas" %>

using System;
using System.Web;
using Entidades;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Web.SessionState;

public class LoadEmpresas : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {

        context.Response.ContentType = "text/plain";

        var textIngresado = "";
        var TipoBusqueda = context.Request.QueryString["Tipo"];
        var clearSession = false;

        if (context.Request.QueryString["clearSession"] != null)
            clearSession = bool.Parse(context.Request.QueryString["clearSession"]);
        
        List<View_EmpresasContratos> datosResult = new List<View_EmpresasContratos>();

        if (context.Session["DatosEmpresa"] != null && clearSession)
        {
            context.Session["DatosEmpresa"] = null;
            context.Response.Write("{query:'" + textIngresado + "',  suggestions:[],  data:[]}");
            return;
        }
        
        if (context.Session["DatosEmpresa"] == null)
        {
            EntidadesConosud a = new EntidadesConosud();
            datosResult = (from e in a.View_EmpresasContratos
                           select e).ToList();

            context.Session.Add("DatosEmpresa", datosResult);
        }
        else
        {
            datosResult = (List<View_EmpresasContratos>)context.Session["DatosEmpresa"];
        }

        List<View_EmpresasContratos> datosFiltrados = new List<View_EmpresasContratos>();
        if (TipoBusqueda == "E")
        {
            textIngresado = context.Request.QueryString["query"];
            datosFiltrados = (from e in datosResult
                              where e.RazonSocial.ToUpper().StartsWith(textIngresado.ToUpper())
                              select e).Distinct().ToList();
        }
        else
        {
            var empIngresada = context.Request.QueryString["Empresa"];
            textIngresado = context.Request.QueryString["query"];

            if (textIngresado != null)
            {
                datosFiltrados = (from e in datosResult
                                  where e.CodigoContrato.ToUpper().StartsWith(textIngresado.ToUpper())
                                  && e.RazonSocial == empIngresada
                                  select e).Distinct().ToList();
            }
        }

        string cadenaDatos = "";
        foreach (View_EmpresasContratos item in datosFiltrados)
        {
            if (TipoBusqueda == "E")
            {
                if (cadenaDatos.IndexOf(item.RazonSocial) < 0)
                    cadenaDatos += "'" + item.RazonSocial + "',";
            }
            else
            {
                cadenaDatos += "'" + item.CodigoContrato + "',";
            }

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