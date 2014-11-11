<%@ Page Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="ConsultaHojasPublicadas.aspx.cs" Inherits="ConsultaHojasPublicadas" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" namespace="System.Web.UI.WebControls" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <style>
        .HandCursor
        {
            cursor: hand;
        }
    </style>

    <script type="text/javascript">
        var contratosCombo = $find("ctl00_ContentPlaceHolder1_cboContratos");

        function LoadContratos(combo, eventarqs) {
            var contratosCombo = $find("ctl00_ContentPlaceHolder1_cboContratos");
            var contratistasCombo = $find("ctl00_ContentPlaceHolder1_cboContratistas");

            var item = eventarqs.get_item();
            contratosCombo.set_text("Loading...");
            contratistasCombo.clearSelection();

            if (item.get_index() > 0) {
                contratosCombo.requestItems(item.get_value(), false);
            }
            else {
                contratosCombo.set_text(" ");
                contratosCombo.clearItems();

                contratistasCombo.set_text(" ");
                contratistasCombo.clearItems();

            }
        }

        function LoadContratistas(combo, eventarqs) {
            var contratistasCombo = $find("ctl00_ContentPlaceHolder1_cboContratistas");

            var item = eventarqs.get_item();

            if (item.get_index() > 0) {
                contratistasCombo.clearSelection();
                contratistasCombo.set_text("Loading...");
                contratistasCombo.requestItems(item.get_value(), false);

            }
            else {
                contratistasCombo.set_text(" ");
                contratistasCombo.clearItems();

            }


        }

        function ItemsLoaded(combo, eventarqs) {
            var contratosCombo = $find("ctl00_ContentPlaceHolder1_cboContratos");

            if (combo.get_items().get_count() > 0) {
                combo.set_text(combo.get_items().getItem(0).get_text());
                combo.get_items().getItem(0).highlight();
            }
            combo.showDropDown();
        }
        
        function ShowReporte() {

            var grid = $find("<%= gvCabeceras.ClientID%>");
            var MasterTable = grid.get_masterTableView();
            var row = MasterTable.get_dataItems()[event.srcElement.parentElement.parentElement.rowIndex - 1];
            var idCabecera = row.getDataKeyValue("IdCabeceraHojasDeRuta");

            window.open('GestionHojadeRuta.aspx?IdCabecera=' + idCabecera, 'mywindowsss', 'width=800,height=600,toolbar=yes, location=yes,directories=yes,status=yes,menubar=yes,scrollbars=yes,copyhistory=yes, resizable=yes');
        }

    </script>

    <table cellpadding="0" cellspacing="5" style="width: 80%">
        <tr>
            <td align="center" style="height: 25px; background: url('images/sprite.gif') 0  -997px repeat-x">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="14pt" Font-Names="Sans-Serif"
                    ForeColor="#E0D6BE" Text="Hojas de Ruta Publicadas" Width="378px"></asp:Label>
            </td>
        </tr>
    </table>
    <table id="Table1" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
        border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #E0D6BE;
        font-family: Sans-Serif; font-size: 11px;">
        <tr>
            <td align="right" style="width: 98px; height: 26px">
                <asp:Label ID="lblEmpresa" runat="server" Font-Bold="True" ForeColor="Maroon" Height="22px"
                    Text="Empresa:" Width="79px"></asp:Label>
            </td>
            <td id="Td1" align="left" style="width: 95px; height: 26px">
                <telerik:RadComboBox ID="cboEmpresas" runat="server" Skin="Sunset" Width="200px"
                    OnClientSelectedIndexChanging="LoadContratos" OnItemsRequested="cboEmpresas_ItemsRequested" />
            </td>
            <td align="right" style="width: 34px">
                <asp:Label ID="lblContr" runat="server" Font-Bold="True" ForeColor="Maroon" Height="22px"
                    Text="Contratos:" Width="85px"></asp:Label>
            </td>
            <td align="left" style="width: 299px">
                <telerik:RadComboBox ID="cboContratos" runat="server" Skin="Sunset" Width="200px"
                    OnItemsRequested="cboContratos_ItemsRequested" OnClientSelectedIndexChanging="LoadContratistas"
                    OnClientItemsRequested="ItemsLoaded" />
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 280px; height: 26px" colspan="2" >
                <asp:Label ID="lblContratistas" runat="server" Font-Bold="True" ForeColor="Maroon"
                    Height="20px" Text="Contratistas:" Width="85px"></asp:Label>
            </td>
            <td align="left" style="width: 200px; height: 26px" colspan="2" >
                <telerik:RadComboBox ID="cboContratistas" runat="server" Skin="Sunset" Width="200px"
                    AutoPostBack="true" Mensaje="Buscando Hojas de Ruta..."
                    OnItemsRequested="cboContratistas_ItemsRequested" OnClientItemsRequested="ItemsLoaded"
                    OnSelectedIndexChanged="cboContratistas_SelectedIndexChanged" />
            </td>
        </tr>
    </table>
    
      <asp:UpdatePanel ID="upResultado" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="border-right: #843431 thin solid; border-top: #843431 thin solid; border-left: #843431 thin solid;
                border-bottom: #843431 thin solid; background-color: #E0D6BE; font-family: Sans-Serif;
                font-size: 11px;"  width="98%">
                <tr id="trReporte" runat="server">
                    <td align="center" valign="top">
                    
                       <telerik:RadGrid ID="gvCabeceras" runat="server" Skin="Sunset" AutoGenerateColumns="False"
                            Width="95%" GridLines="None" AllowPaging="false" AllowMultiRowSelection="true"
                            OnItemDataBound="gvCabeceras_ItemDataBound"  > 
                            
                            <MasterTableView  ClientDataKeyNames="IdCabeceraHojasDeRuta" DataKeyNames="IdCabeceraHojasDeRuta" TableLayout="Fixed" ShowFooter="true"   >
                                <RowIndicatorColumn Visible="False" >
                                    <HeaderStyle Width="20px"></HeaderStyle>
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn Visible="False" Resizable="False">
                                    <HeaderStyle Width="20px"></HeaderStyle>
                                </ExpandCollapseColumn>
                                <Columns>

                                    <telerik:GridBoundColumn DataField="ContratoEmpresas.Contrato.Codigo" HeaderText="Codigo" UniqueName="Codigo">
                                        <ItemStyle HorizontalAlign="Center" />
                                         <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    
                                    
                                    <telerik:GridBoundColumn DataField="Periodo" DataFormatString="{0:yyyy/MM}" HeaderText="Periodo"
                                        UniqueName="Periodo">
                                        <ItemStyle HorizontalAlign="Center" />
                                         <HeaderStyle Width="40px" HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="Aprobada" HeaderText="Estado" UniqueName="Estado" EmptyDataText="No Aprobada">
                                        <ItemStyle HorizontalAlign="Center" Font-Bold="true"  Font-Size="10px" />
                                        <HeaderStyle HorizontalAlign="Center" Width="70" />
                                    </telerik:GridBoundColumn>
                                    
                                    <telerik:GridTemplateColumn HeaderText="Motivo">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblMotivo" > </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="100%" />
                                            <HeaderStyle HorizontalAlign="Center" Width="180" />
                                        </telerik:GridTemplateColumn>
                                        
                                        
                                    <telerik:GridTemplateColumn HeaderText="Ir a Hoja" UniqueName="ReporteColumn">
                                        <ItemTemplate>
                                            <asp:ImageButton Style="cursor: hand; text-align: center" ID="imgSubContratistas"
                                                Mensaje="Generando Reporte.." runat="server" ImageUrl="~/images/notepad_16x16.gif"
                                                OnClientClick="ShowReporte(); return false;" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" Width="45px" />
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings>
                                <Selecting AllowRowSelect="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                      
                      
                    </td>
                </tr>
                <tr id="trResultadoVacio" runat="server" visible="false">
                    <td align="center" valign="top">
                        <asp:Label ID="Label3" runat="server"  Font-Size="13pt" Font-Names="Sans-Serif"
                            ForeColor="white" Text="No existen hojas con documentación recivida en el período seleccionado, que puedan ser puclicadas" Width="468px"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="cboContratistas" EventName="SelectedIndexChanged" />
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

