<%@ Page Title="Gestion de Vehículos" Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="ABMVehiculosEquipos.aspx.cs" Inherits="ABMVehiculosEquipos" %>

<%@ Register Assembly="ControlsAjaxNotti" Namespace="ControlsAjaxNotti" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        var IdVehiculoEquipo = "";
        var showResultado;
        function FillGridFilter(result) {


            $find("<%=ServerControlWindow1.ClientID %>").CloseWindows('divPrincipal');
            $find("<%=GridVehiculos.ClientID %>").ChangeClientVirtualCount(result["Cantidad"]);
            $find("<%=GridVehiculos.ClientID %>").set_ClientdataSource(result["Datos"]);

        }

        function FillGrid(result) {

            $find("<%=ServerControlWindow1.ClientID %>").CloseWindows('divPrincipal');
            $find("<%=GridVehiculos.ClientID %>").set_ClientdataSource(result["Datos"]);

        }

        function Error(err) {
            alert(err.get_message());
        }

        function AplicarCambios() {

            var result = $find("<%=GridVehiculos.ClientID %>").getValuesEdit($find("<%=pnlEdicion.ClientID %>"));
            var id = $find("<%=GridVehiculos.ClientID %>").get_KeyValueSelected();
            var FechaVencimientoContrato = "";

            if (id == undefined) {
                id = "";
                FechaVencimientoContrato = result["NroContrato"] != null && result["NroContrato"].split(':').length > 0 ? result["NroContrato"].split(':')[1].trim() : "";
            }
            else {
                if ($find("<%=GridVehiculos.ClientID %>").get_ItemDataByKey(id).FechaVencimientoContrato != undefined)
                    FechaVencimientoContrato = $find("<%=GridVehiculos.ClientID %>").get_ItemDataByKey(id).FechaVencimientoContrato.format("dd/MM/yyyy");
                else
                    FechaVencimientoContrato = Date("10/10/2080");
            }

            var fechaMinima = FechaMinimaValida(result, FechaVencimientoContrato);
            if (fechaMinima == null) {
                $find("<%=ServerControlWindow1.ClientID %>").ShowWaiting(false, 'Grabando Cambios...');
                PageMethods.Grabar(result, id, ($find("<%=GridVehiculos.ClientID %>").get_pageSize() * $find("<%=GridVehiculos.ClientID %>").get_currentPageIndex()), $find("<%=GridVehiculos.ClientID %>").get_pageSize(), FillGrid, Error);
            }
            else {
                alert("La Fecha de Vencimiento de la Credencial no es válida, la misma es MAYOR a la fecha mínima: " + fechaMinima);
            }

            return;

//            $find("<%=ServerControlWindow1.ClientID %>").ShowWaiting(false, 'Grabando Cambios...');

//            var result = $find("<%=GridVehiculos.ClientID %>").getValuesEdit($find("<%=pnlEdicion.ClientID %>"));
//            var id = $find("<%=GridVehiculos.ClientID %>").get_KeyValueSelected();
//            if (id == undefined)
//                id = "";

//            PageMethods.Grabar(result, id, ($find("<%=GridVehiculos.ClientID %>").get_pageSize() * $find("<%=GridVehiculos.ClientID %>").get_currentPageIndex()), $find("<%=GridVehiculos.ClientID %>").get_pageSize(), FillGrid, Error);
        }

        function FechaMinimaValida(datos, FechaVencimientoContrato) {
            var date_sort_asc = function (date1, date2) {
                // This is a comparison function that will result in dates being sorted in  
                // ASCENDING order. As you can see, JavaScript's native comparison operators  
                // can be used to compare dates. This was news to me. 
                if (date1 > date2) return 1;
                if (date1 < date2) return -1;
                return 0;
            };


            var arrayFecha = new Array();
            var fechaMinima = null;
            var fechaVenciminetoContrato = TransformDate($find("<%=cboContrato.ClientID %>").get_selectedItem() != null ? $find("<%=cboContrato.ClientID %>").get_selectedItem().get_attributes().getAttribute("FechaMinima") : FechaVencimientoContrato);
            var fechaSeguro = TransformDate(datos.FechaVencimientoSeguro);
            var fechaCENT = TransformDate(datos.FechaVencimientoHabilitacion);
            var fechaUltimoPagoSeguro = TransformDate(datos.FechaUltimoPagoSeguro);
            var fechaEE = TransformDate(datos.FechaVencimientoHabilitacionEE);
            var fechaIngresada = TransformDate(datos.VencimientoCredencial);
            

            

            if (fechaSeguro != null) { arrayFecha.push(fechaSeguro) };
            if (fechaCENT != null) { arrayFecha.push(fechaCENT) };
            if (fechaVenciminetoContrato != null) { arrayFecha.push(fechaVenciminetoContrato) };
            if (fechaUltimoPagoSeguro != null) { arrayFecha.push(fechaUltimoPagoSeguro) };
            if (fechaEE != null) { arrayFecha.push(fechaEE) };

            arrayFecha.sort(date_sort_asc);
            fechaMinima = arrayFecha[0];

            if (fechaIngresada != undefined && fechaIngresada > fechaMinima) {
                var strDesc = " correspondiente al ";
                if (fechaMinima == fechaVenciminetoContrato)
                    strDesc += " vencimiento del contrato";
                else if (fechaMinima == fechaSeguro)
                    strDesc += " vencimiento del seguro";
                else if (fechaMinima == fechaCENT)
                    strDesc += " vencimiento de la habilitación CENT";
                else if (fechaMinima == fechaUltimoPagoSeguro)
                    strDesc += " vencimiento otorgado por el último pago del seguro";
                else if (fechaMinima == fechaEE)
                    strDesc += " vencimiento de la habilitación Equipos Elevación";


                return fechaMinima.format("dd/MM/yyyy") + strDesc;
            }
            else
                return null;

        }



        function TransformDate(date) {
            if (date != undefined) {
                var dia = parseFloat(date.substr(0, 2));
                var mes = parseFloat(date.substr(3, 2)) - 1;
                var año = parseFloat(date.substr(6));

                return new Date(año, mes, dia);
            }
            else
                return null;
        }

        function CambioPagina(sender, page) {

            var skip = $find("<%=GridVehiculos.ClientID %>").get_pageSize() * $find("<%=GridVehiculos.ClientID %>").get_currentPageIndex();
            var take = $find("<%=GridVehiculos.ClientID %>").get_pageSize();

            PageMethods.GetData("", skip, take, FillGrid, Error);
        }

        function lTrim0(sStr) {
            while (sStr.charAt(0) == '0')
                sStr = sStr.substr(1, sStr.length - 1);
            return sStr;
        }

    </script>
    <script type="text/javascript">
        function EliminarVehiculo(sender, Id) {

            radconfirm("Esta seguro que desea eliminar los datos?", ConfirmDelete, 300, 100, null, "Eliminación");

            function ConfirmDelete(result) {

                if (result) {
                    var patente = $find("<%= txtNroPatente.ClientID %>").get_value();
                    PageMethods.EliminarRegistro(Id, patente, ($find("<%=GridVehiculos.ClientID %>").get_pageSize() * $find("<%=GridVehiculos.ClientID %>").get_currentPageIndex()), $find("<%=GridVehiculos.ClientID %>").get_pageSize(), FillGrid, Error);

                }
            }
        }

        function EditarVehiculo(sender, Id) {
            showResultado = false;
            $find("<%= cboContrato.ClientID %>").clearItems();
            $find("<%= cboContrato.ClientID %>").clearSelection();

            $find("<%=pnlEdicion.ClientID %>").ClearElements();

            $find("<%=GridVehiculos.ClientID %>").initEdit($find("<%=ServerControlWindow1.ClientID %>"), Id);

            $find("<%=ServerControlWindow1.ClientID %>").set_CollectionDiv('divPrincipal');
            $find("<%=ServerControlWindow1.ClientID %>").ShowWindows('divPrincipal', "Edición " + "<%= TipoAlta %>");

            if ("<%=EsContratista %>" == "True" || "<%=PoseePermisoSoloLectura %>" == "True")
                $find("<%=pnlEdicion.ClientID %>").DisabledElement(true);
        }

        function NuevoVehiculo() {

            $find("<%=pnlEdicion.ClientID %>").ClearElements();
            $find("<%=ServerControlWindow1.ClientID %>").set_CollectionDiv('divPrincipal');
            $find("<%=ServerControlWindow1.ClientID %>").ShowWindows('divPrincipal', "Nuevo " + "<%= TipoAlta %>");
        }

        function ShowNewCredencial(sender, id) {
            if (id != "") {

                var HabilitarCredencial = $find("<%=GridVehiculos.ClientID %>").get_ItemDataByKey(id).DescHabilitarCredencial;

                if (HabilitarCredencial.trim() == "NO") {
                    radalert("La impresión de la credencial no se encuentra habilitada para el vehículo seleccionado.", 300, 100, "Impresión Credencial");
                    return;
                }


                var url = "ViewerCredenciales.aspx?IdVehiculoEquipo=" + id;
                var name = "RadWindowCredencial";
                var manager = $find("<%=RadWindowManagerVehiculos.ClientID %>");
                var oWnd = manager.open(url, name);

                //                //return false;
                //                var oWnd = radopen('ViewerCredenciales.aspx?IdVehiculoEquipo=' + id, 'RadWindowCredencial');
            }
            else {
                radalert("Debe seleccionar un Vehiculo o Equipo para ver su Credencial", 250, 100, "Selección");
            }
        }

        function BuscarDatos() {

            var patente = $find("<%= txtNroPatente.ClientID %>").get_value();
            PageMethods.GetData(patente, ($find("<%=GridVehiculos.ClientID %>").get_pageSize() * $find("<%=GridVehiculos.ClientID %>").get_currentPageIndex()), $find("<%=GridVehiculos.ClientID %>").get_pageSize(), FillGridFilter, Error);

        }

        function CargarContratosySeguros(combo, eventarqs) {
            var contratosCombo = $find("<%= cboContrato.ClientID %>");

            var item = eventarqs.get_item();

            if (item != null) {
                contratosCombo.set_text("Loading...");

                if (item.get_index() >= 0) {
                    showResultado = true;
                    contratosCombo.requestItems(item.get_value(), false);
                }
                else {
                    contratosCombo.set_text(" ");
                    contratosCombo.clearItems();
                }
            }
        }

        function ItemsLoaded(combo, eventarqs) {

            var contratosCombo = $find("<%= cboContrato.ClientID %>");

            if (showResultado) {
                contratosCombo.set_text("");
                contratosCombo.showDropDown();
                showResultado = false;
            }
        }

        function SetearDatosEmpresa(sender, eventArgs) {

            var valorEmpresa = $find("<% =cboEmpresa.ClientID %>").get_value();
            var contratosCombo = $find("<%= cboContrato.ClientID %>");
            contratosCombo.requestItems(valorEmpresa, false);

        }
    </script>
    <telerik:RadWindowManager ID="RadWindowManagerVehiculos" runat="server" Skin="Sunset"
        VisibleTitlebar="true" Style="z-index: 100000000" Title="Sub Contratistas">
        <Windows>
            <telerik:RadWindow ID="RadWindowCredencial" runat="server" Behaviors="Close" Width="750"
                Height="440" Modal="true" NavigateUrl="ViewerCredenciales.aspx" VisibleTitlebar="true"
                Style="z-index: 100000000" Title="Impresion Credencial" ReloadOnShow="true" VisibleStatusbar="false"
                ShowContentDuringLoad="false" Skin="Sunset">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <table cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td align="left">
                <table cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td align="center" style="height: 35px; padding-left: 15px; padding-top: 15px">
                            <asp:Label ID="lblTipoGestion" runat="server" Font-Bold="True" Font-Size="20pt" Font-Underline="false"
                                Font-Italic="True" ForeColor="black" Text="Gestión de" Font-Names="Arno Pro"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
                                <contenttemplate>
                                    <table style="background-color: transparent; font-family: Sans-Serif; font-size: 11px;
                                        width: 100%; vertical-align: middle;" border="0">
                                        <tr>
                                            <td valign="middle" align="right" style="padding-left: 10px; width: 340px">
                                                <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Names="Arno Pro" ForeColor="#8C8C8C"
                                                    Font-Size="16px" Text="Dominio:"></asp:Label>
                                            </td>
                                            <td valign="middle" align="left" width="310px">
                                                <telerik:RadTextBox ID="txtNroPatente" runat="server" EmptyMessage="Ingrese el Dominio para realizar busqueda"
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
                                            <td colspan="3" align="center">
                                                <div style="width: 95%; height: 8px; border-top-style: solid; border-top-width: 2px;
                                                    border-top-color: #808080;">
                                                    &nbsp;
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </contenttemplate>
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
                            <cc1:ClientControlGrid ID="GridVehiculos" runat="server" AllowMultiSelection="false"
                                onClientChangePageIndex="CambioPagina" TypeSkin="Sunset" PositionAdd="Top" AllowRowSelection="true"
                                Width="100%" KeyName="IdVehiculoEquipo" AllowPaging="true" PageSize="8" EmptyMessage="No se encuentran datos registrados">
                                <FunctionsGral>
                                    <cc1:FunctionGral Type="Add" Text="Crear Nuevo" ClickFunction="NuevoVehiculo" />
                                    <cc1:FunctionGral Type="Custom" Text="Credencial" ImgUrl="Images/notepad_16x16.gif"
                                        ClickFunction="ShowNewCredencial" />
                                    <cc1:FunctionGral Type="Excel" Text="Exportar Datos" />
                                </FunctionsGral>
                                <FunctionsColumns>
                                    <cc1:FunctionColumnRow Type="Delete" ClickFunction="EliminarVehiculo" Text="Eliminar Datos" />
                                    <cc1:FunctionColumnRow Type="Edit" ClickFunction="EditarVehiculo" Text="Ver Datos" />
                                </FunctionsColumns>
                                <Columns>
                                    <cc1:Column CssClass="tdSimple" HeaderName="Nro Patente" DataFieldName="Patente"
                                        Align="Centrado" NameControlManger="txtPatente" ExportToExcel="true" />
                                    <cc1:Column CssClass="tdSimple" HeaderName="Nro Interno" DataFieldName="NroInterno"
                                        Align="Centrado" NameControlManger="txtNroInterno" ExportToExcel="true" Display="false" />

                                    <cc1:Column CssClass="tdSimple" HeaderName="Tipo Unidad" DataFieldName="DescTipoUnidad"
                                        DataFieldNameValueCombo="TipoUnidad" Align="Derecha" NameControlManger="cboTipoUnidad"
                                        ExportToExcel="true" />
                                    <cc1:Column Capitalice="true" CssClass="tdSimple" HeaderName="Marca" DataFieldName="Marca"
                                        Align="Derecha" NameControlManger="txtMarca" ExportToExcel="true" />
                                    <cc1:Column Capitalice="true" CssClass="tdSimple" HeaderName="Empresa" DataFieldName="NombreEmpresa"
                                        DataFieldNameValueCombo="Empresa" Align="Derecha" NameControlManger="cboEmpresa"
                                        ExportToExcel="true" />
                                    <cc1:Column CssClass="tdSimple" HeaderName="Contrato" DataFieldName="NroContrato"
                                        DataFieldNameValueCombo="ContratoAfectado" Align="Derecha" NameControlManger="cboContrato"
                                        ExportToExcel="true" />
                                    <cc1:Column HeaderName="CapacidadCarga" DataFieldName="CapacidadCarga" Display="false"
                                        NameControlManger="txtCapacidad" ExportToExcel="true" />
                                    <cc1:Column HeaderName="FechaFabricacion" DataFieldName="FechaFabricacion" Display="false"
                                        NameControlManger="txtFechaFabricación" ExportToExcel="true" />
                                    <cc1:Column HeaderName="FechaHabilitacion" DataFieldName="FechaHabilitacion" Display="false"
                                        NameControlManger="txtFechaOtorgado" ExportToExcel="true" />
                                    <cc1:Column HeaderName="FechaVencimientoHabilitacion" DataFieldName="FechaVencimientoHabilitacion"
                                        Display="false" NameControlManger="txtFechaVencimiento" ExportToExcel="true" />
                                    <cc1:Column HeaderName="AltaEmpresa" DataFieldName="AltaEmpresa" Display="false"
                                        NameControlManger="txtFechaAltaEmpresa" ExportToExcel="true" />
                                    <cc1:Column HeaderName="BajaEmpresa" DataFieldName="BajaEmpresa" Display="false"
                                        NameControlManger="txtFechaBajaEmpresa" ExportToExcel="true" />
                                    
                                    

                                    <cc1:Column HeaderName="NroMotor" DataFieldName="NroMotor" Display="false" NameControlManger="txtNroMotor"
                                        ExportToExcel="true" />
                                    <cc1:Column HeaderName="NroChasis" DataFieldName="NroChasis" Display="false" NameControlManger="txtNroChasis"
                                        ExportToExcel="true" />
                                    <cc1:Column HeaderName="Modelo" DataFieldName="Modelo" Display="false" NameControlManger="txtModelo"
                                        ExportToExcel="true" />
                                    <cc1:Column HeaderName="NroHabilitacion" DataFieldName="NroHabilitacion" Display="false"
                                        NameControlManger="txtNroHabilitacion" ExportToExcel="true" />
                                    <cc1:Column HeaderName="Puesto Ingreso" DataFieldName="PuestoIngreso" Display="false"
                                        NameControlManger="txtPuestoIngreso" ExportToExcel="true" />
                                    <cc1:Column HeaderName="EsPropio" DataFieldName="EsPropio" Display="false" NameControlManger="chkPropio"
                                        ExportToExcel="true" />
                                    <cc1:Column HeaderName="NombreTitular" DataFieldName="NombreTitular" Display="false"
                                        NameControlManger="txtTitular" ExportToExcel="true" />
                                    <cc1:Column HeaderName="EquipamientoAgregado" DataFieldName="EquipamientoAgregado"
                                        Display="false" NameControlManger="txtEquipoAgregado" ExportToExcel="true" />
                                    
                                    <cc1:Column Align="Centrado" HeaderName="Venc. Credencial" DataFieldName="VencimientoCredencial"
                                        Display="true" NameControlManger="txtFechaVenCredencial" ExportToExcel="true" />

                                    <cc1:Column HeaderName="Hab. Cred." DataFieldName="HabilitarCredencial" Align="Centrado"
                                        Display="false" NameControlManger="chkHabilitarCred" />

                                    <cc1:Column HeaderName="Hab. Cred." DataFieldName="DescHabilitarCredencial" Align="Centrado"
                                        Display="true" ExportToExcel="true" />

                                    <cc1:Column HeaderName="NroPolizaSeguro" DataFieldName="NroPolizaSeguro" Display="false"
                                        NameControlManger="txtNroPoliza" ExportToExcel="true" />
                                    <cc1:Column HeaderName="DescCompañia" DataFieldName="DescCompañia" DataFieldNameValueCombo="CompañiaSeguro"
                                        Display="false" NameControlManger="cboCompañia" ExportToExcel="true" />
                                    <cc1:Column HeaderName="FechaInicialSeguro" DataFieldName="FechaInicialSeguro" Display="false"
                                        NameControlManger="txtFechaInicio" ExportToExcel="true" />
                                    <cc1:Column HeaderName="FechaVencimientoSeguro" DataFieldName="FechaVencimientoSeguro"
                                        Display="false" NameControlManger="txtFechaVenicimiento" ExportToExcel="true" />
                                    <cc1:Column HeaderName="FechaUltimoPagoSeguro" DataFieldName="FechaUltimoPagoSeguro"
                                        Display="false" NameControlManger="txtFechaUltPago" ExportToExcel="true" />
                                    <cc1:Column HeaderName="DescripcionSeguro" DataFieldName="DescripcionSeguro" Display="false"
                                        NameControlManger="txtDescripcion" ExportToExcel="true" />
                                    <cc1:Column HeaderName="NroHabilitacionEE" DataFieldName="NroHabilitacionEE" Display="false"
                                        NameControlManger="txtNroHabilitacionEE" ExportToExcel="true" />
                                    <cc1:Column HeaderName="FechaHabilitacionEE" DataFieldName="FechaHabilitacionEE"
                                        Display="false" NameControlManger="txtFechaHabilitacionEE" ExportToExcel="true" />
                                    <cc1:Column HeaderName="FechaVencimientoHabilitacionEE" DataFieldName="FechaVencimientoHabilitacionEE"
                                        Display="false" NameControlManger="txtFechaVencimientoEE" ExportToExcel="true" />
                                    <cc1:Column HeaderName="FechaVencimientoContrato" DataFieldName="FechaVencimientoContrato"
                                        Display="false" ExportToExcel="false" />
                                </Columns>
                            </cc1:ClientControlGrid>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="upEdicion" runat="server" UpdateMode="Conditional">
        <contenttemplate> 
                    <cc1:ServerControlWindow ID="ServerControlWindow1" runat="server" BackColor="WhiteSmoke"
                        WindowColor="Rojo">
                        <ContentControls>
                       <cc1:ServerPanel runat="server" ID="pnlEdicion">
        <BodyContent>
                                <div id="divPrincipal"  style="height: 860px; width: 900px">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="padding-top: 0px;
                                    text-align: left; height: 550px">
                                    <tr >
                                        <td colspan="4" style="background-color: #CACACA; padding-left: 10px;height:30px">
                                            <asp:Label ID="lblTituloCaractesticas" runat="server" SkinID="lblConosud" Text="CARACTERISTICAS DEL VEHICULO"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="padding-top:5px">
                                        <td>
                                            <asp:Label ID="Label2" runat="server" SkinID="lblConosud" Text="Patente:"></asp:Label>
                                        </td>
                                        <td align="left" style="padding-left: 5px; padding-right: 5px">
                                            <asp:TextBox Width="250px" ID="txtPatente" runat="server" Style="text-transform: uppercase"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" SkinID="lblConosud" Text="Número Interno:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <asp:TextBox Width="250px" ID="txtNroInterno" ReadOnly="false" runat="server" Style="text-transform: uppercase"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label14" runat="server" SkinID="lblConosud" Text="Tipo Unidad:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <telerik:RadComboBox ID="cboTipoUnidad" runat="server" Skin="Sunset" Width="90%"
                                                EmptyMessage="Seleccione Tipo Unidad" ZIndex="922000000" AllowCustomText="true">
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" SkinID="lblConosud" Text="Nro Chasis:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <asp:TextBox Width="100px" ID="txtNroChasis" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" SkinID="lblConosud" Text="Capacidad Carga:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <asp:TextBox Width="250px" ID="txtCapacidad" runat="server"></asp:TextBox>
                                        </td>
                                        <td style="width: 180px">
                                            <asp:Label ID="Label5" runat="server" SkinID="lblConosud" Text="Fecha Fabricación:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <telerik:RadDatePicker ID="txtFechaFabricación" MinDate="1950/1/1" runat="server"
                                                ZIndex="922000000" >
                                                <DateInput ID="DateInput1" DateFormat="yyyy" runat="server" DisplayDateFormat="yyyy"></DateInput> 
                                            </telerik:RadDatePicker>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label7" runat="server" SkinID="lblConosud" Text="Marca:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <asp:TextBox Width="250px" ID="txtMarca" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label8" runat="server" SkinID="lblConosud" Text="Modelo:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <asp:TextBox Width="80px" ID="txtModelo" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="padding-bottom:5px">
                                        <td>
                                            <asp:Label ID="Label9" runat="server" SkinID="lblConosud" Text="Nro Motor:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px" colspan="3">
                                            <asp:TextBox Width="150px" ID="txtNroMotor" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                        <td colspan="4" style="background-color: #CACACA; padding-left: 10px;height:30px">
                                            <asp:Label ID="Label24" runat="server" SkinID="lblConosud" Text="HABILITACION CENT"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="padding-top:5px">
                                        <td colspan="4" align="left" style="padding-left: 5px; padding-right: 5px">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" style="text-align: left;">
                                                <tr>
                                                    <td style="width: 180px">
                                                        <asp:Label ID="Label19" runat="server" SkinID="lblConosud" Text="Número de Habilitación:"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox Width="250px" ID="txtNroHabilitacion" runat="server" Style="text-transform: uppercase"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 180px">
                                                        <asp:Label ID="Label27" runat="server" SkinID="lblConosud" Text="Ingreso por Puesto Nº:"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox Width="250px" ID="txtPuestoIngreso" runat="server" Style="text-transform: uppercase"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="left" style="padding-left: 5px; padding-right: 5px">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" style="text-align: left;">
                                                <tr>
                                                    <td style="width: 180px">
                                                        <asp:Label ID="Label20" runat="server" SkinID="lblConosud" Text="Otorgado el día:"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <telerik:RadDatePicker ID="txtFechaOtorgado" MinDate="1950/1/1" runat="server" ZIndex="922000000">
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="padding-bottom:5px">
                                        <td colspan="4" align="left" style="padding-left: 5px; padding-right: 5px">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" style="text-align: left;">
                                                <tr>
                                                    <td style="width: 180px">
                                                        <asp:Label ID="Label21" runat="server" SkinID="lblConosud" Text="Vencimiento:"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <telerik:RadDatePicker ID="txtFechaVencimiento" MinDate="1950/1/1" runat="server"
                                                            ZIndex="922000000">
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                      <tr>
                                        <td colspan="4" style="background-color: #CACACA; padding-left: 10px;height:30px">
                                            <asp:Label ID="Label17" runat="server" SkinID="lblConosud" Text="HABILITACION EQUIPOS ELEVACION"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="padding-top:5px">
                                        <td colspan="4" align="left" style="padding-left: 5px; padding-right: 5px">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" style="text-align: left;">
                                                <tr>
                                                    <td style="width: 180px">
                                                        <asp:Label ID="Label22" runat="server" SkinID="lblConosud" Text="Número de Habilitación:"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox Width="250px" ID="txtNroHabilitacionEE" runat="server" Style="text-transform: uppercase"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="left" style="padding-left: 5px; padding-right: 5px">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" style="text-align: left;">
                                                <tr>
                                                    <td style="width: 180px">
                                                        <asp:Label ID="Label23" runat="server" SkinID="lblConosud" Text="Otorgado el día:"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <telerik:RadDatePicker ID="txtFechaHabilitacionEE" MinDate="1950/1/1" runat="server" ZIndex="922000000">
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="padding-bottom:5px">
                                        <td colspan="4" align="left" style="padding-left: 5px; padding-right: 5px">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" style="text-align: left;">
                                                <tr>
                                                    <td style="width: 180px">
                                                        <asp:Label ID="Label26" runat="server" SkinID="lblConosud" Text="Vencimiento:"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <telerik:RadDatePicker ID="txtFechaVencimientoEE" MinDate="1950/1/1" runat="server"
                                                            ZIndex="922000000">
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="background-color: #CACACA; padding-left: 10px;height:30px">
                                            <asp:Label ID="Label25" runat="server" SkinID="lblConosud" Text="OTROS DATOS"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="padding-top:5px">
                                        <td>
                                            <asp:Label ID="lblPropio" runat="server" SkinID="lblConosud" Text="Vehículo Propio?:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <asp:CheckBox Text="" runat="server" ID="chkPropio" Checked="true" />
                                        </td>
                                        <td style="width: 180px">
                                            <asp:Label ID="Label11" runat="server" SkinID="lblConosud" Text="Titular:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <asp:TextBox Width="250px" ID="txtTitular" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label12" runat="server" SkinID="lblConosud" Text="Fecha alta empresa:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <telerik:RadDatePicker ID="txtFechaAltaEmpresa" MinDate="1950/1/1" runat="server"
                                                ZIndex="922000000">
                                            </telerik:RadDatePicker>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label13" runat="server" SkinID="lblConosud" Text="Fecha baja empresa:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <telerik:RadDatePicker ID="txtFechaBajaEmpresa" MinDate="1950/1/1" runat="server"
                                                ZIndex="922000000">
                                            </telerik:RadDatePicker>
                                        </td>
                                    </tr>
                                    <tr style="padding-bottom:5px">
                                        <td>
                                            <asp:Label ID="Label15" runat="server" SkinID="lblConosud" Text="Equipamiento Agregado:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <asp:TextBox Width="90%" ID="txtEquipoAgregado" runat="server" Rows="4" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label18" runat="server" SkinID="lblConosud" Text="Empresa y Contrato:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px;">
                                            <div style="padding-bottom: 3px">
                                                <telerik:RadComboBox ID="cboEmpresa" runat="server" Skin="Sunset" Width="100%" EmptyMessage="Seleccione Empresa"
                                                    ZIndex="922000000" AllowCustomText="true" EnableLoadOnDemand="true" OnItemsRequested="cboEmpresa_ItemsRequested"
                                                    OnClientSelectedIndexChanged="CargarContratosySeguros" OnClientItemsRequested="ItemsLoaded">
                                                </telerik:RadComboBox>
                                            </div>
                                            <telerik:RadComboBox ID="cboContrato" runat="server" Skin="Sunset" Width="100%" EmptyMessage="Seleccione Contrato"
                                                ZIndex="922000000" AllowCustomText="true" OnItemsRequested="cboContratos_ItemsRequested" 
                                                OnClientItemsRequested="ItemsLoaded" OnClientDropDownOpened="SetearDatosEmpresa">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr >
                                        <td colspan="4" style="background-color: #CACACA; padding-left: 10px;height:30px">
                                            <asp:Label ID="lblTiuloSeguro" runat="server" SkinID="lblConosud" Text="SEGURO DEL VEHICULO"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="padding-top:5px">
                                        <td colspan="4" align="left" style="padding-left: 5px; padding-right: 5px">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" style="text-align: left;">
                                                <tr>
                                                    <td style="width: 120px">
                                                        <asp:Label ID="Label16" runat="server" SkinID="lblConosud" Text="Compañia:"></asp:Label>
                                                    </td>
                                                    <td align="left" style="padding-left: 5px; padding-right: 5px">
                                                        <telerik:RadComboBox ID="cboCompañia" runat="server" Skin="Sunset" Width="90%" EmptyMessage="Seleccione Compañia Seguro"
                                                            DataValueField="IdClasificacion" DataTextField="Descripcion" ZIndex="922000000"
                                                            AllowCustomText="true">
                                                        </telerik:RadComboBox>
                                                    </td>
                                                    <td style="width: 120px">
                                                        <asp:Label ID="Label29" runat="server" SkinID="lblConosud" Text="Nro Poliza:"></asp:Label>
                                                    </td>
                                                    <td style="padding-left: 5px; padding-right: 5px">
                                                        <asp:TextBox Width="100px" ID="txtNroPoliza" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label33" runat="server" SkinID="lblConosud" Text="Fecha Inicial:"></asp:Label>
                                                    </td>
                                                    <td style="padding-left: 5px; padding-right: 5px">
                                                        <telerik:RadDatePicker ID="txtFechaInicio" MinDate="1950/1/1" runat="server" ZIndex="922000000">
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label34" runat="server" SkinID="lblConosud" Text="Fecha Venc.:"></asp:Label>
                                                    </td>
                                                    <td style="padding-left: 5px; padding-right: 5px">
                                                        <telerik:RadDatePicker ID="txtFechaVenicimiento" MinDate="1950/1/1" runat="server"
                                                            ZIndex="922000000">
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label35" runat="server" SkinID="lblConosud" Text="Vigencia Ult. Pago:"></asp:Label>
                                                    </td>
                                                    <td style="padding-left: 5px; padding-right: 5px">
                                                        <telerik:RadDatePicker ID="txtFechaUltPago" MinDate="1950/1/1" runat="server" ZIndex="922000000">
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label36" runat="server" SkinID="lblConosud" Text="Observación:"></asp:Label>
                                                    </td>
                                                    <td style="padding-left: 5px; padding-right: 5px">
                                                        <asp:TextBox Width="90%" ID="txtDescripcion" runat="server" Rows="4" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                     <tr runat="server" id="RowHeaderAuditoria">
                                        <td colspan="4" style="background-color: #CACACA; padding-left: 10px;height:30px">
                                            <asp:Label ID="Label1" runat="server" SkinID="lblConosud" Text="DATOS AUDITORIA"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="RowBodyAuditoria" style="padding-top:5px">
                                        <td colspan="4" align="left" style="padding-left: 5px; padding-right: 5px">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" style="text-align: left;">
                                                <tr>
                                                    <td align="center" width="30%">
                                                        <asp:CheckBox ID="chkHabilitarCred" SkinID="chkConosud" runat="server" Text="Habilitar Credencial" />
                                                    </td>
                                                    <td align="right" style="padding-left: 5px">
                                                        <asp:Label ID="Label30" runat="server" SkinID="lblConosud" Text="Venc. Cred:"></asp:Label>
                                                    </td>
                                                    <td align="left" width="50%">
                                                        <telerik:RadDatePicker ID="txtFechaVenCredencial" MinDate="1950/1/1" runat="server"
                                                            ZIndex="922000000">
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="center" style="padding-top: 5px">
                                            <asp:Button ID="btnAplicar" OnClientClick="AplicarCambios();return false;" runat="server"
                                                Text="Grabar" SkinID="btnConosudBasic" />
                                        </td>
                                    </tr>
                                </table>
                                </div>
                           </BodyContent>
    </cc1:ServerPanel>
                        </ContentControls>
                    </cc1:ServerControlWindow>
                 
                        </contenttemplate>
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
