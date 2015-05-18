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

public partial class GestionEliminacionPedidosLideres : BasePage
{

    protected override void PageLoad()
    {
        if (!IsPostBack)
        {
            Session.Add("Clientes", null);
            Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext();

            var cliente = (from C in dc.Clientes
                           where C.IdCliente == long.Parse(Session["IdUsuario"].ToString())
                           orderby C.Nombre
                           select C).Single<Cliente>();


            List<Cliente> consultores = Helper.ObtenerConsultoresSubordinados((Cliente)cliente);
            Session["Clientes"] = consultores;

            if (consultores.Count > 0)
            {
                cboConsultores.AppendDataBoundItems = true;
                cboConsultores.Items.Add(new RadComboBoxItem("Cualquier Revendedor", "-1"));
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

            GrillaResultados.DataSource = new List<CabeceraPedido>();
            GrillaResultados.DataBind();

            txtFechaInicial.SelectedDate = DateTime.Now.AddDays(-37);
            txtFechaFinal.SelectedDate = DateTime.Now;
        }
    }

    protected void btnConsultar_Click(object sender, EventArgs e)
    {

        Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext();
        List<CabeceraPedido> cabeceras = null;
        List<long> idsClientes = cboConsultores.Items.Select(w => long.Parse(w.Value)).ToList();

        if (txtNroPedido.Text == "")
        {
            DateTime fechaDesde = txtFechaInicial.SelectedDate.HasValue ? txtFechaInicial.SelectedDate.Value : new DateTime(1999,01,01);
            DateTime fechaHasta = txtFechaFinal.SelectedDate.HasValue ? txtFechaFinal.SelectedDate.Value : new DateTime(2999,01,01);

            if (cboConsultores.SelectedValue != "-1")
            {
                long idcliente = long.Parse(cboConsultores.SelectedValue);

                cabeceras = (from C in dc.CabeceraPedidos
                             where C.Cliente == idcliente
                             && C.TipoPedido == "NP" && (!C.NroImpresion.HasValue || C.NroImpresion == 0)
                             && (!C.EsTemporal.HasValue || C.EsTemporal == false)
                             && (C.FechaPedido >= fechaDesde && C.FechaPedido <= fechaHasta)
                             select C).Take(20).ToList<CabeceraPedido>();

            }
            else
            {
                
                cabeceras = (from C in dc.CabeceraPedidos
                             where idsClientes.Contains(C.Cliente)
                             && C.TipoPedido == "NP" && (!C.NroImpresion.HasValue || C.NroImpresion == 0)
                             && (!C.EsTemporal.HasValue || C.EsTemporal == false)
                             && (C.FechaPedido >= fechaDesde && C.FechaPedido <= fechaHasta)
                             select C).Take(20).ToList<CabeceraPedido>();
            }

        }
        else
        {
           
            cabeceras = (from C in dc.CabeceraPedidos
                         where idsClientes.Contains(C.Cliente)
                         && C.Nro == txtNroPedido.Text.Trim()
                         && C.TipoPedido == "NP" && (!C.NroImpresion.HasValue || C.NroImpresion == 0)
                         && (!C.EsTemporal.HasValue || C.EsTemporal == false)
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
    protected void GrillaResultados_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            if (e.Item.DataItem is CabeceraPedido)
            {
                (e.Item.FindControl("btnEliminar") as ImageButton).Attributes.Add("onclick", "return blockConfirm('Esta REALMENTE seguro que desea eliminar el pedido? Tenga en cuenta que no quedará registro alguno del pedido.', event, 330, 100,'','Eliminar Pedido');");
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
