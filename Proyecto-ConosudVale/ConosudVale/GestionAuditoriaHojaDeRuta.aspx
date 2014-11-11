<%@ Page Title="" Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="GestionAuditoriaHojaDeRuta.aspx.cs" Inherits="GestionAuditoriaHojaDeRuta" %>

<%@ Register Assembly="ControlsAjaxNotti" Namespace="ControlsAjaxNotti" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="App_Themes/MiTema/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/ui-select/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/ui-select/select.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/ui-select/selectize.default.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/ui-select/angular.js" type="text/javascript"></script>
    <script src="Scripts/ui-select/angular-sanitize.js" type="text/javascript"></script>
    <script src="Scripts/ui-select/select.js" type="text/javascript"></script>
    <script src="Scripts/controllers/controller_HojaRuta.js" type="text/javascript"></script>
    <script src="Scripts/ui-select/bootstrap-tooltip.js" type="text/javascript"></script>
    <!--[if lt IE 9]>
      <script src="Scripts/ui-select/es5-shim.js" type="text/javascript"></script>
    <script>
      document.createElement('ui-select');
      document.createElement('ui-select-match');
      document.createElement('ui-select-choices');
    </script>
  <![endif]-->
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript">

            var Constants = {
                idCabeceraHojaRuta: '<%= idCabeceraHojaRuta %>'
            };


        </script>
    </telerik:RadScriptBlock>
    <cc1:ServerControlWindow ID="ServerControlContratos" runat="server" BackColor="WhiteSmoke"
        WindowColor="Rojo">
        <ContentControls>
        </ContentControls>
    </cc1:ServerControlWindow>
    <div id="ng-app" ng-app="myApp" ng-controller="controller_HojaRuta" style="text-align: center;
        padding: 5px;">
        <table style="border-right: #843431 thin solid; border-top: #843431 thin solid; border-left: #843431 thin solid;
            border-bottom: #843431 thin solid; background-color: #E0D6BE; font-family: Sans-Serif;
            font-size: 11px; height: 100%" width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="right" valign="top" style="padding-left: 5px; padding-top: 4px; width: 50px">
                    <table cellpadding="0" cellspacing="0">
                        <tr runat="server" id="trLegajos">
                            <td>
                                <asp:LinkButton ToolTip="Exportar Listado Legajos" Mensaje="Exportando Legajos...."
                                    ID="ExportExcel" runat="server">
                                    <img style="padding-top: 2px; border: 0px; vertical-align: middle;cursor:pointer" alt="Lista de Legajos Asociados" src="Images/Legajos.gif" />
                                </asp:LinkButton>
                            </td>
                        </tr>
                        <tr runat="server" id="trEstimacion">
                            <td>
                                <asp:ImageButton runat="server" ID="imgEstimacion" ImageUrl="Images/comentariotab.gif"
                                    OnClientClick="ShowEstimacion(); return false;" Style="cursor: hand;" ToolTip="Estimación Hoja de Ruta" />
                            </td>
                        </tr>
                        <tr runat="server" id="trAprobacion">
                            <td>
                                <asp:ImageButton runat="server" ID="imgAprobacion" ImageUrl="Images/AprobarTab.gif"
                                    OnClientClick="js_AprobacionAsync(); return false;" Style="cursor: hand;" ToolTip="Aprobar Hoja de Ruta" />
                            </td>
                        </tr>
                        <tr runat="server" id="trReporte">
                            <td>
                                <asp:ImageButton runat="server" ID="imgReporte" ImageUrl="Images/InformeTab.gif"
                                    OnClientClick="ShowReporteHistorico(); return false;" Style="cursor: hand;" ToolTip="Generar Informe Histórico" />
                            </td>
                        </tr>
                        <tr runat="server" id="trCertificado">
                            <td>
                                <asp:ImageButton runat="server" ID="ImageButton1" ImageUrl="Images/CertificadoTab.gif"
                                    OnClientClick="ImprimirCertificadoHoja(); return false;" Style="cursor: hand;"
                                    ToolTip="Impresión Certificado" />
                            </td>
                        </tr>
                        <tr runat="server" id="trAprobacionEspecial">
                            <td>
                                <asp:ImageButton runat="server" ID="ImageButton2" ImageUrl="Images/AprobarEspecialTab.gif"
                                    OnClientClick=" AprobacionEspecial();return false;" Style="cursor: hand;" ToolTip="Aprobación Especial" />
                            </td>
                        </tr>
                        <tr runat="server" id="trDesaprobacion">
                            <td>
                                <asp:ImageButton runat="server" ID="imgDesaprobar" ImageUrl="Images/DesaprobarTab.gif"
                                    OnClientClick="js_Desaprobacion(); return false;" Style="cursor: hand;" ToolTip="Desaprobar Hoja de Ruta" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding-right: 10px; padding-top: 5px; padding-bottom: 5px">
                    <table id="tblContratosAsignados" width="100%" class="TSunset" border="0" cellpadding="0"
                        cellspacing="0">
                        <thead>
                            <tr>
                                <th class="Theader">
                                    &nbsp;
                                </th>
                                <th class="Theader">
                                    Descripcion
                                </th>
                                <th class="Theader">
                                    Doc.
                                </th>
                                <th class="Theader">
                                    Item
                                </th>
                                <th class="Theader">
                                    Gral
                                </th>
                                <th class="Theader">
                                    Aprobar
                                </th>
                                <th class="Theader">
                                    Aprobacion
                                </th>
                                <th class="Theader">
                                    Finalizado
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr class="trDatos" ng-repeat="item in ItemsHoja">
                                <td class="tdSimple" align="center" style="width: 25px; vertical-align: bottom">
                                    <center>
                                        <asp:Image ng-show="item.permitirEditarItem==true" ImageUrl="~/images/Edit.gif" ID="Image2"
                                            runat="server" rel="tooltip" title="Editar Item Hoja de Ruta" Style="cursor: hand;" /></center>
                                </td>
                                <td class="tdSimple" align="left" style="width: 350px">
                                    {{item.Descripcion}}
                                </td>
                                <td class="tdSimple" align="center" style="width: 25px; vertical-align: bottom">
                                    <center>
                                        <asp:Image ng-hide="item.DocComentario==''" ImageUrl="~/images/notepad_16x16.gif"
                                            ID="imgcomentariodoc" runat="server" rel="tooltip" title='{{item.DocComentario}}'
                                            Style="cursor: hand;" /></center>
                                </td>
                                <td class="tdSimple" align="center" style="width: 25px; vertical-align: bottom">
                                    <center>
                                        <asp:Image ng-hide="item.HojaComentario==''" ImageUrl="~/images/notepad_16x16.gif"
                                            ID="imgcomentarioitem" runat="server" rel="tooltip" title="{{item.HojaComentario}}"
                                            Style="cursor: hand;" /></center>
                                </td>
                                <td class="tdSimple" align="center" style="width: 25px; vertical-align: bottom">
                                    <center>
                                        <asp:Image ng-hide="item.HojaComentario==''" ImageUrl="~/images/notepad_16x16.gif"
                                            ID="Image1" runat="server" rel="tooltip" title="{{item.HojaComentario}}" Style="cursor: hand;" /></center>
                                </td>
                                <td class="tdSimple" align="center" style="width: 25px; vertical-align: bottom">
                                    <center>
                                        <input type="checkbox" ng-show="item.permitirAprobarItem==true" id="chkAprobo" originalvalue='{{item.HojaFechaAprobacion}}' />
                                        <asp:Image rel="tooltip" ng-show="item.permitirVerAuditadoPor==true" ImageUrl="~/images/AuditadoPor.gif"
                                            ID="imagenAuditadoPor" runat="server" title="Aprobado por: {{item.AuditadoPor}}"
                                            Style="cursor: hand;" />
                                    </center>
                                </td>
                                <td class="tdSimple" align="center" style="width: 75px; vertical-align: bottom">
                                    <center>
                                        {{item.HojaFechaAprobacion}}</center>
                                </td>
                                <td class="tdSimple" align="center" style="width: 25px; vertical-align: bottom">
                                    <center>
                                        <input type="checkbox" ng-disabled="item.permitirTerminarAuditoria==false" ng-model="item.AuditoriaTerminada" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8" style="padding: 2px; background-color: Gray">
                                    <asp:Button ID="btnAplicar" runat="server" SkinID="btnConosudBasic" Text="Aplicar Cambios" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">


        $(document).ready(function () {
            window.setTimeout(function () {
                if ($("[rel=tooltip]").length > 0) {
                    $("[rel=tooltip]").tooltip();
                }
            }, 200);
        });

    </script>
</asp:Content>
