<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TotalizadorPromos.ascx.cs"
    Inherits="TotalizadorPromos" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="ProductoRegalo.ascx" TagName="ProductoRegalo" TagPrefix="uc1" %>
<%@ Register Assembly="ControlsAjaxNotti" Namespace="ControlsAjaxNotti" TagPrefix="cc6" %>
<script type="text/javascript">
    function Cerrar() {
        var animBehavior = $find("SeleccionProducto");
        animBehavior.hide();


    }
    function CloseActiveToolTip() {
        setTimeout(function () {
            var controller = Telerik.Web.UI.RadToolTipController.getInstance();
            var tooltip = controller.get_activeToolTip();
            if (tooltip) tooltip.hide();
        }, 1000);
    }

    function TotalizadorPromos_ControlProductosRequeridos() {
        //debugger;
        var grillaPromos = $find("<%= grillaPromosGanadas.ClientID %>");

        if (grillaPromos != null) {

            if (grillaPromos.get_masterTableView() != null) {
                var itemsGrilla = grillaPromos.get_masterTableView().get_dataItems();

                for (var i = 0; i < itemsGrilla.length; i++) {
                    var TotalRequeridos = itemsGrilla[i].get_element().getAttribute("TotalProductosRequeridos");
                    
                    //alert($('#' + itemsGrilla[i].get_element().id + ' .classRequeridos'));
                    var TotalRequeridosIngresados = $('#' + itemsGrilla[i].get_element().id + ' .classRequeridos').attr("TotalRequeridosIngresados");
                    //alert("Este Cartel: " + TotalRequeridos);
                    //alert("Este Cartel: " + TotalRequeridosIngresados);

                    if (TotalRequeridos != undefined &&
                        TotalRequeridos != null &&
                        TotalRequeridos > 0 &&
                        parseInt(TotalRequeridos) != parseInt(TotalRequeridosIngresados))
                        return true;
                }
            }
        }

        return false;
    }


    var dataListName = "";
    var idDetallePedidoPromo = "";
    function MostrarComponentesPromocion(sender, DataListName, IdDetallePedidoPromo) {

        dataListName = DataListName;
        idDetallePedidoPromo = IdDetallePedidoPromo;
        WebServiceHelper.WebServiceHelper_GenerarTablaProductosRequeridos(idDetallePedidoPromo, UpdateDataList);
    }


    function UpdateDataList(result) {

        /// Recupero los datos de la tabla que se tiene que mostrar
        var datos = result.split('|')[1];

        $get("divPrincipalSeleccion").innerHTML = datos;
        var altoPromoedio = 0;

        /// Calculo el alto de la ventana para mostrar
        for (var i = 0; i < $(datos).length; i++) {
            altoPromoedio += $(datos)[i].rows.length * 30;
        }

        if (altoPromoedio < 480)
            $("#divPrincipalSeleccion").css("height", altoPromoedio + "px");
        else
            $("#divPrincipalSeleccion").css("height", "510px");


        $find("<%=ServerControlSeleccionProducto.ClientID %>").set_CollectionDiv('divPrincipalSeleccion');
        $find("<%=ServerControlSeleccionProducto.ClientID %>").ShowWindows('divPrincipalSeleccion', "Selección Productos Requeridos");

        // Actualizo el estado de los controles para la seleccion 
        // de las cantidades
        var grupos = result.split('|')[0].split('@');
        for (var i = 0; i < grupos.length; i++) {
            if (grupos[i] != "")
                ControlarSeleccionProductos(grupos[i], "");
        }
    }

    function FinalizarSeleccion() {

        var seleccionTotal = "";
        $("#divPrincipalSeleccion input").each(function () {

            if (this.value > 0) {
                var idProducto = this.getAttribute("idProducto");
                var idPresentacion = this.getAttribute("idPresentacion");
                var cantidad = this.value;
                var descripcion = $("#" + this.id.replace("inp", "lbl")).text();
                var precio = this.getAttribute("precio");
                var codigo = this.getAttribute("codigo");

                seleccionTotal += idProducto + "@" + idPresentacion + "@" + cantidad + "@" + descripcion + "@" + precio + "@" + codigo + "|";
            }
        });

        WebServiceHelper.WebServiceHelper_AsociarProductosRequeridos(idDetallePedidoPromo, seleccionTotal, UpdateProductosSeleccionados);

    }

    function UpdateProductosSeleccionados(result) {
        var datos = result.split('@')[0].split('|');
        var totalIngresado = result.split('@')[1];

        //var datos = result.split('|');
        var cantEliminar = $get(dataListName).rows.length;

        for (var i = 0; i < cantEliminar; i++) {
            $get(dataListName).deleteRow(0);
        }

        $get(dataListName).setAttribute("TotalRequeridosIngresados", totalIngresado);

        for (var i = 0; i < datos.length; i++) {
            var row = $get(dataListName).insertRow(0);
            var cell = row.insertCell(0);
            cell.innerHTML = datos[i];
        }

        $find("<%=ServerControlSeleccionProducto.ClientID %>").CloseWindows();
    }
</script>
<style type="text/css">
    .wrapper_ul
    {
        list-style-type: none;
        text-align: center;
        margin: 0;
        padding: 0;
    }
    .wrapper_li_opotunidad
    {
        background-color: WhiteSmoke;
        width: 40%;
        display: list-item;
        margin: 1px;
        border: 1px solid #e3d7c0 !important;
    }
</style>
<table width="100%" border="0" style="height: 100%" id="tblPromos">
    <tr runat="server" id="rowOportunidades" visible="false">
        <td>
            <ul class="wrapper_ul">
                <asp:Repeater ID="repeaperOportunidades" runat="server" OnItemDataBound="repeaperOportunidades_ItemDataBound">
                    <ItemTemplate>
                        <li class="wrapper_li_opotunidad">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0" style="filter: progid:dximagetransform.microsoft.gradient(gradienttype=0,startcolorstr=Red,endcolorstr=WhiteSmoke);
                                background-color: Transparent; font-family: Sans-Serif; font-size: 11px">
                                <tr>
                                    <td align="center" valign="middle" style="height: 20px; font-weight: bold; font-family: Sans-Serif;
                                        font-size: 11px; color: White; font-weight: bold; background-color: Transparent">
                                        <asp:Label ID="lblPromo" runat="server" Text="NO LO DEJES PASAR!!"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="font-family: Sans-Serif; font-size: 12px; color: WhiteSmoke;
                                        font-weight: bold">
                                        <asp:Label ID="lbl12" runat="server" Text="AGREGANDO A TU PEDIDO:"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="font-family: Sans-Serif; font-size: 12px; color: Black;">
                                        <asp:Label ID="lblProductoFaltante" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="font-family: Sans-Serif; font-size: 12px; color: White;
                                        font-weight: bold">
                                        <asp:Label ID="Label1" runat="server" Text="ACCEDERAS A:"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="font-family: Comic Sans MS; font-size: 12px; color: Black;">
                                        <asp:Label ID="Label6" runat="server"><%# Eval("Promo.Descripcion")%></asp:Label>
                                    </td>
                                </tr>
                                <tr valign="middle">
                                    <td align="center" valign="middle" style="font-family: Comic Sans MS; font-size: 12px;
                                        color: Black; height: 100%">
                                        <div style="height: 100%; vertical-align: middle; width: 100%">
                                            <asp:Label ID="Label2" runat="server" Text="QUE TE REGALA!" Style="font-family: Sans-Serif;
                                                font-size: 12px; color: WhiteSmoke; font-weight: bold"></asp:Label>
                                            <br />
                                            <asp:Label ID="lblRegaloOpertunidad1" runat="server" Text="" Visible="false" Width="300px"
                                                Style="padding-bottom: 3px"></asp:Label>
                                            <asp:Label ID="lblRegaloOpertunidad2" runat="server" Text="" Visible="false" Width="300px"
                                                Style="padding-bottom: 3px"></asp:Label>
                                            <asp:Label ID="lblRegaloOpertunidad3" runat="server" Text="" Visible="false" Width="300px"
                                                Style="padding-bottom: 3px; width: 100%"></asp:Label>
                                            <asp:Label ID="lblRegaloOpertunidad4" runat="server" Text="" Visible="false" Width="300px"
                                                Style="padding-bottom: 3px; width: 100%"></asp:Label>
                                            <asp:Label ID="lblRegaloOpertunidad5" runat="server" Text="" Visible="false" Width="300px"
                                                Style="padding-bottom: 3px; width: 100%"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </td>
    </tr>
    <tr>
        <td>
            <asp:UpdatePanel ID="upPromosGanada" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <telerik:RadGrid ID="grillaPromosGanadas" runat="server" AllowPaging="False" Width="90%"
                        Font-Size="11px" Font-Names="Segoe UI,tahoma,verdana,sans-serif" GridLines="None"
                        Skin="Vista" OnItemDataBound="grillaPromosGanadas_ItemDataBound">
                        <MasterTableView ShowFooter="false" GroupLoadMode="Client" ShowGroupFooter="true"
                            AutoGenerateColumns="False" ShowHeader="false" CommandItemDisplay="None" GroupHeaderItemStyle-HorizontalAlign="Left"
                            DataKeyNames="Producto" NoDetailRecordsText="" NoMasterRecordsText="">
                            <GroupByExpressions>
                                <telerik:GridGroupByExpression>
                                    <SelectFields>
                                        <telerik:GridGroupByField FieldName="ProductoDesc" HeaderText=" " />
                                    </SelectFields>
                                    <GroupByFields>
                                        <telerik:GridGroupByField FieldName="ProductoDesc" SortOrder="Descending" />
                                    </GroupByFields>
                                </telerik:GridGroupByExpression>
                            </GroupByExpressions>
                            <RowIndicatorColumn CurrentFilterFunction="NoFilter" FilterListOptions="VaryByDataType"
                                Visible="False">
                                <HeaderStyle Width="20px" />
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn CurrentFilterFunction="NoFilter" FilterListOptions="VaryByDataType"
                                Resizable="False" Visible="False">
                                <HeaderStyle Width="20px" />
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridTemplateColumn UniqueName="Template1" HeaderText="Producto">
                                    <ItemTemplate>
                                        <table width="60%" border="0" cellpadding="0" cellspacing="0" style="background-color: WhiteSmoke;
                                            font-family: Sans-Serif; font-size: 11px">
                                            <tr>
                                                <td align="center" style="font-family: Sans-Serif; font-size: 12px; color: Blue;
                                                    font-weight: bold">
                                                    <asp:Label ID="lblDescComprado" runat="server" Text="Por haber comprado:"></asp:Label>
                                                    <asp:Label ID="lblIdPromo" runat="server" Text='<%# Eval("Producto")%>' Visible="false"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:DataList ID="DataList1" class="classRequeridos" runat="server" Width="100%"
                                                        Height="40px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label7" runat="server"><%# Eval("Descripcion")%></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                    <asp:Button ID="btnSeleccionarProductosPromo" runat="server" Text="Seleccionar" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" style="font-family: Sans-Serif; font-size: 12px; color: Red; font-weight: bold">
                                                    Te Obsequiamos:
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:DataList ID="ListRegalos" runat="server" Width="100%" Height="40px" OnItemDataBound="ListRegalos_ItemDataBound">
                                                        <ItemTemplate>
                                                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Label ID="lblGrupo" runat="server" Text='<%# Eval("Grupo")%>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblNroRegalo" runat="server" Style="font-weight: bold"></asp:Label>
                                                                        <asp:Label ID="Label7" runat="server"><%# Eval("DescripcionRegalo")%></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trProductoEleccion" runat="server">
                                                                    <td>
                                                                        Producto a Elección:&nbsp;
                                                                        <asp:Label Font-Bold="true" Font-Size="10px" Font-Names="Tahoma" ID="lblProdRegalo"
                                                                            runat="server" Text=""></asp:Label>
                                                                        <asp:Button Height="15px" ID="btnSeleccionarProducto" runat="server" Text="..." />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                </td>
                                            </tr>
                                            <!-- Row de Regalos 1 -->
                                            <tr id="trEleccionDesc" runat="server" style="display: none">
                                                <td align="center">
                                                    <asp:Label ID="Label6" runat="server">1º <%# Eval("RegaloDesc")%></asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="trEleccion" runat="server" style="display: none">
                                                <td>
                                                    Fragancia a elección:&nbsp;&nbsp;<asp:Label Font-Bold="true" Font-Size="10px" Font-Names="Tahoma"
                                                        ID="lblProdRegalo" runat="server" Text=""></asp:Label>
                                                    <asp:Button Height="15px" ID="btnSeleccionarProducto" runat="server" Text="..." />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="100%" />
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings>
                        </ClientSettings>
                    </telerik:RadGrid>
                    <input id="DetallePromoHiden" type="hidden" runat="server" />
                    <input id="PromosCompletasHiden" type="hidden" runat="server" value="true" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
<cc6:ServerControlWindow ID="ServerControlSeleccionProducto" runat="server" BackColor="WhiteSmoke"
    WindowColor="Azul">
    <ContentControls>
        <div id="divPrincipalSeleccion" style="height: 520px; width: 720px; overflow: auto">
        </div>
        <div id="div1" style="text-align: center; padding-top: 5px">
            <asp:Button SkinID="btnGray" ID="btnSeleccion" Text="Finalizar Selección" runat="server"
                OnClientClick="FinalizarSeleccion();return  false;" />
        </div>
    </ContentControls>
</cc6:ServerControlWindow>
<telerik:RadToolTipManager runat="server" ID="RadToolTipManager1" Position="Center"
    RelativeTo="Element" Width="320px" ManualClose="true" Height="200px" ShowEvent="OnClick"
    Animation="Resize" Sticky="true" Skin="Vista" ToolTipZoneID="tblPromos" OnAjaxUpdate="OnAjaxUpdate">
</telerik:RadToolTipManager>
