<%@ Page Theme="MiTema" Language="C#" AutoEventWireup="true" CodeFile="GestionCopiadoLegajos.aspx.cs" Inherits="GestionCopiadoLegajos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" namespace="System.Web.UI.WebControls" tagprefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Copiado de Legajos</title>
    <link href="App_Themes/MiTema/StyleSheet.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript">
    
    function CloseWindows() {

        var oWnd = GetRadWindow();
        var oArg = new Object();

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
    <table style="border-right: #843431 thin solid; border-top: #843431 thin solid; border-left: #843431 thin solid;
        border-bottom: #843431 thin solid; background-color: #E0D6BE; font-family: Sans-Serif;
        font-size: 11px;height:350%" width="100%"  >
        <tr>
            <td align="center">
                Por Favor seleccione el contrato de donde desea copiar los legajos
            </td>
        </tr>
        <tr>
            <td align="center">
                <table cellpadding="0" cellspacing="5" style="width: 80%">
                    <tr>
                        <td align="center" style="height: 25px; background: url('images/sprite.gif') 0  -997px repeat-x">
                            <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Size="14pt" Font-Names="Sans-Serif"
                                ForeColor="#E0D6BE" Text="Copiado de Legajos" Width="378px"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table id="Table1" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
                    border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #E0D6BE;
                    font-family: Sans-Serif; font-size: 11px;">
                    <tr>
                        <td align="right" style="width: 198px;">
                            <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Maroon" 
                                Text="Contratos Disponibles:" Width="198px"></asp:Label>
                        </td>
                        <td >
                             <telerik:RadComboBox ID="cboContratoSeleccionable" runat="server" Skin="Sunset" Width="200px"
                                AllowCustomText="true" MarkFirstMatch="true" AutoPostBack="true"
                                 EmptyMessage="Contratos Disponibles" Mensaje="Cargando legajos del Contrato..."
                                 onselectedindexchanged="cboContratoSeleccionable_SelectedIndexChanged" ToolTip="Solo se muestran los contratos de la empresa seleccionada y donde el último período es menor al periodo actual seleccionado"  />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
         <tr>
            <td align="center">
                <asp:UpdatePanel ID="upGrilla" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <telerik:RadGrid ID="gvLegajos" runat="server" 
                            AutoGenerateColumns="false" GridLines="None"
                            Skin="Sunset" Height="300px" Width="525px" 
                            AllowMultiRowSelection="true"  >
                            
                            <MasterTableView DataKeyNames="IdLegajos" ShowHeadersWhenNoRecords="true"
                             ShowHeader ="true"  ShowFooter= "false" TableLayout="Fixed" NoMasterRecordsText="No existen legajos para seleccionar">
                                <RowIndicatorColumn Visible="False">
                                    <HeaderStyle Width="20px"></HeaderStyle>
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn Visible="False" Resizable="False">
                                    <HeaderStyle Width="20px"></HeaderStyle>
                                </ExpandCollapseColumn>
                                <Columns>
                                    <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn"   ItemStyle-Width="20px" HeaderStyle-Width="20px" />
                                 
                                    
                                     <telerik:GridBoundColumn DataField="Apellido" HeaderText="Apellido" UniqueName="ApellidoColumn"
                                        ReadOnly="true">
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                    
                                    <telerik:GridBoundColumn DataField="Nombre" HeaderText="Nombre" UniqueName="NombreColumn"
                                        ReadOnly="true">
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                    
                                    <telerik:GridBoundColumn DataField="NroDoc" HeaderText="Documento"
                                        UniqueName="DocumentoColumn" ReadOnly="true">
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
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="cboContratoSeleccionable" EventName="SelectedIndexChanged" />
                    </Triggers>
                  </asp:UpdatePanel>
                
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="upAsignar" runat="server"  UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnCopiado" runat="server" Enabled="false" 
                            SkinID="btnConosudBasic" Mensaje="Copiando legajos..."
                            Text="Copiar Legajos" onclick="btnCopiado_Click"  />
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
