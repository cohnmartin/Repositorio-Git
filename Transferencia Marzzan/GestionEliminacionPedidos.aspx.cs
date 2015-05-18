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


public partial class GestionEliminacionPedidos : BasePage
{

    protected override void PageLoad()
    {
        if (!IsPostBack)
        {
            GrillaResultados.DataSource = new List<CabeceraPedido>();
            GrillaResultados.DataBind();
        }
    }

    protected void btnConsultar_Click(object sender, EventArgs e)
    {

        Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext();
        List<CabeceraPedido> cabeceras = null;

        if (txtNroPedido.Text == "")
        {
            return;
        }
        else 
        {
          
             cabeceras = (from C in dc.CabeceraPedidos
                                 where C.Nro == txtNroPedido.Text.Trim()
                                 && C.TipoPedido == "NP"
                                 select C).ToList<CabeceraPedido>();

        }


        List<long> ids = (from C in cabeceras
                          select C.IdCabeceraPedido).ToList<long>();


        List<DetallePedido> detalles = (from D in dc.DetallePedidos
                                        where ids.Contains(D.CabeceraPedido.Value)
                                        select D).ToList<DetallePedido>();


        GrillaResultados.DataSource = cabeceras;


        GrillaResultados.MasterTableView.DetailTables[0].DataSource = detalles;
        GrillaResultados.MasterTableView.DetailTables[0].DataBind();

        GrillaResultados.DataBind();
        upResultado.Update();
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
        if (estado != null && estado.ToString().ToUpper() == "ENVIADO")
        {
            return "Procesado";

        }
        else
        {
            return "Sin Procesar";
        }

    }
    protected void GrillaResultados_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            if (e.Item.DataItem is CabeceraPedido)
            {
                (e.Item.FindControl("btnEliminar") as ImageButton).Attributes.Add("onclick", "return blockConfirm('Esta REALMENTE seguro que desea eliminar el pedido? Recuerde señor administrador que no quedará registro alguno del pedido.', event, 330, 100,'','Eliminar Pedido');");
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

        foreach (DetallePedido item in CabEliminar.DetallePedidos)
        {
            dc.SolicitudProductosEspeciales.DeleteAllOnSubmit(item.SolicitudProductosEspeciales);
        }

        dc.PedidosConCreditos.DeleteAllOnSubmit(CabEliminar.PedidosConCreditos);
        dc.RemitosAfectados.DeleteAllOnSubmit(CabEliminar.colRemitosAfectados);
        dc.DetallePedidos.DeleteAllOnSubmit(CabEliminar.DetallePedidos);
        dc.CabeceraPedidos.DeleteOnSubmit(CabEliminar);
        dc.SubmitChanges();



        btnConsultar_Click(null, null);


    }
}
