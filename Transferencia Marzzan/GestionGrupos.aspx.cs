using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;
using CommonMarzzan;
using System.Web.Services;
using System.Web.SessionState;
using System.Web.Script.Services;
using System.Data.OleDb;

public partial class GestionGrupos : BasePage
{
    protected override void PageLoad()
    {

    }

    [WebMethod(EnableSession = true)]
    public static object getData()
    {

        //Dictionary<string, object> datos = new Dictionary<string, object>();

        using (Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext())
        {
            var grupos = (from g in dc.ConfGrupos
                          where g.objPadre != null
                          orderby g.Padre
                          select new
                          {
                              Nombre = g.objPadre.Nombre,
                              Responsable = g.objPadre.objResponsable.Nombre,
                              IdResponsable = g.objPadre.objResponsable.IdCliente,
                              Integrante = g.Integrante,
                              idGrupo = g.Padre.Value
                          }).ToList();

            return grupos;

        }

    }
    [WebMethod(EnableSession = true)]
    public static object getDataGrupo(long idPadre)
    {


        using (Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext())
        {
            var grupos = (from g in dc.ConfGrupos
                          where g.Padre == idPadre
                          orderby g.Padre
                          select new
                          {
                              Integrante = g.Integrante,
                              idGrupo = g.Padre.Value,
                              id= g.Id,

                          }).ToList();
           
            return grupos;

        }

    }

     [WebMethod(EnableSession = true)]
    public static object insertarGrupoIntegrante(long idGrupo, string Integrante)
    {


        using (Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext())
        {
            ConfGrupo grupo = new ConfGrupo();
            grupo.Padre = idGrupo;
            grupo.Integrante = Integrante;
            grupo.Tipo = "G";

            dc.ConfGrupos.InsertOnSubmit(grupo);
            dc.SubmitChanges();

                       
            return getDataGrupo(idGrupo);

        }

    }

     [WebMethod(EnableSession = true)]
     public static object eliminarGrupoIntegrante(long id , long idGrupo)
    {


        using (Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext())
        {
            ConfGrupo grupo = (from g in dc.ConfGrupos
                               where g.Id == id
                               select g).FirstOrDefault();

            dc.ConfGrupos.DeleteOnSubmit(grupo);
            dc.SubmitChanges();

            return getDataGrupo(idGrupo);

        }

    }

    [WebMethod(EnableSession = true)]
     public static object eliminarGrupo(long idGrupo)
    {


        using (Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext())
        {
            var grupos = (from g in dc.ConfGrupos
                               where g.Id == idGrupo || g.Padre == idGrupo
                               select g).ToList();

            dc.ConfGrupos.DeleteAllOnSubmit(grupos);
            dc.SubmitChanges();

            return getData();

        }

    }
    

     [WebMethod(EnableSession = true)]
     public static object grabarGrupo(long idGrupo, string Nombre, long IdResponsable)
     {
         using (Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext())
         {
             ConfGrupo grupo = null;
             if (idGrupo != 0)
             {
                 grupo = (from g in dc.ConfGrupos
                          where g.Id == idGrupo
                          select g).FirstOrDefault();
             }
             else
             {
                 grupo = new ConfGrupo();
                 grupo.Tipo = "CAB";
                 dc.ConfGrupos.InsertOnSubmit(grupo);
             }

             grupo.Nombre = Nombre;
             grupo.Responsable = IdResponsable;

             dc.SubmitChanges();

             return grupo.Id;

         }

     }

}