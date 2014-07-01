<%@ Page Language="C#" Theme="SkinMarzzan" AutoEventWireup="true" CodeFile="GestionPedidosTemporales.aspx.cs" Inherits="GestionPedidosTemporales" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>Pedidos Pendientes Sandra Marzzan</title>
     <link href="Styles.css" type="text/css" rel="stylesheet" />
     <script type="text/javascript" language="javascript">
         function ShowPedido(id) {
             window.open('NotaDePedido.aspx?IdPedido=' + id, 'mywindowsss');
         }
    </script>
    
</head>

<body style="background-image: url(Imagenes/repetido.jpg); margin-top:1px; background-repeat:repeat-x;background-color:White;"  >
    <form id="form1" runat="server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Scripts>
            <asp:ScriptReference Path="FuncionesComunes.js" />
        </Scripts>
    </asp:ScriptManager>
    
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Web20" VisibleTitlebar="true" Title="Atención">
    </telerik:RadWindowManager>
     <table cellpadding="0" cellspacing="0" border="0"  style="height:100%;width:100%">
            <tr>
                <td  >
                      
                      <div class="Header_panelContainerSimple">
                          <div class="CabeceraInicial">
                          </div>
                      </div>
                      <div class="CabeceraContent">
                          <table width="95%" border="0" cellspacing="0" >
                              <tr>
                                  <td  style="width:100px; ">
                                     <asp:Label SkinID="lblBlue" ID="UserNameLabel" runat="server" >Revendedores:</asp:Label>
                                  </td>
                                  <td  align="left">
                                      <telerik:RadComboBox ID="cboConsultores" Runat="server" Skin="WebBlue"
                                      Width="290px" AllowCustomText="true" CloseDropDownOnBlur="true" MarkFirstMatch="true" >
                                                <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                      </telerik:RadComboBox>
                                  </td>
                                  <td>
                                    <asp:Button ID="btnConsultar" runat="server" Text="Consultar" SkinID="btnBasic"
                                          Width="77px" onclick="btnConsultar_Click" Mensaje="Buscando Pedido Pendiente..." />
                                  </td>
                              </tr>
                              <tr>
                                  <td align="center" colspan="3" style="font-weight:bold;" >
                                    <asp:Label SkinID="lblBlue" ID="Label2" runat="server" >PEDIDOS PENDIENTES</asp:Label>
                                  </td>
                              </tr>
                              <tr>
                                  <td style="color: #993300; font-family: Sans-Serif; font-size: 11px" colspan="3">
                                      <asp:UpdatePanel ID="upResultado" runat="server" UpdateMode="Conditional">
                                          <ContentTemplate>
                                              <telerik:RadGrid ID="GrillaTemporales" runat="server" GridLines="None" Skin="Vista"
                                                  Width="100%"   OnItemDataBound="GrillaTemporales_ItemDataBound">
                                                  <MasterTableView AutoGenerateColumns="False" DataKeyNames="IdCabeceraPedido" ClientDataKeyNames="IdCabeceraPedido"
                                                      ShowHeadersWhenNoRecords="true" HierarchyLoadMode="Client" TableLayout="Fixed"
                                                      NoMasterRecordsText="No hay pedidos pendientes">
                                                      <RowIndicatorColumn Visible="False">
                                                          <HeaderStyle Width="20px"></HeaderStyle>
                                                      </RowIndicatorColumn>
                                                      <ExpandCollapseColumn Visible="False" Resizable="False">
                                                          <HeaderStyle Width="20px"></HeaderStyle>
                                                      </ExpandCollapseColumn>
                                                      <DetailTables>
                                                          <telerik:GridTableView ClientDataKeyNames="IdDetallePedido" DataKeyNames="IdDetallePedido"
                                                              HierarchyLoadMode="Client" Width="100%" TableLayout="Fixed" GridLines="Horizontal"
                                                              AutoGenerateColumns="false">
                                                              <ParentTableRelation>
                                                                  <telerik:GridRelationFields DetailKeyField="CabeceraPedido" MasterKeyField="IdCabeceraPedido" />
                                                              </ParentTableRelation>
                                                              <RowIndicatorColumn Visible="False">
                                                                  <HeaderStyle Width="20px" />
                                                              </RowIndicatorColumn>
                                                              <ExpandCollapseColumn Resizable="False" Visible="False">
                                                                  <HeaderStyle Width="20px" />
                                                              </ExpandCollapseColumn>
                                                              <Columns>
                                                                  <telerik:GridBoundColumn DataField="Cantidad" DataType="System.Int64" HeaderText="Cantidad"
                                                                      ReadOnly="True" SortExpression="Cantidad" UniqueName="Cantidad">
                                                                      <ItemStyle Width="50px" />
                                                                      <HeaderStyle Width="50px" />
                                                                  </telerik:GridBoundColumn>
                                                                  <telerik:GridTemplateColumn HeaderText="Producto" UniqueName="ProductoColumn">
                                                                      <ItemTemplate>
                                                                          <%# GenerarEtiqueta( Eval("objProducto.DescripcionCompleta").ToString(), Eval("objPresentacion.Descripcion").ToString(), Eval("objProducto.Descripcion").ToString())%>
                                                                      </ItemTemplate>
                                                                      <HeaderStyle HorizontalAlign="Center" />
                                                                  </telerik:GridTemplateColumn>
                                                                  <telerik:GridBoundColumn DataField="ValorUnitario" DataType="System.Decimal" HeaderText="Valor Unitario"
                                                                      ReadOnly="True" SortExpression="ValorUnitario" UniqueName="ValorUnitario">
                                                                      <ItemStyle Width="75px" HorizontalAlign="Center" />
                                                                      <HeaderStyle Width="75px" />
                                                                  </telerik:GridBoundColumn>
                                                                  <telerik:GridBoundColumn DataField="ValorTotal" DataType="System.Decimal" HeaderText="Valor Total"
                                                                      ReadOnly="True" SortExpression="ValorTotal" UniqueName="ValorTotal">
                                                                      <ItemStyle Width="65px" HorizontalAlign="Center" />
                                                                      <HeaderStyle Width="65px" />
                                                                  </telerik:GridBoundColumn>
                                                              </Columns>
                                                              <SortExpressions>
                                                              </SortExpressions>
                                                              <EditFormSettings>
                                                                  <PopUpSettings ScrollBars="None" />
                                                              </EditFormSettings>
                                                          </telerik:GridTableView>
                                                      </DetailTables>
                                                      <Columns>
                                                          <telerik:GridBoundColumn DataField="Nro" HeaderText="Nro" SortExpression="Nro" UniqueName="Nro">
                                                              <ItemStyle Width="25px" HorizontalAlign="Center" />
                                                              <HeaderStyle Width="25px" />
                                                          </telerik:GridBoundColumn>
                                                          <telerik:GridTemplateColumn HeaderText="Solicitante a:" UniqueName="SolicitanteColumn">
                                                              <ItemTemplate>
                                                                  <asp:Label ID="Label1ff" runat="server" Style="text-transform: capitalize"><%# Eval("objClienteSolicitante.Nombre").ToString().ToLower()%></asp:Label>
                                                              </ItemTemplate>
                                                              <HeaderStyle HorizontalAlign="Center" Width="140px" />
                                                              <ItemStyle Width="140px" />
                                                          </telerik:GridTemplateColumn>
                                                          <telerik:GridTemplateColumn HeaderText="Revendedor" UniqueName="ClienteColumn">
                                                              <ItemTemplate>
                                                                  <asp:Label ID="Label1dfd" runat="server" Style="text-transform: capitalize"><%# Eval("objCliente.Nombre").ToString().ToLower()%></asp:Label>
                                                              </ItemTemplate>
                                                              <ItemStyle Width="140px" />
                                                              <HeaderStyle Width="140px" HorizontalAlign="Center" />
                                                          </telerik:GridTemplateColumn>
                                                          <telerik:GridBoundColumn DataField="UltimaModificacion" DataType="System.DateTime" DataFormatString="{0:dd/MM/yyyy HH:mm}"
                                                              HeaderText="Ultima Modificación" SortExpression="FechaPedido" UniqueName="FechaPedido">
                                                              <ItemStyle HorizontalAlign="Center" />
                                                              <HeaderStyle Width="90px" HorizontalAlign="Center" />
                                                          </telerik:GridBoundColumn>
                                                          
                                                          <telerik:GridTemplateColumn HeaderText="Total" UniqueName="ClienteColumn">
                                                              <ItemTemplate>
                                                                  <asp:Label ID="LabelTotalMenosDesc" runat="server" ><%# decimal.Parse(Eval("MontoTotal").ToString()) - decimal.Parse(Eval("DescuentoProvincia").ToString())%></asp:Label>
                                                              </ItemTemplate>
                                                              <HeaderStyle Width="65px" HorizontalAlign="Center" />
                                                              <ItemStyle Width="100%" HorizontalAlign="Center" />
                                                              
                                                          </telerik:GridTemplateColumn>
                                                          
                                                         
                                                          
                                                          
                                                          <telerik:GridTemplateColumn HeaderText="" UniqueName="EditarColumn">
                                                              <ItemTemplate>
                                                                  <img alt="Editarr" style="cursor: hand" src="Imagenes/Edit.gif" onclick="ShowPedido(<%# Eval("IdCabeceraPedido")%>)" />
                                                              </ItemTemplate>
                                                              <HeaderStyle HorizontalAlign="Center" Width="18px" />
                                                              <ItemStyle Width="18px" HorizontalAlign="Center" />
                                                          </telerik:GridTemplateColumn>
                                                          <telerik:GridTemplateColumn UniqueName="Template1" HeaderText="">
                                                              <ItemTemplate>
                                                                  <asp:ImageButton runat="server" ID="btnEliminar" ImageUrl="~/Imagenes/Delete.gif"
                                                                      OnClick="btnEliminar_Click" Mensaje="Eliminando Pedido Pendiente..."  />
                                                              </ItemTemplate>
                                                               <HeaderStyle HorizontalAlign="Center" Width="18px" />
                                                              <ItemStyle Width="18px" HorizontalAlign="Center" />
                                                          </telerik:GridTemplateColumn>
                                                      </Columns>
                                                      <EditFormSettings>
                                                          <PopUpSettings ScrollBars="None"></PopUpSettings>
                                                      </EditFormSettings>
                                                  </MasterTableView>
                                                  <ClientSettings>
                                                      <Selecting AllowRowSelect="True" />
                                                  </ClientSettings>
                                              </telerik:RadGrid>
                                          </ContentTemplate>
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="btnConsultar" EventName="Click" />
                                          </Triggers>
                                      </asp:UpdatePanel>
                                  </td>
                              </tr>
                              <tr>
                                  <td align="center" colspan="3" style="font-weight:bold;" >
                                    <asp:Label SkinID="lblBlue" ID="Label1" runat="server" >PEDIDOS PENDIENTES PAGO TARJETA</asp:Label>
                                  </td>
                              </tr>
                              <tr>
                                  <td style="color: #993300; font-family: Sans-Serif; font-size: 11px" colspan="3">
                                      <asp:UpdatePanel ID="upResultadoTarjeta" runat="server" UpdateMode="Conditional" >
                                          <ContentTemplate>
                                              <telerik:RadGrid ID="GrillaTemporalesTarjeta" runat="server" GridLines="None" Skin="Vista" Width="100%"
                                              OnItemDataBound="GrillaTemporalesTarjeta_ItemDataBound" >
                                                  <MasterTableView AutoGenerateColumns="False" DataKeyNames="IdCabeceraPedido" ClientDataKeyNames="IdCabeceraPedido"
                                                      ShowHeadersWhenNoRecords="true" HierarchyLoadMode="Client" TableLayout="Fixed"
                                                      NoMasterRecordsText="No hay pedidos pendientes con pago con tarjeta">
                                                      <RowIndicatorColumn Visible="False">
                                                          <HeaderStyle Width="20px"></HeaderStyle>
                                                      </RowIndicatorColumn>
                                                      <ExpandCollapseColumn Visible="False" Resizable="False">
                                                          <HeaderStyle Width="20px"></HeaderStyle>
                                                      </ExpandCollapseColumn>
                                                      <DetailTables>
                                                          <telerik:GridTableView ClientDataKeyNames="IdDetallePedido" DataKeyNames="IdDetallePedido"
                                                              HierarchyLoadMode="Client" Width="100%" TableLayout="Fixed" GridLines="Horizontal"
                                                              AutoGenerateColumns="false">
                                                              <ParentTableRelation>
                                                                  <telerik:GridRelationFields DetailKeyField="CabeceraPedido" MasterKeyField="IdCabeceraPedido" />
                                                              </ParentTableRelation>
                                                              <RowIndicatorColumn Visible="False">
                                                                  <HeaderStyle Width="20px" />
                                                              </RowIndicatorColumn>
                                                              <ExpandCollapseColumn Resizable="False" Visible="False">
                                                                  <HeaderStyle Width="20px" />
                                                              </ExpandCollapseColumn>
                                                              <Columns>
                                                                  <telerik:GridBoundColumn DataField="Cantidad" DataType="System.Int64" HeaderText="Cantidad"
                                                                      ReadOnly="True" SortExpression="Cantidad" UniqueName="Cantidad">
                                                                      <ItemStyle Width="50px" />
                                                                      <HeaderStyle Width="50px" />
                                                                  </telerik:GridBoundColumn>
                                                                  <telerik:GridTemplateColumn HeaderText="Producto" UniqueName="ProductoColumn">
                                                                      <ItemTemplate>
                                                                          <%# GenerarEtiqueta( Eval("objProducto.DescripcionCompleta").ToString(), Eval("objPresentacion.Descripcion").ToString(), Eval("objProducto.Descripcion").ToString())%>
                                                                      </ItemTemplate>
                                                                      <HeaderStyle HorizontalAlign="Center" />
                                                                  </telerik:GridTemplateColumn>
                                                                  <telerik:GridBoundColumn DataField="ValorUnitario" DataType="System.Decimal" HeaderText="Valor Unitario"
                                                                      ReadOnly="True" SortExpression="ValorUnitario" UniqueName="ValorUnitario">
                                                                      <ItemStyle Width="75px" HorizontalAlign="Center" />
                                                                      <HeaderStyle Width="75px" />
                                                                  </telerik:GridBoundColumn>
                                                                  <telerik:GridBoundColumn DataField="ValorTotal" DataType="System.Decimal" HeaderText="Valor Total"
                                                                      ReadOnly="True" SortExpression="ValorTotal" UniqueName="ValorTotal">
                                                                      <ItemStyle Width="65px" HorizontalAlign="Center" />
                                                                      <HeaderStyle Width="65px" />
                                                                  </telerik:GridBoundColumn>
                                                              </Columns>
                                                              <SortExpressions>
                                                              </SortExpressions>
                                                              <EditFormSettings>
                                                                  <PopUpSettings ScrollBars="None" />
                                                              </EditFormSettings>
                                                          </telerik:GridTableView>
                                                      </DetailTables>
                                                      <Columns>
                                                          <telerik:GridBoundColumn DataField="Nro" HeaderText="Nro" SortExpression="Nro" UniqueName="Nro">
                                                              <ItemStyle Width="25px" HorizontalAlign="Center" />
                                                              <HeaderStyle Width="25px" />
                                                          </telerik:GridBoundColumn>
                                                          <telerik:GridTemplateColumn HeaderText="Solicitante a:" UniqueName="SolicitanteColumn">
                                                              <ItemTemplate>
                                                                  <asp:Label ID="Label1dfd" runat="server" Style="text-transform: capitalize"><%# Eval("objClienteSolicitante.Nombre").ToString().ToLower()%></asp:Label>
                                                              </ItemTemplate>
                                                              <HeaderStyle HorizontalAlign="Center" Width="140px" />
                                                              <ItemStyle Width="140px" />
                                                          </telerik:GridTemplateColumn>
                                                          <telerik:GridTemplateColumn HeaderText="Revendedor" UniqueName="ClienteColumn">
                                                              <ItemTemplate>
                                                                  <asp:Label ID="Label1df" runat="server" Style="text-transform: capitalize"><%# Eval("objCliente.Nombre").ToString().ToLower()%></asp:Label>
                                                              </ItemTemplate>
                                                              <ItemStyle Width="140px" />
                                                              <HeaderStyle Width="140px" HorizontalAlign="Center" />
                                                          </telerik:GridTemplateColumn>


                                                          <telerik:GridBoundColumn DataField="objTarjeta.Descripcion" 
                                                              HeaderText="Tarjeta" UniqueName="TarjetaColumn">
                                                              <ItemStyle HorizontalAlign="Center" />
                                                              <HeaderStyle Width="90px" HorizontalAlign="Center" />
                                                          </telerik:GridBoundColumn>
                                                          
                                                          <telerik:GridTemplateColumn HeaderText="Total" UniqueName="ClienteColumn">
                                                              <ItemTemplate>
                                                                  <asp:Label ID="LabelTotalMenosDesc" runat="server" ><%# decimal.Parse(Eval("MontoTotal").ToString()) - decimal.Parse(Eval("DescuentoProvincia").ToString())%></asp:Label>
                                                              </ItemTemplate>
                                                              <HeaderStyle Width="40px" HorizontalAlign="Center" />
                                                              <ItemStyle Width="100%" HorizontalAlign="Center" />
                                                          </telerik:GridTemplateColumn>
                                                          
                                                          
                                                          <telerik:GridBoundColumn DataField="objCliente.SaldoCtaCte" DataType="System.Decimal" HeaderText="Saldo Actual"
                                                              UniqueName="SaldoCtaCteColumn" EmptyDataText="0,00">
                                                              <ItemStyle HorizontalAlign="Center" />
                                                              <HeaderStyle Width="55px" HorizontalAlign="Center" />
                                                          </telerik:GridBoundColumn>

                                                          <telerik:GridBoundColumn DataField="EstadoOperacionTarjeta"  HeaderText="Estado"
                                                              UniqueName="EstadoOperacionTarjetaColumn" >
                                                              <ItemStyle HorizontalAlign="Center" />
                                                              <HeaderStyle Width="155px" HorizontalAlign="Center" />
                                                          </telerik:GridBoundColumn>

                                                         
                                                          
                                                          <telerik:GridTemplateColumn HeaderText="" UniqueName="EditarColumn">
                                                              <ItemTemplate>
                                                                  <img alt="Editar Pedido" style="cursor: hand" src="Imagenes/Edit.gif" onclick="ShowPedido(<%# Eval("IdCabeceraPedido")%>)" />
                                                              </ItemTemplate>
                                                              <HeaderStyle HorizontalAlign="Center" Width="18px" />
                                                              <ItemStyle Width="18px" HorizontalAlign="Center" />
                                                          </telerik:GridTemplateColumn>
                                                          <telerik:GridTemplateColumn UniqueName="Template1" HeaderText="">
                                                              <ItemTemplate>
                                                                  <asp:ImageButton runat="server" ID="btnEliminarTarjeta" ImageUrl="~/Imagenes/Delete.gif"
                                                                      OnClick="btnEliminar_Click" Mensaje="Eliminando Pedido Pendientes..."/>
                                                              </ItemTemplate>
                                                               <HeaderStyle HorizontalAlign="Center" Width="18px" />
                                                              <ItemStyle Width="18px" HorizontalAlign="Center" />
                                                          </telerik:GridTemplateColumn>
                                                      </Columns>
                                                      <EditFormSettings>
                                                          <PopUpSettings ScrollBars="None"></PopUpSettings>
                                                      </EditFormSettings>
                                                  </MasterTableView>
                                                  <ClientSettings>
                                                      <Selecting AllowRowSelect="True" />
                                                  </ClientSettings>
                                              </telerik:RadGrid>
                                          </ContentTemplate>
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="btnConsultar" EventName="Click" />
                                          </Triggers>
                                      </asp:UpdatePanel>
                                  </td>
                              </tr>
                              <tr>
                                  <td align="center" colspan="3" style="font-weight:bold;" >
                                    <asp:Label SkinID="lblBlue" ID="Label3" runat="server" >PEDIDOS PENDIENTES POR FALTA DE SALDO</asp:Label>
                                  </td>
                              </tr>
                              <tr>
                                  <td style="color: #993300; font-family: Sans-Serif; font-size: 11px" colspan="3">
                                      <asp:UpdatePanel ID="upResultadoSaldo" runat="server" UpdateMode="Conditional" >
                                          <ContentTemplate>
                                              <telerik:RadGrid ID="GrillaTemporalesSaldo" runat="server" GridLines="None" Skin="Vista" Width="100%"
                                              OnItemDataBound="GrillaTemporalesSaldo_ItemDataBound" >
                                                  <MasterTableView AutoGenerateColumns="False" DataKeyNames="IdCabeceraPedido" ClientDataKeyNames="IdCabeceraPedido"
                                                      ShowHeadersWhenNoRecords="true" HierarchyLoadMode="Client" TableLayout="Fixed"
                                                      NoMasterRecordsText="No hay pedidos pendientes por falta de saldo">
                                                      <RowIndicatorColumn Visible="False">
                                                          <HeaderStyle Width="20px"></HeaderStyle>
                                                      </RowIndicatorColumn>
                                                      <ExpandCollapseColumn Visible="False" Resizable="False">
                                                          <HeaderStyle Width="20px"></HeaderStyle>
                                                      </ExpandCollapseColumn>
                                                      <DetailTables>
                                                          <telerik:GridTableView ClientDataKeyNames="IdDetallePedido" DataKeyNames="IdDetallePedido"
                                                              HierarchyLoadMode="Client" Width="100%" TableLayout="Fixed" GridLines="Horizontal"
                                                              AutoGenerateColumns="false">
                                                              <ParentTableRelation>
                                                                  <telerik:GridRelationFields DetailKeyField="CabeceraPedido" MasterKeyField="IdCabeceraPedido" />
                                                              </ParentTableRelation>
                                                              <RowIndicatorColumn Visible="False">
                                                                  <HeaderStyle Width="20px" />
                                                              </RowIndicatorColumn>
                                                              <ExpandCollapseColumn Resizable="False" Visible="False">
                                                                  <HeaderStyle Width="20px" />
                                                              </ExpandCollapseColumn>
                                                              <Columns>
                                                                  <telerik:GridBoundColumn DataField="Cantidad" DataType="System.Int64" HeaderText="Cantidad"
                                                                      ReadOnly="True" SortExpression="Cantidad" UniqueName="Cantidad">
                                                                      <ItemStyle Width="50px" />
                                                                      <HeaderStyle Width="50px" />
                                                                  </telerik:GridBoundColumn>
                                                                  <telerik:GridTemplateColumn HeaderText="Producto" UniqueName="ProductoColumn">
                                                                      <ItemTemplate>
                                                                          <%# GenerarEtiqueta( Eval("objProducto.DescripcionCompleta").ToString(), Eval("objPresentacion.Descripcion").ToString(), Eval("objProducto.Descripcion").ToString())%>
                                                                      </ItemTemplate>
                                                                      <HeaderStyle HorizontalAlign="Center" />
                                                                  </telerik:GridTemplateColumn>
                                                                  <telerik:GridBoundColumn DataField="ValorUnitario" DataType="System.Decimal" HeaderText="Valor Unitario"
                                                                      ReadOnly="True" SortExpression="ValorUnitario" UniqueName="ValorUnitario">
                                                                      <ItemStyle Width="75px" HorizontalAlign="Center" />
                                                                      <HeaderStyle Width="75px" />
                                                                  </telerik:GridBoundColumn>
                                                                  <telerik:GridBoundColumn DataField="ValorTotal" DataType="System.Decimal" HeaderText="Valor Total"
                                                                      ReadOnly="True" SortExpression="ValorTotal" UniqueName="ValorTotal">
                                                                      <ItemStyle Width="65px" HorizontalAlign="Center" />
                                                                      <HeaderStyle Width="65px" />
                                                                  </telerik:GridBoundColumn>
                                                              </Columns>
                                                              <SortExpressions>
                                                              </SortExpressions>
                                                              <EditFormSettings>
                                                                  <PopUpSettings ScrollBars="None" />
                                                              </EditFormSettings>
                                                          </telerik:GridTableView>
                                                      </DetailTables>
                                                      <Columns>
                                                          <telerik:GridBoundColumn DataField="Nro" HeaderText="Nro" SortExpression="Nro" UniqueName="Nro">
                                                              <ItemStyle Width="25px" HorizontalAlign="Center" />
                                                              <HeaderStyle Width="25px" />
                                                          </telerik:GridBoundColumn>
                                                          <telerik:GridTemplateColumn HeaderText="Solicitante a:" UniqueName="SolicitanteColumn">
                                                              <ItemTemplate>
                                                                  <asp:Label ID="Label1dfd" runat="server" Style="text-transform: capitalize"><%# Eval("objClienteSolicitante.Nombre").ToString().ToLower()%></asp:Label>
                                                              </ItemTemplate>
                                                              <HeaderStyle HorizontalAlign="Center" Width="140px" />
                                                              <ItemStyle Width="140px" />
                                                          </telerik:GridTemplateColumn>
                                                          <telerik:GridTemplateColumn HeaderText="Revendedor" UniqueName="ClienteColumn">
                                                              <ItemTemplate>
                                                                  <asp:Label ID="Label1df" runat="server" Style="text-transform: capitalize"><%# Eval("objCliente.Nombre").ToString().ToLower()%></asp:Label>
                                                              </ItemTemplate>
                                                              <ItemStyle Width="140px" />
                                                              <HeaderStyle Width="140px" HorizontalAlign="Center" />
                                                          </telerik:GridTemplateColumn>
                                                          <telerik:GridBoundColumn DataField="UltimaModificacion" DataType="System.DateTime" DataFormatString="{0:dd/MM/yyyy HH:mm}"
                                                              HeaderText="Ultima Modificación" SortExpression="FechaPedido" UniqueName="FechaPedido">
                                                              <ItemStyle HorizontalAlign="Center" />
                                                              <HeaderStyle Width="90px" HorizontalAlign="Center" />
                                                          </telerik:GridBoundColumn>
                                                          
                                                          <telerik:GridTemplateColumn HeaderText="Total" UniqueName="ClienteColumn">
                                                              <ItemTemplate>
                                                                  <asp:Label ID="LabelTotalMenosDesc" runat="server" ><%# decimal.Parse(Eval("MontoTotal").ToString()) - decimal.Parse(Eval("DescuentoProvincia").ToString())%></asp:Label>
                                                              </ItemTemplate>
                                                              <HeaderStyle Width="40px" HorizontalAlign="Center" />
                                                              <ItemStyle Width="100%" HorizontalAlign="Center" />
                                                          </telerik:GridTemplateColumn>
                                                          
                                                          
                                                          <telerik:GridBoundColumn DataField="objCliente.SaldoCtaCte" DataType="System.Decimal" HeaderText="Saldo Actual"
                                                              UniqueName="SaldoCtaCteColumn" EmptyDataText="0,00">
                                                              <ItemStyle HorizontalAlign="Center" />
                                                              <HeaderStyle Width="55px" HorizontalAlign="Center" />
                                                          </telerik:GridBoundColumn>
                                                          
                                                          <telerik:GridTemplateColumn HeaderText="Estado" UniqueName="EstadoColumn">
                                                              <ItemTemplate>
                                                                  <img id="imgEstado" runat="server" alt="Aun no tiene Saldo" src="Imagenes/mailFlagRed.gif" />
                                                              </ItemTemplate>
                                                              <HeaderStyle HorizontalAlign="Center" Width="18px" />
                                                              <ItemStyle Width="18px" HorizontalAlign="Center" />
                                                          </telerik:GridTemplateColumn>
                                                          
                                                          <telerik:GridTemplateColumn HeaderText="" UniqueName="EditarColumn">
                                                              <ItemTemplate>
                                                                  <img alt="Editar Pedido" style="cursor: hand" src="Imagenes/Edit.gif" onclick="ShowPedido(<%# Eval("IdCabeceraPedido")%>)" />
                                                              </ItemTemplate>
                                                              <HeaderStyle HorizontalAlign="Center" Width="18px" />
                                                              <ItemStyle Width="18px" HorizontalAlign="Center" />
                                                          </telerik:GridTemplateColumn>
                                                          <telerik:GridTemplateColumn UniqueName="Template1" HeaderText="">
                                                              <ItemTemplate>
                                                                  <asp:ImageButton runat="server" ID="btnEliminarSaldo" ImageUrl="~/Imagenes/Delete.gif"
                                                                      OnClick="btnEliminar_Click" Mensaje="Eliminando Pedido Pendientes..."/>
                                                              </ItemTemplate>
                                                               <HeaderStyle HorizontalAlign="Center" Width="18px" />
                                                              <ItemStyle Width="18px" HorizontalAlign="Center" />
                                                          </telerik:GridTemplateColumn>
                                                      </Columns>
                                                      <EditFormSettings>
                                                          <PopUpSettings ScrollBars="None"></PopUpSettings>
                                                      </EditFormSettings>
                                                  </MasterTableView>
                                                  <ClientSettings>
                                                      <Selecting AllowRowSelect="True" />
                                                  </ClientSettings>
                                              </telerik:RadGrid>
                                          </ContentTemplate>
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="btnConsultar" EventName="Click" />
                                          </Triggers>
                                      </asp:UpdatePanel>
                                  </td>
                              </tr>
                          </table>
                       
                    </div>
                </td>
            </tr>
    </table>

     <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="50">
        <ProgressTemplate>
            <div id="divBloq1">
            </div>
            <div class="processMessageTooltipGral">
                <table border="0" cellpadding="0" cellspacing="0" style="height: 62px;">
                    <tr>
                        <td align="center">
                            <img alt="a" src="Imagenes/waiting.gif" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <div id="divTituloCarga" style="font-weight: bold; font-family: Tahoma; font-size: 12px;
                                color: Gray; vertical-align: middle">
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    
    </form>
</body>


</html>
