<%@ WebHandler Language="C#" Class="LoadGrupos" %>

using System;
using System.Web;
using CommonMarzzan;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Web.SessionState;

public class LoadGrupos : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string textIngresado = context.Request.QueryString["query"];

        List<View_Grupo> datosResult = new List<View_Grupo>();
        List<View_Grupo> datosFiltrados = new List<View_Grupo>();


        /// si la variable esta vacia indica que el usuario tiene
        /// acceso a todos los grupos disponibles
        if (context.Session["DatosGruposAshx"] == null)
        {
            /// Cargo la variable de session que luego se utilizara 
            /// para la busquede de grupos
            Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext();
            datosResult = (from t in dc.View_Grupos
                           orderby t.Grupo
                           select t).ToList();

            
            /// Agrego el grupo de lideres para este caso.
            /// cuando el usuario selecciona este grupo se enviará un mail 
            /// a todos los usuario del tipo lider.
            View_Grupo temp = new View_Grupo();
            temp.Grupo = "LIDERES";
            datosResult.Add(temp);


            /// Agrego el grupo de revendedores para este caso.
            /// cuando el usuario selecciona este grupo se enviará un mail 
            /// a todos los revendedores activos de la web.
            temp = new View_Grupo();
            temp.Grupo = "REVENDEDORES";
            datosResult.Add(temp);
            
            
            context.Session.Add("DatosGruposAshx", datosResult);
        }
        else
        {
            datosResult = (List<View_Grupo>)context.Session["DatosGruposAshx"];
        }


        datosFiltrados = (from m in datosResult
                          where m.Grupo.ToUpper().StartsWith(textIngresado.ToUpper())
                          select m).ToList();


        string cadenaDatos = "";
        string cadenaValues = "";
        foreach (View_Grupo item in datosFiltrados)
        {
            cadenaDatos += "'" + item.Grupo + "',";
            cadenaValues += "'" + item.Grupo + "',";
        }

        if (cadenaDatos.Length > 0)
            cadenaDatos = cadenaDatos.Substring(0, cadenaDatos.Length - 1);

        context.Response.Write("{query:'" + textIngresado + "',  suggestions:[" + cadenaDatos + "],  data:[" + cadenaValues + "]}");
        
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}