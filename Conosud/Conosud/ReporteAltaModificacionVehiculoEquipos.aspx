<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true"
    CodeFile="ReporteAltaModificacionVehiculoEquipos.aspx.cs" Inherits="ReporteAltaModificacionVehiculoEquipos" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Assembly="ControlsAjaxNotti" Namespace="ControlsAjaxNotti" TagPrefix="AjaxInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript">

            function BuscarDatos() {
                $find("<%=GridAltaBajas.ClientID %>").ShowWaiting("Buscando Vehiculos y Equipos...");
                PageMethods.GetData($get("<%=cboPeriodos.ClientID %>").value, 0, $find("<%=GridAltaBajas.ClientID %>").get_pageSize(), FillGrid, Error);

            }


            function FillGrid(result) {
                $find("<%=GridAltaBajas.ClientID %>").set_ClientdataSource(result);
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
                                Font-Italic="True" ForeColor="black" Text="REPORTE ALTA MODIFICACION VEHICULOS Y EQUIPOS"
                                Font-Names="Arno Pro"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
                                <contenttemplate>
                                    <table style="background-color: transparent; font-family: Sans-Serif; font-size: 11px;
                                        width: 100%; vertical-align: middle;" border="0">
                                        <tr>
                                            <td style="width: 359px; height: 26px; text-align: right">
                                                <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="#8C8C8C" Text="Período:"
                                                    Width="94px"></asp:Label>
                                            </td>
                                            <td style="width: 219px; height: 26px; text-align: left">
                                                <asp:DropDownList ID="cboPeriodos" runat="server" Width="208px" Height="22px">
                                                </asp:DropDownList>
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
                                </contenttemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table id="Table3" style="font-family: Sans-Serif; font-size: 11px;" width="98%">
                    <tr>
                        <td align="center">
                            <AjaxInfo:ClientControlGrid ID="GridAltaBajas" runat="server" AllowMultiSelection="false"
                                TypeSkin="Sunset" PositionAdd="Top" AllowRowSelection="false" Width="100%" KeyName="idVehiculo"
                                AllowPaging="true" PageSize="20" EmptyMessage="Seleccione un pariodo para ver los datos" >
                                <FunctionsGral>
                                    <AjaxInfo:FunctionGral Type="Excel" Text="Exportar Datos" />
                                </FunctionsGral>
                                <Columns>
                                    <AjaxInfo:Column HeaderName="Periodo" Align="Centrado" DataFieldName="Periodo" 
                                        ExportToExcel="true" />
                                    <AjaxInfo:Column HeaderName="Tipo" DataFieldName="Tipo" Align="Derecha" ExportToExcel="true" />
                                    <AjaxInfo:Column HeaderName="Mov" DataFieldName="Accion" Align="Derecha" ExportToExcel="true" />
                                    <AjaxInfo:Column Capitalice="true" HeaderName="Empresa" DataFieldName="NombreEmpresa"
                                        Align="Derecha" ExportToExcel="true" />
                                    <AjaxInfo:Column HeaderName="Nº Contrato" DataFieldName="CodigoContrato" Align="Derecha"
                                        ExportToExcel="true" />
                                    <AjaxInfo:Column HeaderName="Patentec" DataFieldName="Patente" Align="Centrado" ExportToExcel="true" />
                                    <AjaxInfo:Column Align="Derecha" HeaderName="Marca" DataFieldName="Marca" ExportToExcel="true" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Fecha Creacion" DataFieldName="FechaCreacion"
                                        ExportToExcel="true" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Fecha Actualizacion" DataFieldName="FechaActualizacion"
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
