<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/DefaultMasterPage.master" CodeFile="ReporteEquiposAfectados.aspx.cs" Inherits="ReporteEquiposAfectados" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Assembly="ControlsAjaxNotti" Namespace="ControlsAjaxNotti" TagPrefix="AjaxInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript">

            function BuscarDatos() {
                $find("<%=GridRecursosAfectados.ClientID %>").ShowWaiting("Buscando Recursos...");
                PageMethods.GetData($get("<%=cboPeriodos.ClientID %>").value, 0, $find("<%=GridRecursosAfectados.ClientID %>").get_pageSize(), FillGrid, Error);

            }


            function FillGrid(result) {
                $find("<%=GridRecursosAfectados.ClientID %>").set_ClientdataSource(result);
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
                                Font-Italic="True" ForeColor="black" Text="REPORTE EQUIPOS ASIGNADOS" Font-Names="Arno Pro"></asp:Label>
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
                                Font-Size="16px" Text="Periodo:"></asp:Label>
                        </td>
                        <td valign="middle" align="left" width="310px">
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
            </td>
        </tr>
        <tr>
            <td align="center">
                <table id="Table3" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
                    border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #E0D6BE;
                    font-family: Sans-Serif; font-size: 11px;" width="98%">
                    <tr>
                        <td align="center">
                            <AjaxInfo:ClientControlGrid ID="GridRecursosAfectados" runat="server" AllowMultiSelection="false"
                                TypeSkin="Sunset" PositionAdd="Top" AllowRowSelection="false" Width="100%" KeyName="IdLegajos"
                                AllowPaging="false" PageSize="8">
                                <FunctionsGral>
                                    <AjaxInfo:FunctionGral Type="Excel" Text="Exportar Datos" />
                                </FunctionsGral>
                                <Columns>

                                    <AjaxInfo:Column HeaderName="Fecha Consulta" DataFieldName="FechaConsulta" Align="Derecha"
                                        ExportToExcel="true" />
                                    <AjaxInfo:Column Capitalice="true" HeaderName="Patente" DataFieldName="Patente" Align="Derecha"
                                        ExportToExcel="true" />
                                    <AjaxInfo:Column HeaderName="Marca" DataFieldName="Marca" Align="Derecha" ExportToExcel="true" />
                                    <AjaxInfo:Column HeaderName="Modelo" DataFieldName="Modelo" Align="Derecha" ExportToExcel="true" />

                                    <AjaxInfo:Column Align="Centrado" HeaderName="Alta Empresa" DataFieldName="AltaEmpresa"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Baja Empresa" DataFieldName="BajaEmpresa"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Capacidad Carga" DataFieldName="CapacidadCarga"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Compañia Seguro" DataFieldName="CompañiaSeguro"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Des Seguro" DataFieldName="DescripcionSeguro"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Equipamiento Agregado" DataFieldName="EquipamientoAgregado"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Es Propio" DataFieldName="EsPropio"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Fecha Fabricacion" DataFieldName="FechaFabricacion"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Fecha Habilitacion" DataFieldName="FechaHabilitacion"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Fecha Inicial Seguro" DataFieldName="FechaInicialSeguro"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Fecha Ult Pago Seguro" DataFieldName="FechaUltimoPagoSeguro"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Fecha Venc Habilitacion" DataFieldName="FechaVencimientoHabilitacion"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Fecha Venc Seguro" DataFieldName="FechaVencimientoSeguro"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="HabilitarCredencial" DataFieldName="HabilitarCredencial"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Nombre Titular" DataFieldName="NombreTitular"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Nro Chasis" DataFieldName="NroChasis"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Nro Habilitacion" DataFieldName="NroHabilitacion"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Nro Motor" DataFieldName="NroMotor"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Nro Poliza Seguro" DataFieldName="NroPolizaSeguro"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Tipo Unidad" DataFieldName="TipoUnidad"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Vencimiento Credencial" DataFieldName="VencimientoCredencial"
                                        ExportToExcel="true" Display="false" />

                                    <AjaxInfo:Column Align="Centrado" HeaderName="Codigo Contrato" DataFieldName="Codigo"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Fecha Inicio Contrato" DataFieldName="FechaInicioContrato"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Fecha Vencimiento Contrato" DataFieldName="FechaVencimientoContrato"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Prorroga" DataFieldName="Prorroga"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Fiscal Nombre" DataFieldName="FiscalNombre"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Fiscal Email" DataFieldName="FiscalEmail"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Gestor Email" DataFieldName="GestorEmail"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Gestor Nombre" DataFieldName="GestorNombre"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Categoria" DataFieldName="DescCategoria"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Contratado Por" DataFieldName="DescContratadoPor"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Tipo Contrato" DataFieldName="DescTipoContrato"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Servicio" DataFieldName="Servicio"
                                        ExportToExcel="true" Display="false" />
                                         <AjaxInfo:Column Align="Centrado" HeaderName="Area" DataFieldName="DescArea" ExportToExcel="true"
                                        Display="false" />



                                    <AjaxInfo:Column Align="Centrado" HeaderName="CUIT Empresa" DataFieldName="CUITEmpresa"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Razon Social Emp" DataFieldName="RazonSocial"
                                        ExportToExcel="true" Display="false" />

                                </Columns>
                            </AjaxInfo:ClientControlGrid>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
