<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true"
    CodeFile="ABMLegajos.aspx.cs" Inherits="ABMLegajos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">

        var myAppModule = angular.module('myApp', []);

        myAppModule.service('PageMethods', function ($http) {


            this.ConsultarLegajos = function (apellido) {
                return $http({
                    method: 'POST',
                    url: 'ABMLegajos.aspx/GetLegajos',
                    data: { filtroApellido: apellido, FiltroDNI: "", TipoUsuario: "", IdEmpresa: "", take: 15 },
                    contentType: 'application/json; charset=utf-8'
                });
            };

        });

        myAppModule.controller('LegCtrl', function ($scope, PageMethods) {
            $scope.Legajos= '';
            $scope.Documento = '26055345';
            $scope.Apellido = '';
            $scope.BuscarLegajos = function () {

                PageMethods.ConsultarLegajos($scope.Apellido)
                    .then(function (response) {
                        $scope.Legajos = response.data.d;
                    });

                

            };

        });


      
    </script>
    <div ng-app="myApp" ng-controller="LegCtrl">
        <table cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr>
                <td align="center" style="height: 35px; padding-left: 15px; padding-top: 10px">
                    <asp:Label ID="lblTipoGestion" runat="server" Font-Bold="True" Font-Size="20pt" Font-Underline="false"
                        Font-Italic="True" ForeColor="black" Text="GESTION DE LEGAJOS" Font-Names="Arno Pro"></asp:Label>
                </td>
            </tr>
        </table>
        <table style="background-color: transparent; font-family: Arno Pro; font-size: 16px;
            width: 100%; vertical-align: middle;" border="0">
            <tr>
                <td valign="middle" align="right" style="width: 240px">
                    <asp:Label ID="lblLegajo" runat="server" Font-Bold="True" ForeColor="#8C8C8C" Height="22px"
                        Text="Apellido:"></asp:Label>
                </td>
                <td valign="middle" align="left" style="width: 220px">
                    <telerik:RadTextBox ID="txtApellidoLegajo" runat="server" EmptyMessage="Ingrese apellido "
                        Skin="Sunset" Width="100%" ng-model="Apellido">
                    </telerik:RadTextBox>
                </td>
                <td valign="middle" align="left" style="width: 60px; padding-left: 20px">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="#8C8C8C" Height="22px"
                        Text="NroDoc:"></asp:Label>
                </td>
                <td valign="middle" align="left" style="width: 220px">
                    <telerik:RadTextBox ID="txtNroDoc" runat="server" EmptyMessage="Ingrese Nro Doc"
                        Skin="Sunset" Width="100%" ng-model="Documento">
                    </telerik:RadTextBox>
                </td>
                <td valign="middle" align="left" style="padding-left: 20px">
                    <img src="images/Search.png" ng-click="BuscarLegajos()" />
                </td>
            </tr>
            <tr>
                <td colspan="5" align="center">
                    <div style="width: 95%; height: 8px; border-top-style: solid; border-top-width: 2px;
                        border-top-color: #808080;">
                        <table id="tblUsuarios" width="65%" class="TVista" border="0" cellpadding="0" cellspacing="0">
                                    <thead>
                                        <tr>
                                            <th class="Theader">
                                                Apellido
                                            </th>
                                            <th class="Theader">
                                                Codigo
                                            </th>
                                            <th class="Theader">
                                                CategoriaContrato
                                            </th>
                                            <th class="Theader">
                                                &nbsp;
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr class="trDatos" ng-repeat="legajo in Legajos | orderBy:'Apellido'">
                                            <td class="tdSimple" align="left">
                                                {{legajo.Apellido}}
                                            </td>
                                            <td class="tdSimple" align="center" style="width: 90px">
                                                {{legajo.dc.Codigo}}
                                            </td>
                                            <td class="tdSimple" align="center" style="width: 35px; vertical-align: bottom">
                                                 {{legajo.dc.CategoriaContrato}}
                                            </td>
                                            <td class="tdSimple" align="center" style="width: 35px; vertical-align: bottom" >
                                                <img src="Images/AddRecord.gif" width="14px" height="15px" style="cursor: pointer"
                                                    title="click: para Cambiar Datos del usuario" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
