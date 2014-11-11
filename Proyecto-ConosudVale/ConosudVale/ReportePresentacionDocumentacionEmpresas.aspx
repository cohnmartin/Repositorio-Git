<%@ Page Title="Reporte Estado Presentacion Dcoumentacion" Theme="MiTema" Language="C#"
    MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="ReportePresentacionDocumentacionEmpresas.aspx.cs"
    Inherits="ReportePresentacionDocumentacionEmpresas" EnableEventValidation="false" ValidateRequest="false" %>

<%@ Register Assembly="ControlsAjaxNotti" Namespace="ControlsAjaxNotti" TagPrefix="AjaxInfo" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td align="left">
                <table cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td align="center" style="height: 35px; padding-left: 15px; padding-top: 15px">
                            <asp:Label ID="lblTipoGestion" runat="server" Font-Bold="True" Font-Size="20pt" Font-Underline="false"
                                Font-Italic="True" ForeColor="black" Text="REPORTE ESTADO PRESENTACION DOCUMENTACION"
                                Font-Names="Arno Pro"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <table style="background-color: transparent; font-family: Sans-Serif; font-size: 11px;
                                width: 100%; vertical-align: middle;" border="0">
                                <tr>
                                    <td style="width: 35%" align="right">
                                        <asp:Label ID="Label4" runat="server" Text="Fecha Inicial:" Font-Bold="True" Font-Names="Arno Pro"
                                            ForeColor="#8C8C8C" Font-Size="16px"></asp:Label>
                                    </td>
                                    <td style="width: 15%" align="center">
                                        <telerik:RadDatePicker ID="txtInicial" MinDate="1950/1/1" runat="server">
                                        </telerik:RadDatePicker>
                                    </td>
                                    <td style="width: 10%" align="right">
                                        <asp:Label ID="lblIdCabecera" runat="server" Text="Fecha Final:" Font-Bold="True"
                                            Font-Names="Arno Pro" ForeColor="#8C8C8C" Font-Size="16px"></asp:Label>
                                    </td>
                                    <td>
                                        <telerik:RadDatePicker ID="txtFinal" MinDate="1950/1/1" runat="server">
                                        </telerik:RadDatePicker>
                                    </td>
                                    <td valign="middle" align="left">
                                        <asp:ImageButton runat="server" Style="padding-left: 15px; padding-bottom: 15px;
                                            border: 0px; vertical-align: middle;" ImageUrl="~/Images/Search.png" ID="imgBuscar"
                                            OnClick="imgBuscar_Click" Mensaje="Buscando Datos..." />
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
                            <asp:UpdatePanel ID="upResultado" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <AjaxInfo:ClientControlGrid ID="gvEstadoPresentacion" runat="server" AllowMultiSelection="false"
                                        TypeSkin="Sunset" PositionAdd="Top" AllowRowSelection="false" Width="100%" KeyName="Contrato"
                                        AllowPaging="false" EmptyMessage="Debe ingresar el rango de fechas y buscar para ver los resultados">
                                        <FunctionsGral>
                                            <AjaxInfo:FunctionGral Type="Excel" Text="Exportar Datos" />
                                        </FunctionsGral>
                                        <Columns>
                                            <AjaxInfo:Column HeaderName="Periodo" DataFieldName="Periodo" ExportToExcel="true"
                                                Align="Centrado" />
                                            <AjaxInfo:Column HeaderName="Contratista" DataFieldName="Contratista" Align="Derecha"
                                                ExportToExcel="true" />
                                            <AjaxInfo:Column HeaderName="SubContratista" DataFieldName="SubContratista" Align="Derecha"
                                                ExportToExcel="true" />
                                            <AjaxInfo:Column Align="Centrado" HeaderName="Contrato" DataFieldName="Contrato"
                                                ExportToExcel="true" />

                                            <AjaxInfo:Column Align="Centrado" HeaderName="Fecha Inicio" DataFieldName="FechaInicio" Display="false" ExportToExcel="true" />
                                            <AjaxInfo:Column Align="Centrado" HeaderName="Fecha Vencimiento" DataFieldName="FechaVencimiento" Display="false" ExportToExcel="true" />
                                            <AjaxInfo:Column Align="Centrado" HeaderName="Fecha Prorroga" DataFieldName="FechaProrroga" Display="false" ExportToExcel="true" />
                                            <AjaxInfo:Column Align="Centrado" HeaderName="Categoria" DataFieldName="Categoria" Display="false" ExportToExcel="true" />
                                            <AjaxInfo:Column Align="Centrado" HeaderName="Contratado Por" DataFieldName="ContratadoPor" Display="false" ExportToExcel="true" />
                                            <AjaxInfo:Column Align="Centrado" HeaderName="Area" DataFieldName="Area" Display="false" ExportToExcel="true" />


                                            <AjaxInfo:Column Align="Centrado" HeaderName="Nro 1" DataFieldName="Nro1" ExportToExcel="true" />
                                            <AjaxInfo:Column Align="Centrado" HeaderName="Nro 2" DataFieldName="Nro2" ExportToExcel="true" />
                                            <AjaxInfo:Column Align="Centrado" HeaderName="Nro 3" DataFieldName="Nro3" ExportToExcel="true" />
                                            <AjaxInfo:Column Align="Centrado" HeaderName="Nro 4" DataFieldName="Nro4" ExportToExcel="true" />
                                            <AjaxInfo:Column Align="Centrado" HeaderName="Nro 5" DataFieldName="Nro5" ExportToExcel="true" />
                                            <AjaxInfo:Column Align="Centrado" HeaderName="Nro 6" DataFieldName="Nro6" ExportToExcel="true" />
                                            <AjaxInfo:Column Align="Centrado" HeaderName="Nro 7" DataFieldName="Nro7" ExportToExcel="true" />
                                            <AjaxInfo:Column Align="Centrado" HeaderName="Nro 8" DataFieldName="Nro8" ExportToExcel="true" />
                                            <AjaxInfo:Column Align="Centrado" HeaderName="Nro 9" DataFieldName="Nro9" ExportToExcel="true" />
                                        </Columns>
                                    </AjaxInfo:ClientControlGrid>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="imgBuscar" EventName="Click" />
                                    <asp:PostBackTrigger  ControlID="gvEstadoPresentacion" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
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
                                Buscando Datos...
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
