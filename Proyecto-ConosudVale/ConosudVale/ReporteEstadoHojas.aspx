﻿<%@ Page Title="" Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="ReporteEstadoHojas.aspx.cs" Inherits="ReporteEstadoHojas" %>

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

        //        function startBlink() {
        //            try {
        //                richter = 5
        //                parent.moveBy(0, richter)
        //                parent.moveBy(0, -richter)
        //                parent.moveBy(richter, 0)
        //                parent.moveBy(-richter, 0)
        //                timer = setTimeout("startBlink()", 15)
        //            }
        //            catch (e) {
        //                timer = setTimeout("startBlink()", 15)
        //            }
        //        }

        //        function stopBlink() {
        //            clearTimeout(timer)
        //        }

        //        startBlink();
    
    </script>
    <table cellpadding="0" cellspacing="5" style="width: 80%">
        <tr>
            <td align="center" style="height: 25px; background: url('images/sprite.gif') 0  -997px repeat-x">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="14pt" Font-Names="Sans-Serif"
                    ForeColor="#E0D6BE" Text="Reporte de Estados Hojas de Ruta" Width="468px"></asp:Label>
            </td>
        </tr>
    </table>
    <table id="Table1" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
        border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #E0D6BE;
        font-family: Sans-Serif; font-size: 11px;">
        <tr>
            <td align="right" style="width: 98px; height: 26px">
                <asp:Label ID="lblPeriodos" runat="server" Font-Bold="True" ForeColor="Maroon" Height="20px"
                    Text="Períodos:" Width="85px"></asp:Label>
            </td>
            <td align="left" style="width: 95px; height: 26px">
                <telerik:RadComboBox ID="cboPeriodos" runat="server" Skin="Sunset" Width="200px"
                    AutoPostBack="true" Mensaje="Generando Informe..." MarkFirstMatch="true" AllowCustomText="true"
                    OnSelectedIndexChanged="cboPeriodos_SelectedIndexChanged" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="upResultado" runat="server" UpdateMode="Conditional">
        <contenttemplate>
            <table style="border-right: #843431 thin solid; border-top: #843431 thin solid; border-left: #843431 thin solid;
                border-bottom: #843431 thin solid; background-color: #E0D6BE; font-family: Sans-Serif;
                font-size: 11px; height: 100%" width="98%">
                <tr id="trReporte" runat="server">
                    <td align="left" valign="top">
                        <telerik:RadGrid ID="gvEstadoContratos" runat="server" AllowPaging="True" AllowSorting="false"
                            GridLines="None" Skin="Sunset" AutoGenerateColumns="false" PageSize="10" 
                            OnItemCommand="gvEstadoContratos_ItemCommand" 
                            OnNeedDataSource="gvEstadoContratos_NeedDataSource"
                            OnItemDataBound="gvEstadoContratos_ItemDataBound" >
                            
                           
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
                                    <telerik:GridBoundColumn DataField="CodigoContrato" HeaderText="N° Contrato" UniqueName="CodigoContratoColumn">
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
                                    <telerik:GridBoundColumn Display="false" DataField="Categoria" HeaderText="Categoria"
                                        UniqueName="CategoriaColumn">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Estado" HeaderText="Estado" UniqueName="EstadoColumn">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="PresentoDocumentacion" HeaderText="Presento Doc."
                                        UniqueName="PresentoDocumentacionColumn">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn Display="true" DataField="Riesgo" HeaderText="Riesgo"
                                        UniqueName="RiesgoColumn">
                                        <HeaderStyle HorizontalAlign="Center" />
                                         <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn Display="true" DataField="Grado" HeaderText="Grado"
                                        UniqueName="GradoColumn">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn Display="false" DataField="Contratadopor" HeaderText="Contratado por"
                                        UniqueName="ContratadoporColumn">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn Display="false" DataField="Servicio" HeaderText="Servicio"
                                        UniqueName="ServicioColumn">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn Display="false" DataField="Area" HeaderText="Area"
                                        UniqueName="AreaColumn">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn Display="false" DataField="Gestor" HeaderText="Gestor"
                                        UniqueName="GestorColumn">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn Display="false" DataField="GestorEmail" HeaderText="Gestor Email"
                                        UniqueName="GestorEmailColumn">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn Display="false" DataField="Fiscal" HeaderText="Fiscales"
                                        UniqueName="FiscalesColumn">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn Display="false" DataField="FiscalEmail" HeaderText="Fiscales Email"
                                        UniqueName="FiscalesEmailColumn">
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
        </contenttemplate>
        <triggers>
            <asp:AsyncPostBackTrigger ControlID="cboPeriodos" EventName="SelectedIndexChanged" />
        </triggers>
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
