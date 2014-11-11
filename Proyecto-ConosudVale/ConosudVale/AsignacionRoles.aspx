﻿<%@ Page Language="C#" Theme="MiTema" AutoEventWireup="true" CodeFile="AsignacionRoles.aspx.cs" Inherits="AsignacionRoles" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" namespace="System.Web.UI.WebControls" tagprefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="App_Themes/MiTema/StyleSheet.css" rel="stylesheet" type="text/css" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7">
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
                Por Favor Selecciones los roles que tendrá el usuario
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="upGrilla" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <telerik:RadGrid ID="gvRoles" runat="server" 
                            AutoGenerateColumns="false" GridLines="None"
                            Skin="Sunset" Height="300px" Width="535px" 
                            onitemdatabound="gvRoles_ItemDataBound" >
                            
                            <MasterTableView DataKeyNames="IdSegRol" ShowHeadersWhenNoRecords="true"
                             ShowHeader ="true"  ShowFooter= "false" TableLayout="Fixed">
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
                                        <ItemStyle Wrap="false" HorizontalAlign="Center" Width="25px" />
                                        <HeaderStyle HorizontalAlign="Center" Width="25px" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="Descripcion" HeaderText="Opcion Menu" UniqueName="MenuColumn"
                                        ReadOnly="true">
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings>
                                <Selecting AllowRowSelect="true"  />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="99.9%" SaveScrollPosition="false" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </ContentTemplate>
                  </asp:UpdatePanel>
                
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="upAsignar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnAsignar" runat="server" CommandName="Asignar" SkinID="btnConosudBasic"
                            Mensaje="Aplicando Cambios del Usuario..." Text="Aplicar" OnClick="btnAsignar_Click" />
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
