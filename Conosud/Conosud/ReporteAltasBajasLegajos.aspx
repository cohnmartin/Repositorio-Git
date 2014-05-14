<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="ReporteAltasBajasLegajos.aspx.cs" Inherits="ReporteAltasBajasLegajos" %>
<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=3.1.9.807, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" namespace="System.Web.UI.WebControls" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <table cellpadding="0" cellspacing="5" style="width: 80%">
        <tr>
            <td align="center" style="height: 25px; background: url('images/sprite.gif') 0  -997px repeat-x">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="14pt" Font-Names="Sans-Serif"
                    ForeColor="#E0D6BE" Text="Reporte Altas Bajas Legajos" 
                    Width="468px"></asp:Label>
            </td>
        </tr>
    </table>
    
   
   <table id="Table1" style="border-right: #843431 thin solid; width: 180px;border-top: #843431 thin solid;
        border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #E0D6BE;
        font-family: Sans-Serif; font-size: 11px;">
        <tr>
            <td style="width: 309px; height: 26px; text-align: left">
                <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Maroon" Text="Período:"
                    Width="94px"></asp:Label>
            </td>
            <td style="width: 309px; height: 26px; text-align: left">
                <asp:DropDownList ID="cboPeriodos" runat="server"  Width="208px"
                    Height="22px">
                </asp:DropDownList>
            </td>
            <td valign="middle" align="left">
                <asp:Button ID="btnBuscar" runat="server" CommandName="Generar" SkinID="btnConosudBasic"
                    Text="Buscar" Mensaje="Generando informe Altas y Bajas..." OnClick="btnBuscar_Click" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="upResultado" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="border-right: #843431 thin solid; border-top: #843431 thin solid; border-left: #843431 thin solid;
                border-bottom: #843431 thin solid; background-color: #E0D6BE; font-family: Sans-Serif;
                font-size: 11px; height: 550px" width="98%">
                <tr id="trReporte" runat="server">
                    <td align="center" valign="top">
                        <telerik:ReportViewer ID="ReportViewer1" runat="server" Style="border: 1px solid #ccc;"
                            Width="99%" Height="550" ProgressText="Generando Reporte..." ShowParametersButton="False"
                            ShowPrintButton="False" ShowRefreshButton="False" ShowZoomSelect="False">
                            <Resources ExportButtonText="Exportar" ExportSelectFormatText="Seleccione Formato Exportación"
                                LabelOf="de" ProcessingReportMessage="Generando Reporte..." />
                        </telerik:ReportViewer>
                    </td>
                </tr>
                <tr id="trResultadoVacio" runat="server" visible="false">
                    <td align="center" valign="top">
                        <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Size="13pt" Font-Names="Sans-Serif"
                            ForeColor="white" Text="No existen resultado para el período seleccionado" Width="468px"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
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
</asp:Content>

