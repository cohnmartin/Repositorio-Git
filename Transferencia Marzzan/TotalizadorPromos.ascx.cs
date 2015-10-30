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
using Telerik.Web.UI;
using CommonMarzzan;

public partial class TotalizadorPromos : System.Web.UI.UserControl, IGrilla
{
    public List<DetallePedido> _promosGeneradas = null;

    public bool HayPromosIncompletas
    {
        get;
        set;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["PromosGeneradas"] == null || (Session["PromosGeneradas"] as IList).Count == 0)
            {
                grillaPromosGanadas.DataSource = new List<DetallePedido>();
                grillaPromosGanadas.DataBind();
            }
        }
    }

    public void InitControl(List<DetallePedido> PromosGeneradas, Hashtable PromosPosibles)
    {
        this.RadToolTipManager1.TargetControls.Clear();

        Session["PromosGeneradas"] = PromosGeneradas;

        grillaPromosGanadas.DataSource = PromosGeneradas;
        grillaPromosGanadas.DataBind();

        if (PromosGeneradas.Count > 0)
        {
            grillaPromosGanadas.Visible = true;
        }
        else
        {
            grillaPromosGanadas.Visible = false;
        }

        if (PromosPosibles.Count > 0)
        {

            List<DetallePromoPosible> allPromosPosibles = new List<DetallePromoPosible>();
            foreach (Producto promo in PromosPosibles.Keys)
            {
                allPromosPosibles.AddRange((List<DetallePromoPosible>)PromosPosibles[promo]);
            }

            repeaperOportunidades.DataSource = allPromosPosibles;
            repeaperOportunidades.DataBind();
            rowOportunidades.Visible = true;

        }
        else
        {
            rowOportunidades.Visible = false;
        }



        /// Total de produtos de regalos a seleccionar, incluido las promociones de tipo descuentos 
        int CantidadRegalosSeleccionar = (from P in (Session["PromosGeneradas"] as List<DetallePedido>)
                                          from R in P.ColRegalos
                                          select R).Count();

        /// Calculo la cantidad de regalos seleccionados incluido los productos
        /// por defecto de las promociones de tipo descuento.
        int RegalosSeleccionados = (from P in (Session["PromosGeneradas"] as List<DetallePedido>)
                                    from R in P.ColRegalos
                                    where R.IdPresentacionRegaloSeleccionado != 0
                                    select R).Distinct().Count();

        if (CantidadRegalosSeleccionar == RegalosSeleccionados)
        {
            PromosCompletasHiden.Value = "true";
            HayPromosIncompletas = false;
        }
        else
        {
            PromosCompletasHiden.Value = "false";
            HayPromosIncompletas = true;
        }


        //int PromosCompletas = (from P in (Session["PromosGeneradas"] as List<DetallePedido>)
        //                       where (P.IdRegaloSeleccionado != 0)
        //                       select P).Count();



        //if ((Session["PromosGeneradas"] as List<DetallePedido>).Count == PromosCompletas)
        //{
        //    PromosCompletasHiden.Value = "true";
        //    HayPromosIncompletas = false;
        //}
        //else
        //{
        //    PromosCompletasHiden.Value = "false";
        //    HayPromosIncompletas = true;
        //}

        upPromosGanada.Update();
    }

    public void AsociarRegalo(string IdRegalo, string DescRegalo, int indexPromo, int indexRegalo)
    {
        List<DetallePedido> promos = (from P in (Session["PromosGeneradas"] as List<DetallePedido>)
                                      orderby P.ProductoDesc descending
                                      select P).ToList<DetallePedido>();


        promos[indexPromo].ColRegalos[indexRegalo].IdPresentacionRegaloSeleccionado = long.Parse(IdRegalo);
        promos[indexPromo].ColRegalos[indexRegalo].IdPresentacionPreSeleccionado = long.Parse(IdRegalo);
        promos[indexPromo].ColRegalos[indexRegalo].DescripcionPreSeleccionado = DescRegalo;
        ((grillaPromosGanadas.Items[indexPromo].FindControl("ListRegalos") as DataList).Items[indexRegalo].FindControl("lblProdRegalo") as Label).Text = DescRegalo;


        /// Total de produtos de regalos a seleccionar, incluido las promociones de tipo descuentos 
        int CantidadRegalosSeleccionar = (from P in promos
                                          from R in P.ColRegalos
                                          select R).Count();

        /// Calculo la cantidad de regalos seleccionados incluido los productos
        /// por defecto de las promociones de tipo descuento.
        int RegalosSeleccionados = (from P in promos
                                    from R in P.ColRegalos
                                    where R.IdPresentacionRegaloSeleccionado != 0
                                    select R).Distinct().Count();

        if (CantidadRegalosSeleccionar == RegalosSeleccionados)
        {
            PromosCompletasHiden.Value = "true";
            HayPromosIncompletas = false;
        }
        else
        {
            PromosCompletasHiden.Value = "false";
            HayPromosIncompletas = true;
        }


        Session["PromosGeneradas"] = promos;
        grillaPromosGanadas.DataSource = promos;
        grillaPromosGanadas.DataBind();
        upPromosGanada.Update();
    }

    protected void repeaperOportunidades_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DetallePromoPosible promoPosible = (DetallePromoPosible)e.Item.DataItem;

        #region Producto Faltante

        Label lblProductoFaltante = (System.Web.UI.WebControls.Label)e.Item.FindControl("lblProductoFaltante");
        Label lblRegaloOpertunidad = (System.Web.UI.WebControls.Label)e.Item.FindControl("lblRegaloOpertunidad");


        long grupoComposicion = (from C in (promoPosible.Promo as Producto).ColComposiciones
                                 where C.objProductoHijo.IdProducto == (promoPosible.Producto as Producto).IdProducto
                                 select C.Grupo.Value).First<long>();


        List<Producto> composicionProdFaltantes = (from C in (promoPosible.Promo as Producto).ColComposiciones
                                                   where C.Grupo == grupoComposicion
                                                   select C.objProductoHijo).ToList<Producto>();

        string descripcion = "";
        if (composicionProdFaltantes.Count > 1)
            descripcion = Helper.ObtenerDescripcionCompletaProductoEnComun(composicionProdFaltantes) + " x ";
        else
            descripcion = composicionProdFaltantes[0].Descripcion + " x ";


        lblProductoFaltante.Text = promoPosible.Cantidad + descripcion + (promoPosible.Presentacion as Presentacion).Descripcion;

        #endregion

        #region Regalo


        var composicionRegalo = from C in (promoPosible.Promo as Producto).ColComposiciones
                                where C.TipoComposicion == "O"
                                group C by C.Grupo into c
                                select new { Grupo = c.Key, componentes = c };

        if (composicionRegalo.Count() > 0)
        {
            int i = 1;
            foreach (var componente in composicionRegalo)
            {
                if (componente.componentes.Count() == 1)
                {
                    descripcion = componente.componentes.First().Cantidad + "  " + componente.componentes.First().objProductoHijo.Descripcion;
                }
                else
                {
                    var productos = from p in componente.componentes
                                    select p.objProductoHijo;

                    descripcion = componente.componentes.First().Cantidad + "  " + Helper.ObtenerDescripcionCompletaProductoEnComun(productos.ToList());
                }

                if (componente.componentes.First().objPresentacion.Descripcion != "Unidad")
                    descripcion = descripcion + " x " + componente.componentes.First().objPresentacion.Descripcion;

                ((Label)e.Item.FindControl("lblRegaloOpertunidad" + i.ToString())).Text = descripcion;
                ((Label)e.Item.FindControl("lblRegaloOpertunidad" + i.ToString())).Visible = true;
                i++;
            }
        }
        else
        {
            ((Label)e.Item.FindControl("lblRegaloOpertunidad1")).Text = "Un Descuento de $" + (promoPosible.Promo as Producto).Precio.ToString();
            ((Label)e.Item.FindControl("lblRegaloOpertunidad1")).Visible = true;
        }




        #endregion

    }

    protected void OnAjaxUpdate(object sender, ToolTipUpdateEventArgs args)
    {
        if (grillaPromosGanadas.Items.Count > 0)
            this.UpdateToolTip(args.Value, args.UpdatePanel);
    }

    private void UpdateToolTip(string datos, UpdatePanel panel)
    {
        string[] parametros = datos.Split('|');
        long idpromo = long.Parse(parametros[0]);
        int indexPromo = int.Parse(parametros[1]);
        int indexRegalo = int.Parse(parametros[2]);
        long grupo = long.Parse(parametros[3]);

        Control ctrl = Page.LoadControl("ProductoRegalo.ascx");
        panel.ContentTemplateContainer.Controls.Add(ctrl);
        ((ProductoRegalo)ctrl).InitControl(idpromo, indexPromo, indexRegalo, grupo);

    }

    public void Clear()
    {
        grillaPromosGanadas.DataSource = null;
        grillaPromosGanadas.DataBind();
    }

    public long productosRequeridosIngresadosTemp
    { get; set; }

    public long totalProductosRequeridosTemp
    { get; set; }

    protected void grillaPromosGanadas_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            DetallePedido currentRow = (DetallePedido)e.Item.DataItem;
            Label lblDescComprado = (System.Web.UI.WebControls.Label)e.Item.FindControl("lblDescComprado");


            DataList detallePromo = (System.Web.UI.WebControls.DataList)e.Item.FindControl("DataList1");


            Button btnSeleccionarProductosPromo = (System.Web.UI.WebControls.Button)e.Item.FindControl("btnSeleccionarProductosPromo");
            btnSeleccionarProductosPromo.Visible = false;


            #region Generacion Promociones
            List<ComponentePromo> componentes = new List<ComponentePromo>();
            Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext();
            long TotalRequeridosIngresados = 0;

            if (currentRow.Tipo == "A")
            {
                lblDescComprado.Text = "Por Haber Comprado:";
            }
            else if (currentRow.Tipo == "P")
            {
                lblDescComprado.Text = "Por Haber Ganado:";
            }
            else if (currentRow.Tipo == "E")
            {
                lblDescComprado.Text = "Por Haber Pagado Con:";
            }
            else if (currentRow.Tipo == "M")
            {
                lblDescComprado.Text = "Por Haber Comprado:";
            }
            else if (currentRow.Tipo == "PS" || currentRow.Tipo == "PSD") // Promo solicitadas en forma directa
            {
                lblDescComprado.Text = "Por Haber Solicitado </br><span style='color:Black'>" + currentRow.PresentacionDesc + "</span>";
            }


            if (currentRow.Tipo == "PS" || currentRow.Tipo == "PSD") // Promo solicitadas en forma directa
            {
                var TotalesProductosRequeridos = (from v in (Session["Context"] as Marzzan_InfolegacyDataContext).View_PromosActivas
                                                  where v.IdPromocion == currentRow.Producto.Value
                                                  select v.CantidadSolicitada).ToList();

                int TotalProductosRequeridos = TotalesProductosRequeridos.Select(w => int.Parse(w)).Sum();

                totalProductosRequeridosTemp = TotalProductosRequeridos;
                e.Item.Attributes.Add("TotalProductosRequeridos", TotalProductosRequeridos.ToString());

                if (currentRow.colProductosRequeridos.Count > 0)
                {
                    foreach (DetalleProductosRequeridos det in currentRow.colProductosRequeridos)
                    {
                        ComponentePromo compPromo = new ComponentePromo();
                        compPromo.TipoPromo = currentRow.Tipo;
                        compPromo.IdProducto = det.IdProducto.Value;
                        compPromo.Descripcion = det.DescripcionProducto;
                        componentes.Add(compPromo);
                        TotalRequeridosIngresados += det.Cantidad.Value;
                    }
                }
                else
                {
                    ComponentePromo compPromo = new ComponentePromo();
                    compPromo.TipoPromo = currentRow.Tipo;
                    compPromo.Descripcion = "Debe Seleccionar los Productos";
                    componentes.Add(compPromo);
                }

                if (currentRow.Tipo == "PS")
                {
                    btnSeleccionarProductosPromo.Attributes.Add("onClick", "MostrarComponentesPromocion(this,'" + detallePromo.ClientID + "','" + currentRow.IdDetallePedido.ToString() + "');return false;");
                    btnSeleccionarProductosPromo.Visible = true;
                }
            }
            else
            {

                foreach (string comp in (currentRow.DescProductosUtilizados as List<string>))
                {
                    string[] datos = comp.Split('|');
                    ComponentePromo compPromo = new ComponentePromo();

                    compPromo.Descripcion = datos[0] + " " + datos[1];
                    compPromo.TipoPromo = currentRow.Tipo;


                    componentes.Add(compPromo);

                }
            }

            productosRequeridosIngresadosTemp = TotalRequeridosIngresados;
            detallePromo.Attributes.Add("TotalRequeridosIngresados", TotalRequeridosIngresados.ToString());
            detallePromo.DataSource = componentes;
            detallePromo.DataBind();


            #endregion

            #region Generacion Regalo

            (e.Item.FindControl("ListRegalos") as DataList).DataSource = currentRow.ColRegalos;
            (e.Item.FindControl("ListRegalos") as DataList).DataBind();

            #endregion
        }
    }

    protected void ListRegalos_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        DetalleRegalos item = (DetalleRegalos)e.Item.DataItem;
        if (item.Cantidad > 0)
            (e.Item.FindControl("lblNroRegalo") as Label).Text = item.Cantidad.ToString() + " ";
        else
        {
            (e.Item.FindControl("lblNroRegalo") as Label).Text = "1 ";
            item.Cantidad = 1;
        }



        if (item.IdPresentacionRegaloSeleccionado != 0 && item.IdPresentacionPreSeleccionado == 0)
        {
            (e.Item.FindControl("trProductoEleccion") as HtmlControl).Visible = false;

        }
        else
        {
            string idPromo = (e.Item.FindControl("trProductoEleccion").NamingContainer.NamingContainer.NamingContainer.FindControl("lblIdPromo") as Label).Text;
            string indicePromo = (e.Item.FindControl("trProductoEleccion").NamingContainer.NamingContainer.NamingContainer as GridDataItem).ItemIndex.ToString();
            string indiceRegalo = e.Item.ItemIndex.ToString();
            string grupo = (e.Item.FindControl("lblGrupo") as Label).Text;


            item.IdPresentacionRegaloSeleccionado = item.IdPresentacionPreSeleccionado;
            (e.Item.FindControl("lblProdRegalo") as Label).Text = item.DescripcionPreSeleccionado;

            string datos = idPromo + "|" + indicePromo + "|" + indiceRegalo + "|" + grupo;
            this.RadToolTipManager1.TargetControls.Add((e.Item.FindControl("btnSeleccionarProducto") as Button).ClientID, datos, true);
        }
    }


}
