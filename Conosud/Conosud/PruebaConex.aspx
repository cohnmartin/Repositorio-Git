<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PruebaConex.aspx.cs" Inherits="PruebaConex" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="ControlsAjaxNotti" Namespace="ControlsAjaxNotti" TagPrefix="ControlsAjax" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <style>
        .Theader
        {
            background-position: 0 -7550px;
            border-top: 0;
            background-repeat: repeat;
            border-style: solid;
            border-width: 1px;
            border-color: #e98879 #cd6a3f #71250a #872b07;
            color: white;
            font-family: "segoe ui" ,arial,sans-serif;
            font-size: 12px;
            height: 23px;
            text-align: center;
            padding: 0px 0px 0px 0px;
            line-height: 1em;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" AsyncPostBackTimeout="360000"
        EnablePageMethods="true" EnablePartialRendering="true">
        <Scripts>
            <asp:ScriptReference Path="~/FuncionesComunes.js" />
            <asp:ScriptReference Path="~/Scripts/jquery-1.3.1.js" />
            <asp:ScriptReference Path="~/Scripts/jquery.autocomplete.js" />
        </Scripts>
    </telerik:RadScriptManager>
    <ControlsAjax:ClientControlGrid ID="gridDoc" runat="server" AllowMultiSelection="false"
        TypeSkin="Sunset" PositionAdd="Botton" AllowRowSelection="true" Width="95%" KeyName="IdHoja"
        AllowPaging="false" PageSize="50" EmptyMessage="Debe Seleccionar la empresa, contrato y periodo para ver los datos">
        <FunctionsGral>
            <ControlsAjax:FunctionGral ClickFunction="AplicarCambios" Text="Guardar Cambios"
                Type="Custom" ImgUrl="images/notepad.gif" />
        </FunctionsGral>
        <FunctionsColumns>
            <ControlsAjax:FunctionColumnRow Type="Edit" Text="Editar Datos" ClickFunction="InitEdit" />
        </FunctionsColumns>
        <Columns>
            <ControlsAjax:Column HeaderName="Titulo" DataFieldName="Titulo" Align="Derecha" Width="55%" />
            <ControlsAjax:Column HeaderName="Fecha Recepción" DataFieldName="FechaEntrega" Align="Centrado"
                AllowClientChange="true" Display="true" NameControlManger="txtFecha" />
            <ControlsAjax:Column HeaderName="Comentario" DataFieldName="Comentario" Align="Centrado"
                ImgUrl="images/notepad_16x16.gif" Display="true" NameControlManger="txtComentario" />
            <ControlsAjax:Column HeaderName="Presento" DataFieldName="Presento" Align="Centrado"
                Enabled="true" onClientClick="CheckPresentoChange" Display="true" />
            <ControlsAjax:Column HeaderName="FechaEntregaOriginal" DataFieldName="FechaEntregaOriginal"
                Display="false" />
            <ControlsAjax:Column HeaderName="Imagen" DataFieldToolTip="EstadoMail" Align="Centrado"
                DataFieldName="ImagenTemp" DataType="UrlImage" Display="true" />
            <ControlsAjax:Column HeaderName="Imagen" Align="Centrado" DataFieldName="ValorDecimal"
                DataType="Decimal" Display="true" />
            <ControlsAjax:Column HeaderName="Imagen" Align="Centrado" DataFieldName="ValorEntero"
                DataType="Integer" Display="true" />
        </Columns>
    </ControlsAjax:ClientControlGrid>
    <asp:Button ID="btnConectar" runat="server" OnClick="btnConectar_Click" Text="Conectar"
        Width="87px" OnClientClick="CleanTable(); return false;" />
    <asp:TextBox ID="lblCadena" runat="server" Height="52px" TextMode="MultiLine" Width="334px">data source=localhost;integrated security=SSPI;initial 
    catalog=w1440327_Encuestas</asp:TextBox>
    <asp:Label ID="lblError" runat="server" ForeColor="#FF3300"></asp:Label>

    <table id="TABLE1" cellspacing="0" cellpadding="0" width="100%">
        <thead>
            <tr>
                <td class="tdEdit Theader">
                    &nbsp;
                </td>
                <td style="width: 55%" class="Theader" totalizar="False">
                    Titulo
                </td>
                <td class="Theader" totalizar="False">
                    Fecha Recepción
                </td>
                <td class="Theader" totalizar="False">
                    Comentario
                </td>
                <td class="Theader" totalizar="False">
                    Presento
                </td>
                <td class="Theader" totalizar="False">
                    Imagen
                </td>
                <td class="Theader" totalizar="False">
                    Imagen
                </td>
                <td class="Theader" totalizar="False">
                    Imagen
                </td>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td style="background-color: white; padding-left: 8px; width: 100%; font: 12px/25px 'segoe ui' ,arial,sans-serif"
                    align="left" colspan="9">
                    <span                        style="position: relative; padding-left: 0px; color: black; top: -3px">Debe Seleccionar
                        la empresa, contrato y periodo para ver los datos</span>
                </td>
            </tr>
        </tbody>
        <tfoot>
            <tr>
                <td class="tdFunctionAdd" colspan="8">
                  &nbsp;
                </td>
            </tr>
        </tfoot>
    </table>
    </form>
</body>
<script type="text/javascript">
    function CleanTable() {

        $find("<%=gridDoc.ClientID %>")._createTableEmpty();
    }
    
</script>
</html>
