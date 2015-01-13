using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CommonMarzzan;

public partial class RegaloDeTuLider : BasePage
{
    private string ObtenerSeguienteNro(string Tipo)
    {

        Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext();

        try
        {
            var ultimoNro = (from D in dc.CabeceraPedidos
                             where D.TipoPedido == Tipo
                             select Convert.ToInt32(D.Nro)).Max<int>();

            return Convert.ToString(long.Parse(ultimoNro.ToString()) + 1);
        }
        catch
        {
            return "1";
        }


    }

    protected override void PageLoad()
    {
        if (!IsPostBack)
        {
            Session.Add("ListaComponente", null);

            Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext();

            var cliente = (from C in dc.Clientes
                           where C.IdCliente == long.Parse(Session["IdUsuario"].ToString())
                           select C).Single<Cliente>();

            List<Cliente> consultores = Helper.ObtenerConsultoresSubordinados((Cliente)cliente);


            if (consultores.Count() > 0)
            {

                cboConsultores.AppendDataBoundItems = true;
                cboConsultores.Items.Add(new RadComboBoxItem("Seleccione un Revendedor", "0"));
                cboConsultores.DataTextField = "Nombre";
                cboConsultores.DataValueField = "IdCliente";
                cboConsultores.DataSource = consultores;
                cboConsultores.DataBind();
                cboConsultores.SelectedIndex = 0;

                lblLider.Text = Session["NombreUsuario"].ToString();
                lblConsultor.Visible = false;
            }
            else
            {
                cboConsultores.Visible = false;
                lblLider.Text = cliente.objPadre.Nombre;
                lblConsultor.Text = Session["NombreUsuario"].ToString();
                lblConsultor.Visible = true;
                CargarDirecciones(long.Parse(Session["IdUsuario"].ToString()));
            }

            cboRegalos.AppendDataBoundItems = true;
            cboRegalos.Items.Add(new RadComboBoxItem("Seleccione un Regalo", "0"));
            cboRegalos.DataTextField = "Descripcion";
            cboRegalos.DataValueField = "IdProducto";
            cboRegalos.DataSource = (from C in dc.Productos
                                     where C.Tipo == 'R' && C.Descripcion.ToLower().Contains("consultor")
                                     select C).ToList();


            cboRegalos.DataBind();
            cboRegalos.SelectedIndex = 0;


            cboFormaPago.DataTextField = "Descripcion";
            cboFormaPago.DataValueField = "IdFormaPago";
            cboFormaPago.DataSource = from Fp in dc.FormaDePagos
                                      where Fp.Cliente == 2
                                      select Fp;
            cboFormaPago.DataBind();
            cboFormaPago.SelectedValue = "4";


            cboDirecciones.AppendDataBoundItems = true;
            cboDirecciones.Items.Add(new RadComboBoxItem("Seleccione una Dirección", "0"));
        }
    }

    protected void btnVolver_Click(object sender, EventArgs e)
    {
        Response.Redirect("Inicio.aspx");
    }

    protected void cboRegalos_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext();


        lblRegalo.Text = cboRegalos.SelectedItem.Text;

        var composicionRegalo = from C in dc.Composicions
                                where C.ComponentePricipal == long.Parse(cboRegalos.SelectedValue)
                                && C.TipoComposicion == "C"
                                group C by C.Grupo into p
                                select new { Grupo = p.Key, componentes = p };

        List<ComponentesRegalo> listaComponente = new List<ComponentesRegalo>();
        foreach (var item in composicionRegalo)
        {
            ComponentesRegalo compReg = new ComponentesRegalo();
            if (item.componentes.Count() == 1)
            {
                compReg.Descripcion = item.componentes.First<Composicion>().objProductoHijo.objPadre.DescripcionCompleta;
                compReg.FraganciaSeleccionada = item.componentes.First<Composicion>().objProductoHijo.Descripcion;
                compReg.Presentacion = item.componentes.First<Composicion>().objPresentacion.Descripcion;
                compReg.CodigoSeleccionado = item.componentes.First<Composicion>().objPresentacion.Codigo;
                compReg.Precio = item.componentes.First<Composicion>().objPresentacion.Precio.Value;
                compReg.IdPresentacion = item.componentes.First<Composicion>().objPresentacion.IdPresentacion;
                compReg.IdProducto = item.componentes.First<Composicion>().objProductoHijo.IdProducto;
                compReg.Fragancias = "";
                compReg.Codigos = "";
                compReg.Grupo = item.Grupo.ToString();
                compReg.EsFraganciaSeleccionable = false;
            }
            else
            {
                List<Producto> productos = (from P in item.componentes
                                            select P.objProductoHijo).ToList<Producto>();


                compReg.Descripcion = Helper.ObtenerDescripcionCompletaProductoEnComun(productos);
                compReg.Presentacion = item.componentes.First<Composicion>().objPresentacion.Descripcion;
                compReg.Precio = item.componentes.First<Composicion>().objPresentacion.Precio.Value;
                compReg.IdProducto = item.componentes.First<Composicion>().objProductoHijo.IdProducto;
                compReg.IdPresentacion = item.componentes.First<Composicion>().objPresentacion.IdPresentacion;
                compReg.CodigoSeleccionado = "";
                compReg.Grupo = item.Grupo.ToString();
                compReg.EsFraganciaSeleccionable = true;

                foreach (Composicion comp in item.componentes)
                {
                    if (comp.objPresentacion.Activo.Value)
                    {
                        compReg.Fragancias += comp.objProductoHijo.objPadre.Descripcion + " " + comp.objProductoHijo.Descripcion + "|";
                        compReg.Codigos += comp.objPresentacion.Codigo + "|";
                    }

                }


            }

            listaComponente.Add(compReg);
        }

        Session["ListaComponente"] = listaComponente;
        dlDetalleRegalo.DataSource = listaComponente;
        dlDetalleRegalo.DataBind();


    }

    protected void cboConsultores_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {

        CargarDirecciones(long.Parse(cboConsultores.SelectedValue));
    }

    private void CargarDirecciones(long id)
    {
        Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext();

        var cliente = (from C in dc.Clientes
                       where C.IdCliente == id
                       select C).Single<Cliente>();

        cboDirecciones.Items.Clear();
        cboDirecciones.AppendDataBoundItems = true;
        cboDirecciones.Items.Add(new RadComboBoxItem("Seleccione una Dirección", "0"));
        cboDirecciones.DataTextField = "Calle";
        cboDirecciones.DataValueField = "IdDireccion";

        cboDirecciones.DataSource = (from D in dc.Direcciones
                                     where D.CodigoExterno == cliente.CodigoExterno
                                     orderby D.CodigoExternoDir
                                     select D).ToList();

        cboDirecciones.DataBind();
        cboDirecciones.SelectedIndex = 0;

    }

    protected void btnRealizarRegalo_Click(object sender, EventArgs e)
    {
        /// tengo que generar un remito para el consultor, donde el detalle es el producto regalo
        /// seleccionado, y una no de pedido para el consultor seleccionado donde el detalle son los
        /// producto que componen el producto regalo.

        if ((Session["ListaComponente"] as List<ComponentesRegalo>).Count == 0)
        {
            toolTipFragancias.Text = "No hay elementos para regalar un el producto seleccionado";
            toolTipFragancias.Show();
            UpdatePanel1.Update();
            return;
        }
        foreach (ComponentesRegalo det in (Session["ListaComponente"] as List<ComponentesRegalo>))
        {
            if (det.FraganciaSeleccionada == null || det.FraganciaSeleccionada == "")
            {
                toolTipFragancias.Text = "Hay regalos que no tienen seleccionada la fragancia. Por favor verifique los datos.";
                toolTipFragancias.Show();
                UpdatePanel1.Update();
                return;
            }
        }

        Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext();


        Producto regalo = (from C in dc.Productos
                           where C.Tipo == 'R' && C.IdProducto == long.Parse(cboRegalos.SelectedValue)
                           select C).Single<Producto>();

        DetallePedido newDetalle = null;

        #region Nota Debito Para Coordinador

        CabeceraPedido cabecera = new CabeceraPedido();
        cabecera.Cliente = long.Parse(Session["idUsuario"].ToString());
        cabecera.ClienteSolicitante = long.Parse(Session["idUsuario"].ToString());
        cabecera.DireccionEntrega = long.Parse(cboDirecciones.SelectedValue);
        cabecera.FechaPedido = DateTime.Now;
        cabecera.FormaPago = long.Parse(cboFormaPago.SelectedValue);
        cabecera.MontoTotal = regalo.Precio.Value;
        cabecera.Nro = ObtenerSeguienteNro("ND");
        cabecera.TipoPedido = "ND";
        cabecera.Impreso = false;
        cabecera.NroImpresion = 0;



        var ConseptosFacturar = from C in regalo.ColComposiciones
                                where C.TipoComposicion == "D"
                                select C;

        foreach (Composicion concepto in ConseptosFacturar)
        {
            newDetalle = new DetallePedido();
            newDetalle.Cantidad = 1;
            newDetalle.CodigoCompleto = concepto.objPresentacion.Codigo;
            newDetalle.Presentacion = concepto.objPresentacion.IdPresentacion;
            newDetalle.Producto = concepto.objProductoHijo.IdProducto;
            newDetalle.ValorUnitario = concepto.objPresentacion.Precio.Value;
            newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;
            cabecera.DetallePedidos.Add(newDetalle);
        }


        dc.CabeceraPedidos.InsertOnSubmit(cabecera);


        #endregion




        #region Remito Para Consultora

        cabecera = new CabeceraPedido();
        cabecera.Cliente = long.Parse(cboConsultores.SelectedValue);
        cabecera.ClienteSolicitante = long.Parse(Session["idUsuario"].ToString());
        cabecera.DireccionEntrega = long.Parse(cboDirecciones.SelectedValue);
        cabecera.FechaPedido = DateTime.Now;
        cabecera.FormaPago = long.Parse(cboFormaPago.SelectedValue);
        cabecera.MontoTotal = 0;
        cabecera.Nro = ObtenerSeguienteNro("RT");
        cabecera.TipoPedido = "RT";
        cabecera.Impreso = false;
        cabecera.NroImpresion = 0;

        /// Concepto que descuenta
        newDetalle = new DetallePedido();
        newDetalle.Cantidad = 1;
        newDetalle.CodigoCompleto = regalo.ColPresentaciones[0].Codigo;
        newDetalle.Presentacion = regalo.ColPresentaciones[0].IdPresentacion;
        newDetalle.Producto = regalo.IdProducto;
        newDetalle.ValorUnitario = -1 * regalo.ColPresentaciones[0].Precio.Value;
        newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;
        cabecera.DetallePedidos.Add(newDetalle);


        // creo los productos componentes de regalo de tu lider
        foreach (ComponentesRegalo det in (Session["ListaComponente"] as List<ComponentesRegalo>))
        {
            newDetalle = new DetallePedido();
            newDetalle.Cantidad = long.Parse(txtCantidad.Text);
            newDetalle.CodigoCompleto = det.CodigoSeleccionado;
            newDetalle.Presentacion = det.IdPresentacion;
            newDetalle.Producto = det.IdProducto;
            newDetalle.ValorUnitario = det.Precio;
            newDetalle.ValorTotal = newDetalle.ValorUnitario * newDetalle.Cantidad;

            cabecera.DetallePedidos.Add(newDetalle);

        }

        dc.CabeceraPedidos.InsertOnSubmit(cabecera);

        #endregion


        dc.SubmitChanges();

        cboConsultores.SelectedIndex = 0;
        cboFormaPago.SelectedIndex = 0;
        cboRegalos.SelectedIndex = 0;
        txtCantidad.Text = "1";
        cboDirecciones.Items.Clear();
        cboDirecciones.AppendDataBoundItems = true;
        cboDirecciones.Items.Add(new RadComboBoxItem("Seleccione una Dirección", "0"));
        dlDetalleRegalo.Items.Clear();
        dlDetalleRegalo.DataSource = null;
        dlDetalleRegalo.DataBind();
        upConsultores.Update();
        upDirecciones.Update();
        upFormaDePago.Update();
        upRegalo.Update();
        upRegalos.Update();

        toolTipFragancias.Title = "Solicitud";
        toolTipFragancias.Text = "La solicitud de regalo se ha realizado con exito.";
        toolTipFragancias.Show();
        UpdatePanel1.Update();


    }

    protected void dlDetalleRegalo_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            if (((e.Item as ListViewDataItem).DataItem as ComponentesRegalo).CodigoSeleccionado != "" &&
                ((e.Item as ListViewDataItem).DataItem as ComponentesRegalo).EsFraganciaSeleccionable == false)
                e.Item.FindControl("tdSeleccionFragancia").Visible = false;
            else
            {
                (e.Item.FindControl("btnSeleccionarFragancia") as ImageButton).Attributes.Add("Codigos", ((e.Item as ListViewDataItem).DataItem as ComponentesRegalo).Codigos);
                (e.Item.FindControl("btnSeleccionarFragancia") as ImageButton).Attributes.Add("Fragancias", ((e.Item as ListViewDataItem).DataItem as ComponentesRegalo).Fragancias);
                (e.Item.FindControl("btnSeleccionarFragancia") as ImageButton).Attributes.Add("Grupo", ((e.Item as ListViewDataItem).DataItem as ComponentesRegalo).Grupo);

            }
        }
    }

    public void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {

        if (e.Argument != "undefined")
        {
            string[] datos = e.Argument.ToString().Split('@');
            string codigo = datos[0];
            string grupo = datos[1];
            string fragancia = datos[2];

            ComponentesRegalo comp = (from C in (Session["ListaComponente"] as List<ComponentesRegalo>)
                                      where C.Grupo == grupo
                                      select C).First<ComponentesRegalo>();


            Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext();
            Presentacion PreseSel = (from P in dc.Presentacions
                                     where P.Codigo == codigo
                                     select P).SingleOrDefault();


            comp.CodigoSeleccionado = PreseSel.Codigo;
            comp.FraganciaSeleccionada = fragancia;
            comp.IdProducto = PreseSel.objProducto.IdProducto;
            comp.IdPresentacion = PreseSel.IdPresentacion;


            dlDetalleRegalo.DataSource = Session["ListaComponente"];
            dlDetalleRegalo.DataBind();
            upRegalo.Update();

        }
    }

    private class ComponentesRegalo
    {
        string _descripcion;
        string _presentacion;
        string _codigoSeleccionado;
        string _codigos;
        string _fragancias;
        string _grupo;
        string _fraganciaSeleccionada;
        bool _esFraganciaSeleccionable = false;
        decimal _precio;
        long _idProducto;
        long _idPresentacion;


        public decimal Precio
        {
            get { return _precio; }
            set { _precio = value; }
        }
        public long IdProducto
        {
            get { return _idProducto; }
            set { _idProducto = value; }
        }
        public long IdPresentacion
        {
            get { return _idPresentacion; }
            set { _idPresentacion = value; }
        }

        public bool EsFraganciaSeleccionable
        {
            get { return _esFraganciaSeleccionable; }
            set { _esFraganciaSeleccionable = value; }
        }

        public string FraganciaSeleccionada
        {
            get { return _fraganciaSeleccionada; }
            set { _fraganciaSeleccionada = value; }
        }

        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        public string Presentacion
        {
            get { return _presentacion; }
            set { _presentacion = value; }
        }

        public string CodigoSeleccionado
        {
            get { return _codigoSeleccionado; }
            set { _codigoSeleccionado = value; }
        }

        public string Codigos
        {
            get { return _codigos; }
            set { _codigos = value; }
        }

        public string Fragancias
        {
            get { return _fragancias; }
            set { _fragancias = value; }
        }

        public string Grupo
        {
            get { return _grupo; }
            set { _grupo = value; }
        }
    }
}
