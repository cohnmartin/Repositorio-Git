<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AsignacionSegContexto.aspx.cs"
    Inherits="AsignacionSegContexto" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Assembly="ControlsAjaxNotti" Namespace="ControlsAjaxNotti" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="App_Themes/MiTema/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/ui-select/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/ui-select/select.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/ui-select/selectize.default.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/ui-select/jquery.js" type="text/javascript"></script>
    <script src="Scripts/ui-select/angular.js" type="text/javascript"></script>
    <script src="Scripts/ui-select/angular-sanitize.js" type="text/javascript"></script>
    <script src="Scripts/ui-select/select.js" type="text/javascript"></script>
    <script src="Scripts/controllers/controller_Usuarios.js" type="text/javascript"></script>
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

            function CloseWindows() {

                var oWnd = GetRadWindow();
                var oArg = new Object();

                oWnd.close();

            }

            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow;
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                return oWindow;
            }

            var Constants = {
                usuarioEdicion: '<%= idUsuario %>',
                controlCombo: 'cboContratos'
            };
    
        </script>
    </telerik:RadScriptBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" AsyncPostBackTimeout="360000"
        EnablePageMethods="true" EnablePartialRendering="true">
    </telerik:RadScriptManager>
    <cc1:ServerControlWindow ID="ServerControlContratos" runat="server" BackColor="WhiteSmoke"
        WindowColor="Rojo">
        <ContentControls>
        </ContentControls>
    </cc1:ServerControlWindow>
    <div id="ng-app" ng-app="myApp" ng-controller="controller_Usuarios" style="text-align: center;
        padding-top: 5px; padding-left: 20px">
        <table width="950%" style="z-index: 9999999999" border="0">
            <tr>
                <td valign="middle" align="left">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="10pt" Font-Names="Sans-Serif"
                        Text="Contratos:"></asp:Label>
                </td>
                <td valign="top" align="left">
                    <ui-select ng-model="NewContrato.contrato" theme="selectize" ng-disabled="disabled"
                        style="width: 650px; z-index: 9999999999">
                                <ui-select-match placeholder="Seleccione un contrato de la lista...">{{$select.selected.Contrato}}&nbsp;{{$select.selected.Empresa}}</ui-select-match>
                                <ui-select-choices repeat="country in contratosDisponibles | filter: $select.search">
                                    <span ng-bind-html="country.Contrato | highlight: $select.search"></span>&nbsp;
                                     <small ng-bind-html="country.Empresa  | highlight: $select.search"></small>
                                </ui-select-choices>
                                </ui-select>
                </td>
                <td valign="middle" align="left" style="width: 50px">
                    <img src="Images/AddRecord.gif" style="cursor: pointer" title="agregar acceso al contrato"
                        ng-click="AgregarContrato()" />
                </td>
            </tr>
        </table>
        <br />
        <table id="tblContratosAsignados" width="95%" class="TSunset" border="0" cellpadding="0"
            cellspacing="0">
            <thead>
                <tr>
                    <th class="Theader">
                        Nro Contrato
                    </th>
                    <th class="Theader">
                        Empresa
                    </th>
                    <th class="Theader">
                        Vencimiento
                    </th>
                    <th class="Theader">
                        &nbsp;
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr class="trDatos" ng-repeat="contrato in ContratosAsignados">
                    <td class="tdSimple" align="left">
                        {{contrato.NroContrato}}
                    </td>
                    <td class="tdSimple" align="center" style="width: 350px">
                        {{contrato.Empresa}}
                    </td>
                    <td class="tdSimple" align="center" style="width: 115px; vertical-align: bottom">
                        {{contrato.Vencimiento}}
                    </td>
                    <td class="tdSimple" align="center" style="width: 25px; vertical-align: bottom">
                        <img src="Images/delete.gif" style="cursor: pointer" title="Eliminar acceso al contrato"
                            ng-click="EliminarContrato(contrato.idSegContexto)" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
