<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="ReporteAltasBajasLegajos.aspx.cs" Inherits="ReporteAltasBajasLegajos" %>

<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=3.1.9.807, Culture=neutral, PublicKeyToken=a9d7983dfcc261be"
    Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Assembly="ControlsAjaxNotti" Namespace="ControlsAjaxNotti" TagPrefix="AjaxInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript">

            function BuscarDatos() {
                $find("<%=GridAltaBajas.ClientID %>").ShowWaiting("Buscando Recursos...");
                PageMethods.GetData($get("<%=cboPeriodos.ClientID %>").value, 0, $find("<%=GridAltaBajas.ClientID %>").get_pageSize(), FillGrid, Error);

            }


            function FillGrid(result) {
                $find("<%=GridAltaBajas.ClientID %>").set_ClientdataSource(result);
            }

            function Error(err) {
                alert(err.get_message());
            }

            function Agrupar() {

                //  $('#' + "tblDynamic_ctl00_ContentPlaceHolder1_GridAltaBajas").tableGroup({ groupColumn: 5, groupClass: 'rgGroupItem', useNumChars: 0 });

            }

            function UnAgrupar() {

                //  $('#' + "tblDynamic_ctl00_ContentPlaceHolder1_GridAltaBajas").tableUnGroup();

            }
        </script>
    </telerik:RadScriptBlock>
    <table cellpadding="0" cellspacing="5" style="width: 80%">
        <tr>
            <td align="center" style="height: 25px; background: url('images/sprite.gif') 0  -997px repeat-x">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="14pt" Font-Names="Sans-Serif"
                    ForeColor="#E0D6BE" Text="Reporte Altas Bajas Legajos" Width="468px"></asp:Label>
            </td>
        </tr>
    </table>

<%--    Botones para ejemplo de agrupacion de la grilla--%>
    <asp:Button ID="myButton" runat="server" Text="Agrupar Datos" Height="22px" Width="100%"
        Style="display: none; cursor: pointer; border-bottom-width: 0px; border-bottom-style: none;
        background-color: transparent" BorderStyle="None" BorderWidth="0px" Font-Bold="True"
        ForeColor="#005F8C" OnClientClick="Agrupar(); return false;" />
    <asp:Button ID="Button1" runat="server" Text="Des Agrupar Datos" Height="22px" Width="100%"
        Style="display: none; cursor: pointer; border-bottom-width: 0px; border-bottom-style: none;
        background-color: transparent" BorderStyle="None" BorderWidth="0px" Font-Bold="True"
        ForeColor="#005F8C" OnClientClick="UnAgrupar(); return false;" />


    <table id="Table1" style="border-right: #843431 thin solid; width: 180px; border-top: #843431 thin solid;
        border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #E0D6BE;
        font-family: Sans-Serif; font-size: 11px;">
        <tr>
            <td style="width: 309px; height: 26px; text-align: left">
                <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Maroon" Text="Período:"
                    Width="94px"></asp:Label>
            </td>
            <td style="width: 309px; height: 26px; text-align: left">
                <asp:DropDownList ID="cboPeriodos" runat="server" Width="208px" Height="22px">
                </asp:DropDownList>
            </td>
            <td valign="middle" align="left">
                <asp:Button ID="btnBuscar" runat="server" CommandName="Generar" SkinID="btnConosudBasic"
                    Text="Buscar" Mensaje="Generando informe Altas y Bajas..." OnClientClick="BuscarDatos();return false;" />
            </td>
        </tr>
    </table>
    <table style="border-right: #843431 thin solid; border-top: #843431 thin solid; border-left: #843431 thin solid;
        border-bottom: #843431 thin solid; background-color: #E0D6BE; font-family: Sans-Serif;
        font-size: 11px; height: 550px" width="98%">
        <tr id="trReporte" runat="server">
            <td align="center" valign="top">
                <AjaxInfo:ClientControlGrid ID="GridAltaBajas" runat="server" AllowMultiSelection="false"
                    TypeSkin="Sunset" PositionAdd="Top" AllowRowSelection="false" Width="100%" KeyName="UniqueID"
                    AllowPaging="true" PageSize="20" EmptyMessage="Seleccione un pariodo para ver los datos">
                    <FunctionsGral>
                        <AjaxInfo:FunctionGral Type="Excel" Text="Exportar Datos" />
                    </FunctionsGral>
                    <Columns>
                        <AjaxInfo:Column HeaderName="Periodo Consulta" DataFieldName="Periodo" Align="Derecha"
                            ExportToExcel="true" />
                        <AjaxInfo:Column HeaderName="Mov" DataFieldName="Accion" Align="Derecha" ExportToExcel="true" />
                        <AjaxInfo:Column Capitalice="true" HeaderName="Contratista" DataFieldName="NombreEmpresaContratista"
                            Align="Derecha" ExportToExcel="true" />
                        <AjaxInfo:Column Capitalice="true" HeaderName="Sub Contratista" DataFieldName="NombreEmpresaSubContratista"
                            Align="Derecha" ExportToExcel="true" />
                        <AjaxInfo:Column HeaderName="Nº Contrato" DataFieldName="CodigoContrato" Align="Derecha"
                            ExportToExcel="true" />
                        <AjaxInfo:Column Capitalice="false" HeaderName="Nombre" DataFieldName="NombreCompleto"
                            Align="Derecha" ExportToExcel="true" />
                        <AjaxInfo:Column HeaderName="Nro Doc" Totalizar="true" DataFieldName="Nrodoc" Align="Centrado"
                            ExportToExcel="true" />
                        <AjaxInfo:Column Align="Derecha" HeaderName="Encuadre" DataFieldName="Encuadre" ExportToExcel="true" />
                        <AjaxInfo:Column Align="Centrado" HeaderName="Fecha Tramite" DataFieldName="FechaTramite"
                            ExportToExcel="true" />
                        <AjaxInfo:Column Align="Centrado" HeaderName="Fecha Baja" DataFieldName="FechaBaja"
                            ExportToExcel="true" />
                    </Columns>
                </AjaxInfo:ClientControlGrid>
            </td>
        </tr>
        <tr id="trResultadoVacio" runat="server" visible="false">
            <td align="center" valign="top">
                <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Size="13pt" Font-Names="Sans-Serif"
                    ForeColor="white" Text="No existen resultado para el período seleccionado" Width="468px"></asp:Label>
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
