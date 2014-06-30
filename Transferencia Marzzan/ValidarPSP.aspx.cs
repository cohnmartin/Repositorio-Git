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
using System.IO;
using CommonMarzzan;
using System.Web.Services;
using System.Web.SessionState;
using System.Web.Script.Services;
using System.IO.Compression;

public partial class ValidarPSP : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        /// DATOS DEVUELTOS
        /// fechahora
        /// codautorizacion
        /// cuotas
        /// titular
        /// tarjeta
        /// emailcomprador
        /// Motivo
        /// noperacion
        /// resultado
        /// monto

        /// DATOS EN LAT ABLA
        ///  Tarjeta              
        ///  Cuotas      
        ///  TarjetaAprobada 
        ///  FechaAprobacion         
        ///  CodigoAutorizacion                                 
        ///  EstadoOperacionTarjeta: PENDEINTE/FINALIZADA                             
        ///  TipoDocTitularTarjeta                              
        ///  NroDocTitularTarjeta                               
        ///  NombreTitularTarjeta
        StreamWriter _sw = null;

        try
        {
           
            _sw = new StreamWriter(HttpContext.Current.Server.MapPath("") + "\\logTarjetas" + ".txt", true);


            var nroOperacion = Request.Form["noperacion"];
            var fechahora = Request.Form["fechahora"];
            var codautorizacion = Request.Form["codautorizacion"];
            var cuotas = Request.Form["cuotas"];
            var titular = Request.Form["titular"];
            var tarjeta = Request.Form["tarjeta"];
            var emailcomprador = Request.Form["emailcomprador"];
            var Motivo = Request.Form["Motivo"];
            var resultado = Request.Form["resultado"];
            var monto = Request.Form["monto"];
            var tipodocdescri = Request.Form["tipodocdescri"];
            var nrodoc = Request.Form["nrodoc"];



            using (Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext())
            {
                CabeceraPedido cab = (from c in dc.CabeceraPedidos
                                      where c.IdCabeceraPedido == long.Parse(nroOperacion)
                                      select c).FirstOrDefault();

                cab.Cuotas = Convert.ToInt32(cuotas);
                cab.TarjetaAprobada = resultado.ToUpper() == "APROBADA" ? true : false;
                cab.FechaAprobacion = Convert.ToDateTime(fechahora.ToString());
                cab.CodigoAutorizacion = codautorizacion;
                cab.EstadoOperacionTarjeta = resultado;
                cab.NombreTitularTarjeta = titular;
                cab.TipoDocTitularTarjeta = tipodocdescri;
                cab.NroDocTitularTarjeta = nrodoc;
                cab.UltimaModificacion = DateTime.Now;
                cab.EsTemporal = false;
                cab.HuboFaltaSaldo = false;

                dc.SubmitChanges();

                _sw.WriteLine("Procesamiento Correcto: " + nroOperacion.ToString());
                _sw.Flush();
                _sw.Close();
            }
        }
        catch(Exception err)
        {
            _sw = new StreamWriter(HttpContext.Current.Server.MapPath("") + "\\logTarjetas" + ".txt", true);
            _sw.WriteLine("Error: " + err.Message);
            _sw.Flush();
            _sw.Flush();
            _sw.Close();
        
        }


        //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Impresion", "ShowComprobante('" + nroOperacion + "');", true);
    }
}
