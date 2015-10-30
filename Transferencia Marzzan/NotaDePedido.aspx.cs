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

public partial class NotaDePedido : BasePage
{
    public List<Producto> _productos = null;
    public long _idDireccionGrabada = 0;
    public string idProductoIncorporacion_0 = "4693";

    // Incorporacion 0 para el 2014 y 2015 (_0_2014)
    public string idProductoIncorporacion_0_2014 = "6823";
    public long idPresentacionIncorporacion_0_2014 = 6878;



    public List<string> idsProductosIncorporaciones_1_4 = new List<string>() { "5714", "5715", "5718", "5719" };
    public List<long> idsPresentacionIncorporaciones_1_4 = new List<long>() { 5796, 5797, 5798, 5799 };

    public List<string> idsProductosIncorporaciones_1_2_2014 = new List<string>() { "6822", "6824" };
    public List<long> idsPresentacionIncorporaciones_1_2_2014 = new List<long>() { 6877, 6879 };

    public List<string> idsProductosIncorporaciones_1_2015 = new List<string>() { "6935" };
    public List<long> idsPresentacionIncorporaciones_1_2015 = new List<long>() { 6947 };

    // Presentaciones de las incorporaciones de la 1  al a 4
    //5796	2516700021001   	Unidad	5714	30.00	1	00025167001
    //5797	2516700021002   	Unidad	5715	175.00	1	00025167002
    //5798	2516700021003   	Unidad	5718	315.00	1	00025167003
    //5799	2516700021004   	Unidad	5719	500.00	1	00025167004

    // Presentaciones de la 1 a 2 para el año 2014
    // 2516700019020        Incorporación Nº 0 Catálogo SM  -->  2516700021005        Incorporación Costo 0 - Catálogo + Revista + Carpe 
    // 2516700021001        Incorporación_N°1  -->  2516700021007        Incorporación Bolso SM     (idPre: 6877) - (IdProd: 6822)
    // 2516700021002        Incorporación_N°2  -->  2516700021006        Incorporación Valija SM  - (idPre: 6879) - (IdProd: 6824)

    // Presentaciones 2015
    //                                         -->  2516700021008        Incorporación Económica SM  - (idPre: 6947) - (IdProd: 6935)


    private Marzzan_InfolegacyDataContext Contexto
    {
        get
        {

            if (Session["Context"] == null)
            {
                Session.Add("Context", new Marzzan_InfolegacyDataContext());
                ((Marzzan_InfolegacyDataContext)Session["Context"]).CommandTimeout = 30;
            }

            return (Marzzan_InfolegacyDataContext)Session["Context"];
        }

    }

    /// Cuando se trata de un cliente especial no se 
    /// realizan las validaciones y ni se generan promociones
    private bool EsClienteEspecial
    {
        get
        {
            return (Session["ClienteLogeado"] as Cliente).Clasif1.Contains("DIRECTORIO") ? true : false;
        }
    }

    private decimal SaldoPagoAnticipado
    {
        get
        {
            if (Session["SaldoAnticipado"] == null)
            {
                Session.Add("SaldoAnticipado", 0);
            }

            return decimal.Parse(Session["SaldoAnticipado"].ToString());
        }
        set
        {

            if (Session["SaldoAnticipado"] == null)
            {
                Session.Add("SaldoAnticipado", value);
            }
            else
                Session["SaldoAnticipado"] = value;




        }


    }

    private long IdDireccionSeleccionada
    {
        get
        {

            if (Session["IdDireccionSeleccionada"] == null)
            {
                Session.Add("IdDireccionSeleccionada", 0);
            }

            return (long)Session["IdDireccionSeleccionada"];
        }

        set
        {
            Session.Add("IdDireccionSeleccionada", value);
        }

    }

    private string ProvinciaDireccionSeleccionada
    {
        get
        {

            if (Session["ProvinciaDireccionSeleccionada"] == null)
            {
                Session.Add("ProvinciaDireccionSeleccionada", "");
            }

            return (string)Session["ProvinciaDireccionSeleccionada"];
        }

        set
        {
            Session.Add("ProvinciaDireccionSeleccionada", value);
        }

    }

    public string TipoCliente
    {
        get
        {
            if (Session["Consultor"] != null)
            {

                return (Session["Consultor"] as Cliente).CodTipoCliente;
            }
            else
            {
                return "";
            }

        }


    }

    public List<Producto> Productos
    {
        get
        {
            if (Session["Productos"] != null)
            {

                return (Session["Productos"] as List<Producto>);
            }
            else
            {
                return new List<Producto>();
            }

        }
        set
        {
            Session["Productos"] = value;
        }

    }

    private decimal ImpuestoCalculado
    {
        get;
        set;
    }

    private string DetalleImpuestosCalculados
    {
        get;
        set;
    }

    public List<VeoArticulosyDescuento> DescuentosProductos
    {
        get
        {
            if (Session["DescuentosProductos"] != null)
            {

                return (Session["DescuentosProductos"] as List<VeoArticulosyDescuento>);
            }
            else
            {
                return new List<VeoArticulosyDescuento>();
            }

        }
        set
        {
            Session["DescuentosProductos"] = value;
        }

    }

    protected override void PageLoad()
    {
        #region PostBack
        if (!IsPostBack)
        {
            LimpiarPedido();
            Session.Add("MontoDisponibleCredito", null);
            Session.Add("ClienteLogeado", null);
            Session.Add("Cliente", null);
            Session.Add("detPedido", new List<DetallePedido>());
            Session.Add("Consultor", null);
            Session.Add("PromosGuardadas", new List<DetallePedido>());
            Session.Add("PromosGeneradas", new List<DetallePedido>());
            Session.Add("IdDireccionSeleccionada", 0);
            Session.Add("ProvinciaDireccionSeleccionada", "");
            Session.Add("PromosActivas", null);
            Session.Add("DescuentosProductos", new List<VeoArticulosyDescuento>());


            ViewState.Add("TransportistaSeleccionado", "");
            Session["Context"] = null;
            Cliente cliente = null;


            using (Marzzan_BejermanDataContext dcB = new Marzzan_BejermanDataContext())
            {
                DescuentosProductos = (from C in dcB.VeoArticulosyDescuentos
                                       select C).ToList();


            }


            Session["ClienteLogeado"] = (from C in Contexto.Clientes
                                         where C.IdCliente == long.Parse(Session["IdUsuario"].ToString())
                                         select C).Single<Cliente>();


            /// Cargo el combo con los tipos de tarjetas habilitadas
            cboTarjetas.DataValueField = "Codigo";
            cboTarjetas.DataTextField = "Descripcion";
            cboTarjetas.DataSource = (from C in Contexto.Tarjetas
                                      where C.Activa.Value
                                      select C).ToList();

            cboTarjetas.DataBind();



            if (Request.QueryString["IdPedido"] != null)
            {

                CabeceraPedido CurrentCabecera = (from C in Contexto.CabeceraPedidos
                                                  where C.IdCabeceraPedido == long.Parse(Request.QueryString["IdPedido"].ToString())
                                                  select C).SingleOrDefault();

                if (CurrentCabecera != null && CurrentCabecera.EsTemporal.HasValue && CurrentCabecera.EsTemporal.Value)
                {
                    cliente = CurrentCabecera.objCliente;
                    Session["Cliente"] = CurrentCabecera.objCliente;
                    Session["Consultor"] = CurrentCabecera.objCliente;


                    Session["detPedido"] = (from d in CurrentCabecera.DetallePedidos
                                            where (d.objProducto.Tipo == 'A' && d.objPromocionOrigen == null)
                                            || ((d.objProducto.Tipo == 'P' || d.objProducto.Tipo == 'D') && d.ValorTotal > 0)
                                            select d).ToList();


                    /// Control de cantidad de productos por pedido
                    /// si el producto es de 5 ml, 55 ml o promoción de una sola por pedido
                    /// entonce marco al detalle como UnoPorPedido
                    foreach (var item in Session["detPedido"] as List<DetallePedido>)
                    {
                        if (item.objPresentacion.Codigo.StartsWith("1128201140"))
                        {
                            item.UnoPorPedido = false;
                        }
                        else
                        {
                            if (item.objPresentacion.Descripcion.StartsWith("5 ml") ||
                            // item.objPresentacion.Descripcion.Contains("55 ml") || // Cambio solicitado 15/10/215
                            ((item.objPresentacion.objProducto.Tipo == 'P' || item.objPresentacion.objProducto.Tipo == 'D') && item.objPresentacion.objProducto.objConfPromocion != null && item.objProducto.objConfPromocion.UnaPorPedido.Value))

                                item.UnoPorPedido = true;
                            else
                                item.UnoPorPedido = false;
                        }
                    }


                    Session["PromosGuardadas"] = (from d in CurrentCabecera.DetallePedidos
                                                  where (d.objProducto.Tipo == 'P' || d.objProducto.Tipo == 'D') && d.ValorTotal < 0
                                                  select d).ToList();

                    Session["productosRegalosSeleccionados"] = (from d in CurrentCabecera.DetallePedidos
                                                                where d.objPromocionOrigen != null
                                                                select d).ToList();

                    CargarArbolProductos();
                    UCTotalizadorNivel.IniciarControl();
                    CargarFormaDePago(cliente);
                    cboFormaPago.SelectedValue = CurrentCabecera.FormaPago.ToString();

                    cboConsultores.Items.Add(new RadComboBoxItem(cliente.Nombre.Trim(), cliente.IdCliente.ToString()));
                    _idDireccionGrabada = CurrentCabecera.DireccionEntrega;
                    CargarEncabezado(cliente, CurrentCabecera.IdCabeceraPedido);
                    cboConsultores.Enabled = false;
                    cboConsultores.SelectedIndex = 0;

                    txtObservacion.Text = CurrentCabecera.Retira;


                    ControlarCambiosPrecios(CurrentCabecera);
                    ActualizarTotalesGenerales();
                    CalcularPromociones();

                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "EdicionInvalida", "AlertaEdicionInvalida();", true);
                    RadTabStrip2.Visible = false;
                    RadMultiPrincipal.Visible = false;
                }
            }
            else
            {

                cliente = (from C in Contexto.Clientes
                           where C.IdCliente == long.Parse(Session["IdUsuario"].ToString())
                           select C).Single<Cliente>();


                List<Cliente> consultores = Helper.ObtenerConsultoresSubordinados(cliente);


                if (consultores.Count() > 1)
                {
                    CargarArbolProductos();
                    cboConsultores.AppendDataBoundItems = true;
                    cboConsultores.Items.Add(new RadComboBoxItem("Seleccione un Revendedor", "0"));
                    cboConsultores.Items.Add(new RadComboBoxItem(cliente.Nombre.Trim(), cliente.IdCliente.ToString()));
                    cboConsultores.DataTextField = "Nombre";
                    cboConsultores.DataValueField = "IdCliente";
                    cboConsultores.DataSource = consultores;
                    cboConsultores.DataBind();
                    CargarFormaDePago(null);
                }
                else
                {
                    Session["Consultor"] = cliente;
                    Session["Cliente"] = cliente;
                    CargarArbolProductos();
                    cboConsultores.Items.Add(new RadComboBoxItem(cliente.Nombre.Trim(), cliente.IdCliente.ToString()));
                    UCTotalizadorNivel.IniciarControl();
                    CargarFormaDePago(cliente);
                    CargarEncabezado(cliente, 0);
                    cboConsultores.Enabled = false;
                    cboConsultores.SelectedIndex = 0;


                }


            }

            /// Construcción del slide de promociones
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Server.MapPath("ImagenesPromos/thumbs"));
            int cantidadImgagenes = dir.GetFiles().Where(w => w.Extension.ToLower() == ".jpg").Count();
            string imagenes = "";

            for (int j = 1; j < 4; j++)
            {
                for (int i = 1; i <= cantidadImgagenes; i++)
                {
                    imagenes += string.Format("<li value='{0}'><img src='ImagenesPromos/thumbs/{1}.jpg' width='179' height='100' alt='' /></li>", i, i);
                }
            }
            thumbs.InnerHtml = imagenes;

            /// Construcción del slide de oportunidades
            dir = new System.IO.DirectoryInfo(Server.MapPath("ImagenesOportunidad/thumbs"));
            cantidadImgagenes = dir.GetFiles().Where(w => w.Extension.ToLower() == ".jpg").Count();
            imagenes = "";


            for (int i = 1; i <= cantidadImgagenes; i++)
            {
                imagenes += string.Format("<a href='ImagenesOportunidad/Full/Optn_{0}.jpg' target='_blank' rel='lightbox' ><img class='cloudcarousel' src='ImagenesOportunidad/thumbs/Optn_{0}.jpg' width='210px' height='121px' /></a> ", i);
            }


            carousel1.InnerHtml = imagenes;


            ///// Link de la mini Revista
            Parametro url = (from D in Contexto.Parametros
                             where D.Tipo.ToLower() == "URLMiniRevista"
                             select D).FirstOrDefault();

            if (url != null)
                urlMiniRevista.HRef = url.Valor;

        }

        #endregion

        ucGrillaDirecciones.DireccionSeleccionada += new RowSelectedHanddler(ucGrillaDirecciones_DireccionSeleccionada);
        UcTotalizadorPedidos.LineaPedidoEliminada += new DeletePedidoHanddler(TotalizadorPedidos_LineaPedidoEliminada);
    }

    /// <summary>
    /// Funcion para controlar:
    /// 1.	Cambio de Precios de articulos
    /// 2.	Cambio de Precios en las promociones
    /// 3.	Cambio de Precio en los gastos de envio
    /// 4.	Desactivación de articulo 
    /// 5.	Vigencia de Promociones
    /// </summary>
    private void ControlarCambiosPrecios(CabeceraPedido CurrentCabecera)
    {
        bool huboCambios = false;

        #region Codigo Actualizacion del Pedido en caso de cambios

        //bool huboCambios = false;
        using (Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext())
        {

            /// Recuperar los valores actuales de todos los elementos del detalle para ver si hay alguna diferencia.
            /// sin tener en cuentas los detalles que hacen referencia a promociones directas. Es decir detalles del tipo P o D con valor
            /// positivo, estas se controlan en un paso posterior.
            List<long> ids = CurrentCabecera.DetallePedidos.Where(d => (d.objProducto.Tipo == 'A' || d.objProducto.Tipo == 'G') || ((d.objProducto.Tipo == 'P' || d.objProducto.Tipo == 'D') && d.ValorTotal < 0)).Select(w => w.Presentacion.Value).ToList();
            List<long> idsPrePedido = CurrentCabecera.DetallePedidos.Select(w => w.IdDetallePedido).ToList();
            List<long> idsRequeridos = (from d in dc.DetalleProductosRequeridos
                                        where idsPrePedido.Contains(d.DetallePedidoPromoDirecta.Value)
                                        select d.IdPresentacion.Value
                                        ).ToList();


            ids.AddRange(idsRequeridos);


            var listaPrecios = (from p in dc.Presentacions
                                where ids.Contains(p.IdPresentacion)
                                select new
                                {
                                    idPresentacion = p.IdPresentacion,
                                    Precio = p.objProducto.Tipo == 'A' || p.objProducto.Tipo == 'G' ? p.Precio : p.objProducto.Precio * -1,
                                    activo = !p.Activo.HasValue ? true : p.Activo.Value


                                }).ToList();


            /// 1 y 3 Calculo las diferencias en el detalle para los productos, gasto de envio y promociones inferidas y directas
            var productosConDiferencia = (from d in CurrentCabecera.DetallePedidos
                                          join p in listaPrecios on d.Presentacion.Value equals p.idPresentacion
                                          where p.Precio != d.ValorUnitario &&
                                          ((d.objProducto.Tipo == 'A' || d.objProducto.Tipo == 'G') || ((d.objProducto.Tipo == 'P' || d.objProducto.Tipo == 'D') && d.ValorTotal < 0))
                                          select new
                                          {
                                              d.objProducto.Descripcion,
                                              d.ValorUnitario,
                                              p.Precio,
                                              d.IdDetallePedido
                                          }).ToList();


            if (productosConDiferencia.Count > 0)
            {
                foreach (var item in productosConDiferencia)
                {
                    DetallePedido detalle = CurrentCabecera.DetallePedidos.Where(w => w.IdDetallePedido == item.IdDetallePedido).FirstOrDefault();
                    if (detalle != null)
                    {

                        detalle.ValorUnitario = item.Precio;
                        detalle.ValorTotal = detalle.Cantidad * detalle.ValorUnitario;
                        huboCambios = true;

                    }
                }

            }

            ///2. buso si hay diferencia en las promociones directas segun los componentes requeridos
            /// es decir debo controlar que la suma de los productos requeridos siga siendo igual al valor indicado
            /// para el detalle de tipo promocion directa. ( Tipo P o D con valor Positivo).
            List<DetallePedido> promosDirectas = CurrentCabecera.DetallePedidos.Where(d => ((d.objProducto.Tipo == 'P' || d.objProducto.Tipo == 'D') && d.ValorTotal > 0)).ToList();
            foreach (var item in promosDirectas)
            {

                /// Recupero la composición de los elementos requeridos del producto tipo PROMOCION
                var composicionRequeridos = from R in dc.Composicions
                                            where R.TipoComposicion == "C" && R.objProducto.IdProducto == item.Producto.Value
                                            group R by R.Grupo into c
                                            select new { Grupo = c.Key, componentes = c };

                decimal valorPromoDirecta = 0;
                foreach (var grupo in composicionRequeridos)
                {
                    decimal valorUnitario = grupo.componentes.FirstOrDefault().objPresentacion.Precio.Value;
                    var cantidadRequerida = Convert.ToInt16(grupo.componentes.FirstOrDefault().Cantidad);
                    valorPromoDirecta += valorUnitario * cantidadRequerida;


                    foreach (var itemRequerido in item.colProductosRequeridos)
                    {
                        var itemNuevoPrecio = listaPrecios.Where(w => w.idPresentacion == itemRequerido.IdPresentacion).FirstOrDefault();
                        if (itemNuevoPrecio != null)
                        {
                            itemRequerido.ValorUnitario = itemNuevoPrecio.Precio;
                        }
                    }


                }

                if (valorPromoDirecta != item.ValorTotal)
                {
                    item.ValorTotal = valorPromoDirecta;
                    item.ValorUnitario = valorPromoDirecta;
                    huboCambios = true;
                }



            }

            /// 4. Determino si hay algun producto del tipo A que ya no este mas activo, de ser así
            /// entonces lo debo eliminar del pedido (las promociones se controlar por separado).
            var productosDesactivados = (from d in CurrentCabecera.DetallePedidos
                                         join p in listaPrecios on d.Presentacion.Value equals p.idPresentacion
                                         where p.activo == false && d.objProducto.Tipo == 'A'
                                         select new
                                         {
                                             d.objProducto.Descripcion,
                                             d.IdDetallePedido
                                         }).ToList();

            foreach (var item in productosDesactivados)
            {
                DetallePedido detEliminar = CurrentCabecera.DetallePedidos.Where(w => w.IdDetallePedido == item.IdDetallePedido).FirstOrDefault();

                (Session["detPedido"] as List<DetallePedido>).Remove(detEliminar);

                if (detEliminar.IdDetallePedido > 0)
                {

                    using (Marzzan_InfolegacyDataContext dcEliminar = new Marzzan_InfolegacyDataContext())
                    {
                        dcEliminar.CommandTimeout = 300;

                        DetallePedido Detalle = (from D in dcEliminar.DetallePedidos
                                                 where D.IdDetallePedido == detEliminar.IdDetallePedido
                                                 select D).SingleOrDefault();

                        dcEliminar.DetallePedidos.DeleteOnSubmit(Detalle);
                        dcEliminar.SubmitChanges();
                        huboCambios = true;
                    }

                }
            }


            /// 5. Busco para las promociones en el pedido si las mismas siguen en vigencia
            /// según la feha inicial, final y el transporte.
            var promosDelPedido = (from d in CurrentCabecera.DetallePedidos
                                   where d.objProducto.Tipo == 'P' || d.objProducto.Tipo == 'D'
                                   select new
                                   {
                                       d.objProducto.Descripcion,
                                       d.objProducto.objConfPromocion.FechaInicio,
                                       d.objProducto.objConfPromocion.FechaFinal,
                                       d.objProducto.objConfPromocion.ColTransportistas,
                                       d.objProducto.objConfPromocion.MontoMinimo,
                                       d.objProducto.objConfPromocion.TipoPromo,
                                       d.IdDetallePedido,
                                       d.ValorTotal,
                                       detalle = d

                                   }).ToList();

            foreach (var item in promosDelPedido)
            {
                decimal MontoActual = (from P in (Session["detPedido"] as List<DetallePedido>) select P.ValorTotal.Value).Sum();
                bool eliminarPromo = false;
                /// Si tiene establecido transportes y el transporte del pedido NO esta dentro de la lista entonces hay que sacar la promo.
                if ((item.ColTransportistas.Count > 0 && !item.ColTransportistas.Any(w => w.Transporte.ToLower() == lblTransporte.Text.ToLower())))
                {
                    eliminarPromo = true;
                }
                /// si la promocion no esta mas vigente por el rango de fechas  entonces hay que sacar la promo.
                else if (DateTime.Now.Date < item.FechaInicio.Date || DateTime.Now.Date > item.FechaFinal.Date)
                {
                    eliminarPromo = true;
                }
                /// si la promocion no esta mas vigente por el monto minimo entonces hay que sacar la promo.
                /// Solo debo realizar el control si la promocion es inferida. Las promos directas tiene el valor del detalle
                /// en positivo.
                else if (MontoActual < item.MontoMinimo && item.ValorTotal < 0)
                {
                    eliminarPromo = true;
                }
                /// si la promocion no esta mas vigente por el TIPO DE CLIENTE entonces hay que sacar la promo.
                else if (
                        (CurrentCabecera.objCliente.TipoConsultor != "INICIAL" && item.TipoPromo != "INICIAL" && item.TipoPromo != CurrentCabecera.objCliente.TipoConsultor.Trim())
                        ||
                        (CurrentCabecera.objCliente.TipoConsultor == "INICIAL" && CurrentCabecera.objCliente.TipoConsultor != item.TipoPromo)
                    )
                {
                    eliminarPromo = true;
                }



                if (eliminarPromo)
                {
                    using (Marzzan_InfolegacyDataContext dcEliminar = new Marzzan_InfolegacyDataContext())
                    {
                        dcEliminar.CommandTimeout = 300;

                        /// entonces debo eliminar:
                        ///     a. el detalle en cuestion
                        ///     b. los productos requeridos que fueron seleccionados 
                        ///     c. los regalos seleccionados si existe.

                        (Session["detPedido"] as List<DetallePedido>).Remove(item.detalle);
                        (Session["PromosGuardadas"] as List<DetallePedido>).Remove(item.detalle);

                        /////////////// a. ///////////////
                        DetallePedido Detalle = (from D in dcEliminar.DetallePedidos
                                                 where D.IdDetallePedido == item.detalle.IdDetallePedido
                                                 select D).SingleOrDefault();

                        dcEliminar.DetallePedidos.DeleteOnSubmit(Detalle);


                        /////////////// b. ///////////////
                        List<DetalleProductosRequeridos> Detalles = (from D in dcEliminar.DetalleProductosRequeridos
                                                                     where D.IdDetalleProductosRequeridos == item.detalle.IdDetallePedido
                                                                     select D).ToList();

                        dcEliminar.DetalleProductosRequeridos.DeleteAllOnSubmit(Detalles);

                        /////////////// c. ///////////////
                        List<DetallePedido> DetalleRelacionados = (from D in dcEliminar.DetallePedidos
                                                                   where D.CabeceraPedido == item.detalle.CabeceraPedido && D.PromocionOrigen == item.detalle.IdDetallePedido
                                                                   select D).ToList();

                        dcEliminar.DetallePedidos.DeleteAllOnSubmit(DetalleRelacionados);

                        /// Persisto los cambios
                        dcEliminar.SubmitChanges();

                        huboCambios = true;
                    }


                }
            }

            /// 6. ACTUALIZACION DE REMITOS
            /// LOS REMITOS NO SE GRABAN CUANDO SE GUARDA EN FORMA TEMPORAL, POR LO QUE NO HAY QUE HACER NADA CUANDO SE EDITA.

            /// 7. CONTROLAR CAMBIOS EN LOS PORCENTAJES POR PROVINCIA
            /// EL PORCENTAJE SE CALCULA EN EL METODO "CalcularTotalPedido" Y ESTE SE EJECUTA CUANDO SE INICIA LA EDICION POR LO QUE LOS
            /// MONTOS SON RE CALCULADOS.

            /// 8. CONTROLAR CAMBIOS EN LOS PORCENTAJES DE DESCUENTO EN LOS PRODUCTOS
            /// CUANDO LOS PORCENTAJES SON ACTUALIZADOS SE PRODUCE LA MISMA SITUACIÓN QUE LOS PORCENTAJES DE LAS PROVINCIAS, POR LO
            /// QUE SE ACTUALIZA AUTOMATICAMENTE CDO SE EDITA LA OPERACION.




        }

        if (huboCambios)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "EdicionActualizada", "AlertaEdicionActualizada();", true);
        }

        #endregion

        return;

        #region Codigo Eliminacion del pedido en caso de cambios
        using (Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext())
        {

            /// Recuperar los valores actuales de todos los elementos del detalle para ver si hay alguna diferencia.
            /// sin tener en cuentas los detalles que hacen referencia a promociones directas. Es decir detalles del tipo P o D con valor
            /// positivo, estas se controlan en un paso posterior.
            List<long> ids = CurrentCabecera.DetallePedidos.Where(d => (d.objProducto.Tipo == 'A' || d.objProducto.Tipo == 'G') || ((d.objProducto.Tipo == 'P' || d.objProducto.Tipo == 'D') && d.ValorTotal < 0)).Select(w => w.Presentacion.Value).ToList();
            List<long> idsPrePedido = CurrentCabecera.DetallePedidos.Select(w => w.IdDetallePedido).ToList();
            List<long> idsRequeridos = (from d in dc.DetalleProductosRequeridos
                                        where idsPrePedido.Contains(d.DetallePedidoPromoDirecta.Value)
                                        select d.IdPresentacion.Value
                                        ).ToList();


            ids.AddRange(idsRequeridos);


            var listaPrecios = (from p in dc.Presentacions
                                where ids.Contains(p.IdPresentacion)
                                select new
                                {
                                    idPresentacion = p.IdPresentacion,
                                    Precio = p.objProducto.Tipo == 'A' || p.objProducto.Tipo == 'G' ? p.Precio : p.objProducto.Precio * -1,
                                    activo = !p.Activo.HasValue ? true : p.Activo.Value


                                }).ToList();


            /// 1 y 3 Calculo las diferencias en el detalle para los productos, gasto de envio y promociones inferidas y directas
            var productosConDiferencia = (from d in CurrentCabecera.DetallePedidos
                                          join p in listaPrecios on d.Presentacion.Value equals p.idPresentacion
                                          where p.Precio != d.ValorUnitario &&
                                          ((d.objProducto.Tipo == 'A' || d.objProducto.Tipo == 'G') || ((d.objProducto.Tipo == 'P' || d.objProducto.Tipo == 'D') && d.ValorTotal < 0))
                                          select new
                                          {
                                              d.objProducto.Descripcion,
                                              d.ValorUnitario,
                                              p.Precio,
                                              d.IdDetallePedido
                                          }).ToList();


            if (productosConDiferencia.Count > 0)
            {
                //foreach (var item in productosConDiferencia)
                //{
                //    DetallePedido detalle = CurrentCabecera.DetallePedidos.Where(w => w.IdDetallePedido == item.IdDetallePedido).FirstOrDefault();
                //    if (detalle != null)
                //    {

                //        detalle.ValorUnitario = item.Precio;
                //        detalle.ValorTotal = detalle.Cantidad * detalle.ValorUnitario;
                //        huboCambios = true;

                //    }
                //}

                // Cambio febrero 2015
                huboCambios = true;
            }

            ///2. buso si hay diferencia en las promociones directas segun los componentes requeridos
            /// es decir debo controlar que la suma de los productos requeridos siga siendo igual al valor indicado
            /// para el detalle de tipo promocion directa. ( Tipo P o D con valor Positivo).
            List<DetallePedido> promosDirectas = CurrentCabecera.DetallePedidos.Where(d => ((d.objProducto.Tipo == 'P' || d.objProducto.Tipo == 'D') && d.ValorTotal > 0)).ToList();
            foreach (var item in promosDirectas)
            {

                /// Recupero la composición de los elementos requeridos del producto tipo PROMOCION
                var composicionRequeridos = from R in dc.Composicions
                                            where R.TipoComposicion == "C" && R.objProducto.IdProducto == item.Producto.Value
                                            group R by R.Grupo into c
                                            select new { Grupo = c.Key, componentes = c };

                decimal valorPromoDirecta = 0;
                foreach (var grupo in composicionRequeridos)
                {
                    decimal valorUnitario = grupo.componentes.FirstOrDefault().objPresentacion.Precio.Value;
                    var cantidadRequerida = Convert.ToInt16(grupo.componentes.FirstOrDefault().Cantidad);
                    valorPromoDirecta += valorUnitario * cantidadRequerida;


                    foreach (var itemRequerido in item.colProductosRequeridos)
                    {
                        var itemNuevoPrecio = listaPrecios.Where(w => w.idPresentacion == itemRequerido.IdPresentacion).FirstOrDefault();
                        if (itemNuevoPrecio != null)
                        {
                            itemRequerido.ValorUnitario = itemNuevoPrecio.Precio;
                        }
                    }


                }

                //if (valorPromoDirecta != item.ValorTotal)
                //{
                //    item.ValorTotal = valorPromoDirecta;
                //    item.ValorUnitario = valorPromoDirecta;
                //    huboCambios = true;
                //}

                // Cambio febrero 2015
                if (valorPromoDirecta != item.ValorTotal)
                {
                    huboCambios = true;
                }

            }

            /// 4. Determino si hay algun producto del tipo A que ya no este mas activo, de ser así
            /// entonces lo debo eliminar del pedido (las promociones se controlar por separado).
            var productosDesactivados = (from d in CurrentCabecera.DetallePedidos
                                         join p in listaPrecios on d.Presentacion.Value equals p.idPresentacion
                                         where p.activo == false && d.objProducto.Tipo == 'A'
                                         select new
                                         {
                                             d.objProducto.Descripcion,
                                             d.IdDetallePedido
                                         }).ToList();

            foreach (var item in productosDesactivados)
            {
                DetallePedido detEliminar = CurrentCabecera.DetallePedidos.Where(w => w.IdDetallePedido == item.IdDetallePedido).FirstOrDefault();

                (Session["detPedido"] as List<DetallePedido>).Remove(detEliminar);

                if (detEliminar.IdDetallePedido > 0)
                {

                    //using (Marzzan_InfolegacyDataContext dcEliminar = new Marzzan_InfolegacyDataContext())
                    //{
                    //    dcEliminar.CommandTimeout = 300;

                    //    DetallePedido Detalle = (from D in dcEliminar.DetallePedidos
                    //                             where D.IdDetallePedido == detEliminar.IdDetallePedido
                    //                             select D).SingleOrDefault();

                    //    dcEliminar.DetallePedidos.DeleteOnSubmit(Detalle);
                    //    dcEliminar.SubmitChanges();
                    //    huboCambios = true;
                    //}

                    // Cambio febrero 2015
                    huboCambios = true;
                }
            }


            /// 5. Busco para las promociones en el pedido si las mismas siguen en vigencia
            /// según la feha inicial, final y el transporte.
            var promosDelPedido = (from d in CurrentCabecera.DetallePedidos
                                   where d.objProducto.Tipo == 'P' || d.objProducto.Tipo == 'D'
                                   select new
                                   {
                                       d.objProducto.Descripcion,
                                       d.objProducto.objConfPromocion.FechaInicio,
                                       d.objProducto.objConfPromocion.FechaFinal,
                                       d.objProducto.objConfPromocion.ColTransportistas,
                                       d.objProducto.objConfPromocion.MontoMinimo,
                                       d.objProducto.objConfPromocion.TipoPromo,
                                       d.IdDetallePedido,
                                       d.ValorTotal,
                                       detalle = d

                                   }).ToList();

            string mensajeDiv = "";
            foreach (var item in promosDelPedido)
            {
                decimal MontoActual = (from P in (Session["detPedido"] as List<DetallePedido>) select P.ValorTotal.Value).Sum();
                bool eliminarPromo = false;
                /// Si tiene establecido transportes y el transporte del pedido NO esta dentro de la lista entonces hay que sacar la promo.
                if ((item.ColTransportistas.Count > 0 && !item.ColTransportistas.Any(w => w.Transporte.ToLower() == lblTransporte.Text.ToLower())))
                {
                    eliminarPromo = true;
                    mensajeDiv = "Transporte";
                }
                /// si la promocion no esta mas vigente por el rango de fechas  entonces hay que sacar la promo.
                else if (DateTime.Now.Date < item.FechaInicio.Date || DateTime.Now.Date > item.FechaFinal.Date)
                {
                    eliminarPromo = true;
                    mensajeDiv += "Fechas";
                }
                /// si la promocion no esta mas vigente por el monto minimo entonces hay que sacar la promo.
                /// Solo debo realizar el control si la promocion es inferida. Las promos directas tiene el valor del detalle
                /// en positivo.
                else if (MontoActual < item.MontoMinimo && item.ValorTotal < 0)
                {
                    eliminarPromo = true;
                    mensajeDiv += "Monto";
                }
                /// si la promocion no esta mas vigente por el TIPO DE CLIENTE entonces hay que sacar la promo.
                else if (
                        (CurrentCabecera.objCliente.TipoConsultor != "INICIAL" && item.TipoPromo != "INICIAL" && item.TipoPromo != CurrentCabecera.objCliente.TipoConsultor.Trim())
                        ||
                        (CurrentCabecera.objCliente.TipoConsultor == "INICIAL" && CurrentCabecera.objCliente.TipoConsultor != item.TipoPromo)
                    )
                {

                    // TC: vip
                    // TP: Inicial



                    eliminarPromo = true;
                    mensajeDiv += "Tipo Promo";
                }

                //DivHelp.InnerHtml = mensajeDiv;


                if (eliminarPromo)
                {
                    //using (Marzzan_InfolegacyDataContext dcEliminar = new Marzzan_InfolegacyDataContext())
                    //{
                    //    dcEliminar.CommandTimeout = 300;

                    //    /// entonces debo eliminar:
                    //    ///     a. el detalle en cuestion
                    //    ///     b. los productos requeridos que fueron seleccionados 
                    //    ///     c. los regalos seleccionados si existe.

                    //    (Session["detPedido"] as List<DetallePedido>).Remove(item.detalle);
                    //    (Session["PromosGuardadas"] as List<DetallePedido>).Remove(item.detalle);

                    //    /////////////// a. ///////////////
                    //    DetallePedido Detalle = (from D in dcEliminar.DetallePedidos
                    //                             where D.IdDetallePedido == item.detalle.IdDetallePedido
                    //                             select D).SingleOrDefault();

                    //    dcEliminar.DetallePedidos.DeleteOnSubmit(Detalle);


                    //    /////////////// b. ///////////////
                    //    List<DetalleProductosRequeridos> Detalles = (from D in dcEliminar.DetalleProductosRequeridos
                    //                                                 where D.IdDetalleProductosRequeridos == item.detalle.IdDetallePedido
                    //                                                 select D).ToList();

                    //    dcEliminar.DetalleProductosRequeridos.DeleteAllOnSubmit(Detalles);

                    //    /////////////// c. ///////////////
                    //    List<DetallePedido> DetalleRelacionados = (from D in dcEliminar.DetallePedidos
                    //                                               where D.CabeceraPedido == item.detalle.CabeceraPedido && D.PromocionOrigen == item.detalle.IdDetallePedido
                    //                                               select D).ToList();

                    //    dcEliminar.DetallePedidos.DeleteAllOnSubmit(DetalleRelacionados);

                    //    /// Persisto los cambios
                    //    dcEliminar.SubmitChanges();

                    //    huboCambios = true;
                    //}

                    // Cambio febrero 2015
                    huboCambios = true;
                }
            }

            /// 6. ACTUALIZACION DE REMITOS
            /// LOS REMITOS NO SE GRABAN CUANDO SE GUARDA EN FORMA TEMPORAL, POR LO QUE NO HAY QUE HACER NADA CUANDO SE EDITA.

            /// 7. CONTROLAR CAMBIOS EN LOS PORCENTAJES POR PROVINCIA
            /// EL PORCENTAJE SE CALCULA EN EL METODO "CalcularTotalPedido" Y ESTE SE EJECUTA CUANDO SE INICIA LA EDICION POR LO QUE LOS
            /// MONTOS SON RE CALCULADOS.

            /// 8. CONTROLAR CAMBIOS EN LOS PORCENTAJES DE DESCUENTO EN LOS PRODUCTOS
            /// CUANDO LOS PORCENTAJES SON ACTUALIZADOS SE PRODUCE LA MISMA SITUACIÓN QUE LOS PORCENTAJES DE LAS PROVINCIAS, POR LO
            /// QUE SE ACTUALIZA AUTOMATICAMENTE CDO SE EDITA LA OPERACION.



            if (huboCambios)
            {
                //ScriptManager.RegisterStartupScript(Page, typeof(Page), "EdicionActualizada", "AlertaEdicionActualizada();", true);

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "EdicionActualizada", "AlertaEdicionAnulada();", true);

                CabeceraPedido cabeliminar = (from c in dc.CabeceraPedidos
                                              where c.IdCabeceraPedido == CurrentCabecera.IdCabeceraPedido
                                              select c).FirstOrDefault();

                dc.CommandTimeout = 300;
                dc.PedidosConCreditos.DeleteAllOnSubmit(cabeliminar.PedidosConCreditos);
                dc.RemitosAfectados.DeleteAllOnSubmit(cabeliminar.colRemitosAfectados);
                dc.DetallePedidos.DeleteAllOnSubmit(cabeliminar.DetallePedidos.ToList());
                dc.CabeceraPedidos.DeleteOnSubmit(cabeliminar);
                dc.SubmitChanges();
            }

        }
        #endregion
    }

    #region Eventos
    public void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {

        if (e.Argument != "undefined")
        {
            if (e.Argument == "Limpiar")
            {
                LimpiarPedido();
            }
            else if (e.Argument == "GuardarTemporal")
            {
                string id = "";
                if (Request.QueryString["IdPedido"] != null)
                    id = Request.QueryString["IdPedido"].ToString();

                GuardarPedido(id, true, true, false);

            }
            else if (e.Argument == "Direcciones")
            {

                Cliente cliente = (from C in Contexto.Clientes
                                   where C.IdCliente == long.Parse(cboConsultores.SelectedValue)
                                   select C).Single<Cliente>();


                Session["Cliente"] = cliente;
                Session["Consultor"] = cliente;

                UCTotalizadorNivel.IniciarControl();

                CargarFormaDePago(cliente);

                CargarDireccionesEntrega(cliente);

                ActualizarProductosEspeciales(cliente);

                if ((Session["detPedido"] as List<DetallePedido>).Count > 0)
                {
                    if (!EsClienteEspecial)
                        CalcularPromociones();
                }


            }
            else
            {

                string[] datos = e.Argument.ToString().Split('@');

                string TipoOperacion = "";
                string[] IdsPresentaciones = datos[0].Split('|');
                string[] Valores = datos[1].Split('|');
                string codigoEdicion = "";

                if (datos.Length >= 4)
                {
                    codigoEdicion = datos[2].ToString();
                    TipoOperacion = datos[3].ToString();
                }
                else
                {
                    TipoOperacion = datos[2].ToString();
                }


                if (
                    (TipoCliente.ToString() == Convert.ToString((int)TipoClientes.PotencialBolso) && IdsPresentaciones.Count() > 2)
                    ||
                    (TipoCliente.ToString() == Convert.ToString((int)TipoClientes.PotencialBolso) && (Session["detPedido"] as List<DetallePedido>).Count > 0))
                {
                    upFichaTecnica.Update();
                    ScriptManager.RegisterStartupScript(upFichaTecnica, typeof(UpdatePanel), "Potencial", "alert('Solo se puede solicitar un tipo de incorporacion')", true);
                    return;
                }


                for (int i = 0; i < IdsPresentaciones.Length - 1; i++)
                {
                    string CodigoPresentacion = IdsPresentaciones[i].ToString();


                    if (CodigoPresentacion != "")
                    {

                        Presentacion presentacion = (from P in Contexto.Presentacions
                                                     where P.Codigo == CodigoPresentacion
                                                     select P).First<Presentacion>();


                        string valor = Valores[i].ToString();

                        int YaPidio = (from P in Session["detPedido"] as List<DetallePedido>
                                       where P.CodigoCompleto.Trim() == CodigoPresentacion.Trim()
                                       select P).Count();


                        if (valor != "" && int.Parse(valor) > 0)
                        {
                            if (YaPidio == 0)
                            {
                                DetallePedido detPedido = new DetallePedido();
                                detPedido.Cantidad = int.Parse(valor);
                                detPedido.Producto = presentacion.objProducto.IdProducto;
                                detPedido.ProductoDesc = presentacion.objProducto.Descripcion;
                                detPedido.Presentacion = presentacion.IdPresentacion;
                                detPedido.PresentacionDesc = presentacion.Descripcion;
                                detPedido.ValorUnitario = presentacion.Precio.Value;
                                detPedido.ValorTotal = detPedido.ValorUnitario * detPedido.Cantidad;
                                detPedido.CodigoCompleto = presentacion.Codigo;
                                detPedido.Tipo = presentacion.objProducto.Tipo.ToString();

                                if (presentacion.objProducto.objPadre.EsUltimoNivel.Value)
                                {
                                    detPedido.DescripcionCompleta = detPedido.ProductoDesc + " x " + detPedido.PresentacionDesc;
                                    detPedido.IdPadre = presentacion.objProducto.objPadre.IdProducto;
                                    detPedido.DescPadre = Helper.ObtenerDescripcionCompletaProducto(presentacion.objProducto.objPadre);
                                    detPedido.DescripcionCompleta = detPedido.ProductoDesc + " x " + detPedido.PresentacionDesc;
                                }
                                else if (presentacion.objProducto.EsUltimoNivel.Value && (detPedido.Tipo == "P" || detPedido.Tipo == "D"))
                                {
                                    detPedido.DescripcionCompleta = detPedido.PresentacionDesc;
                                    detPedido.IdPadre = presentacion.objProducto.objPadre.IdProducto;
                                    detPedido.DescPadre = Helper.ObtenerDescripcionCompletaProducto(presentacion.objProducto.objPadre);
                                    detPedido.ValorUnitario = -1 * presentacion.Precio.Value;
                                    detPedido.ValorTotal = detPedido.ValorUnitario * detPedido.Cantidad;
                                }
                                else
                                {
                                    detPedido.DescripcionCompleta = presentacion.objProducto.DescripcionCompleta + detPedido.PresentacionDesc;
                                    detPedido.IdPadre = presentacion.objProducto.objPadre.objPadre.IdProducto;
                                    detPedido.DescPadre = Helper.ObtenerDescripcionCompletaProducto(presentacion.objProducto.objPadre.objPadre);
                                    detPedido.DescripcionCompleta = detPedido.ProductoDesc + " x " + detPedido.PresentacionDesc;
                                }

                                /// Control de cantidad de productos por pedido
                                /// si el producto es de 5 ml, 55 ml o promoción de una sola por pedido
                                /// entonce marco al detalle como UnoPorPedido
                                if (presentacion.Codigo.StartsWith("1128201140"))
                                {
                                    detPedido.UnoPorPedido = false;
                                }
                                else
                                {
                                    if (presentacion.Descripcion.StartsWith("5 ml") ||
                                        //presentacion.Descripcion.Contains("55 ml") || // Cambio solicitado 15/10/215
                                        ((presentacion.objProducto.Tipo == 'P' || presentacion.objProducto.Tipo == 'D') && presentacion.objProducto.objConfPromocion != null && presentacion.objProducto.objConfPromocion.UnaPorPedido.Value))

                                        detPedido.UnoPorPedido = true;
                                    else
                                        detPedido.UnoPorPedido = false;
                                }


                                (Session["detPedido"] as List<DetallePedido>).Add(detPedido);
                            }
                            else
                            {
                                DetallePedido pedidoExistente = (from P in Session["detPedido"] as List<DetallePedido>
                                                                 where P.CodigoCompleto.Trim() == CodigoPresentacion.Trim()
                                                                 select P).First<DetallePedido>();

                                if (TipoOperacion == "N")
                                    pedidoExistente.Cantidad += int.Parse(valor);
                                else
                                    pedidoExistente.Cantidad = int.Parse(valor);

                                pedidoExistente.ValorTotal = pedidoExistente.Cantidad * pedidoExistente.ValorUnitario;

                            }


                        }
                        else
                        {
                            if (YaPidio > 0 && codigoEdicion == CodigoPresentacion.Trim())
                            {
                                DetallePedido CurrentePedido = (from P in Session["detPedido"] as List<DetallePedido>
                                                                where P.CodigoCompleto.Trim() == CodigoPresentacion.Trim()
                                                                select P).First<DetallePedido>();

                                (Session["detPedido"] as List<DetallePedido>).Remove(CurrentePedido);

                                TotalizadorPedidos_LineaPedidoEliminada();
                            }
                        }
                    }
                    else
                        break;

                }

                ActualizarTotalesGenerales();

                /// Recalculo las promociones luego de actualizar los totales
                CalcularPromociones();
            }
        }
    }

    protected void btnPedidoTarjeta_click(object sender, EventArgs e)
    {
        string id = "";
        if (Request.QueryString["IdPedido"] != null)
            id = Request.QueryString["IdPedido"].ToString();

        GuardarPedido(id, false, false, true);
    }

    protected void btnPedido_RealizarPedido(object sender, EventArgs e)
    {
        string id = "";
        if (Request.QueryString["IdPedido"] != null)
            id = Request.QueryString["IdPedido"].ToString();

        GuardarPedido(id, false, false, false);
    }

    protected void btnPedidoTemporal_RealizarPedido(object sender, EventArgs e)
    {
        string id = "";
        if (Request.QueryString["IdPedido"] != null)
            id = Request.QueryString["IdPedido"].ToString();

        GuardarPedido(id, false, true, false);
    }

    protected void RadTreeProductos_NodeDataBound(object sender, RadTreeNodeEventArgs e)
    {
        DataRowView row = (DataRowView)e.Node.DataItem;
        e.Node.Attributes.Add("Padre", row["IdProducto"].ToString());

        List<Producto> Hijos = (from P in _productos
                                where P.Padre == long.Parse(row["IdProducto"].ToString())
                                select P).First<Producto>().ColHijos.ToList<Producto>();


        if (Hijos.Count == 0)
        {
            e.Node.Attributes.Add("Cargar", "true");
            e.Node.Attributes.Add("RutaImagen", row["Image"].ToString());

            if (e.Node.Text.Trim() == "Ninguna")
                e.Node.Visible = false;
        }
        else
        {
            if (row["IdProducto"].ToString() == "4251")
            {
                e.Node.Visible = false;
            }

            e.Node.Attributes.Add("Cargar", "false");
            e.Node.Attributes.Add("RutaImagen", row["Image"].ToString());

        }

        if (row["Descripcion"].ToString() == "Incorporaciones" && row["Nivel"].ToString() == "2")
        {
            e.Node.Attributes.Add("NodoIncorporaciones", "true");
        }


    }

    /// <summary>
    ///  Metodo que se dispara cuando se selecciona una nueva dirección de la grilla 
    ///  de direcciones.
    /// </summary>
    /// <param name="Id">Id de la dirección seleccionada</param>
    protected void ucGrillaDirecciones_DireccionSeleccionada(long Id)
    {

        Direccione dirSeleccionada = (from D in Contexto.Direcciones
                                      where D.IdDireccion == Id
                                      select D).First<Direccione>();


        lblCalle.Text = dirSeleccionada.Calle.ToLower();
        lblDireccionEntrega.Text = dirSeleccionada.Provincia.ToLower() + " - " + dirSeleccionada.Localidad.ToLower();
        IdDireccionSeleccionada = dirSeleccionada.IdDireccion;
        ProvinciaDireccionSeleccionada = dirSeleccionada.Provincia.ToUpper();


        var ultimoPedido = (from v in Contexto.View_UltimoPedidoClientes
                            where v.IdCliente == dirSeleccionada.Cliente.Value
                            select v.UltimaFechaPedido).FirstOrDefault();

        lblUltimoPedido.Text = ultimoPedido == null ? "Sin Pedido Previo" : ultimoPedido.Value.ToShortDateString();


        ConfTransporte confT = (from C in Contexto.ConfTransportes
                                where C.Provincia.ToLower() == dirSeleccionada.Provincia.ToLower() &&
                           C.Localidad.ToLower() == dirSeleccionada.Localidad.ToLower() &&
                           C.FormaDePago == cboFormaPago.SelectedItem.Text
                                select C).FirstOrDefault<ConfTransporte>();

        if (confT != null)
        {
            lblTransporte.Text = confT.Transporte.ToLower();
            lblTransporteHidden.Value = confT.Transporte.ToLower();
            lblTransporteValorHidden.Value = confT.objProducto.ColPresentaciones[0].Precio.ToString();

            Parametro ParamProvincia = (from D in Contexto.Parametros
                                        where D.Tipo.ToLower() == dirSeleccionada.Provincia.ToLower()
                                        select D).FirstOrDefault();

            if (ParamProvincia != null)
            {
                lblProvinciaPorcentajeDescuentoHidden.Value = ParamProvincia.Valor.Replace(".", ",");
            }
            else
            {
                lblProvinciaPorcentajeDescuentoHidden.Value = "0";
                lblDescuentoProvincia.Text = "$ 0";
            }
        }
        else
        {
            lblTransporte.Text = "SIN TRANSPORTE";
            lblTransporteHidden.Value = "";
            lblTransporteValorHidden.Value = "0";
            lblProvinciaPorcentajeDescuentoHidden.Value = "0";
            lblDescuentoProvincia.Text = "$ 0";
        }

        ActualizarTotalesGenerales();
        CargarDatosJerarquia();
        upEncImp.Update();

    }

    protected void cboFormaPago_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (cboFormaPago.SelectedItem.Value != "0" && IdDireccionSeleccionada != 0)
        {

            Direccione dirEntrega = (from D in Contexto.Direcciones
                                     where D.IdDireccion == IdDireccionSeleccionada
                                     select D).FirstOrDefault<Direccione>();


            if (dirEntrega != null)
            {
                ConfTransporte confT = (from C in Contexto.ConfTransportes
                                        where C.Provincia.ToLower() == dirEntrega.Provincia.ToLower() &&
                                        C.Localidad.ToLower() == dirEntrega.Localidad.ToLower() &&
                                        C.FormaDePago == cboFormaPago.SelectedItem.Text
                                        select C).FirstOrDefault<ConfTransporte>();


                if (confT != null)
                {
                    lblTransporte.Text = confT.Transporte.ToLower();
                    lblTransporteHidden.Value = confT.Transporte.ToLower();
                    lblTransporteValorHidden.Value = confT.objProducto.ColPresentaciones[0].Precio.ToString();

                    Parametro ParamProvincia = (from D in Contexto.Parametros
                                                where D.Tipo.ToLower() == dirEntrega.Provincia.ToLower()
                                                select D).FirstOrDefault();

                    if (ParamProvincia != null)
                    {
                        lblProvinciaPorcentajeDescuentoHidden.Value = ParamProvincia.Valor.Replace(".", ",");

                    }
                }
                else
                {
                    lblTransporte.Text = "SIN TRANSPORTE";
                    lblTransporteHidden.Value = "";
                    lblTransporteValorHidden.Value = "0";
                    lblProvinciaPorcentajeDescuentoHidden.Value = "0";
                    lblDescuentoProvincia.Text = "$ 0";
                }

            }
            else
            {

                lblTransporte.Text = "SIN TRANSPORTE";
                lblTransporteHidden.Value = "";
                lblTransporteValorHidden.Value = "0";
                lblProvinciaPorcentajeDescuentoHidden.Value = "0";
                lblDescuentoProvincia.Text = "$ 0";
            }

            if (cboFormaPago.SelectedItem.Text == "Pago Fácil")
                lnkPagoFacil.Visible = true;
            else
                lnkPagoFacil.Visible = false;

            if (cboFormaPago.SelectedItem.Text == "Pago Mis Cuentas")
                lnkPagoMisCuentas.Visible = true;
            else
                lnkPagoMisCuentas.Visible = false;

            if (cboFormaPago.SelectedItem.Text == "Rapi Pago")
                lnkRapiPago.Visible = true;
            else
                lnkRapiPago.Visible = false;

            upDirec.Update();
            upTransporte.Update();
        }

        ActualizarTotalesGenerales();
    }

    protected void btnVolver_Click(object sender, EventArgs e)
    {
        Response.Redirect("Inicio.aspx");
    }

    protected void btnSolicitar_Click(object sender, EventArgs e)
    {
        object aa = resultshere.FindControl("tblCargaPedido");
    }

    #endregion

    #region Metodos

    private void EliminarProductosKit(bool EsTemporal, CabeceraPedido cabecera)
    {
        #region Eliminacion de Productos del tipo KIT

        /// Elimino los productos que fueron reemplazados por otros
        /// según la logica de negocio especifica para estos casos.
        if (!EsTemporal)
        {
            List<DetallePedido> DetallesEliminar = new List<DetallePedido>();

            /// Elimino el producto kit Deditos Sticker y Ambientador (5472)
            List<DetallePedido> detallesKitDeditos = (from d in cabecera.DetallePedidos
                                                      where d.Presentacion == 5472
                                                      select d).ToList();

            if (detallesKitDeditos.Count > 0)
            {
                foreach (var item in detallesKitDeditos)
                {
                    if (item.IdDetallePedido != 0)
                    {
                        Contexto.DetallePedidos.DeleteOnSubmit(item);
                    }

                    DetallesEliminar.Add(item);
                }
            }

            /// Elimino el producto kit Inquietos Sticker y Ambientador (5473)
            List<DetallePedido> detallesKitInquietos = (from d in cabecera.DetallePedidos
                                                        where d.Presentacion == 5473
                                                        select d).ToList();

            if (detallesKitInquietos.Count > 0)
            {
                foreach (var item in detallesKitInquietos)
                {
                    if (item.IdDetallePedido != 0)
                    {
                        Contexto.DetallePedidos.DeleteOnSubmit(item);
                    }
                    DetallesEliminar.Add(item);
                }
            }
            
            /// Elimino el producto Catálogo x 10 unidades  (2506600030159)
            List<DetallePedido> detallesCatalogox10 = (from d in cabecera.DetallePedidos
                                                       where d.Presentacion == 7212
                                                       select d).ToList();

            if (detallesCatalogox10.Count > 0)
            {
                foreach (var item in detallesCatalogox10)
                {
                    if (item.IdDetallePedido != 0)
                    {
                        Contexto.DetallePedidos.DeleteOnSubmit(item);
                    }
                    DetallesEliminar.Add(item);
                }
            }


            foreach (DetallePedido item in DetallesEliminar)
            {
                cabecera.DetallePedidos.Remove(item);
            }


            /// Elimino el producto Catálogo x 50 unidades  (2506600030089)
            List<DetallePedido> detallesCatalogox50 = (from d in cabecera.DetallePedidos
                                                       where d.Presentacion == 7102
                                                       select d).ToList();

            if (detallesCatalogox50.Count > 0)
            {
                foreach (var item in detallesCatalogox50)
                {
                    if (item.IdDetallePedido != 0)
                    {
                        Contexto.DetallePedidos.DeleteOnSubmit(item);
                    }
                    DetallesEliminar.Add(item);
                }
            }


            foreach (DetallePedido item in DetallesEliminar)
            {
                cabecera.DetallePedidos.Remove(item);
            }

        }

        #endregion
    }

    private void ReemplazarProductos(CabeceraPedido cabecera)
    {
        List<DetallePedido> nuevosDetalles = new List<DetallePedido>();
        foreach (DetallePedido det in cabecera.DetallePedidos)
        {
            if (det.Presentacion == 5472)
            {
                // 5472: idPresentacino kit Deditos Sticker y Ambientador
                /// Si esta este producto y se esta realizando el pedido, no se debe
                /// agregar este producto sino que se tiene que reemplazar por:
                /// 1. Stickers Deditos $10 (2041600039001-000-00 ) - IdPre: 5479
                /// 2. SM Ambientador Automático Analógico x unidad (1.04.1600.116.002-000-00 ) - IdPre: 5486

                Presentacion StickersDeditos = (from P in Contexto.Presentacions
                                                where P.IdPresentacion == 5532
                                                select P).FirstOrDefault<Presentacion>();

                Presentacion AmbientadorAutomaticoAnalogico = (from P in Contexto.Presentacions
                                                               where P.IdPresentacion == 6707
                                                               select P).FirstOrDefault<Presentacion>();


                DetallePedido newDetalleST = new DetallePedido();
                newDetalleST.Cantidad = det.Cantidad;
                newDetalleST.CodigoCompleto = StickersDeditos.Codigo;
                newDetalleST.Presentacion = StickersDeditos.IdPresentacion;
                newDetalleST.Producto = StickersDeditos.objProducto.IdProducto;
                newDetalleST.ValorUnitario = StickersDeditos.Precio;
                newDetalleST.ValorTotal = det.Cantidad * StickersDeditos.Precio;
                nuevosDetalles.Add(newDetalleST);


                DetallePedido newDetallePlacerAD = new DetallePedido();
                newDetallePlacerAD.Cantidad = det.Cantidad;
                newDetallePlacerAD.CodigoCompleto = AmbientadorAutomaticoAnalogico.Codigo;
                newDetallePlacerAD.Presentacion = AmbientadorAutomaticoAnalogico.IdPresentacion;
                newDetallePlacerAD.Producto = AmbientadorAutomaticoAnalogico.objProducto.IdProducto;
                newDetallePlacerAD.ValorUnitario = AmbientadorAutomaticoAnalogico.Precio;
                newDetallePlacerAD.ValorTotal = det.Cantidad * AmbientadorAutomaticoAnalogico.Precio;
                nuevosDetalles.Add(newDetallePlacerAD);


            }
            else if (det.Presentacion == 5473)
            {
                // 5473: idPresentacino kit Inquietos Sticker y Ambientador
                /// Si esta este producto y se esta realizando el pedido, no se debe
                /// agregar este producto sino que se tiene que reemplazar por:
                /// 1. Stickers Inquietos $10 (2051600039001-000-00 ) - IdPre: 5480
                /// 2. SM Ambientador Automático Analógico x unidad (1.04.1600.116.002-000-00 ) - IdPre: 5486

                Presentacion StickersInquietos = (from P in Contexto.Presentacions
                                                  where P.IdPresentacion == 5533
                                                  select P).FirstOrDefault<Presentacion>();

                Presentacion AmbientadorAutomaticoAnalogico = (from P in Contexto.Presentacions
                                                               where P.IdPresentacion == 6707
                                                               select P).FirstOrDefault<Presentacion>();


                DetallePedido newDetalleSI = new DetallePedido();
                newDetalleSI.Cantidad = det.Cantidad;
                newDetalleSI.CodigoCompleto = StickersInquietos.Codigo;
                newDetalleSI.Presentacion = StickersInquietos.IdPresentacion;
                newDetalleSI.Producto = StickersInquietos.objProducto.IdProducto;
                newDetalleSI.ValorUnitario = StickersInquietos.Precio;
                newDetalleSI.ValorTotal = det.Cantidad * StickersInquietos.Precio;
                nuevosDetalles.Add(newDetalleSI);

                DetallePedido newDetallePlacerAD = new DetallePedido();
                newDetallePlacerAD.Cantidad = det.Cantidad;
                newDetallePlacerAD.CodigoCompleto = AmbientadorAutomaticoAnalogico.Codigo;
                newDetallePlacerAD.Presentacion = AmbientadorAutomaticoAnalogico.IdPresentacion;
                newDetallePlacerAD.Producto = AmbientadorAutomaticoAnalogico.objProducto.IdProducto;
                newDetallePlacerAD.ValorUnitario = AmbientadorAutomaticoAnalogico.Precio;
                newDetallePlacerAD.ValorTotal = det.Cantidad * AmbientadorAutomaticoAnalogico.Precio;
                nuevosDetalles.Add(newDetallePlacerAD);


            }
            else if (det.CodigoCompleto.Trim() == "2506600030159")
            {

                /// /// /// ///  Codigo Anterior /// /// /// ///  
                // 1041600116   -000-00: Catálogo x 10 unidades 
                /// Si esta este producto y se esta realizando el pedido, no se debe
                /// agregar este producto sino que se tiene que reemplazar por:
                //  2.50.6600.030.076        Catálogo SM x 5 u 2015 01        (2 artículos de este que sumen las 10 unidades) 
                //  2.15.0000.021.006        Descuento Catálogo x 10 u. 2015 01        (sólo 1 artículo de este) 


                /// /// /// ///  Cambio 31/03/2015 /// /// /// ///  
                /// Nuevo Producto Inicial: 2506600030082 Catálogo SM x 10 2015 02 
                /// Si esta este producto y se esta realizando el pedido, no se debe
                /// agregar este producto sino que se tiene que reemplazar por:
                /// 2.50.6600.030.079       Catálogo SM x 5 u 2015 01        (2 artículos de este que sumen las 10 unidades) 
                /// 2.15.0000.021.006       Descuento Catálogo x 10 u. 2015 01        (sólo 1 artículo de este) 


                /// /// /// ///  Cambio 16/06/2015 /// /// /// ///  
                /// Nuevo Producto Inicial: 2506600030085  Catálogo SM x 10 2015 03 
                /// Si esta este producto y se esta realizando el pedido, no se debe
                /// agregar este producto sino que se tiene que reemplazar por:
                /// 2506600030086        Catálogo SM x 5 u 2015 03        (2 artículos de este que sumen las 10 unidades) 
                /// 2150000021006       Descuento Catálogo x 10 u. 2015 01        (sólo 1 artículo de este) 


                 /// /// /// ///  Cambio 24/09/2015 /// /// /// ///  
                /// Nuevo Producto Inicial: 2506600030159 Catálogo SM x 10 u 2015 04 
                /// Si esta este producto y se esta realizando el pedido, no se debe
                /// agregar este producto sino que se tiene que reemplazar por:
                /// 2506600030158  Catálogo SM x 5 u 2015 04        (2 artículos de este que sumen las 10 unidades) 
                /// 2150000021006  Descuento Catoalogo x 10 u.        (sólo 1 artículo de este) 
                





                Presentacion preCatalogoSM = (from P in Contexto.Presentacions
                                              where P.Codigo == "2506600030158"
                                              select P).FirstOrDefault<Presentacion>();

                Presentacion preDescuentoCatalogo = (from P in Contexto.Presentacions
                                                     where P.Codigo == "2150000021006"
                                                     select P).FirstOrDefault<Presentacion>();


                DetallePedido newDetalleSI = new DetallePedido();
                newDetalleSI.Cantidad = det.Cantidad * 2;
                newDetalleSI.CodigoCompleto = preCatalogoSM.Codigo;
                newDetalleSI.Presentacion = preCatalogoSM.IdPresentacion;
                newDetalleSI.Producto = preCatalogoSM.objProducto.IdProducto;
                newDetalleSI.ValorUnitario = preCatalogoSM.Precio;
                newDetalleSI.ValorTotal = newDetalleSI.Cantidad * preCatalogoSM.Precio;
                nuevosDetalles.Add(newDetalleSI);

                DetallePedido newDetallePlacerAD = new DetallePedido();
                newDetallePlacerAD.Cantidad = det.Cantidad;
                newDetallePlacerAD.CodigoCompleto = preDescuentoCatalogo.Codigo;
                newDetallePlacerAD.Presentacion = preDescuentoCatalogo.IdPresentacion;
                newDetallePlacerAD.Producto = preDescuentoCatalogo.objProducto.IdProducto;
                newDetallePlacerAD.ValorUnitario = preDescuentoCatalogo.Precio * -1;
                newDetallePlacerAD.ValorTotal = det.Cantidad * preDescuentoCatalogo.Precio * -1;
                nuevosDetalles.Add(newDetallePlacerAD);


            }
            //else if (det.CodigoCompleto.Trim() == "2506600030089")
            //{

            //    /// /// /// ///  Cambio 16/06/2015 /// /// /// ///  
            //    /// Nuevo Producto Inicial: 2506600030089   Catálogo SM x 50 2015 03 
            //    /// Si esta este producto y se esta realizando el pedido, no se debe
            //    /// agregar este producto sino que se tiene que reemplazar por:
            //    /// 2506600030086        Catálogo SM x 5 u 2015 03        (2 artículos de este que sumen las 10 unidades) 
            //    /// 2150000021030       Descuento Catálogo x 50 u. (sólo 1 artículo de este) 


            //    Presentacion preCatalogoSM = (from P in Contexto.Presentacions
            //                                  where P.Codigo == "2506600030086"
            //                                  select P).FirstOrDefault<Presentacion>();

            //    Presentacion preDescuentoCatalogo = (from P in Contexto.Presentacions
            //                                         where P.Codigo == "2150000021030"
            //                                         select P).FirstOrDefault<Presentacion>();


            //    DetallePedido newDetalleSI = new DetallePedido();
            //    newDetalleSI.Cantidad = det.Cantidad * 10;
            //    newDetalleSI.CodigoCompleto = preCatalogoSM.Codigo;
            //    newDetalleSI.Presentacion = preCatalogoSM.IdPresentacion;
            //    newDetalleSI.Producto = preCatalogoSM.objProducto.IdProducto;
            //    newDetalleSI.ValorUnitario = preCatalogoSM.Precio;
            //    newDetalleSI.ValorTotal = newDetalleSI.Cantidad * preCatalogoSM.Precio;
            //    nuevosDetalles.Add(newDetalleSI);

            //    DetallePedido newDetallePlacerAD = new DetallePedido();
            //    newDetallePlacerAD.Cantidad = det.Cantidad;
            //    newDetallePlacerAD.CodigoCompleto = preDescuentoCatalogo.Codigo;
            //    newDetallePlacerAD.Presentacion = preDescuentoCatalogo.IdPresentacion;
            //    newDetallePlacerAD.Producto = preDescuentoCatalogo.objProducto.IdProducto;
            //    newDetallePlacerAD.ValorUnitario = preDescuentoCatalogo.Precio * -1;
            //    newDetallePlacerAD.ValorTotal = det.Cantidad * preDescuentoCatalogo.Precio * -1;
            //    nuevosDetalles.Add(newDetallePlacerAD);


            //}
        }

        cabecera.DetallePedidos.AddRange(nuevosDetalles);
        EliminarProductosKit(false, cabecera);

    }

    private static void CalcularSaldoPagoAnticipado(Cliente cliente, Marzzan_InfolegacyDataContext contexto)
    {
        try
        {
            decimal SaldoInformado = cliente.SaldoPagoAnticipado.Value;
            List<decimal?> PagoAnticipadoUtilizado = (from c in contexto.PedidosConCreditos
                                                      where c.Cliente == cliente.IdCliente
                                                      && c.Procesado.Value
                                                      && (c.MontoCredito.Value - c.MontoPagado.Value) >= 0
                                                      select c.MontoPagado).ToList();

            if (SaldoInformado != 0)
            {
                HttpContext.Current.Session["SaldoAnticipado"] = (SaldoInformado + PagoAnticipadoUtilizado.Sum());
            }
            else
            {
                HttpContext.Current.Session["SaldoAnticipado"] = cliente.SaldoCtaCte;
            }

        }
        catch (Exception err)
        {

        }

    }

    private static decimal TotalCompradoParaPromociones()
    {
        decimal TotalParaPromociones = 0;
        /// Para el calculo del total solo se tiene en cuenta Productos Comisionables
        /// Y se dejan fuera del monto: Descuentos promociones, Descuentos por Provincia, Flete.
        /// Calculo de los productos en las promociones Directas
        if (HttpContext.Current.Session["PromosGeneradas"] != null)
        {
            List<DetallePedido> promosTemp = (HttpContext.Current.Session["PromosGeneradas"] as List<DetallePedido>).Where(w => w.IdRelacionDetallePromo != 0).ToList();
            foreach (DetallePedido promo in promosTemp)
            {
                TotalParaPromociones += promo.colProductosRequeridos.Where(w => w.CodigoCompleto.Substring(0, 1) != "2").Sum(w => w.ValorUnitario * w.Cantidad).Value;
            }
        }

        /// Si hay promociones directas dentro del pedido y aun no se han cargado los elementos 
        /// requeridos entoces uso el valor de la promoción del detalle.
        if (TotalParaPromociones == 0 && (HttpContext.Current.Session["detPedido"] as List<DetallePedido>).Any(w => w.Tipo == "P"))
        {
            TotalParaPromociones += (from P in (HttpContext.Current.Session["detPedido"] as List<DetallePedido>)
                                     where (P.Tipo == "P")
                                     select P.ValorTotal.Value).Sum();
        }

        /// Calculo de los productos solicitados
        TotalParaPromociones += (from P in (HttpContext.Current.Session["detPedido"] as List<DetallePedido>)
                                 where (P.CodigoCompleto.Substring(0, 1) != "2" && P.Tipo == "A")
                                 select P.ValorTotal.Value).Sum();

        return TotalParaPromociones;

    }

    private decimal TotalParaPromocionesPagoAnticipado()
    {
        /// a partir del 01/01/2015 se utila esta forma de calculo para determiar
        /// si las promociones fijas de pago anticipado son válidas .

        decimal costoFlete = decimal.Parse(lblCostoFlete.Text.Replace("$", ""));
        decimal totalGeneral = decimal.Parse(txtMontoGeneral.Text.Replace("$", ""));
        //decimal subTotal = decimal.Parse(lblSubTotal.Text.Replace("$", "")); // esta valor no incluye los impuestos.

        return totalGeneral - costoFlete;
        //return subTotal - costoFlete;

    }

    private void GuardarPedido(string idPedido, bool FaltaSaldo, bool EsTemporal, bool EsPagoConTarjeta)
    {
        Contexto.CommandTimeout = 30000;
        DetallePedido newDetalle = null;
        string mensajeStock = "";

        /// Si el pedido NO es temporal, PERO el medio de pago es tarjeta, entonces lo marco como 
        /// temporal, ya que es necesario la aprobación de la misma, y grabo la operación como temporal
        /// indicando que es pago con tarjeta para aplicar la logica particular de la misma.
        if (!EsTemporal && cboFormaPago.SelectedItem.Text.Contains("Tarjeta"))
        {
            EsPagoConTarjeta = true;
            EsTemporal = false;
        }


        if (ControlStockValido(EsTemporal, out mensajeStock))
        {
            try
            {
                #region Calculo el Total Comprado
                ///// Solo tiene en cuenta los productos comisionables para el calculo, es decir
                ///// aquello donde el código comienza con un 1.
                //decimal TotalComprado = (from P in (Session["detPedido"] as List<DetallePedido>)
                //                         where (P.CodigoCompleto.Substring(0, 1) == "1" && P.Tipo == "A") || (P.Tipo == "P") || (P.Tipo == "D")
                //                         select P.ValorTotal.Value).Sum();


                //// Sumo el costo del transporte
                ////TotalComprado += decimal.Parse(lblCostoFlete.Text.Replace("$", ""));





                #endregion

                #region Calculo el Total Comprado para control PROMOCIONES


                //decimal TotalParaPromociones = TotalCompradoParaPromociones();


                ///// Recupero los montos minimos de las promociones segun su configuracion.
                //List<long> idsPromosTemp = (Session["PromosGeneradas"] as List<DetallePedido>).Select(w => w.Producto.Value).ToList();
                //var PromocionesInvalidas = (from p in Contexto.ConfPromociones
                //                            where idsPromosTemp.Contains(p.IdProductoPromo) && p.MontoMinimo > TotalParaPromociones
                //                            select new
                //                            {
                //                                Descripcion = p.objProductoPromo.Descripcion,
                //                                Monto = p.MontoMinimo
                //                            }).ToList();

                //if (PromocionesInvalidas.Count > 0)
                //{
                //    string mensajePromos = "Las siguientes promociones no pueden ser solicitadas, ya que las mismas poseen un monto mínimo establecido, el cual es mayor al monto del pedido.</br>Promociones:</br>";
                //    foreach (var item in PromocionesInvalidas)
                //    {
                //        mensajePromos += "&nbsp;&nbsp;&nbsp;" + item.Descripcion + " <b>Monto Mínimo: " + string.Format("${0:###.0#}", item.Monto.Value) + "</b>";
                //    }

                //    ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "MinimoReqCredito", "AlertaMinimoPromociones('" + mensajePromos + "');", true);
                //    return;
                //}

                #endregion

                #region Verifico que la dirección seleccionada pertenezca al cliente seleccionado

                Direccione dirEntrega = (from D in Contexto.Direcciones
                                         where D.IdDireccion == IdDireccionSeleccionada
                                         select D).FirstOrDefault<Direccione>();

                Cliente CliSeleccionado = (from D in Contexto.Clientes
                                           where D.IdCliente == long.Parse(cboConsultores.SelectedValue)
                                           select D).First<Cliente>();



                if (dirEntrega == null || dirEntrega.CodigoExterno != CliSeleccionado.CodigoExterno)
                {
                    ucGrillaDirecciones.InitControl(CliSeleccionado.CodigoExterno);
                    lblDireccionEntrega.Text = "";
                    lblCalle.Text = "";
                    IdDireccionSeleccionada = 0;
                    ProvinciaDireccionSeleccionada = "";
                    upEncImp.Update();
                    ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "VerificacionDireccion", "AlertaDireccion();", true);
                    return;

                }

                #endregion

                #region Busco el limite de compra por provincia
                //decimal LimitePorProvincia = 0;




                //var LimiteCompraProvincia = (from P in Contexto.LimitesDeCompras
                //                             where P.Provincia == dirEntrega.Provincia && P.Localidad == dirEntrega.Localidad
                //                             select P.Limite).FirstOrDefault();

                ///// si es null significa que no hay una definicion especifica
                ///// para localidad y provincia entonces busco solo para la provincia.
                //if (LimiteCompraProvincia == null)
                //{
                //    LimiteCompraProvincia = (from P in Contexto.LimitesDeCompras
                //                             where P.Provincia == dirEntrega.Provincia
                //                             select P.Limite).FirstOrDefault();

                //    if (LimiteCompraProvincia != null)
                //        LimitePorProvincia = LimiteCompraProvincia.Value;
                //}
                //else
                //{
                //    LimitePorProvincia = LimiteCompraProvincia.Value;
                //}

                #endregion

                #region Busco el limite de compra general
                //string LimiteCompraGeneral = (from P in Session["ParametrosSistema"] as List<Parametro>
                //                              where P.IdParametro == (int)TiposDeParametros.LimiteCompra
                //                              select P.Valor).Single();

                #endregion

                #region Busco el limite de compra en Contra Reembolso
                //string LimiteContraReembolso = (from P in Session["ParametrosSistema"] as List<Parametro>
                //                                where P.IdParametro == (int)TiposDeParametros.LimiteContraReembolso
                //                                select P.Valor).Single();

                #endregion

                #region Control LIMITE GENERAL
                //if ((!EsTemporal && !EsClienteEspecial))
                //{

                //    if (TipoCliente.ToString() != Convert.ToString((int)TipoClientes.PotencialBolso))
                //    {
                //        /// ESTE CONTROL NO SE REALIZA MAS SOBRE LOS PEDIDOS COMISIONABLES
                //        /// SINO QUE SE DEBE HACER SOBRE EL TOTAL DEL PEDIDO. MAIL: 30/10/2014
                //        ///// Control LIMITE POR PROVINCIA
                //        //if (TotalComprado < LimitePorProvincia)
                //        //{
                //        //    ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "MinimoReq", "AlertaMinimoRequeridoProvincia(" + LimitePorProvincia + ");", true);
                //        //    return;
                //        //}


                //        /// Control LIMITE GENERAL
                //        if (!cboConsultores.Text.Contains("bolsos"))
                //        {
                //            if (TotalComprado < int.Parse(LimiteCompraGeneral))
                //            {
                //                ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "MinimoReq", "AlertaMinimoRequerido(" + LimiteCompraGeneral + ");", true);
                //                return;
                //            }
                //        }
                //    }
                //}
                #endregion

                #region Control LIMITE EN CREDITO
                //if ((!EsTemporal && !EsClienteEspecial))
                //{
                //    if (cboFormaPago.SelectedItem.Text == "Crédito")
                //    {
                //        decimal MontoDisponibleCredito = decimal.Parse(Session["MontoDisponibleCredito"].ToString());
                //        if (TotalComprado > MontoDisponibleCredito)
                //        {
                //            ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "MinimoReqCredito", "AlertaMinimoRequeridoCredito(" + string.Format("{0:0.00}", MontoDisponibleCredito) + ");", true);
                //            return;
                //        }
                //    }
                //}
                #endregion

                #region Generacion de la Cabecera del Pedido
                CabeceraPedido cabecera = null;

                if (idPedido != "")
                {

                    cabecera = (from C in Contexto.CabeceraPedidos
                                where C.IdCabeceraPedido == long.Parse(Request.QueryString["IdPedido"].ToString())
                                select C).SingleOrDefault();

                    cabecera.DireccionEntrega = IdDireccionSeleccionada;
                    cabecera.FormaPago = long.Parse(cboFormaPago.SelectedValue);
                    cabecera.Retira = txtObservacion.Text;
                    cabecera.FechaPedido = DateTime.Now;
                    cabecera.PorcentajeDescuentoProvincia = Convert.ToDecimal(lblProvinciaPorcentajeDescuentoHidden.Value);
                    cabecera.DescuentoProvincia = Convert.ToDecimal(lblDescuentoProvincia.Text.Replace("$", ""));
                    cabecera.DescuentosGenerales = Convert.ToDecimal(lblDescuentosGenerales.Text.Replace("$", ""));

                }
                else
                {
                    cabecera = new CabeceraPedido();
                    cabecera.Cliente = long.Parse(cboConsultores.SelectedValue);
                    cabecera.ClienteSolicitante = long.Parse(Session["idUsuario"].ToString());
                    cabecera.DireccionEntrega = IdDireccionSeleccionada;
                    cabecera.FechaPedido = DateTime.Now;
                    cabecera.FormaPago = long.Parse(cboFormaPago.SelectedValue);
                    cabecera.MontoTotal = 0;
                    cabecera.Retira = txtObservacion.Text;
                    cabecera.Nro = ObtenerSeguienteNro(0, Contexto);
                    cabecera.TipoPedido = "NP";
                    cabecera.Impreso = false;
                    cabecera.NroImpresion = 0;
                    cabecera.PorcentajeDescuentoProvincia = Convert.ToDecimal(lblProvinciaPorcentajeDescuentoHidden.Value);
                    cabecera.DescuentoProvincia = Convert.ToDecimal(lblDescuentoProvincia.Text.Replace("$", ""));
                    cabecera.DescuentosGenerales = Convert.ToDecimal(lblDescuentosGenerales.Text.Replace("$", ""));

                }

                /// Cuando el pago es con tarjeta guardo el estado inicial, para determinar luego 
                /// si se cargo los datos del pago.
                if (EsPagoConTarjeta)
                {
                    cabecera.EstadoOperacionTarjeta = "Inicial";
                    cabecera.Tarjeta = long.Parse(cboTarjetas.SelectedItem.Value);
                    cabecera.Cuotas = Convert.ToInt32(txtCuotas.Value);

                }
                else
                {
                    cabecera.Tarjeta = null;
                    cabecera.Cuotas = null;
                    cabecera.EstadoOperacionTarjeta = "";
                }


                #endregion

                #region Generacion de los Detalle

                #region Eliminacion Detalle Temporales

                if (idPedido != "")
                {
                    List<DetallePedido> DetallesEliminar = new List<DetallePedido>();

                    /// Elimino los producto de regalos guardados ya que se volveran a generar.
                    DetallesEliminar.AddRange(cabecera.DetallePedidos.Where(w => w.objPromocionOrigen != null));

                    /// Elimino la promociones guardadas pero NO las promociones del detalle, es decir
                    /// aquellas que fueron solicitadas en forma directa,
                    DetallesEliminar.AddRange(cabecera.DetallePedidos.Where(w =>
                                            (w.objProducto.Tipo != 'A' &&
                                            w.objProducto.Tipo != 'G' &&
                                            w.objProducto.Tipo != 'R' &&
                                            w.objProducto.Tipo != 'N' &&
                                            w.objProducto.Tipo != 'I') && ((w.objProducto.Tipo == 'P' || w.objProducto.Tipo == 'D') && w.ValorTotal < 0))
                        );



                    Contexto.DetallePedidos.DeleteAllOnSubmit(DetallesEliminar);



                    /// Elimino las bolsas de papel
                    DetallePedido detalleBolsas = (from d in cabecera.DetallePedidos
                                                   where d.CodigoCompleto == "2520000018001"
                                                   select d).FirstOrDefault();
                    if (detalleBolsas != null)
                    {
                        Contexto.DetallePedidos.DeleteOnSubmit(detalleBolsas);
                        DetallesEliminar.Add(detalleBolsas);
                    }


                    /// Elimino las bolsas de papel chica de la linea Images
                    DetallePedido detalleBolsasCH = (from d in cabecera.DetallePedidos
                                                     where d.CodigoCompleto == "2520000018090"
                                                     select d).FirstOrDefault();
                    if (detalleBolsasCH != null)
                    {
                        Contexto.DetallePedidos.DeleteOnSubmit(detalleBolsasCH);
                        DetallesEliminar.Add(detalleBolsasCH);
                    }





                    /// Elimino las bolsas de papel del So Pink
                    DetallePedido detalleBolsasSoPink = (from d in cabecera.DetallePedidos
                                                         where d.CodigoCompleto.Trim() == "2500000018045"
                                                         select d).FirstOrDefault();

                    if (detalleBolsasSoPink != null)
                    {
                        Contexto.DetallePedidos.DeleteOnSubmit(detalleBolsasSoPink);
                        DetallesEliminar.Add(detalleBolsasSoPink);
                    }



                    /// Elimino los gatillos
                    DetallePedido detalleGatillos = (from d in cabecera.DetallePedidos
                                                     where d.CodigoCompleto == "2520000032073"
                                                     select d).FirstOrDefault();
                    if (detalleGatillos != null)
                    {
                        Contexto.DetallePedidos.DeleteOnSubmit(detalleGatillos);
                        DetallesEliminar.Add(detalleGatillos);
                    }


                    /// Elimino las Varillas
                    DetallePedido detalleVarillas = (from d in cabecera.DetallePedidos
                                                     where d.CodigoCompleto == "2520700110010"
                                                     select d).FirstOrDefault();
                    if (detalleVarillas != null)
                    {
                        Contexto.DetallePedidos.DeleteOnSubmit(detalleVarillas);
                        DetallesEliminar.Add(detalleVarillas);
                    }

                    /// Elimino el gasto de Envio
                    DetallePedido detalleGastoEnvio = (from d in cabecera.DetallePedidos
                                                       where d.objProducto.Tipo == 'G'
                                                       select d).FirstOrDefault();

                    if (detalleGastoEnvio != null)
                    {
                        Contexto.DetallePedidos.DeleteOnSubmit(detalleGastoEnvio);
                        DetallesEliminar.Add(detalleGastoEnvio);
                    }


                    foreach (DetallePedido item in DetallesEliminar)
                    {
                        cabecera.DetallePedidos.Remove(item);
                    }

                    Contexto.SubmitChanges();


                }


                #endregion

                #region Detalles de Pedido
                foreach (DetallePedido det in (Session["detPedido"] as List<DetallePedido>))
                {
                    if (det.IdDetallePedido == 0)
                    {

                        /// Si se trata de un producto de tipo promocion
                        /// entonces no genero el producto y lo reemplazo por los
                        /// elementos requeridos de la promoción.
                        if ((!EsTemporal) && det.IdRelacionDetallePromo != 0)
                        {

                            List<DetallePedido> promos = (Session["PromosGeneradas"] as List<DetallePedido>).Where(w => w.IdRelacionDetallePromo == det.IdRelacionDetallePromo).ToList();
                            foreach (DetallePedido promo in promos)
                            {
                                foreach (DetalleProductosRequeridos item in promo.colProductosRequeridos)
                                {

                                    newDetalle = cabecera.DetallePedidos.Where(w => w.Producto.Value == item.IdProducto && w.Presentacion.Value == item.IdPresentacion).FirstOrDefault();

                                    if (newDetalle == null)
                                    {
                                        newDetalle = new DetallePedido();
                                        newDetalle.Cantidad = item.Cantidad;
                                        newDetalle.CodigoCompleto = item.CodigoCompleto;
                                        newDetalle.Presentacion = item.IdPresentacion;
                                        newDetalle.Producto = item.IdProducto;
                                        newDetalle.ValorUnitario = item.ValorUnitario;
                                        newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;
                                        cabecera.DetallePedidos.Add(newDetalle);
                                    }
                                    else
                                    {
                                        newDetalle.Cantidad += item.Cantidad;
                                        newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;
                                        cabecera.DetallePedidos.Add(newDetalle);
                                    }


                                }
                            }


                        }
                        //else if (!EsTemporal && (det.Presentacion == 5473 || det.Presentacion == 5472))
                        //{
                        //    ReemplazarProductos(det, cabecera);
                        //}
                        else
                        {
                            newDetalle = new DetallePedido();
                            newDetalle.Cantidad = long.Parse(det.Cantidad.ToString());
                            newDetalle.CodigoCompleto = det.CodigoCompleto;
                            newDetalle.Presentacion = det.Presentacion;
                            newDetalle.Producto = det.Producto.Value;
                            newDetalle.ValorUnitario = det.ValorUnitario;
                            newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;
                            cabecera.DetallePedidos.Add(newDetalle);


                            /// Asocio al detalle de tipo promo los productos requeridos seleccionados
                            /// para luego poder reconstruir cuando se edite
                            if (det.IdRelacionDetallePromo != 0 && det.Tipo == "P" && EsTemporal)
                            {
                                List<DetallePedido> promos = (Session["PromosGeneradas"] as List<DetallePedido>).Where(w => w.IdRelacionDetallePromo == det.IdRelacionDetallePromo).ToList();
                                int grupo = 0;
                                foreach (DetallePedido promo in promos)
                                {
                                    foreach (DetalleProductosRequeridos item in promo.colProductosRequeridos)
                                    {
                                        DetalleProductosRequeridos detReq = new DetalleProductosRequeridos();
                                        detReq.Cantidad = item.Cantidad;
                                        detReq.IdProducto = item.IdProducto;
                                        detReq.IdPresentacion = item.IdPresentacion;
                                        detReq.ValorUnitario = item.ValorUnitario;
                                        detReq.CodigoCompleto = item.CodigoCompleto;
                                        detReq.objDetallePedido = newDetalle;
                                        detReq.Grupo = grupo;
                                        detReq.Tipo = item.Tipo;
                                        detReq.DescripcionProducto = item.DescripcionProducto;
                                    }

                                    grupo++;
                                }
                            }

                        }
                    }
                    /// Si es un pedido guardado y que se esta editando
                    /// se aplica est logica.
                    else
                    {

                        DetallePedido detGuardado = (from D in cabecera.DetallePedidos
                                                     where D.IdDetallePedido == det.IdDetallePedido
                                                     select D).FirstOrDefault<DetallePedido>();
                        if (detGuardado != null)
                        {

                            detGuardado.Cantidad = long.Parse(det.Cantidad.ToString());
                            detGuardado.ValorUnitario = det.ValorUnitario;
                            detGuardado.ValorTotal = detGuardado.ValorUnitario * detGuardado.Cantidad;


                            /// Asocio al detalle de tipo promo los productos requeridos seleccionados
                            /// para luego poder reconstruir cuando se edite
                            if (detGuardado.IdRelacionDetallePromo != 0 && (det.Tipo == "P" || det.Tipo == "D"))
                            {
                                if (EsTemporal)
                                {
                                    /// Elimino el detalle existente y los vuelvo a generar para reflejar los cambios posibles.
                                    Contexto.DetalleProductosRequeridos.DeleteAllOnSubmit(detGuardado.colProductosRequeridos);

                                    /// vuelvo a generar para reflejar los cambios posibles.
                                    List<DetallePedido> promos = (Session["PromosGeneradas"] as List<DetallePedido>).Where(w => w.IdRelacionDetallePromo == det.IdRelacionDetallePromo).ToList();
                                    int grupo = 0;

                                    foreach (DetallePedido promo in promos)
                                    {
                                        foreach (DetalleProductosRequeridos item in promo.colProductosRequeridos)
                                        {
                                            DetalleProductosRequeridos detReq = new DetalleProductosRequeridos();
                                            detReq.Cantidad = item.Cantidad;
                                            detReq.IdProducto = item.IdProducto;
                                            detReq.IdPresentacion = item.IdPresentacion;
                                            detReq.ValorUnitario = item.ValorUnitario;
                                            detReq.CodigoCompleto = item.CodigoCompleto;
                                            detReq.objDetallePedido = detGuardado;
                                            detReq.Grupo = grupo;
                                            detReq.Tipo = item.Tipo;
                                            detReq.DescripcionProducto = item.DescripcionProducto;
                                        }

                                        grupo++;
                                    }
                                }
                                else
                                {

                                    List<DetallePedido> promos = (Session["PromosGeneradas"] as List<DetallePedido>).Where(w => w.IdRelacionDetallePromo == detGuardado.IdRelacionDetallePromo).ToList();
                                    foreach (DetallePedido promo in promos)
                                    {
                                        foreach (DetalleProductosRequeridos item in promo.colProductosRequeridos)
                                        {

                                            newDetalle = cabecera.DetallePedidos.Where(w => w.Producto.Value == item.IdProducto && w.Presentacion.Value == item.IdPresentacion).FirstOrDefault();

                                            if (newDetalle == null)
                                            {
                                                newDetalle = new DetallePedido();
                                                newDetalle.Cantidad = item.Cantidad;
                                                newDetalle.CodigoCompleto = item.CodigoCompleto;
                                                newDetalle.Presentacion = item.IdPresentacion;
                                                newDetalle.Producto = item.IdProducto;
                                                newDetalle.ValorUnitario = item.ValorUnitario;
                                                newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;
                                                cabecera.DetallePedidos.Add(newDetalle);
                                            }
                                            else
                                            {
                                                newDetalle.Cantidad += item.Cantidad;
                                                newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;
                                                cabecera.DetallePedidos.Add(newDetalle);
                                            }


                                        }
                                    }


                                    /// Elimino el detalle pedido de tipo promocion, ya que fue reemplazo por los 
                                    /// productos requeridos que lo componen.
                                    Contexto.DetallePedidos.DeleteOnSubmit(detGuardado);
                                    cabecera.DetallePedidos.Remove(detGuardado);
                                }
                            }
                        }
                    }
                }


                #endregion

                #region Reemplazo de Productos Especificos
                if (!EsTemporal)
                {
                    ReemplazarProductos(cabecera);
                }
                #endregion

                #region Generacion de las promociones del Pedido
                if ((Session["PromosGeneradas"] as List<DetallePedido>) != null)
                {
                    foreach (DetallePedido det in (Session["PromosGeneradas"] as List<DetallePedido>))
                    {
                        Producto ProductoPromo = (from P in Contexto.Productos
                                                  where P.IdProducto == det.Producto
                                                  select P).First<Producto>();


                        DetallePedido newDetallePromo = new DetallePedido();
                        newDetallePromo.Cantidad = long.Parse(det.Cantidad.ToString());
                        newDetallePromo.CodigoCompleto = det.CodigoCompleto;
                        newDetallePromo.Presentacion = det.Presentacion;
                        newDetallePromo.Producto = det.Producto;
                        newDetallePromo.ValorUnitario = ProductoPromo.Precio * -1;
                        newDetallePromo.ValorTotal = newDetallePromo.ValorUnitario * newDetallePromo.Cantidad;
                        cabecera.DetallePedidos.Add(newDetallePromo);

                        foreach (DetalleRegalos detRegalo in det.ColRegalos)
                        {

                            if (detRegalo.TipoRegalo == "Producto" && detRegalo.IdPresentacionRegaloSeleccionado > 0)
                            {

                                Presentacion PresentacionRegalo = (from P in Contexto.Presentacions
                                                                   where P.IdPresentacion == detRegalo.IdPresentacionRegaloSeleccionado
                                                                   select P).First<Presentacion>();


                                newDetalle = new DetallePedido();
                                newDetalle.Cantidad = detRegalo.Cantidad;// long.Parse(det.Cantidad.ToString());
                                newDetalle.CodigoCompleto = PresentacionRegalo.Codigo;
                                newDetalle.Presentacion = PresentacionRegalo.IdPresentacion;
                                newDetalle.Producto = PresentacionRegalo.objProducto.IdProducto;
                                newDetalle.objPromocionOrigen = newDetallePromo;

                                if (PresentacionRegalo.objProducto.Tipo.ToString() == "A")
                                {
                                    newDetalle.ValorUnitario = PresentacionRegalo.Precio;
                                }
                                else
                                {
                                    newDetalle.ValorUnitario = PresentacionRegalo.objProducto.Precio;
                                }


                                newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;
                                cabecera.DetallePedidos.Add(newDetalle);

                            }
                        }

                        det.ColRegalos = new System.Data.Linq.EntitySet<DetalleRegalos>();



                    }
                }
                #endregion

                #region Generacion detalle Bolsas de Papel de Regalo
                // Id del producto imagen: 5586 valor fijo en la aplicación
                string[] CodigosLlevanBolsas = (from N in Contexto.Presentacions
                                                where N.objProducto.Tipo == 'A'
                                                && !(N.objProducto.objPadre.objPadre.IdProducto == 5586)
                                                select N.Codigo).ToArray<string>();


                long CantidadArticulosBolsa = (from N in cabecera.DetallePedidos
                                               where CodigosLlevanBolsas.Contains(N.CodigoCompleto)
                                               && N.CodigoCompleto.Substring(0, 1) == "1"
                                               select N.Cantidad.Value).Sum();

                DateTime fechaImplementacion = DateTime.Parse("13/01/2013");
                if (DateTime.Now > fechaImplementacion)
                    CantidadArticulosBolsa = Convert.ToInt64(CantidadArticulosBolsa / 4);
                else
                    CantidadArticulosBolsa = Convert.ToInt64(CantidadArticulosBolsa / 2);



                if (CantidadArticulosBolsa > 0)
                {

                    Presentacion pre = (from P in Contexto.Presentacions
                                        where P.Codigo == "2520000018001"
                                        select P).SingleOrDefault();

                    newDetalle = new DetallePedido();
                    newDetalle.Cantidad = CantidadArticulosBolsa;
                    newDetalle.CodigoCompleto = pre.Codigo;
                    newDetalle.Presentacion = pre.IdPresentacion;
                    newDetalle.Producto = pre.objProducto.IdProducto;
                    newDetalle.ValorUnitario = 0; //Se pidio que el valor sea 0: 26/05/2015 pre.Precio;
                    newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;

                    cabecera.DetallePedidos.Add(newDetalle);
                }

                #endregion

                #region Generacion detalle Bolsas chica para la linea IMAGEN
                // Id del producto imagen: 5586 valor fijo en la aplicación

                string[] CodigosLlevanBolsasChica = (from N in Contexto.Presentacions
                                                     where N.objProducto.Tipo == 'A' && N.objProducto.objPadre.objPadre.IdProducto == 5586
                                                     select N.Codigo).ToArray<string>();


                long CantidadArticulosBolsaChica = (from N in cabecera.DetallePedidos
                                                    where CodigosLlevanBolsasChica.Contains(N.CodigoCompleto)
                                                    && N.CodigoCompleto.Substring(0, 1) == "1"
                                                    select N.Cantidad.Value).Sum();

                CantidadArticulosBolsaChica = Convert.ToInt64(CantidadArticulosBolsaChica / 4);



                if (CantidadArticulosBolsaChica > 0)
                {

                    Presentacion pre = (from P in Contexto.Presentacions
                                        where P.Codigo == "2520000018090"
                                        select P).SingleOrDefault();

                    newDetalle = new DetallePedido();
                    newDetalle.Cantidad = CantidadArticulosBolsaChica;
                    newDetalle.CodigoCompleto = pre.Codigo;
                    newDetalle.Presentacion = pre.IdPresentacion;
                    newDetalle.Producto = pre.objProducto.IdProducto;
                    newDetalle.ValorUnitario = pre.Precio;
                    newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;

                    cabecera.DetallePedidos.Add(newDetalle);
                }

                #endregion

                #region Generacion detalle Gatillos Para Embases de 1000 ml

                string[] CodigosLlevanGatillo = (from N in Contexto.Presentacions
                                                 where N.objProducto.Tipo == 'A'
                                                 && N.Descripcion.Contains("1000 ml")
                                                 select N.Codigo).ToArray<string>();


                long CantidadArticulosGatillos = Convert.ToInt64(((from N in cabecera.DetallePedidos
                                                                   where CodigosLlevanGatillo.Contains(N.CodigoCompleto)
                                                                && N.CodigoCompleto.Substring(0, 1) == "1"
                                                                   select N.Cantidad.Value).Sum() * 1));


                if (CantidadArticulosGatillos > 0)
                {

                    Presentacion pre = (from P in Contexto.Presentacions
                                        where P.Codigo == "2520000032073"
                                        select P).SingleOrDefault();

                    newDetalle = new DetallePedido();
                    newDetalle.Cantidad = CantidadArticulosGatillos;
                    newDetalle.CodigoCompleto = pre.Codigo;
                    newDetalle.Presentacion = pre.IdPresentacion;
                    newDetalle.Producto = pre.objProducto.IdProducto;
                    newDetalle.ValorUnitario = pre.Precio;
                    newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;

                    cabecera.DetallePedidos.Add(newDetalle);
                }

                #endregion

                #region  Detalle Varillas de Bambú
                // Producto Difusor: 4868
                // Presentaciones con descripcion: Repuesto

                string[] CodigosRepuestosDifusores = new string[] { "1010700110   -194-20 ", "1010700110   -195-20 ", "1010700110   -196-20 ", "1010700110   -197-20 ", "1010700110   -215-20 ", "1010700110   -209-20 ", "1010700110   -310-20 " };

                long CantidadRepuestos = Convert.ToInt64(((from N in cabecera.DetallePedidos
                                                           where CodigosRepuestosDifusores.Contains(N.CodigoCompleto)
                                                           select N.Cantidad.Value).Sum() * 1));


                if (CantidadRepuestos > 0)
                {

                    Presentacion preVarilla = (from P in Contexto.Presentacions
                                               where P.Codigo == "2520700110010"
                                               select P).SingleOrDefault();


                    newDetalle = new DetallePedido();
                    newDetalle.Cantidad = CantidadRepuestos;
                    newDetalle.CodigoCompleto = preVarilla.Codigo;
                    newDetalle.Presentacion = preVarilla.IdPresentacion;
                    newDetalle.Producto = preVarilla.objProducto.IdProducto;
                    newDetalle.ValorUnitario = preVarilla.Precio;
                    newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;

                    cabecera.DetallePedidos.Add(newDetalle);
                }

                #endregion

                #region  Detalle Gatillo Aromatizador 500 ml
                /// Cambio solicitado 30/09/2015: Se desactiva los gatillos por productos xq los mismos se cobras desde esta fecha.
                //if (!EsTemporal)
                //{

                //    string[] CodigosAromatizadoresEspeciales = new string[] { "1054300001   -148-50" };
                //        //, "1010100001   -009-50 ", "1010300001   -142-50 ", "1010100001   -003-50 ", "1010507001   -203-50 "
                //    //    , "1010100001   -012-50 ", "1010300001   -244-50 ", "1010300001   -021-50 ", "1010200001   -015-50 ", "1010100001   -017-50 ", "1010100001   -011-50 ","1010200001   -017-50 ","1010300001   -026-50 "
                //    //    ,"1010100001   -001-50 ","1010100001   -005-50 ","1010100001   -008-50 ","1010100001   -131-50 ","101100001   -013-50 ","1010300001   -025-50 ","1010300001   -024-50 ","1010300001   -027-50 ","1010300001   -028-50 ","1010300001   -180-50 ","1010300001   -152-50 ","1010300001   -022-50 ","1010300001   -023-50 ","1010504001   -115-50 "
                //    //    ,"1010100001   -013-50 ","1010100001   -130-50 ","1010200001   -014-50 ","1010200001   -016-50 ","1010100001   -007-50 ","1010100001   -002-50 ","1010100001   -109-50 ","1010501001   -116-50 "};
                   
                //    // 1493	01	Fresh
                //    // 1507	02	Aromaterapia
                //    // 1513	03	Selectivo
                //    // 1522	04	Aromas de la Casa

                //    var IdLineasIncluidas = (from p in Contexto.Productos
                //                             where p.Padre == 1493 || p.Padre == 1507 || p.Padre == 1513 || p.Padre == 1525
                //                             select p.IdProducto).ToList();


                //    string[] CodigosAromatizadores = (from p in Contexto.Presentacions
                //                                      where (IdLineasIncluidas.Contains(p.Padre.Value) && p.Descripcion == "500 ml" && p.Codigo != "1010300001   -133-50 ")
                //                                      select p.Codigo.Trim()).ToArray();


                //    long CantidadAromatizador = Convert.ToInt64(((from N in cabecera.DetallePedidos
                //                                                  where CodigosAromatizadores.Contains(N.CodigoCompleto.Trim()) ||
                //                                                  CodigosAromatizadoresEspeciales.Contains(N.CodigoCompleto.Trim())
                //                                                  select N.Cantidad.Value).Sum() * 1));


                //    if (CantidadAromatizador > 0)
                //    {

                //        Presentacion preGatilloAromatizador = (from P in Contexto.Presentacions
                //                                               where P.Codigo == "2500000032001"
                //                                               select P).SingleOrDefault();


                //        newDetalle = new DetallePedido();
                //        newDetalle.Cantidad = CantidadAromatizador;
                //        newDetalle.CodigoCompleto = preGatilloAromatizador.Codigo;
                //        newDetalle.Presentacion = preGatilloAromatizador.IdPresentacion;
                //        newDetalle.Producto = preGatilloAromatizador.objProducto.IdProducto;
                //        newDetalle.ValorUnitario = 0; // preGatilloAromatizador.Precio; Solicitado 04/02/2015
                //        newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;

                //        cabecera.DetallePedidos.Add(newDetalle);
                //    }
                //}

                #endregion

                #region  Detalle Hornillos : 26/09/2014
                /*
                 * Cambio 24/09/2014
                1.01.8500.170.003-000-00        Hornillo Geo Salmón - -        :         
                1.01.8500.170.004-000-00        Hornillo Geo Verde - -         
                1.01.8500.170.001-000-00        Hornillo Prisma - - 
                1.01.8500.170.007-000-00        Hornillo Deco - -   
                Cada vez que pidan cualquiera de los hornillos, la nota de pedido de agregar una vela por cada hornillo pedido. 

                El artículo correspondiente a la vela es 2.50.0000.110.001        Vela de Regalo Hornillo         

                 */

                if (!EsTemporal)
                {
                    string[] CodigosHornillos = new string[] { "1018500170003-000-00 ", "1018500170004-000-00 ", "1018500170001-000-00 ", "1018500170007-000-00 " };

                    long CantidadHornillos = Convert.ToInt64(((from N in cabecera.DetallePedidos
                                                               where CodigosHornillos.Contains(N.CodigoCompleto)
                                                               select N.Cantidad.Value).Sum() * 1));


                    if (CantidadHornillos > 0)
                    {

                        Presentacion preVela = (from P in Contexto.Presentacions
                                                where P.Codigo == "2500000110001"
                                                select P).SingleOrDefault();


                        newDetalle = new DetallePedido();
                        newDetalle.Cantidad = CantidadHornillos;
                        newDetalle.CodigoCompleto = preVela.Codigo;
                        newDetalle.Presentacion = preVela.IdPresentacion;
                        newDetalle.Producto = preVela.objProducto.IdProducto;
                        newDetalle.ValorUnitario = preVela.Precio;
                        newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;

                        cabecera.DetallePedidos.Add(newDetalle);
                    }
                }

                #endregion

                #region  Detalle Bolsa Lienzo LCDA
                // Producto Bolsa Lienzo LCDA: 4868
                // Presentaciones con descripcion: Repuesto

                if (!EsTemporal)
                {

                    string CodigoBolsa = "2500000018045";
                    string CodigoAromatizadorSoPink = "1010300001   -232-50 ";

                    long CantidadAromatizadore = Convert.ToInt64(((from N in cabecera.DetallePedidos
                                                                   where N.CodigoCompleto == CodigoAromatizadorSoPink
                                                                   select N.Cantidad.Value).Sum()));


                    if (CantidadAromatizadore > 0)
                    {

                        Presentacion preBolzaLienzo = (from P in Contexto.Presentacions
                                                       where P.Codigo.Trim() == CodigoBolsa
                                                       select P).FirstOrDefault();


                        if (preBolzaLienzo != null)
                        {
                            newDetalle = new DetallePedido();
                            newDetalle.Cantidad = CantidadAromatizadore;
                            newDetalle.CodigoCompleto = preBolzaLienzo.Codigo;
                            newDetalle.Presentacion = preBolzaLienzo.IdPresentacion;
                            newDetalle.Producto = preBolzaLienzo.objProducto.IdProducto;
                            newDetalle.ValorUnitario = preBolzaLienzo.Precio;
                            newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;

                            cabecera.DetallePedidos.Add(newDetalle);
                        }
                    }
                }
                #endregion

                #region  Detalle Folleto Porta Plumin
                // Producto Bolsa Lienzo LCDA: 4868
                // Presentaciones con descripcion: Repuesto

                if (!EsTemporal)
                {
                    /// Regla de Negocio:
                    /// 1. El cambio consiste en que para todos los plumines que sean o esten dentro de la linea ''Tal Cual'' , en la nota de pedido debera traer el codigo 2500000023104 Porta Plumin Tal Cual Unisex 
                    /// 2. Para todos los plumines que esten dentro de  la linea ''Sandra Marzzan Exclusiva'', en la nota de pedido no debera traer ningun tipo de leyenda. Esto se debe a que estos plumines ya salen con su porta plumin desde producicon. 
                    /// Por ultimo todos los demas que se encuentren y no pertenezcan a los puntos anteriores, en la nota de pedido debera traer el codigo 2506600030057 Folleto Portaplumin. 


                    /// Datos Fijos
                    //1540 - "Ta Ccual"
                    //1707 - "Sandra Marzzan Exclusiva "
                    //5586 - "Imagen"
                    string CodigoFolletoPortaPlumin = "2506600030057";

                    string CodigoPortaPluminTalCual = "2500000023104";


                    long CantidadPluminesTalCual = Convert.ToInt64(((from N in (List<DetallePedido>)Session["detPedido"]
                                                                     where N.PresentacionDesc.StartsWith("5 ml") && N.IdPadre == 1540
                                                                     select N.Cantidad.Value).Sum()));

                    /// Otros distintos de "Sandra Marzzan Exclusiva " e "Imagen" 
                    long CantidadPluminesOtros = Convert.ToInt64(((from N in (List<DetallePedido>)Session["detPedido"]
                                                                   where N.PresentacionDesc.StartsWith("5 ml")
                                                                   && N.IdPadre != 1540 && N.IdPadre != 1707 && N.IdPadre != 5586
                                                                   select N.Cantidad.Value).Sum()));


                    if (CantidadPluminesTalCual > 0)
                    {

                        Presentacion preCodigoPortaPlumin = (from P in Contexto.Presentacions
                                                             where P.Codigo.Trim() == CodigoPortaPluminTalCual
                                                             select P).FirstOrDefault();


                        if (preCodigoPortaPlumin != null)
                        {
                            newDetalle = new DetallePedido();
                            newDetalle.Cantidad = CantidadPluminesTalCual;
                            newDetalle.CodigoCompleto = preCodigoPortaPlumin.Codigo;
                            newDetalle.Presentacion = preCodigoPortaPlumin.IdPresentacion;
                            newDetalle.Producto = preCodigoPortaPlumin.objProducto.IdProducto;
                            newDetalle.ValorUnitario = preCodigoPortaPlumin.Precio;
                            newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;

                            cabecera.DetallePedidos.Add(newDetalle);
                        }
                    }


                    if (CantidadPluminesOtros > 0)
                    {

                        Presentacion preCodigoFolletoPortaPlumin = (from P in Contexto.Presentacions
                                                                    where P.Codigo.Trim() == CodigoFolletoPortaPlumin
                                                                    select P).FirstOrDefault();


                        if (preCodigoFolletoPortaPlumin != null)
                        {
                            newDetalle = new DetallePedido();
                            newDetalle.Cantidad = CantidadPluminesOtros;
                            newDetalle.CodigoCompleto = preCodigoFolletoPortaPlumin.Codigo;
                            newDetalle.Presentacion = preCodigoFolletoPortaPlumin.IdPresentacion;
                            newDetalle.Producto = preCodigoFolletoPortaPlumin.objProducto.IdProducto;
                            newDetalle.ValorUnitario = preCodigoFolletoPortaPlumin.Precio;
                            newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;

                            cabecera.DetallePedidos.Add(newDetalle);
                        }
                    }

                }
                #endregion

                #region  Detalle Cupon 0Km 25/04/213 : Fuera de uso 17/03/2014
                //if (!EsTemporal)
                //{
                //    // 2520000111921 : Cupon Okm
                //    // 2520000111922 : Nuevo Cupon Okm
                //    Presentacion preCuponOK = (from P in Contexto.Presentacions
                //                               where P.Codigo == "2520000111923"
                //                               select P).FirstOrDefault();

                //    DateTime fechaAplicacion = Convert.ToDateTime("31/12/2013 08:30:00");

                //    if (preCuponOK != null && DateTime.Now >= fechaAplicacion)
                //    {

                //        /// Regla de Negocio:
                //        /// 1. Si el producto se encuentra dentro de la lista de productos con regalo, entonces
                //        /// entrego un cupon OK segun la cantidad establecida.
                //        Dictionary<string, Int32> ProductosConRegalos = new Dictionary<string, int>();
                //        ProductosConRegalos.Add("1010300001   -181-50", 1);
                //        ProductosConRegalos.Add("1010300001   -021-50", 1);
                //        ProductosConRegalos.Add("1010300001   -023-50", 1);
                //        ProductosConRegalos.Add("1010300001   -022-50", 1);
                //        ProductosConRegalos.Add("1010300001   -024-50", 1);
                //        ProductosConRegalos.Add("1010300001   -152-50", 1);
                //        ProductosConRegalos.Add("1010300001   -025-50", 1);
                //        ProductosConRegalos.Add("1010300001   -026-50", 1);
                //        ProductosConRegalos.Add("1010300001   -180-50", 1);
                //        ProductosConRegalos.Add("1010300001   -027-50", 1);
                //        ProductosConRegalos.Add("1010300001   -028-50", 1);
                //        ProductosConRegalos.Add("1010300001   -309-50", 1);
                //        ProductosConRegalos.Add("1010300001   -153-50", 1);
                //        ProductosConRegalos.Add("1010300001   -243-50", 2);
                //        ProductosConRegalos.Add("1018500170005-000-00", 1);
                //        ProductosConRegalos.Add("1151600021001", 2);
                //        ProductosConRegalos.Add("1151600021002", 2);
                //        ProductosConRegalos.Add("1150000021553", 1);
                //        ProductosConRegalos.Add("2150000021075", 5);
                //        ProductosConRegalos.Add("1150000021354", 5);
                //        ProductosConRegalos.Add("1031800010005-000-35", 1);
                //        ProductosConRegalos.Add("1031800009001-000-35", 1);
                //        ProductosConRegalos.Add("1031800011007-000-35", 1);
                //        ProductosConRegalos.Add("1031800011006-000-35", 1);
                //        ProductosConRegalos.Add("1031800009004-000-10", 1);
                //        ProductosConRegalos.Add("1031800009003-000-10", 2);
                //        ProductosConRegalos.Add("1031800009002-000-10", 1);
                //        ProductosConRegalos.Add("1031800009006-000-50", 1);
                //        ProductosConRegalos.Add("1031800009007-000-50", 1);
                //        ProductosConRegalos.Add("1031800009008-000-50", 1);
                //        ProductosConRegalos.Add("1032100015   -155-50", 1);
                //        ProductosConRegalos.Add("1032100015   -156-50", 1);
                //        ProductosConRegalos.Add("1031900009002-000-05", 1);
                //        ProductosConRegalos.Add("1031900009001-000-05", 1);
                //        ProductosConRegalos.Add("1031900009004-000-10", 1);
                //        ProductosConRegalos.Add("1031900009003-000-10", 2);
                //        ProductosConRegalos.Add("1031900010006-000-30", 1);
                //        ProductosConRegalos.Add("1128200141000-000-03", 1);
                //        ProductosConRegalos.Add("1128200142000-000-03", 1);
                //        ProductosConRegalos.Add("1128200143000-000-03", 1);
                //        ProductosConRegalos.Add("1128300230001-000-00", 1);
                //        ProductosConRegalos.Add("1128300230002-000-00", 1);
                //        ProductosConRegalos.Add("1128300240001-000-00", 1);
                //        ProductosConRegalos.Add("1128300244001-000-00", 1);
                //        ProductosConRegalos.Add("1128300244002-000-00", 1);
                //        ProductosConRegalos.Add("1128300243001-000-00", 1);
                //        ProductosConRegalos.Add("1128301150001-000-00", 1);
                //        ProductosConRegalos.Add("1128301150002-000-00", 1);
                //        ProductosConRegalos.Add("1128301150003-000-00", 1);
                //        ProductosConRegalos.Add("1128301150020-000-00", 1);
                //        ProductosConRegalos.Add("1128301150021-000-00", 1);
                //        ProductosConRegalos.Add("1128301150022-000-00", 1);
                //        ProductosConRegalos.Add("1128301150023-000-00", 1);
                //        ProductosConRegalos.Add("1128301150024-000-00", 1);
                //        ProductosConRegalos.Add("1128301150025-000-00", 1);
                //        ProductosConRegalos.Add("1128301150026-000-00", 1);
                //        ProductosConRegalos.Add("1128301150027-000-00", 1);
                //        ProductosConRegalos.Add("1128301150028-000-00", 1);
                //        ProductosConRegalos.Add("1128301150029-000-00", 1);
                //        ProductosConRegalos.Add("1128301150040-000-00", 1);
                //        ProductosConRegalos.Add("1128301150041-000-00", 1);
                //        ProductosConRegalos.Add("1128301150042-000-00", 1);
                //        ProductosConRegalos.Add("1128301150043-000-00", 1);
                //        ProductosConRegalos.Add("1128301150044-000-00", 1);
                //        ProductosConRegalos.Add("1128301150045-000-00", 1);
                //        ProductosConRegalos.Add("1128301150046-000-00", 1);
                //        ProductosConRegalos.Add("1128301150047-000-00", 1);
                //        ProductosConRegalos.Add("1128301150048-000-00", 1);
                //        ProductosConRegalos.Add("1128300152001-000-00", 1);
                //        ProductosConRegalos.Add("1128300152002-000-00", 1);
                //        ProductosConRegalos.Add("1128300152003-000-00", 1);
                //        ProductosConRegalos.Add("1128300152004-000-00", 1);
                //        ProductosConRegalos.Add("1128300152005-000-00", 1);
                //        ProductosConRegalos.Add("1128300242001-000-00", 1);
                //        ProductosConRegalos.Add("1128300242002-000-00", 1);
                //        ProductosConRegalos.Add("1128300242003-000-00", 1);
                //        ProductosConRegalos.Add("1128300242004-000-00", 1);
                //        ProductosConRegalos.Add("1128300241001-000-00", 1);
                //        ProductosConRegalos.Add("1128300241002-000-00", 1);
                //        ProductosConRegalos.Add("1128201140024-000-01", 1);
                //        ProductosConRegalos.Add("1128201140025-000-01", 1);
                //        ProductosConRegalos.Add("1128201140040-000-01", 1);
                //        ProductosConRegalos.Add("1128201140047-000-01", 1);
                //        ProductosConRegalos.Add("1128201140048-000-01", 1);
                //        ProductosConRegalos.Add("1128201140049-000-01", 1);
                //        ProductosConRegalos.Add("1128201140050-000-01", 1);
                //        ProductosConRegalos.Add("1128201140054-000-01", 1);
                //        ProductosConRegalos.Add("1128201140055-000-01", 1);
                //        ProductosConRegalos.Add("1128201140056-000-01", 1);
                //        ProductosConRegalos.Add("1128201140057-000-01", 1);
                //        ProductosConRegalos.Add("1128201140058-000-01", 1);
                //        ProductosConRegalos.Add("1128201140060-000-01", 1);
                //        ProductosConRegalos.Add("1128201140080-000-01", 1);
                //        ProductosConRegalos.Add("1128201140020-000-03", 1);
                //        ProductosConRegalos.Add("1128201140021-000-03", 1);
                //        ProductosConRegalos.Add("1128201140022-000-03", 1);
                //        ProductosConRegalos.Add("1128201140023-000-03", 1);
                //        ProductosConRegalos.Add("1128201140026-000-03", 1);
                //        ProductosConRegalos.Add("1128201140042-000-03", 1);
                //        ProductosConRegalos.Add("1128201140043-000-03", 1);
                //        ProductosConRegalos.Add("1128201140044-000-03", 1);
                //        ProductosConRegalos.Add("1128201140045-000-03", 1);
                //        ProductosConRegalos.Add("1128201140046-000-03", 1);
                //        ProductosConRegalos.Add("1128201140051-000-03", 1);
                //        ProductosConRegalos.Add("1128201140052-000-03", 1);
                //        ProductosConRegalos.Add("1128201140053-000-03", 1);
                //        ProductosConRegalos.Add("1150000021559", 1);
                //        ProductosConRegalos.Add("1150000021265", 1);
                //        ProductosConRegalos.Add("1151111021001", 2);
                //        ProductosConRegalos.Add("1151212021001", 2);
                //        ProductosConRegalos.Add("1151015021001", 2);
                //        ProductosConRegalos.Add("1151313021001", 2);
                //        ProductosConRegalos.Add("1151414021001", 2);
                //        ProductosConRegalos.Add("1086100002   -317-15", 2);
                //        ProductosConRegalos.Add("1086200002   -316-10", 2);
                //        ProductosConRegalos.Add("1150000021588", 2);
                //        //ProductosConRegalos.Add("1150000021082", 3);
                //        //ProductosConRegalos.Add("1150000021148", 3);
                //        ProductosConRegalos.Add("1043100020   -024-00", 3);
                //        ProductosConRegalos.Add("1043300020   -026-00", 3);
                //        ProductosConRegalos.Add("1150000021083", 2);
                //        ProductosConRegalos.Add("1150000021149", 2);
                //        ProductosConRegalos.Add("1150000021084", 1);
                //        ProductosConRegalos.Add("1150000021150", 1);
                //        ProductosConRegalos.Add("1032000106001-000-36", 1);
                //        ProductosConRegalos.Add("1032000106002-000-36", 1);
                //        ProductosConRegalos.Add("1032000107001-000-36", 1);
                //        ProductosConRegalos.Add("1032000107002-000-36", 1);
                //        ProductosConRegalos.Add("1032000011003-000-36", 1);
                //        ProductosConRegalos.Add("1097000102   -242-14", 1);
                //        ProductosConRegalos.Add("1097000102   -241-14", 1);
                //        ProductosConRegalos.Add("1097000102   -240-14", 1);
                //        ProductosConRegalos.Add("1137900184001-000-00", 2);
                //        ProductosConRegalos.Add("1137900184002-000-00", 2);
                //        ProductosConRegalos.Add("1138700180167-000-00", 3);
                //        ProductosConRegalos.Add("1138700180166-000-00", 3);
                //        ProductosConRegalos.Add("1138700180165-000-00", 3);
                //        ProductosConRegalos.Add("1138700180010-000-00", 3);
                //        ProductosConRegalos.Add("1138700180061-000-00", 3);
                //        ProductosConRegalos.Add("1138700180060-000-00", 3);
                //        ProductosConRegalos.Add("1138700183004-000-00", 2);
                //        ProductosConRegalos.Add("1138700183005-000-00", 2);
                //        ProductosConRegalos.Add("1150000021491", 1);
                //        ProductosConRegalos.Add("1150000021490", 1);
                //        ProductosConRegalos.Add("1150000021585", 1);
                //        ProductosConRegalos.Add("1150000021586", 1);
                //        ProductosConRegalos.Add("1150000021587", 1);






                //        List<string> codigos = ProductosConRegalos.Keys.ToList();
                //        var ProductosPara_0KM = (from N in cabecera.DetallePedidos
                //                                 where codigos.Contains(N.CodigoCompleto.Trim())
                //                                 select new
                //                                 {
                //                                     Cantidad = N.Cantidad.Value,
                //                                     Codigo = N.CodigoCompleto.Trim()
                //                                 }).ToList();


                //        /// Busco ademas dentro de los remitos 
                //        foreach (RemitosPendiente item in UCTotalizadorNivel.RemitosPendientesNoAfectados)
                //        {
                //            if (codigos.Contains(item.CodArticulo))
                //                ProductosPara_0KM.Add(new { Cantidad = Convert.ToInt64(item.Cantidad), Codigo = item.CodArticulo.Trim() });
                //        }


                //        if (ProductosPara_0KM.Count > 0)
                //        {

                //            long CantidadCupones = 0;
                //            foreach (var item in ProductosPara_0KM)
                //            {
                //                int cantidadParcial = ProductosConRegalos[item.Codigo] != null ? ProductosConRegalos[item.Codigo] : 0;
                //                CantidadCupones += cantidadParcial * item.Cantidad;
                //            }

                //            if (CantidadCupones > 0)
                //            {
                //                newDetalle = new DetallePedido();
                //                newDetalle.Cantidad = CantidadCupones;
                //                newDetalle.CodigoCompleto = preCuponOK.Codigo;
                //                newDetalle.Presentacion = preCuponOK.IdPresentacion;
                //                newDetalle.Producto = preCuponOK.objProducto.IdProducto;
                //                newDetalle.ValorUnitario = 0; //preCuponOK.Precio;
                //                newDetalle.ValorTotal = 0;//newDetalle.ValorUnitario * newDetalle.Cantidad;

                //                cabecera.DetallePedidos.Add(newDetalle);

                //            }
                //        }
                //    }

                //}
                #endregion

                #region  Detalle Mochila Aromatizador Circus 08/07/2013
                //if (!EsTemporal)
                //{

                //    string CodigoMochilaCircus = "2500000182001";
                //    string CodigoAromatizadorCircus = "1010300001   -133-50 ";

                //    /// si en el detalle esta el producto buscado: CodigoAromatizadorCircus entonces AGREGO 
                //    /// el producto elejido: CodigoMochilaCircus
                //    long CantidadAromatizadorCircus = Convert.ToInt64(((from N in cabecera.DetallePedidos
                //                                                        where N.CodigoCompleto == CodigoAromatizadorCircus
                //                                                        select N.Cantidad.Value).Sum()));


                //    if (CantidadAromatizadorCircus > 0)
                //    {

                //        Presentacion preMochilaCircus = (from P in Contexto.Presentacions
                //                                         where P.Codigo.Trim() == CodigoMochilaCircus
                //                                         select P).FirstOrDefault();


                //        if (preMochilaCircus != null)
                //        {
                //            newDetalle = new DetallePedido();
                //            newDetalle.Cantidad = CantidadAromatizadorCircus;
                //            newDetalle.CodigoCompleto = preMochilaCircus.Codigo;
                //            newDetalle.Presentacion = preMochilaCircus.IdPresentacion;
                //            newDetalle.Producto = preMochilaCircus.objProducto.IdProducto;
                //            newDetalle.ValorUnitario = preMochilaCircus.Precio;
                //            newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;

                //            cabecera.DetallePedidos.Add(newDetalle);
                //        }
                //    }
                //}

                #endregion

                /// Cambios para ENERO del 2015

                #region  Detalle Bolsa Institucional : 01/01/2015
                /// Lógica anulada a partir del 01/04/2015 (Fecha de mail: 31/03/2015)
                //if (!EsTemporal)
                //{
                //    string[] CodigosEauPerfume = new string[] { "1080000002   -125-10", "1086100002   -134-11", "1086200002   -233-11", "1080000002   -128-21", "1086100002   -204-23", "1086100002   -166-10", "1080000002   -123-15", "1080000002   -132-23", "1080000002   -122-10", "1080000002   -124-10", "1086100002   -317-15", "1086200002   -316-10", "1086200002   -312-15", "1086100002   -311-15", "1080000002   -127-15", "1086200002   -318-11" };

                //    long CantidadCodigosEauPerfume = Convert.ToInt64(((from N in cabecera.DetallePedidos
                //                                                       where CodigosEauPerfume.Contains(N.CodigoCompleto.Trim())
                //                                                       select N.Cantidad.Value).Sum()));


                //    if (CantidadCodigosEauPerfume > 0)
                //    {

                //        Presentacion preBolsaInstitucional = (from P in Contexto.Presentacions
                //                                              where P.Codigo == "2506900018008" // BOLSA REGALO INSTITUCIONAL SM
                //                                              select P).SingleOrDefault();


                //        newDetalle = new DetallePedido();
                //        newDetalle.Cantidad = CantidadCodigosEauPerfume;
                //        newDetalle.CodigoCompleto = preBolsaInstitucional.Codigo;
                //        newDetalle.Presentacion = preBolsaInstitucional.IdPresentacion;
                //        newDetalle.Producto = preBolsaInstitucional.objProducto.IdProducto;
                //        newDetalle.ValorUnitario = preBolsaInstitucional.Precio;
                //        newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;

                //        cabecera.DetallePedidos.Add(newDetalle);


                //        /// Micaela: Cambio solicitado el 22/01/2015 se debe utilizar este producto 
                //        /// 1150000021092        DESCUENTO BOLSA DE REGALO EXCLUSIVA EN-MAR 2015
                //        /// en lugar 
                //        /// 2150000021013    DESCUENTO BOLSA DE REGALO EXCLUSIVA EN-MAR 2015 
                //        Presentacion preDescuentoBolsaInstitucional = (from P in Contexto.Presentacions
                //                                                       where P.Codigo == "1150000021092" //  DESCUENTO BOLSA DE REGALO EXCLUSIVA EN-MAR 2015
                //                                                       select P).SingleOrDefault();


                //        newDetalle = new DetallePedido();
                //        newDetalle.Cantidad = CantidadCodigosEauPerfume;
                //        newDetalle.CodigoCompleto = preDescuentoBolsaInstitucional.Codigo;
                //        newDetalle.Presentacion = preDescuentoBolsaInstitucional.IdPresentacion;
                //        newDetalle.Producto = preDescuentoBolsaInstitucional.objProducto.IdProducto;
                //        newDetalle.ValorUnitario = preDescuentoBolsaInstitucional.Precio * -1;
                //        newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;

                //        cabecera.DetallePedidos.Add(newDetalle);
                //    }
                //}

                #endregion

                #region  Detalle Hornillo Tao 01/01/2015
                if (!EsTemporal)
                {

                    string CodigoHornilloTao = "1018500170006-000-0";
                    string CodigoVelaRegalo = "2500000110001";

                    /// si en el detalle esta el producto buscado: CodigoHornilloTao entonces AGREGO 
                    /// el producto elejido: CodigoVelaRegalo
                    long CantidadHornilloTao = Convert.ToInt64(((from N in cabecera.DetallePedidos
                                                                 where N.CodigoCompleto.Trim() == CodigoHornilloTao.Trim()
                                                                 select N.Cantidad.Value).Sum()));


                    if (CantidadHornilloTao > 0)
                    {

                        Presentacion preVelaRegalo = (from P in Contexto.Presentacions
                                                      where P.Codigo.Trim() == CodigoVelaRegalo
                                                      select P).FirstOrDefault();


                        if (preVelaRegalo != null)
                        {
                            newDetalle = new DetallePedido();
                            newDetalle.Cantidad = CantidadHornilloTao;
                            newDetalle.CodigoCompleto = preVelaRegalo.Codigo;
                            newDetalle.Presentacion = preVelaRegalo.IdPresentacion;
                            newDetalle.Producto = preVelaRegalo.objProducto.IdProducto;
                            newDetalle.ValorUnitario = preVelaRegalo.Precio;
                            newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;

                            cabecera.DetallePedidos.Add(newDetalle);
                        }
                    }
                }

                #endregion

                #region Valija Vacía 
                /// 30/06/2015: Cambio implementado 
                
                //Cada vez que se pida el siguiente artículo: 
                //2506900023020        Cartuchera SM (Vacía)   
   
                //Debe agregar en la nota de pedido el siguiente artículo 4 veces: 
                //2506900023029        Blister Vacío      ( 4 unidades)


                if (!EsTemporal)
                {
                    /// cambio del 30-06-2015
                    /// Se cambio a este producto: Aromatizador Mujercitas Inqu 500 ml   
                    string CodigoCartucheraVacia = "2506900023020";
                    string CodigoBlisterVacio = "2506900023029";

                    /// si en el detalle esta el producto Aromatizador Mujercitas Inqu 500 ml   doy de regalo Gatillo Aromatizador 500 ml
                    long CantidadCartucherasVacias = Convert.ToInt64(((from N in cabecera.DetallePedidos
                                                                                 where N.CodigoCompleto.Trim() == CodigoCartucheraVacia.Trim()
                                                                                 select N.Cantidad.Value).Sum()));


                    if (CantidadCartucherasVacias > 0)
                    {

                        Presentacion preBlisterVacio = (from P in Contexto.Presentacions
                                                        where P.Codigo.Trim() == CodigoBlisterVacio
                                                               select P).FirstOrDefault();


                        if (preBlisterVacio != null)
                        {
                            newDetalle = new DetallePedido();
                            newDetalle.Cantidad = 4 * CantidadCartucherasVacias;
                            newDetalle.CodigoCompleto = preBlisterVacio.Codigo;
                            newDetalle.Presentacion = preBlisterVacio.IdPresentacion;
                            newDetalle.Producto = preBlisterVacio.objProducto.IdProducto;
                            newDetalle.ValorUnitario = preBlisterVacio.Precio;
                            newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;

                            cabecera.DetallePedidos.Add(newDetalle);
                        }
                    }
                }


                #endregion

                #region  Detalle Colección Vintage Romance 01/01/2015
                if (!EsTemporal)
                {

                    string CodigoColeccionVintageRomance = "1097200008001-000-00";
                    string CodigoCajaVintageRomance = "2500000008001";

                    /// si en el detalle esta el producto buscado: CodigoHornilloTao entonces AGREGO 
                    /// el producto elejido: CodigoVelaRegalo
                    long CantidadColeccionVintageRomance = Convert.ToInt64(((from N in cabecera.DetallePedidos
                                                                             where N.CodigoCompleto.Trim() == CodigoColeccionVintageRomance.Trim()
                                                                             select N.Cantidad.Value).Sum()));


                    if (CantidadColeccionVintageRomance > 0)
                    {

                        Presentacion preCajaVintageRomance = (from P in Contexto.Presentacions
                                                              where P.Codigo.Trim() == CodigoCajaVintageRomance
                                                              select P).FirstOrDefault();


                        if (preCajaVintageRomance != null)
                        {
                            newDetalle = new DetallePedido();
                            newDetalle.Cantidad = CantidadColeccionVintageRomance;
                            newDetalle.CodigoCompleto = preCajaVintageRomance.Codigo;
                            newDetalle.Presentacion = preCajaVintageRomance.IdPresentacion;
                            newDetalle.Producto = preCajaVintageRomance.objProducto.IdProducto;
                            newDetalle.ValorUnitario = preCajaVintageRomance.Precio;
                            newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;

                            cabecera.DetallePedidos.Add(newDetalle);
                        }
                    }
                }

                #endregion

                #region  Repuesto Difusores varios
                if (!EsTemporal)
                {
                    List<string> CodigoDifusorPet = new List<string>() { "1010700126   -193-23", "1010700126   -192-23", "1010700126   -190-23", "1010700126   -308-23", "1010700126   -200-23", "1010700126   -191-23", "1010700126   -198-23" };
               

                    //string CodigoDifusorPet = "1010700126   -193-23";
                    string CodigoVarrila = "2500000110106"; //Paquete Varillas de Bambú x 16 u (sólo 1) 
                    //string CodigoRepuestoDif = "2506900125001"; //Embudo Repuesto Difusor (sólo 1) 
                    string CodigoDescuentoRepDif = "2150000021025"; // Descuento Embudo Repuesto Difusor (sólo 1) 



                    /// si en el detalle esta el producto buscado: CodigoDifusorPet entonces AGREGO 
                    /// el producto elejido: CodigoVarrila y CodigoRepuestoDif
                    long CantidadDifusorPet = Convert.ToInt64(((from N in cabecera.DetallePedidos
                                                                where CodigoDifusorPet.Contains(N.CodigoCompleto.Trim())
                                                                select N.Cantidad.Value).Sum()));

                    if (CantidadDifusorPet > 0)
                    {

                        Presentacion preVarille = (from P in Contexto.Presentacions
                                                   where P.Codigo.Trim() == CodigoVarrila
                                                   select P).FirstOrDefault();


                        if (preVarille != null)
                        {
                            newDetalle = new DetallePedido();
                            newDetalle.Cantidad = CantidadDifusorPet;
                            newDetalle.CodigoCompleto = preVarille.Codigo;
                            newDetalle.Presentacion = preVarille.IdPresentacion;
                            newDetalle.Producto = preVarille.objProducto.IdProducto;
                            newDetalle.ValorUnitario = preVarille.Precio;
                            newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;

                            cabecera.DetallePedidos.Add(newDetalle);
                        }

                        /// 30/06/2015: No se utiliza mas esta regla de negocio
                        
                        //Presentacion preRepuestoDifusor = (from P in Contexto.Presentacions
                        //                                   where P.Codigo.Trim() == CodigoRepuestoDif
                        //                                   select P).FirstOrDefault();


                        //if (preRepuestoDifusor != null)
                        //{
                        //    newDetalle = new DetallePedido();
                        //    newDetalle.Cantidad = CantidadDifusorPet;
                        //    newDetalle.CodigoCompleto = preRepuestoDifusor.Codigo;
                        //    newDetalle.Presentacion = preRepuestoDifusor.IdPresentacion;
                        //    newDetalle.Producto = preRepuestoDifusor.objProducto.IdProducto;
                        //    newDetalle.ValorUnitario = preRepuestoDifusor.Precio;
                        //    newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;

                        //    cabecera.DetallePedidos.Add(newDetalle);
                        //}

                        //Presentacion preDescuentoRepuestoDifusor = (from P in Contexto.Presentacions
                        //                                            where P.Codigo.Trim() == CodigoDescuentoRepDif
                        //                                            select P).FirstOrDefault();


                        //if (preDescuentoRepuestoDifusor != null)
                        //{
                        //    newDetalle = new DetallePedido();
                        //    newDetalle.Cantidad = CantidadDifusorPet;
                        //    newDetalle.CodigoCompleto = preDescuentoRepuestoDifusor.Codigo;
                        //    newDetalle.Presentacion = preDescuentoRepuestoDifusor.IdPresentacion;
                        //    newDetalle.Producto = preDescuentoRepuestoDifusor.objProducto.IdProducto;
                        //    newDetalle.ValorUnitario = preDescuentoRepuestoDifusor.Precio * -1;
                        //    newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;

                        //    cabecera.DetallePedidos.Add(newDetalle);
                        //}


                    }
                }

                #endregion
           
                #endregion

                #region Generacion del Gasto de Envio del Pedido

                decimal valorTansporte = 0;

                //try
                //{
                ConfTransporte confT = (from C in Contexto.ConfTransportes
                                        where C.Provincia == dirEntrega.Provincia &&
                                        C.Localidad == dirEntrega.Localidad &&
                                        C.FormaDePago == cboFormaPago.SelectedItem.Text
                                        select C).FirstOrDefault<ConfTransporte>();

                if (confT != null)
                {

                    newDetalle = new DetallePedido();
                    newDetalle.Cantidad = 1;
                    newDetalle.CodigoCompleto = confT.objProducto.ColPresentaciones[0].Codigo;
                    newDetalle.Presentacion = confT.objProducto.ColPresentaciones[0].IdPresentacion;
                    newDetalle.Producto = confT.objProducto.IdProducto;
                    newDetalle.ValorUnitario = confT.objProducto.ColPresentaciones[0].Precio;
                    newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;
                    cabecera.DetallePedidos.Add(newDetalle);

                    valorTansporte = newDetalle.ValorTotal.Value;
                }
                else if (confT == null && !EsTemporal)
                {
                    ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "Transporte", "AlertaTransporte();", true);
                    return;
                }

                //}
                //catch
                //{
                //    ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "Transporte", "AlertaTransporte();", true);
                //    return;
                //}


                #endregion

                /// Martin (17/03/2014): Este codigo es obsoleto, ya que la variable lblSubTotal contiene 
                /// todos los calculos necesarios para determinar el monto final del pedido antes de impuestos.
                //decimal Total = 0;
                //foreach (DetallePedido item in cabecera.DetallePedidos)
                //{
                //    if (item.ValorTotal.HasValue)
                //        Total += item.ValorTotal.Value;
                //}

                ///// Descuento del total el descuento por provincia realizado
                //if (cabecera.DescuentoProvincia.HasValue)
                //    Total -= cabecera.DescuentoProvincia.Value;

                ///// Descuento del total el descuento general realizado
                //if (cabecera.DescuentosGenerales.HasValue)
                //    Total -= cabecera.DescuentosGenerales.Value;

                //cabecera.MontoTotal = Total;

                /// CODIGO NUEVO
                decimal Total = decimal.Parse(lblSubTotal.Text.Replace("$", ""));


                /// Calculo nuevamente los impuesto por las dudas que exista algun
                /// cambio no contemplado.
                cabecera.MontoTotal = CalculoImpuestos(Total);
                cabecera.Impuestos = ImpuestoCalculado;
                cabecera.DetalleImpuestos = DetalleImpuestosCalculados;

                #region Control LIMITE POR PROVINCIA -Sin tener en cuenta los impuestos
                //if ((!EsTemporal && !EsClienteEspecial))
                //{

                //    if (TipoCliente.ToString() != Convert.ToString((int)TipoClientes.PotencialBolso))
                //    {
                //        /// Control LIMITE POR PROVINCIA
                //        if (Total < LimitePorProvincia)
                //        {
                //            ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "MinimoReq", "AlertaMinimoRequeridoProvincia(" + LimitePorProvincia + ");", true);
                //            return;
                //        }
                //    }
                //}
                #endregion

                #region Control de limites de compra por la forma de pago
                //if ((!FaltaSaldo && !EsTemporal))
                //{
                //    /// Este control se hace para todos lo clientes salvo los clientes del grupo DIRECTORIO
                //    if (!EsClienteEspecial)
                //    {
                //        decimal SaldoActual = -1 * SaldoPagoAnticipado;
                //        switch (cboFormaPago.Text)
                //        {
                //            case "Pago Fácil":
                //                {
                //                    if (Total > SaldoActual)
                //                    {
                //                        decimal TotalSinTransporte = Total - valorTansporte;
                //                        ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "SaldoReq", "AlertaSaldoInsuficiente('El monto del pedido (Productos: $" + TotalSinTransporte.ToString() + " + Transporte: $" + valorTansporte.ToString() + ") supera el saldo disponible que posee ($" + SaldoActual.ToString() + "), el mismo no puede ser realizado hasta que tenga saldo suficiente. Si lo desea puede guardar el pedido temporalmente para realizarlo en otro momento.');", true);
                //                        return;
                //                    }
                //                    else
                //                        break;


                //                }
                //            case "Pago Mis Cuentas":
                //                {
                //                    if (Total > SaldoActual)
                //                    {
                //                        decimal TotalSinTransporte = Total - valorTansporte;
                //                        ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "SaldoReq", "AlertaSaldoInsuficiente('El monto del pedido (Productos: $" + TotalSinTransporte.ToString() + " + Transporte: $" + valorTansporte.ToString() + ") supera el saldo disponible que posee ($" + SaldoActual.ToString() + "), el mismo no puede ser realizado hasta que tenga saldo suficiente. Si lo desea puede guardar el pedido temporalmente para realizarlo en otro momento.');", true);
                //                        return;
                //                    }
                //                    else
                //                        break;


                //                }
                //            case "Contra Reembolso":
                //                {
                //                    if ((Total - SaldoActual) > decimal.Parse(LimiteContraReembolso))
                //                    {
                //                        ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "SaldoReq", "AlertaSaldoInsuficiente('El monto del pedido supera el límite en contra reemboldo ($ " + LimiteContraReembolso + "), el mismo no puede ser realizado. Si lo desea puede guardar el pedido temporalmente para realizarlo en otro momento.');", true);
                //                        return;
                //                    }
                //                    else
                //                        break;

                //                }
                //            case "Rapi Pago":
                //                {
                //                    if (Total > SaldoActual)
                //                    {
                //                        decimal TotalSinTransporte = Total - valorTansporte;
                //                        ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "SaldoReq", "AlertaSaldoInsuficiente('El monto del pedido (Productos: $" + TotalSinTransporte.ToString() + " + Transporte: $" + valorTansporte.ToString() + ") supera el saldo disponible que posee ($" + SaldoActual.ToString() + "), el mismo no puede ser realizado hasta que tenga saldo suficiente. Si lo desea puede guardar el pedido temporalmente para realizarlo en otro momento.');", true);
                //                        return;
                //                    }
                //                    else
                //                        break;


                //                }
                //        }
                //    }
                //}


                #endregion


                /// Si el idPedido=="" 
                /// significa que estoy en un pedido nuevo.
                if (idPedido == "")
                {
                    Contexto.CabeceraPedidos.InsertOnSubmit(cabecera);

                    if (EsTemporal)
                    {
                        cabecera.UltimaModificacion = DateTime.Now;
                        cabecera.EsTemporal = true;
                        cabecera.HuboFaltaSaldo = FaltaSaldo;
                        Contexto.SubmitChanges();
                        //ActualizarTotalPedido(cabecera.IdCabeceraPedido);

                    }
                    else
                    {
                        RelacionarRemitosPendientes(cabecera);
                        GenerarPedidoConCredito(cabecera);
                        Contexto.SubmitChanges();
                        ActualizarSolicitudProductosEspeciales(cabecera.DetallePedidos.ToList());
                        cabecera = ActualizarTotalPedido(cabecera.IdCabeceraPedido);

                        if (!EsClienteEspecial)
                            EnviarMailAvisoPedido(cabecera);

                        if (EsPagoConTarjeta)
                            ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "PedidoConTarjeta", "LlmarValidarPaginaPSP('" + cabecera.IdCabeceraPedido.ToString() + "','" + string.Format("{0:0.00}", cabecera.MontoTotal).Replace(",", "") + "'," + cabecera.Tarjeta + "," + cabecera.Cuotas + ");", true);
                        else
                            ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "GrabacionPedido", "AlertaGrabacion('" + cabecera.IdCabeceraPedido.ToString() + "');", true);
                    }

                    LimpiarPedido();
                }
                /// Estoy editando un pedido guardado.
                else
                {
                    if (EsTemporal)
                    {
                        cabecera.UltimaModificacion = DateTime.Now;
                        cabecera.EsTemporal = true;
                        cabecera.HuboFaltaSaldo = FaltaSaldo;
                        Contexto.SubmitChanges();

                    }
                    else
                    {
                        RelacionarRemitosPendientes(cabecera);
                        GenerarPedidoConCredito(cabecera);
                        if (!EsClienteEspecial)
                            EnviarMailAvisoPedido(cabecera);
                        cabecera.UltimaModificacion = null;
                        cabecera.EsTemporal = null;
                        cabecera.HuboFaltaSaldo = null;
                        Contexto.SubmitChanges();
                        ActualizarSolicitudProductosEspeciales(cabecera.DetallePedidos.ToList());


                    }

                    cabecera = ActualizarTotalPedido(cabecera.IdCabeceraPedido);

                    /// Mando a imprimir, si se realizo la solicitud del pedido
                    if (EsTemporal)
                    {
                        ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "GrabacionPedidoTemp", "javascript:window.close();", true);
                    }
                    else
                    {
                        if (EsPagoConTarjeta)
                            ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "PedidoConTarjeta", "LlmarValidarPaginaPSP('" + cabecera.IdCabeceraPedido.ToString() + "','" + string.Format("{0:0.00}", cabecera.MontoTotal).Replace(",", "") + "'," + cabecera.Tarjeta + "," + cabecera.Cuotas + ");", true);
                        else
                            ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "GrabacionPedido", "GrabacionFinalPendiente('" + cabecera.IdCabeceraPedido.ToString() + "');", true);
                    }
                }


            }
            catch (Exception error)
            {
                ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "Error General", "radalert('" + error.Message + "');", true);

            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "Error Stock", "radalert('" + mensajeStock + "',400,200,'Stock Controlado');", true);
        }
    }

    private void CargarArbolProductos()
    {
        _productos = (from prod in Contexto.Productos
                      where prod.Tipo == 'A'
                      select prod).ToList<Producto>();

        Productos = _productos;

        /// Busco todos las presentaciones que tiene padre
        /// y recupero el id del padre de estos
        List<long> idProductosExistente = (from prod in Contexto.Presentacions
                                           where (prod.objProducto.Tipo == 'A' && prod.Activo == true)
                                           select prod.objProducto.objPadre.IdProducto).Distinct().ToList<long>();

        /// 1.Unidad de negocio - 2.linea - 3.fragancias - presentaciones (Tabla Presentaciones)
        /// Solo cargo en el arbol aquellos lineas donde las fregancias posea
        /// almenos una presentación y las unidades de negocio que poseen al menos una linea.

        //Busco todas las lineas que tienen al menos una presentacion.
        var idsProductosUltimoNivel = (from prod in _productos
                                       where (prod.Nivel > 1 && idProductosExistente.Contains(prod.IdProducto))
                                       select prod.IdProducto).ToList();

        // Busco los productos del primer nivel que esten en la lista de productos de tipo linea que poseen hijos 
        // y las lineas que tiene al menos una fragancia activa.
        var productos = from prod in _productos
                        where (prod.Nivel < 2 && prod.Tipo == 'A' && prod.ColHijos.Any(w => idsProductosUltimoNivel.Contains(w.IdProducto)))
                        || (prod.Nivel > 1 && idProductosExistente.Contains(prod.IdProducto))
                        select prod;


        if (TipoCliente.ToString() == Convert.ToString((int)TipoClientes.PotencialBolso))
        {
            productos = from prod in _productos
                        where ((prod.Nivel < 2 && prod.ColHijos.Count > 0 && prod.Tipo == 'A')
                        || (prod.Nivel > 1 && idProductosExistente.Contains(prod.IdProducto)))
                        && prod.Descripcion.Contains("Incorpor")
                        select prod;

            /// Esto es para excluir la incorporacion nro 0
            HiddenIncorporacion0.Value = "2516700019020";
        }
        else
        {
            /// Esto es para incluir todos los productos en la busqueda
            HiddenIncorporacion0.Value = "@";
        }

        RadTreeProductos.DataSource = Helper.LINQToDataTable<Producto>(productos);
        RadTreeProductos.DataBind();

        if (TipoCliente.ToString() != Convert.ToString((int)TipoClientes.PotencialBolso))
        {
            RadTreeNode nodoPromo = new RadTreeNode("Promociones", "Promociones");
            nodoPromo.Attributes.Add("Padre", "");
            nodoPromo.Attributes.Add("Cargar", "true");
            nodoPromo.Attributes.Add("RutaImagen", "Imagenes/Placas-nuevas-promociones.jpg");
            RadTreeProductos.Nodes.Add(nodoPromo);
        }

    }

    private bool ControlStockValido(bool EsTemporal, out string mensajeStock)
    {
        mensajeStock = "";

        ///// Comentado por que no se hace mas el control de estos productos: 04/09/2012
        //mensajeStock = "";
        //return true;

        if (!EsTemporal)
        {
            List<long> productoControl = new List<long>() { 4707, 6687 };
            productoControl.Add(idPresentacionIncorporacion_0_2014);


            List<DetallePedido> ProductoStockLimitado = (from P in (Session["detPedido"] as List<DetallePedido>)
                                                         where productoControl.Contains(P.Presentacion.Value)
                                                         select P).ToList();


            /// Control Comentado a partir de 01/04/2015
            //List<DetallePedido> ProductoStockLimitadoAgrupado = (from P in (Session["detPedido"] as List<DetallePedido>)
            //                                                     where idsPresentacionIncorporaciones_1_4.Contains(P.Presentacion.Value)
            //                                                     select P).ToList();

            //ProductoStockLimitadoAgrupado.AddRange((from P in (Session["detPedido"] as List<DetallePedido>)
            //                                        where idsPresentacionIncorporaciones_1_2_2014.Contains(P.Presentacion.Value)
            //                                        select P).ToList());


            //long? CantidadSolicitada = ProductoStockLimitado.Sum(w => w.Cantidad);
            if (ProductoStockLimitado.Any(w => w.Cantidad > 1))
            {
                mensajeStock = "<b>Por el momento solo se permite adquirir un máximo de 1 unidad para los siguientes productos solicitados:</b></br></br>";

                foreach (DetallePedido item in ProductoStockLimitado)
                {
                    mensajeStock += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>" + item.ProductoDesc + " " + item.PresentacionDesc + "</b> Cantidad Solicitada: " + item.Cantidad + "</br>";
                }

                //mensajeStock += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Total Solicitado: " + CantidadSolicitada.ToString() + "</b>";
                return false;
            }
            //else if (ProductoStockLimitadoAgrupado.Sum(w => w.Cantidad) > 1)
            //{

            //    mensajeStock = "<b>Por el momento solo se permite adquirir uno solo de los siguientes productos:</b></br></br>";

            //    foreach (DetallePedido item in ProductoStockLimitadoAgrupado)
            //    {
            //        mensajeStock += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>" + item.ProductoDesc + " " + item.PresentacionDesc + "</b> " + "</br>";
            //    }

            //    return false;

            //}
            else
            {
                mensajeStock = "";
                return true;
            }
        }
        else
        {
            mensajeStock = "";
            return true;
        }
    }


    private decimal CalcularDescuentosGenerales()
    {
        decimal descuentoGeneral = 0;

        var productosSolicitados = (from P in (Session["detPedido"] as List<DetallePedido>)
                                    select new
                                    {
                                        Codigo = P.CodigoCompleto,
                                        ValorTotal = P.ValorTotal
                                    }).ToList();

        foreach (var item in productosSolicitados)
        {
            var porcentajeDescuento = DescuentosProductos.Where(w => w.CodigoCompleto.Replace(" ", "") == item.Codigo.Replace(" ", "")).FirstOrDefault();
            if (porcentajeDescuento != null)
            {
                descuentoGeneral += item.ValorTotal.Value * (Convert.ToDecimal(porcentajeDescuento.Porcentaje) / 100);
            }
        }

        var promosDirectas = (from P in (Session["PromosGeneradas"] as List<DetallePedido>)
                              where P.Tipo.Contains("PS")
                              select new
                              {
                                  Codigo = P.CodigoCompleto,
                                  ValorTotal = P.ValorTotal,
                                  colProductosRequeridos = P.colProductosRequeridos,
                                  IdProducto = P.Producto.Value,
                                  NombrePromo = P.PresentacionDesc
                              }).ToList();

        string cadena = "";
        foreach (var item in promosDirectas)
        {
            /// Dado de que se trata de un producto del tipo PROMOCION, se debe calcular
            /// los descuentos de los productos requeridos, ya que los mismos se generan en 
            /// forma automatica al grabar la operacion.


            /// Recupero la composición de los elementos requeridos del producto tipo PROMOCION
            var composicionRequeridos = from R in Contexto.Composicions
                                        where R.TipoComposicion == "C" && R.objProducto.IdProducto == item.IdProducto
                                        group R by R.Grupo into c
                                        select new { Grupo = c.Key, componentes = c };

            //cadena += item.NombrePromo + ":</br>";
            foreach (var grupo in composicionRequeridos)
            {
                var componente = grupo.componentes.FirstOrDefault().objPresentacion;
                var cantidadRequerida = Convert.ToInt16(grupo.componentes.FirstOrDefault().Cantidad);
                var porcentajeDescuento = DescuentosProductos.Where(w => w.CodigoCompleto.Replace(" ", "") == componente.Codigo.Replace(" ", "")).FirstOrDefault();
                if (porcentajeDescuento != null)
                {
                    descuentoGeneral += componente.Precio.Value * cantidadRequerida * (Convert.ToDecimal(porcentajeDescuento.Porcentaje) / 100);
                    //cadena = cadena + componente.objProducto.Descripcion + ": " + componente.Precio.Value.ToString() + "*" + cantidadRequerida.ToString() + "*" + porcentajeDescuento.Porcentaje.ToString() + "</br>";
                }
            }
        }

        //DivHelp.InnerHtml = cadena;
        //upCabeceraPagina.Update();

        return descuentoGeneral;

    }

    private decimal CalcularDescuentoRemitos()
    {
        decimal descuentoRemito = 0;
        if (UCTotalizadorNivel.RemitosPendientesNoAfectados.Count > 0)
        {

            var RemitosConComposicion = (from p in Contexto.ComposicionRemitos
                                         select p).Distinct().ToList();

            string divDetalle = "<div>";
            foreach (var remito in UCTotalizadorNivel.RemitosPendientesNoAfectados)
            {
                if (RemitosConComposicion.Any(w => w.CodigoRemito == remito.CodArticulo))
                {
                    // 1. Genero el ingreso por el remito
                    var remitoPresentacion = (from p in Contexto.Presentacions
                                              where p.Codigo == remito.CodArticulo
                                              select p).FirstOrDefault();

                    divDetalle += "REM: " + remitoPresentacion.Descripcion + "  $ " + remito.Cantidad * remito.Precio * -1 + "<br>";

                    if (remitoPresentacion != null)
                    {
                        descuentoRemito += remito.Cantidad * remito.Precio * -1;
                    }


                    // 2. Genero el ingreso por los componentes del remito
                    var componentes = (from c in RemitosConComposicion
                                       where c.CodigoRemito.Trim() == remito.CodArticulo.Trim()
                                       select c).Distinct().ToList();

                    if (componentes.Count > 0)
                    {
                        foreach (var comp in componentes)
                        {
                            descuentoRemito += comp.Cantidad.Value * comp.objPresentacion.Precio.Value;

                            divDetalle += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;COMP: " + comp.objPresentacion.objProducto.Descripcion + "  $ " + comp.Cantidad.Value * comp.objPresentacion.Precio.Value + "<br>";
                        }
                    }
                }
                else
                {
                    descuentoRemito += remito.Cantidad * remito.Precio * -1;

                    divDetalle += "REM: " + remito.DescArticulo + "  $ " + remito.Cantidad * remito.Precio * -1 + "<br>";
                }

            }

            //divDetalle += "</div>";
            //DivHelp.InnerHtml = divDetalle;
            //upCabeceraPagina.Update();
        }


        return descuentoRemito;

    }

    private void RelacionarRemitosPendientes(CabeceraPedido cabecera)
    {
        if (UCTotalizadorNivel.RemitosPendientesNoAfectados.Count > 0)
        {
            List<string> codsRemitos = UCTotalizadorNivel.RemitosPendientesNoAfectados.Select(w => w.CodArticulo).Distinct().ToList();

            var ProductosComponentesRemitos = (from p in Contexto.ComposicionRemitos
                                               where codsRemitos.Contains(p.CodigoRemito)
                                               select p).Distinct().ToList();


            foreach (RemitosPendiente item in UCTotalizadorNivel.RemitosPendientesNoAfectados)
            {
                if (!cabecera.colRemitosAfectados.Any(w => w.CodArticulo == item.CodArticulo && w.NroRemito == item.NroRemito))
                {
                    RemitosAfectados remAfe = new RemitosAfectados();
                    remAfe.Cantidad = item.Cantidad;
                    remAfe.Precio = item.Precio;
                    remAfe.FechaRemito = item.FechaRemito;
                    remAfe.CodArticulo = item.CodArticulo;
                    remAfe.CodCliente = item.CodCliente;
                    remAfe.DescArticulo = item.DescArticulo;
                    remAfe.objCabeceraPedido = cabecera;
                    remAfe.NroRemito = item.NroRemito;
                    cabecera.colRemitosAfectados.Add(remAfe);


                    /// Regla Negocio: Genero para la nota de pedido el detalle del remito y el detalle
                    /// para cada uno de los componentes

                    // 1. Genero el detalle del remito
                    var remitoPresentacion = (from p in Contexto.Presentacions
                                              where p.Codigo == item.CodArticulo
                                              select p).FirstOrDefault();

                    if (remitoPresentacion != null)
                    {
                        DetallePedido detRemito = new DetallePedido();
                        detRemito.Cantidad = 1;
                        detRemito.CodigoCompleto = remitoPresentacion.Codigo;
                        detRemito.objPresentacion = remitoPresentacion;
                        detRemito.objProducto = remitoPresentacion.objProducto;
                        detRemito.ValorUnitario = remitoPresentacion.Precio * -1;
                        detRemito.ValorTotal = detRemito.Cantidad * remitoPresentacion.Precio * -1;
                        cabecera.DetallePedidos.Add(detRemito);
                    }


                    // 2. Genero los detalles según los componentes del remito
                    var componentes = (from c in ProductosComponentesRemitos
                                       where c.CodigoRemito.Trim() == item.CodArticulo.Trim()
                                       select c).Distinct().ToList();

                    if (componentes.Count > 0)
                    {
                        foreach (var comp in componentes)
                        {
                            DetallePedido detRemito = new DetallePedido();
                            detRemito.Cantidad = comp.Cantidad;
                            detRemito.CodigoCompleto = comp.objPresentacion.Codigo;
                            detRemito.objPresentacion = comp.objPresentacion;
                            detRemito.objProducto = comp.objPresentacion.objProducto;
                            detRemito.ValorUnitario = comp.objPresentacion.Precio;
                            detRemito.ValorTotal = comp.Cantidad * comp.objPresentacion.Precio;
                            cabecera.DetallePedidos.Add(detRemito);
                        }
                    }



                }
            }
        }

    }

    private void GenerarPedidoConCredito(CabeceraPedido cabecera)
    {
        /// Si la forma de pago es en credito entonces tengo 
        /// que actualizar el valor solicitado en credito para saber si puede
        /// seguir solicitando con esta forma de pago.
        if (cabecera.FormaPago == long.Parse(FormaDePagosConstantes.CREDITO))
        {

            ConfCredito CurrentConf = (from c in Contexto.ConfCreditos
                                       where c.objCliente.IdCliente == cabecera.Cliente
                                       select c).FirstOrDefault();

            if (CurrentConf == null)
            {
                CurrentConf = (from c in Contexto.ConfCreditos
                               where c.objCliente.IdCliente == cabecera.ClienteSolicitante
                               select c).FirstOrDefault();
            }

            CurrentConf.MontoUtilizado += cabecera.MontoTotal;

            /// Luego de la actualización tengo que generar el registro del pedido en credito para
            /// saber cuando se vence.
            PedidosConCredito currentPedidoCredito = new PedidosConCredito();
            currentPedidoCredito.Cliente = cabecera.Cliente;
            currentPedidoCredito.ClienteRespCredito = CurrentConf.objCliente.IdCliente;
            currentPedidoCredito.FechaVencimiento = cabecera.FechaPedido.AddDays(CurrentConf.TiempoReposicion).Date;
            currentPedidoCredito.MontoCredito = cabecera.MontoTotal;
            currentPedidoCredito.MontoPagado = 0;
            currentPedidoCredito.objCabeceraPedido = cabecera;
            currentPedidoCredito.Procesado = false;
            Contexto.PedidosConCreditos.InsertOnSubmit(currentPedidoCredito);
            Contexto.SubmitChanges();
        }

    }

    private void EnviarMailAvisoPedido(CabeceraPedido cabecera)
    {

        if (cabecera.objClienteSolicitante.TipoCliente.ToUpper() == TipoClientes.Consultor.ToString().ToUpper())
        {
            if (Helper.ObtenerLider(cabecera.objClienteSolicitante.IdCliente, cabecera.objClienteSolicitante.Clasif1) > 0)
            {
                MailsCabecera mail = new MailsCabecera();
                mail.Cuerpo = "El consultor " + cabecera.objClienteSolicitante.Nombre + " ha realizado un nuevo pedido con Nro:" + @"<a href='ConsultaPedidos.aspx?NroPedido=" + cabecera.Nro.ToString() + "&p=" + Helper.Encriptar(cabecera.objClienteSolicitante.CodigoExterno + "|" + cabecera.Nro) + "' target='_blank' >" + cabecera.Nro.ToString() + "</a>";
                mail.Fecha = DateTime.Now;
                mail.Subject = "Solicitud Nuevo Pedido Nro:" + cabecera.Nro.ToString();
                mail.Usuario = long.Parse(Session["IdUsuario"].ToString());



                MailsDestino mDestino = new MailsDestino();
                mDestino.MailsCabecera = mail;
                mDestino.Usuario = Helper.ObtenerLider(cabecera.objClienteSolicitante.IdCliente, cabecera.objClienteSolicitante.Clasif1);
                mDestino.Estado = EstadosMails.SINLEER;
                mDestino.FechaCambioEstado = DateTime.Now;
                mail.MailsDestinos.Add(mDestino);


                Contexto.MailsCabeceras.InsertOnSubmit(mail);
                Contexto.SubmitChanges();

                HelperReglasMails.GestionarReglaMoverMailSegunOrigen(mail.Usuario.Value, mail.MailsDestinos.ToList(), Contexto);
            }
        }



    }

    /// <summary>
    /// Este metodo se utiliza para recalcular el monto total del pedido
    /// despues de grabar, por las dudas de errores al grabar.
    /// </summary>
    /// <param name="IdPedido"></param>
    private CabeceraPedido ActualizarTotalPedido(long IdPedido)
    {
        Marzzan_InfolegacyDataContext NewContext = new Marzzan_InfolegacyDataContext();

        CabeceraPedido cab = (from c in NewContext.CabeceraPedidos
                              where c.IdCabeceraPedido == IdPedido
                              select c).First();

        cab.MontoTotal = cab.DetallePedidos.Sum(w => w.ValorTotal.Value);

        if (cab.DescuentoProvincia.HasValue)
            cab.MontoTotal -= cab.DescuentoProvincia.Value;


        if (cab.DescuentosGenerales.HasValue)
            cab.MontoTotal -= cab.DescuentosGenerales.Value;


        /// Calculo nuevamente los impuesto por las dudas que exista algun
        /// cambio no contemplado despues de grabar la operacion.
        cab.MontoTotal = CalculoImpuestos(cab.MontoTotal);
        cab.Impuestos = ImpuestoCalculado;
        cab.DetalleImpuestos = DetalleImpuestosCalculados;

        NewContext.SubmitChanges();

        return cab;
    }


    private void ActualizarSolicitudProductosEspeciales(List<DetallePedido> DetallePedidos)
    {
        DetallePedido detEspecial = null;
        /// Guardo en la tabla de solicitudesproductosespeciales los productos especiales que se han
        /// solicitado.
        foreach (var item in idsProductosIncorporaciones_1_4)
        {
            detEspecial = DetallePedidos.Where(w => w.objProducto.IdProducto == long.Parse(item)).FirstOrDefault();
            if (detEspecial != null)
            {
                SolicitudProductosEspeciale solProd = new SolicitudProductosEspeciale();
                solProd.Producto = detEspecial.objProducto.IdProducto;
                solProd.Presentacion = detEspecial.objPresentacion.IdPresentacion;
                solProd.Cliente = detEspecial.objCabecera.Cliente;
                solProd.DetallePedido = detEspecial.IdDetallePedido;
                solProd.Cantidad = Convert.ToInt32(detEspecial.Cantidad);

                Contexto.SolicitudProductosEspeciales.InsertOnSubmit(solProd);
            }
        }


        foreach (var item in idsProductosIncorporaciones_1_2_2014)
        {
            detEspecial = DetallePedidos.Where(w => w.objProducto.IdProducto == long.Parse(item)).FirstOrDefault();
            if (detEspecial != null)
            {
                SolicitudProductosEspeciale solProd = new SolicitudProductosEspeciale();
                solProd.Producto = detEspecial.objProducto.IdProducto;
                solProd.Presentacion = detEspecial.objPresentacion.IdPresentacion;
                solProd.Cliente = detEspecial.objCabecera.Cliente;
                solProd.DetallePedido = detEspecial.IdDetallePedido;
                solProd.Cantidad = Convert.ToInt32(detEspecial.Cantidad);

                Contexto.SolicitudProductosEspeciales.InsertOnSubmit(solProd);
            }
        }

        foreach (var item in idsProductosIncorporaciones_1_2015)
        {
            detEspecial = DetallePedidos.Where(w => w.objProducto.IdProducto == long.Parse(item)).FirstOrDefault();
            if (detEspecial != null)
            {
                SolicitudProductosEspeciale solProd = new SolicitudProductosEspeciale();
                solProd.Producto = detEspecial.objProducto.IdProducto;
                solProd.Presentacion = detEspecial.objPresentacion.IdPresentacion;
                solProd.Cliente = detEspecial.objCabecera.Cliente;
                solProd.DetallePedido = detEspecial.IdDetallePedido;
                solProd.Cantidad = Convert.ToInt32(detEspecial.Cantidad);

                Contexto.SolicitudProductosEspeciales.InsertOnSubmit(solProd);
            }
        }


        detEspecial = DetallePedidos.Where(w => w.objProducto.IdProducto == long.Parse(idProductoIncorporacion_0)).FirstOrDefault();
        if (detEspecial != null)
        {
            SolicitudProductosEspeciale solProd = new SolicitudProductosEspeciale();
            solProd.Producto = detEspecial.objProducto.IdProducto;
            solProd.Presentacion = detEspecial.objPresentacion.IdPresentacion;
            solProd.Cliente = detEspecial.objCabecera.Cliente;
            solProd.DetallePedido = detEspecial.IdDetallePedido;
            solProd.Cantidad = Convert.ToInt32(detEspecial.Cantidad);

            Contexto.SolicitudProductosEspeciales.InsertOnSubmit(solProd);
        }


        detEspecial = DetallePedidos.Where(w => w.objProducto.IdProducto == long.Parse(idProductoIncorporacion_0_2014)).FirstOrDefault();
        if (detEspecial != null)
        {
            SolicitudProductosEspeciale solProd = new SolicitudProductosEspeciale();
            solProd.Producto = detEspecial.objProducto.IdProducto;
            solProd.Presentacion = detEspecial.objPresentacion.IdPresentacion;
            solProd.Cliente = detEspecial.objCabecera.Cliente;
            solProd.DetallePedido = detEspecial.IdDetallePedido;
            solProd.Cantidad = Convert.ToInt32(detEspecial.Cantidad);

            Contexto.SolicitudProductosEspeciales.InsertOnSubmit(solProd);
        }


        Contexto.SubmitChanges();
    }

    private void CargarFormaDePago(Cliente CurrrentCliente)
    {
        List<FormaDePago> FormasDePago = new List<FormaDePago>();
        cboFormaPago.Items.Clear();
        cboFormaPago.DataTextField = "Descripcion";
        cboFormaPago.DataValueField = "IdFormaPago";

        if (CurrrentCliente != null)
        {
            if (UCTotalizadorNivel.PoseeRequerimientoPagoFacil)
            {
                if (CurrrentCliente.Login == "DEMO")
                {
                    FormasDePago = (from Fp in Contexto.FormaDePagos
                                    where Fp.Cliente == 2
                                    select Fp).ToList();
                }
                else
                {

                    /// Si tiene un requerimiento de pago facil
                    /// quiere decir que esta penalizado y solo puede
                    /// usar como forma de pago Pago Fácil o Pago Mis Cuentas
                    FormasDePago = Contexto.FormaDePagos.Where(w => !w.Descripcion.Contains("Contra") && w.Cliente == 2 && w.Codigo != "TRJ").ToList();
                }
            }
            else
            {
                if (CurrrentCliente.Login == "DEMO")
                {
                    FormasDePago = (from Fp in Contexto.FormaDePagos
                                    where Fp.Cliente == 2
                                    select Fp).ToList();
                }

                else if (CurrrentCliente.Cod_CondVta == "SIN")
                {

                    FormasDePago = (from Fp in Contexto.FormaDePagos
                                    where Fp.Cliente == 2 && Fp.Codigo != "TRJ"
                                    select Fp).ToList();
                }
                else
                {
                    FormasDePago = (from Fp in Contexto.FormaDePagos
                                    where Fp.Cliente == 2
                                    && Fp.Codigo != "SIN" && Fp.Codigo != "TRJ"
                                    select Fp).ToList();
                }
            }


            /// Controlo si el cliente puede utilizar la forma de pago en crédito
            /// se controla sobre el cliente que esta logeado, que puede ser o no el destinatario
            /// de la nota de pedido.
            if ((Session["ClienteLogeado"] as Cliente).ColConfCreditos.Count > 0)
            {
                Session["MontoDisponibleCredito"] = (Session["ClienteLogeado"] as Cliente).ColConfCreditos.First().MontoAsignado - (Session["ClienteLogeado"] as Cliente).ColConfCreditos.First().MontoUtilizado;

                if ((Session["ClienteLogeado"] as Cliente).ColConfCreditos.First().Activo &&
                    decimal.Parse(Session["MontoDisponibleCredito"].ToString()) > 0)
                {
                    FormasDePago.AddRange((from Fp in Contexto.FormaDePagos
                                           where Fp.Cliente == 4
                                           select Fp).ToList());


                }
            }


            cboFormaPago.DataSource = FormasDePago;
            cboFormaPago.DataBind();
            cboFormaPago.SelectedValue = "0";
            upFormaDePago.Update();
        }
        else
        {
            cboFormaPago.DataSource = new List<FormaDePago>();
            cboFormaPago.DataBind();
            upFormaDePago.Update();
        }






    }

    private void CalcularTotalPedido()
    {
        if ((Session["detPedido"] as List<DetallePedido>).Count > 0)
        {

            decimal Total = 0;
            decimal MontoTotal = 0;
            decimal DescuentosPromociones = 0;
            decimal DescuentoProvincia = 0;
            decimal ValorFlete = Convert.ToDecimal(lblTransporteValorHidden.Value);
            decimal PorcentajeDescuentoProvincia = Convert.ToDecimal(lblProvinciaPorcentajeDescuentoHidden.Value);
            decimal DescuentosRemitos = 0;
            decimal DescuentosGenerales = 0;

            Total = (from P in (Session["detPedido"] as List<DetallePedido>)
                     select P.Cantidad.Value).Sum();

            /// Sumo todos los productos directos
            MontoTotal = (from P in (Session["detPedido"] as List<DetallePedido>) select P.ValorTotal.Value).Sum();


            if (Session["PromosGeneradas"] != null && (Session["PromosGeneradas"] as List<DetallePedido>).Count > 0)
            {
                DescuentosPromociones = (from P in (Session["PromosGeneradas"] as List<DetallePedido>)
                                         where P.ColRegalos != null && P.ColRegalos.Any(w => w.TipoRegalo == "Descuento")
                                         && P.ValorTotal.HasValue
                                         select P.ValorTotal.Value).Sum();
            }

            /// Se calculan los descuentos según el porcentaje de cada producto.
            DescuentosGenerales = CalcularDescuentosGenerales();

            /// Se calcula de acuerdo a los componentes de los mismos.
            DescuentosRemitos = CalcularDescuentoRemitos();

            /// Descuento de las provincias
            /// Articulos - promos + flete = total 
            DescuentoProvincia = Math.Round(((MontoTotal - DescuentosPromociones + ValorFlete + DescuentosRemitos + DescuentosGenerales) * PorcentajeDescuentoProvincia) / 100, 2);

            /// Actualizo tooltip del detalle del monto del pedido.
            lblMontoProductos.Text = string.Format("$ {0:0.00}", (MontoTotal));
            lblCostoFlete.Text = string.Format("$ {0:0.00}", (ValorFlete));
            lblDescuentos.Text = string.Format("$ {0:0.00}", (DescuentosPromociones));
            lblDescuentoProvincia.Text = string.Format("$ {0:0.00}", (DescuentoProvincia));

            /// Cambio solicitado el 15/06/2015: Dejar de utilizar el valor absoluto para mostrar el descuento del remito.
            /// ademas de cambio de la etiqueta.
            //lblDescuentoRemitos.Text = string.Format("$ {0:0.00}", (Math.Abs(DescuentosRemitos)));
            lblDescuentoRemitos.Text = string.Format("$ {0:0.00}", DescuentosRemitos);

            lblDescuentosGenerales.Text = string.Format("$ {0:0.00}", (Math.Abs(DescuentosGenerales)));

            MontoTotal += ValorFlete - DescuentosPromociones - DescuentoProvincia + DescuentosRemitos + DescuentosGenerales;

            MontoTotal = CalculoImpuestos(MontoTotal);

            txtTotalGeneral.Text = string.Format("{0:0}", (Total));
            txtMontoGeneral.Text = string.Format("$ {0:0.00}", (MontoTotal));
            lblMontoActual.Text = string.Format("$ {0:0.00}", (MontoTotal));
            lblTotalDetalle.Text = string.Format("$ {0:0.00}", (MontoTotal));


        }
        else
        {
            lblMontoProductos.Text = string.Format("$ {0:0.00}", 0);
            lblCostoFlete.Text = string.Format("$ {0:0.00}", 0);
            lblDescuentos.Text = string.Format("$ {0:0.00}", 0);
            lblDescuentoProvincia.Text = string.Format("$ {0:0.00}", 0);
            lblDescuentoRemitos.Text = string.Format("$ {0:0.00}", 0);
            lblDescuentosGenerales.Text = string.Format("$ {0:0.00}", 0);
            txtTotalGeneral.Text = string.Format("{0:0}", 0);
            txtMontoGeneral.Text = string.Format("$ {0:0.00}", 0);
            lblMontoActual.Text = string.Format("$ {0:0.00}", 0);
            lblImpuestos.Text = string.Format("$ {0:0.00}", 0);
            lblNeto.Text = string.Format("$ {0:0.00}", 0);
            lblIVA.Text = string.Format("$ {0:0.00}", 0);
            lblRG30.Text = string.Format("$ {0:0.00}", 0);
            lblRG212.Text = string.Format("$ {0:0.00}", 0);
        }

        UpToolTipMontoPedido.Update();
    }

    private string DeteminarSituacionImpositiva(Cliente currentCliente)
    {
        /// Determino la situación impositva del cliente buscando al mismo en la tabla de Sujetos No Categorizdos, si esta 
        /// en esta tabla entonces es sujeto no categorizado, caso contrario utilizo la situación impositiva del cliente.
        using (Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext())
        {
            bool esSujetoNoCategorizado = dc.SujetosNoCategorizados.Any(c => c.CodClienteBejerman.Trim() == currentCliente.CodigoExterno.Trim());
            if (esSujetoNoCategorizado)
            {
                return "7"; //Sujeto no Categorizado
            }
            else
                return currentCliente.Cod_SitIVA;
        }

    }

    private decimal CalculoImpuestos(decimal MontoTotal)
    {

        lblSubTotal.Text = string.Format("$ {0:0.00}", (MontoTotal));


        /// Calculo el Impuesto RG212 y RG30
        Cliente currentCliente = (Cliente)Session["Cliente"];
        decimal GR212 = Convert.ToDecimal("10,5");
        decimal GR30InsMono = Convert.ToDecimal("1");
        decimal GR30SNC = Convert.ToDecimal("2");
        decimal GR30JUJUY = Convert.ToDecimal("3");

        decimal imp212 = 0;
        decimal imp30 = 0;
        decimal neto = 0;

        decimal montoSujetoNoCategorizado = 0;
        string codSituacionImpositiva = DeteminarSituacionImpositiva(currentCliente);

        ///Cambio solicitado el 02/12/2013 donde la regla de negocio dice que si es de tierra del fuego
        ///no se tiene que calcular ningun tipo de impuesto.
        if (ProvinciaDireccionSeleccionada != "" && ProvinciaDireccionSeleccionada == "TIERRA DEL FUEGO")
        {
            ImpuestoCalculado = 0;
            DetalleImpuestosCalculados = "";
            lblImpuestos.Text = string.Format("$ {0:0.00}", 0);
            lblNeto.Text = string.Format("$ {0:0.00}", 0);
            lblIVA.Text = string.Format("$ {0:0.00}", (0));
            lblRG30.Text = string.Format("$ {0:0.00}", 0);
            lblRG212.Text = string.Format("$ {0:0.00}", 0);
        }
        else if (ProvinciaDireccionSeleccionada != "" && ProvinciaDireccionSeleccionada == "JUJUY")
        {


            ImpuestoCalculado = 0;
            DetalleImpuestosCalculados = "";
            lblImpuestos.Text = string.Format("$ {0:0.00}", 0);
            lblNeto.Text = string.Format("$ {0:0.00}", 0);
            lblIVA.Text = string.Format("$ {0:0.00}", (0));
            lblRG30.Text = string.Format("$ {0:0.00}", 0);
            lblRG212.Text = string.Format("$ {0:0.00}", 0);

            switch (codSituacionImpositiva)
            {
                case "7": //Sujeto no Categorizado
                    if (MontoTotal >= montoSujetoNoCategorizado)
                    {

                        // Cambio solicitado el 15/06/2015
                        //neto = MontoTotal * Convert.ToDecimal("0,904975");
                        neto = MontoTotal;

                        // Calculo del RG212
                        imp212 = Math.Round((neto) * (GR212 / 100), 2);

                        ///Cambio solicitado el 09/01/2015 donde la regla de negocio dice que si es de Jujuy
                        ///ya no se tiene que calcular el impuesto RG30.

                        // Calculo del RG30
                        //imp30 = Math.Round(((neto) / Convert.ToDecimal("1,21")) * (GR30JUJUY / 100), 2);

                        MontoTotal = neto + imp30 + imp212;
                        lblImpuestos.Text = string.Format("$ {0:0.00}", (imp30 + imp212));
                        lblNeto.Text = string.Format("$ {0:0.00}", (neto));
                        lblIVA.Text = string.Format("$ {0:0.00}", (0));
                        lblRG30.Text = string.Format("$ {0:0.00}", (imp30));
                        lblRG212.Text = string.Format("$ {0:0.00}", (imp212));

                        /// Finalmente actualizo el total final dentro del detalle.
                        lblTotalDetalle.Text = string.Format("$ {0:0.00}", (MontoTotal));

                        ImpuestoCalculado = imp30 + imp212;
                        DetalleImpuestosCalculados = lblNeto.Text + "@" + lblIVA.Text + "@" + lblRG30.Text + "@" + lblRG212.Text + "@" + lblTotalDetalle.Text;

                    }
                    else
                    {
                        ImpuestoCalculado = 0;
                        DetalleImpuestosCalculados = "";
                        lblImpuestos.Text = string.Format("$ {0:0.00}", 0);
                        lblNeto.Text = string.Format("$ {0:0.00}", 0);
                        lblIVA.Text = string.Format("$ {0:0.00}", (0));
                        lblRG30.Text = string.Format("$ {0:0.00}", 0);
                        lblRG212.Text = string.Format("$ {0:0.00}", 0);
                    }
                    break;
                case "1": //Inscripto
                    neto = MontoTotal / Convert.ToDecimal("1,21");
                    imp30 = Math.Round(neto * (GR30JUJUY / 100), 2);


                    lblImpuestos.Text = string.Format("$ {0:0.00}", (imp30));
                    lblNeto.Text = string.Format("$ {0:0.00}", (neto));
                    lblIVA.Text = string.Format("$ {0:0.00}", (MontoTotal - neto));
                    lblRG30.Text = string.Format("$ {0:0.00}", (imp30));
                    lblRG212.Text = string.Format("$ {0:0.00}", (0));

                    MontoTotal += imp30;

                    /// Finalmente actualizo el total final dentro del detalle.
                    lblTotalDetalle.Text = string.Format("$ {0:0.00}", (MontoTotal));

                    ImpuestoCalculado = imp30;
                    DetalleImpuestosCalculados = lblNeto.Text + "@" + lblIVA.Text + "@" + lblRG30.Text + "@" + lblRG212.Text + "@" + lblTotalDetalle.Text;


                    break;
                case "6": //Responsable Monotributo
                    imp30 = Math.Round(MontoTotal * (GR30JUJUY / 100), 2);

                    lblNeto.Text = string.Format("$ {0:0.00}", (MontoTotal));
                    lblIVA.Text = string.Format("$ {0:0.00}", 0);
                    lblRG30.Text = string.Format("$ {0:0.00}", imp30);
                    lblImpuestos.Text = string.Format("$ {0:0.00}", (imp30));
                    lblRG212.Text = string.Format("$ {0:0.00}", (0));

                    MontoTotal += imp30;

                    /// Finalmente actualizo el total final dentro del detalle.
                    lblTotalDetalle.Text = string.Format("$ {0:0.00}", (MontoTotal));

                    ImpuestoCalculado = imp30;
                    DetalleImpuestosCalculados = lblNeto.Text + "@" + lblIVA.Text + "@" + lblRG30.Text + "@" + lblRG212.Text + "@" + lblTotalDetalle.Text;


                    break;
                default:
                    lblImpuestos.Text = string.Format("$ {0:0.00}", 0);
                    ImpuestoCalculado = 0;
                    DetalleImpuestosCalculados = "";
                    break;
            }
        }
        else
        {

            if (currentCliente.Provincia.ToUpper() == "MENDOZA")
            {

                switch (codSituacionImpositiva)
                {
                    case "7": //Sujeto no Categorizado
                        if (MontoTotal >= montoSujetoNoCategorizado)
                        {

                            // Caclulo del Neto del pedido
                            // Cambio solicitado el 15/06/2015
                            //neto = MontoTotal * Convert.ToDecimal("0,904975");
                            neto = MontoTotal;

                            // Calculo del RG212
                            imp212 = Math.Round((neto) * (GR212 / 100), 2);

                            // Calculo del RG30
                            imp30 = Math.Round(((neto) / Convert.ToDecimal("1,21")) * (GR30SNC / 100), 2);

                            MontoTotal = neto + imp30 + imp212;
                            lblImpuestos.Text = string.Format("$ {0:0.00}", (imp30 + imp212));
                            lblNeto.Text = string.Format("$ {0:0.00}", (neto));
                            lblIVA.Text = string.Format("$ {0:0.00}", (0));
                            lblRG30.Text = string.Format("$ {0:0.00}", (imp30));
                            lblRG212.Text = string.Format("$ {0:0.00}", (imp212));

                            /// Finalmente actualizo el total final dentro del detalle.
                            lblTotalDetalle.Text = string.Format("$ {0:0.00}", (MontoTotal));

                            ImpuestoCalculado = imp30 + imp212;
                            DetalleImpuestosCalculados = lblNeto.Text + "@" + lblIVA.Text + "@" + lblRG30.Text + "@" + lblRG212.Text + "@" + lblTotalDetalle.Text;

                        }
                        else
                        {
                            ImpuestoCalculado = 0;
                            DetalleImpuestosCalculados = "";
                            lblImpuestos.Text = string.Format("$ {0:0.00}", 0);
                            lblNeto.Text = string.Format("$ {0:0.00}", 0);
                            lblIVA.Text = string.Format("$ {0:0.00}", (0));
                            lblRG30.Text = string.Format("$ {0:0.00}", 0);
                            lblRG212.Text = string.Format("$ {0:0.00}", 0);
                        }
                        break;
                    case "1": //Inscripto
                        neto = MontoTotal / Convert.ToDecimal("1,21");
                        imp30 = Math.Round(neto * (GR30InsMono / 100), 2);


                        lblImpuestos.Text = string.Format("$ {0:0.00}", (imp30));
                        lblNeto.Text = string.Format("$ {0:0.00}", (neto));
                        lblIVA.Text = string.Format("$ {0:0.00}", (MontoTotal - neto));
                        lblRG30.Text = string.Format("$ {0:0.00}", (imp30));
                        lblRG212.Text = string.Format("$ {0:0.00}", (0));

                        MontoTotal += imp30;

                        /// Finalmente actualizo el total final dentro del detalle.
                        lblTotalDetalle.Text = string.Format("$ {0:0.00}", (MontoTotal));

                        ImpuestoCalculado = imp30;
                        DetalleImpuestosCalculados = lblNeto.Text + "@" + lblIVA.Text + "@" + lblRG30.Text + "@" + lblRG212.Text + "@" + lblTotalDetalle.Text;


                        break;
                    case "6": //Responsable Monotributo
                        imp30 = Math.Round(MontoTotal * (GR30InsMono / 100), 2);

                        lblNeto.Text = string.Format("$ {0:0.00}", (MontoTotal));
                        lblIVA.Text = string.Format("$ {0:0.00}", 0);
                        lblRG30.Text = string.Format("$ {0:0.00}", imp30);
                        lblImpuestos.Text = string.Format("$ {0:0.00}", (imp30));
                        lblRG212.Text = string.Format("$ {0:0.00}", (0));

                        MontoTotal += imp30;

                        /// Finalmente actualizo el total final dentro del detalle.
                        lblTotalDetalle.Text = string.Format("$ {0:0.00}", (MontoTotal));

                        ImpuestoCalculado = imp30;
                        DetalleImpuestosCalculados = lblNeto.Text + "@" + lblIVA.Text + "@" + lblRG30.Text + "@" + lblRG212.Text + "@" + lblTotalDetalle.Text;


                        break;
                    default:
                        lblImpuestos.Text = string.Format("$ {0:0.00}", 0);
                        ImpuestoCalculado = 0;
                        DetalleImpuestosCalculados = "";
                        break;
                }


            }
            ///// Para el resto de las Provincias
            else
            {
                switch (codSituacionImpositiva)
                {
                    case "7": //Sujeto no Categorizado
                        if (MontoTotal >= montoSujetoNoCategorizado)
                        {
                            // Caclulo del Neto del pedido
                            // Cambio solicitado el 15/06/2015
                            //neto = MontoTotal * Convert.ToDecimal("0,904975");
                            neto = MontoTotal;


                            // Calculo del RG212
                            imp212 = Math.Round((neto) * (GR212 / 100), 2);


                            MontoTotal = neto + imp212;
                            lblImpuestos.Text = string.Format("$ {0:0.00}", (imp212));
                            lblNeto.Text = string.Format("$ {0:0.00}", (neto));
                            lblIVA.Text = string.Format("$ {0:0.00}", (0));
                            lblRG30.Text = string.Format("$ {0:0.00}", (0));
                            lblRG212.Text = string.Format("$ {0:0.00}", (imp212));


                            /// Finalmente actualizo el total final dentro del detalle.
                            lblTotalDetalle.Text = string.Format("$ {0:0.00}", (MontoTotal));

                            ImpuestoCalculado = imp212;
                            DetalleImpuestosCalculados = lblNeto.Text + "@" + lblIVA.Text + "@" + lblRG30.Text + "@" + lblRG212.Text + "@" + lblTotalDetalle.Text;

                        }
                        else
                        {
                            ImpuestoCalculado = 0;
                            DetalleImpuestosCalculados = "";
                            lblImpuestos.Text = string.Format("$ {0:0.00}", 0);
                            lblNeto.Text = string.Format("$ {0:0.00}", 0);
                            lblIVA.Text = string.Format("$ {0:0.00}", (0));
                            lblRG30.Text = string.Format("$ {0:0.00}", 0);
                            lblRG212.Text = string.Format("$ {0:0.00}", 0);
                        }
                        break;
                    case "1": //Inscripto
                        neto = MontoTotal / Convert.ToDecimal("1,21");

                        lblImpuestos.Text = string.Format("$ {0:0.00}", 0);
                        lblNeto.Text = string.Format("$ {0:0.00}", (neto));
                        lblIVA.Text = string.Format("$ {0:0.00}", (MontoTotal - neto));
                        lblRG30.Text = string.Format("$ {0:0.00}", (imp30));
                        lblRG212.Text = string.Format("$ {0:0.00}", (0));


                        /// Finalmente actualizo el total final dentro del detalle.
                        lblTotalDetalle.Text = string.Format("$ {0:0.00}", (MontoTotal));


                        ImpuestoCalculado = 0;
                        DetalleImpuestosCalculados = lblNeto.Text + "@" + lblIVA.Text + "@" + lblRG30.Text + "@" + lblRG212.Text + "@" + lblTotalDetalle.Text;


                        break;
                    case "6": //Responsable Monotributo
                        imp30 = Math.Round(MontoTotal * (GR30InsMono / 100), 2);

                        lblNeto.Text = string.Format("$ {0:0.00}", (MontoTotal));
                        lblIVA.Text = string.Format("$ {0:0.00}", 0);
                        lblRG30.Text = string.Format("$ {0:0.00}", 0);
                        lblImpuestos.Text = string.Format("$ {0:0.00}", 0);
                        lblRG212.Text = string.Format("$ {0:0.00}", 0);


                        /// Finalmente actualizo el total final dentro del detalle.
                        lblTotalDetalle.Text = string.Format("$ {0:0.00}", (MontoTotal));


                        ImpuestoCalculado = 0;
                        DetalleImpuestosCalculados = lblNeto.Text + "@" + lblIVA.Text + "@" + lblRG30.Text + "@" + lblRG212.Text + "@" + lblTotalDetalle.Text;

                        break;
                    default:
                        lblImpuestos.Text = string.Format("$ {0:0.00}", 0);
                        ImpuestoCalculado = 0;
                        DetalleImpuestosCalculados = "";
                        break;
                }
            }

        }

        return MontoTotal;
    }

    private void ActualizarTotalesGenerales()
    {

        if ((Session["detPedido"] as List<DetallePedido>).Count > 0)
        {
            /// Calculo las promociones para actualizar la promociones
            CalcularPromociones();

            /// El primer llamado es para actualizar el total del 
            /// pedido con la modificación del detalle que se ha realizado
            /// pedido, actualización o eliminación de productos
            CalcularTotalPedido();

        }
        else
        {
            txtTotalGeneral.Text = "0";
            txtMontoGeneral.Text = "$ 0";
            lblMontoActual.Text = "$ 0";
        }

        UcTotalizadorPedidos.InitControl(0);
        upResumen.Update();
        upTotalizarCabecera.Update();
        upMontoActual.Update();
    }

    private void CargarDireccionesEntrega(Cliente CurrentCliente)
    {
        #region  Actualizo las direcciones de entrega

        lblFechaImp.Text = DateTime.Now.ToShortDateString();
        lblNroNotaImp.Text = ObtenerSeguienteNro(0, Contexto);


        Direccione direccionIncial = (from D in Contexto.Direcciones
                                      where D.CodigoExterno == CurrentCliente.CodigoExterno
                                      && D.EsPrincipal.Value
                                      orderby D.CodigoExternoDir
                                      select D).FirstOrDefault<Direccione>();

        if (direccionIncial == null)
        {

            direccionIncial = (from D in Contexto.Direcciones
                               where D.CodigoExterno == CurrentCliente.CodigoExterno
                               orderby D.CodigoExternoDir
                               select D).FirstOrDefault<Direccione>();
        }



        if (direccionIncial != null)
        {
            IdDireccionSeleccionada = direccionIncial.IdDireccion;
            ProvinciaDireccionSeleccionada = direccionIncial.Provincia.ToUpper();

            ucGrillaDirecciones.InitControl(CurrentCliente.CodigoExterno);
            upDirec.Update();
        }
        else
        {
            ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "SinDir", "AlertaSinDireccion();", true);
            upSolicitudPedido.Update();
            return;
        }
        #endregion

        #region  Busco si hay descuento por provincia
        Parametro ParamProvincia = (from D in Contexto.Parametros
                                    where D.Tipo.ToLower() == direccionIncial.Provincia.ToLower()
                                    select D).FirstOrDefault();

        if (ParamProvincia != null)
        {
            lblProvinciaPorcentajeDescuentoHidden.Value = ParamProvincia.Valor.Replace(".", ",");
        }
        else
        {
            lblProvinciaPorcentajeDescuentoHidden.Value = "0";
        }

        #endregion

        #region  Busco el costo del flete segun los datos del transportistas.
        ConfTransporte confT = (from C in Contexto.ConfTransportes
                                where C.Provincia.ToLower() == direccionIncial.Provincia.ToLower() &&
                                C.Localidad.ToLower() == direccionIncial.Localidad.ToLower() &&
                                C.FormaDePago == cboFormaPago.SelectedItem.Text
                                select C).FirstOrDefault<ConfTransporte>();

        if (confT != null)
        {

            lblTransporte.Text = confT.Transporte.ToLower();
            lblTransporteHidden.Value = confT.Transporte.ToLower();
            lblTransporteValorHidden.Value = confT.objProducto.ColPresentaciones[0].Precio.ToString();
            upTransporte.Update();
        }
        #endregion

        #region Actualización ToolTip

        lblToolTipNombre.Text = CurrentCliente.Nombre.ToLower();
        lblToolTipDNI.Text = CurrentCliente.Dni;
        lblToolTipTel.Text = CurrentCliente.Telefono;
        lblToolTipEmail.Text = CurrentCliente.Email;

        if (CurrentCliente.TipoConsultor != null)
            lblToolTipTipoConsultor.Text = CurrentCliente.TipoConsultor.ToLower();

        lblToolTipSitImpositiva.Text = CurrentCliente.Desc_SitIVA.ToLower();
        upFichaTecnica.Update();
        #endregion

    }

    private void ActualizarProductosEspeciales(Cliente CurrentCliente)
    {

        /// (1 Reglas): Solo aplicable al mes de enero del 2013, luego se aplica la regla segun las solicitudes de productos especiales.
        /// 1. si el cliente es de tipo consultor verifico la cantidad de pedido que posee, si el mismo
        /// posee mas de un pedido entonces no se le debe dejar solicitar incorporaciones.
        /// 
        /// 2. si el cliente no es consultor o si es consultor pero tiene cero o un pedido entonces, realizo el control
        /// segun las solicitudes de productos especiales. (2 Regla)

        /// (2 Regla): regla segun las solicitudes de productos especiales.
        List<SolicitudProductosEspeciale> solicitudesIncorporaciones = (from I in Contexto.SolicitudProductosEspeciales
                                                                        where I.Cliente == CurrentCliente.IdCliente
                                                                        select I).ToList();

        ///1. Busco primero si ya solicito la incorporacion 0, si es asi la
        /// excluyo de la lista de productos posibles de solicitar.
        if (solicitudesIncorporaciones.Any(w => w.Producto == long.Parse(idProductoIncorporacion_0) || w.Producto == long.Parse(idProductoIncorporacion_0_2014)))
        {
            /// Actualización Regla 01/04/2015: si durante el mes en cuestion no ha solicitado la incorportacion 0 entonces lo dejo solicitar.
            /// es decir una sola incorportacion por pedido y por mes.
            if (solicitudesIncorporaciones.Any(w => w.objDetallePedido.objCabecera.FechaPedido.Month == DateTime.Now.Month && w.objDetallePedido.objCabecera.FechaPedido.Year == DateTime.Now.Year))
            {
                HiddenProductosOcultos.Value += idProductoIncorporacion_0.ToString();
                HiddenProductosOcultos.Value += "|" + idProductoIncorporacion_0_2014.ToString();
            }
        }

        /// Estas reglas se comentaron a partir del 01/04/2015

        /////2. Busco si ya a solicitado alguna de las incorporaciones de la 1 a la 4 si es asi las
        ///// excluyo de la lista de productos posibles de solicitar y tb excluyo la incorporacion 0, ya que al pedir 
        ///// una de estas automaticamente queda imposibilitado de pedir la 0.
        //foreach (var item in idsProductosIncorporaciones_1_4)
        //{
        //    if (solicitudesIncorporaciones.Any(w => w.Producto == long.Parse(item)))
        //    {

        //        RadTreeNode NodoInc = RadTreeProductos.FindNodeByAttribute("NodoIncorporaciones", "true");
        //        NodoInc.Visible = false;
        //        upTreeProductos.Update();

        //        break;
        //    }
        //}

        //foreach (var item in idsProductosIncorporaciones_1_2_2014)
        //{
        //    if (solicitudesIncorporaciones.Any(w => w.Producto == long.Parse(item)))
        //    {
        //        RadTreeNode NodoInc = RadTreeProductos.FindNodeByAttribute("NodoIncorporaciones", "true");
        //        NodoInc.Visible = false;
        //        upTreeProductos.Update();
        //        break;
        //    }
        //}

        upHiddenProductos.Update();

        #region Logica utilizada hasta el 03/01/2013
        //IncorporacionesHistorica IncorporacionHistorica = (from I in Contexto.IncorporacionesHistoricas
        //                                                   where I.CodCliente == CurrentCliente.CodigoExterno
        //                                                   select I).SingleOrDefault();

        ///// Regla de Negocio segun datos tabla clientes.
        ///// 1. Si ya pidio el producto CARTUCHERA no le doy la posibilidad 
        ///// de hacer otro pedido.
        ///// 2. Si posee incorporacion en los pedidos o en historico no le
        ///// dejo pedir ningun otro tipo de incorporación, si no posee incorporación
        ///// y es el PRIMER PEDIDO que va a realizar entonces lo dejo solicitar

        //#region 1. Producto Particular Cartuchera

        ///// Cartuchera Completa con Ficha Técnica : 2858
        //if (CurrentCliente.PoseeCartuchera.Value)
        //    HiddenPoseeCartuchera.Value = "false";
        //else
        //{
        //    if (IncorporacionHistorica == null)
        //        HiddenPoseeCartuchera.Value = "false";
        //    else if (IncorporacionHistorica.PoseeCartuchera.Value)
        //        HiddenPoseeCartuchera.Value = "false";
        //    else
        //        HiddenPoseeCartuchera.Value = "true";
        //}

        //#endregion


        //#region 2. Nodo Incorporaciones del árbol
        //RadTreeNode NodoInc = RadTreeProductos.FindNodeByAttribute("NodoIncorporaciones", "true");

        //if (CurrentCliente.PoseeIncorporacion.Value)
        //    NodoInc.Visible = false;
        //else
        //{
        //    long idCliente = CurrentCliente.IdCliente;

        //    var PermitirPedido = (from c in Contexto.CabeceraPedidos
        //                          where c.Cliente == idCliente
        //                          select c).Count();

        //    if (PermitirPedido > 0)
        //    {
        //        NodoInc.ParentNode.Visible = false;
        //    }
        //    else
        //    {

        //        if (IncorporacionHistorica == null)
        //            NodoInc.Visible = true;
        //        else
        //            NodoInc.Visible = false;
        //    }
        //}

        //upTreeProductos.Update();

        //#endregion

        #endregion

    }

    private void CargarEncabezado(Cliente CurrentCliente, long IdPedido)
    {
        Direccione dirPrincipal = null;
        List<Direccione> direcciones = (from D in Contexto.Direcciones
                                        where D.CodigoExterno == CurrentCliente.CodigoExterno
                                        orderby D.CodigoExternoDir
                                        select D).ToList<Direccione>();



        if (direcciones.Count > 0)
        {
            if (IdPedido == 0)
            {
                dirPrincipal = direcciones.Where(w => w.EsPrincipal.Value).FirstOrDefault();
            }
            else
            {
                var DirGrabada = (from D in Contexto.Direcciones
                                  where D.IdDireccion == _idDireccionGrabada
                                  select D).FirstOrDefault<Direccione>();

                if (DirGrabada != null)
                    dirPrincipal = DirGrabada;
                else
                    dirPrincipal = direcciones.First();
            }

        }

        if (dirPrincipal != null)
        {

            lblDireccionEntrega.Text = dirPrincipal.Provincia.ToLower() + " - " + dirPrincipal.Localidad.ToLower();
            lblCalle.Text = dirPrincipal.Calle.ToLower();
            lblFechaImp.Text = DateTime.Now.ToShortDateString();
            lblNroNotaImp.Text = ObtenerSeguienteNro(IdPedido, Contexto);
            IdDireccionSeleccionada = dirPrincipal.IdDireccion;
            ProvinciaDireccionSeleccionada = dirPrincipal.Provincia.ToUpper();

            var ultimoPedido = (from v in Contexto.View_UltimoPedidoClientes
                                where v.IdCliente == CurrentCliente.IdCliente
                                select v.UltimaFechaPedido).FirstOrDefault();

            lblUltimoPedido.Text = ultimoPedido == null ? "Sin Pedido Previo" : ultimoPedido.Value.ToShortDateString();


            try
            {

                ConfTransporte confT = (from C in Contexto.ConfTransportes
                                        where C.Provincia.ToLower() == dirPrincipal.Provincia.ToLower() &&
                                        C.Localidad.ToLower() == dirPrincipal.Localidad.ToLower() &&
                                        C.FormaDePago == cboFormaPago.SelectedItem.Text
                                        select C).First<ConfTransporte>();



                lblTransporte.Text = confT.Transporte.ToLower();
                lblTransporteHidden.Value = confT.Transporte.ToLower();
                lblTransporteValorHidden.Value = confT.objProducto.ColPresentaciones[0].Precio.ToString();


                Parametro ParamProvincia = (from D in Contexto.Parametros
                                            where D.Tipo.ToLower() == dirPrincipal.Provincia.ToLower()
                                            select D).FirstOrDefault();

                if (ParamProvincia != null)
                {
                    lblProvinciaPorcentajeDescuentoHidden.Value = ParamProvincia.Valor.Replace(".", ",");

                }

            }
            catch
            {
                lblTransporte.Text = "SIN TRANSPORTE";
                lblTransporteHidden.Value = "";
                lblTransporteValorHidden.Value = "0";
                lblProvinciaPorcentajeDescuentoHidden.Value = "0";
                lblDescuentoProvincia.Text = "$ 0";

            }

            CargarDatosJerarquia();

            lblToolTipNombre.Text = CurrentCliente.Nombre.ToLower();
            lblToolTipDNI.Text = CurrentCliente.Dni;
            lblToolTipTel.Text = CurrentCliente.Telefono;
            lblToolTipEmail.Text = CurrentCliente.Email;


            if (CurrentCliente.TipoConsultor != null)
                lblToolTipTipoConsultor.Text = CurrentCliente.TipoConsultor.ToLower();

            lblToolTipSitImpositiva.Text = CurrentCliente.Desc_SitIVA.ToLower();
            upFichaTecnica.Update();


            Session["Consultor"] = CurrentCliente;
            ucGrillaDirecciones.InitControl(CurrentCliente.CodigoExterno);
            upDirec.Update();
            upEncImp.Update();

        }
        else
        {
            Session["Consultor"] = CurrentCliente;
            ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "SinDir", "AlertaSinDireccion();", true);
            upSolicitudPedido.Update();
            return;
        }

        ActualizarProductosEspeciales(CurrentCliente);
    }

    private void CargarDatosJerarquia()
    {
        Cliente CurrentCliente = (Cliente)Session["Cliente"];
        CalcularSaldoPagoAnticipado(CurrentCliente, Contexto);

        if (CurrentCliente.CodTipoCliente == TipoClientesConstantes.CONSULTOR)
        {
            Cliente clienteSuperior = (from C in Contexto.Clientes
                                       where C.CodTipoCliente != TipoClientesConstantes.CONSULTOR
                                          && C.CodClasif1 == CurrentCliente.CodClasif1
                                       select C).First();

            if ((clienteSuperior.CodTipoCliente == TipoClientesConstantes.SUBCONGESTION || clienteSuperior.CodTipoCliente == TipoClientesConstantes.SUBSINGESTION)
                && clienteSuperior.TipoCliente.ToUpper().Contains("SUB"))
            {
                lblResponsable.Text = clienteSuperior.Vendedor.ToLower();

            }
            else
            {
                lblResponsable.Text = clienteSuperior.Nombre.ToLower();
            }

        }
        else
        {
            lblResponsable.Text = CurrentCliente.Nombre.ToLower();
        }

        /// Muestro siempre el grupo al que pertenece el usuario. clasif1 == Grupo
        lblSubCoor.Text = CurrentCliente.Clasif1;
        lblSaldoCta.Text = SaldoPagoAnticipado.ToString(); //CurrentCliente.SaldoCtaCte.ToString();

        if (CurrentCliente.UltimaActualizacion.HasValue)
            lblUltimaAct.Text = string.Format("{0:d}", CurrentCliente.UltimaActualizacion);
        else
            lblUltimaAct.Text = "";

    }


    private void LimpiarPedido()
    {

        lblResponsable.Text = "";
        lblSubCoor.Text = "";
        lblDireccionEntrega.Text = "";
        lblTransporte.Text = "";
        lblTransporteHidden.Value = "";
        lblTransporteValorHidden.Value = "0";
        lblProvinciaPorcentajeDescuentoHidden.Value = "0";

        txtObservacion.Text = "";
        lblFechaImp.Text = "";
        lblCalle.Text = "";
        lblNroNotaImp.Text = "";
        txtMontoGeneral.Text = "0";
        lblMontoActual.Text = "0";
        txtTotalGeneral.Text = "0";
        lblUltimoPedido.Text = "";

        phTotalizadorPedido.Controls.Clear();
        TotalizadorPromos1.Clear();
        UcTotalizadorPedidos.Clear();

        Session.Add("detPedido", new List<DetallePedido>());
        Session.Add("PromosGeneradas", new List<DetallePedido>());
        Session.Add("IdDireccionSeleccionada", 0);
        Session.Add("ProvinciaDireccionSeleccionada", "");


        cboConsultores.SelectedIndex = 0;
        cboFormaPago.SelectedIndex = 2;


        upResumen.Update();
        upEncImp.Update();
        upPromos.Update();
        upTotalizarCabecera.Update();
        upMontoActual.Update();
        upCabeceraPagina.Update();
    }

    private static string ObtenerSeguienteNro(long IdPedido, Marzzan_InfolegacyDataContext Contexto)
    {


        try
        {
            if (IdPedido > 0)
            {
                return (from C in Contexto.CabeceraPedidos
                        where C.IdCabeceraPedido == IdPedido
                        select C.Nro.ToString()).SingleOrDefault();
            }
            else
            {
                var ultimoNro = (from D in Contexto.CabeceraPedidos
                                 where D.TipoPedido == "NP"
                                 select Convert.ToInt32(D.Nro)).Max<int>();
                return Convert.ToString(long.Parse(ultimoNro.ToString()) + 1);
            }

        }
        catch
        {
            return "1";
        }


    }

    private void CalcularPromociones()
    {


        if (Session["Cliente"] != null && TipoCliente.ToString() != Convert.ToString((int)TipoClientes.PotencialBolso))
        {

            Cliente CurrentClient = (Cliente)Session["Cliente"];
            List<DetallePedido> promosGeneradasConRegalo = new List<DetallePedido>();
            List<DetallePedido> promosGeneradasConRegaloVIP = new List<DetallePedido>();
            List<DetallePedido> promosGeneradasSinRegalos = new List<DetallePedido>();
            List<DetallePedido> promosGeneradasUnaxPedido = new List<DetallePedido>();
            List<DetallePedido> AllPromosGeneradas = new List<DetallePedido>();
            List<DetallePedido> PedidoTemp = new List<DetallePedido>();
            List<DetallePedido> PedidoActual = (Session["detPedido"] as List<DetallePedido>).Where(w => w.Tipo != "P").ToList();


            #region Carga de Variables
            Helpers.HelperPromocion helper = new Helpers.HelperPromocion();

            decimal MontoActual = (from P in (Session["detPedido"] as List<DetallePedido>) select P.ValorTotal.Value).Sum(); //decimal.Parse(lblMontoProductos.Text.Replace("$", ""));


            long[] idProductosSolicitados = (from P in (Session["detPedido"] as List<DetallePedido>)
                                             select P.Producto.Value).ToArray<long>();


            List<Producto> promosValidas = (from P in Contexto.Productos
                                            where P.objConfPromocion != null
                                            && (DateTime.Now.Date >= P.objConfPromocion.FechaInicio.Date && DateTime.Now.Date <= P.objConfPromocion.FechaFinal.Date)
                                            && (P.objConfPromocion.ColTransportistas.Count == 0 || P.objConfPromocion.ColTransportistas.Where(t => t.Transporte.ToUpper() == lblTransporteHidden.Value.ToString().ToUpper()).Count() > 0)
                                            select P).Distinct<Producto>().ToList<Producto>();



            List<Producto> promosConRegalos = (from P in promosValidas
                                               join C in Contexto.Composicions on P.IdProducto equals C.ComponentePricipal
                                               where P.objConfPromocion != null
                                               && (DateTime.Now.Date >= P.objConfPromocion.FechaInicio.Date && DateTime.Now.Date <= P.objConfPromocion.FechaFinal.Date)
                                               && (idProductosSolicitados.Contains(C.objProductoHijo.IdProducto))
                                               && (P.ColComposiciones.Where(p => p.TipoComposicion == "O").Count() > 0)
                                                   /// Para la promociones de tipo INICIL
                                               && (P.objConfPromocion.TipoPromo == "INICIAL")
                                               && (P.objConfPromocion.UnaPorPedido.Value == false)
                                               && (P.objConfPromocion.MontoMinimo.Value == 0 || MontoActual >= P.objConfPromocion.MontoMinimo.Value)
                                               && (P.objConfPromocion.ColTransportistas.Count == 0 || P.objConfPromocion.ColTransportistas.Where(t => t.Transporte.ToUpper() == lblTransporteHidden.Value.ToString().ToUpper()).Count() > 0)
                                               select P).Distinct<Producto>().ToList<Producto>();

            promosConRegalos = promosConRegalos.OrderByDescending(c => c.CantidadElementoRequeridos).ToList<Producto>();


            List<Producto> promosVIPConRegalos = (from P in promosValidas
                                                  join C in Contexto.Composicions on P.IdProducto equals C.ComponentePricipal
                                                  where P.objConfPromocion != null
                                                  && (DateTime.Now.Date >= P.objConfPromocion.FechaInicio.Date && DateTime.Now.Date <= P.objConfPromocion.FechaFinal.Date)
                                                      /// Para la promociones de tipo VIP o VIP SENIOR
                                                  && (P.objConfPromocion.TipoPromo == CurrentClient.TipoConsultor.Trim() && P.objConfPromocion.TipoPromo != "INICIAL")
                                                  && (P.ColComposiciones.Where(p => p.TipoComposicion == "O").Count() > 0)
                                                  && (P.objConfPromocion.UnaPorPedido.Value == false)
                                                  && (P.objConfPromocion.MontoMinimo.Value == 0 || MontoActual >= P.objConfPromocion.MontoMinimo.Value)
                                                  && (P.objConfPromocion.ColTransportistas.Count == 0 || P.objConfPromocion.ColTransportistas.Where(t => t.Transporte.ToUpper() == lblTransporteHidden.Value.ToString().ToUpper()).Count() > 0)
                                                  select P).Distinct<Producto>().ToList<Producto>();

            promosVIPConRegalos = promosVIPConRegalos.OrderByDescending(c => c.CantidadElementoRequeridos).ToList<Producto>();


            long[] idPromoConRegalos = (from P in promosConRegalos
                                        select P.IdProducto).ToArray<long>();


            List<Producto> promosSinRegalos = (from P in promosValidas
                                               join C in Contexto.Composicions on P.IdProducto equals C.ComponentePricipal
                                               where P.objConfPromocion != null
                                               && (DateTime.Now.Date >= P.objConfPromocion.FechaInicio.Date && DateTime.Now.Date <= P.objConfPromocion.FechaFinal.Date)
                                               && (idProductosSolicitados.Contains(C.objProductoHijo.IdProducto) || C.objProductoHijo.Tipo == 'P')
                                                   /// Para los tres tipos de promociones
                                               && (P.objConfPromocion.TipoPromo == "INICIAL" || P.objConfPromocion.TipoPromo == CurrentClient.TipoConsultor.Trim())
                                               && (P.ColComposiciones.Where(w => w.TipoComposicion == "O").Count() == 0)
                                               && (P.objConfPromocion.UnaPorPedido.Value == false)
                                               && (P.objConfPromocion.MontoMinimo.Value == 0 || MontoActual >= P.objConfPromocion.MontoMinimo.Value)
                                               && (P.objConfPromocion.ColTransportistas.Count == 0 || P.objConfPromocion.ColTransportistas.Where(t => t.Transporte.ToUpper() == lblTransporteHidden.Value.ToString().ToUpper()).Count() > 0)
                                               select P).Distinct<Producto>().ToList<Producto>();

            promosSinRegalos = promosSinRegalos.OrderByDescending(c => c.CantidadElementoRequeridos).ToList<Producto>();


            List<Producto> promosUnaPorPedido = (from P in promosValidas
                                                 join C in Contexto.Composicions on P.IdProducto equals C.ComponentePricipal
                                                 where P.objConfPromocion != null
                                                 && (DateTime.Now.Date >= P.objConfPromocion.FechaInicio.Date && DateTime.Now.Date <= P.objConfPromocion.FechaFinal.Date)
                                                 && (idProductosSolicitados.Contains(C.objProductoHijo.IdProducto))
                                                 && (P.objConfPromocion.TipoPromo == "INICIAL" || P.objConfPromocion.TipoPromo == CurrentClient.TipoConsultor.Trim())
                                                 && P.objConfPromocion.UnaPorPedido.Value
                                                 && (P.objConfPromocion.MontoMinimo.Value == 0 || MontoActual >= P.objConfPromocion.MontoMinimo.Value)
                                                 && (P.objConfPromocion.ColTransportistas.Count == 0 || P.objConfPromocion.ColTransportistas.Where(t => t.Transporte.ToUpper() == lblTransporteHidden.Value.ToString().ToUpper()).Count() > 0)
                                                 select P).Distinct().ToList<Producto>();

            promosUnaPorPedido = promosUnaPorPedido.OrderByDescending(c => c.CantidadElementoRequeridos).ToList<Producto>();

            #endregion

            if (!EsClienteEspecial)
            {
                #region Gestor de Promociones : Calculo de Promociones
                if (promosConRegalos.Count > 0 || promosSinRegalos.Count > 0 || promosUnaPorPedido.Count > 0 || promosVIPConRegalos.Count > 0)
                {
                    // Generacion de promociones con regalo
                    promosGeneradasConRegalo = helper.GenerarPromociones(PedidoActual, promosConRegalos, true).ToList<DetallePedido>();


                    // Generacion de promociones sin regalo
                    PedidoTemp.AddRange(promosGeneradasConRegalo);
                    PedidoTemp.AddRange(PedidoActual);
                    promosGeneradasSinRegalos = helper.GenerarPromociones(PedidoTemp, promosSinRegalos, false).ToList<DetallePedido>();


                    // Generacion de Promociones VIP
                    PedidoTemp.Clear();
                    PedidoTemp.AddRange(promosGeneradasConRegalo);
                    PedidoTemp.AddRange(PedidoActual);
                    PedidoTemp.AddRange(promosGeneradasSinRegalos);
                    promosGeneradasConRegaloVIP = helper.GenerarPromociones(PedidoTemp, promosVIPConRegalos, true).ToList<DetallePedido>();


                    List<Producto> promosExcluyentes = new List<Producto>();
                    promosExcluyentes.AddRange(promosConRegalos);
                    promosExcluyentes.AddRange(promosSinRegalos);

                    // Generacion de Promociones de tipo una por PEDIDO
                    // Pedido Micaela: 22/01/2015
                    // Se controla la regla de negocio de una por pedido teniendo en cuenta las promociones directas solicitadas,
                    // es decir las promos directas cuentan al momento de controal si se ha pedido o no un promocion que es de 
                    // una por pedido.
                    List<long> idsPromosSolicitadasDirectas = (Session["detPedido"] as List<DetallePedido>).Where(w => w.Tipo == "P" || w.Tipo == "D").Select(w => w.Producto.Value).ToList();
                    promosUnaPorPedido = promosUnaPorPedido.Where(w => !idsPromosSolicitadasDirectas.Contains(w.IdProducto)).ToList();

                    // Realizo la generación de promociones una por pedido
                    promosGeneradasUnaxPedido = helper.GenerarPromocionesUnaxPedido(PedidoTemp, promosUnaPorPedido, promosExcluyentes).ToList<DetallePedido>();

                }

                #endregion

                #region Generación Promociones con Solicitud Directa
                Random randomUsoInterno = new Random();
                List<DetallePedido> promosSolicitadasDirectas = (Session["detPedido"] as List<DetallePedido>).Where(w => w.Tipo == "P" || w.Tipo == "D").ToList();

                foreach (DetallePedido promoDelDetalleDirectas in promosSolicitadasDirectas)
                {
                    bool generarPromociones = false;
                    List<DetallePedido> promosGeneradaExistente = new List<DetallePedido>();
                    if (promoDelDetalleDirectas.IdRelacionDetallePromo != 0)
                    {

                        promosGeneradaExistente = (Session["PromosGeneradas"] as List<DetallePedido>).Where(w => w.IdRelacionDetallePromo == promoDelDetalleDirectas.IdRelacionDetallePromo).ToList();
                        if (promosGeneradaExistente.Count > 0)
                        {
                            if (promosGeneradaExistente.Count <= promoDelDetalleDirectas.Cantidad)
                            {
                                generarPromociones = true;
                                AllPromosGeneradas.AddRange(promosGeneradaExistente);
                            }
                            else
                            {
                                int CantidadValida = Convert.ToInt32(promoDelDetalleDirectas.Cantidad.Value);
                                AllPromosGeneradas.AddRange(promosGeneradaExistente.Take(CantidadValida).ToList());
                            }
                        }
                        else
                        {
                            generarPromociones = true;
                        }

                    }
                    else
                    {
                        generarPromociones = true;
                    }

                    if (generarPromociones)
                    {
                        /// Le asigno un Id Interno para poder relaciones la promo del detalle 
                        /// con la promo generada.
                        if (promoDelDetalleDirectas.IdRelacionDetallePromo == 0)
                        {
                            if (promoDelDetalleDirectas.IdDetallePedido == 0)
                                promoDelDetalleDirectas.IdRelacionDetallePromo = randomUsoInterno.Next(-1000, -1);
                            else
                                promoDelDetalleDirectas.IdRelacionDetallePromo = promoDelDetalleDirectas.IdDetallePedido;
                        }


                        List<string> descripcionPromo = new List<string>();
                        descripcionPromo.Add("|");

                        Producto promoSolicitada = (from P in Contexto.Presentacions
                                                    where P.Codigo.Trim() == promoDelDetalleDirectas.CodigoCompleto
                                                    select P.objProducto).First<Producto>();



                        var composicionRegalo = from R in promoSolicitada.ColComposiciones
                                                where R.TipoComposicion == "O"
                                                group R by R.Grupo into c
                                                select new { Grupo = c.Key, componentes = c };

                        for (int i = 0; i < promoDelDetalleDirectas.Cantidad - promosGeneradaExistente.Count; i++)
                        {

                            DetallePedido promoGeneradaxSolicitud = new DetallePedido();


                            /// Recupero y genero los datos para los elementos de regalo
                            /// de la promoción que se esta solicitando.
                            if (composicionRegalo.Count() > 0)
                            {

                                foreach (var itemComponente in composicionRegalo)
                                {
                                    Presentacion presentacionRegalo = composicionRegalo.First().componentes.First<Composicion>().objPresentacion;

                                    List<Producto> productosRegalo = (from P in itemComponente.componentes
                                                                      select P.objProductoHijo).ToList<Producto>();


                                    DetalleRegalos newRegalo = new DetalleRegalos();

                                    if (itemComponente.componentes.Count() == 1)
                                    {
                                        if (!itemComponente.componentes.First().objProductoHijo.Descripcion.Contains("x Unidad"))
                                            newRegalo.DescripcionRegalo = itemComponente.componentes.First().objProductoHijo.Descripcion + " x " + itemComponente.componentes.First().objPresentacion.Descripcion;
                                        else
                                            newRegalo.DescripcionRegalo = itemComponente.componentes.First().objProductoHijo.Descripcion;

                                        newRegalo.IdPresentacionRegaloSeleccionado = itemComponente.componentes.First().objPresentacion.IdPresentacion;
                                        newRegalo.TipoRegalo = "Producto";

                                    }
                                    else
                                    {
                                        List<Producto> productos = (from P in itemComponente.componentes
                                                                    select P.objProductoHijo).ToList<Producto>();

                                        newRegalo.DescripcionRegalo = Helper.ObtenerDescripcionCompletaProductoEnComun(productos);
                                        newRegalo.IdPresentacionRegaloSeleccionado = 0;
                                        newRegalo.TipoRegalo = "Producto";

                                    }

                                    newRegalo.objDetallePedido = promoGeneradaxSolicitud;
                                    newRegalo.Grupo = itemComponente.Grupo.Value;
                                    promoGeneradaxSolicitud.ColRegalos.Add(newRegalo);



                                    //List<Producto> productos = (from P in itemComponente.componentes
                                    //                            select P.objProductoHijo).ToList<Producto>();

                                    //DetalleRegalos newRegalo = new DetalleRegalos();
                                    //newRegalo.DescripcionRegalo = Helper.ObtenerDescripcionCompletaProductoEnComun(productos) + " x " + itemComponente.componentes.First().objPresentacion.Descripcion;
                                    //newRegalo.IdPresentacionRegaloSeleccionado = 0;
                                    //newRegalo.TipoRegalo = "Producto";
                                    //newRegalo.objDetallePedido = promoGeneradaxSolicitud;
                                    //newRegalo.Grupo = itemComponente.componentes.First().Grupo.Value;
                                    //promoGeneradaxSolicitud.ColRegalos.Add(newRegalo);
                                }

                            }
                            else
                            {
                                DetalleRegalos newRegalo = new DetalleRegalos();
                                newRegalo.DescripcionRegalo = "Un descuento de $" + promoSolicitada.Precio.ToString();
                                newRegalo.IdPresentacionRegaloSeleccionado = -1;
                                newRegalo.TipoRegalo = "Descuento";
                                newRegalo.objDetallePedido = promoGeneradaxSolicitud;
                                newRegalo.Grupo = 0;
                                promoGeneradaxSolicitud.ColRegalos.Add(newRegalo);
                            }


                            /// Recupero y genero los datos para los elementos requeridos
                            /// de la promoción que se esta solicitando.
                            var composicionRequeridos = from R in promoSolicitada.ColComposiciones
                                                        where R.TipoComposicion == "C"
                                                        group R by R.Grupo into c
                                                        select new { Grupo = c.Key, componentes = c };



                            decimal valorPromocionSegunComponentes = 0;
                            bool puedeSeleccionarProductosRequeridos = false;
                            foreach (var item in composicionRequeridos)
                            {
                                if (item.componentes.Count() == 1)
                                {
                                    /// Verifico si en la coleccion de elementos requeridos guardado esta este definicion
                                    DetalleProductosRequeridos detExistente = promoDelDetalleDirectas.colProductosRequeridos.Where(w => w.Tipo == "Fijo"
                                        && w.IdProducto == item.componentes.FirstOrDefault().objProductoHijo.IdProducto
                                        && w.IdPresentacion == item.componentes.FirstOrDefault().objPresentacion.IdPresentacion
                                        && w.Grupo == i).FirstOrDefault();

                                    if (detExistente == null || promosGeneradaExistente.Count > 0)
                                    {
                                        DetalleProductosRequeridos det = new DetalleProductosRequeridos();
                                        det.Cantidad = int.Parse(item.componentes.FirstOrDefault().Cantidad);
                                        det.Tipo = "Fijo";
                                        det.objDetallePedido = promoGeneradaxSolicitud;
                                        det.IdProducto = item.componentes.FirstOrDefault().objProductoHijo.IdProducto;
                                        det.IdPresentacion = item.componentes.FirstOrDefault().objPresentacion.IdPresentacion;
                                        det.ValorUnitario = item.componentes.FirstOrDefault().objPresentacion.Precio.Value;
                                        det.CodigoCompleto = item.componentes.FirstOrDefault().objPresentacion.Codigo;
                                        det.DescripcionProducto = "<b><span style='color:Blue' >" + item.componentes.FirstOrDefault().Cantidad + "</span></b> " + item.componentes.FirstOrDefault().objProductoHijo.Descripcion + " x " + item.componentes.FirstOrDefault().objPresentacion.Descripcion;
                                        promoGeneradaxSolicitud.colProductosRequeridos.Add(det);
                                        valorPromocionSegunComponentes += item.componentes.FirstOrDefault().objPresentacion.Precio.Value * int.Parse(item.componentes.FirstOrDefault().Cantidad);
                                    }
                                    else
                                    {
                                        DetalleProductosRequeridos det = new DetalleProductosRequeridos();
                                        det.Cantidad = int.Parse(item.componentes.FirstOrDefault().Cantidad);
                                        det.Tipo = "Fijo";
                                        det.objDetallePedido = promoGeneradaxSolicitud;
                                        det.IdProducto = detExistente.IdProducto;
                                        det.IdPresentacion = detExistente.IdPresentacion;
                                        det.ValorUnitario = detExistente.ValorUnitario.Value;
                                        det.CodigoCompleto = detExistente.CodigoCompleto;
                                        det.DescripcionProducto = detExistente.DescripcionProducto;
                                        promoGeneradaxSolicitud.colProductosRequeridos.Add(det);
                                        valorPromocionSegunComponentes += detExistente.ValorUnitario.Value * int.Parse(item.componentes.FirstOrDefault().Cantidad);

                                    }
                                }
                                else
                                {
                                    puedeSeleccionarProductosRequeridos = true;
                                    valorPromocionSegunComponentes += item.componentes.FirstOrDefault().objPresentacion.Precio.Value * int.Parse(item.componentes.FirstOrDefault().Cantidad);
                                }
                            }


                            /// Solo si hay promociones generadas y no se esta creando una nueva
                            /// busco los productos requeridos que estan guardados.
                            if (promoDelDetalleDirectas.Cantidad > 0 && promosGeneradaExistente.Count == 0)
                            {
                                /// Genero todos los productos requeridos que fueron guardado
                                List<DetalleProductosRequeridos> detalles = promoDelDetalleDirectas.colProductosRequeridos.Where(w => w.Tipo == "Dinamico" && w.Grupo == i).ToList();
                                foreach (DetalleProductosRequeridos detalle in detalles)
                                {
                                    DetalleProductosRequeridos det = new DetalleProductosRequeridos();
                                    det.Cantidad = detalle.Cantidad;
                                    det.Tipo = "Dinamico";
                                    det.objDetallePedido = promoGeneradaxSolicitud;
                                    det.IdProducto = detalle.IdProducto;
                                    det.IdPresentacion = detalle.IdPresentacion;
                                    det.ValorUnitario = detalle.ValorUnitario.Value;
                                    det.CodigoCompleto = detalle.CodigoCompleto;
                                    det.DescripcionProducto = detalle.DescripcionProducto;
                                    promoGeneradaxSolicitud.colProductosRequeridos.Add(det);
                                }
                            }



                            if (puedeSeleccionarProductosRequeridos)
                            {
                                promoGeneradaxSolicitud.Tipo = "PS";
                            }
                            else
                                promoGeneradaxSolicitud.Tipo = "PSD";


                            promoGeneradaxSolicitud.IdDetallePedido = randomUsoInterno.Next(-1000, -1);
                            promoGeneradaxSolicitud.IdRelacionDetallePromo = promoDelDetalleDirectas.IdRelacionDetallePromo;
                            promoGeneradaxSolicitud.Cantidad = 1;
                            promoGeneradaxSolicitud.Producto = promoSolicitada.IdProducto;
                            promoGeneradaxSolicitud.Presentacion = promoSolicitada.ColPresentaciones[0].IdPresentacion;
                            promoGeneradaxSolicitud.IdRegaloSeleccionado = -1;
                            promoGeneradaxSolicitud.ProductoDesc = "Promociones Solicitadas";
                            promoGeneradaxSolicitud.PresentacionDesc = promoSolicitada.ColPresentaciones[0].Descripcion;

                            promoGeneradaxSolicitud.DescripcionCompleta = promoGeneradaxSolicitud.ProductoDesc;
                            promoGeneradaxSolicitud.DescProductosUtilizados = descripcionPromo;
                            promoGeneradaxSolicitud.CodigoCompleto = promoSolicitada.ColPresentaciones[0].Codigo;
                            promoGeneradaxSolicitud.ValorUnitario = promoSolicitada.Precio;
                            promoGeneradaxSolicitud.ValorTotal = promoSolicitada.Precio;

                            promoDelDetalleDirectas.ValorUnitario = valorPromocionSegunComponentes;
                            promoDelDetalleDirectas.ValorTotal = promoDelDetalleDirectas.ValorUnitario * promoDelDetalleDirectas.Cantidad;

                            AllPromosGeneradas.Add(promoGeneradaxSolicitud);
                        }
                    }
                    //}
                }

                #endregion

                #region  Gestor de Prediccion : Generacion de Promociones Posibles
                PedidoTemp.Clear();
                PedidoTemp.AddRange(promosGeneradasConRegalo);
                PedidoTemp.AddRange(PedidoActual);
                PedidoTemp.AddRange(promosGeneradasSinRegalos);
                Hashtable promosPosiblesConRegalos = helper.GenerarPromocionesPosibles(PedidoTemp, promosConRegalos);
                Hashtable promosPosiblesSinRegalos = helper.GenerarPromocionesPosibles(PedidoTemp, promosSinRegalos);

                Hashtable promosPosibles = new Hashtable();
                foreach (object item in promosPosiblesConRegalos.Keys)
                {
                    promosPosibles.Add(item, promosPosiblesConRegalos[item]);
                }

                foreach (object item in promosPosiblesSinRegalos.Keys)
                {
                    promosPosibles.Add(item, promosPosiblesSinRegalos[item]);
                }
                #endregion

                #region Generacion Promociones FIJAS

                #region Promociones por monto
                /// Genero la promocion Fija que se entraga por pedido
                if (decimal.Parse(txtMontoGeneral.Text.Replace("$", "")) <= 350)
                {
                    // Promo Pedido Hasta $350
                    string codigoPedidoMenor = "1150000021427";

                    Producto promoPedidoMenor = (from P in Contexto.Presentacions
                                                 where P.Codigo.Trim() == codigoPedidoMenor
                                                 select P.objProducto).First<Producto>();

                    if (promoPedidoMenor.objConfPromocion != null && promoPedidoMenor.objConfPromocion.FechaInicio <= DateTime.Now && promoPedidoMenor.objConfPromocion.FechaFinal > DateTime.Now)
                    {

                        List<string> descripcionPromo = new List<string>();
                        descripcionPromo.Add("Hasta|$350");

                        DetallePedido pedidoPedidoMenor = new DetallePedido();


                        var composicionRegalo = from R in promoPedidoMenor.ColComposiciones
                                                where R.TipoComposicion == "O"
                                                group R by R.Grupo into c
                                                select new { Grupo = c.Key, componentes = c };

                        if (composicionRegalo.Count() > 0)
                        {

                            // ESTE CODIGO ES EL QUE SE DEBE PONER
                            // PARA SOPORTAR MAS DE UN GRUPO EN LAS PROMOCION DE 
                            // 'PAGO FACIL', HACER LO MISMO PARA 'PAGO MIS CUENTAS'
                            foreach (var itemComponente in composicionRegalo)
                            {
                                List<Producto> productos = (from P in itemComponente.componentes
                                                            select P.objProductoHijo).ToList<Producto>();

                                DetalleRegalos newRegalo = new DetalleRegalos();
                                newRegalo.DescripcionRegalo = Helper.ObtenerDescripcionCompletaProductoEnComun(productos) + " x " + itemComponente.componentes.First().objPresentacion.Descripcion;
                                newRegalo.IdPresentacionRegaloSeleccionado = 0;
                                newRegalo.TipoRegalo = "Producto";
                                newRegalo.objDetallePedido = pedidoPedidoMenor;
                                newRegalo.Grupo = itemComponente.componentes.First().Grupo.Value;
                                pedidoPedidoMenor.ColRegalos.Add(newRegalo);
                            }


                            pedidoPedidoMenor.Cantidad = 1;
                            pedidoPedidoMenor.Producto = promoPedidoMenor.IdProducto;
                            pedidoPedidoMenor.Presentacion = promoPedidoMenor.ColPresentaciones[0].IdPresentacion;
                            pedidoPedidoMenor.ProductoDesc = promoPedidoMenor.Descripcion;
                            pedidoPedidoMenor.PresentacionDesc = promoPedidoMenor.ColPresentaciones[0].Descripcion;
                            pedidoPedidoMenor.DescripcionCompleta = pedidoPedidoMenor.ProductoDesc;
                            pedidoPedidoMenor.DescProductosUtilizados = descripcionPromo;
                            pedidoPedidoMenor.CodigoCompleto = promoPedidoMenor.ColPresentaciones[0].Codigo;
                            pedidoPedidoMenor.Tipo = "M";

                            AllPromosGeneradas.Add(pedidoPedidoMenor);
                        }

                    }
                }
                else if (decimal.Parse(txtMontoGeneral.Text.Replace("$", "")) >= 990)
                {
                    // Promo Pedido Hasta $900
                    string codigoPromoPedidoMayor = "1150000122010"; // Beneficio Primavera- 212 Men x 500 ml

                    Producto promoPedidoMayor = (from P in Contexto.Presentacions
                                                 where P.Codigo.Trim() == codigoPromoPedidoMayor
                                                 select P.objProducto).FirstOrDefault<Producto>();

                    if (promoPedidoMayor != null && promoPedidoMayor.objConfPromocion != null && promoPedidoMayor.objConfPromocion.FechaInicio <= DateTime.Now && promoPedidoMayor.objConfPromocion.FechaFinal > DateTime.Now)
                    {

                        List<string> descripcionPromo = new List<string>();
                        descripcionPromo.Add("Mas de|$990");

                        DetallePedido pedidoPedidoMayor = new DetallePedido();


                        var composicionRegalo = from R in promoPedidoMayor.ColComposiciones
                                                where R.TipoComposicion == "O"
                                                group R by R.Grupo into c
                                                select new { Grupo = c.Key, componentes = c };

                        if (composicionRegalo.Count() > 0)
                        {
                            foreach (var itemComponente in composicionRegalo)
                            {
                                List<Producto> productos = (from P in itemComponente.componentes
                                                            select P.objProductoHijo).ToList<Producto>();

                                DetalleRegalos newRegalo = new DetalleRegalos();
                                newRegalo.DescripcionRegalo = productos.First().Descripcion + " x " + itemComponente.componentes.First().objPresentacion.Descripcion;
                                newRegalo.IdPresentacionRegaloSeleccionado = itemComponente.componentes.First().objPresentacion.IdPresentacion;
                                newRegalo.TipoRegalo = "Producto";
                                newRegalo.objDetallePedido = pedidoPedidoMayor;
                                newRegalo.Grupo = itemComponente.componentes.First().Grupo.Value;
                                pedidoPedidoMayor.ColRegalos.Add(newRegalo);
                            }


                            pedidoPedidoMayor.Cantidad = 1;
                            pedidoPedidoMayor.Producto = promoPedidoMayor.IdProducto;
                            pedidoPedidoMayor.Presentacion = promoPedidoMayor.ColPresentaciones[0].IdPresentacion;
                            pedidoPedidoMayor.ProductoDesc = promoPedidoMayor.Descripcion;
                            pedidoPedidoMayor.PresentacionDesc = promoPedidoMayor.ColPresentaciones[0].Descripcion;
                            pedidoPedidoMayor.DescripcionCompleta = pedidoPedidoMayor.ProductoDesc;
                            pedidoPedidoMayor.DescProductosUtilizados = descripcionPromo;
                            pedidoPedidoMayor.CodigoCompleto = promoPedidoMayor.ColPresentaciones[0].Codigo;
                            pedidoPedidoMayor.Tipo = "M";

                            AllPromosGeneradas.Add(pedidoPedidoMayor);
                        }

                    }
                }
                /// Promociones fijas por monto y para productos de tipo nivel.
                if (decimal.Parse(txtMontoGeneral.Text.Replace("$", "")) >= 850 && decimal.Parse(txtMontoGeneral.Text.Replace("$", "")) <= decimal.Parse("1099,99")) //$850 a $1099.99
                {
                    // Promo Pedido Hasta $900
                    string codigoPromoNivel = "1150000041001";    //Nivel 1 Premiamos tu Crecimiento Noviembre 15        

                    Producto promoPedidoNivel= (from P in Contexto.Presentacions
                                                 where P.Codigo.Trim() == codigoPromoNivel
                                                 select P.objProducto).FirstOrDefault<Producto>();

                    if (promoPedidoNivel != null && promoPedidoNivel.objConfPromocion != null && promoPedidoNivel.objConfPromocion.FechaInicio <= DateTime.Now && promoPedidoNivel.objConfPromocion.FechaFinal > DateTime.Now)
                    {

                        List<string> descripcionPromo = new List<string>();
                        descripcionPromo.Add("Desde $850 Hasta|$1099");

                        DetallePedido pedidoPromoNivel = new DetallePedido();


                        var composicionRegalo = from R in promoPedidoNivel.ColComposiciones
                                                where R.TipoComposicion == "O"
                                                group R by R.Grupo into c
                                                select new { Grupo = c.Key, componentes = c };

                        if (composicionRegalo.Count() > 0)
                        {
                            foreach (var itemComponente in composicionRegalo)
                            {
                                List<Producto> productos = (from P in itemComponente.componentes
                                                            select P.objProductoHijo).ToList<Producto>();

                                DetalleRegalos newRegalo = new DetalleRegalos();
                                newRegalo.DescripcionRegalo = productos.First().Descripcion + " x " + itemComponente.componentes.First().objPresentacion.Descripcion;
                                newRegalo.IdPresentacionRegaloSeleccionado = itemComponente.componentes.First().objPresentacion.IdPresentacion;
                                newRegalo.TipoRegalo = "Producto";
                                newRegalo.objDetallePedido = pedidoPromoNivel;
                                newRegalo.Grupo = itemComponente.componentes.First().Grupo.Value;
                                newRegalo.Cantidad = int.Parse(itemComponente.componentes.First().Cantidad);
                                pedidoPromoNivel.ColRegalos.Add(newRegalo);
                            }


                            pedidoPromoNivel.Cantidad = 1;
                            pedidoPromoNivel.Producto = promoPedidoNivel.IdProducto;
                            pedidoPromoNivel.Presentacion = promoPedidoNivel.ColPresentaciones[0].IdPresentacion;
                            pedidoPromoNivel.ProductoDesc = promoPedidoNivel.Descripcion;
                            pedidoPromoNivel.PresentacionDesc = promoPedidoNivel.ColPresentaciones[0].Descripcion;
                            pedidoPromoNivel.DescripcionCompleta = pedidoPromoNivel.ProductoDesc;
                            pedidoPromoNivel.DescProductosUtilizados = descripcionPromo;
                            pedidoPromoNivel.CodigoCompleto = promoPedidoNivel.ColPresentaciones[0].Codigo;
                            pedidoPromoNivel.Tipo = "M";

                            AllPromosGeneradas.Add(pedidoPromoNivel);
                        }

                    }
                }
                else if (decimal.Parse(txtMontoGeneral.Text.Replace("$", "")) >= 1010 && decimal.Parse(txtMontoGeneral.Text.Replace("$", "")) <= decimal.Parse("1999,99"))
                {
                    // Promo Pedido Hasta $900
                    string codigoPromoNivel = "1150000041002";  //Nivel 2 Premiamos tu Crecimiento Noviembre 15        

                    Producto promoPedidoNivel = (from P in Contexto.Presentacions
                                                 where P.Codigo.Trim() == codigoPromoNivel
                                                 select P.objProducto).FirstOrDefault<Producto>();

                    if (promoPedidoNivel != null && promoPedidoNivel.objConfPromocion != null && promoPedidoNivel.objConfPromocion.FechaInicio <= DateTime.Now && promoPedidoNivel.objConfPromocion.FechaFinal > DateTime.Now)
                    {

                        List<string> descripcionPromo = new List<string>();
                        descripcionPromo.Add("Desde $1100 Hasta|$1999");

                        DetallePedido pedidoPromoNivel = new DetallePedido();


                        var composicionRegalo = from R in promoPedidoNivel.ColComposiciones
                                                where R.TipoComposicion == "O"
                                                group R by R.Grupo into c
                                                select new { Grupo = c.Key, componentes = c };

                        if (composicionRegalo.Count() > 0)
                        {
                            foreach (var itemComponente in composicionRegalo)
                            {
                                List<Producto> productos = (from P in itemComponente.componentes
                                                            select P.objProductoHijo).ToList<Producto>();

                                DetalleRegalos newRegalo = new DetalleRegalos();
                                newRegalo.DescripcionRegalo = productos.First().Descripcion + " x " + itemComponente.componentes.First().objPresentacion.Descripcion;
                                newRegalo.IdPresentacionRegaloSeleccionado = itemComponente.componentes.First().objPresentacion.IdPresentacion;
                                newRegalo.TipoRegalo = "Producto";
                                newRegalo.objDetallePedido = pedidoPromoNivel;
                                newRegalo.Grupo = itemComponente.componentes.First().Grupo.Value;
                                newRegalo.Cantidad = int.Parse(itemComponente.componentes.First().Cantidad);
                                pedidoPromoNivel.ColRegalos.Add(newRegalo);
                            }


                            pedidoPromoNivel.Cantidad = 1;
                            pedidoPromoNivel.Producto = promoPedidoNivel.IdProducto;
                            pedidoPromoNivel.Presentacion = promoPedidoNivel.ColPresentaciones[0].IdPresentacion;
                            pedidoPromoNivel.ProductoDesc = promoPedidoNivel.Descripcion;
                            pedidoPromoNivel.PresentacionDesc = promoPedidoNivel.ColPresentaciones[0].Descripcion;
                            pedidoPromoNivel.DescripcionCompleta = pedidoPromoNivel.ProductoDesc;
                            pedidoPromoNivel.DescProductosUtilizados = descripcionPromo;
                            pedidoPromoNivel.CodigoCompleto = promoPedidoNivel.ColPresentaciones[0].Codigo;
                            pedidoPromoNivel.Tipo = "M";

                            AllPromosGeneradas.Add(pedidoPromoNivel);
                        }

                    }
                }
                else if (decimal.Parse(txtMontoGeneral.Text.Replace("$", "")) >= 2000 && decimal.Parse(txtMontoGeneral.Text.Replace("$", "")) <= decimal.Parse("2999,99"))
                {
                    string codigoPromoNivel = "1150000041003"; //Nivel 3 Premiamos tu Crecimiento Noviembre 15        

                    Producto promoPedidoNivel = (from P in Contexto.Presentacions
                                                 where P.Codigo.Trim() == codigoPromoNivel
                                                 select P.objProducto).FirstOrDefault<Producto>();

                    if (promoPedidoNivel != null && promoPedidoNivel.objConfPromocion != null && promoPedidoNivel.objConfPromocion.FechaInicio <= DateTime.Now && promoPedidoNivel.objConfPromocion.FechaFinal > DateTime.Now)
                    {

                        List<string> descripcionPromo = new List<string>();
                        descripcionPromo.Add("Desde $2000 Hasta|$2999");

                        DetallePedido pedidoPromoNivel = new DetallePedido();


                        var composicionRegalo = from R in promoPedidoNivel.ColComposiciones
                                                where R.TipoComposicion == "O"
                                                group R by R.Grupo into c
                                                select new { Grupo = c.Key, componentes = c };

                        if (composicionRegalo.Count() > 0)
                        {
                            foreach (var itemComponente in composicionRegalo)
                            {
                                List<Producto> productos = (from P in itemComponente.componentes
                                                            select P.objProductoHijo).ToList<Producto>();

                                DetalleRegalos newRegalo = new DetalleRegalos();
                                newRegalo.DescripcionRegalo = productos.First().Descripcion + " x " + itemComponente.componentes.First().objPresentacion.Descripcion;
                                newRegalo.IdPresentacionRegaloSeleccionado = itemComponente.componentes.First().objPresentacion.IdPresentacion;
                                newRegalo.TipoRegalo = "Producto";
                                newRegalo.objDetallePedido = pedidoPromoNivel;
                                newRegalo.Grupo = itemComponente.componentes.First().Grupo.Value;
                                newRegalo.Cantidad = int.Parse(itemComponente.componentes.First().Cantidad);
                                pedidoPromoNivel.ColRegalos.Add(newRegalo);
                            }


                            pedidoPromoNivel.Cantidad = 1;
                            pedidoPromoNivel.Producto = promoPedidoNivel.IdProducto;
                            pedidoPromoNivel.Presentacion = promoPedidoNivel.ColPresentaciones[0].IdPresentacion;
                            pedidoPromoNivel.ProductoDesc = promoPedidoNivel.Descripcion;
                            pedidoPromoNivel.PresentacionDesc = promoPedidoNivel.ColPresentaciones[0].Descripcion;
                            pedidoPromoNivel.DescripcionCompleta = pedidoPromoNivel.ProductoDesc;
                            pedidoPromoNivel.DescProductosUtilizados = descripcionPromo;
                            pedidoPromoNivel.CodigoCompleto = promoPedidoNivel.ColPresentaciones[0].Codigo;
                            pedidoPromoNivel.Tipo = "M";

                            AllPromosGeneradas.Add(pedidoPromoNivel);
                        }

                    }
                }
                else if (decimal.Parse(txtMontoGeneral.Text.Replace("$", "")) >= 3000 )
                {
                    string codigoPromoNivel = "1150000041004";  //Nivel 4 Premiamos tu Crecimiento Noviembre 15        

                    Producto promoPedidoNivel = (from P in Contexto.Presentacions
                                                 where P.Codigo.Trim() == codigoPromoNivel
                                                 select P.objProducto).FirstOrDefault<Producto>();

                    if (promoPedidoNivel != null && promoPedidoNivel.objConfPromocion != null && promoPedidoNivel.objConfPromocion.FechaInicio <= DateTime.Now && promoPedidoNivel.objConfPromocion.FechaFinal > DateTime.Now)
                    {

                        List<string> descripcionPromo = new List<string>();
                        descripcionPromo.Add("Mayor a |$3000");

                        DetallePedido pedidoPromoNivel = new DetallePedido();


                        var composicionRegalo = from R in promoPedidoNivel.ColComposiciones
                                                where R.TipoComposicion == "O"
                                                group R by R.Grupo into c
                                                select new { Grupo = c.Key, componentes = c };

                        if (composicionRegalo.Count() > 0)
                        {
                            foreach (var itemComponente in composicionRegalo)
                            {
                                List<Producto> productos = (from P in itemComponente.componentes
                                                            select P.objProductoHijo).ToList<Producto>();

                                DetalleRegalos newRegalo = new DetalleRegalos();
                                newRegalo.DescripcionRegalo = productos.First().Descripcion + " x " + itemComponente.componentes.First().objPresentacion.Descripcion;
                                newRegalo.IdPresentacionRegaloSeleccionado = itemComponente.componentes.First().objPresentacion.IdPresentacion;
                                newRegalo.TipoRegalo = "Producto";
                                newRegalo.objDetallePedido = pedidoPromoNivel;
                                newRegalo.Grupo = itemComponente.componentes.First().Grupo.Value;
                                newRegalo.Cantidad = int.Parse(itemComponente.componentes.First().Cantidad);
                                pedidoPromoNivel.ColRegalos.Add(newRegalo);
                            }


                            pedidoPromoNivel.Cantidad = 1;
                            pedidoPromoNivel.Producto = promoPedidoNivel.IdProducto;
                            pedidoPromoNivel.Presentacion = promoPedidoNivel.ColPresentaciones[0].IdPresentacion;
                            pedidoPromoNivel.ProductoDesc = promoPedidoNivel.Descripcion;
                            pedidoPromoNivel.PresentacionDesc = promoPedidoNivel.ColPresentaciones[0].Descripcion;
                            pedidoPromoNivel.DescripcionCompleta = pedidoPromoNivel.ProductoDesc;
                            pedidoPromoNivel.DescProductosUtilizados = descripcionPromo;
                            pedidoPromoNivel.CodigoCompleto = promoPedidoNivel.ColPresentaciones[0].Codigo;
                            pedidoPromoNivel.Tipo = "M";

                            AllPromosGeneradas.Add(pedidoPromoNivel);
                        }

                    }
                }

                #endregion

                #region Promociones por Forma de Pago hasta el 12/01/2015

                if (DateTime.Now.Date < DateTime.Parse("12/01/2017"))
                {
                    /// 30/06/2015: Si ya posee una promoción de pago anticipado durente el mes actual
                    /// entonces no se debe entregar mas promociones de este tipo y ademas se cambiaron por las nuevas
                    /// promociones de pago por adelantado.
                    if (!PoseePromoPedidoPagoAnticipado((Session["Cliente"] as Cliente).IdCliente))
                    {
                        if (cboFormaPago.Text.Contains("Pago Fácil") || cboFormaPago.Text.Contains("Pago Mis Cuentas") || cboFormaPago.Text.Contains("Rapi Pago"))
                        {
                            string codigoPromoPagoAdelantado = "";
                            if (decimal.Parse(txtMontoGeneral.Text.Replace("$", "")) >= Convert.ToDecimal("850") && decimal.Parse(txtMontoGeneral.Text.Replace("$", "")) <= Convert.ToDecimal("1999"))
                                 codigoPromoPagoAdelantado = "1150000021133";
                            else if (decimal.Parse(txtMontoGeneral.Text.Replace("$", "")) >= Convert.ToDecimal("2000"))
                            {
                                codigoPromoPagoAdelantado = "1150000021135";
                               
                            }

                            Producto promoPagoAdelantado = (from P in Contexto.Presentacions
                                                            where P.Codigo.Trim() == codigoPromoPagoAdelantado
                                                            select P.objProducto).FirstOrDefault<Producto>();


                            if (promoPagoAdelantado != null)
                            {
                                if (promoPagoAdelantado.objConfPromocion != null && promoPagoAdelantado.objConfPromocion.FechaInicio <= DateTime.Now && promoPagoAdelantado.objConfPromocion.FechaFinal > DateTime.Now
                                   && TotalCompradoParaPromociones() >= promoPagoAdelantado.objConfPromocion.MontoMinimo.Value
                                   && (promoPagoAdelantado.objConfPromocion.ColTransportistas.Count == 0 || promoPagoAdelantado.objConfPromocion.ColTransportistas.Any(w => w.Transporte.ToUpper() == lblTransporte.Text.ToUpper())))
                                {

                                    List<string> descripcionPromo = new List<string>();
                                    string descripcionPromoPagoAdelantado = cboFormaPago.Text.Contains("Pago Fácil") ? "Pago|Fácil" : cboFormaPago.Text.Contains("Pago Mis Cuentas") ? "Pago| Mis Cuentas" : "Rapi|Pago";
                                    descripcionPromo.Add(descripcionPromoPagoAdelantado);

                                    DetallePedido detallePagoAdelantado = new DetallePedido();


                                    var composicionRegalo = from R in promoPagoAdelantado.ColComposiciones
                                                            where R.TipoComposicion == "O"
                                                            group R by R.Grupo into c
                                                            select new { Grupo = c.Key, componentes = c };

                                    if (composicionRegalo.Count() > 0)
                                    {
                                        foreach (var itemComponente in composicionRegalo)
                                        {
                                            List<Producto> productos = (from P in itemComponente.componentes
                                                                        select P.objProductoHijo).ToList<Producto>();

                                            DetalleRegalos newRegalo = new DetalleRegalos();
                                            newRegalo.DescripcionRegalo = Helper.ObtenerDescripcionCompletaProductoEnComun(productos) + " x " + itemComponente.componentes.First().objPresentacion.Descripcion;
                                            newRegalo.IdPresentacionRegaloSeleccionado = 0;
                                            newRegalo.TipoRegalo = "Producto";
                                            newRegalo.objDetallePedido = detallePagoAdelantado;
                                            newRegalo.Grupo = itemComponente.componentes.First().Grupo.Value;
                                            detallePagoAdelantado.ColRegalos.Add(newRegalo);
                                        }

                                        detallePagoAdelantado.Cantidad = 1;
                                        detallePagoAdelantado.Producto = promoPagoAdelantado.IdProducto;
                                        detallePagoAdelantado.Presentacion = promoPagoAdelantado.ColPresentaciones[0].IdPresentacion;
                                        detallePagoAdelantado.ProductoDesc = promoPagoAdelantado.Descripcion;
                                        detallePagoAdelantado.PresentacionDesc = promoPagoAdelantado.ColPresentaciones[0].Descripcion;
                                        detallePagoAdelantado.DescripcionCompleta = detallePagoAdelantado.ProductoDesc;
                                        detallePagoAdelantado.DescProductosUtilizados = descripcionPromo;
                                        detallePagoAdelantado.CodigoCompleto = promoPagoAdelantado.ColPresentaciones[0].Codigo;
                                        detallePagoAdelantado.Tipo = "E";

                                        AllPromosGeneradas.Add(detallePagoAdelantado);
                                    }
                                }
                            }
                        }
                    //{

                    //    if (cboFormaPago.Text.Contains("Pago Fácil") || cboFormaPago.Text.Contains("Pago Mis Cuentas") || cboFormaPago.T
                       
                    ///// 02/05/2013: Si ya posee una promoción de pago anticipado durente el mes actual
                    ///// entonces no se debe entregar mas promociones de este tipo.
                    //if (!PoseePromoPedidoPagoAnticipado((Session["Cliente"] as Cliente).IdCliente))ext.Contains("Rapi Pago"))
                    //    {
                    //        string codigoPromoPagoAdelantado = "1150000021052";
                    //        if (decimal.Parse(txtMontoGeneral.Text.Replace("$", "")) > Convert.ToDecimal("1814,99"))
                    //        {
                    //            codigoPromoPagoAdelantado = "1150000021073";
                    //        }

                    //        Producto promoPagoAdelantado = (from P in Contexto.Presentacions
                    //                                        where P.Codigo.Trim() == codigoPromoPagoAdelantado
                    //                                        select P.objProducto).First<Producto>();



                    //        if (promoPagoAdelantado.objConfPromocion != null && promoPagoAdelantado.objConfPromocion.FechaInicio <= DateTime.Now && promoPagoAdelantado.objConfPromocion.FechaFinal > DateTime.Now
                    //           && TotalCompradoParaPromociones() >= promoPagoAdelantado.objConfPromocion.MontoMinimo.Value
                    //           && (promoPagoAdelantado.objConfPromocion.ColTransportistas.Count == 0 || promoPagoAdelantado.objConfPromocion.ColTransportistas.Any(w => w.Transporte.ToUpper() == lblTransporte.Text.ToUpper())))
                    //        {
                    //            List<string> descripcionPromo = new List<string>();
                    //            string descripcionPromoPagoAdelantado = cboFormaPago.Text.Contains("Pago Fácil") ? "Pago|Fácil" : cboFormaPago.Text.Contains("Pago Mis Cuentas") ? "Pago| Mis Cuentas" : "Rapi|Pago";
                    //            descripcionPromo.Add(descripcionPromoPagoAdelantado);

                    //            DetallePedido detallePagoAdelantado = new DetallePedido();


                    //            var composicionRegalo = from R in promoPagoAdelantado.ColComposiciones
                    //                                    where R.TipoComposicion == "O"
                    //                                    group R by R.Grupo into c
                    //                                    select new { Grupo = c.Key, componentes = c };

                    //            if (composicionRegalo.Count() > 0)
                    //            {
                    //                foreach (var itemComponente in composicionRegalo)
                    //                {
                    //                    List<Producto> productos = (from P in itemComponente.componentes
                    //                                                select P.objProductoHijo).ToList<Producto>();

                    //                    DetalleRegalos newRegalo = new DetalleRegalos();
                    //                    newRegalo.DescripcionRegalo = Helper.ObtenerDescripcionCompletaProductoEnComun(productos) + " x " + itemComponente.componentes.First().objPresentacion.Descripcion;
                    //                    newRegalo.IdPresentacionRegaloSeleccionado = 0;
                    //                    newRegalo.TipoRegalo = "Producto";
                    //                    newRegalo.objDetallePedido = detallePagoAdelantado;
                    //                    newRegalo.Grupo = itemComponente.componentes.First().Grupo.Value;
                    //                    detallePagoAdelantado.ColRegalos.Add(newRegalo);
                    //                }

                    //                detallePagoAdelantado.Cantidad = 1;
                    //                detallePagoAdelantado.Producto = promoPagoAdelantado.IdProducto;
                    //                detallePagoAdelantado.Presentacion = promoPagoAdelantado.ColPresentaciones[0].IdPresentacion;
                    //                detallePagoAdelantado.ProductoDesc = promoPagoAdelantado.Descripcion;
                    //                detallePagoAdelantado.PresentacionDesc = promoPagoAdelantado.ColPresentaciones[0].Descripcion;
                    //                detallePagoAdelantado.DescripcionCompleta = detallePagoAdelantado.ProductoDesc;
                    //                detallePagoAdelantado.DescProductosUtilizados = descripcionPromo;
                    //                detallePagoAdelantado.CodigoCompleto = promoPagoAdelantado.ColPresentaciones[0].Codigo;
                    //                detallePagoAdelantado.Tipo = "E";

                    //                AllPromosGeneradas.Add(detallePagoAdelantado);
                    //            }
                    //        }
                    //    }



                        if (cboFormaPago.Text.Contains("Pago Fácil"))
                        {
                            //string codigoPromoPagoFacil = "1150000021052";
                            //if (decimal.Parse(txtMontoGeneral.Text.Replace("$", "")) > Convert.ToDecimal("1814,99"))
                            //{
                            //    codigoPromoPagoFacil = "1150000021506";
                            //}

                            //Producto promoPagoFacil = (from P in Contexto.Presentacions
                            //                           where P.Codigo.Trim() == codigoPromoPagoFacil
                            //                           select P.objProducto).First<Producto>();



                            //if (promoPagoFacil.objConfPromocion != null && promoPagoFacil.objConfPromocion.FechaInicio <= DateTime.Now && promoPagoFacil.objConfPromocion.FechaFinal > DateTime.Now
                            //   && TotalCompradoParaPromociones() >= promoPagoFacil.objConfPromocion.MontoMinimo.Value
                            //   && (promoPagoFacil.objConfPromocion.ColTransportistas.Count == 0 || promoPagoFacil.objConfPromocion.ColTransportistas.Any(w => w.Transporte.ToUpper() == lblTransporte.Text.ToUpper())))
                            //{
                            //    List<string> descripcionPromo = new List<string>();
                            //    descripcionPromo.Add("Pago|Fácil");

                            //    DetallePedido pedidoPagoFacil = new DetallePedido();


                            //    var composicionRegalo = from R in promoPagoFacil.ColComposiciones
                            //                            where R.TipoComposicion == "O"
                            //                            group R by R.Grupo into c
                            //                            select new { Grupo = c.Key, componentes = c };

                            //    if (composicionRegalo.Count() > 0)
                            //    {

                            //        // ESTE CODIGO ES EL QUE SE DEBE PONER
                            //        // PARA SOPORTAR MAS DE UN GRUPO EN LAS PROMOCION DE 
                            //        // 'PAGO FACIL', HACER LO MISMO PARA 'PAGO MIS CUENTAS'
                            //        foreach (var itemComponente in composicionRegalo)
                            //        {
                            //            List<Producto> productos = (from P in itemComponente.componentes
                            //                                        select P.objProductoHijo).ToList<Producto>();

                            //            DetalleRegalos newRegalo = new DetalleRegalos();
                            //            newRegalo.DescripcionRegalo = Helper.ObtenerDescripcionCompletaProductoEnComun(productos) + " x " + itemComponente.componentes.First().objPresentacion.Descripcion;
                            //            newRegalo.IdPresentacionRegaloSeleccionado = 0;
                            //            newRegalo.TipoRegalo = "Producto";
                            //            newRegalo.objDetallePedido = pedidoPagoFacil;
                            //            newRegalo.Grupo = itemComponente.componentes.First().Grupo.Value;
                            //            pedidoPagoFacil.ColRegalos.Add(newRegalo);
                            //        }

                            //        pedidoPagoFacil.Cantidad = 1;
                            //        pedidoPagoFacil.Producto = promoPagoFacil.IdProducto;
                            //        pedidoPagoFacil.Presentacion = promoPagoFacil.ColPresentaciones[0].IdPresentacion;
                            //        pedidoPagoFacil.ProductoDesc = promoPagoFacil.Descripcion;
                            //        pedidoPagoFacil.PresentacionDesc = promoPagoFacil.ColPresentaciones[0].Descripcion;
                            //        pedidoPagoFacil.DescripcionCompleta = pedidoPagoFacil.ProductoDesc;
                            //        pedidoPagoFacil.DescProductosUtilizados = descripcionPromo;
                            //        pedidoPagoFacil.CodigoCompleto = promoPagoFacil.ColPresentaciones[0].Codigo;
                            //        pedidoPagoFacil.Tipo = "E";

                            //        AllPromosGeneradas.Add(pedidoPagoFacil);
                            //    }
                            //}
                        }
                        else if (cboFormaPago.Text.Contains("Pago Mis Cuentas"))
                        {
                            //string codigoPromoPagoMisCuenas = "1150000021207";
                            //if (decimal.Parse(txtMontoGeneral.Text.Replace("$", "")) > Convert.ToDecimal("1814,99"))
                            //{
                            //    codigoPromoPagoMisCuenas = "1150000021208";
                            //}

                            //Producto promoPagoMisCuentas = (from P in Contexto.Presentacions
                            //                                where P.Codigo.Trim() == codigoPromoPagoMisCuenas
                            //                                select P.objProducto).First<Producto>();



                            //if (promoPagoMisCuentas.objConfPromocion != null && promoPagoMisCuentas.objConfPromocion.FechaInicio <= DateTime.Now && promoPagoMisCuentas.objConfPromocion.FechaFinal > DateTime.Now
                            //   && TotalCompradoParaPromociones() >= promoPagoMisCuentas.objConfPromocion.MontoMinimo.Value
                            //   && (promoPagoMisCuentas.objConfPromocion.ColTransportistas.Count == 0 || promoPagoMisCuentas.objConfPromocion.ColTransportistas.Any(w => w.Transporte.ToUpper() == lblTransporte.Text.ToUpper())))
                            //{
                            //    List<string> descripcionPromo = new List<string>();
                            //    descripcionPromo.Add("Pago|Mis Cuentas");

                            //    DetallePedido pedidoPagoFacil = new DetallePedido();


                            //    var composicionRegalo = from R in promoPagoMisCuentas.ColComposiciones
                            //                            where R.TipoComposicion == "O"
                            //                            group R by R.Grupo into c
                            //                            select new { Grupo = c.Key, componentes = c };

                            //    if (composicionRegalo.Count() > 0)
                            //    {

                            //        foreach (var itemComponente in composicionRegalo)
                            //        {
                            //            List<Producto> productos = (from P in itemComponente.componentes
                            //                                        select P.objProductoHijo).ToList<Producto>();

                            //            DetalleRegalos newRegalo = new DetalleRegalos();
                            //            newRegalo.DescripcionRegalo = Helper.ObtenerDescripcionCompletaProductoEnComun(productos) + " x " + itemComponente.componentes.First().objPresentacion.Descripcion;
                            //            newRegalo.IdPresentacionRegaloSeleccionado = 0;
                            //            newRegalo.TipoRegalo = "Producto";
                            //            newRegalo.objDetallePedido = pedidoPagoFacil;
                            //            newRegalo.Grupo = itemComponente.componentes.First().Grupo.Value;
                            //            pedidoPagoFacil.ColRegalos.Add(newRegalo);
                            //        }




                            //        pedidoPagoFacil.Cantidad = 1;
                            //        pedidoPagoFacil.Producto = promoPagoMisCuentas.IdProducto;
                            //        pedidoPagoFacil.Presentacion = promoPagoMisCuentas.ColPresentaciones[0].IdPresentacion;
                            //        pedidoPagoFacil.ProductoDesc = promoPagoMisCuentas.Descripcion;
                            //        pedidoPagoFacil.PresentacionDesc = promoPagoMisCuentas.ColPresentaciones[0].Descripcion;
                            //        pedidoPagoFacil.DescripcionCompleta = pedidoPagoFacil.ProductoDesc;
                            //        pedidoPagoFacil.DescProductosUtilizados = descripcionPromo;
                            //        pedidoPagoFacil.CodigoCompleto = promoPagoMisCuentas.ColPresentaciones[0].Codigo;
                            //        pedidoPagoFacil.Tipo = "E";

                            //        AllPromosGeneradas.Add(pedidoPagoFacil);
                            //    }
                            //}
                        }
                        else if (cboFormaPago.Text.Contains("Rapi Pago"))
                        {
                            //string codigoPromoRapiPago = "1150000021516";

                            //if (decimal.Parse(txtMontoGeneral.Text.Replace("$", "")) > Convert.ToDecimal("1814,99"))
                            //{
                            //    codigoPromoRapiPago = "1150000021518";
                            //}

                            //Producto promoRapiPago = (from P in Contexto.Presentacions
                            //                          where P.Codigo.Trim() == codigoPromoRapiPago
                            //                          select P.objProducto).First<Producto>();



                            //if (promoRapiPago.objConfPromocion != null && promoRapiPago.objConfPromocion.FechaInicio <= DateTime.Now && promoRapiPago.objConfPromocion.FechaFinal > DateTime.Now
                            //   && TotalCompradoParaPromociones() >= promoRapiPago.objConfPromocion.MontoMinimo.Value
                            //   && (promoRapiPago.objConfPromocion.ColTransportistas.Count == 0 || promoRapiPago.objConfPromocion.ColTransportistas.Any(w => w.Transporte.ToUpper() == lblTransporte.Text.ToUpper())))
                            //{
                            //    List<string> descripcionPromo = new List<string>();
                            //    descripcionPromo.Add("Rapi|Pago");

                            //    DetallePedido pedidoRapiPago = new DetallePedido();


                            //    var composicionRegalo = from R in promoRapiPago.ColComposiciones
                            //                            where R.TipoComposicion == "O"
                            //                            group R by R.Grupo into c
                            //                            select new { Grupo = c.Key, componentes = c };

                            //    if (composicionRegalo.Count() > 0)
                            //    {
                            //        foreach (var itemComponente in composicionRegalo)
                            //        {
                            //            List<Producto> productos = (from P in itemComponente.componentes
                            //                                        select P.objProductoHijo).ToList<Producto>();

                            //            DetalleRegalos newRegalo = new DetalleRegalos();
                            //            newRegalo.DescripcionRegalo = Helper.ObtenerDescripcionCompletaProductoEnComun(productos) + " x " + itemComponente.componentes.First().objPresentacion.Descripcion;
                            //            newRegalo.IdPresentacionRegaloSeleccionado = 0;
                            //            newRegalo.TipoRegalo = "Producto";
                            //            newRegalo.objDetallePedido = pedidoRapiPago;
                            //            newRegalo.Grupo = itemComponente.componentes.First().Grupo.Value;
                            //            pedidoRapiPago.ColRegalos.Add(newRegalo);
                            //        }




                            //        pedidoRapiPago.Cantidad = 1;
                            //        pedidoRapiPago.Producto = promoRapiPago.IdProducto;
                            //        pedidoRapiPago.Presentacion = promoRapiPago.ColPresentaciones[0].IdPresentacion;
                            //        pedidoRapiPago.ProductoDesc = promoRapiPago.Descripcion;
                            //        pedidoRapiPago.PresentacionDesc = promoRapiPago.ColPresentaciones[0].Descripcion;
                            //        pedidoRapiPago.DescripcionCompleta = pedidoRapiPago.ProductoDesc;
                            //        pedidoRapiPago.DescProductosUtilizados = descripcionPromo;
                            //        pedidoRapiPago.CodigoCompleto = promoRapiPago.ColPresentaciones[0].Codigo;
                            //        pedidoRapiPago.Tipo = "E";

                            //        AllPromosGeneradas.Add(pedidoRapiPago);
                            //    }
                            //}
                        }
                        else if (cboFormaPago.Text.Contains("Contra Reembolso"))
                        {
                            /// 30/06/2015: No existe mas la promoción combinada de contra reembolso y pago adelantado.
                            
                            ///// Limite de compra con la forma de pago contra reembolso.
                            //decimal LimiteContraReembolso = decimal.Parse((from P in Session["ParametrosSistema"] as List<Parametro>
                            //                                               where P.IdParametro == (int)TiposDeParametros.LimiteContraReembolso
                            //                                               select P.Valor).Single());

                            //string codigoPromoContraReembolso = "";

                            ///// Monto total del pedido
                            //decimal MontoPedidoActual = decimal.Parse(txtMontoGeneral.Text.Replace("$", ""));
                            ///// Diferencia a pagar con el saldo de la cta cte.
                            //decimal DiferenciaCtaCte = MontoPedidoActual - LimiteContraReembolso;


                            //// 1. Determinimo que promoción es la que se debe dar.
                            //if (DiferenciaCtaCte >= 1815)
                            //    codigoPromoContraReembolso = "1150000021593";
                            //else
                            //    codigoPromoContraReembolso = "1150000021244";

                            ///// 2. Si el monto del pedido es mayor al limite en contra reembolso y la diferencia se puede pagar
                            ///// con el saldo de la cta cte continuo.
                            //if (MontoPedidoActual > LimiteContraReembolso && Math.Abs(SaldoPagoAnticipado) >= DiferenciaCtaCte)
                            //{

                            //    Producto promoContraReembolso = (from P in Contexto.Presentacions
                            //                                     where P.Codigo.Trim() == codigoPromoContraReembolso
                            //                                     select P.objProducto).FirstOrDefault<Producto>();

                            //    /// Controlo que exista la promocion y que el transporte este habilitado para dicha promoción
                            //    if (promoContraReembolso != null && (promoContraReembolso.objConfPromocion.ColTransportistas.Count == 0 || promoContraReembolso.objConfPromocion.ColTransportistas.Any(w => w.Transporte.ToUpper() == lblTransporte.Text.ToUpper())))
                            //    {
                            //        decimal MontoMinimoPromocion = promoContraReembolso.objConfPromocion.MontoMinimo.Value;


                            //        /// 3. Detalle Total: Si el pedido es realizado con la forma de pago contra reembolso, 
                            //        /// el monto del pedido es mayor o igual al parámetro: limite de compra en contra reembolso y 
                            //        /// el cliente de la nota de pedido posee en ese momento un valor en la cuenta corriente a favor (en negativo)
                            //        /// y superior al MontoMinimo, entonces se generará para dicho pedido la promoción en cuestión
                            //        if (promoContraReembolso != null && promoContraReembolso.objConfPromocion != null && promoContraReembolso.objConfPromocion.FechaInicio <= DateTime.Now && promoContraReembolso.objConfPromocion.FechaFinal > DateTime.Now
                            //           && Math.Abs(SaldoPagoAnticipado) >= MontoMinimoPromocion)
                            //        {
                            //            List<string> descripcionPromo = new List<string>();
                            //            descripcionPromo.Add("Contra|Reembolso y Poseer un Saldo Mayor a $" + (MontoMinimoPromocion - decimal.Parse("0,01")) + " en Cta. Cte.");

                            //            DetallePedido pedidoPagoFacil = new DetallePedido();


                            //            var composicionRegalo = from R in promoContraReembolso.ColComposiciones
                            //                                    where R.TipoComposicion == "O"
                            //                                    group R by R.Grupo into c
                            //                                    select new { Grupo = c.Key, componentes = c };

                            //            if (composicionRegalo.Count() > 0)
                            //            {
                            //                foreach (var itemComponente in composicionRegalo)
                            //                {
                            //                    List<Producto> productos = (from P in itemComponente.componentes
                            //                                                select P.objProductoHijo).ToList<Producto>();

                            //                    DetalleRegalos newRegalo = new DetalleRegalos();
                            //                    newRegalo.DescripcionRegalo = Helper.ObtenerDescripcionCompletaProductoEnComun(productos) + " x " + itemComponente.componentes.First().objPresentacion.Descripcion;
                            //                    newRegalo.IdPresentacionRegaloSeleccionado = 0;
                            //                    newRegalo.TipoRegalo = "Producto";
                            //                    newRegalo.objDetallePedido = pedidoPagoFacil;
                            //                    newRegalo.Grupo = itemComponente.componentes.First().Grupo.Value;
                            //                    pedidoPagoFacil.ColRegalos.Add(newRegalo);
                            //                }




                            //                pedidoPagoFacil.Cantidad = 1;
                            //                pedidoPagoFacil.Producto = promoContraReembolso.IdProducto;
                            //                pedidoPagoFacil.Presentacion = promoContraReembolso.ColPresentaciones[0].IdPresentacion;
                            //                pedidoPagoFacil.ProductoDesc = promoContraReembolso.Descripcion;
                            //                pedidoPagoFacil.PresentacionDesc = promoContraReembolso.ColPresentaciones[0].Descripcion;
                            //                pedidoPagoFacil.DescripcionCompleta = pedidoPagoFacil.ProductoDesc;
                            //                pedidoPagoFacil.DescProductosUtilizados = descripcionPromo;
                            //                pedidoPagoFacil.CodigoCompleto = promoContraReembolso.ColPresentaciones[0].Codigo;
                            //                pedidoPagoFacil.Tipo = "E";

                            //                AllPromosGeneradas.Add(pedidoPagoFacil);
                            //            }
                            //        }
                            //    }
                            //}
                        }
                    }
                }
                #endregion


                #region Promociones por Forma de Pago a partir del 12/01/2015

                if (DateTime.Now.Date >= DateTime.Parse("12/01/2016"))
                {
                    if (cboFormaPago.Text.Contains("Pago Fácil") || cboFormaPago.Text.Contains("Pago Mis Cuentas") || cboFormaPago.Text.Contains("Rapi Pago"))
                    {
                        decimal montoTotalPedido = decimal.Parse(txtMontoGeneral.Text.Replace("$", ""));
                        string codigoPromoPagoAdelantado = "";

                        if (Math.Abs(SaldoPagoAnticipado) >= montoTotalPedido)
                        {

                            if (montoTotalPedido >= Convert.ToDecimal("850") && montoTotalPedido <= Convert.ToDecimal("1000"))
                            {
                                codigoPromoPagoAdelantado = "1150000021133";
                            }
                            else if (montoTotalPedido >= Convert.ToDecimal("1001") && montoTotalPedido <= Convert.ToDecimal("2000"))
                            {
                                codigoPromoPagoAdelantado = "1150000021134";
                            }
                            else if (montoTotalPedido > Convert.ToDecimal("2000"))
                            {
                                codigoPromoPagoAdelantado = "1150000021135";
                            }

                            if (codigoPromoPagoAdelantado != "")
                            {
                                Producto promoPagoAdelantado = (from P in Contexto.Presentacions
                                                                where P.Codigo.Trim() == codigoPromoPagoAdelantado
                                                                select P.objProducto).FirstOrDefault<Producto>();

                                if (promoPagoAdelantado != null)
                                {

                                    if (promoPagoAdelantado.objConfPromocion != null && promoPagoAdelantado.objConfPromocion.FechaInicio <= DateTime.Now && promoPagoAdelantado.objConfPromocion.FechaFinal > DateTime.Now
                                        && (promoPagoAdelantado.objConfPromocion.ColTransportistas.Count == 0 || promoPagoAdelantado.objConfPromocion.ColTransportistas.Any(w => w.Transporte.ToUpper() == lblTransporte.Text.ToUpper())))
                                    {
                                        List<string> descripcionPromo = new List<string>();
                                        string descripcionPromoPagoAdelantado = cboFormaPago.Text.Contains("Pago Fácil") ? "Pago|Fácil" : cboFormaPago.Text.Contains("Pago Mis Cuentas") ? "Pago| Mis Cuentas" : "Rapi|Pago";
                                        descripcionPromo.Add(descripcionPromoPagoAdelantado);

                                        DetallePedido detallePagoAdelantado = new DetallePedido();


                                        var composicionRegalo = from R in promoPagoAdelantado.ColComposiciones
                                                                where R.TipoComposicion == "O"
                                                                group R by R.Grupo into c
                                                                select new { Grupo = c.Key, componentes = c };

                                        if (composicionRegalo.Count() > 0)
                                        {
                                            foreach (var itemComponente in composicionRegalo)
                                            {
                                                List<Producto> productos = (from P in itemComponente.componentes
                                                                            select P.objProductoHijo).ToList<Producto>();

                                                DetalleRegalos newRegalo = new DetalleRegalos();
                                                newRegalo.DescripcionRegalo = Helper.ObtenerDescripcionCompletaProductoEnComun(productos) + " x " + itemComponente.componentes.First().objPresentacion.Descripcion;
                                                newRegalo.IdPresentacionRegaloSeleccionado = 0;
                                                newRegalo.TipoRegalo = "Producto";
                                                newRegalo.objDetallePedido = detallePagoAdelantado;
                                                newRegalo.Grupo = itemComponente.componentes.First().Grupo.Value;
                                                detallePagoAdelantado.ColRegalos.Add(newRegalo);
                                            }

                                            detallePagoAdelantado.Cantidad = 1;
                                            detallePagoAdelantado.Producto = promoPagoAdelantado.IdProducto;
                                            detallePagoAdelantado.Presentacion = promoPagoAdelantado.ColPresentaciones[0].IdPresentacion;
                                            detallePagoAdelantado.ProductoDesc = promoPagoAdelantado.Descripcion;
                                            detallePagoAdelantado.PresentacionDesc = promoPagoAdelantado.ColPresentaciones[0].Descripcion;
                                            detallePagoAdelantado.DescripcionCompleta = detallePagoAdelantado.ProductoDesc;
                                            detallePagoAdelantado.DescProductosUtilizados = descripcionPromo;
                                            detallePagoAdelantado.CodigoCompleto = promoPagoAdelantado.ColPresentaciones[0].Codigo;
                                            detallePagoAdelantado.Tipo = "E";

                                            AllPromosGeneradas.Add(detallePagoAdelantado);
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
                #endregion

                #endregion

                #region Inicialización del control de visualización de Promociones
                AllPromosGeneradas.AddRange(promosGeneradasUnaxPedido);
                AllPromosGeneradas.AddRange(promosGeneradasConRegalo);
                AllPromosGeneradas.AddRange(promosGeneradasSinRegalos);
                AllPromosGeneradas.AddRange(promosGeneradasConRegaloVIP);


                ///Marco las promociones generada con el id de la promoción
                /// que se guardo anteriormente.
                if (Session["PromosGuardadas"] != null)
                {
                    foreach (DetallePedido promoGuardada in (Session["PromosGuardadas"] as List<DetallePedido>))
                    {
                        var promoReGenerada = (from p in AllPromosGeneradas
                                               where p.Producto == promoGuardada.objProducto.IdProducto
                                               && p.IdDetalleLineaGuardado == 0
                                               select p).FirstOrDefault();

                        if (promoReGenerada != null)
                        {
                            promoReGenerada.IdDetalleLineaGuardado = promoGuardada.IdDetallePedido;
                            promoReGenerada.ValorTotal = Math.Abs(promoGuardada.ValorTotal.Value);

                            int indexRegalo = 0;
                            //foreach (DetallePedido detProdSel in promoGuardada.ColProductosSeleccionados)
                            foreach (DetallePedido detProdSel in (Session["productosRegalosSeleccionados"] as List<DetallePedido>).Where(w => w.PromocionOrigen == promoGuardada.IdDetallePedido).ToList())
                            {
                                if (promoReGenerada.ColRegalos.Count() > 0)
                                {
                                    if (promoReGenerada.ColRegalos.Count < indexRegalo)
                                    {
                                        promoReGenerada.ColRegalos[indexRegalo].TipoRegalo = "Producto";
                                        promoReGenerada.ColRegalos[indexRegalo].IdPresentacionPreSeleccionado = detProdSel.objPresentacion.IdPresentacion;
                                        promoReGenerada.IdRegaloSeleccionado = detProdSel.objPresentacion.IdPresentacion;

                                        if (!detProdSel.objProducto.Descripcion.Contains("x Unidad"))
                                            promoReGenerada.ColRegalos[indexRegalo].DescripcionPreSeleccionado = detProdSel.objProducto.objPadre.Descripcion + " " + detProdSel.objProducto.Descripcion + " x " + detProdSel.objPresentacion.Descripcion;
                                        else
                                            promoReGenerada.ColRegalos[indexRegalo].DescripcionPreSeleccionado = detProdSel.objProducto.Descripcion;

                                        indexRegalo++;
                                    }
                                }
                            }

                        }

                    }
                }

                TotalizadorPromos1.InitControl(AllPromosGeneradas, promosPosibles);
                upPromos.Update();

                #endregion
            }
        }

    }

    private void TotalizadorPedidos_LineaPedidoEliminada()
    {
        ActualizarTotalesGenerales();
        if (!EsClienteEspecial)
            CalcularPromociones();
    }


    private bool PoseePromoPedidoPagoAnticipado(long idCliente)
    {

        long año = DateTime.Now.Year;
        long mes = DateTime.Now.Month;
        /// 30/06/2015: No se utiliza mas esta regla de negocio, se paso a utilizar nuevas promociones de pago adelantado
        //List<string> promosPagoAdelantado = new List<string>() {"1150000021052", "1150000021073", "1150000021506", "1150000021207", "1150000021208", "1150000021516", "1150000021518", "1150000021593", "1150000021244" };
        // Nuevas Promociones Pago Adelantado
        List<string> promosPagoAdelantado = new List<string>() { "1150000021133", "1150000021135"};
        bool result = false;

        if (Request.QueryString["IdPedido"] != null)
        {
            long idCab = long.Parse(Request.QueryString["IdPedido"].ToString());
            result = Contexto.CabeceraPedidos.Any(w =>
                       w.IdCabeceraPedido != idCab &&
                       w.Cliente == idCliente &&
                       w.FechaPedido.Year == año &&
                       w.FechaPedido.Month == mes &&
                           //(!w.EsTemporal.HasValue || !w.EsTemporal.Value) &&
                       w.DetallePedidos.Any(d => promosPagoAdelantado.Contains(d.CodigoCompleto.Trim())));
        }
        else
        {
            result = Contexto.CabeceraPedidos.Any(w =>
                       w.Cliente == idCliente &&
                       w.FechaPedido.Year == año &&
                       w.FechaPedido.Month == mes &&
                           //(!w.EsTemporal.HasValue || !w.EsTemporal.Value) &&
                       w.DetallePedidos.Any(d => promosPagoAdelantado.Contains(d.CodigoCompleto.Trim())));
        }


        return result;
    }
    #endregion

    #region WEB Metodos
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static String GetDireccionEntrega(string IdConsultor, string IdFormaDePago)
    {
        string retorno = "false";
        string dirEntrega = "";
        string Transporte = "";
        string Coordinador = "";
        string SubConsultor = "";

        Marzzan_InfolegacyDataContext dc = (Marzzan_InfolegacyDataContext)HttpContext.Current.Session["Context"];


        Cliente CurrentConsultor = (from D in dc.Clientes
                                    where D.IdCliente == long.Parse(IdConsultor)
                                    select D).First<Cliente>();

        CalcularSaldoPagoAnticipado(CurrentConsultor, dc);

        Direccione direccionIncial = (from D in dc.Direcciones
                                      where D.CodigoExterno == CurrentConsultor.CodigoExterno
                                      && D.EsPrincipal.Value
                                      orderby D.CodigoExternoDir
                                      select D).FirstOrDefault<Direccione>();

        if (direccionIncial == null)
        {

            direccionIncial = (from D in dc.Direcciones
                               where D.CodigoExterno == CurrentConsultor.CodigoExterno
                               orderby D.CodigoExternoDir
                               select D).FirstOrDefault<Direccione>();
        }


        if (direccionIncial != null)
        {


            dirEntrega = direccionIncial.Provincia.ToLower().Trim() + " - " + direccionIncial.Localidad.ToLower().Trim();
            dirEntrega += "@" + direccionIncial.Calle.ToLower().Trim();

            ConfTransporte confT = (from C in dc.ConfTransportes
                                    where C.Provincia.ToLower() == direccionIncial.Provincia.ToLower() &&
                                    C.Localidad.ToLower() == direccionIncial.Localidad.ToLower() &&
                                    C.FormaDePago == IdFormaDePago
                                    select C).FirstOrDefault<ConfTransporte>();


            if (confT != null)
            {
                Transporte = confT.Transporte.ToLower();
            }
            else
                Transporte = "SIN TRANSPORTE";



            if (CurrentConsultor.CodTipoCliente == TipoClientesConstantes.CONSULTOR)
            {
                Cliente clienteSuperior = (from C in dc.Clientes
                                           where C.CodTipoCliente != TipoClientesConstantes.CONSULTOR
                                              && C.CodClasif1 == CurrentConsultor.CodClasif1
                                           select C).First();

                if ((clienteSuperior.CodTipoCliente == TipoClientesConstantes.SUBCONGESTION ||
                    clienteSuperior.CodTipoCliente == TipoClientesConstantes.SUBSINGESTION) && clienteSuperior.TipoCliente.ToUpper().Contains("SUB"))
                {
                    SubConsultor = clienteSuperior.Nombre.ToLower();
                    Coordinador = clienteSuperior.Vendedor.ToLower();

                }
                else
                {
                    SubConsultor = "Sin Asignar";
                    Coordinador = clienteSuperior.Nombre.ToLower();

                }

            }
            else
            {

                SubConsultor = "Sin Asignar";
                Coordinador = CurrentConsultor.Nombre.ToLower();

            }

            string ultimaActualizacion = "";
            if (CurrentConsultor.UltimaActualizacion.HasValue)
                ultimaActualizacion = CurrentConsultor.UltimaActualizacion.Value.ToShortDateString();


            var ultimoPedido = (from v in dc.View_UltimoPedidoClientes
                                where v.IdCliente == CurrentConsultor.IdCliente
                                select v.UltimaFechaPedido).FirstOrDefault();

            SubConsultor = CurrentConsultor.Clasif1;
            retorno = dirEntrega + "%" + Transporte + "%" + Coordinador + "@" + SubConsultor;
            retorno += "%" + DateTime.Now.ToShortDateString() + "%" + ObtenerSeguienteNro(0, dc);
            retorno += "%" + ((decimal)HttpContext.Current.Session["SaldoAnticipado"]).ToString() + "%" + ultimaActualizacion;
            retorno += "%" + (ultimoPedido == null ? "Sin Pedido Previo" : ultimoPedido.Value.ToShortDateString());

            return retorno;
        }
        else
        {
            return "";
        }
    }


    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string ControlesDeGrabacion(decimal subTotal, decimal saldoPagoAnticipado, decimal valorTransporte, string formaDePago, string provincia, string localidad, decimal totalPedido, string transporte)
    {
        string alertaControles = "";
        bool esClienteEspecial = (HttpContext.Current.Session["ClienteLogeado"] as Cliente).Clasif1.Contains("DIRECTORIO") ? true : false;

        using (Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext())
        {
            #region Calculo el Total Comprado
            /// Solo tiene en cuenta los productos comisionables para el calculo, es decir
            /// aquello donde el código comienza con un 1.
            decimal TotalComprado = (from P in (HttpContext.Current.Session["detPedido"] as List<DetallePedido>)
                                     where (P.CodigoCompleto.Substring(0, 1) == "1" && P.Tipo == "A") || (P.Tipo == "P") || (P.Tipo == "D")
                                   || (P.CodigoCompleto.Trim() == "2506600030159") || (P.CodigoCompleto.Trim() == "2506600030089") 
                                     select P.ValorTotal.Value).Sum();


            #endregion

            #region Calculo el Total Comprado para control PROMOCIONES


            decimal TotalParaPromociones = TotalCompradoParaPromociones();


            /// Recupero los montos minimos de las promociones segun su configuracion.
            List<long> idsPromosTemp = (HttpContext.Current.Session["PromosGeneradas"] as List<DetallePedido>).Select(w => w.Producto.Value).ToList();
            var PromocionesInvalidas = (from p in dc.ConfPromociones
                                        where idsPromosTemp.Contains(p.IdProductoPromo) && p.MontoMinimo > TotalParaPromociones
                                        select new
                                        {
                                            Descripcion = p.objProductoPromo.Descripcion,
                                            Monto = p.MontoMinimo
                                        }).ToList();

            if (PromocionesInvalidas.Count > 0)
            {
                string mensajePromos = "Las siguientes promociones no pueden ser solicitadas, ya que las mismas poseen un monto mínimo establecido, el cual es mayor al monto del pedido.</br>Promociones:</br>";
                foreach (var item in PromocionesInvalidas)
                {
                    mensajePromos += "&nbsp;&nbsp;&nbsp;" + item.Descripcion + " <b>Monto Mínimo: " + string.Format("${0:###.0#}", item.Monto.Value) + "</b>";
                }

                alertaControles = "AlertaMinimoPromociones('" + mensajePromos + "');";
                return alertaControles;

                //ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "MinimoReqCredito", "AlertaMinimoPromociones('" + mensajePromos + "');", true);
                //return alertaControles;
            }

            #endregion

            #region Busco el limite de compra por provincia
            decimal LimitePorProvincia = 0;
            var limetesDeCompra = (from P in dc.LimitesDeCompras
                                   select P).ToList();

            var LimiteCompraProvincia = (from P in limetesDeCompra
                                         where P.Provincia.Trim().ToUpper() == provincia.Trim().ToUpper() && P.Localidad.Trim().ToUpper() == localidad.Trim().ToUpper()
                                         select P.Limite).FirstOrDefault();

            /// si es null significa que no hay una definicion especifica
            /// para localidad y provincia entonces busco solo para la provincia.
            if (LimiteCompraProvincia == null)
            {
                LimiteCompraProvincia = (from P in limetesDeCompra
                                         where P.Provincia.Trim().ToUpper() == provincia.Trim().ToUpper()
                                         select P.Limite).FirstOrDefault();

                if (LimiteCompraProvincia != null)
                    LimitePorProvincia = LimiteCompraProvincia.Value;
            }
            else
            {
                LimitePorProvincia = LimiteCompraProvincia.Value;
            }

            #endregion

            #region Busco el limite de compra general
            string LimiteCompraGeneral = (from P in HttpContext.Current.Session["ParametrosSistema"] as List<Parametro>
                                          where P.IdParametro == (int)TiposDeParametros.LimiteCompra
                                          select P.Valor).Single();

            #endregion

            #region Busco el limite de compra en Contra Reembolso
            string LimiteContraReembolso = (from P in HttpContext.Current.Session["ParametrosSistema"] as List<Parametro>
                                            where P.IdParametro == (int)TiposDeParametros.LimiteContraReembolso
                                            select P.Valor).Single();

            #endregion

            #region Control LIMITE GENERAL
            /// ESTE CONTROL NO SE REALIZA MAS SOBRE LOS PEDIDOS COMISIONABLES
            /// SINO QUE SE DEBE HACER SOBRE EL TOTAL DEL PEDIDO. MAIL: 30/10/2014
            ///// Control LIMITE POR PROVINCIA
            //if (TotalComprado < LimitePorProvincia)
            //{
            //    ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "MinimoReq", "AlertaMinimoRequeridoProvincia(" + LimitePorProvincia + ");", true);
            //    return;
            //}

            if (TotalComprado < int.Parse(LimiteCompraGeneral))
            {

                alertaControles = "AlertaMinimoRequerido(" + LimiteCompraGeneral + ");";
                return alertaControles;

                //ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "MinimoReq", "AlertaMinimoRequerido(" + LimiteCompraGeneral + ");", true);
                //return;
            }

            #endregion

            #region Control LIMITE EN CREDITO

            if (formaDePago == "Crédito")
            {
                decimal MontoDisponibleCredito = decimal.Parse(HttpContext.Current.Session["MontoDisponibleCredito"].ToString());
                if (TotalComprado > MontoDisponibleCredito)
                {
                    alertaControles = "AlertaMinimoRequeridoCredito(" + string.Format("{0:0.00}", MontoDisponibleCredito) + ");";
                    return alertaControles;

                    //ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "MinimoReqCredito", "AlertaMinimoRequeridoCredito(" + string.Format("{0:0.00}", MontoDisponibleCredito) + ");", true);
                    //return;
                }
            }

            #endregion

            #region Control LIMITE POR PROVINCIA -Sin tener en cuenta los impuestos

            /// Control LIMITE POR PROVINCIA
            if (subTotal < LimitePorProvincia)
            {
                alertaControles = "AlertaMinimoRequeridoProvincia(" + LimitePorProvincia + ");";
                return alertaControles;

                //ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "MinimoReq", "AlertaMinimoRequeridoProvincia(" + LimitePorProvincia + ");", true);
                //return;
            }

            #endregion

            #region Control de limites de compra por la forma de pago

            /// Este control se hace para todos lo clientes salvo los clientes del grupo DIRECTORIO
            if (!esClienteEspecial)
            {
                decimal SaldoActual = -1 * saldoPagoAnticipado;
                switch (formaDePago)
                {
                    case "Pago Fácil":
                        {
                            if (subTotal > SaldoActual)
                            {
                                decimal TotalSinTransporte = subTotal - valorTransporte;
                                alertaControles = "AlertaSaldoInsuficiente('El monto del pedido (Productos: $" + TotalSinTransporte.ToString() + " + Transporte: $" + valorTransporte.ToString() + ") supera el saldo disponible que posee ($" + SaldoActual.ToString() + "), el mismo no puede ser realizado hasta que tenga saldo suficiente. Si lo desea puede guardar el pedido temporalmente para realizarlo en otro momento.');";
                                return alertaControles;


                                //ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "SaldoReq", "AlertaSaldoInsuficiente('El monto del pedido (Productos: $" + TotalSinTransporte.ToString() + " + Transporte: $" + valorTransporte.ToString() + ") supera el saldo disponible que posee ($" + SaldoActual.ToString() + "), el mismo no puede ser realizado hasta que tenga saldo suficiente. Si lo desea puede guardar el pedido temporalmente para realizarlo en otro momento.');", true);
                                //return;
                            }
                            else
                                break;


                        }
                    case "Pago Mis Cuentas":
                        {
                            if (subTotal > SaldoActual)
                            {
                                decimal TotalSinTransporte = subTotal - valorTransporte;

                                alertaControles = "AlertaSaldoInsuficiente('El monto del pedido (Productos: $" + TotalSinTransporte.ToString() + " + Transporte: $" + valorTransporte.ToString() + ") supera el saldo disponible que posee ($" + SaldoActual.ToString() + "), el mismo no puede ser realizado hasta que tenga saldo suficiente. Si lo desea puede guardar el pedido temporalmente para realizarlo en otro momento.');";
                                return alertaControles;


                                //ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "SaldoReq", "AlertaSaldoInsuficiente('El monto del pedido (Productos: $" + TotalSinTransporte.ToString() + " + Transporte: $" + valorTransporte.ToString() + ") supera el saldo disponible que posee ($" + SaldoActual.ToString() + "), el mismo no puede ser realizado hasta que tenga saldo suficiente. Si lo desea puede guardar el pedido temporalmente para realizarlo en otro momento.');", true);
                                //return;
                            }
                            else
                                break;


                        }
                    case "Contra Reembolso":
                        {

                            if (transporte.ToUpper().Contains("VILA"))
                            {
                                if ((totalPedido - SaldoActual) > decimal.Parse("1500"))
                                {
                                    alertaControles = "AlertaSaldoInsuficiente('El monto del pedido supera el límite en contra reemboldo ($ 1500), el mismo no puede ser realizado. Si lo desea puede guardar el pedido temporalmente para realizarlo en otro momento.');";
                                    return alertaControles;
                                }
                                else
                                    break;
                            }
                            /// CAMBIO SOLICITADO EL 23/09/2015
                            /// La modificación del valor de contra rembolso para el transporte Koliseo y Koliseo II debería 
                            /// quedar implementada en la web de producción el 25/09/2015 a partir de las 16 hs. 
                            else if ((transporte.ToUpper().Contains("KOLISEO") || transporte.ToUpper().Contains("KOLISEO II")) && DateTime.Now >= DateTime.Parse("24/09/2015 12:00:00"))
                            {
                                if ((totalPedido - SaldoActual) > decimal.Parse("1500"))
                                {
                                    alertaControles = "AlertaSaldoInsuficiente('El monto del pedido supera el límite en contra reemboldo ($ 1500), el mismo no puede ser realizado. Si lo desea puede guardar el pedido temporalmente para realizarlo en otro momento.');";
                                    return alertaControles;
                                }
                                else
                                    break;
                            }
                            /// CAMBIO SOLICITADO EL 23/09/2015 
                            /// La modificación del valor de contra rembolso para el transporte Castillo 
                            /// debería quedar implementada en la web de producción el 01/10/2015 a partir de las 16 hs. 
                            else if (transporte.ToUpper().Contains("CASTILLO") && DateTime.Now >= DateTime.Parse("01/10/2015 16:00:00"))
                            {
                                if ((totalPedido - SaldoActual) > decimal.Parse("1500"))
                                {
                                    alertaControles = "AlertaSaldoInsuficiente('El monto del pedido supera el límite en contra reemboldo ($ 1500), el mismo no puede ser realizado. Si lo desea puede guardar el pedido temporalmente para realizarlo en otro momento.');";
                                    return alertaControles;
                                }
                                else
                                    break;
                            }

                            else
                            {
                                /// 02/07/2015: Se aplico regla para controlar el limite en contra reembolso, contra el total del pedido
                                /// el cual incluye producto, fletes e impuestos.
                                if ((totalPedido - SaldoActual) > decimal.Parse(LimiteContraReembolso))
                                {
                                    alertaControles = "AlertaSaldoInsuficiente('El monto del pedido supera el límite en contra reemboldo ($ " + LimiteContraReembolso + "), el mismo no puede ser realizado. Si lo desea puede guardar el pedido temporalmente para realizarlo en otro momento.');";
                                    return alertaControles;

                                    //ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "SaldoReq", "AlertaSaldoInsuficiente('El monto del pedido supera el límite en contra reemboldo ($ " + LimiteContraReembolso + "), el mismo no puede ser realizado. Si lo desea puede guardar el pedido temporalmente para realizarlo en otro momento.');", true);
                                    //return;
                                }
                                else
                                    break;
                            }
                            break;

                        }
                    case "Rapi Pago":
                        {
                            if (subTotal > SaldoActual)
                            {
                                decimal TotalSinTransporte = subTotal - valorTransporte;

                                alertaControles = "AlertaSaldoInsuficiente('El monto del pedido (Productos: $" + TotalSinTransporte.ToString() + " + Transporte: $" + valorTransporte.ToString() + ") supera el saldo disponible que posee ($" + SaldoActual.ToString() + "), el mismo no puede ser realizado hasta que tenga saldo suficiente. Si lo desea puede guardar el pedido temporalmente para realizarlo en otro momento.');";
                                return alertaControles;


                                //ScriptManager.RegisterStartupScript(upSolicitudPedido, typeof(UpdatePanel), "SaldoReq", "AlertaSaldoInsuficiente('El monto del pedido (Productos: $" + TotalSinTransporte.ToString() + " + Transporte: $" + valorTransporte.ToString() + ") supera el saldo disponible que posee ($" + SaldoActual.ToString() + "), el mismo no puede ser realizado hasta que tenga saldo suficiente. Si lo desea puede guardar el pedido temporalmente para realizarlo en otro momento.');", true);
                                //return;
                            }
                            else
                                break;


                        }
                }
            }



            #endregion

        }

        return alertaControles;
    }

    #endregion

}