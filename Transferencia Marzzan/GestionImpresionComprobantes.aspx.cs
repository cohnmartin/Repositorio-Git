using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CommonMarzzan;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Telerik.Reporting;
using Telerik.Reporting.Processing;
using ReportesMarzzan;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using System.Web.SessionState;
using System.Web.Script.Services;

public partial class GestionImpresionComprobantes : BasePage
{

    private const int ItemsPerRequest = 10;
    private decimal _CostoFlete = 0;
    private Dictionary<long, string> DetalleTrasporte = new Dictionary<long, string>();
    private List<TempClass.tempEnviosMisiones> EnviosMisiones = new List<TempClass.tempEnviosMisiones>();
    private string nroLote = "";

    /// <summary>
    /// Contiene los clientes que poseen mas de un pedido en el lote de
    /// impresión
    /// </summary>
    private IDictionary<long, int> ClientesCantidadPedidos
    {
        get;
        set;
    }

    private List<DistribucionProducto> AlldistribucionExistente
    {

        get
        {

            if (Session["AlldistribucionExistente"] == null)
            {
                Session.Add("AlldistribucionExistente", (from d in Contexto.DistribucionProductos select d).ToList());
            }

            return (List<DistribucionProducto>)Session["AlldistribucionExistente"];
        }

    }

    private Marzzan_InfolegacyDataContext Contexto
    {
        get
        {

            if (Session["Context"] == null)
            {
                Session.Add("Context", new Marzzan_InfolegacyDataContext());
            }

            return (Marzzan_InfolegacyDataContext)Session["Context"];
        }

    }

    protected override void PageLoad()
    {
        if (!IsPostBack)
        {
            Session["Context"] = null;
            Session["GruposValidos"] = null;
            txtFechaInicial.SelectedDate = DateTime.Now;
            txtFechaFinal.SelectedDate = DateTime.Now;

            GrillaResultados.DataSource = new List<CabeceraPedido>();
            GrillaResultados.DataBind();


            cboTransporte.DataTextField = "Transporte";
            cboTransporte.DataValueField = "Transporte";
            cboTransporte.DataSource = (from T in Contexto.View_Transportes
                                        orderby T.Transporte
                                        select new { IdConfTransportes = T.Transporte, Transporte = T.Transporte }).ToList();

            cboTransporte.DataBind();
            cboTransporte.Items.Insert(0, new RadComboBoxItem("Seleccione un Transportista"));



            //AlldistribucionExistente = (from d in Contexto.DistribucionProductos
            //                            select d).ToList();


            //ControlarClientesSinConfiguracion();
        }


    }

    protected void cboProfesionales_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {


        var data = (from C in Contexto.Clientes
                    where C.Nombre.Contains(e.Text) && C.Habilitado == true
                    orderby C.Nombre
                    select new { C.IdCliente, C.Nombre }).Take(15);


        int itemOffset = e.NumberOfItems;
        int endOffset = Math.Min(itemOffset + ItemsPerRequest, data.Count());
        e.EndOfItems = endOffset == data.Count();

        for (int i = itemOffset; i < endOffset; i++)
        {
            this.cboConsultores.Items.Add(new RadComboBoxItem(data.ToList()[i].Nombre, data.ToList()[i].IdCliente.ToString()));
        }

        e.Message = GetStatusMessage(endOffset, data.Count());
    }

    protected void cboProfesionales_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {

    }

    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No existen coincidencias";

        return String.Format("Registros: <b>1</b>-<b>{0}</b> de <b>{1}</b>", offset, total);
    }

    public string GenerarEtiqueta(string DescripcionCompleta, string DescripcionPresentacion, string DescripcionSimple)
    {
        if (DescripcionCompleta.ToLower().Contains("incorporac"))
        {
            return DescripcionSimple.ToString() + " x " + DescripcionPresentacion;
        }
        else
            return DescripcionCompleta + DescripcionPresentacion;

    }

    public string GenerarEstado(object estado)
    {
        int CantidadImpresiones = int.Parse(estado.ToString());

        if (CantidadImpresiones == 0)
        {
            return "ORIGINAL";

        }
        else
        {
            return "IMPRESION NRO " + CantidadImpresiones.ToString();
        }

    }

    private void ControlarClientesSinConfiguracion()
    {

        var clientesConCargo = (from C in Contexto.Clientes
                                where (C.CodTipoCliente != TipoClientesConstantes.CONSULTOR
                                && C.CodTipoCliente != TipoClientesConstantes.INTERNO
                                && C.TipoCliente != TipoClientesConstantes.DESC_BOLSOS
                                && C.TipoCliente != TipoClientesConstantes.DESC_POTECIAL
                                && C.TipoCliente != TipoClientesConstantes.DESC_SINCARGO
                                && C.TipoCliente != TipoClientesConstantes.DESC_STOCK)
                                && Convert.ToInt32(C.CodVendedor) > 0
                                select new
                                {
                                    C.IdCliente,
                                    C.Nombre,
                                    C.TipoCliente
                                }).ToList();

        long[] IdsClientesConCargoConfigurados = (from C in Contexto.ConfMails
                                                  where C.Consultor.HasValue
                                                  select C.Consultor.Value).ToArray<long>();


        var clientesSinConfigurar = (from C in clientesConCargo
                                     where !IdsClientesConCargoConfigurados.Contains(C.IdCliente)
                                     orderby C.Nombre
                                     select C).ToList();


        if (clientesSinConfigurar.Count > 0)
        {
            Session.Add("ClienteSinConf", clientesSinConfigurar);
            gvClientes.DataSource = clientesSinConfigurar;
            gvClientes.DataBind();
            ToolTipClienteSinConf.Visible = true;
        }
        else
            ToolTipClienteSinConf.Visible = false;

    }

    protected void gvClientes_PageIndexChanged(object source, GridPageChangedEventArgs e)
    {
        gvClientes.CurrentPageIndex = e.NewPageIndex;
        gvClientes.DataSource = Session["ClienteSinConf"];
        gvClientes.DataBind();
        UpdatePanel1.Update();

    }

    protected void btnConsultar_Click(object sender, EventArgs e)
    {

        string tiempos = "";

        try
        {
            List<CabeceraPedido> cabeceras = new List<CabeceraPedido>();
            Cliente clienteCtaBolsos = new Cliente();
            List<Cliente> consultores = new List<Cliente>();
            List<long> IdsConsultores = new List<long>();
            List<CabeceraPedido> cabTemp = new List<CabeceraPedido>();
            Cliente cliente = null;

            tiempos += "Inicio: " + string.Format("{0:mm:ss}", DateTime.Now) + " - ";

            if (cboConsultores.SelectedValue != "")
            {

                cliente = (from C in Contexto.Clientes
                           where C.IdCliente == long.Parse(cboConsultores.SelectedValue)
                           orderby C.Nombre
                           select C).Single<Cliente>();

                consultores = Helper.ObtenerConsultoresSubordinados(cliente);


                if (chkBolsos.Checked)
                {
                    clienteCtaBolsos = (from C in Contexto.Clientes
                                        where C.Dni == cliente.Dni && C.Nombre.Contains("bolsos")
                                        select C).SingleOrDefault<Cliente>();

                    if (clienteCtaBolsos != null)
                    {
                        consultores.Add(clienteCtaBolsos);
                    }
                }


                IdsConsultores = (from C in consultores
                                  select C.IdCliente).ToList();
            }
            else
            {
                //obtengo los coordinadores que son administrados por el asistente que esta
                //logeado en el sistema.
                var coordinadoresACargo = (from C in Contexto.ConfMails
                                           where C.EmailDestino.ToLower() == Session["EmailUsuario"].ToString().ToLower()
                                           && C.objCoordinador.Habilitado.Value
                                           && C.objCoordinador.Clasif1.Contains("-")
                                           select new
                                           {
                                               C.objCoordinador.Clasif1,
                                               C.objCoordinador.CodVendedor,
                                               C.objCoordinador.IdCliente,
                                               C.objCoordinador.Nombre,
                                               C.objCoordinador.CodTipoCliente
                                           }).ToList();


                List<string> GrupoSuperior = (from g in coordinadoresACargo
                                              where g.Clasif1.Split('-')[0].Trim() == g.Clasif1.Split('-')[1].Trim()
                                              && g.CodTipoCliente != "1"
                                              select g.CodVendedor).ToList();


                List<string> GrupoDependiente = (from g in coordinadoresACargo
                                                 where g.Clasif1.Split('-')[0].Trim() != g.Clasif1.Split('-')[1].Trim()
                                                 && g.CodTipoCliente != "1"
                                                 select g.Clasif1).ToList();


                tiempos += "consultores Principales: " + string.Format("{0:mm:ss}", DateTime.Now) + " - ";

                IdsConsultores = (from C in Contexto.Clientes
                                  where C.Habilitado.Value &&
                                  (GrupoSuperior.Contains(C.CodVendedor) || GrupoDependiente.Contains(C.Clasif1) || C.Nombre.Contains("bolsos"))
                                  orderby C.Nombre
                                  select C.IdCliente).ToList<long>();

                tiempos += "Todos los Consultores: " + string.Format("{0:mm:ss}", DateTime.Now) + " - ";
            }


            int IncluirImpresas = 0;
            if (chkImpresas.Checked)
            {
                IncluirImpresas = 100;
            }

            tiempos += "consultores: " + string.Format("{0:mm:ss}", DateTime.Now) + " - ";


            List<View_PedidosParaImpresion> ResultadoFiltradoPorConsultores = new List<View_PedidosParaImpresion>();
            List<View_PedidosParaImpresion> Resultado = (from v in Contexto.View_PedidosParaImpresions
                                                         where (v.FechaPedido.Date >= txtFechaInicial.SelectedDate.Value.Date && v.FechaPedido.Date <= txtFechaFinal.SelectedDate.Value.Date)
                                                         && v.Transporte == cboTransporte.SelectedItem.Text.ToUpper()
                                                         && (v.NroImpresion == 0 || v.NroImpresion <= IncluirImpresas)
                                                         select v).ToList();

            //List<CabeceraPedido> Resultado111 = (from v in Contexto.CabeceraPedidos
            //                                             where (v.FechaPedido.Date >= txtFechaInicial.SelectedDate.Value.Date && v.FechaPedido.Date <= txtFechaFinal.SelectedDate.Value.Date)
            //                                             && v.Transporte == cboTransporte.SelectedItem.Text.ToUpper()
            //                                             && (v.NroImpresion == 0 || v.NroImpresion <= IncluirImpresas)
            //                                             select v).ToList();

            foreach (View_PedidosParaImpresion cab in Resultado)
            {
                if (IdsConsultores.Contains(cab.Cliente))
                {
                    ResultadoFiltradoPorConsultores.Add(cab);
                }
            }



            /// Cargo la grilla solamente con los valores necesarios (optimizacion)
            GrillaResultados.DataSource = ResultadoFiltradoPorConsultores;
            GrillaResultados.DataBind();
            upResultado.Update();

            ///// Guardo las cabeceras para luego utilizarlas para la impresión.
            List<long> lista = (from C in ResultadoFiltradoPorConsultores
                                select C.IdCabeceraPedido).Distinct().ToList<long>();

            Session.Add("ListaPedido", lista);



            return;

            //List<string> ProvSegunTransp = (from C in Contexto.View_Provincias_Transportes
            //                                where C.Transporte.ToUpper() == cboTransporte.SelectedItem.Text.ToUpper()
            //                                select C.Provincia.ToUpper().Trim()).ToList<string>();


            //List<string> LocSegunTransp = (from C in Contexto.View_Localidades_Transportes
            //                               where C.Transporte.ToUpper() == cboTransporte.SelectedItem.Text.ToUpper()
            //                               select C.Localidad.ToUpper().Trim()).ToList<string>();


            //tiempos += "Provincias: " + string.Format("{0:mm:ss}", DateTime.Now) + " - ";

            //if (consultores != null)
            //{

            //    /// Primero busco en la base de datos segun:
            //    /// Que no sea un pedido temporal
            //    /// Que este dentro del rango de fechas seleccionado
            //    /// Que tenga direccion de entrega y que la provincia coincida con la del transportista seleccionado
            //    /// Que tenga haya sido impreso o no segun el filtro
            //    cabTemp = (from C in Contexto.CabeceraPedidos
            //               where (C.FechaPedido.Date >= txtFechaInicial.SelectedDate.Value.Date && C.FechaPedido.Date <= txtFechaFinal.SelectedDate.Value.Date)
            //                      && (C.EsTemporal == null || C.EsTemporal.Value == false)
            //                      && (C.objDireccion != null && ProvSegunTransp.Contains(C.objDireccion.Provincia.Trim().ToUpper()))
            //                      && (C.NroImpresion == 0 || C.NroImpresion <= IncluirImpresas)
            //               select C).ToList<CabeceraPedido>();

            //    /// Segundo filtro el resultado segun las localidades del transportista seleccionado
            //    cabTemp = (from C in cabTemp
            //               where (C.objDireccion != null && LocSegunTransp.Contains(C.objDireccion.Localidad.Trim().ToUpper()))
            //               select C).ToList<CabeceraPedido>();

            //    /// Tercero solo muestro los pedido realizados por los 
            //    /// consultores que dependen del asistente que esta logeado.
            //    foreach (CabeceraPedido cab in cabTemp)
            //    {
            //        if (IdsConsultores.Contains(cab.Cliente))
            //        {
            //            cabeceras.Add(cab);
            //        }
            //    }
            //}

            //tiempos += "Pedidos: " + string.Format("{0:mm:ss}", DateTime.Now) + " - ";

            ///// Cargo la grilla solamente con los valores necesarios (optimizacion)
            //GrillaResultados.DataSource = (from C in cabeceras
            //                               orderby C.FechaPedido, C.TipoPedido, C.Nro, C.objCliente.Nombre
            //                               select new
            //                               {
            //                                   IdCabeceraPedido = C.IdCabeceraPedido,
            //                                   Nro = C.Nro,
            //                                   Solicitante = C.objClienteSolicitante.Nombre,
            //                                   Destinatario = C.objCliente.Nombre,
            //                                   C.FechaPedido,
            //                                   C.TipoPedido,
            //                                   C.MontoTotal,
            //                                   FormaPago = C.objFormaDePago.Descripcion,
            //                                   Direccion = C.objDireccion.DireccionCompleta,
            //                                   Estado = this.GenerarEstado(C.NroImpresion)
            //                               }).Distinct().ToList();
            //GrillaResultados.DataBind();
            //upResultado.Update();

            ///// Guardo las cabeceras para luego utilizarlas para la impresión.
            //List<CabeceraPedido> lista = (from C in cabeceras
            //                              orderby C.FechaPedido, C.TipoPedido, C.Nro, C.objCliente.Nombre
            //                              select C).Distinct().ToList<CabeceraPedido>();

            //Session.Add("ListaPedido", lista);



        }
        catch (Exception err)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert(' se ha producido un error al consultar: " + err.Message + "');", true);
        }
    }

    #region Metodos de Impresion

    protected void btnActDatos_Click(object sender, EventArgs e)
    {
        List<long> ids = (List<long>)Session["ListaPedido"];


        if (GrillaResultados.SelectedItems.Count > 0)
        {
            ids = new List<long>();
            foreach (GridItem item in GrillaResultados.SelectedItems)
            {

                ids.Add(long.Parse(GrillaResultados.Items[item.DataSetIndex].GetDataKeyValue("IdCabeceraPedido").ToString()));

            }


        }

        List<CabeceraPedido> cabeceras = (from c in Contexto.CabeceraPedidos
                                          where ids.Contains(c.IdCabeceraPedido)
                                          select c).ToList();


        ActualizarDatosBejerman(cabeceras);

    }

    protected void btnGenerar_Click(object sender, EventArgs e)
    {
        nroLote = "";
        Contexto.CommandTimeout = 600;

        List<long> ids = hdf_ids.Value.Split(',').Select(w => long.Parse(w)).ToList();


        var cabecerasLoad = (from c in Contexto.CabeceraPedidos
                             where ids.Contains(c.IdCabeceraPedido)
                             && c.FechaPedido.Date >= txtFechaInicial.SelectedDate.Value.Date
                             && c.FechaPedido.Date <= txtFechaFinal.SelectedDate.Value.Date
                             select new
                             {
                                 c,
                                 c.DetallePedidos
                             }).ToList();


        List<CabeceraPedido> cabeceras = cabecerasLoad.Select(w => w.c).ToList();


        #region  Calculo los clientes que poseen mas de un pedido

        ClientesCantidadPedidos = new Dictionary<long, int>();

        var SumaPedidos = from p in cabeceras
                          group p by p.Cliente into g
                          where g.Count() > 1
                          select new
                          {
                              Cliente = g.Key,
                              Cantidad = g.Count()
                          };

        foreach (var item in SumaPedidos)
        {
            ClientesCantidadPedidos.Add(item.Cliente, item.Cantidad);
        }

        #endregion


        #region Genero los datos de auditoria de impresion

        List<long> idsCab = cabecerasLoad.Select(w => w.c.IdCabeceraPedido).ToList();

        List<TempClass.TempAuditoriaImpresion> PedidosAgrupados = (from p in Contexto.View_PedidosParaImpresions
                                                                   where idsCab.Contains(p.IdCabeceraPedido)
                                                                   group p by new { p.Transporte, p.Grupo } into g
                                                                   select new TempClass.TempAuditoriaImpresion
                                                                   {
                                                                       Transporte = g.Key.Transporte,
                                                                       Grupo = g.Key.Grupo,
                                                                       Cantidad = g.Count(),
                                                                       Pedidos = g.Select(w => w.IdCabeceraPedido).ToList()
                                                                   }).ToList();


        #endregion



        string ruta = @"HideWaiting();window.open('ImpresionesPedido/" + GenerarReporte(cabeceras, PedidosAgrupados) + "');";

        /// Envio de mail al asistente2 con la suma de los pedidos de misiones
        EnviarMailMisiones();

        if (!ruta.Contains("Error"))
        {
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel2, typeof(UpdatePanel), "clickConsultar", ruta, true);
        }
        else
        {
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel2, typeof(UpdatePanel), "clickConsultar", "alert('" + ruta + "')", true);
        }


    }

    private string GenerarReporte(List<CabeceraPedido> lista, List<TempClass.TempAuditoriaImpresion> PedidosAgrupados)
    {
        #region Fuente
        iTextSharp.text.Font font10B = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD);
        iTextSharp.text.Font font10 = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10);
        iTextSharp.text.Font font9 = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 9);
        iTextSharp.text.Font font24B = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 24, iTextSharp.text.Font.BOLD);
        #endregion

        string NombreDocumento = "";
        if (lista.Count > 0)
        {

            NombreDocumento = "InformePedididos_" + Session["IdUsuario"].ToString() + ".pdf";
            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.LEGAL);
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(Server.MapPath("ImpresionesPedido") + "\\" + NombreDocumento, FileMode.Create));
            document.Open();
            iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
            document.NewPage();

            try
            {
                /// Genero la Hoja con el detalle del lote
                GenerarDetalleImpresion(cb, PedidosAgrupados, document);
                document.NewPage();
                //document.Close();
                //return NombreDocumento;

                foreach (CabeceraPedido cab in lista)
                {

                    /// Genero la tabla con el detalle del pedido
                    GenerarDetallePedido(cb, cab, document);

                    /// Genero la tabla con los remitos pendientes solo si los mismos
                    /// no han sido ya incluidos dentro del detalla de la nota de pedido
                    if (!ExistenRemitosEnDetalle(cab))
                    {
                        document.Add(new iTextSharp.text.Phrase("Remitos Pendientes:", font10B));
                        document.Add(GenerarDetalleRemitos(cab));
                    }


                    /// Genero la tabla con las observaciones
                    document.Add(new Paragraph(" "));/// Linea de espacio en blanco
                    document.Add(new iTextSharp.text.Phrase("Observaciones:", font10B));
                    document.Add(GenerarDetalleObservacion(cab));


                    /// Marco el pedido con credito como que fue procesado
                    /// para tenerlo en cuenta para el proceso de calcelación
                    /// de pedido con credito en la actuliazación de clientes.
                    PedidosConCredito PedidoConCredito = (from c in Contexto.PedidosConCreditos
                                                          where c.Registracion == cab.IdCabeceraPedido
                                                          select c).FirstOrDefault();

                    if (PedidoConCredito != null)
                    {
                        PedidoConCredito.Procesado = true;
                    }


                    document.NewPage();
                    cab.NroImpresion++;
                    cab.FechaImpresion = DateTime.Now;


                }

                document.Close();
                Contexto.SubmitChanges();

                //EnviarMailPromo2x1(lista);
                EnviarMailProcesamiento(lista);
                ActualizarDatosBejerman(lista);
            }
            catch (Exception e)
            {
                document.Close();
                NombreDocumento = "Error: " + e.Message;
                StreamWriter _sw = new StreamWriter(Server.MapPath("") + "\\LogImpresion.txt", true);
                _sw.WriteLine("Fecha: " + DateTime.Now.ToString());
                _sw.WriteLine(e.Message);
                _sw.WriteLine(e.StackTrace);
                _sw.Close();

            }
        }



        return NombreDocumento;
    }

    private void EnviarMailPromo2x1(List<CabeceraPedido> Pedidos)
    {

        /// Solo se tiene que enviar el mail de haber ganado el descuento si es
        /// procesados por primera vez.
        var pedidosValidos = (from p in Pedidos
                              where p.NroImpresion == 1
                              select p).ToList();

        /// Cambio solicitado por Raúl (31/01/2014): Solo se debe procesar los pedidos que han sido realizado
        /// por revendedores distinto a potencial.
        List<long> idclientesPedidos = (from p in Pedidos
                                        where p.objCliente.CodTipoCliente != ((int)TipoClientes.PotencialBolso).ToString()
                                        select p.Cliente).Distinct().ToList();

        using (Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext())
        {
            /// Cambio solicitado por  miguel (16/01/2014): Se agregaron los líderes 4 y 5 a la lista 
            /// de líderes válidos
            List<string> coordinadoresAplicables = new List<string>() { "2", "3", "4", "5" };
            var coordinadoresValidos = (from c in dc.Clientes
                                        where coordinadoresAplicables.Contains(c.CodTipoCliente)
                                        select new
                                        {
                                            c.CodigoExterno,
                                            c.IdCliente

                                        }).ToList();

            /// Busco todos los clientes que tiene un solo pedido y que dentro del mismo ha solicitado
            /// la incorporacion 2,3,4.
            DateTime fechaInicialPedido = DateTime.Parse("01/01/2013");
            var ClientesUnicoPedido = (from p in dc.CabeceraPedidos
                                       where idclientesPedidos.Contains(p.Cliente)
                                       // que no sea temporal 
                                        && (p.EsTemporal == null || p.EsTemporal.Value == false) && p.FechaPedido.Date >= fechaInicialPedido
                                       // agrupo para poder contar cuanto pedidos tienen
                                       group p by p.objCliente.CodigoExterno into g
                                       // solo los clientes que tiene un solo pedido
                                       where g.Count() == 1
                                       select new
                                       {
                                           idclient = g.Key,
                                           Pedido = g.FirstOrDefault()
                                       }).ToList();



            var ClientesPrimerPedido = (from p in ClientesUnicoPedido
                                        where
                                            // pedido que contengan en su detalle las incorportaciones 2,3 o 4
                                        p.Pedido.DetallePedidos.Any(w => w.Presentacion.Value == 5797 || w.Presentacion.Value == 5798 || w.Presentacion.Value == 5799)
                                        select new
                                        {
                                            idclient = p.idclient
                                        }).ToList();



            /// Busco las altas válidas para luego determinar si el pedido que se esta imprimiendo
            /// esta dentro de estas para proceder a enviar el mail para información que se obtuvo el 
            /// descuento.
            DateTime fechaInicial = DateTime.Parse("01/11/2013");
            DateTime fechaFinal = DateTime.Parse("28/02/2014");
            var altasValidas = (from a in dc.SolicitudesAltas
                                where
                                    // que la solictud este hecha en el periodo especifico
                                a.FechaSolicitud.Value.Date >= fechaInicial && a.FechaSolicitud.Value.Date <= fechaFinal
                                    // que sea un alta satisfactria
                                && a.CodigoBejerman != null
                                    // que el coordinador sea del tipo 2 o 3
                                && coordinadoresValidos.Select(w => w.CodigoExterno).Contains(a.IdCoordinadorBejerman)
                                    // que se le este procesando el primer pedido al cliente
                                && ClientesPrimerPedido.Select(w => w.idclient).Contains(a.CodigoBejerman)
                                select new
                                {
                                    a.CodigoBejerman,
                                    a.Nombre,
                                    a.IdCoordinadorBejerman
                                }).ToList();


            List<string> codigosClientesPedido = pedidosValidos.Select(w => w.objCliente.CodigoExterno).ToList();

            var AltaConPromo = altasValidas.Where(w => codigosClientesPedido.Contains(w.CodigoBejerman)).ToList();

            foreach (var alta in AltaConPromo)
            {
                /// deberia buscar el lider del cliente y enviar el mail, tanto al lider como al asistente 1
                var coordinador = coordinadoresValidos.Where(w => w.CodigoExterno == alta.IdCoordinadorBejerman).FirstOrDefault();

                #region Envio del mail

                string mensaje = "<p>Felicitaciones estimada Lider, por haber realizado un alta efectiva de la persona " + alta.Nombre + ", accedes al beneficio de la promo 2 x 1 incorporaciones.</p>" +
                "<p>Por favor indica a tu Asistente, respondiendo este mail, a que nuevo revendedor le asignas esta oportunidad.</p>" +
                "<p>Saludos</p>";

                MailsCabecera mail = new MailsCabecera();
                mail.Cuerpo = mensaje;
                mail.Fecha = DateTime.Now;
                mail.Subject = "Pedido Ganaste Promo 2x1";
                mail.Usuario = long.Parse(Session["IdUsuario"].ToString());



                MailsDestino mDestino = new MailsDestino();
                mDestino.MailsCabecera = mail;
                mDestino.Usuario = coordinador.IdCliente;
                mDestino.Estado = EstadosMails.SINLEER;
                mDestino.FechaCambioEstado = DateTime.Now;
                mail.MailsDestinos.Add(mDestino);
                dc.MailsCabeceras.InsertOnSubmit(mail);

                mDestino = new MailsDestino();
                mDestino.MailsCabecera = mail;
                mDestino.Usuario = long.Parse(Session["IdUsuario"].ToString());
                mDestino.Estado = EstadosMails.SINLEER;
                mDestino.FechaCambioEstado = DateTime.Now;
                mail.MailsDestinos.Add(mDestino);
                dc.MailsCabeceras.InsertOnSubmit(mail);

                dc.SubmitChanges();


                #endregion

            }

        }
    }

    private void EnviarMailProcesamiento(List<CabeceraPedido> Pedidos)
    {

        /// Solo se tiene que enviar la información de los predidos que son
        /// procesados por primera vez.
        var infoPedidos = (from p in Pedidos
                           where p.NroImpresion == 1
                           group p by p.objCliente.Clasif1 into g
                           select new TempClass.TempInfoPedidos
                           {
                               Grupo = g.Key,
                               Pedidos = g.ToList(),
                           }).OrderBy(w => w.Grupo).ToList();



        List<TempClass.TempInfoPedidos> infoPedidoCompletos = new List<TempClass.TempInfoPedidos>();
        infoPedidoCompletos.AddRange(infoPedidos.ToList());

        List<long> lideres = new List<long>();
        foreach (var item in infoPedidos)
        {

            List<string> partesGrupo = item.Grupo.Split('-').ToList();
            string grupoSuperior = "";
            for (int i = 0; i < partesGrupo.Count - 1; i++)
            {
                if (partesGrupo.Count == 2 && partesGrupo[0] != partesGrupo[1])
                {
                    grupoSuperior += string.Join("-", partesGrupo.Take(partesGrupo.Count - 1 - i).ToArray());
                    if (partesGrupo.Count - 1 == 1)
                        grupoSuperior += "-" + grupoSuperior;

                    var infoGrupoSuperior = infoPedidoCompletos.Where(w => w.Grupo == grupoSuperior).FirstOrDefault();
                    if (infoGrupoSuperior == null)
                    {
                        TempClass.TempInfoPedidos info = new TempClass.TempInfoPedidos();
                        info.Grupo = grupoSuperior;
                        info.Pedidos = item.Pedidos;
                        infoPedidoCompletos.Add(info);
                    }
                    else
                    {
                        infoGrupoSuperior.Pedidos.AddRange(item.Pedidos);
                    }
                }

            }
        }

        //Recupero el id de cada liber para enviar el mail.
        foreach (var item in infoPedidoCompletos.OrderBy(w => w.Grupo))
        {
            long idLider = Helper.ObtenerLider(0, item.Grupo);

            if (idLider > 0)
            {
                #region Definicion de la tabla
                HtmlTable tbl = new HtmlTable();
                tbl.Attributes.Add("cellpadding", "0px");
                tbl.Attributes.Add("cellspacing", "0px");
                tbl.Attributes.Add("width", "100%");
                tbl.Attributes.Add("class", "MailPedidos");

                HtmlTableRow row = new HtmlTableRow();
                HtmlTableCell cellNombre = new HtmlTableCell();
                HtmlTableCell cellNroPedido = new HtmlTableCell();
                HtmlTableCell cellGrupo = new HtmlTableCell();
                HtmlTableCell cellFecha = new HtmlTableCell();
                HtmlTableCell cellCodigo = new HtmlTableCell();
                HtmlTableCell cellMonto = new HtmlTableCell();
                HtmlTableCell cellTransporte = new HtmlTableCell();

                #endregion

                #region cabecera Tabla
                row = new HtmlTableRow();

                cellFecha = new HtmlTableCell();
                cellFecha.InnerText = "Fecha Pedido";
                cellFecha.Attributes.Add("class", "Header");

                cellNroPedido = new HtmlTableCell();
                cellNroPedido.InnerText = "Nro Pedido";
                cellNroPedido.Attributes.Add("class", "Header");


                cellNombre = new HtmlTableCell();
                cellNombre.InnerText = "Apellido y Nombre";
                cellNombre.Attributes.Add("class", "Header");

                cellCodigo = new HtmlTableCell();
                cellCodigo.InnerText = "Código Cliente";
                cellCodigo.Attributes.Add("class", "Header");

                cellMonto = new HtmlTableCell();
                cellMonto.InnerText = "Monto";
                cellMonto.Attributes.Add("class", "Header");

                cellGrupo = new HtmlTableCell();
                cellGrupo.InnerText = "Grupo";
                cellGrupo.Attributes.Add("class", "Header");


                cellTransporte = new HtmlTableCell();
                cellTransporte.InnerText = "Transporte";
                cellTransporte.Attributes.Add("class", "Header");




                row.Cells.Add(cellFecha);
                row.Cells.Add(cellNroPedido);
                row.Cells.Add(cellNombre);
                row.Cells.Add(cellCodigo);
                row.Cells.Add(cellMonto);
                row.Cells.Add(cellGrupo);
                row.Cells.Add(cellTransporte);

                tbl.Rows.Add(row);

                #endregion

                #region Detalle de Pedidos
                decimal Total = 0;
                foreach (CabeceraPedido cab in item.Pedidos.OrderBy(w => w.FechaPedido))
                {
                    row = new HtmlTableRow();
                    cellNombre = new HtmlTableCell();
                    cellNombre.Attributes.Add("class", "Row");

                    cellNroPedido = new HtmlTableCell();
                    cellNroPedido.Attributes.Add("class", "Row");

                    cellGrupo = new HtmlTableCell();
                    cellGrupo.Attributes.Add("class", "Row");

                    cellFecha = new HtmlTableCell();
                    cellFecha.Attributes.Add("class", "Row");

                    cellCodigo = new HtmlTableCell();
                    cellCodigo.Attributes.Add("class", "Row");

                    cellMonto = new HtmlTableCell();
                    cellMonto.Attributes.Add("class", "Row");

                    cellTransporte = new HtmlTableCell();
                    cellTransporte.Attributes.Add("class", "Row");


                    cellNombre.InnerText = cab.objCliente.Nombre;
                    cellGrupo.InnerText = cab.objCliente.Clasif1;
                    cellNroPedido.InnerHtml = "<" + @"a href='ConsultaPedidos.aspx?NroPedido=" + cab.Nro.ToString() + "&p=" + Helper.Encriptar(cab.objClienteSolicitante.CodigoExterno + "|" + cab.Nro) + "' target='_blank' >" + cab.Nro.ToString() + "</a>";
                    cellFecha.InnerText = cab.FechaPedido.ToShortDateString();
                    cellCodigo.InnerText = cab.objCliente.CodigoExterno;
                    cellMonto.InnerText = cab.MontoTotal.ToString();

                    cellTransporte.InnerText = cboTransporte.Text;
                    //if (DetalleTrasporte.Any(w => w.Key == cab.IdCabeceraPedido))
                    //    cellTransporte.InnerText = DetalleTrasporte.First(w => w.Key == cab.IdCabeceraPedido).Value;
                    //else
                    //    cellTransporte.InnerText = "";


                    Total += cab.MontoTotal;


                    row.Cells.Add(cellFecha);
                    row.Cells.Add(cellNroPedido);
                    row.Cells.Add(cellNombre);
                    row.Cells.Add(cellCodigo);
                    row.Cells.Add(cellMonto);
                    row.Cells.Add(cellGrupo);
                    row.Cells.Add(cellTransporte);

                    tbl.Rows.Add(row);


                }

                #endregion

                #region Fila con el total de los pedido
                row = new HtmlTableRow();
                HtmlTableCell cell1 = new HtmlTableCell();
                cell1.InnerText = " ";
                cell1.ColSpan = 4;

                HtmlTableCell cellTotal = new HtmlTableCell();
                cellTotal.InnerText = Total.ToString();

                HtmlTableCell cell2 = new HtmlTableCell();
                cell2.InnerText = " ";

                row.Cells.Add(cell1);
                row.Cells.Add(cellTotal);
                row.Cells.Add(cell2);
                tbl.Rows.Add(row);

                #endregion

                StringWriter writer = new StringWriter();
                Html32TextWriter htmlWriter = new Html32TextWriter(writer);
                tbl.RenderControl(htmlWriter);
                string body = writer.ToString();

                #region Envio del mail

                MailsCabecera mail = new MailsCabecera();
                mail.Cuerpo = "<p>Estimado Líder, se informa que en el día de la fecha se ha procesado un total de: " + item.Pedidos.Count + " pedidos, con los siguientes datos:</p>" + body;
                mail.Fecha = DateTime.Now;
                mail.Subject = "Procesamiento de Pedidos";
                mail.Usuario = long.Parse(Session["IdUsuario"].ToString());



                MailsDestino mDestino = new MailsDestino();
                mDestino.MailsCabecera = mail;
                mDestino.Usuario = idLider; // 16464; // Usuario DEMO
                mDestino.Estado = EstadosMails.SINLEER;
                mDestino.FechaCambioEstado = DateTime.Now;
                mail.MailsDestinos.Add(mDestino);


                Contexto.MailsCabeceras.InsertOnSubmit(mail);



                #endregion
            }

        }

        Contexto.SubmitChanges();

    }

    private void EnviarMailMisiones()
    {
        if (EnviosMisiones.Count > 0)
        {
            #region Recupero el asistente nro 2, que es el destinatario del mail


            long IdAsistente = (from d in Contexto.Clientes
                                where d.Email.ToLower() == "asistente1@sandramarzzan.com.ar"
                                && d.TipoCliente == "INTERNO"
                                select d.IdCliente).FirstOrDefault();


            #endregion

            #region Definicion de la tabla
            HtmlTable tbl = new HtmlTable();
            tbl.Attributes.Add("cellpadding", "0px");
            tbl.Attributes.Add("cellspacing", "0px");
            tbl.Attributes.Add("width", "100%");
            tbl.Attributes.Add("class", "MailPedidos");

            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell cellNombre = new HtmlTableCell();
            HtmlTableCell cellNroPedido = new HtmlTableCell();
            HtmlTableCell cellFecha = new HtmlTableCell();
            HtmlTableCell cellMonto = new HtmlTableCell();

            #endregion

            #region cabecera Tabla
            row = new HtmlTableRow();

            cellFecha = new HtmlTableCell();
            cellFecha.InnerText = "Fecha Pedido";
            cellFecha.Attributes.Add("class", "Header");

            cellNroPedido = new HtmlTableCell();
            cellNroPedido.InnerText = "Nro Pedido";
            cellNroPedido.Attributes.Add("class", "Header");


            cellNombre = new HtmlTableCell();
            cellNombre.InnerText = "Apellido y Nombre";
            cellNombre.Attributes.Add("class", "Header");


            cellMonto = new HtmlTableCell();
            cellMonto.InnerText = "Monto";
            cellMonto.Attributes.Add("class", "Header");


            row.Cells.Add(cellFecha);
            row.Cells.Add(cellNroPedido);
            row.Cells.Add(cellNombre);
            row.Cells.Add(cellMonto);

            tbl.Rows.Add(row);

            #endregion

            #region Detalle de Pedidos
            decimal Total = 0;
            foreach (TempClass.tempEnviosMisiones cab in EnviosMisiones.OrderBy(w => w.FechaPedido))
            {
                row = new HtmlTableRow();
                cellNombre = new HtmlTableCell();
                cellNombre.Attributes.Add("class", "Row");

                cellNroPedido = new HtmlTableCell();
                cellNroPedido.Attributes.Add("class", "Row");

                cellFecha = new HtmlTableCell();
                cellFecha.Attributes.Add("class", "Row");

                cellMonto = new HtmlTableCell();
                cellMonto.Attributes.Add("class", "Row");


                cellNombre.InnerText = cab.Cliente;
                cellNroPedido.InnerHtml = cab.NroPedido.ToString();
                cellFecha.InnerText = cab.FechaPedido.ToShortDateString();
                cellMonto.InnerText = cab.MontoTotal.ToString();

                Total += cab.MontoTotal;


                row.Cells.Add(cellFecha);
                row.Cells.Add(cellNroPedido);
                row.Cells.Add(cellNombre);
                row.Cells.Add(cellMonto);

                tbl.Rows.Add(row);


            }

            #endregion

            #region Fila con el total de los pedido
            row = new HtmlTableRow();
            HtmlTableCell cell1 = new HtmlTableCell();
            cell1.InnerText = " ";
            cell1.ColSpan = 3;

            HtmlTableCell cellTotal = new HtmlTableCell();
            cellTotal.InnerText = Total.ToString();

            row.Cells.Add(cell1);
            row.Cells.Add(cellTotal);
            tbl.Rows.Add(row);

            #endregion

            StringWriter writer = new StringWriter();
            Html32TextWriter htmlWriter = new Html32TextWriter(writer);
            tbl.RenderControl(htmlWriter);
            string body = writer.ToString();

            #region Envio del mail

            MailsCabecera mail = new MailsCabecera();
            mail.Cuerpo = "<p>Estimado Asistente, se informa que en el día de la fecha se ha procesado un total de: " + EnviosMisiones.Count + " pedidos a misiones, con los siguientes datos:</p>" + body;
            mail.Fecha = DateTime.Now;
            mail.Subject = "Pedidos Misiones";
            mail.Usuario = long.Parse(Session["IdUsuario"].ToString());



            MailsDestino mDestino = new MailsDestino();
            mDestino.MailsCabecera = mail;
            mDestino.Usuario = IdAsistente;
            mDestino.Estado = EstadosMails.SINLEER;
            mDestino.FechaCambioEstado = DateTime.Now;
            mail.MailsDestinos.Add(mDestino);


            Contexto.MailsCabeceras.InsertOnSubmit(mail);
            Contexto.SubmitChanges();


            #endregion
        }

    }

    private void ActualizarDatosBejerman(List<CabeceraPedido> Pedidos)
    {
        string posibleError = "";

        try
        {
            //if (Pedidos.Any(w => w.Cliente == 16464)) /// Cliente DEMO
            //{

            #region Recupero los datos de conexion para la base de datos de Bejerman.
            posibleError = "Conectandose a Bejerman";

            Marzzan_InfolegacyDataContext dc_Web = new Marzzan_InfolegacyDataContext();

            Parametro ParamConexion = (from P in dc_Web.Parametros
                                       where P.Tipo == "CadenaConexionBejerman"
                                       select P).First<Parametro>();

            string _conexionBejerman = "";

            foreach (Parametro item in ParamConexion.ColHijos)
            {
                if (item.Tipo == "Servidor")
                {
                    _conexionBejerman += "Data Source=" + item.Valor;
                }
                else if (item.Tipo == "Source")
                {
                    _conexionBejerman += ";Initial Catalog=" + item.Valor;
                }
                else if (item.Tipo == "UserId")
                {
                    _conexionBejerman += ";Persist Security Info=True;User ID=" + item.Valor;
                }
                else
                {
                    _conexionBejerman += ";Password=" + item.Valor;
                }

            }

            Marzzan_BejermanDataContext dc_Berjeman = new Marzzan_BejermanDataContext();
            dc_Berjeman.Connection.ConnectionString = _conexionBejerman;

            #endregion

            #region Generacion de variables para proceso
            posibleError = "Consultado los datos de los pedidos";
            //List<CabeceraPedido> Pedidos = (from c in dc_Web.CabeceraPedidos
            //                                where c.IdCabeceraPedido >= 147004
            //                                select c).Take(60).ToList();


            List<long> idsPedidos = (from c in Pedidos
                                     select c.IdCabeceraPedido).ToList();

            List<string> codigosExternos = (from c in Pedidos
                                            select c.objCliente.CodigoExterno).Distinct().ToList();


            var codigosExternosDirecciones = (from c in Pedidos
                                              select new
                                              {
                                                  IdCabecera = c.IdCabeceraPedido,
                                                  CodigoDireccion = c.objDireccion.CodigoExternoDir
                                              }).Distinct().ToList();


            var detallesGastoEnvio = (from d in dc_Web.DetallePedidos
                                      where idsPedidos.Contains(d.objCabecera.IdCabeceraPedido)
                                      && d.objProducto.Tipo == 'G'
                                      select new
                                      {
                                          IdGastoEnvio = d.objProducto.IdProducto,
                                          IdCabecera = d.objCabecera.IdCabeceraPedido
                                      }
                                     ).ToList();


            var confTransportes = (from ct in dc_Web.ConfTransportes
                                   select new
                                   {
                                       Provincia = ct.Provincia.Trim().ToLower(),
                                       Localidad = ct.Localidad.Trim().ToLower(),
                                       FormaDePago = ct.FormaDePago.Trim().ToLower(),
                                       ct.IdProducto,
                                       ct.Transporte
                                   }).ToList();

            posibleError = "Consultado los datos de los clientes en bejerman, intento de conexion fallida.";

            var clientesBejerman = (from c in dc_Berjeman.Clientes
                                    where codigosExternos.Contains(c.cli_Cod)
                                    select c).ToList();

            #endregion

            #region Proceso de Actualización de datos

            StreamWriter _sw = new StreamWriter(Server.MapPath("") + "\\LogActualizacionDatosBejerman.txt", false);


            foreach (CabeceraPedido Pedido in Pedidos)
            {

                string datosPedido = "--------------- Código Cliente Bejerman: " + Pedido.objCliente.CodigoExterno + " Nombre: " + Pedido.objCliente.Nombre + "--------------- </br>" +
                "--------------- Pedido Nro: " + Pedido.Nro + "---------------";

                //if (Pedido.Cliente == 16464) /// Cliente DEMO
                //{
                posibleError = "Actualizar los datos en bejerman" + datosPedido;

                /// 1. Recupero el Codido del cliente en bejerman
                string codigoCliente = Pedido.objCliente.CodigoExterno;

                posibleError = "Recupero el transporte utilizado en el pedido" + datosPedido;
                /// 2. Recupero el transporte utilizo en el pedido
                string nombreTransporte = "";
                string idTransporte = "";
                long idGastoEnvio = 0;

                if (detallesGastoEnvio.Any(w => w.IdCabecera == Pedido.IdCabeceraPedido))
                    idGastoEnvio = detallesGastoEnvio.Where(w => w.IdCabecera == Pedido.IdCabeceraPedido).FirstOrDefault().IdGastoEnvio;

                string provincia = Pedido.objDireccion.Provincia.Trim().ToLower();
                string localidad = Pedido.objDireccion.Localidad.Trim().ToLower();
                string formadepago = Pedido.objFormaDePago.Descripcion.Trim().ToLower();

                var confTransporte = (from C in confTransportes
                                      where C.Provincia == provincia
                                      && C.Localidad == localidad
                                      && C.FormaDePago == formadepago
                                      && C.IdProducto == idGastoEnvio
                                      select C).FirstOrDefault();

                if (confTransporte != null)
                {
                    nombreTransporte = confTransporte.Transporte.Trim().ToLower();

                    /// Con el nombre del transportista busco en Bejerman el codigo que le correponde
                    idTransporte = (from c in dc_Berjeman.Transportes where c.trn_RazSoc.Trim().ToLower() == nombreTransporte select c.trn_Cod).FirstOrDefault();
                }



                posibleError = "Recupero el cliente de bejerman para actualizar los datos" + datosPedido;
                /// 3. Recupero el cliente de bejerman para actualizar los datos 
                /// de transporte y condicion de venta
                var clienteBejerman = (from c in clientesBejerman
                                       where c.cli_Cod == codigoCliente
                                       select c).FirstOrDefault();

                if (clienteBejerman != null)
                {
                    /// Actualizo la condicion de venta
                    if (Pedido.objFormaDePago.Codigo != "CON")
                        clienteBejerman.clicvt_Cod = "SIN";
                    else
                        clienteBejerman.clicvt_Cod = Pedido.objFormaDePago.Codigo;


                    /// Actualizo el transporte
                    if (idTransporte != "")
                    {
                        clienteBejerman.clitrn_Cod = idTransporte;
                    }
                    else
                    {
                        _sw.WriteLine("--------------- No se encontro el transporte ---------------");
                        _sw.WriteLine("--------------- Código Cliente Bejerman: " + Pedido.objCliente.CodigoExterno + " Nombre: " + Pedido.objCliente.Nombre + "---------------");
                        _sw.WriteLine("--------------- Pedido Nro: " + Pedido.Nro + "---------------");
                        _sw.WriteLine("Provincia: " + Pedido.objDireccion.Provincia);
                        _sw.WriteLine("Localidad: " + Pedido.objDireccion.Localidad);
                        _sw.WriteLine("Forma de Pago: " + Pedido.objFormaDePago.Descripcion);
                        _sw.WriteLine("Id Gasto Envio: " + idGastoEnvio);
                        _sw.WriteLine("----------------------------------------------------------------------------------------------------------------");
                        _sw.WriteLine("");
                    }
                }


                /// 4. Recupero las direcciones de entrega del cliente de bejerman
                /// y actualizo la dirección por defecto 
                /// según la dirección utiliza en el pedido.
                if (clienteBejerman != null)
                {
                    string codigoLugarEntrega = codigosExternosDirecciones.Where(w => w.IdCabecera == Pedido.IdCabeceraPedido).FirstOrDefault().CodigoDireccion;
                    bool seteoDirDefault = false;
                    LugarEnt direccionPrincipal = null;
                    foreach (LugarEnt LugarEntrega in clienteBejerman.LugarEnts)
                    {
                        /// guardo la dirección 1, que seria la principal para luego establecer esta por si hay algun
                        /// problema.
                        if (LugarEntrega.len_Cod.Trim() == "1")
                        {
                            direccionPrincipal = LugarEntrega;
                        }


                        if (LugarEntrega.len_ID.Trim() == codigoLugarEntrega.Trim())
                        {
                            LugarEntrega.len_EsDefault = true;
                            seteoDirDefault = true;
                        }
                        else
                            LugarEntrega.len_EsDefault = false;

                    }

                    /// Si no se seteo ninguna dirección por defecto, porque no la encontro y ademas
                    /// hay un dirección  principal, entonces pongo esta como por defaul.
                    if (!seteoDirDefault && direccionPrincipal != null)
                        direccionPrincipal.len_EsDefault = true;
                }
            }

            _sw.Flush();
            _sw.Close();

            #endregion

            dc_Berjeman.SubmitChanges();
            //}
        }
        catch (Exception err)
        {
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel2, typeof(UpdatePanel), "ActDatosBejerman", "alert('Error: " + posibleError + " - " + err.Message + "');", true);
        }
    }

    private PdfPTable GenerarPrimerTabla(iTextSharp.text.pdf.PdfContentByte cb, CabeceraPedido cab)
    {
        iTextSharp.text.pdf.PdfPTable ptable = new iTextSharp.text.pdf.PdfPTable(3);
        ptable.SetWidths(new float[] { 27, 55, 18 });
        ptable.DefaultCell.PaddingLeft = 4;
        ptable.DefaultCell.PaddingTop = 2;
        ptable.DefaultCell.PaddingBottom = 4;
        ptable.DefaultCell.BorderColor = BaseColor.BLACK;



        //Barcode128 code128 = new Barcode128();
        //code128.Code = cab.objCliente.CodigoExterno;
        //code128.BarHeight = 15;
        //code128.Size = 1;
        //ptable.AddCell(code128.CreateImageWithBarcode(cb, null, BaseColor.WHITE));

        ptable.AddCell(GetTableCodigosBarra(cab, cb));
        ptable.AddCell(GetTableDetalleCabecera(cab));
        ptable.AddCell(GetTableDetalleCabeceraPedido(cab));
        ptable.WidthPercentage = 100;




        ptable.Rows[0].GetCells()[0].UseVariableBorders = true;
        ptable.Rows[0].GetCells()[0].BorderColor = BaseColor.BLACK;
        ptable.Rows[0].GetCells()[0].BorderColorRight = BaseColor.WHITE;

        ptable.Rows[0].GetCells()[1].UseVariableBorders = true;
        ptable.Rows[0].GetCells()[1].BorderColor = BaseColor.BLACK;
        ptable.Rows[0].GetCells()[1].BorderColorLeft = BaseColor.WHITE;
        ptable.Rows[0].GetCells()[1].BorderColorRight = BaseColor.WHITE;

        ptable.Rows[0].GetCells()[2].UseVariableBorders = true;
        ptable.Rows[0].GetCells()[2].UseBorderPadding = true;
        ptable.Rows[0].GetCells()[2].BorderColor = BaseColor.BLACK;
        ptable.Rows[0].GetCells()[2].BorderColorLeft = BaseColor.WHITE;


        return ptable;
    }

    private PdfPTable GenerarSegundaTabla(iTextSharp.text.pdf.PdfContentByte cb, CabeceraPedido cab)
    {
        iTextSharp.text.pdf.PdfPTable ptable = new iTextSharp.text.pdf.PdfPTable(2);
        ptable.DefaultCell.PaddingLeft = 4;
        ptable.DefaultCell.PaddingTop = 2;
        ptable.DefaultCell.PaddingBottom = 1;


        ptable.AddCell(GetTableDetalleCabeceraFlete(cab));
        ptable.AddCell(GetTableDetalleCabeceraDireccion(cab, cb));

        ptable.WidthPercentage = 100;


        return ptable;
    }

    private List<DetallePedido> OrdenarDetalleSegunDistribucion(List<DetallePedido> detalles)
    {
        List<DetallePedido> detallesOrdenados = new List<DetallePedido>();

        List<string> allCodigosProductos = (from d in detalles
                                            select d.CodigoCompleto.Replace("-", "").Replace(" ", "").Trim()).ToList();

        List<DistribucionProducto> distribucionExistente = (from d in AlldistribucionExistente
                                                            where allCodigosProductos.Contains(d.CodigoProducto)
                                                            select d).ToList();



        List<string> productosExistentesCodigos = (from d in distribucionExistente select d.CodigoProducto).Distinct().ToList();


        /// Ordeno y Recorro los productos que tiene una distribución
        distribucionExistente = distribucionExistente.OrderByDescending(d => d.Deposito.Value).ThenBy(d => d.Pasillo.Value).ThenBy(d => d.Gondola.Value).ToList();
        foreach (DistribucionProducto item in distribucionExistente)
        {
            /// Antes de agregar los productos del detalle a la coleccion ordenada, verifico que los mismo no 
            /// hallan sido agregado anteriormente, ya que puede existir por error en la definicion de distribuciones duplicidad
            /// de definicion. es decir para un producto mas de una definicion para deposito, pasillo y gondola.
            if (!detallesOrdenados.Any(w => w.CodigoCompleto.Replace("-", "").Replace(" ", "").Trim() == item.CodigoProducto))
            {

                List<DetallePedido> dets = (from d in detalles
                                            where d.CodigoCompleto.Replace("-", "").Replace(" ", "").Trim() == item.CodigoProducto
                                            select d).ToList();

                detallesOrdenados.AddRange(dets);
            }
        }


        /// finalmente agrego los detalles que no poseen una distribución.
        detallesOrdenados.AddRange((from d in detalles
                                    where !productosExistentesCodigos.Contains(d.CodigoCompleto.Replace("-", "").Replace(" ", "").Trim())
                                    select d).ToList());

        return detallesOrdenados;
    }

    private void GenerarDetalleImpresion(iTextSharp.text.pdf.PdfContentByte cb, List<TempClass.TempAuditoriaImpresion> cabAgrupadas, iTextSharp.text.Document document)
    {
        nroLote = string.Format("{0:HHmmss}", DateTime.Now);
        DateTime fechaImpresion = DateTime.Now;

        #region Grabo los datos de auditoria

        CabeceraAuditoriaImpresione cab = new CabeceraAuditoriaImpresione();
        cab.Transporte = cabAgrupadas.FirstOrDefault().Transporte;
        cab.TotalPedidos = cabAgrupadas.Sum(w => w.Cantidad);
        cab.FechaInicial = txtFechaInicial.SelectedDate;
        cab.FechaFinal = txtFechaFinal.SelectedDate;
        cab.Lote = nroLote;
        cab.FechaImpresion = fechaImpresion;

        foreach (var item in cabAgrupadas)
        {
            foreach (var pedido in item.Pedidos)
            {
                DetalleAuditoriaImpresion det = new DetalleAuditoriaImpresion();
                det.objCabeceraAuditoriaImpresion = cab;
                det.Pedido = pedido;
                det.Grupo = item.Grupo;
                Contexto.DetalleAuditoriaImpresions.InsertOnSubmit(det);
            }

            Contexto.CabeceraAuditoriaImpresiones.InsertOnSubmit(cab);
        }


        #endregion

        #region Fuentes
        iTextSharp.text.Font font12 = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 12);
        iTextSharp.text.Font font25_Bold_Black = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 20, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font font12_Bold_Black = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font font30_Bold_Black = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 30, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        #endregion

        #region Genero la primer pagina de las impresiones

        PdfPTable ptable = new iTextSharp.text.pdf.PdfPTable(2);
        ptable.SetWidths(new float[] { 70, 30 });
        ptable.DefaultCell.BorderColor = BaseColor.BLUE;
        ptable.DefaultCell.PaddingLeft = 4;
        ptable.DefaultCell.PaddingTop = 0;
        ptable.DefaultCell.PaddingBottom = 4;
        ptable.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
        ptable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
        ptable.WidthPercentage = 95;

        iTextSharp.text.pdf.PdfPCell cell = new PdfPCell(new iTextSharp.text.Phrase("DETALLE CONTENIDO LOTE", font30_Bold_Black));
        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        cell.Colspan = 2;
        cell.Padding = 10;
        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        ptable.AddCell(cell);


        ptable.AddCell(new iTextSharp.text.Phrase("LOTE NRO:", font25_Bold_Black));
        ptable.AddCell(new iTextSharp.text.Phrase(nroLote, font12));

        ptable.AddCell(new iTextSharp.text.Phrase("TRANSPORTE:", font25_Bold_Black));
        ptable.AddCell(new iTextSharp.text.Phrase(cboTransporte.SelectedValue.ToUpper(), font12));

        ptable.AddCell(new iTextSharp.text.Phrase("TOTAL PEDIDOS LOTE", font25_Bold_Black));
        ptable.AddCell(new iTextSharp.text.Phrase(cabAgrupadas.Sum(w => w.Cantidad).ToString(), font12));

        ptable.AddCell(new iTextSharp.text.Phrase("FECHA IMPRESIÓN LOTE", font25_Bold_Black));
        ptable.AddCell(new iTextSharp.text.Phrase(fechaImpresion.ToString(), font12));

        ptable.AddCell(new iTextSharp.text.Phrase("RANGO IMPRESO", font25_Bold_Black));
        ptable.AddCell(new iTextSharp.text.Phrase(string.Format("{0:dd/MM/yyyy} a {1:dd/MM/yyyy}", txtFechaInicial.SelectedDate, txtFechaFinal.SelectedDate), font12));

        cell = new PdfPCell(new iTextSharp.text.Phrase(" ", font25_Bold_Black));
        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        cell.Colspan = 2;
        ptable.AddCell(cell);

        foreach (var item in cabAgrupadas.OrderBy(w => w.Grupo))
        {
            cell = new PdfPCell(new iTextSharp.text.Phrase(item.Grupo, font12_Bold_Black));
            cell.PaddingLeft = 120;
            ptable.AddCell(cell);
            ptable.AddCell(new iTextSharp.text.Phrase(item.Cantidad.ToString(), font12));

        }


        document.Add(ptable);

        #endregion
    }

    private void GenerarDetallePedido(iTextSharp.text.pdf.PdfContentByte cb, CabeceraPedido cab, iTextSharp.text.Document document)
    {
        #region Fuentes
        iTextSharp.text.Font font10 = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 12);
        iTextSharp.text.Font font10B = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD);
        iTextSharp.text.Font font10BRed = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD, BaseColor.RED);
        iTextSharp.text.Font font10BBlack = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        #endregion

        List<DetallePedido> DetallesOrdenado = OrdenarDetalleSegunDistribucion(cab.DetallePedidos.ToList());

        int RegActuales = 0;
        long cantidadProductos = 0;
        int CantRegPorPagina = 40;
        decimal TotalPedido = 0;
        do
        {
            ///// Genero la primer tabla del encabezado
            document.Add(GenerarPrimerTabla(cb, cab));

            ///// Genero la primer tabla del encabezado
            document.Add(GenerarSegundaTabla(cb, cab));

            /// Linea de espacio en blanco
            document.Add(new iTextSharp.text.Phrase(" ", font10B));


            document.Add(GenerarDetallePedidoPaginado(cb, DetallesOrdenado.Skip(RegActuales).Take(CantRegPorPagina).ToList(), cab.TipoPedido, ref cantidadProductos, ref TotalPedido, cab.IdCabeceraPedido));
            RegActuales += CantRegPorPagina;
            if (DetallesOrdenado.Skip(RegActuales).Take(CantRegPorPagina).Count() > 0)
            {
                document.NewPage();
            }


        } while (DetallesOrdenado.Skip(RegActuales).Take(CantRegPorPagina).Count() > 0);




        #region Pie - Fila con el Total del Pedido
        iTextSharp.text.pdf.PdfPTable ptable = new iTextSharp.text.pdf.PdfPTable(4);
        ptable.SetWidths(new float[] { 10, 60, 15, 15 });
        ptable.DefaultCell.BorderColor = BaseColor.BLUE;
        ptable.DefaultCell.PaddingLeft = 4;
        ptable.DefaultCell.PaddingTop = 0;
        ptable.DefaultCell.PaddingBottom = 4;
        ptable.DefaultCell.VerticalAlignment = 1;
        ptable.DefaultCell.HorizontalAlignment = 1;
        ptable.WidthPercentage = 95;



        ptable.AddCell(new iTextSharp.text.Phrase(cantidadProductos.ToString(), font10B));

        PdfPCell celVacia = new PdfPCell(new iTextSharp.text.Phrase("TOTAL EN PRODUCTOS", font10));
        celVacia.HorizontalAlignment = 2;
        celVacia.Colspan = 2;
        ptable.AddCell(celVacia);

        ptable.AddCell(new iTextSharp.text.Phrase(TotalPedido.ToString(), font10B));

        document.Add(ptable);

        #endregion

        #region Tabla Detalle TOTAL

        document.Add(new Paragraph(" "));/// Linea de espacio en blanco


        ptable = new iTextSharp.text.pdf.PdfPTable(5);
        ptable.SetWidths(new float[] { 20, 20, 20, 20, 20 });
        ptable.DefaultCell.BorderColor = BaseColor.BLUE;
        ptable.DefaultCell.PaddingLeft = 4;
        ptable.DefaultCell.PaddingTop = 0;
        ptable.DefaultCell.PaddingBottom = 4;
        ptable.DefaultCell.VerticalAlignment = 1;
        ptable.DefaultCell.HorizontalAlignment = 1;
        ptable.WidthPercentage = 95;


        ptable.AddCell(new iTextSharp.text.Phrase("SUBTOTAL", font10BBlack));
        ptable.AddCell(new iTextSharp.text.Phrase("IVA", font10BBlack));
        ptable.AddCell(new iTextSharp.text.Phrase("RG30/99", font10BBlack));
        ptable.AddCell(new iTextSharp.text.Phrase("RG2126/06", font10BBlack));
        ptable.AddCell(new iTextSharp.text.Phrase("TOTAL", font10BBlack));

        ptable.Rows[0].GetCells()[0].BackgroundColor = new BaseColor(System.Drawing.Color.SkyBlue);
        ptable.Rows[0].GetCells()[1].BackgroundColor = new BaseColor(System.Drawing.Color.SkyBlue);
        ptable.Rows[0].GetCells()[2].BackgroundColor = new BaseColor(System.Drawing.Color.SkyBlue);
        ptable.Rows[0].GetCells()[3].BackgroundColor = new BaseColor(System.Drawing.Color.SkyBlue);
        ptable.Rows[0].GetCells()[3].BackgroundColor = new BaseColor(System.Drawing.Color.SkyBlue);
        ptable.Rows[0].GetCells()[4].BackgroundColor = new BaseColor(System.Drawing.Color.SkyBlue);

        if (cab.DetalleImpuestos != null && cab.DetalleImpuestos != "")
        {
            /// Recupero del pedido el calculo de impuestos guardado
            string[] detalleImpuesto = cab.DetalleImpuestos.Split('@');

            string Neto = detalleImpuesto.Count() >= 1 ? detalleImpuesto[0] : "-";
            string Iva = detalleImpuesto.Count() >= 2 ? detalleImpuesto[1] : "-";
            string RG30 = detalleImpuesto.Count() >= 3 ? detalleImpuesto[2] : "-";
            string RG212 = detalleImpuesto.Count() >= 4 ? detalleImpuesto[3] : "-";
            string Totalfinal = detalleImpuesto.Count() >= 5 ? detalleImpuesto[4] : "-";

            ptable.AddCell(new iTextSharp.text.Phrase(Neto.ToString(), font10B));
            ptable.AddCell(new iTextSharp.text.Phrase(Iva.ToString(), font10B));
            ptable.AddCell(new iTextSharp.text.Phrase(RG30.ToString(), font10B));
            ptable.AddCell(new iTextSharp.text.Phrase(RG212.ToString(), font10B));
            ptable.AddCell(new iTextSharp.text.Phrase(Totalfinal.ToString(), font10B));
        }
        else
        {
            if (cab.FechaPedido.Date >= DateTime.Parse("15/11/2013"))
            {
                ptable.AddCell(new iTextSharp.text.Phrase("-", font10B));
                ptable.AddCell(new iTextSharp.text.Phrase("-", font10B));
                ptable.AddCell(new iTextSharp.text.Phrase("-", font10B));
                ptable.AddCell(new iTextSharp.text.Phrase("-", font10B));
            }
            else
            {
                PdfPCell cellError = new PdfPCell(new iTextSharp.text.Phrase("Pedido generado antes de los cambios: 14/11/2013", font10B));
                cellError.Colspan = 4;
                ptable.AddCell(cellError);
            }



            PdfPCell cellTotal = new PdfPCell(new iTextSharp.text.Phrase(cab.MontoTotal.ToString(), font10B));
            cellTotal.HorizontalAlignment = 1;
            ptable.AddCell(cellTotal);
        }

        document.Add(ptable);
        #endregion

        #region Imagenes de ATENCION

        /// Se imprime una imagen para resaltar la condicion impositva del 
        /// cliente de la nota. 31/10/2013
        if (cab.objCliente.Cod_SitIVA == "7") //Sujeto no Categorizado
        {
            float height = cab.DetallePedidos.Count() <= CantRegPorPagina ? cab.DetallePedidos.Count() * 20 : CantRegPorPagina * 20;
            iTextSharp.text.Rectangle recMarca = new Rectangle(180, 380);
            string ruta = Server.MapPath("Imagenes") + "\\" + "AtencionSNC.png";
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(ruta);
            img.ScaleToFit(recMarca.Width, recMarca.Height);

            Single X = ((document.PageSize.Width - img.Width) / 2) - 25;
            Single Y = 680 - (height / 2);
            img.SetAbsolutePosition(X, Y);
            img.RotationDegrees = 30;
            cb.AddImage(img);
        }
        else if (cab.objCliente.Cod_SitIVA == "1") // Responsable Inscripto
        {
            float height = cab.DetallePedidos.Count() <= CantRegPorPagina ? cab.DetallePedidos.Count() * 20 : CantRegPorPagina * 20;
            iTextSharp.text.Rectangle recMarca = new Rectangle(180, 380);
            string ruta = Server.MapPath("Imagenes") + "\\" + "AtencionRI.png";
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(ruta);
            img.ScaleToFit(recMarca.Width, recMarca.Height);

            Single X = ((document.PageSize.Width - img.Width) / 2) - 25;
            Single Y = 680 - (height / 2);
            img.SetAbsolutePosition(X, Y);
            img.RotationDegrees = 30;
            cb.AddImage(img);
        }
        else if (cab.objCliente.Cod_SitIVA == "6") // Monotributista
        {
            float height = cab.DetallePedidos.Count() <= CantRegPorPagina ? cab.DetallePedidos.Count() * 20 : CantRegPorPagina * 20;
            iTextSharp.text.Rectangle recMarca = new Rectangle(180, 380);
            string ruta = Server.MapPath("Imagenes") + "\\" + "AtencionMonotributista.png";
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(ruta);
            img.ScaleToFit(recMarca.Width, recMarca.Height);

            Single X = ((document.PageSize.Width - img.Width) / 2) - 25;
            Single Y = 680 - (height / 2);
            img.SetAbsolutePosition(X, Y);
            img.RotationDegrees = 30;
            cb.AddImage(img);
        }

        /// Si el cliente posee mas de un pedido en el lote que se esta imprimiendo
        /// entonces agrego una leyenda informando de la situación.
        if (ClientesCantidadPedidos.Count(w => w.Key == cab.Cliente) > 0)
        {
            document.Add(new Paragraph(" "));/// Linea de espacio en blanco


            ptable = new iTextSharp.text.pdf.PdfPTable(1);
            ptable.SetWidths(new float[] { 1000 });
            ptable.DefaultCell.BorderColor = BaseColor.RED;
            ptable.DefaultCell.PaddingLeft = 4;
            ptable.DefaultCell.PaddingTop = 0;
            ptable.DefaultCell.PaddingBottom = 4;
            ptable.DefaultCell.VerticalAlignment = 1;
            ptable.DefaultCell.HorizontalAlignment = 1;
            ptable.WidthPercentage = 95;

            PdfPCell celAlerta = new PdfPCell(new iTextSharp.text.Phrase("ATENCION ESTE CLIENTE POSEE " + ClientesCantidadPedidos.FirstOrDefault(w => w.Key == cab.Cliente).Value.ToString() + " PEDIDOS EN EL LOTE ACTUAL", font10BRed));
            celAlerta.HorizontalAlignment = 1;
            celAlerta.Colspan = 1;
            ptable.AddCell(celAlerta);

            document.Add(ptable);


            //float height = cab.DetallePedidos.Count() <= CantRegPorPagina ? cab.DetallePedidos.Count() * 20 : CantRegPorPagina * 20;
            //iTextSharp.text.Rectangle recMarca = new Rectangle(180, 380);
            //string ruta = Server.MapPath("Imagenes") + "\\" + "AtencionPDF.png";
            //iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(ruta);
            //img.ScaleToFit(recMarca.Width, recMarca.Height);

            //Single X = ((document.PageSize.Width - img.Width) / 2) - 25;
            //Single Y = 480 - (height / 2);
            //img.SetAbsolutePosition(X, Y);
            //img.RotationDegrees = 30;
            //document.Add(img);

        }

        /// Solo para el caso de pedidos para la provincia de misione se le agrega una
        /// marca de agua para indicar que el mismo pertenece a esta provincia.
        /// Cambio solicitado el 29/07/2013
        if (cab.objDireccion.Provincia.ToLower() == "misiones")
        {
            float height = cab.DetallePedidos.Count() <= CantRegPorPagina ? cab.DetallePedidos.Count() * 20 : CantRegPorPagina * 20;
            iTextSharp.text.Rectangle recMarca = new Rectangle(180, 380);
            string ruta = Server.MapPath("Imagenes") + "\\" + "AtencionMisiones.png";
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(ruta);
            img.ScaleToFit(recMarca.Width, recMarca.Height);

            Single X = ((document.PageSize.Width - img.Width) / 2) - 25;
            Single Y = 600 - (height / 2);
            img.SetAbsolutePosition(X, Y);
            img.RotationDegrees = 30;
            cb.AddImage(img);

            /// guardo en la lista de envios a misiones el pedido correspondiente a misiones
            /// pera luego enviar el mail al asistente2
            TempClass.tempEnviosMisiones envio = new TempClass.tempEnviosMisiones();
            envio.Cliente = cab.objCliente.CodigoExterno + " - " + cab.objCliente.Nombre;
            envio.FechaPedido = cab.FechaPedido;
            envio.MontoTotal = cab.MontoTotal;
            envio.NroPedido = cab.Nro;
            EnviosMisiones.Add(envio);
        }

        #endregion



        #region COMENTADO - -Descriminacion de Impuestos - REEMPLAZADO POR LA TABLA DETALLE TOTAL

        ///// Fila con el Detalla según la situación del IVA
        //if (TotalPedido >= 600 && cab.objCliente.Cod_SitIVA == "7") //Sujeto no Categorizado
        //{

        //    /// fila con el valor total - el impuesto
        //    celVacia = new PdfPCell(new iTextSharp.text.Phrase("NETO", font10));
        //    celVacia.HorizontalAlignment = 2;
        //    celVacia.Colspan = 3;
        //    ptable.AddCell(celVacia);

        //    decimal TotalImpositivo = Math.Round((TotalPedido / decimal.Parse("1,105")), 1);
        //    ptable.AddCell(new iTextSharp.text.Phrase(string.Format("{0:0.00}", TotalImpositivo), font10B));


        //    // fila con el impuesto solamente
        //    celVacia = new PdfPCell(new iTextSharp.text.Phrase("IMPUESTO", font10));
        //    celVacia.HorizontalAlignment = 2;
        //    celVacia.Colspan = 3;
        //    ptable.AddCell(celVacia);

        //    decimal Impusto = TotalPedido - TotalImpositivo;
        //    ptable.AddCell(new iTextSharp.text.Phrase(string.Format("{0:0.00}", Impusto), font10));
        //}
        //else if (cab.objCliente.Cod_SitIVA == "1") //Inscripto
        //{
        //    /// fila con el valor total - el impuesto
        //    celVacia = new PdfPCell(new iTextSharp.text.Phrase("NETO", font10));
        //    celVacia.HorizontalAlignment = 2;
        //    celVacia.Colspan = 3;
        //    ptable.AddCell(celVacia);

        //    decimal TotalImpositivo = Math.Round((TotalPedido / decimal.Parse("1,21")), 1);
        //    ptable.AddCell(new iTextSharp.text.Phrase(string.Format("{0:0.00}", TotalImpositivo), font10));


        //    // fila con el impuesto solamente
        //    celVacia = new PdfPCell(new iTextSharp.text.Phrase("IMPUESTO", font10));
        //    celVacia.HorizontalAlignment = 2;
        //    celVacia.Colspan = 3;
        //    ptable.AddCell(celVacia);

        //    decimal Impusto = TotalPedido - TotalImpositivo;
        //    ptable.AddCell(new iTextSharp.text.Phrase(string.Format("{0:0.00}", Impusto), font10));

        //}

        #endregion

    }

    private PdfPTable GenerarDetallePedidoPaginado(iTextSharp.text.pdf.PdfContentByte cb, List<DetallePedido> detalles, string TipoPedido,
        ref long cantidadProducto, ref decimal TotalProductos, long idCabeceraPedido)
    {
        iTextSharp.text.Font font9 = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10);
        iTextSharp.text.Font font10 = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 12);
        iTextSharp.text.Font font10B = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD);
        iTextSharp.text.Font font10BBlack = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

        iTextSharp.text.pdf.PdfPTable ptable = new iTextSharp.text.pdf.PdfPTable(4);
        ptable.SetWidths(new float[] { 10, 65, 10, 15 });
        ptable.HeaderRows = 1;
        ptable.DefaultCell.BorderColor = BaseColor.BLUE;
        ptable.DefaultCell.PaddingLeft = 4;
        ptable.DefaultCell.PaddingTop = 2;
        ptable.DefaultCell.PaddingBottom = 2;
        ptable.DefaultCell.VerticalAlignment = 1;
        ptable.DefaultCell.HorizontalAlignment = 1;
        ptable.WidthPercentage = 95;

        #region Encabezado tabla

        ptable.AddCell(new iTextSharp.text.Phrase("Cantidad", font10BBlack));
        ptable.AddCell(new iTextSharp.text.Phrase("Nombre Producto", font10BBlack));
        ptable.AddCell(new iTextSharp.text.Phrase("V. U.", font10BBlack));
        ptable.AddCell(new iTextSharp.text.Phrase("Valor Total", font10BBlack));

        ptable.Rows[0].GetCells()[0].BackgroundColor = new BaseColor(System.Drawing.Color.SkyBlue);
        ptable.Rows[0].GetCells()[1].BackgroundColor = new BaseColor(System.Drawing.Color.SkyBlue);
        ptable.Rows[0].GetCells()[2].BackgroundColor = new BaseColor(System.Drawing.Color.SkyBlue);
        ptable.Rows[0].GetCells()[3].BackgroundColor = new BaseColor(System.Drawing.Color.SkyBlue);

        #endregion

        #region Detalle
        int i = 0;
        long cantidadProductolocal = 0;
        decimal TotalProductoslocal = 0;

        //if (detalles.FirstOrDefault() != null && ImprimirRemitos)
        //{
        //    var remitos = (from r in Contexto.RemitosAfectados
        //                   where r.objCabeceraPedido.IdCabeceraPedido == idCabeceraPedido
        //                   select r);

        //    foreach (RemitosAfectados remito in remitos)
        //    {
        //        //ptable.AddCell(new iTextSharp.text.Phrase(remito.NroRemito + " - " + remito.CodArticulo + " - " + remito.DescArticulo, font10));

        //        ptable.AddCell(new iTextSharp.text.Phrase(string.Format("{0:0}", remito.Cantidad), font10));
        //        ptable.AddCell(new iTextSharp.text.Phrase(remito.DescArticulo + "(" + remito.CodArticulo + ")", font9));
        //        ptable.AddCell(new iTextSharp.text.Phrase((remito.Precio * -1).ToString(), font10));
        //        ptable.AddCell(new iTextSharp.text.Phrase((remito.Precio * -1).ToString(), font10));
        //        TotalProductoslocal += (remito.Precio * -1);
        //        ptable.Rows[i + 1].GetCells()[1].HorizontalAlignment = Element.ALIGN_LEFT;
        //        i++;
        //    }
        //}

        foreach (DetallePedido item in detalles)
        {
            if (item.objProducto.Tipo == 'P' || item.objProducto.Tipo == 'A'
                || item.objProducto.Tipo == 'R' || item.objProducto.Tipo == 'I'
                || item.objProducto.Tipo == 'D' || item.objProducto.Tipo == 'N')
            {

                // Si los tipo son RT o ND es porque se trata de un remito o nota de debito que proviene 
                /// del regalo de tu líder
                if (TipoPedido == "RT" || TipoPedido == "ND")
                {
                    ptable.AddCell(new iTextSharp.text.Phrase(item.Cantidad.ToString(), font10));
                    cantidadProductolocal += item.Cantidad.Value;

                    if (item.ValorUnitario >= 0)
                        ptable.AddCell(new iTextSharp.text.Phrase(GetDescripcionCompleta(item), font10));
                    else
                        ptable.AddCell(new iTextSharp.text.Phrase(GetDescripcionCompleta(item), font10B));


                    ptable.AddCell(new iTextSharp.text.Phrase(item.ValorUnitario.ToString(), font10));
                    ptable.AddCell(new iTextSharp.text.Phrase(item.ValorTotal.Value.ToString(), font10));
                    TotalProductoslocal += item.ValorTotal.Value;
                    ptable.Rows[i + 1].GetCells()[1].HorizontalAlignment = Element.ALIGN_LEFT;
                    i++;
                }
                else
                {
                    /// Para los otros casos: se trata de un remito de la cuenta bolsos
                    /// o un pedido normal, entonces debo contar los productos normales


                    if (item.objProducto.Tipo == 'A')
                        cantidadProductolocal += item.Cantidad.Value;


                    if (item.ValorUnitario >= 0)
                    {
                        ptable.AddCell(new iTextSharp.text.Phrase(item.Cantidad.ToString(), font10));
                        ptable.AddCell(new iTextSharp.text.Phrase(GetDescripcionCompleta(item), font10));
                        ptable.AddCell(new iTextSharp.text.Phrase(item.ValorUnitario.ToString(), font10));
                        ptable.AddCell(new iTextSharp.text.Phrase(item.ValorTotal.Value.ToString(), font10));
                    }
                    else
                    {
                        ptable.AddCell(new iTextSharp.text.Phrase(item.Cantidad.ToString(), font10));
                        ptable.AddCell(GenerarDetallePromocion(GetDescripcionCompleta(item), item.objPresentacion.CodigoBarras, font10B, cb));
                        ptable.AddCell(new iTextSharp.text.Phrase(item.ValorUnitario.ToString(), font10));
                        ptable.AddCell(new iTextSharp.text.Phrase(item.ValorTotal.Value.ToString(), font10));

                    }

                    TotalProductoslocal += item.ValorTotal.Value;

                    ptable.Rows[i + 1].GetCells()[1].HorizontalAlignment = Element.ALIGN_LEFT;
                    i++;

                }



            }

        }

        #endregion

        cantidadProducto += cantidadProductolocal;
        TotalProductos += TotalProductoslocal;


        return ptable;
    }

    private PdfPTable GenerarDetallePromocion(string descripcion, string codigo, iTextSharp.text.Font font10B, iTextSharp.text.pdf.PdfContentByte cb)
    {
        try
        {
            if (codigo == null) { codigo = ""; }

            iTextSharp.text.pdf.PdfPTable ptable = new iTextSharp.text.pdf.PdfPTable(1);
            ptable.DefaultCell.BorderColor = BaseColor.WHITE;
            ptable.DefaultCell.BorderWidth = 1;
            ptable.DefaultCell.PaddingLeft = 0;
            ptable.DefaultCell.PaddingTop = 0;
            ptable.DefaultCell.PaddingBottom = 0;
            ptable.SetWidths(new float[] { 95});
            ptable.HorizontalAlignment = Rectangle.ALIGN_LEFT;

            ptable.AddCell(new iTextSharp.text.Phrase(descripcion, font10B));

            ///Solicitado por Miguel (13/08/2014)
            
            //Barcode128 code128 = new Barcode128();
            //code128.Code = codigo;
            //code128.StartStopText = false;
            //code128.BarHeight = 10;
            //code128.X = 0.5f;
            //ptable.AddCell(new iTextSharp.text.Phrase(new iTextSharp.text.Chunk(code128.CreateImageWithBarcode(cb, null, null), 0, 0)));
            //ptable.Rows[0].GetCells()[1].HorizontalAlignment = Element.ALIGN_CENTER;
            //ptable.Rows[0].GetCells()[1].VerticalAlignment = Element.ALIGN_MIDDLE;

            return ptable;

        }
        catch
        {
            return null;
        }
    }

    private PdfPTable GetTableCodigosBarra(CabeceraPedido cab, iTextSharp.text.pdf.PdfContentByte cb)
    {

        iTextSharp.text.pdf.PdfPTable ptable = new iTextSharp.text.pdf.PdfPTable(1);
        ptable.DefaultCell.BorderColor = BaseColor.WHITE;
        ptable.DefaultCell.Padding = 0;

        Barcode128 code128 = new Barcode128();
        code128.Code = cab.objCliente.CodigoExterno;
        code128.BarHeight = 10;
        code128.Size = 4;
        ptable.AddCell(code128.CreateImageWithBarcode(cb, null, BaseColor.BLACK));

        /// 31/07/2014: Se solicito que no se imprima mas el codigo correspondiente al cliente en el fical.
        //code128 = new Barcode128();
        //code128.Code = Convert.ToString(long.Parse(cab.objCliente.CodigoExterno) + 600000);
        //code128.BarHeight = 10;
        //code128.Size = 4;
        //ptable.AddCell(code128.CreateImageWithBarcode(cb, null, BaseColor.BLACK));
        //ptable.WidthPercentage = 90;
        return ptable;

    }

    private PdfPTable GetTableDetalleCabecera(CabeceraPedido cab)
    {
        iTextSharp.text.Font font10 = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10);
        iTextSharp.text.Font font10B = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD);
        iTextSharp.text.Font font12B = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD);

        iTextSharp.text.pdf.PdfPTable ptable = new iTextSharp.text.pdf.PdfPTable(2);
        ptable.SetWidths(new float[] { 25, 75 });
        ptable.DefaultCell.BorderColor = BaseColor.WHITE;
        ptable.DefaultCell.PaddingLeft = 7;
        ptable.DefaultCell.PaddingTop = 0;
        ptable.DefaultCell.PaddingBottom = 4;


        string Transportista = "PROXIMO ENVIO";

        DetallePedido DetalleGastoEnvio = (from P in cab.DetallePedidos
                                           where P.objProducto.Tipo == 'G'
                                           select P).FirstOrDefault<DetallePedido>();
        if (DetalleGastoEnvio != null)
        {

            ConfTransporte confTransporte = (from C in Contexto.ConfTransportes
                                             where C.Provincia == cab.objDireccion.Provincia &&
                                             C.Localidad == cab.objDireccion.Localidad &&
                                             C.FormaDePago == cab.objFormaDePago.Descripcion &&
                                             C.IdProducto == DetalleGastoEnvio.objProducto.IdProducto
                                             select C).FirstOrDefault<ConfTransporte>();

            if (confTransporte != null)
            {
                Transportista = confTransporte.Transporte;
            }

        }


        ptable.AddCell(new iTextSharp.text.Phrase("Consultor:", font10));
        ptable.AddCell(new iTextSharp.text.Phrase(cab.objCliente.Nombre, font10B));

        ptable.AddCell(new iTextSharp.text.Phrase("Código Cliente:", font10));
        ptable.AddCell(new iTextSharp.text.Phrase(cab.objCliente.CodigoExterno, font10B));

        ptable.AddCell(new iTextSharp.text.Phrase("F. Pedido:", font10));
        ptable.AddCell(new iTextSharp.text.Phrase(cab.FechaPedido.ToShortDateString(), font10B));

        ptable.AddCell(new iTextSharp.text.Phrase("F. Impresión:", font10));
        ptable.AddCell(new iTextSharp.text.Phrase(DateTime.Now.ToShortDateString(), font10B));

        ptable.AddCell(new iTextSharp.text.Phrase("Situación IVA:", font10));
        ptable.AddCell(new iTextSharp.text.Phrase(cab.objCliente.Desc_SitIVA, font10B));

        ptable.AddCell(new iTextSharp.text.Phrase("Transporte:", font10));
        ptable.AddCell(new iTextSharp.text.Phrase(Transportista, font12B));

        ptable.WidthPercentage = 100;
        return ptable;

    }

    private PdfPTable GetTableDetalleCabeceraPedido(CabeceraPedido cab)
    {
        iTextSharp.text.Font font7 = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 7);
        iTextSharp.text.Font font10B = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD);
        iTextSharp.text.Font font20B = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 20, iTextSharp.text.Font.BOLD, BaseColor.RED);
        iTextSharp.text.Font font9B = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD);

        iTextSharp.text.pdf.PdfPTable ptable = new iTextSharp.text.pdf.PdfPTable(1);
        ptable.DefaultCell.BorderColor = BaseColor.WHITE;
        ptable.DefaultCell.PaddingLeft = 4;
        ptable.DefaultCell.PaddingTop = 0;
        ptable.DefaultCell.PaddingBottom = 4;
        ptable.DefaultCell.HorizontalAlignment = 1;

        ptable.AddCell(new iTextSharp.text.Phrase(cab.TipoPedido, font10B));

        ptable.AddCell(new iTextSharp.text.Phrase(cab.Nro, font20B));

        ptable.AddCell(new iTextSharp.text.Phrase("IMPRESIÓN NRO " + cab.NroImpresion.ToString(), font9B));

        ptable.AddCell(new iTextSharp.text.Phrase(Session["NombreUsuario"].ToString(), font7));

        ptable.AddCell(new iTextSharp.text.Phrase("Lote: " + nroLote, font9B));

        ptable.WidthPercentage = 100;
        return ptable;

    }

    private PdfPTable GetTableDetalleCabeceraDireccion(CabeceraPedido cab, iTextSharp.text.pdf.PdfContentByte cb)
    {
        iTextSharp.text.Font font10 = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10);
        iTextSharp.text.Font font10B = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD);
        iTextSharp.text.Font font7B = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 7, iTextSharp.text.Font.BOLD);

        iTextSharp.text.pdf.PdfPTable ptable = new iTextSharp.text.pdf.PdfPTable(1);
        ptable.DefaultCell.BorderColor = BaseColor.WHITE;
        ptable.DefaultCell.PaddingLeft = 4;
        ptable.DefaultCell.PaddingTop = 0;
        ptable.DefaultCell.PaddingBottom = 1;




        ptable.AddCell(new iTextSharp.text.Phrase(cab.objDireccion.Calle, font10));

        ptable.AddCell(new iTextSharp.text.Phrase(cab.objDireccion.Localidad, font10));

        ptable.AddCell(new iTextSharp.text.Phrase(cab.objDireccion.Provincia, font10));





        /// Genero la linea con el codigo de barra del Gasto de envio
        DetallePedido DetalleGastoEnvio = (from P in cab.DetallePedidos
                                           where P.objProducto.Tipo == 'G'
                                           select P).FirstOrDefault<DetallePedido>();

        if (DetalleGastoEnvio != null)
        {
            if (DetalleGastoEnvio.objPresentacion.CodigoBarras != null)
            {
                Barcode128 code128 = new Barcode128();
                code128.Code = DetalleGastoEnvio.objPresentacion.CodigoBarras;
                code128.AltText = "Código Barra Gasto: " + DetalleGastoEnvio.objPresentacion.CodigoBarras;
                code128.BarHeight = 10;
                code128.Size = 5;
                ptable.AddCell(code128.CreateImageWithBarcode(cb, null, null));
                ptable.Rows[3].GetCells()[0].FixedHeight = 30;
                ptable.Rows[3].GetCells()[0].HorizontalAlignment = 1;
            }

        }


        ptable.WidthPercentage = 100;
        return ptable;

    }

    private PdfPTable GetTableDetalleCabeceraFlete(CabeceraPedido cab)
    {
        iTextSharp.text.Font font10 = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10);
        iTextSharp.text.Font font7 = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 7);
        iTextSharp.text.Font font10B = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD);

        iTextSharp.text.pdf.PdfPTable ptable = new iTextSharp.text.pdf.PdfPTable(2);
        ptable.SetWidths(new float[] { 25, 65 });
        ptable.DefaultCell.BorderColor = BaseColor.WHITE;
        ptable.DefaultCell.PaddingLeft = 4;
        ptable.DefaultCell.PaddingTop = 0;
        ptable.DefaultCell.PaddingBottom = 1;

        string CostoFlete = "0";


        DetallePedido DetalleGastoEnvio = (from P in cab.DetallePedidos
                                           where P.objProducto.Tipo == 'G'
                                           select P).FirstOrDefault<DetallePedido>();
        if (DetalleGastoEnvio != null)
        {
            CostoFlete = DetalleGastoEnvio.objPresentacion.Precio.Value.ToString();
        }

        ptable.AddCell(new iTextSharp.text.Phrase("Costo Flete:", font10));
        ptable.AddCell(new iTextSharp.text.Phrase(CostoFlete, font10B));
        _CostoFlete = decimal.Parse(CostoFlete);

        ptable.AddCell(new iTextSharp.text.Phrase("Des. Prov.:", font10));
        ptable.AddCell(new iTextSharp.text.Phrase(cab.DescuentoProvincia.ToString(), font10B));

        ptable.AddCell(new iTextSharp.text.Phrase("Des. Gral.:", font10));
        ptable.AddCell(new iTextSharp.text.Phrase(cab.DescuentosGenerales.ToString(), font10B));

        ptable.AddCell(new iTextSharp.text.Phrase("Líder:", font7));
        ptable.AddCell(new iTextSharp.text.Phrase(cab.objCliente.Vendedor, font7));

        ptable.AddCell(new iTextSharp.text.Phrase("Grupo:", font7));
        ptable.AddCell(new iTextSharp.text.Phrase(cab.objClienteSolicitante.Clasif1, font7));

        ptable.AddCell(new iTextSharp.text.Phrase("FP:", font10B));
        ptable.AddCell(new iTextSharp.text.Phrase(cab.objFormaDePago.Descripcion.ToUpper(), font10B));





        ptable.WidthPercentage = 100;
        return ptable;

    }

    private PdfPTable GenerarDetalleRemitos(CabeceraPedido cab)
    {

        iTextSharp.text.Font font10 = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10);
        iTextSharp.text.Font font10B = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD);

        iTextSharp.text.pdf.PdfPTable ptable = new iTextSharp.text.pdf.PdfPTable(1);
        ptable.DefaultCell.PaddingLeft = 4;
        ptable.DefaultCell.PaddingTop = 0;
        ptable.DefaultCell.PaddingBottom = 4;

        var remitos = (from r in Contexto.RemitosAfectados
                       where r.objCabeceraPedido.IdCabeceraPedido == cab.IdCabeceraPedido
                       select r);

        foreach (RemitosAfectados remito in remitos)
        {
            ptable.AddCell(new iTextSharp.text.Phrase(remito.NroRemito + " - " + remito.CodArticulo + " - " + remito.DescArticulo, font10));
        }

        ptable.WidthPercentage = 70;
        return ptable;

    }

    private PdfPTable GenerarDetalleObservacion(CabeceraPedido cab)
    {

        iTextSharp.text.Font font10 = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10);
        iTextSharp.text.Font font10B = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD);

        iTextSharp.text.pdf.PdfPTable ptable = new iTextSharp.text.pdf.PdfPTable(1);
        ptable.DefaultCell.BorderColor = BaseColor.WHITE;
        ptable.DefaultCell.PaddingLeft = 4;
        ptable.DefaultCell.PaddingTop = 0;
        ptable.DefaultCell.PaddingBottom = 4;


        ptable.AddCell(new iTextSharp.text.Phrase(cab.Retira, font10));


        ptable.WidthPercentage = 80;
        return ptable;

    }

    private string GetDescripcionCompleta(DetallePedido detalle)
    {
        try
        {
            if (detalle.objProducto.Tipo == 'P' || detalle.objProducto.Tipo == 'R' || detalle.objProducto.Tipo == 'G'
                         || detalle.objProducto.Tipo == 'D' || detalle.objProducto.Tipo == 'I' || detalle.objProducto.Tipo == 'N')
            {
                return detalle.objPresentacion.objProducto.Descripcion;
            }
            else
            {
                if (detalle.objPresentacion.objProducto.DescripcionCompleta.ToLower().Contains("incorporac"))
                {
                    return detalle.objPresentacion.objProducto.Descripcion + " x " + detalle.objPresentacion.Descripcion;
                }
                else
                {
                    if (detalle.objPresentacion.objProducto.objPadre.objPadre.Codigo == "02")
                    {
                        return detalle.objPresentacion.objProducto.objPadre.Descripcion + " " + detalle.objPresentacion.objProducto.DescripcionCompleta + detalle.objPresentacion.Descripcion;
                    }
                    else
                        return detalle.objPresentacion.objProducto.DescripcionCompleta + detalle.objPresentacion.Descripcion;
                }
            }

        }
        catch
        {
            return "";
        }

    }

    /// <summary>
    /// Se utiliza para determinar si los remitos afectos fueron incluidos dentro de la nota de 
    /// pedido como items del detalle o no.
    /// </summary>
    /// <param name="cab"></param>
    /// <returns></returns>
    private bool ExistenRemitosEnDetalle(CabeceraPedido cab)
    {

        var remitos = (from r in Contexto.RemitosAfectados
                       where r.objCabeceraPedido.IdCabeceraPedido == cab.IdCabeceraPedido
                       select r.CodArticulo.Trim()).ToList();

        /// Si la nota posee remitos afectodos evaluo si estan o no dentro 
        /// del detalle del pedido.
        if (remitos.Count > 0)
        {
            if (cab.DetallePedidos.Any(w => remitos.Contains(w.CodigoCompleto.Trim())))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        else
        {
            /// si no hay remitos afectados devuelvo true como si ubiera 
            /// y los mismos estubieran dentro del detalle el pedido
            /// para que no se dibuje la tabla vacia.
            return true;
        }

    }
    #endregion


    [WebMethod(EnableSession = true)]
    public static object ConsultarPedidos(string grupo, string cliente, string transporte, string FechaInicial, string FechaFinal, bool impresas)
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        DateTime FI = Convert.ToDateTime(FechaInicial);
        DateTime FF = Convert.ToDateTime(FechaFinal);

        using (Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext())
        {
            List<View_PedidosParaImpresion> Resultado = new List<View_PedidosParaImpresion>();
            List<CabeceraPedido> cabeceras = new List<CabeceraPedido>();
            Cliente clienteCtaBolsos = new Cliente();
            List<Cliente> consultores = new List<Cliente>();
            List<long> IdsConsultores = new List<long>();
            int IncluirImpresas = impresas ? 100 : 0;

            if (grupo != null)
            {

                IdsConsultores = (from C in dc.Clientes
                                  where C.Habilitado.Value &&
                                  (C.Clasif1 == grupo)
                                  orderby C.Nombre
                                  select C.IdCliente).ToList<long>();


                Resultado = (from v in dc.View_PedidosParaImpresions
                             where (v.FechaPedido.Date >= FI && v.FechaPedido.Date <= FF)
                             && v.Transporte == transporte.ToUpper() && IdsConsultores.Contains(v.Cliente)
                             && (v.NroImpresion == 0 || v.NroImpresion <= IncluirImpresas)
                             select v).ToList();

            }
            else if (cliente != null)
            {
                Resultado = (from v in dc.View_PedidosParaImpresions
                             where (v.FechaPedido.Date >= FI && v.FechaPedido.Date <= FF)
                             && v.Transporte == transporte.ToUpper()
                             && (v.NroImpresion == 0 || v.NroImpresion <= IncluirImpresas)
                             && v.Cliente == long.Parse(cliente)
                             select v).ToList();
            }
            else
            {
                Resultado = (from v in dc.View_PedidosParaImpresions
                             where (v.FechaPedido.Date >= FI && v.FechaPedido.Date <= FF)
                             && v.Transporte == transporte.ToUpper()
                             && (v.NroImpresion == 0 || v.NroImpresion <= IncluirImpresas)
                             select v).ToList();
            }


            ///// Guardo las cabeceras para luego utilizarlas para la impresión.
            List<long> lista = (from C in Resultado
                                select C.IdCabeceraPedido).Distinct().ToList<long>();

            result.Add("Datos", Resultado.OrderBy(w => w.FechaPedido.Date).ThenBy(w => w.Solicitante).ToList());

            HttpContext.Current.Session.Add("ListaPedido", lista);


        }

        return result;
    }
}
