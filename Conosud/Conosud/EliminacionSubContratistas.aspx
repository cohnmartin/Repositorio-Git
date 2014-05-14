<%@ Page Theme="MiTema" Language="C#" AutoEventWireup="true" CodeFile="EliminacionSubContratistas.aspx.cs"
    Inherits="EliminacionSubContratistas" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Eliminación Sub Contratistas</title>
    <link href="App_Themes/MiTema/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">

        function CloseWindows() {

            var oWnd = GetRadWindow();
            var oArg = new Object();
            oArg.Elimino = true;

            oWnd.argument = oArg;
            oWnd.close();

        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }
    
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Scripts>
            <asp:ScriptReference Path="FuncionesComunes.js" />
        </Scripts>
    </asp:ScriptManager>
    <telerik:RadWindowManager ID="RadWindowManager1" ShowContentDuringLoad="false" VisibleStatusbar="false"
        ReloadOnShow="false" runat="server" Skin="Sunset">
    </telerik:RadWindowManager>
    <table style="border-right: #843431 thin solid; border-top: #843431 thin solid; border-left: #843431 thin solid;
        border-bottom: #843431 thin solid; background-color: #E0D6BE; font-family: Sans-Serif;
        font-size: 11px; height: 350%" width="100%" border="1">
        <tr>
            <td align="center" colspan="2">
                Por Favor Selecciones los sub contratistas que desea eliminar del contrato
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:UpdatePanel ID="upGrilla" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <telerik:RadGrid ID="gvSubContratistas" runat="server" AutoGenerateColumns="false"
                            GridLines="None" Skin="Sunset" Height="300px" Width="525px">
                            <MasterTableView DataKeyNames="IdContratoEmpresas" ShowHeadersWhenNoRecords="true"
                                NoDetailRecordsText="No existen Sub contratistas para poder eliminar" 
                                NoMasterRecordsText="No existen Sub contratistas para poder eliminar"
                                ShowHeader="true" ShowFooter="false" TableLayout="Fixed">
                                <RowIndicatorColumn Visible="False">
                                    <HeaderStyle Width="20px"></HeaderStyle>
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn Visible="False" Resizable="False">
                                    <HeaderStyle Width="20px"></HeaderStyle>
                                </ExpandCollapseColumn>
                                <Columns>
                                    <telerik:GridTemplateColumn>
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkSeleccion" />
                                        </ItemTemplate>
                                        <ItemStyle Wrap="false" HorizontalAlign="Center" Width="15px" />
                                        <HeaderStyle HorizontalAlign="Center" Width="15px" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="Empresa.RazonSocial" HeaderText="Sub Contratista"
                                        UniqueName="SubContratistaColumn" ReadOnly="true">
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Empresa.CUIT" HeaderText="CUIT" UniqueName="CUITColumn"
                                        ReadOnly="true">
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" Width="100px" />
                                        <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings>
                                <Selecting AllowRowSelect="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="99.9%" SaveScrollPosition="false" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Label SkinID="lblConosudNormal" ID="Label4" runat="server" Text="Fecha Baja:"></asp:Label>
                <telerik:RadDatePicker ID="txtFechaBaja" ToolTip="Eliminar a los subcontratistas desde el mes siguiente a la fecha ingresada"
                    MinDate="1950/1/1" runat="server" ZIndex="922000000">
                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x">
                    </Calendar>
                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                    <DateInput InvalidStyleDuration="100">
                    </DateInput>
                </telerik:RadDatePicker>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:UpdatePanel ID="upAsignar" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnEliminar" runat="server" CommandName="Asignar" SkinID="btnConosudBasic"
                            Mensaje="Eliminando Sub Contratistas..." Text="Eliminar" OnClientClick="return blockConfirm('Esta seguro de eliminar las empresas seleccionadas como sub contratista del contrato a partir del mes siguiente al seleccionado?', event, 330, 100,'','Sub Contratistas');"
                            OnClick="btnEliminar_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
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
    </form>
</body>
</html>
