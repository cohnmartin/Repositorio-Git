<%@ Page Title="" Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="GestionPublicacion.aspx.cs" Inherits="GestionPublicacion" %>

<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=3.1.9.807, Culture=neutral, PublicKeyToken=a9d7983dfcc261be"
    Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">

        function DeSelectingRow(sender, eventArgs) {

            var dataItem = $get(eventArgs.get_id());
            var grid = sender;
            var MasterTable = grid.get_masterTableView();
            var row = MasterTable.get_dataItems()[eventArgs.get_itemIndexHierarchical()];
            var cell = MasterTable.getCellByColumnUniqueName(row, "publicadaColumn");
            var chk = MasterTable.getCellByColumnUniqueName(row, "ClientSelectColumn").all[0].checked;

            if (cell.innerText == "Si" && chk)
                eventArgs.set_cancel(true);
            else if (cell.innerText == "No" && chk)
                eventArgs.set_cancel(true);

        }

    </script>
    <table cellpadding="0" cellspacing="5" style="width: 80%">
        <tr>
            <td align="center" style="height: 25px; background: url('images/sprite.gif') 0  -997px repeat-x">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="14pt" Font-Names="Sans-Serif"
                    ForeColor="#E0D6BE" Text="Hojas de Ruta con Documentación Recepcionada Durante el "
                    Width="568px"></asp:Label>
            </td>
        </tr>
    </table>
    <table id="Table1" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
        border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #E0D6BE;
        font-family: Sans-Serif; font-size: 11px;">
        <tr>
            <td align="right" style="width: 98px; height: 26px">
                <asp:Label ID="lblEmpresa" runat="server" Font-Bold="True" ForeColor="Maroon" Height="22px"
                    Text="Mes:" Width="79px"></asp:Label>
            </td>
            <td id="Td1" align="left" style="width: 55px; height: 26px">
                <telerik:RadNumericTextBox ID="txtMes" runat="server" MinValue="1" MaxValue="12"
                    ShowSpinButtons="true" NumberFormat-DecimalDigits="0" Skin="Sunset" Width="50px"
                    Style="text-align: center">
                </telerik:RadNumericTextBox>
            </td>
            <td align="right" style="width: 50px; height: 26px">
                <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Maroon" Height="22px"
                    Text="Año:" Width="79px"></asp:Label>
            </td>
            <td id="Td2" align="left" style="width: 95px; height: 26px">
                <telerik:RadNumericTextBox ID="txtAño" runat="server" MinValue="2000" MaxValue="2050"
                    ShowSpinButtons="true" NumberFormat-DecimalDigits="0" Skin="Sunset" Width="75px"
                    Style="text-align: center" NumberFormat-GroupSeparator="" MaxLength="4">
                </telerik:RadNumericTextBox>
            </td>
            <td id="Td3" align="left" style="width: 95px; height: 26px">
                <asp:CheckBox ID="chkPeriodo" Text="Incluir Todas" Checked="false" runat="server" />
            </td>
            <td valign="middle" align="left">
                <asp:Button ID="btnBuscar" runat="server" CommandName="Buscar" SkinID="btnConosudBasic"
                    Text="Buscar" Mensaje="Buscando hojas de ruta..." OnClick="btnBuscar_Click" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="upResultado" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="border-right: #843431 thin solid; border-top: #843431 thin solid; border-left: #843431 thin solid;
                border-bottom: #843431 thin solid; background-color: #E0D6BE; font-family: Sans-Serif;
                font-size: 11px;" width="98%">
                <tr id="trReporte" runat="server">
                    <td align="center" valign="top">
                        <telerik:RadGrid ID="gvCabeceras" runat="server" Skin="Sunset" AutoGenerateColumns="False"
                            Width="98%" GridLines="None" AllowPaging="false" AllowMultiRowSelection="true"
                            OnItemDataBound="gvCabeceras_ItemDataBound" ToolTip="Solo se muestran las hojas de ruta que tenga documentación recibida en el período de consulta">
                            <MasterTableView ClientDataKeyNames="IdCabeceraHojasDeRuta" DataKeyNames="IdCabeceraHojasDeRuta"
                                ShowFooter="true">
                                <RowIndicatorColumn Visible="False">
                                    <HeaderStyle Width="20px"></HeaderStyle>
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn Visible="False" Resizable="False">
                                    <HeaderStyle Width="20px"></HeaderStyle>
                                </ExpandCollapseColumn>
                                <SortExpressions>
                                    <telerik:GridSortExpression FieldName="ConstratistaParaSubConstratista" SortOrder="Ascending" />
                                </SortExpressions>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="EsContratista" HeaderText="EsConrtatista" UniqueName="EsContratista"
                                        Visible="false">
                                    </telerik:GridBoundColumn>


                                    <telerik:GridTemplateColumn HeaderText="Gral" UniqueName="ImagenAprobacionColumn" >
                                        <ItemTemplate>
                                            <asp:Image ImageUrl="~/Images/ListaParaPublicar.gif" ID="imgAprobacion" runat="server"
                                                Style="cursor: hand;" ToolTip="Hoja apta para ser aprobada" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="30px" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Font-Bold="true" Width="30px" />
                                    </telerik:GridTemplateColumn>


                                    <telerik:GridBoundColumn DataField="ContratoCodigo" SortedBackColor="ActiveBorder"
                                        ShowSortIcon="true" HeaderText="Codigo" UniqueName="Codigo">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ConstratistaParaSubConstratista" HeaderText="Contratista"
                                        UniqueName="Contratista">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn HeaderText="Sub Contratista" UniqueName="SubContratista">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblSubContratista" Text='<%# Eval("EmpresaRazonSocial")%>'> </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="EstadoDescripcion" HeaderText="Estado" UniqueName="Estado"
                                        EmptyDataText="No Aprobada">
                                        <ItemStyle HorizontalAlign="Center" Font-Bold="true" Font-Size="10px" />
                                        <HeaderStyle HorizontalAlign="Center" Width="70px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Periodo" DataFormatString="{0:yyyy/MM}" HeaderText="Periodo"
                                        UniqueName="Periodo">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="40px" HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Publicada" HeaderText="Publicada" UniqueName="publicadaColumn">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" ItemStyle-Width="30px"
                                        HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" />
                                </Columns>
                            </MasterTableView>
                            <ClientSettings>
                                <ClientEvents OnRowDeselecting="DeSelectingRow" />
                                <Selecting AllowRowSelect="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr id="tr1" runat="server">
                    <td align="center" valign="top">
                        <asp:Button ID="btnPublicar" runat="server" SkinID="btnConosudBasic" Text="Guardar"
                            Mensaje="Publicando Hojas de Ruta..." OnClick="btnPublicar_Click" />
                    </td>
                </tr>
                <tr id="trResultadoVacio" runat="server" visible="false">
                    <td align="center" valign="top">
                        <asp:Label ID="Label3" runat="server" Font-Size="13pt" Font-Names="Sans-Serif" ForeColor="white"
                            Text="No existen hojas con documentación recibida en el período seleccionado, que puedan ser publicadas"
                            Width="468px"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="50">
        <ProgressTemplate>
            <div id="divBloq1">
            </div>
            <div class="processMessageTooltip">
                <table border="0" cellpadding="0" cellspacing="0" style="height: 62px;">
                    <tr>
                        <td align="center">
                            <img alt="a" src="Images/LoadingSunset.gif" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <div id="divTituloCarga" style="font-weight: bold; font-family: Tahoma; font-size: 12px;
                                color: White; vertical-align: middle">
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
