using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CommonMarzzan;
using Telerik.Web.UI;

public partial class Inicio : BasePage
{
    public int CantMailsSinLeer
    { get; set; }

    protected override void PageLoad()
    {
        if (!IsPostBack)
        {

            Session.Timeout = 30;
            CommonMarzzan.Marzzan_InfolegacyDataContext dc = new CommonMarzzan.Marzzan_InfolegacyDataContext();
            Parametro menu = null;


            /// Cargo la variable para mostrar cuantos mail nuevos tiene.
            CantMailsSinLeer = (from m in dc.MailsDestinos
                                where m.Usuario == long.Parse(Session["IdUsuario"].ToString())
                                && m.Estado == EstadosMails.SINLEER
                                select m).Count();


            if (Session["TipoUsuario"].ToString() == "INTERNO")
            {
                menu = (from M in dc.Parametros
                        where M.Tipo == "Menu" && M.Valor == "INTERNO"
                                  select M).First<Parametro>();

                long idCliente = long.Parse(Session["IdUsuario"].ToString());
                if (idCliente != 16308 && idCliente != 16203)
                { 
                    var menuImpresion =menu.ColHijos.Where(w=>w.Valor.Contains("GestionImpresionComprobantes.aspx")).FirstOrDefault();
                    menu.ColHijos.Remove(menuImpresion);
                }

            }
            else if (Session["TipoUsuario"].ToString() == "CONFIGURADOR")
            {
                menu = (from M in dc.Parametros
                        where M.Tipo == "Menu" && M.Valor == "CONFIGURADOR"
                        select M).First<Parametro>();

            }
            else if (Session["TipoUsuario"].ToString() == "CLIENTEPDF")
            {
                menu = (from M in dc.Parametros
                        where M.Tipo == "Menu" && M.Valor == "CLIENTEPDF"
                        select M).First<Parametro>();

            }
            else if (Session["TipoUsuario"].ToString() == "RISM" || Session["TipoUsuario"].ToString() == "CONSULTOR")
            {
                menu = (from M in dc.Parametros
                        where M.Tipo == "Menu" && M.Valor == "CONSULTOR"
                        select M).First<Parametro>();

            }
            else if (Session["TipoUsuario"].ToString() == "MARKETING")
            {
                menu = (from M in dc.Parametros
                        where M.Tipo == "Menu" && M.Valor == "MARKETING"
                        select M).First<Parametro>();

            }
            else if (Session["TipoUsuario"].ToString() == "ADMIN")
            {
                menu = (from M in dc.Parametros
                        where M.Tipo == "Menu" && M.Valor == "ADMIN"
                        select M).First<Parametro>();

            }
            else if (Session["TipoUsuario"].ToString() == "ADMINISTRACION")
            {
                menu = (from M in dc.Parametros
                        where M.Tipo == "Menu" && M.Valor == "ADMINISTRACION"
                        select M).First<Parametro>();

            }
            else if (Session["TipoUsuario"].ToString() == "PRODUCCION")
            {
                menu = (from M in dc.Parametros
                        where M.Tipo == "Menu" && M.Valor == "PRODUCCION"
                        select M).First<Parametro>();

            }
            else if (Session["TipoUsuario"].ToString() == "DESPACHO")
            {
                menu = (from M in dc.Parametros
                        where M.Tipo == "Menu" && M.Valor == "DESPACHO"
                        select M).First<Parametro>();
            }
            else if (Session["TipoUsuario"].ToString() == "TRANSPORTISTA")
            {
                menu = (from M in dc.Parametros
                        where M.Tipo == "Menu" && M.Valor == "TRANSPORTISTA"
                        select M).First<Parametro>();

            }
            else if (Session["TipoUsuario"].ToString() == "POTENCIAL BOLSO" || Session["TipoUsuario"].ToString() == "POTENCIAL")
            {
                menu = (from M in dc.Parametros
                        where M.Tipo == "Menu" && M.Valor == "POTENCIAL"
                        select M).First<Parametro>();

                long idCliente = long.Parse(Session["IdUsuario"].ToString());
                
                var PermitirPedido = (from c in dc.CabeceraPedidos
                        where c.Cliente == idCliente
                        select c).Count();

                /// Si el cliente es de tipo potencial y ya posee un pedido
                /// no puede realizar otro pedido mas hasta que lo cambien de 
                /// tipo de cliente.
                if (PermitirPedido > 0)
                {
                    menu.ColHijos.RemoveAt(0);
                    ScriptManager.RegisterStartupScript(this, this.Page.GetType(),"AlertaPotencial", "ShowAlertaPotencial();", true);
                }
            }
            else
            {
                menu = (from M in dc.Parametros
                        where M.Tipo == "Menu" && M.Valor == "COORDINADOR"
                        select M).First<Parametro>();
       
                  long idCliente = long.Parse(Session["IdUsuario"].ToString());
                
                /// Pedido realizado el 21/05/2014 no se deja a este usuario utlizar la consola de mail.
                if (8164 == idCliente)
                {
                    Parametro paramMail = menu.ColHijos.Where(w => w.Tipo.Contains("de Mensaje")).FirstOrDefault();
                    menu.ColHijos.Remove(paramMail);
                    CantMailsSinLeer = 0;
                }
            }



            Parametro ItemCerrarSession = null;
            RadTreeNode node = null;

            foreach (Parametro item in menu.ColHijos)
            {
                /// creo todos los nodos según lo definido en parametros
                /// si crear la opcion de cerrar session, esta opción siempre
                /// se tiene que generar al final.
                if (item.Tipo != "Cerrar Sesión")
                {
                    node = GenerarNodo(item);
                    TreeMenu.Nodes.Add(node);
                }
                else
                {
                    ItemCerrarSession = item;
                }


                if (item.ColHijos.Count > 0)
                {
                    foreach (Parametro itemH in item.ColHijos)
                    {
                        Telerik.Web.UI.RadTreeNode nodeH = new Telerik.Web.UI.RadTreeNode(itemH.Tipo, "", itemH.Valor.Split('|')[0].ToString());
                        nodeH.Target = itemH.Valor.Split('|')[1].ToString();
                        node.Nodes.Add(nodeH);
                    }
                }
            }

            /// genero finalmente la opción de cerrar session
            node = GenerarNodo(ItemCerrarSession);
            TreeMenu.Nodes.Add(node);
           

            List<Parametro> parametrosSistema = (from P in dc.Parametros
                                                 where P.Contexto == "WEB"
                                                 select P).ToList<Parametro>();

            Session.Add("ParametrosSistema", parametrosSistema);


           

        }
       
        lblUsuario.Text = Session["NombreUsuario"].ToString();
    }

    private Telerik.Web.UI.RadTreeNode GenerarNodo(Parametro item)
    {

        Telerik.Web.UI.RadTreeNode node = new Telerik.Web.UI.RadTreeNode(item.Tipo, "", item.Valor.Split('|')[0].ToString());
        if (item.Valor != "")
            node.Target = item.Valor.Split('|')[1].ToString();

        return node;
    }
}
