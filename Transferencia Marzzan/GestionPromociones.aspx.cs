using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Telerik.Web.UI;
using CommonMarzzan;

public partial class GestionPromociones : System.Web.UI.Page
{
    public static List<Composicion> componentes = new List<Composicion>();
    public static Marzzan_InfolegacyDataContext _dc = null;

    public class TempProductos
    {
        public long IdProducto { get; set; }
        public string Descripcion { get; set; }
        public long IdPadre { get; set; }
        public char Tipo { get; set; }
    }

    private List<TempProductos> _productos
    {
        get
        {

            if ((Session["_productos"] as List<TempProductos>).Count == 0)
            {
                Session["_productos"] = (from prod in _dc.Productos
                                         where (prod.Tipo == 'A' || prod.Tipo == 'P')
                                         select new TempProductos
                                         {
                                             IdProducto = prod.IdProducto,
                                             Descripcion = prod.Descripcion,
                                             IdPadre = prod.objPadre != null ? prod.objPadre.IdProducto : 0,
                                             Tipo = prod.Tipo
                                         }).ToList();
            }

            return (List<TempProductos>)Session["_productos"];
        }
        set
        {

            Session["_productos"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session.Add("Componentes", new List<Composicion>());
            componentes = new List<Composicion>();
            _dc = new Marzzan_InfolegacyDataContext();
            _productos = new List<TempProductos>();

            grillaComponentes.DataSource = new List<Composicion>();
            grillaComponentes.DataBind();

            InitCombos();
        }
    }



    private void InitCombos()
    {

        cboUndNeg.DataTextField = "Descripcion";
        cboUndNeg.DataValueField = "IdProducto";
        cboUndNeg.DataSource = Helper.GetProductosByNivel(1);
        cboUndNeg.DataBind();
        cboUndNeg.Items.Insert(0, new RadComboBoxItem("- Seleccione una Unidad -"));


        //cboPromociones.DataSource = (from prod in _dc.Productos
        //                             where (prod.Tipo != 'A' && prod.Tipo != 'G'
        //                             && prod.Tipo != 'R' && prod.Tipo != 'N' && prod.Tipo != 'I')
        //                             orderby prod.Descripcion
        //                             select new
        //                             {
        //                                 Descripcion = prod.Descripcion + " $" + prod.Precio.ToString(),
        //                                 IdProducto = prod.IdProducto

        //                             }).ToList();

        //cboPromociones.DataBind();
    }

    protected void cboPromociones_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        if (e.Text != "")
        {

            var datos = (from prod in _dc.Productos
                         where (prod.Tipo != 'A' && prod.Tipo != 'G'
                         && prod.Tipo != 'R' && prod.Tipo != 'I') //&& prod.Tipo != 'N'
                         && prod.Descripcion.StartsWith(e.Text)
                         orderby prod.Descripcion
                         select new
                         {
                             Descripcion = prod.Descripcion + " $" + prod.Precio.ToString(),
                             IdProducto = prod.IdProducto
                         }).Take(20).ToList();

            foreach (var item in datos)
            {
                cboPromociones.Items.Add(new RadComboBoxItem(item.Descripcion, item.IdProducto.ToString()));
            }

        }
    }

    protected void cboLineas_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        if (e.Text != "")
        {
            //long id = long.Parse(e.Text);
            //cboLineas.DataTextField = "Descripcion";
            //cboLineas.DataValueField = "IdProducto";
            //cboLineas.DataSource = (from p in _productos
            //                        where p.IdPadre == id
            //                        select p).ToList();
            ////cboLineas.DataSource = Helper.GetProductosByParent(long.Parse(e.Text));

            //cboLineas.DataBind();

            long id = long.Parse(e.Text);
            var datos = (from p in _productos
                         where p.IdPadre == id
                         select p).ToList();

            foreach (var item in datos)
            {
                cboLineas.Items.Add(new RadComboBoxItem(item.Descripcion, item.IdProducto.ToString()));
            }
            cboLineas.Items.Insert(0, new RadComboBoxItem("- Todas -", "0"));
        }
    }

    protected void cboFragancias_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        if (e.Text != "")
        {
            string[] datos = e.Text.Split('|');

            if (datos[0].ToString() != "0")
            {
                //Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext();
                //Producto linea = (from P in dc.Productos
                //                  where P.IdProducto == long.Parse(datos[0].ToString())
                //                  select P).First<Producto>();

                TempProductos linea = (from P in _productos
                                       where P.IdProducto == long.Parse(datos[0].ToString())
                                       select P).First<TempProductos>();

                if (linea.Tipo != 'P' && linea.Tipo != 'D')
                {

                    cboFragancias.DataTextField = "Descripcion";
                    cboFragancias.DataValueField = "IdProducto";
                    cboFragancias.DataSource = (from p in _productos
                                                where p.IdPadre == long.Parse(datos[0].ToString())
                                                select p).ToList();

                    //cboFragancias.DataSource = Helper.GetProductosByParent(long.Parse(datos[0].ToString()));
                    cboFragancias.DataBind();
                    cboFragancias.Items.Insert(0, new RadComboBoxItem("- Todas -", "0"));
                }
                else
                {
                    cboFragancias.Text = "";
                    cboFragancias.Enabled = false;
                    cboPresentaciones.Enabled = false;
                    upAgregarDefinicion.Update();
                }
            }
            else
            {
                cboFragancias.DataTextField = "Descripcion";
                cboFragancias.DataValueField = "IdProducto";
                cboFragancias.DataSource = (from p in _productos
                                            where p.IdPadre == long.Parse(datos[1].ToString())
                                            select p).ToList();

                //cboFragancias.DataSource = Helper.GetFraganciasByUndNeg(long.Parse(datos[1].ToString()));
                cboFragancias.DataBind();
                cboFragancias.Items.Insert(0, new RadComboBoxItem("- Todas -", "0"));
            }






        }
    }

    protected void cboPresentaciones_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        string[] datos = e.Text.Split('|');

        cboPresentaciones.DataTextField = "Descripcion";
        cboPresentaciones.DataValueField = "IdPresentacion";
        if (datos[0].ToString() != "0")
        {
            cboPresentaciones.DataSource = (from p in Helper.GetPresentaciones(long.Parse(datos[0].ToString()))
                                            where p.Activo.Value
                                            select new
                                            {
                                                Descripcion = p.Descripcion,
                                                IdPresentacion = p.IdPresentacion
                                            }).Distinct().ToList();
        }
        else if (datos[2].ToString() == "byLinea")
        {
            cboPresentaciones.DataSource = (from p in Helper.GetPresentacionesByLinea(long.Parse(datos[1].ToString()))
                                            where p.Activo.Value
                                            select new
                                            {
                                                Descripcion = p.Descripcion,
                                                IdPresentacion = p.IdPresentacion
                                            }).Distinct().ToList();


        }
        else if (datos[2].ToString() == "byUndNeg")
        {
            cboPresentaciones.DataSource = (from p in Helper.GetPresentacionesByUndNeg(long.Parse(datos[1].ToString()))
                                            where p.Activo.Value
                                            select new
                                            {
                                                Descripcion = p.Descripcion,
                                                IdPresentacion = p.IdPresentacion
                                            }).Distinct().ToList();

        }

        cboPresentaciones.DataBind();
        cboPresentaciones.Items.Insert(0, new RadComboBoxItem("- Todas -", "0"));
    }

    protected void cboDefiniciones_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        for (int i = 0; i < 10; i++)
        {
            cboDefiniciones.Items.Insert(i, new RadComboBoxItem("Productos Requeridos Nº " + Convert.ToString(i + 1), Convert.ToString(i + 1)));
        }

        cboDefiniciones.Items.Insert(10, new RadComboBoxItem("Productos de Regalo Grupo 1", "11"));
        cboDefiniciones.Items.Insert(11, new RadComboBoxItem("Productos de Regalo Grupo 2", "12"));
        cboDefiniciones.Items.Insert(12, new RadComboBoxItem("Productos de Regalo Grupo 3", "13"));
        cboDefiniciones.Items.Insert(13, new RadComboBoxItem("Productos de Regalo Grupo 4", "14"));
        cboDefiniciones.Items.Insert(14, new RadComboBoxItem("Productos de Regalo Grupo 5", "15"));
        cboDefiniciones.Items.Insert(15, new RadComboBoxItem("Productos de Regalo Grupo 6", "16"));

    }

    protected void btnAgregarDefinicion_Click(object sender, EventArgs e)
    {
        List<Presentacion> presentaciones = new List<Presentacion>();
        if (cboFragancias.SelectedValue == "")
        {
            presentaciones = Helper.GetAllPresentacionesByPromo(long.Parse(cboLineas.SelectedValue)).Where(w => w.Activo.Value).ToList();
        }
        else if (cboLineas.SelectedValue == "0")
        {
            presentaciones = Helper.GetAllPresentacionesByUndNeg(long.Parse(cboUndNeg.SelectedValue)).Where(w => w.Activo.Value).ToList();
        }
        else if (cboFragancias.SelectedValue == "0")
        {
            presentaciones = Helper.GetAllPresentacionesByLnea(long.Parse(cboLineas.SelectedValue)).Where(w => w.Activo.Value).ToList();
        }
        else
        {
            presentaciones = Helper.GetPresentaciones(long.Parse(cboFragancias.SelectedValue)).Where(w => w.Activo.Value).ToList();
        }



        foreach (Presentacion itemPre in presentaciones)
        {
            if ((itemPre.Descripcion == cboPresentaciones.Text && cboFragancias.SelectedValue != "")
                || cboFragancias.SelectedValue == "" || (cboLineas.SelectedValue == "0" && cboFragancias.SelectedValue == "0" && cboPresentaciones.SelectedValue == "0"))
            {
                Composicion newcomp = new Composicion();
                newcomp.Cantidad = txtCantidad.Text;
                newcomp.ComponentePricipal = long.Parse(cboPromociones.SelectedValue);
                newcomp.ComponenteHijo = itemPre.objProducto.IdProducto;
                newcomp.Presentacion = itemPre.IdPresentacion;
                newcomp.objProductoHijo = itemPre.objProducto;
                newcomp.objPresentacion = itemPre;
                newcomp.Grupo = long.Parse(cboDefiniciones.SelectedValue);

                if (cboDefiniciones.Text.IndexOf("Regalo") >= 0)
                    newcomp.TipoComposicion = "O";
                else
                    newcomp.TipoComposicion = "C";

                (Session["Componentes"] as List<Composicion>).Add(newcomp);
            }
        }

        grillaComponentes.DataSource = (from C in Session["Componentes"] as List<Composicion>
                                        orderby C.Grupo
                                        select C).ToList();

        grillaComponentes.DataBind();
        upGrilla.Update();

        cboUndNeg.SelectedIndex = 0;
        cboLineas.Text = "";
        cboFragancias.Text = "";
        cboPresentaciones.Text = "";
        cboDefiniciones.Text = "";
        upAgregarDefinicion.Update();

    }

    protected void btnGrabar_Click(object sender, EventArgs e)
    {

        Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext();
        long idProdPromo = long.Parse(cboPromociones.SelectedValue);
        foreach (Composicion itemComp in (Session["Componentes"] as List<Composicion>))
        {
            if (itemComp.IdComposicion == 0)
            {
                Composicion newcomp = new Composicion();
                newcomp.Cantidad = itemComp.Cantidad;
                newcomp.ComponentePricipal = itemComp.ComponentePricipal;
                newcomp.ComponenteHijo = itemComp.ComponenteHijo;
                newcomp.Presentacion = itemComp.Presentacion;
                newcomp.Grupo = itemComp.Grupo;
                newcomp.TipoComposicion = itemComp.TipoComposicion;
                dc.Composicions.InsertOnSubmit(newcomp);

            }
        }

        Producto ProdPromo = (from P in dc.Productos
                              where P.IdProducto == idProdPromo
                              select P).First<Producto>();


        if (ProdPromo.objConfPromocion == null)
        {
            ConfPromocione confPromo = new ConfPromocione();
            confPromo.objProductoPromo = ProdPromo;
            confPromo.FechaInicio = txtFechaInicial.SelectedDate.Value;
            confPromo.FechaFinal = txtFechaFinal.SelectedDate.Value;
            confPromo.TipoPromo = cboTipoPromo.Text;
            confPromo.UnaPorPedido = chkUnicaxPedido.Checked;
            confPromo.MontoMinimo = Convert.ToDecimal(txtMontoMinimo.Value);
            ProdPromo.objConfPromocion = confPromo;
        }
        else
        {
            ProdPromo.objConfPromocion.FechaInicio = txtFechaInicial.SelectedDate.Value;
            ProdPromo.objConfPromocion.FechaFinal = txtFechaFinal.SelectedDate.Value;
            ProdPromo.objConfPromocion.TipoPromo = cboTipoPromo.Text;
            ProdPromo.objConfPromocion.UnaPorPedido = chkUnicaxPedido.Checked;
            ProdPromo.objConfPromocion.MontoMinimo = Convert.ToDecimal(txtMontoMinimo.Value);
        }



        dc.SubmitChanges();
        grillaComponentes.DataSource = null;
        grillaComponentes.DataBind();
        upGrilla.Update();

        Session["Componentes"] = new List<Composicion>();
        txtFechaInicial.SelectedDate = null;
        txtFechaFinal.SelectedDate = null;
        cboTipoPromo.SelectedIndex = 0;
        chkUnicaxPedido.Checked = false;
        cboPromociones.SelectedIndex = 0;

        cboLineas.Text = "";
        cboFragancias.Text = "";
        cboPresentaciones.Text = "";
        cboDefiniciones.Text = "";
        IdConfPromoSeleccionada.Value = "";
        cboPromociones.Text = "";
        txtCantidad.Value = 1;
        txtMontoMinimo.Value = 0;
        upAgregarDefinicion.Update();
        upConfPromo.Update();
        upSelectedPromo.Update();
        _dc = new Marzzan_InfolegacyDataContext();
    }

    protected void btnVolver_Click(object sender, EventArgs e)
    {
        Response.Redirect("Inicio.aspx");

    }

    protected void cboPromociones_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (cboPromociones.SelectedValue != "" && long.Parse(cboPromociones.SelectedValue) > 0)
        {
            List<Composicion> composiciones = (from C in _dc.Composicions
                                               orderby C.Grupo
                                               where C.ComponentePricipal == long.Parse(cboPromociones.SelectedValue)
                                               select C).ToList();

            Session["Componentes"] = composiciones;
            grillaComponentes.DataSource = composiciones;
            grillaComponentes.DataBind();
            upGrilla.Update();

            if (composiciones.Count > 0 && composiciones[0].objProducto.objConfPromocion != null)
            {
                cboTipoPromo.Items.FindItemByText(composiciones[0].objProducto.objConfPromocion.TipoPromo).Selected = true;
                txtFechaFinal.SelectedDate = composiciones[0].objProducto.objConfPromocion.FechaFinal;
                txtFechaInicial.SelectedDate = composiciones[0].objProducto.objConfPromocion.FechaInicio;
                chkUnicaxPedido.Checked = composiciones[0].objProducto.objConfPromocion.UnaPorPedido.Value;
                IdConfPromoSeleccionada.Value = composiciones[0].objProducto.objConfPromocion.IdConfPromocion.ToString();
                txtMontoMinimo.Value = Convert.ToDouble(composiciones[0].objProducto.objConfPromocion.MontoMinimo.Value);
                upConfPromo.Update();
            }
            else
            {
                txtFechaFinal.SelectedDate = null;
                txtFechaInicial.SelectedDate = null;
                chkUnicaxPedido.Checked = false;
                IdConfPromoSeleccionada.Value = "";
                txtMontoMinimo.Value = 0;
                upConfPromo.Update();
            }
        }

    }

    protected void grillaComponentes_ItemCreated(object sender, GridItemEventArgs e)
    {
        CreateHeaderControls(e);
    }
    protected void grillaComponentes_ItemDataBound(object sender, GridItemEventArgs e)
    {
        CreateHeaderControls(e);
    }
    private void CreateHeaderControls(GridItemEventArgs e)
    {
        if (e.Item is GridGroupHeaderItem)
        {
            GridGroupHeaderItem item = (GridGroupHeaderItem)e.Item;
            System.Data.DataRowView groupDataRow = (System.Data.DataRowView)e.Item.DataItem;
            Label lblTitulo = new Label();
            lblTitulo.ID = "lbltitutlogrupo";

            if (groupDataRow != null)
            {
                if (groupDataRow["TipoComposicion"].ToString() != "O")
                    lblTitulo.Text = "          Productos Requeridos Nº " + groupDataRow["Grupo"].ToString() + " - Cantidad Requerida: " + groupDataRow["Cantidad"].ToString();
                else
                {
                    string descHeader = "";
                    if (groupDataRow["Grupo"].ToString() == "0")
                    {
                        descHeader = "1";
                    }
                    else
                        descHeader = Convert.ToString(int.Parse(groupDataRow["Grupo"].ToString()) - 10);


                    lblTitulo.Text = "          Productos de Regalo Grupo " + descHeader + " - Cantidad a Regalar: " + groupDataRow["Cantidad"].ToString();
                }
            }

            ImageButton img = new ImageButton();
            img.ID = "DeleteAll";
            img.ImageUrl = "~/Imagenes/Delete.gif";
            img.Click += new ImageClickEventHandler(img_Click);
            img.Attributes.Add("Mensaje", "Eliminando Grupo Completo...");
            item.DataCell.Controls.Add(img);
            item.DataCell.Controls.Add(lblTitulo);
        }

    }
    private void img_Click(object sender, ImageClickEventArgs e)
    {
        Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext();
        ImageButton img = (ImageButton)sender;
        GridGroupHeaderItem groupHeader = (GridGroupHeaderItem)img.NamingContainer;
        long grupo = long.Parse(groupHeader.DataCell.Text.Split(';')[0].Split(':')[1].Trim());

        var composiciones = (from C in (Session["Componentes"] as List<Composicion>)
                             where C.Grupo == grupo
                             select C).ToList<Composicion>();


        foreach (Composicion itemComp in composiciones)
        {

            (Session["Componentes"] as List<Composicion>).Remove(itemComp);

            if (itemComp.IdComposicion > 0)
            {
                Composicion compDelete = (from C in dc.Composicions
                                          where C.IdComposicion == itemComp.IdComposicion
                                          select C).First<Composicion>();

                dc.Composicions.DeleteOnSubmit(compDelete);
            }
        }

        dc.SubmitChanges();
        grillaComponentes.DataSource = (Session["Componentes"] as List<Composicion>).ToList<Composicion>();
        grillaComponentes.DataBind();
        upGrilla.Update();


    }
    protected void btnEliminar_Click(object sender, ImageClickEventArgs e)
    {
        long idConf = long.Parse(((sender as ImageButton).NamingContainer as GridDataItem).GetDataKeyValue("Idcomposicion").ToString());

        if (idConf > 0)
        {
            Composicion conf = (from C in _dc.Composicions
                                where C.IdComposicion == idConf
                                select C).First<Composicion>();

            _dc.Composicions.DeleteOnSubmit(conf);
            _dc.SubmitChanges();
        }

        (Session["Componentes"] as List<Composicion>).RemoveAt(((sender as ImageButton).NamingContainer as GridDataItem).DataSetIndex);

        grillaComponentes.DataSource = (Session["Componentes"] as List<Composicion>).ToList<Composicion>();
        grillaComponentes.DataBind();
        upGrilla.Update();


    }



}
