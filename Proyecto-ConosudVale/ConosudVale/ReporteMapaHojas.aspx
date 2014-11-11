<%@ Page Title="" Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="ReporteMapaHojas.aspx.cs" Inherits="ReporteMapaHojas" %>

<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=3.1.9.807, Culture=neutral, PublicKeyToken=a9d7983dfcc261be"
    Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="Scripts/jquery.tmpl.1.1.1.js" type="text/javascript"></script>
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript">

            function BuscarDatos() {
                var contratosCombo = $find("ctl00_ContentPlaceHolder1_cboContratos");
                var periodoCombo = $find("ctl00_ContentPlaceHolder1_cboPeriodos");

                PageMethods.GetData(contratosCombo.get_value(), periodoCombo.get_value(), FillGrid, Error);

            }


            function FillGrid(result) {

                $get("tblReporte").innerHTML = result;
            }

            function Error(err) {
                alert(err.get_message());
            }

        </script>
    </telerik:RadScriptBlock>
    <script type="text/javascript">
        var contratosCombo = $find("ctl00_ContentPlaceHolder1_cboContratos");

        function LoadContratos(combo, eventarqs) {
            var contratosCombo = $find("ctl00_ContentPlaceHolder1_cboContratos");
            var periodoCombo = $find("ctl00_ContentPlaceHolder1_cboPeriodos");

            var item = eventarqs.get_item();
            contratosCombo.set_text("Loading...");
            periodoCombo.clearSelection();

            if (item != null) {
                contratosCombo.requestItems(item.get_value(), false);
            }
            else {
                contratosCombo.set_text(" ");
                contratosCombo.clearItems();

            }
        }

        function LoadPeriodos(combo, eventarqs) {
            var periodoCombo = $find("ctl00_ContentPlaceHolder1_cboPeriodos");

            var item = eventarqs.get_item();

            if (item.get_index() > 0) {
                periodoCombo.clearSelection();
                periodoCombo.set_text("Loading...");
                periodoCombo.requestItems(item.get_value(), false);

            }
            else {
                periodoCombo.set_text(" ");
                periodoCombo.clearItems();

            }


        }

        function ItemsLoaded(combo, eventarqs) {
            var contratosCombo = $find("ctl00_ContentPlaceHolder1_cboContratos");

            if (combo.get_items().get_count() > 0) {
                combo.set_text(combo.get_items().getItem(0).get_text());
                combo.get_items().getItem(0).highlight();
            }
            combo.showDropDown();
        }
        
    </script>
    <style>
        .TSunset .tdSimple
        {
            color: #333;
            font: 12px/16px "segoe ui" ,arial,sans-serif;
            border-color: #fff #EFEFEF #fff #EFEFEF;
            border-style: solid;
            border-width: 0 1px 1px;
            padding-left: 2px;
            padding-top: 3px;
            padding-bottom: 4px;
            background-color: White;
            text-align: center;
        }
        
        .TSunset .Theader
        {
            background-position: 0 -7550px;
            border-top: 0;
            background-image: url('images/sprite_sunset.gif');
            background-repeat: repeat;
            border-style: solid;
            border-width: 1px;
            border-color: #e98879 #cd6a3f #71250a #872b07;
            color: white;
            font-family: "segoe ui" ,arial,sans-serif;
            font-size: 12px;
            height: 23px;
            text-align: center;
            padding: 0px 0px 0px 0px;
            line-height: 1em;
        }
    </style>
    <table cellpadding="0" cellspacing="5" style="width: 80%">
        <tr>
            <td align="center" style="height: 25px; background: url('images/sprite.gif') 0  -997px repeat-x">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="14pt" Font-Names="Sans-Serif"
                    ForeColor="#E0D6BE" Text="Reporte Mapa Hojas de Ruta" Width="468px"></asp:Label>
            </td>
        </tr>
    </table>
    <table id="Table1" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
        border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #E0D6BE;
        font-family: Sans-Serif; font-size: 11px;">
        <tr>
            <td align="right" style="width: 98px; height: 26px">
                <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="Maroon" Height="22px"
                    Text="Empresa:" Width="79px"></asp:Label>
            </td>
            <td id="Td3" align="left" style="width: 95px; height: 26px">
                <telerik:RadComboBox ID="cboEmpresas" runat="server" Skin="Sunset" Width="200px"
                    AllowCustomText="true" MarkFirstMatch="true" OnClientSelectedIndexChanging="LoadContratos"
                    OnItemsRequested="cboEmpresas_ItemsRequested" />
            </td>
            <td align="right" style="width: 34px">
                <asp:Label ID="lblContr" runat="server" Font-Bold="True" ForeColor="Maroon" Height="22px"
                    Text="Contratos:" Width="85px"></asp:Label>
            </td>
            <td align="left" style="width: 299px">
                <telerik:RadComboBox ID="cboContratos" runat="server" Skin="Sunset" Width="250px"
                    OnItemsRequested="cboContratos_ItemsRequested" OnClientSelectedIndexChanging="LoadPeriodos"
                    OnClientItemsRequested="ItemsLoaded" />
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 98px; height: 26px">
                <asp:Label ID="lblPeriodos" runat="server" Font-Bold="True" ForeColor="Maroon" Height="20px"
                    Text="Períodos:" Width="85px"></asp:Label>
            </td>
            <td align="left" style="width: 95px; height: 26px">
                <telerik:RadComboBox ID="cboPeriodos" runat="server" Skin="Sunset" Width="200px"
                    AutoPostBack="false" Mensaje="Buscando Hojas de Ruta..." MarkFirstMatch="true"
                    AllowCustomText="true" OnItemsRequested="cboPeriodos_ItemsRequested" OnClientItemsRequested="ItemsLoaded" />
            </td>
            <td valign="middle" align="center" colspan="2">
                <asp:Button ID="btnBuscar" runat="server" CommandName="Buscar" SkinID="btnConosudBasic" 
                    Text="Buscar" Mensaje="Buscando hojas de ruta..." OnClientClick="BuscarDatos();return false;" />
            </td>
        </tr>
    </table>
    <div id="tblReporte" style="width:80%;padding-top:10px">
    </div>

    <asp:UpdatePanel ID="upResultado" runat="server" UpdateMode="Conditional">
        <contenttemplate>
            <table style="border-right: #843431 thin solid; border-top: #843431 thin solid; border-left: #843431 thin solid;
                border-bottom: #843431 thin solid; background-color: #E0D6BE; font-family: Sans-Serif;
                font-size: 11px; height: 550px;display:none" width="98%">
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
        </contenttemplate>
        <triggers>
            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
        </triggers>
    </asp:UpdatePanel>
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
