<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true"
    CodeFile="ReporteHistoricoLegajo.aspx.cs" Inherits="ReporteHistoricoLegajo" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="ControlsAjaxNotti" Namespace="ControlsAjaxNotti" TagPrefix="AjaxInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="Scripts/jquery.tmpl.1.1.1.js" type="text/javascript"></script>
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript">

            function BuscarDatos() {
                $find("<%=GridHistoricoLegajos.ClientID %>").ShowWaiting("Buscando Datos Legajo...");
                PageMethods.GetData($find("<%=txtNroDoc.ClientID %>").get_value(), FillGrid, Error);

            }


            function FillGrid(result) {
                $find("<%=GridHistoricoLegajos.ClientID %>").set_ClientdataSource(result);
            }

            function Error(err) {
                alert(err.get_message());
            }

        </script>
    </telerik:RadScriptBlock>
    <table cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td align="left">
                <table cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td align="center" style="height: 35px; padding-left: 15px; padding-top: 15px">
                            <asp:Label ID="lblTipoGestion" runat="server" Font-Bold="True" Font-Size="20pt" Font-Underline="false"
                                Font-Italic="True" ForeColor="black" Text="REPORTE HISTORICO LEGAJOS" Font-Names="Arno Pro"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left">
                <table style="background-color: transparent; font-family: Sans-Serif; font-size: 11px;
                    width: 100%; vertical-align: middle;" border="0">
                    <tr>
                        <td valign="middle" align="right" style="padding-left: 10px; width: 340px">
                            <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Names="Arno Pro" ForeColor="#8C8C8C"
                                Font-Size="16px" Text="Documento:"></asp:Label>
                        </td>
                        <td valign="middle" align="left" width="310px">
                        <telerik:RadTextBox ID="txtNroDoc" runat="server" EmptyMessage="Ingrese Nro Doc"
                            Skin="Sunset" Width="100%">
                        </telerik:RadTextBox>
                        </td>
                        <td valign="middle" align="left">
                            <asp:ImageButton runat="server" Style="padding-left: 15px; padding-bottom: 15px;
                                border: 0px; vertical-align: middle;" ImageUrl="~/Images/Search.png" ID="imgBuscar"
                                OnClientClick="BuscarDatos();return false;" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <div style="width: 95%; height: 8px; border-top-style: solid; border-top-width: 2px;
                                border-top-color: #808080;">
                                &nbsp;
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table id="Table3" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
                    border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #E0D6BE;
                    font-family: Sans-Serif; font-size: 11px;" width="98%">
                    <tr>
                        <td align="center">
                            <AjaxInfo:ClientControlGrid ID="GridHistoricoLegajos" runat="server" AllowMultiSelection="false"
                                TypeSkin="Sunset" PositionAdd="Top" AllowRowSelection="false" Width="100%" KeyName="Id"
                                AllowPaging="false" PageSize="8"  EmptyMessage="No exiten datos para el documento ingresado">
                                <FunctionsGral>
                                    <AjaxInfo:FunctionGral Type="Excel" Text="Exportar Datos" />
                                </FunctionsGral>
                                <Columns>
                                    <AjaxInfo:Column Capitalice="true" HeaderName="Apellido" DataFieldName="Apellido"
                                        Align="Derecha" ExportToExcel="true" />
                                    <AjaxInfo:Column Capitalice="true" HeaderName="Nombre" DataFieldName="Nombre" Align="Derecha"
                                        ExportToExcel="true" />
                                    <AjaxInfo:Column Capitalice="true" HeaderName="Empresa" DataFieldName="Empresa" Align="Derecha"
                                        ExportToExcel="true" />
                                    <AjaxInfo:Column HeaderName="Nro Contrato" DataFieldName="contrato" Align="Derecha"
                                        ExportToExcel="true" />
                                    <AjaxInfo:Column HeaderName="Periodo Inicial" DataFieldName="PeriodoInicial" Align="Derecha"
                                        ExportToExcel="true" />
                                    <AjaxInfo:Column HeaderName="Periodo Final" DataFieldName="PeriodoFinal" Align="Derecha"
                                        ExportToExcel="true" />
                                </Columns>
                            </AjaxInfo:ClientControlGrid>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
