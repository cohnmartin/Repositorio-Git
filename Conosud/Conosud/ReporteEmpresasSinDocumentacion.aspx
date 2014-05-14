<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="ReporteEmpresasSinDocumentacion.aspx.cs" Inherits="ReporteEmpresasSinDocumentacion" %>
<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=3.1.9.807, Culture=neutral, PublicKeyToken=a9d7983dfcc261be"
    Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        function requestStart(sender, args) {
            if (args.get_eventTarget().indexOf("ExportExcel") > 0) {
                args.set_enableAjax(false);
            }
        }
    
    </script>

    <table cellpadding="0" cellspacing="5" style="width: 80%">
        <tr>
            <td align="center" style="height: 25px; background: url('images/sprite.gif') 0  -997px repeat-x">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="14pt" Font-Names="Sans-Serif"
                    ForeColor="#E0D6BE" Text="Reporte de Contratos Sin Documentacion" Width="468px"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="upResultado" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="border-right: #843431 thin solid; border-top: #843431 thin solid; border-left: #843431 thin solid;
                border-bottom: #843431 thin solid; background-color: #E0D6BE; font-family: Sans-Serif;
                font-size: 11px; height: 100%" width="98%">
                <tr id="trReporte" runat="server">
                    <td align="left" valign="top">
                        <telerik:RadGrid ID="gvEstadoContratos" runat="server" AllowPaging="True" AllowSorting="false"
                            GridLines="None" Skin="Sunset" AutoGenerateColumns="false" PageSize="10" 
                            OnItemCommand="gvEstadoContratos_ItemCommand" 
                            OnNeedDataSource="gvEstadoContratos_NeedDataSource" >
                            
                           
                            <MasterTableView DataKeyNames="CodigoContrato" CommandItemDisplay="Top" NoMasterRecordsText="No hay resultados para el período seleccionado."
                                HorizontalAlign="NotSet">
                                <CommandItemTemplate>
                                    <div style="padding: 5px 5px;">
                                        <asp:LinkButton Mensaje="Exportando Contratos...." ID="ExportExcel" runat="server"
                                            CommandName="ExportContratos">
                                            <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/Excel_16x16.gif" />Excel</asp:LinkButton>&nbsp;&nbsp;
                                    </div>
                                </CommandItemTemplate>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="Periodo" HeaderText="Periodo" UniqueName="PeriodoColumn">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="CodigoContrato" DataType="System.String" HeaderText="Codigo" UniqueName="CodigoContratoColumn">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="NombreEmpresaContratista" HeaderText="Contratista"
                                        UniqueName="NombreEmpresaContratistaColumn">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="NombreEmpresaSubContratista" HeaderText="Sub Contratista"
                                        UniqueName="NombreEmpresaSubContratistaColumn">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="FechaInicio" HeaderText="Inicio" UniqueName="FechaInicioColumn">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="FechaFin" HeaderText="Fin" UniqueName="FechaFinColumn">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="FechaProrroga" HeaderText="Prorroga" UniqueName="FechaProrrogaColumn">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn Display="false" DataField="Servicio" HeaderText="Servicio"
                                        UniqueName="ServicioColumn" >
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings>
                                <Selecting AllowRowSelect="false" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" ClientEvents-OnRequestStart="requestStart">
        <ClientEvents OnRequestStart="requestStart"></ClientEvents>
       <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="gvEstadoContratos">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvEstadoContratos" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
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
