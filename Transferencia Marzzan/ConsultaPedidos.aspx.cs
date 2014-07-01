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

public partial class ConsultaPedidos : BasePage
{
    protected override void PageLoad()
    {

        if (!IsPostBack)
        {
            txtFechaInicial.SelectedDate = DateTime.Now.AddDays(-7);
            txtFechaFinal.SelectedDate = DateTime.Now;

            Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext();

            if (Request.QueryString["NroPedido"] == null)
            {


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
                    cboConsultores.SelectedIndex = 0;
                    cboConsultores.DataBind();
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
            }
            else
            {
                try
                {
                    long nroPedido = long.Parse(Request.QueryString["NroPedido"]);
                    cboTipoPedido.Enabled = false;
                    cboConsultores.Enabled = false;
                    txtFechaInicial.Enabled = false;
                    txtFechaFinal.Enabled = false;
                    btnConsultar.Enabled = false;

                    var pedidos = (from p in dc.CabeceraPedidos
                                   where p.Nro == Request.QueryString["NroPedido"]
                                   && p.TipoPedido == "NP"
                                   select p).ToList();

                    CabeceraPedido pedido = pedidos.FirstOrDefault();
                    string valoresControl = Helper.DesEncriptar(Request.QueryString["p"]);
                    string codigoCliente = valoresControl.Split('|')[0].ToString();
                    string nroDelPedido = valoresControl.Split('|')[1].ToString();

                    if (pedidos.Count == 0 || codigoCliente != pedido.objClienteSolicitante.CodigoExterno ||
                        nroDelPedido != pedido.Nro)
                    {
                        ScriptManager.RegisterStartupScript(upResultado, typeof(UpdatePanel), "GrabacionPedido", "alert('El link que esta utilizando no es válido.');", true);
                    }
                    else
                    {
                        GrillaResultados.DataSource = pedidos;
                        GrillaResultados.DataBind();
                        upResultado.Update();
                    }
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(upResultado, typeof(UpdatePanel), "GrabacionPedido", "alert('El link que esta utilizando no es válido.');", true);
                }
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



            Cliente clienteCtaBolsos = (from C in dc.Clientes
                                        where C.Dni == cliente.Dni && C.Nombre.Contains("bolsos")
                                        select C).FirstOrDefault<Cliente>();


            if (clienteCtaBolsos != null)
            {
                if (cboTipoPedido.SelectedValue == "0")

                    cabeceras = (from C in dc.CabeceraPedidos
                                 where (C.Cliente == cliente.IdCliente || C.ClienteSolicitante == clienteCtaBolsos.IdCliente)
                                 && (C.FechaPedido.Date >= txtFechaInicial.SelectedDate.Value.Date && C.FechaPedido.Date <= txtFechaFinal.SelectedDate.Value.Date)
                                 && (C.EsTemporal == null || C.EsTemporal.Value == false) && (C.EstadoOperacionTarjeta == "" || C.EstadoOperacionTarjeta == "APROBADA")
                                 select C).ToList<CabeceraPedido>();
                else

                    cabeceras = (from C in dc.CabeceraPedidos
                                 where (C.Cliente == cliente.IdCliente || C.ClienteSolicitante == clienteCtaBolsos.IdCliente)
                                 && (C.FechaPedido.Date >= txtFechaInicial.SelectedDate.Value.Date && C.FechaPedido.Date <= txtFechaFinal.SelectedDate.Value.Date)
                                 && (C.EsTemporal == null || C.EsTemporal.Value == false) && (C.EstadoOperacionTarjeta == "" || C.EstadoOperacionTarjeta == "APROBADA")
                                 && C.TipoPedido == cboTipoPedido.SelectedValue
                                 select C).ToList<CabeceraPedido>();

            }
            else
            {
                if (cboTipoPedido.SelectedValue == "0")
                    cabeceras = (from C in dc.CabeceraPedidos
                                 where C.Cliente == cliente.IdCliente
                                 && (C.FechaPedido.Date >= txtFechaInicial.SelectedDate.Value.Date && C.FechaPedido.Date <= txtFechaFinal.SelectedDate.Value.Date)
                                 && (C.EsTemporal == null || C.EsTemporal.Value == false) && (C.EstadoOperacionTarjeta == "" || C.EstadoOperacionTarjeta == "APROBADA")
                                 select C).ToList<CabeceraPedido>();
                else
                    cabeceras = (from C in dc.CabeceraPedidos
                                 where C.Cliente == cliente.IdCliente
                                 && (C.FechaPedido.Date >= txtFechaInicial.SelectedDate.Value.Date && C.FechaPedido.Date <= txtFechaFinal.SelectedDate.Value.Date)
                                 && (C.EsTemporal == null || C.EsTemporal.Value == false) && (C.EstadoOperacionTarjeta == "" || C.EstadoOperacionTarjeta == "APROBADA")
                                 && C.TipoPedido == cboTipoPedido.SelectedValue
                                 select C).ToList<CabeceraPedido>();


            }
        }
        else
        {
            var cliente = (from C in dc.Clientes
                           where C.IdCliente == long.Parse(Session["IdUsuario"].ToString())
                           orderby C.Nombre
                           select C).Single<Cliente>();



            Cliente clienteCtaBolsos = (from C in dc.Clientes
                                        where C.Dni == cliente.Dni && C.Nombre.Contains("bolsos")
                                        select C).FirstOrDefault<Cliente>();


            List<Cliente> consultores = Helper.ObtenerConsultoresSubordinados((Cliente)cliente);


            List<long> IdsConsultores = (from C in consultores
                                         orderby C.Nombre
                                         select C.IdCliente).ToList<long>();


            /// Si el cliente logeado no posee cuenta bolsos
            /// entoces hago que el cliente de la cuenta bolsos
            /// sea igual al cliente logeado.
            if (clienteCtaBolsos == null)
            {
                clienteCtaBolsos = cliente;
            }


            if (cboTipoPedido.SelectedValue == "0")
                cabeceras = (from C in dc.CabeceraPedidos
                             where (IdsConsultores.Contains(C.Cliente) || C.ClienteSolicitante == clienteCtaBolsos.IdCliente)
                             && (C.FechaPedido.Date >= txtFechaInicial.SelectedDate.Value.Date && C.FechaPedido.Date <= txtFechaFinal.SelectedDate.Value.Date)
                             && (C.EsTemporal == null || C.EsTemporal.Value == false) && (C.EstadoOperacionTarjeta == "" || C.EstadoOperacionTarjeta == "APROBADA")
                             select C).ToList<CabeceraPedido>();
            else
                cabeceras = (from C in dc.CabeceraPedidos
                             where (IdsConsultores.Contains(C.Cliente) || C.ClienteSolicitante == clienteCtaBolsos.IdCliente)
                             && (C.FechaPedido.Date >= txtFechaInicial.SelectedDate.Value.Date && C.FechaPedido.Date <= txtFechaFinal.SelectedDate.Value.Date)
                             && (C.EsTemporal == null || C.EsTemporal.Value == false) && (C.EstadoOperacionTarjeta == "" || C.EstadoOperacionTarjeta == "APROBADA")
                             && C.TipoPedido == cboTipoPedido.SelectedValue
                             select C).ToList<CabeceraPedido>();

        }


        List<long> ids = (from C in cabeceras
                          select C.IdCabeceraPedido).ToList<long>();


        //List<DetallePedido> detalles = (from D in dc.DetallePedidos
        //                                where ids.Contains(D.CabeceraPedido.Value)
        //                                select D).ToList<DetallePedido>();

        //GrillaResultados.MasterTableView.DetailTables[0].DataSource = detalles;
        //GrillaResultados.MasterTableView.DetailTables[0].DataBind();

        GrillaResultados.DataSource = cabeceras;

        GrillaResultados.DataBind();
        upResultado.Update();
    }
    protected void btnVolver_Click(object sender, EventArgs e)
    {
        Response.Redirect("Inicio.aspx");
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
    public string GenerarEstado(object NroImpresion)
    {
        if (int.Parse(NroImpresion.ToString()) > 0)
        {
            return "Procesado";

        }
        else
        {
            return "Sin Procesar";
        }

        //if (estado != null && estado.ToString().ToUpper() == "ENVIADO")
        //{
        //    return "Procesado";

        //}
        //else
        //{
        //    return "Sin Procesar";
        //}

    }
    protected void GrillaResultados_DetailTableDataBind(object source, Telerik.Web.UI.GridDetailTableDataBindEventArgs e)
    {
        Marzzan_InfolegacyDataContext dc = new Marzzan_InfolegacyDataContext();
        GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
        long id = long.Parse(e.DetailTableView.ParentItem.GetDataKeyValue("IdCabeceraPedido").ToString());

        List<DetallePedido> detalles = (from D in dc.DetallePedidos
                                        where D.CabeceraPedido == id
                                        select D).ToList<DetallePedido>();


        e.DetailTableView.DataSource = detalles;
        //upResultado.Update();


    }
    protected void GrillaResultados_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == "ExportCtaCte")
        {
            foreach (Telerik.Web.UI.GridColumn column in GrillaResultados.MasterTableView.Columns)
            {
                if (!column.Visible || !column.Display)
                {
                    column.Visible = true;
                    column.Display = true;
                }
            }

            GrillaResultados.MasterTableView.Columns.FindByUniqueName("ImprimirColumn").Visible = false;

            GrillaResultados.ExportSettings.ExportOnlyData = true;
            GrillaResultados.ExportSettings.IgnorePaging = true;
            GrillaResultados.ExportSettings.FileName = "PedidoRealizados" + "_" + string.Format("{0:ddMM}", txtFechaInicial.SelectedDate) + "_" + string.Format("{0:ddMM}", txtFechaFinal.SelectedDate);
            GrillaResultados.MasterTableView.ExportToExcel();

        }
    }
}
