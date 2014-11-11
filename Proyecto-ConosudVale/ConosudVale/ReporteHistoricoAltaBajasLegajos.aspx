<%@ Page Title="" Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="ReporteHistoricoAltaBajasLegajos.aspx.cs" Inherits="ReporteHistoricoAltaBajasLegajos" %>

<%@ Register Assembly="ControlsAjaxNotti" Namespace="ControlsAjaxNotti" TagPrefix="AjaxInfo" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript">
            var ctrLegajos;
          

            function BuscarDatos(data, value) {

                if (value != undefined) {
                    $find("<%=GridAltaBajas.ClientID %>").ShowWaiting("Buscando Datos...");
                    PageMethods.GetData(value, FillGrid, Error);
                } else if (ctrLegajos.SelectedValue != undefined ) {
                    $find("<%=GridAltaBajas.ClientID %>").ShowWaiting("Buscando Datos...");
                    PageMethods.GetData(ctrLegajos.SelectedValue, FillGrid, Error);
                } else if (ctrLegajos.currentValue != undefined) {
                    $find("<%=GridAltaBajas.ClientID %>").ShowWaiting("Buscando Datos...");
                    PageMethods.GetData(ctrLegajos.currentValue, FillGrid, Error);

                }

            }


            function FillGrid(result) {
                $find("<%=GridAltaBajas.ClientID %>").set_ClientdataSource(result);
            }

            function Error(err) {
                alert(err.get_message());
            }

            jQuery(function () {
                options = {
                    serviceUrl: 'LoadLegajos.ashx',
                    width: '384',
                    onSelect: BuscarDatos,
                    minChars: 3,
                    zIndex: 922000000
                };
                ctrLegajos = $('#' + "<%= txtNroDoc.ClientID %>").autocomplete(options);

            });

        </script>
    </telerik:RadScriptBlock>
    <table cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td align="center" style="height: 35px; padding-left: 15px; padding-top: 10px">
                <asp:Label ID="lblTipoGestion" runat="server" Font-Bold="True" Font-Size="20pt" Font-Underline="false"
                    Font-Italic="True" ForeColor="black" Text="REPORTE HISTORICO ALTA BAJAS DE LEGAJOS"
                    Font-Names="Arno Pro"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <table style="background-color: transparent; font-family: Arno Pro; font-size: 16px;
                width: 100%; vertical-align: middle;" border="0">
                <tr>
                    <td valign="middle" align="right" style="width: 360px">
                        <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="#8C8C8C" Height="22px"
                            Text="NroDoc:"></asp:Label>
                    </td>
                    <td valign="middle" align="left" style="width: 220px">
                        <asp:TextBox Width="250px" ID="txtNroDoc" runat="server"></asp:TextBox>
                    </td>
                    <td valign="middle" align="left">
                        <asp:ImageButton runat="server" Style="padding-left: 15px; padding-bottom: 15px;
                            border: 0px; vertical-align: middle;" ImageUrl="~/Images/Search.png" ID="imgBuscar"
                            OnClientClick="BuscarDatos();return false;" CausesValidation="false" Mensaje="Buscando Legajos.." />
                    </td>
                </tr>
                <tr>
                    <td colspan="5" align="center">
                        <div style="width: 95%; height: 8px; border-top-style: solid; border-top-width: 2px;
                            border-top-color: #808080;">
                            &nbsp;
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table style="border-right: #843431 thin solid; border-top: #843431 thin solid; border-left: #843431 thin solid;
        border-bottom: #843431 thin solid; background-color: #E0D6BE; font-family: Sans-Serif;
        font-size: 11px; height: 350px" width="90%">
        <tr id="trReporte" runat="server">
            <td align="center" valign="top">
                <AjaxInfo:ClientControlGrid ID="GridAltaBajas" runat="server" AllowMultiSelection="false"
                    TypeSkin="Sunset" PositionAdd="Top" AllowRowSelection="false" Width="100%" KeyName="UniqueID"
                    AllowPaging="false" PageSize="120" EmptyMessage="Ingrese un nro de documento para ver el historial">
                    <FunctionsGral>
                        <AjaxInfo:FunctionGral Type="Excel" Text="Exportar Datos" />
                    </FunctionsGral>
                    <Columns>
                        <AjaxInfo:Column HeaderName="Nombre" DataFieldName="NombreLegajo" Align="Derecha"
                            ExportToExcel="true" />
                        <AjaxInfo:Column Capitalice="true" HeaderName="Nro Contrato" DataFieldName="NroContrato"
                            Align="Centrado" ExportToExcel="true" />
                        <AjaxInfo:Column HeaderName="Contratista" DataFieldName="NombreEmpresaContratista"
                            Align="Derecha" ExportToExcel="true" />
                        <AjaxInfo:Column HeaderName="Sub Contratista" DataFieldName="NombreEmpresaSubContratista"
                            Align="Derecha" ExportToExcel="true" />
                        <AjaxInfo:Column Capitalice="true" HeaderName="P. Periodo" DataFieldName="PrimerPeriodoString"
                            Align="Centrado" ExportToExcel="true" />
                        <AjaxInfo:Column Capitalice="true" HeaderName="U. Periodo" DataFieldName="UltimoPeriodoString"
                            Align="Centrado" ExportToExcel="true" />
                        <AjaxInfo:Column HeaderName="Fecha Tramite Baja" DataFieldName="FechaTramiteBaja"
                            Align="Centrado" ExportToExcel="true" />
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
</asp:Content>
