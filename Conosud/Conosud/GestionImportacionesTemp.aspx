<%@ Page Title="" Language="C#" Theme="MiTema" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="GestionImportacionesTemp.aspx.cs" Inherits="GestionImportacionesTemp" %>

<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=3.1.9.807, Culture=neutral, PublicKeyToken=a9d7983dfcc261be"
    Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellpadding="0" cellspacing="5" style="width: 80%">
        <tr>
            <td align="center" style="height: 25px; background: url('images/sprite.gif') 0  -997px repeat-x">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="14pt" Font-Names="Sans-Serif"
                    ForeColor="#E0D6BE" Text="Gestion de Importaciones Temporales " Width="568px"></asp:Label>
            </td>
        </tr>
    </table>
    <table id="Table1" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
        border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #E0D6BE;
        font-family: Sans-Serif; font-size: 11px;">
        <tr>
            <td align="right" style="height: 26px">
                <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Maroon" Height="22px"
                    Text="Proceso para Actualizar Vehiculos:" ></asp:Label>
            </td>
            <td id="Td2" align="left" style="width: 55px; height: 26px">
                <asp:Button ID="btnActualizar" runat="server" SkinID="btnConosudBasic" Text="Analizar"
                    Mensaje="Publicando Hojas de Ruta..." OnClick="btnActualizar_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="left">
                <div runat="server" id="ResultActualizacion">
                </div>
            </td>
        </tr>

        <tr>
            <td align="right" style="height: 26px">
                <asp:Label ID="lblEmpresa" runat="server" Font-Bold="True" ForeColor="Maroon" Height="22px"
                    Text="Proceso para importar Vehiculos:" ></asp:Label>
            </td>
            <td id="Td1" align="left" style="width: 55px; height: 26px">
                <asp:Button ID="btnImpVehiculos" runat="server" SkinID="btnConosudBasic" Text="Analizar"
                    Mensaje="Publicando Hojas de Ruta..." OnClick="btnImpVehiculos_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="left">
                <div runat="server" id="ResultImportacion">
                </div>
            </td>
        </tr>

        <tr>
            <td align="right" style="height: 26px">
                <asp:Label ID="Label3" runat="server" Font-Bold="True" ForeColor="Maroon" Height="22px"
                    Text="Proceso para Actualizar Vehiculos YPF:" ></asp:Label>
            </td>
            <td id="Td3" align="left" style="width: 55px; height: 26px">
                <asp:Button ID="Button1" runat="server" SkinID="btnConosudBasic" Text="Analizar"
                    Mensaje="Publicando Hojas de Ruta..." OnClick="btnActualizarVehiculos_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="left">
                <div runat="server" id="Div1">
                </div>
            </td>
        </tr>
    </table>

    <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="50">
        <progresstemplate>
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
        </progresstemplate>
    </asp:UpdateProgress>
</asp:Content>
