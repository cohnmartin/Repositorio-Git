<%@ Page Language="C#" Theme="MiTema" AutoEventWireup="true" CodeFile="CambioDeEncuadre.aspx.cs"
    Inherits="CambioDeEncuadre" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cambio de Encuadre</title>
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
        font-size: 11px; height: 350%" width="100%">
        <tr>
            <td align="center" colspan="2">
                Por Favor Selecciones el nuevo encuadre que desea asignar
            </td>
        </tr>
        <tr>
            <td align="right" valign="middle" style="width:60%">
                <asp:Label ID="lblLegajo" runat="server" Font-Bold="True" ForeColor="Maroon" Font-Size="18px"
                    Text="Encuadre Actual:"></asp:Label>
            </td>
            <td align="left">
                <asp:Label ID="lblEncuadreActual" runat="server" Font-Bold="True" ForeColor="Black"
                     Font-Size="18px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:EntityDataSource ID="EntityDataSourceConvenio" runat="server" ConnectionString="name=EntidadesConosud" ContextTypeName="Entidades.EntidadesConosud" 
                    DefaultContainerName="EntidadesConosud" EntitySetName="Clasificacion" Select="it.[IdClasificacion], it.[Descripcion]"
                    Where="it.Tipo == @Tipo">
                    <WhereParameters>
                        <asp:Parameter DefaultValue="Convenio" Name="Tipo" DbType="String" />
                    </WhereParameters>
                </asp:EntityDataSource>
                <telerik:RadComboBox ID="cboEncuadre" runat="server" Skin="Sunset" Width="250px"
                    DataSourceID="EntityDataSourceConvenio" DataValueField="IdClasificacion" DataTextField="Descripcion"
                    AppendDataBoundItems="true">
                    <Items>
                        <telerik:RadComboBoxItem Text="" Value="0" />
                    </Items>
                </telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:UpdatePanel ID="upAsignar" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnCambiar" runat="server" SkinID="btnConosudBasic" Mensaje="Cambiando Encuadre..."
                            Text="Cambiar" OnClientClick="return blockConfirm('Esta seguro de asignar el nuevo encuadre?', event, 330, 100,'','Cambio Encuadre');"
                            OnClick="btnCambiar_Click" />
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
