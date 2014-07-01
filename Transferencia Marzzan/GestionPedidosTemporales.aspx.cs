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

public partial class GestionPedidosTemporales : BasePage
{

    protected override void PageLoad()
    {
        if (!IsPostBack)
        {
 
            Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext();

            var cliente = (from C in dc.Clientes
                           where C.IdCliente == long.Parse(Session["IdUsuario"].ToString())
                           orderby C.Nombre
                           select C).Single<Cliente>();

            List<Cliente> consultores = Helper.ObtenerConsultoresSubordinados((Cliente)cliente);

            if (consultores.Count > 0)
            {
                cboConsultores.AppendDataBoundItems = true;
                cboConsultores.Items.Add(new RadComboBoxItem("Todos los Revendedores", "-1"));
                cboConsultores.DataTextField = "Nombre";
                cboConsultores.DataValueField = "IdCliente";
                cboConsultores.DataSource = consultores;
                cboConsultores.DataBind();
                cboConsultores.SelectedIndex = 0;
            }
            else
            {
                cboConsultores.AppendDataBoundItems = true;
                cboConsultores.DataTextField = "Nombre";
                cboConsultores.DataValueField = "IdCliente";
                cboConsultores.Items.Add(new RadComboBoxItem(cliente.Nombre, cliente.IdCliente.ToString()));
                cboConsultores.DataSource = consultores;
                cboConsultores.DataBind();
                cboConsultores.SelectedIndex = 0;
                cboConsultores.Enabled = false;
            }

        }

    }


    protected void btnConsultar_Click(object sender, EventArgs e)
    {

        Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext();
        List<CabeceraPedido> cabeceras = null;

        if (cboConsultores.SelectedValue == "")
        {
            return;
        }
        else if (cboConsultores.SelectedValue != "-1")
        {
            var cliente = (from C in dc.Clientes
                           where C.IdCliente == long.Parse(cboConsultores.SelectedValue)
                           orderby C.Nombre
                           select C).Single<Cliente>();


             cabeceras = (from C in dc.CabeceraPedidos
                         where (C.Cliente == cliente.IdCliente )
                         && (((C.EsTemporal.HasValue && C.EsTemporal.Value) && (C.HuboFaltaSaldo.HasValue)) || (C.objFormaDePago.Descripcion.Contains("Tarjeta") && C.EstadoOperacionTarjeta != "APROBADA"))
                         select C).ToList<CabeceraPedido>();
      
        }
        else
        {
            var cliente = (from C in dc.Clientes
                           where C.IdCliente == long.Parse(Session["IdUsuario"].ToString())
                           orderby C.Nombre
                           select C).Single<Cliente>();


            List<Cliente> consultores = Helper.ObtenerConsultoresSubordinados((Cliente)cliente);


            List<long> IdsConsultores = (from C in consultores
                                         orderby C.Nombre
                                         select C.IdCliente).ToList<long>();


            cabeceras = (from C in dc.CabeceraPedidos
                             where IdsConsultores.Contains(C.Cliente)
                             && (((C.EsTemporal.HasValue && C.EsTemporal.Value) && (C.HuboFaltaSaldo.HasValue)) || (C.objFormaDePago.Descripcion.Contains("Tarjeta") && C.EstadoOperacionTarjeta != "APROBADA"))
                             select C).ToList<CabeceraPedido>();

        }


        List<long> ids = (from C in cabeceras
                          select C.IdCabeceraPedido).ToList<long>();


        List<DetallePedido> detalles = (from D in dc.DetallePedidos
                                        where ids.Contains(D.CabeceraPedido.Value)
                                        select D).ToList<DetallePedido>();



        GrillaTemporales.DataSource =  from C in  cabeceras
                                       where ! C.HuboFaltaSaldo.Value
                                       select C;


        GrillaTemporalesSaldo.DataSource =  from C in  cabeceras
                                       where C.HuboFaltaSaldo.Value
                                       select C;


        GrillaTemporalesTarjeta.DataSource = from C in cabeceras
                                             where C.objFormaDePago.Descripcion.Contains("Tarjeta")
                                             select C;



        GrillaTemporales.MasterTableView.DetailTables[0].DataSource = from D in  detalles
                                                                          where ! D.objCabecera.HuboFaltaSaldo.Value
                                                                          select D;
        GrillaTemporales.MasterTableView.DetailTables[0].DataBind();


        GrillaTemporalesSaldo.MasterTableView.DetailTables[0].DataSource = from D in detalles
                                                                           where D.objCabecera.HuboFaltaSaldo.Value
                                                                           select D;
        GrillaTemporalesSaldo.MasterTableView.DetailTables[0].DataBind();


        GrillaTemporalesTarjeta.MasterTableView.DetailTables[0].DataSource = from D in detalles
                                                                           where D.objCabecera.objFormaDePago.Descripcion.Contains("Tarjeta")
                                                                           select D;

        GrillaTemporalesTarjeta.MasterTableView.DetailTables[0].DataBind();


        
        GrillaTemporales.DataBind();
        GrillaTemporalesSaldo.DataBind();
        GrillaTemporalesTarjeta.DataBind();

        upResultado.Update();
        upResultadoSaldo.Update();
        upResultadoTarjeta.Update();
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

    protected void GrillaTemporalesSaldo_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            if (e.Item.DataItem is CabeceraPedido)
            {
                CabeceraPedido currentCab = (CabeceraPedido)e.Item.DataItem;

                if (currentCab.objCliente.SaldoCtaCte > currentCab.MontoTotal)
                {
                    (e.Item.FindControl("imgEstado") as HtmlImage).Src = "Imagenes/mailFlag.gif";
                    (e.Item.FindControl("imgEstado") as HtmlImage).Alt = "Saldo Dispobible";
                }
                else
                {
                    (e.Item.FindControl("imgEstado") as HtmlImage).Src = "Imagenes/mailFlagRed.gif";
                    (e.Item.FindControl("imgEstado") as HtmlImage).Alt = "Saldo Insuficiente";
                }


                (e.Item.FindControl("btnEliminarSaldo") as ImageButton).Attributes.Add("onclick", "return blockConfirm('Esta seguro que desea eliminar el pedido pendiente por falta de saldo?', event, 330, 100,'','Eliminar Pedido');");
                (e.Item.FindControl("btnEliminarSaldo") as ImageButton).Attributes.Add("IdPedido", (e.Item.DataItem as CabeceraPedido).IdCabeceraPedido.ToString());
                
            }
        }
    }

    protected void GrillaTemporales_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            if (e.Item.DataItem is CabeceraPedido)
            {
                (e.Item.FindControl("btnEliminar") as ImageButton).Attributes.Add("onclick", "return blockConfirm('Esta seguro que desea eliminar el pedido pendiente?', event, 330, 100,'','Eliminar Pedido');");
                (e.Item.FindControl("btnEliminar") as ImageButton).Attributes.Add("IdPedido", (e.Item.DataItem as CabeceraPedido).IdCabeceraPedido.ToString());

            }
        }
    }
    
    
    protected void btnEliminar_Click(object sender, EventArgs e)
    {

        long id = long.Parse((sender as ImageButton).Attributes["IdPedido"].ToString());

        Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext();

        CabeceraPedido CabEliminar = (from C in dc.CabeceraPedidos
                                      where C.IdCabeceraPedido == id
                                      select C).SingleOrDefault();

        if (! CabEliminar.EsTemporal.HasValue || ! CabEliminar.EsTemporal.Value)
        {
            GrillaTemporales.DataSource = new List<CabeceraPedido>();
            GrillaTemporalesSaldo.DataSource = new List<CabeceraPedido>();
            btnConsultar_Click(null, null);
            upResultado.Update();
            upResultadoSaldo.Update();
        }
        else if (CabEliminar.EsTemporal.HasValue && CabEliminar.EsTemporal.Value)
        {
            dc.DetallePedidos.DeleteAllOnSubmit(CabEliminar.DetallePedidos);
            dc.CabeceraPedidos.DeleteOnSubmit(CabEliminar);
            dc.SubmitChanges();
            btnConsultar_Click(null, null);
        }

    }
}
