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

public partial class GestionBajaClientes : BasePage
{
    private static Marzzan_BejermanDataContext ContextoBejerman
    {
        get
        {

            if (HttpContext.Current.Session["ContextoBejerman"] == null)
            {
                HttpContext.Current.Session.Add("ContextoBejerman", new Marzzan_BejermanDataContext());
            }

            return (Marzzan_BejermanDataContext)HttpContext.Current.Session["ContextoBejerman"];
        }

    }

    private static Marzzan_InfolegacyDataContext Contexto
    {
        get
        {

            if (HttpContext.Current.Session["Context"] == null)
            {
                HttpContext.Current.Session.Add("Context", new Marzzan_InfolegacyDataContext());
            }

            return (Marzzan_InfolegacyDataContext)HttpContext.Current.Session["Context"];
        }

    }

    public string CantidadBajas
    {
        get
        {
            return HttpContext.Current.Session["BajasDelMes"].ToString();
        }

    }


    protected override void PageLoad()
    {
        if (!IsPostBack)
        {
            Session["GruposHibilitados"] = Helper.ObtenerGruposSubordinados(Session["GrupoCliente"].ToString());
            Session["SolicitudesFiltradas"] = new List<SolicitudesAlta>();
            Session["Context"] = new Marzzan_InfolegacyDataContext();
            Session["ContextoBejerman"] = new Marzzan_BejermanDataContext();
            Session["BajasDelMes"] = 0;

            cboGrupos.AppendDataBoundItems = true;
            cboGrupos.DataSource = (List<string>)Session["GruposHibilitados"];
            cboGrupos.DataBind();


            CalcularBajasMes();


        }
    }

    private static void CalcularBajasMes()
    {
        long usuarioLogin = long.Parse(HttpContext.Current.Session["IdUsuario"].ToString());
        using (Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext())
        {
            HttpContext.Current.Session["BajasDelMes"] = (from b in dc.HistorialBajas
                                                          where b.Usuario == usuarioLogin
                                                          && b.FechaBaja.Value.Month == DateTime.Now.Month
                                                          && b.FechaBaja.Value.Year == DateTime.Now.Year
                                                          select b).Count();
        }
    }

    [WebMethod(EnableSession = true)]
    public static IDictionary<string, object> GetClientesBajas(string grupo)
    {

        Dictionary<string, object> datos = new Dictionary<string, object>();

        try
        {
            if (grupo != "")
            {
                var clientes = (from c in ContextoBejerman.veocliultcpras
                                where c.dc1_Desc == grupo && c.DiasSCompra >= 180
                                orderby c.cli_RazSoc
                                select new
                                {
                                    Cliente = c.cli_RazSoc,
                                    Codigo = c.cli_Cod,
                                    UltimaCompra = c.cfc_FUltCpra.Value,
                                    DiasSinCompras = c.DiasSCompra,
                                    Grupo = c.dc1_Desc,
                                    Saldo = string.Format("{0:#0.00}", c.SaldoPagoAnticipado)

                                }).ToList();


                datos.Add("Clientes", clientes.ToList());
                datos.Add("CantidadBajas", HttpContext.Current.Session["BajasDelMes"].ToString());
            }
            else
            {
                List<string> TodosGrupos = (List<string>)HttpContext.Current.Session["GruposHibilitados"];

                var clientes = (from c in ContextoBejerman.veocliultcpras
                                where TodosGrupos.Contains(c.dc1_Desc)
                                && c.DiasSCompra >= 180
                                orderby c.cli_RazSoc
                                select new
                                {
                                    Cliente = c.cli_RazSoc,
                                    Codigo = c.cli_Cod,
                                    UltimaCompra = c.cfc_FUltCpra.Value,
                                    DiasSinCompras = c.DiasSCompra,
                                    Grupo = c.dc1_Desc,
                                    Saldo = string.Format("{0:#0.00}", c.SaldoPagoAnticipado)

                                }).ToList();

                datos.Add("Clientes", clientes.ToList());
                datos.Add("CantidadBajas", HttpContext.Current.Session["BajasDelMes"].ToString());
            }


        }
        catch
        {


        }

        return datos;

    }


    [WebMethod(EnableSession = true)]
    public static IDictionary<string, object> GrabarBajas(List<Dictionary<string, string>> datosComp)
    {

        var CodigosRevendedores = datosComp.Where(dict => dict.ContainsKey("codigo")).Select(dict => dict["codigo"])
                     .Distinct()
                     .ToList();

        var SaldosRevendedores = datosComp.Where(dict => dict.ContainsKey("codigo")).Select(dict => new { codigo = dict["codigo"], saldo = dict["saldo"] })
                     .Distinct()
                     .ToList();

        #region  Bajas en la base de datos de Bejerman.

        #region Datos Directos del cliente de Bejerman
        var clientesBejerman = (from c in ContextoBejerman.Clientes
                                where CodigosRevendedores.Contains(c.cli_Cod)
                                select c).ToList();

        foreach (var item in clientesBejerman)
        {
            item.cli_Habilitado = false;
        }

        #endregion

        #region Actualizo los datos adicionales para el cliente

        var clientesBejermanDTS = (from c in ContextoBejerman.DtsClientes
                                   where CodigosRevendedores.Contains(c.cli_Cod)
                                   select c).ToList();

        foreach (var item in clientesBejermanDTS)
        {
            item.DcliFBaja = DateTime.Now;
        }

        #endregion


        #endregion

        #region Bajas en la Web
        var clientesWeb = (from c in Contexto.Clientes
                           where CodigosRevendedores.Contains(c.CodigoExterno)
                           select c).ToList();

        foreach (var item in clientesWeb)
        {
            item.Habilitado = false;

            HistorialBaja newHist = new HistorialBaja();
            newHist.FechaBaja = DateTime.Now;
            newHist.Usuario = long.Parse(HttpContext.Current.Session["IdUsuario"].ToString());
            newHist.CodigoRevendedorBejerman = item.CodigoExterno;
            newHist.Grupo = item.Clasif1;
            newHist.RevendedorBaja = item.IdCliente;
            newHist.DiasSinPedido = Convert.ToInt32(datosComp.Where(dict => dict["codigo"] == item.CodigoExterno).Select(dict => dict["dias"]).FirstOrDefault());
            Contexto.HistorialBajas.InsertOnSubmit(newHist);


        }

        #endregion



        string revendedores = "";
        foreach (var item in clientesWeb)
        {
            decimal saldo = decimal.Parse(SaldosRevendedores.Where(w => w.codigo == item.CodigoExterno).FirstOrDefault().saldo);
            revendedores += item.CodigoExterno + " - " + item.Nombre + " Saldo al momento de la baja: " + string.Format("{0:#0.00}", saldo) + "<br>";
        }

        #region Recupero el asistente nro 6, que es el destinatario del mail
        /// Pedido solicitado por Miguel el 05/01/2015 cambio de asistente responsable del 2 al 6

        long IdAsistente = (from d in Contexto.Clientes
                            where d.Email.ToLower() == "asistente6@sandramarzzan.com.ar"
                            && d.TipoCliente == "INTERNO"
                            select d.IdCliente).FirstOrDefault();

        #endregion

        #region Envio de Mail al lider del usuario logeado

        MailsCabecera mail = new MailsCabecera();
        mail.Cuerpo = "Se informa que en el día de la fecha se ha dado de baja los siguientes revendedores:<br>" + revendedores;
        mail.Fecha = DateTime.Now;
        mail.Subject = "Baja de Clientes";
        mail.Usuario = long.Parse(HttpContext.Current.Session["IdUsuario"].ToString());



        MailsDestino mDestino = new MailsDestino();
        mDestino.MailsCabecera = mail;
        mDestino.Usuario = IdAsistente;
        mDestino.Estado = EstadosMails.SINLEER;
        mDestino.FechaCambioEstado = DateTime.Now;
        mail.MailsDestinos.Add(mDestino);


        Contexto.MailsCabeceras.InsertOnSubmit(mail);


        #endregion

        #region



        #endregion

        Contexto.SubmitChanges();
        ContextoBejerman.SubmitChanges();


        /// Depues de grabar las bajas re calculo el valor de la cantidad de bajas.
        CalcularBajasMes();

        return GetClientesBajas("");
    }
}
