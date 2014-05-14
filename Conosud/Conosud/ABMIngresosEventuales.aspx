<%@ Page Title="" Language="C#" Theme="MiTema" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="ABMIngresosEventuales.aspx.cs" Inherits="ABMIngresosEventuales" %>

<%@ Register Assembly="ControlsAjaxNotti" Namespace="ControlsAjaxNotti" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .RadWindow.RadWindow_Sunset.rwNormalWindow.rwTransparentWindow
        {
            z-index: 999900000 !important;
        }
        .TdIngresos
        {
        }
    </style>
    <script type="text/javascript">
        var options, ctrContrato;
        var ctrEmpresa, ctrApellido, ctrDNI, ctrActividad, ctrCitado;

        function EliminarIngresoEventual(sender, Id) {

            radconfirm("Esta seguro que desea eliminar los datos?", ConfirmDelete, 300, 100, null, "Eliminación");

            function ConfirmDelete(result) {

                if (result) {
                    var patente = $find("<%= txtApellidoBusqueda.ClientID %>").get_value();
                    PageMethods.EliminarRegistro(Id, patente, ($find("<%=GridIngresos.ClientID %>").get_pageSize() * $find("<%=GridIngresos.ClientID %>").get_currentPageIndex()), $find("<%=GridIngresos.ClientID %>").get_pageSize(), FillGrid, Error);

                }
            }
        }

        function EditarIngresoEventual(sender, Id) {
            showResultado = false;
            $find("<%=pnlEdicion.ClientID %>").ClearElements();

            $find("<%=GridIngresos.ClientID %>").initEdit($find("<%=ServerControlWindow1.ClientID %>"), Id);

            $find("<%=ServerControlWindow1.ClientID %>").set_CollectionDiv('divPrincipal');
            $find("<%=ServerControlWindow1.ClientID %>").ShowWindows('divPrincipal', "Edición Ingreso Eventual");

            if ("<%=EsContratista %>" == "True" || "<%=PoseePermisoSoloLectura %>" == "True")
                $find("<%=pnlEdicion.ClientID %>").DisabledElement(true);


            var value = $('#' + "<%= txtEmpresa.ClientID %>").val();

            if (ctrContrato != null) {
                ctrContrato.changeParams({ Tipo: 'C', Empresa: value });
            }
            else {
                options = {
                    serviceUrl: 'LoadEmpresas.ashx',
                    width: '284',
                    showInit: true,
                    IsEdit: true,
                    params: { Tipo: 'C', Empresa: value },
                    zIndex: 922000000
                };


                ctrContrato = $('#' + "<%= txtContrato.ClientID %>").autocomplete(options); 


            }
        }

        function NuevoIngresoEventual() {

            $find("<%=pnlEdicion.ClientID %>").ClearElements();
            $find("<%=ServerControlWindow1.ClientID %>").set_CollectionDiv('divPrincipal');
            $find("<%=ServerControlWindow1.ClientID %>").ShowWindows('divPrincipal', "Nuevo Ingreso Eventual");
        }



        function BuscarDatos() {

            var patente = $find("<%= txtApellidoBusqueda.ClientID %>").get_value();
            var dni = $find("<%= txtDniBusqueda.ClientID %>").get_value();
            PageMethods.GetData(patente,dni, ($find("<%=GridIngresos.ClientID %>").get_pageSize() * $find("<%=GridIngresos.ClientID %>").get_currentPageIndex()), $find("<%=GridIngresos.ClientID %>").get_pageSize(), FillGrid, Error);

        }




        jQuery(function () {
            options = {
                serviceUrl: 'LoadEmpresas.ashx',
                width: '384',
                onSelect: ShowContratos,
                params: { Tipo: 'E' },
                zIndex: 922000000
            };
            ctrEmpresa = $('#' + "<%= txtEmpresa.ClientID %>").autocomplete(options);


            options = {
                serviceUrl: 'BusquedaDatosIngresosEventuales.ashx',
                width: '384',
                params: { Tipo: 'Ape' },
                zIndex: 922000000
            };
            ctrApellido = $('#' + "<%= txtApellidoNombre.ClientID %>").autocomplete(options);


            options = {
                serviceUrl: 'BusquedaDatosIngresosEventuales.ashx',
                width: '384',
                params: { Tipo: 'DNI' },
                zIndex: 922000000
            };
            ctrDNI = $('#' + "<%= txtDNI.ClientID %>").autocomplete(options); 

            options = {
                serviceUrl: 'BusquedaDatosIngresosEventuales.ashx',
                width: '384',
                params: { Tipo: 'Act' },
                zIndex: 922000000
            };
            ctrActividad = $('#' + "<%= txtActividad.ClientID %>").autocomplete(options); 


            options = {
                serviceUrl: 'BusquedaDatosIngresosEventuales.ashx',
                width: '384',
                params: { Tipo: 'Cit' },
                zIndex: 922000000
            };
            ctrCitado = $('#' + "<%= txtCitadopor.ClientID %>").autocomplete(options); 



        });

        function ShowContratos(value, data) {
            $('#' + "<%= txtContrato.ClientID %>").val("");
            $('#' + "<%= txtContrato.ClientID %>").html("");

            if (ctrContrato != null) {
                ctrContrato.changeParams({ Tipo: 'C', Empresa: value });
            }
            else {
                options = {
                    serviceUrl: 'LoadEmpresas.ashx',
                    width: '284',
                    showInit: true,
                    IsEdit: true,
                    params: { Tipo: 'C', Empresa: value },
                    zIndex: 922000000
                };


                ctrContrato = $('#' + "<%= txtContrato.ClientID %>").autocomplete(options); 


            }

            $('#' + "<%= txtContrato.ClientID %>").focus();
        }

    </script>
    <script type="text/javascript">
        var IdVehiculoEquipo = "";
        var showResultado;
        var clearSession = false;

        function FillGrid(result) {
            $find("<%=ServerControlWindow1.ClientID %>").CloseWindows('divPrincipal');
            $find("<%=GridIngresos.ClientID %>").set_ClientdataSource(result);

            if (ctrContrato != null) {
                ctrContrato.clearSession();
            }

            if (ctrEmpresa != null) {
                ctrEmpresa.clearSession();
                ctrApellido.clearSession();
                ctrDNI.clearSession();
                ctrActividad.clearSession();
                ctrCitado.clearSession();
            }
        }

        function Error(err) {
            alert(err.get_message());
        }

        function AplicarCambios() {

            if (Page_ClientValidate()) {
                $find("<%=ServerControlWindow1.ClientID %>").ShowWaiting(false, 'Grabando Cambios...');

                var result = $find("<%=GridIngresos.ClientID %>").getValuesEdit($find("<%=pnlEdicion.ClientID %>"));
                var id = $find("<%=GridIngresos.ClientID %>").get_KeyValueSelected();
                if (id == undefined) {
                    id = "";
                    clearSession = true;
                }
                else
                    clearSession = false;

                PageMethods.Grabar(result, id, ($find("<%=GridIngresos.ClientID %>").get_pageSize() * $find("<%=GridIngresos.ClientID %>").get_currentPageIndex()), $find("<%=GridIngresos.ClientID %>").get_pageSize(), FillGrid, Error);
            }
        }

        function lTrim0(sStr) {
            while (sStr.charAt(0) == '0')
                sStr = sStr.substr(1, sStr.length - 1);
            return sStr;
        }
       
    </script>
   
    <table cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td align="left">
                <table cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td align="center" style="height: 35px; padding-left: 15px; padding-top: 15px">
                            <asp:Label ID="lblTipoGestion" runat="server" Font-Bold="True" Font-Size="20pt" Font-Underline="false"
                                Font-Italic="True" ForeColor="black" Text="GESTION DE INGRESOS EVENTUALES" Font-Names="Arno Pro"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <table style="background-color: transparent; font-family: Sans-Serif; font-size: 11px;
                                        width: 100%; vertical-align: middle;" border="0">
                                        <tr>
                                            <td valign="middle" align="right" style="padding-left: 10px; width: 15%">
                                                <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Names="Arno Pro" ForeColor="#8C8C8C"
                                                    Font-Size="16px" Text="Apellido:"></asp:Label>
                                            </td>
                                            <td valign="middle" align="left" width="310px">
                                                <telerik:RadTextBox ID="txtApellidoBusqueda" runat="server" EmptyMessage="Ingrese el Apellido para buscar"
                                                    Skin="Sunset" Width="100%">
                                                </telerik:RadTextBox>
                                            </td>
                                            <td valign="middle" align="right" style="width: 50px">
                                                <asp:Label ID="Label12" runat="server" Font-Bold="True" Font-Names="Arno Pro" ForeColor="#8C8C8C"
                                                    Font-Size="16px" Text="DNI:"></asp:Label>
                                            </td>
                                            <td valign="middle" align="left" width="280px">
                                                <telerik:RadTextBox ID="txtDniBusqueda" runat="server" EmptyMessage="Ingrese el DNI para buscar"
                                                    Skin="Sunset" Width="100%">
                                                </telerik:RadTextBox>
                                            </td>
                                            <td valign="middle" align="left">
                                                <asp:ImageButton runat="server" Style="padding-left: 15px; padding-bottom: 15px;
                                                    border: 0px; vertical-align: middle;" ImageUrl="~/Images/Search.png" ID="imgBuscar"
                                                    OnClientClick="BuscarDatos();return false;" />
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
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table id="Table2" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
                    border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #E0D6BE;
                    font-family: Sans-Serif; font-size: 11px;" width="90%">
                    <tr>
                        <td align="center">
                            <cc1:ClientControlGrid ID="GridIngresos" runat="server" AllowMultiSelection="false"
                                TypeSkin="Sunset" PositionAdd="Top" AllowRowSelection="true" Width="100%" KeyName="IdIngresoEventual"
                                AllowPaging="false" PageSize="8" EmptyMessage="No Existen Ingresos Eventuales">
                                <FunctionsGral>
                                    <cc1:FunctionGral Type="Add" Text="Crear Nuevo Ingreso" ClickFunction="NuevoIngresoEventual" />
                                    <cc1:FunctionGral Type="Excel" Text="Exportar Datos" />
                                </FunctionsGral>
                                <FunctionsColumns>
                                    <cc1:FunctionColumnRow Type="Delete" ClickFunction="EliminarIngresoEventual" Text="Eliminar Datos" />
                                    <cc1:FunctionColumnRow Type="Edit" ClickFunction="EditarIngresoEventual" Text="Ver Datos" />
                                </FunctionsColumns>
                                <Columns>
                                    <cc1:Column CssClass="tdSimple" HeaderName="Fecha Solicitud" DataFieldName="FechaSolicitud"
                                        Align="Centrado" NameControlManger="txtFechaSolicitud" ExportToExcel="true" />
                                    <cc1:Column CssClass="tdSimple" HeaderName="Apellido y Nombre" DataFieldName="ApellidoNombre"
                                        Align="Centrado" NameControlManger="txtApellidoNombre" ExportToExcel="true" />
                                    <cc1:Column CssClass="tdSimple" HeaderName="DNI" DataFieldName="DNI" Align="Centrado"
                                        NameControlManger="txtDNI" ExportToExcel="true" />
                                    <cc1:Column CssClass="tdSimple" HeaderName="Empresa" DataFieldName="Empresa" Align="Centrado"
                                        NameControlManger="txtEmpresa" ExportToExcel="true" />
                                    <cc1:Column CssClass="tdSimple" HeaderName="Contrato" DataFieldName="Contrato" Align="Centrado"
                                        NameControlManger="txtContrato" ExportToExcel="true" />
                                    <cc1:Column CssClass="tdSimple" HeaderName="Actividad" DataFieldName="Actividad"
                                        Display="false" Align="Centrado" NameControlManger="txtActividad" ExportToExcel="true" />
                                    <cc1:Column CssClass="tdSimple" HeaderName="Citadopor" DataFieldName="Citadopor"
                                        Display="false" Align="Centrado" NameControlManger="txtCitadopor" ExportToExcel="true" />
                                    <cc1:Column HeaderName="FI1" DataFieldName="FechaIngreso1" Display="true" NameControlManger="txtFechaIngreso1"
                                        ExportToExcel="true" />
                                    <cc1:Column HeaderName="FI2" DataFieldName="FechaIngreso2" Display="true" NameControlManger="txtFechaIngreso2"
                                        ExportToExcel="true" />
                                    <cc1:Column HeaderName="FI3" DataFieldName="FechaIngreso3" Display="true" NameControlManger="txtFechaIngreso3"
                                        ExportToExcel="true" />
                                    <cc1:Column HeaderName="FI4" DataFieldName="FechaIngreso4" Display="true" NameControlManger="txtFechaIngreso4"
                                        ExportToExcel="true" />
                                    <cc1:Column HeaderName="FI5" DataFieldName="FechaIngreso5" Display="true" NameControlManger="txtFechaIngreso5"
                                        ExportToExcel="true" />
                                </Columns>
                            </cc1:ClientControlGrid>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="upEdicion" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <cc1:ServerControlWindow ID="ServerControlWindow1" runat="server" BackColor="WhiteSmoke"
                WindowColor="Rojo">
                <ContentControls>
                    <cc1:ServerPanel runat="server" ID="pnlEdicion">
                        <BodyContent>
                            <div id="divPrincipal" style="height: 300px; width: 780px">
                                <table border="0" cellpadding="0" cellspacing="2" width="100%" style="padding-top: 0px;
                                    text-align: left;">
                                    <tr>
                                        <td colspan="4" style="background-color: #CACACA; padding-left: 10px; height: 30px;">
                                            <asp:Label ID="lblTituloCaractesticas" runat="server" SkinID="lblConosud" Text="DATOS PRINCIPALES"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="padding-top: 5px">
                                        <td style="width: 250px">
                                            <asp:Label ID="Label2" runat="server" SkinID="lblConosud" Text="Fecha Solicitud:"></asp:Label>
                                        </td>
                                        <td colspan="3" align="left" style="padding-left: 5px; padding-right: 5px">
                                            <telerik:RadDatePicker ID="txtFechaSolicitud" MinDate="1950/1/1" runat="server" ZIndex="922000000">
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatortxtFechaSolicitud" ControlToValidate="txtFechaSolicitud"
                                                        Display="Dynamic" ErrorMessage="*" runat="server">
                                                    </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label14" runat="server" SkinID="lblConosud" Text="Apellido y Nombre:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            
                                           <asp:TextBox Width="250px" ID="txtApellidoNombre" runat="server" ></asp:TextBox>
                                           <asp:RequiredFieldValidator ID="RequiredFieldValidatortxtApellidoNombre" ControlToValidate="txtApellidoNombre"
                                                        Display="Dynamic" ErrorMessage="*" runat="server">
                                                    </asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" SkinID="lblConosud" Text="DNI:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                           <asp:TextBox Width="120px" ID="txtDNI" runat="server" ></asp:TextBox>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidatortxtDNI" ControlToValidate="txtDNI"
                                                        Display="Dynamic" ErrorMessage="*" runat="server">
                                                    </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" SkinID="lblConosud" Text="Empresa:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <asp:TextBox Width="250px" ID="txtEmpresa" runat="server" ></asp:TextBox>
                                               <asp:RequiredFieldValidator ID="RequiredFieldValidatortxtEmpresa" ControlToValidate="txtEmpresa"
                                                        Display="Dynamic" ErrorMessage="*" runat="server">
                                                    </asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 180px">
                                            <asp:Label ID="Label5" runat="server" SkinID="lblConosud" Text="Contrato:"></asp:Label>

                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <asp:TextBox Width="250px" ID="txtContrato" runat="server" ></asp:TextBox>
                                               <asp:RequiredFieldValidator ID="RequiredFieldValidatortxtContrato" ControlToValidate="txtContrato"
                                                        Display="Dynamic" ErrorMessage="*" runat="server">
                                                    </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label7" runat="server" SkinID="lblConosud" Text="Actividad:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                              <asp:TextBox Width="250px" ID="txtActividad" runat="server" ></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label8" runat="server" SkinID="lblConosud" Text="Citado por:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                              <asp:TextBox Width="250px" ID="txtCitadopor" runat="server" ></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="background-color: #CACACA; padding-left: 10px; height: 30px">
                                            <asp:Label ID="Label24" runat="server" SkinID="lblConosud" Text="FECHAS INGRESO"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="padding-top: 5px">
                                        <td colspan="4" align="left" style="padding-left: 5px; padding-right: 5px">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" style="text-align: left;">
                                                <tr>
                                                    <td style="width: 50px">
                                                        <asp:Label ID="Label19" runat="server" SkinID="lblConosud" Text="FI1:"></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <telerik:RadDatePicker ID="txtFechaIngreso1" MinDate="1950/1/1" runat="server" ZIndex="922000000">
                                                        </telerik:RadDatePicker>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatortxtFechaIngreso1" ControlToValidate="txtFechaIngreso1"
                                                        Display="Dynamic" ErrorMessage="*" runat="server">
                                                    </asp:RequiredFieldValidator>
                                                    </td>
                                                    <td style="width: 50px">
                                                        <asp:Label ID="Label1" runat="server" SkinID="lblConosud" Text="FI2:"></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <telerik:RadDatePicker ID="txtFechaIngreso2" MinDate="1950/1/1" runat="server" ZIndex="922000000">
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                    <td style="width: 50px">
                                                        <asp:Label ID="Label3" runat="server" SkinID="lblConosud" Text="FI3:"></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <telerik:RadDatePicker ID="txtFechaIngreso3" MinDate="1950/1/1" runat="server" ZIndex="922000000">
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="padding-top: 5px">
                                        <td colspan="4" align="left" style="padding-left: 5px; padding-right: 5px">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" style="text-align: left;">
                                                <tr>
                                                    <td style="width: 100px">
                                                        <asp:Label ID="Label9" runat="server" SkinID="lblConosud" Text="FI4:"></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <telerik:RadDatePicker ID="txtFechaIngreso4" MinDate="1950/1/1" runat="server" ZIndex="922000000">
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                    <td style="width: 100px">
                                                        <asp:Label ID="Label11" runat="server" SkinID="lblConosud" Text="FI5:"></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <telerik:RadDatePicker ID="txtFechaIngreso5" MinDate="1950/1/1" runat="server" ZIndex="922000000">
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="center" style="padding-top: 5px">
                                            <asp:Button ID="btnAplicar" OnClientClick="AplicarCambios();return false;" runat="server"
                                                Text="Grabar" SkinID="btnConosudBasic" CausesValidation="true" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </BodyContent>
                    </cc1:ServerPanel>
                </ContentControls>
            </cc1:ServerControlWindow>
        </ContentTemplate>
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
