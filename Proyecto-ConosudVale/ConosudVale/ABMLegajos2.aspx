<%@ Page Title="" Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="ABMLegajos2.aspx.cs" Inherits="ABMLegajos2" %>

<%@ Register Assembly="ControlsAjaxNotti" Namespace="ControlsAjaxNotti" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .RadWindow.RadWindow_Sunset.rwNormalWindow.rwTransparentWindow
        {
            z-index: 999900000 !important;
        }
        
        tbody.on
        {
            display: table-row-group;
        }
        tbody.off
        {
            display: none;
        }
    </style>
    <script type="text/javascript">

        var EsEdicion = false;
        //var PrimeraVez = true;

        function ControlCheckRacs(chkComp, chkEstudios) {
            if (chkComp.checked) {
                document.getElementById(chkEstudios).checked = true;
                document.getElementById(chkEstudios).disabled = true;
            }
            else {
                document.getElementById(chkEstudios).checked = false;
                document.getElementById(chkEstudios).disabled = false;
            }

        }

        function gvRAC_RowDataBound(sender, args) {
            args.get_item().get_cell("IdCursoLegajoColumn").all[0].innerText = args.get_dataItem()["IdCursoLegajo"];
        }

        function LimpiarGrillaCursos() {
            var tableView = $find("<%= gvRAC.ClientID %>").get_masterTableView();
            tableView.set_dataSource(new Array());
            tableView.dataBind();
        }

        function pageLoad(sender, eventArgs) {
            LimpiarGrillaCursos();
        }

        //////Carga cursos
        function CargarGrillaCursos(IdLegajo) {
            PageMethods.ObtenerCursos(IdLegajo, FillGridLegajos, ErrorFillGridLegajos);
        }

        function FillGridLegajos(result) {

            var tableView = $find("<%= gvRAC.ClientID %>").get_masterTableView();
            tableView.set_dataSource(result);
            tableView.dataBind();

        }

        function ErrorFillGridLegajos() {


        }

        function ResponseEnd(sender, args) {

            if ($get("<%=txtNroDocEdit.ClientID %>").getAttribute("NroExistente") != null) {

                if ($get("<%=txtNroDocEdit.ClientID %>").getAttribute("NroExistente") == "True") {
                    radalert("El número de documento que intenta asignar ya existe.", 300, 100, 'Legajos');
                }
                else {
                    $find("<%=ServerControlWindow1.ClientID %>").CloseWindows();
                    $get("<%=txtNroDocEdit.ClientID %>").removeAttribute("NroExistente");
                }
            }
        }

        function GuardarCambios() {

            if (Page_ClientValidate()) {

                var cboContratoAsigando = $find("<%= cboContratoLegajo.ClientID%>");

                if (cboContratoAsigando.get_value() != "" && cboContratoAsigando.get_value() != "-1") {
                    if ($find("<%= txtFechaVenCredencial.ClientID%>") != null) {

                        var datePickerCredencial = $find("<%= txtFechaVenCredencial.ClientID%>")
                        /// Esto indica que la fecha ingresada no es válida.
                        if (datePickerCredencial.get_selectedDate() == null && datePickerCredencial.get_textBox().value != "") {
                            radalert("La fecha ingresada para el vencimiento de la credencial no es válida ya que supera la vigencia del contrato. Ingrese una fecha válida porfavor", 400, 100, 'Legajos');
                            return false;
                        }
                        else {
                            var fecha = datePickerCredencial.get_selectedDate();
                            var obsBloqueo = $get("<%= txtObsBloqueo.ClientID%>").value;

                            if (fecha == null && obsBloqueo.trim() == "") {
                                $get("<%= txtObsBloqueo.ClientID%>").scrollIntoView();
                                radalert("El legajo ha sido bloqueado, debe ingresar la obs. del bloqueo o ingresar una fecha de vencimiento de la credencial.", 300, 100, 'Legajos');
                                return false;
                            }
                        }
                        
                        var fecha = datePickerCredencial.get_selectedDate();
                        var obsAuditoria = $get("<%= txtObservacion.ClientID%>").value;
                        if (fecha != null && obsAuditoria.trim() != "" && cboContratoAsigando.get_value() != "" && cboContratoAsigando.get_value() != "-1") 
                        {
                            radalert("El legajo esta asignado a un contrato y posee un vencimiento en la credencial, por lo que el mismo no puede tener una observación de Auditoría.", 300, 100, 'Legajos');
                            return false;
                        }

                    }
                }
                else {
                    $find("<%= txtFechaVenCredencial.ClientID%>").set_enabled(true);
                    $find("<%= txtFechaVenCredencial.ClientID%>").set_selectedDate("");
                }


                var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");
                var Item = $find("<%= RadGrid1.ClientID%>").get_masterTableView().get_selectedItems();

                $find("<%= ServerControlWindow1.ClientID%>").ShowWaiting(true, "Guardando Cambios...");

                if (Item.length > 0) {

                    ajaxManager.ajaxRequest("Update");
                }
                else {
                    ajaxManager.ajaxRequest("Insert");
                }
            }
            return false;
        }

        function InitInsert() {

            LimpiarGrillaCursos();

            ///Habilito los controles
            $find("<%= cboEmpresaLegajo.ClientID%>").set_enabled(true);
            $find("<%= cboContratoLegajo.ClientID%>").set_enabled(true);
            $find("<%= cboPeriodoLegajo.ClientID%>").set_enabled(true);

            //Habilito los eventos para la insercion
            $find("<%= cboEmpresaLegajo.ClientID%>").remove_selectedIndexChanging(CargarContratos);
            $find("<%= cboContratoLegajo.ClientID%>").remove_selectedIndexChanged(CargarPeriodos);
            $find("<%= cboEmpresaLegajo.ClientID%>").add_selectedIndexChanging(CargarContratos);
            $find("<%= cboContratoLegajo.ClientID%>").add_selectedIndexChanged(CargarPeriodos);


            $find("<%=pnlEdicion.ClientID %>").ClearElements();
            $find("<%= RadGrid1.ClientID%>").get_masterTableView().clearSelectedItems();
            $find("<%=ServerControlWindow1.ClientID %>").set_CollectionDiv('divPrincipal');
            $find("<%=ServerControlWindow1.ClientID %>").ShowWindows('divPrincipal', "Nuevo Legajo", $get("<%=txtApellido.ClientID %>"));
            Page_ClientValidate();
        }

        function EditLegajo() {

            $find("<%=pnlEdicion.ClientID %>").ClearElements();

            var grid = $find("<%= RadGrid1.ClientID%>");
            var MasterTable = grid.get_masterTableView();
            var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");



            var Item = MasterTable.get_selectedItems();
            if (Item.length > 0) {

                CargarGrillaCursos(MasterTable.getCellByColumnUniqueName(Item[0], "IdLegajosColumn").outerText);
                var cellApellido = MasterTable.getCellByColumnUniqueName(Item[0], "ApellidoColumn");
                var cellNombre = MasterTable.getCellByColumnUniqueName(Item[0], "NombreColumn");
                var cellNroDoc = MasterTable.getCellByColumnUniqueName(Item[0], "NroDocColumn");
                var cellFechaNacimiento = MasterTable.getCellByColumnUniqueName(Item[0], "FechaNacimientoColumn");
                var cellCUIL = MasterTable.getCellByColumnUniqueName(Item[0], "CUILColumn");
                var cellDireccion = MasterTable.getCellByColumnUniqueName(Item[0], "DireccionColumn");
                var cellCodigoPostal = MasterTable.getCellByColumnUniqueName(Item[0], "CodigoPostalColumn");
                var cellTelefonoFijo = MasterTable.getCellByColumnUniqueName(Item[0], "TelefonoFijoColumn");
                var cellCorreoElectronico = MasterTable.getCellByColumnUniqueName(Item[0], "CorreoElectronicoColumn");
                var cellEstadoCivil = MasterTable.getCellByColumnUniqueName(Item[0], "EstadoCivilColumn");
                var cellNacionalidad = MasterTable.getCellByColumnUniqueName(Item[0], "NacionalidadColumn");
                var cellProvincia = MasterTable.getCellByColumnUniqueName(Item[0], "ProvinciaColumn");
                var cellEmpresaLegajo = MasterTable.getCellByColumnUniqueName(Item[0], "EmpresaLegajoColumn");
                var cellTipoDocumento = MasterTable.getCellByColumnUniqueName(Item[0], "TipoDocumentoColumn");
                var cellConvenio = MasterTable.getCellByColumnUniqueName(Item[0], "ConvenioColumn");
                var cellFechaIngreos = MasterTable.getCellByColumnUniqueName(Item[0], "FechaIngreosColumn");
                var cellEstadoVerificacion = MasterTable.getCellByColumnUniqueName(Item[0], "EstadoVerificacionColumn");
                var cellFechaVerificacion = MasterTable.getCellByColumnUniqueName(Item[0], "FechaVerificacionColumn");
                var cellObservacion = MasterTable.getCellByColumnUniqueName(Item[0], "ObservacionColumn");
                var cellFechaUltimaModificacion = MasterTable.getCellByColumnUniqueName(Item[0], "FechaUltimaModificacionColumn");
                var cellGrupoSangre = MasterTable.getCellByColumnUniqueName(Item[0], "GrupoSangreColumn");
                var cellAutorizadoCond = MasterTable.getCellByColumnUniqueName(Item[0], "AutorizadoCondColumn");


                var cellEstudiosBasicos = MasterTable.getCellByColumnUniqueName(Item[0], "EstudiosBasicosColumn");
                var cellComplementarioRacs = MasterTable.getCellByColumnUniqueName(Item[0], "ComplementarioRacsColumn");
                var cellAdicionalQuimicos = MasterTable.getCellByColumnUniqueName(Item[0], "AdicionalQuimicosColumn");


                var cellFechaUltimoExamen = MasterTable.getCellByColumnUniqueName(Item[0], "FechaUltimoExamenColumn");
                var cellRutaFoto = MasterTable.getCellByColumnUniqueName(Item[0], "RutaFotoColumn");
                var cellCredVencimiento = MasterTable.getCellByColumnUniqueName(Item[0], "CredVencimientoColumn");
                var cellObservacionBloqueo = MasterTable.getCellByColumnUniqueName(Item[0], "ObservacionBloqueoColumn");


                var cellFuncion = MasterTable.getCellByColumnUniqueName(Item[0], "FuncionColumn");

                var cellContrato = MasterTable.getCellByColumnUniqueName(Item[0], "ContratoColumn");
                var cellPeriodo = MasterTable.getCellByColumnUniqueName(Item[0], "PeriodoColumn");
                var cellVencimientoContrato = MasterTable.getCellByColumnUniqueName(Item[0], "FechaVencimientoContratoColumn");
                var cellCategoriaContrato = MasterTable.getCellByColumnUniqueName(Item[0], "CategoriaContratoColumn");




                ////////////////// Controles de Seguro  ////////////////////////////////
                var cellNroPolizaColumn = MasterTable.getCellByColumnUniqueName(Item[0], "NroPolizaColumn").outerText;
                var cellIdCompañiaColumn = MasterTable.getCellByColumnUniqueName(Item[0], "IdCompañiaColumn").outerText;
                var cellFechaInicialColumn = MasterTable.getCellByColumnUniqueName(Item[0], "FechaInicialColumn").outerText;
                var cellFechaUltimoPagoColumn = MasterTable.getCellByColumnUniqueName(Item[0], "FechaUltimoPagoColumn").outerText;
                var cellFechaVencimientoColumn = MasterTable.getCellByColumnUniqueName(Item[0], "FechaVencimientoColumn").outerText;
                var cellDescripcionColumn = MasterTable.getCellByColumnUniqueName(Item[0], "DescripcionColumn").outerText;


                document.getElementById("<%= txtNroPoliza.ClientID%>").value = cellNroPolizaColumn.trim();
                document.getElementById("<%= txtDescripcion.ClientID%>").value = cellDescripcionColumn.trim();

                // Combo Compaia de Seguro
                if (cellIdCompañiaColumn.trim() != "") {
                    var itemTipoDoc = $find("<%= cboCompañia.ClientID%>").findItemByValue(cellIdCompañiaColumn);
                    $find("<%= cboCompañia.ClientID%>").set_selectedItem(itemTipoDoc);
                    itemTipoDoc.select();
                }
                else {
                    $find("<%= cboCompañia.ClientID%>").clearSelection();
                }


                if (cellFechaInicialColumn.trim() != "") {
                    dia = parseInt(lTrim0(cellFechaInicialColumn.substr(0, 2)));
                    mes = parseInt(lTrim0(cellFechaInicialColumn.substr(3, 2))) - 1;
                    año = parseInt(lTrim0(cellFechaInicialColumn.substr(6, 4)));
                    Fecha = new Date(año, mes, dia);
                    if ($find("<%= txtFechaInicio.ClientID%>") != null) {
                        $find("<%= txtFechaInicio.ClientID%>").set_selectedDate(Fecha);
                    }
                }

                if (cellFechaVencimientoColumn.trim() != "") {
                    dia = parseInt(lTrim0(cellFechaVencimientoColumn.substr(0, 2)));
                    mes = parseInt(lTrim0(cellFechaVencimientoColumn.substr(3, 2))) - 1;
                    año = parseInt(lTrim0(cellFechaVencimientoColumn.substr(6, 4)));
                    Fecha = new Date(año, mes, dia);
                    if ($find("<%= txtFechaVenicimiento.ClientID%>") != null) {
                        $find("<%= txtFechaVenicimiento.ClientID%>").set_selectedDate(Fecha);
                    }
                }

                if (cellFechaUltimoPagoColumn.trim() != "") {
                    dia = parseInt(lTrim0(cellFechaUltimoPagoColumn.substr(0, 2)));
                    mes = parseInt(lTrim0(cellFechaUltimoPagoColumn.substr(3, 2))) - 1;
                    año = parseInt(lTrim0(cellFechaUltimoPagoColumn.substr(6, 4)));
                    Fecha = new Date(año, mes, dia);
                    if ($find("<%= txtFechaUltPago.ClientID%>") != null) {
                        $find("<%= txtFechaUltPago.ClientID%>").set_selectedDate(Fecha);
                    }
                }

                ////////////////////////////////////////////////////////////////////////////////
                /// Controles Tipo Label
                if ($get("<%= lblTipo.ClientID%>") != null)
                    $("#<%= lblTipo.ClientID%>").text(cellCategoriaContrato.innerText.trim());


                ////////////////////////////////////////////////////////////////////////////////
                /// Controles Tipo TextBox
                document.getElementById("<%= txtApellido.ClientID%>").value = cellApellido.innerText.trim();
                document.getElementById("<%= txtNombre.ClientID%>").value = cellNombre.innerText.trim();
                document.getElementById("<%= txtNroDocEdit.ClientID%>").value = cellNroDoc.innerText.trim();
                document.getElementById("<%= txtDireccion.ClientID%>").value = cellDireccion.innerText.trim();
                document.getElementById("<%= txtCodigoPostal.ClientID%>").value = cellCodigoPostal.innerText.trim();
                document.getElementById("<%= txtTelFijo.ClientID%>").value = cellTelefonoFijo.innerText.trim();
                document.getElementById("<%= txtEmail.ClientID%>").value = cellCorreoElectronico.innerText.trim();
                document.getElementById("<%= txtGrupoSangre.ClientID%>").value = cellGrupoSangre.innerText.trim();
                document.getElementById("<%= txtObservacion.ClientID%>").value = cellObservacion.innerText.trim();

                if (document.getElementById("<%= txtObsBloqueo.ClientID%>") != null)
                    document.getElementById("<%= txtObsBloqueo.ClientID%>").value = cellObservacionBloqueo.innerText.trim();


                document.getElementById("<%= txtFuncion.ClientID%>").value = cellFuncion.innerText.trim();

                var ultmod = document.getElementById("<%= txtFechaUltimaModificacion.ClientID%>");
                if (ultmod != null) { ultmod.value = cellFechaUltimaModificacion.innerText; }

                if (cellRutaFoto.innerText != "")
                    document.getElementById("<%= imgLegajo.ClientID%>").src = "ImagenesLegajos/" + cellRutaFoto.innerText;
                else
                    document.getElementById("<%= imgLegajo.ClientID%>").src = "";


                /// Controles Tipo CheckBox
                var autcond = false, habcred = false;
                var estBas = false, comRacs = false, adiQui = false;
                if (cellAutorizadoCond.outerText == 'True') { autcond = true };

                if (cellEstudiosBasicos.outerText == 'True') { estBas = true };
                if (cellComplementarioRacs.outerText == 'True') {
                    comRacs = true
                    document.getElementById("<%= chkEstudosBasicos.ClientID %>").disabled = true;
                }
                else {
                    document.getElementById("<%= chkEstudosBasicos.ClientID %>").disabled = false;
                }
                if (cellAdicionalQuimicos.outerText == 'True') { adiQui = true };

                if (document.getElementById("<%= chkAutorizadoConducir.ClientID %>") != null)
                    document.getElementById("<%= chkAutorizadoConducir.ClientID %>").checked = autcond;

                document.getElementById("<%= chkEstudosBasicos.ClientID %>").checked = estBas;
                document.getElementById("<%= chkCompRacs.ClientID %>").checked = comRacs;
                document.getElementById("<%= chkAdicionalQuimicos.ClientID %>").checked = adiQui;



                /// Controles Tipo Telerik
                $find("<%= txtCUIL.ClientID%>").set_value(cellCUIL.innerText.trim());

                /// Controles Tipo Fecha
                if (cellFechaVerificacion.outerText.trim() != "") {
                    dia = parseInt(lTrim0(cellFechaVerificacion.outerText.substr(0, 2)));
                    mes = parseInt(lTrim0(cellFechaVerificacion.outerText.substr(3, 2))) - 1;
                    año = parseInt(lTrim0(cellFechaVerificacion.outerText.substr(6, 4)));
                    Fecha = new Date(año, mes, dia);
                    if ($find("<%= txtFechaVerificacion.ClientID%>") != null) {
                        $find("<%= txtFechaVerificacion.ClientID%>").set_selectedDate(Fecha);
                    }
                }



                if (cellCredVencimiento.outerText.trim() != "") {
                    dia = parseInt(lTrim0(cellCredVencimiento.outerText.substr(0, 2)));
                    mes = parseInt(lTrim0(cellCredVencimiento.outerText.substr(3, 2))) - 1;
                    año = parseInt(lTrim0(cellCredVencimiento.outerText.substr(6, 4)));
                    Fecha = new Date(año, mes, dia);

                    dia = parseInt(lTrim0(cellVencimientoContrato.outerText.substr(0, 2)));
                    mes = parseInt(lTrim0(cellVencimientoContrato.outerText.substr(3, 2))) - 1;
                    año = parseInt(lTrim0(cellVencimientoContrato.outerText.substr(6, 4)));
                    FechaContrato = new Date(año, mes, dia);


                    if ($find("<%= txtFechaVenCredencial.ClientID%>") != null) {
                        $find("<%= txtFechaVenCredencial.ClientID%>").set_selectedDate();

                        if (FechaContrato != "NaN")
                            $find("<%= txtFechaVenCredencial.ClientID%>").set_maxDate(FechaContrato);

                        if (Fecha > FechaContrato)
                            $find("<%= txtFechaVenCredencial.ClientID%>").set_selectedDate(FechaContrato);
                        else
                            $find("<%= txtFechaVenCredencial.ClientID%>").set_selectedDate(Fecha);
                    }
                }
                else {

                    dia = parseInt(lTrim0(cellVencimientoContrato.outerText.substr(0, 2)));
                    mes = parseInt(lTrim0(cellVencimientoContrato.outerText.substr(3, 2))) - 1;
                    año = parseInt(lTrim0(cellVencimientoContrato.outerText.substr(6, 4)));
                    FechaContrato = new Date(año, mes, dia);

                    if (FechaContrato != "NaN" && $find("<%= txtFechaVenCredencial.ClientID%>")!= null)
                        $find("<%= txtFechaVenCredencial.ClientID%>").set_maxDate(FechaContrato);
                }


                if (cellFechaUltimoExamen.outerText.trim() != "") {
                    dia = parseInt(lTrim0(cellFechaUltimoExamen.outerText.substr(0, 2)));
                    mes = parseInt(lTrim0(cellFechaUltimoExamen.outerText.substr(3, 2))) - 1;
                    año = parseInt(lTrim0(cellFechaUltimoExamen.outerText.substr(6, 4)));
                    Fecha = new Date(año, mes, dia);
                    $find("<%= txtFechaUltExa.ClientID%>").set_selectedDate(Fecha);
                }

                if (cellFechaNacimiento.outerText.trim() != "") {
                    dia = parseInt(lTrim0(cellFechaNacimiento.outerText.substr(0, 2)));
                    mes = parseInt(lTrim0(cellFechaNacimiento.outerText.substr(3, 2))) - 1;
                    año = parseInt(lTrim0(cellFechaNacimiento.outerText.substr(6, 4)));
                    Fecha = new Date(año, mes, dia);
                    $find("<%= txtFechaNacimiento.ClientID%>").set_selectedDate(Fecha);
                }

                if (cellFechaIngreos.outerText.trim() != "") {
                    dia = parseInt(lTrim0(cellFechaIngreos.outerText.substr(0, 2)));
                    mes = parseInt(lTrim0(cellFechaIngreos.outerText.substr(3, 2))) - 1;
                    año = parseInt(lTrim0(cellFechaIngreos.outerText.substr(6, 4)));
                    Fecha = new Date(año, mes, dia);
                    $find("<%= txtFechaIngreso.ClientID%>").set_selectedDate(Fecha);
                }




                /// Controles Tipo Combos
                var cboEstVer = $find("<%= cboEstadoVerificacion.ClientID%>");
                if (cboEstVer != null) {
                    if (cellEstadoVerificacion.outerText.trim() != "") {
                        var itemEstadoVerificacion = cboEstVer.findItemByText(cellEstadoVerificacion.outerText.trim());
                        cboEstVer.set_selectedItem(itemEstadoVerificacion);
                        itemEstadoVerificacion.select();
                    }
                    else {

                        cboEstVer.clearSelection();
                    }
                }




                //Deshabilito los eventos porque en la edicion no son necesarios
                $find("<%= cboEmpresaLegajo.ClientID%>").remove_selectedIndexChanging(CargarContratos);



                if ($find("<%= cboContratoLegajo.ClientID%>") != null)
                    $find("<%= cboContratoLegajo.ClientID%>").remove_selectedIndexChanged(CargarPeriodos);



                if (cellEmpresaLegajo.outerText.trim() != "") {
                    EsEdicion = true;
                    var cboEmpresa = $find("<%= cboEmpresaLegajo.ClientID%>");
                    var itemEmpresaLeg = cboEmpresa.findItemByValue(cellEmpresaLegajo.outerText.trim());
                    cboEmpresa.set_selectedItem(itemEmpresaLeg);
                    cboEmpresa.trackChanges();
                    itemEmpresaLeg.select();
                    cboEmpresa.set_text(itemEmpresaLeg.get_text());
                    cboEmpresa.set_value(itemEmpresaLeg.get_value());
                    cboEmpresa.commitChanges();

                    if ($find("<%= cboContratoLegajo.ClientID%>") != null) {
                        var contratosCombo = $find("<%= cboContratoLegajo.ClientID %>");
                        contratosCombo.requestItems(itemEmpresaLeg.get_value(), false);
                    }


                }
                else {
                    $find("<%= cboEmpresaLegajo.ClientID%>").clearSelection();
                }



                if ($find("<%= cboContratoLegajo.ClientID%>") != null) {
                    if (cellContrato.outerText.trim() != "") {
                        $find("<%= cboContratoLegajo.ClientID%>").set_text(cellContrato.outerText.trim());
                        $find("<%= cboContratoLegajo.ClientID%>").set_value(cellContrato.outerText.trim());
                        $find("<%= cboContratoLegajo.ClientID%>").set_enabled(false);
                    }
                    else {
                        $find("<%= cboContratoLegajo.ClientID%>").clearSelection();
                        $find("<%= cboContratoLegajo.ClientID%>").set_text("Sin Afectación");
                        $find("<%= cboContratoLegajo.ClientID%>").set_enabled(true);
                    }
                }

                if ($find("<%= cboPeriodoLegajo.ClientID%>") != null) {
                    if (cellPeriodo.outerText.trim() != "") {
                        $find("<%= cboPeriodoLegajo.ClientID%>").set_text(cellPeriodo.outerText.trim());
                        $find("<%= cboPeriodoLegajo.ClientID%>").set_enabled(false);
                    }
                    else {
                        $find("<%= cboPeriodoLegajo.ClientID%>").clearSelection();
                        $find("<%= cboPeriodoLegajo.ClientID%>").set_text("Sin Afectación");
                        $find("<%= cboPeriodoLegajo.ClientID%>").set_enabled(false);

                        var txtFechaCredencial = $find("<%=txtFechaVenCredencial.ClientID %>");
                        txtFechaCredencial.set_selectedDate();
                        txtFechaCredencial.set_enabled(false);
                    }
                }

                ////Valida si es administrador para habilitar los controles de asignacion de contrato
                if (document.getElementById("<%= HiddenEsAdministrador.ClientID%>").value == "False" && document.getElementById("<%= HiddenTipoUsuario.ClientID%>").value == "Cliente") {
                    $find("<%= cboEmpresaLegajo.ClientID%>").set_enabled(false);
                    if ($find("<%= cboPeriodoLegajo.ClientID%>") != null) {
                        $find("<%= cboContratoLegajo.ClientID%>").set_enabled(false);
                        $find("<%= cboPeriodoLegajo.ClientID%>").set_enabled(false);
                    }
                }
                else {
                    if (cellPeriodo.outerText.trim() != "" && cellContrato.outerText.trim() != "") {
                        $find("<%= cboEmpresaLegajo.ClientID%>").set_enabled(false);
                        $find("<%= cboContratoLegajo.ClientID%>").set_enabled(false);
                        $find("<%= cboPeriodoLegajo.ClientID%>").set_enabled(false);
                    }
                    else {
                        $find("<%= cboEmpresaLegajo.ClientID%>").set_enabled(true);
                        $find("<%= cboContratoLegajo.ClientID%>").set_enabled(true);
                        $find("<%= cboPeriodoLegajo.ClientID%>").set_enabled(true);

                        $find("<%= cboEmpresaLegajo.ClientID%>").add_selectedIndexChanging(CargarContratos);
                        $find("<%= cboContratoLegajo.ClientID%>").add_selectedIndexChanged(CargarPeriodos);
                    }
                }



                if (cellTipoDocumento.outerText.trim() != "") {
                    var itemTipoDoc = $find("<%= cboTipoDoc.ClientID%>").findItemByText(cellTipoDocumento.outerText.trim());
                    $find("<%= cboTipoDoc.ClientID%>").set_selectedItem(itemTipoDoc);
                    itemTipoDoc.select();
                }
                else {
                    $find("<%= cboTipoDoc.ClientID%>").clearSelection();
                }

                if (cellEstadoCivil.outerText.trim() != "") {
                    var itemEstadoCivial = $find("<%= cboEstadoCivil.ClientID%>").findItemByText(cellEstadoCivil.outerText.trim());
                    $find("<%= cboEstadoCivil.ClientID%>").set_selectedItem(itemEstadoCivial);
                    itemEstadoCivial.select();
                }
                else {
                    $find("<%= cboEstadoCivil.ClientID%>").clearSelection();
                }

                if (cellNacionalidad.outerText.trim() != "") {
                    var itemNacionalidad = $find("<%= cboNacionalidad.ClientID%>").findItemByText(cellNacionalidad.outerText.trim());
                    $find("<%= cboNacionalidad.ClientID%>").set_selectedItem(itemNacionalidad);
                    itemNacionalidad.select();
                }
                else {
                    $find("<%= cboNacionalidad.ClientID%>").clearSelection();
                }

                if (cellConvenio.outerText.trim() != "") {
                    var itemConvenio = $find("<%= cboConvenio.ClientID%>").findItemByText(cellConvenio.outerText.trim());
                    $find("<%= cboConvenio.ClientID%>").set_selectedItem(itemConvenio);
                    itemConvenio.select();
                }
                else {
                    $find("<%= cboConvenio.ClientID%>").clearSelection();
                }

                if (cellProvincia.outerText.trim() != "") {
                    var itemProvincia = $find("<%= cboProvincia.ClientID%>").findItemByText(cellProvincia.outerText.trim());
                    $find("<%= cboProvincia.ClientID%>").set_selectedItem(itemProvincia);
                    itemProvincia.select();
                }
                else {
                    $find("<%= cboProvincia.ClientID%>").clearSelection();
                }



                ///Seguridad deshabilito el boton guardar en la modificacion si es cliente (btnGuardar)
                if (document.getElementById("<%= HiddenTipoUsuario.ClientID%>").value == "Cliente" || "<%=PoseePermisoSoloLectura%>" == "True") {
                    document.getElementById("<%= btnGuardar.ClientID%>").style.display = "none";
                }
                else {
                    document.getElementById("<%= btnGuardar.ClientID%>").style.display = "inline";
                }

                $find("<%=ServerControlWindow1.ClientID %>").set_CollectionDiv('divPrincipal');
                $find("<%=ServerControlWindow1.ClientID %>").ShowWindows('divPrincipal', "Edición: " + cellApellido.innerText + ", " + cellNombre.innerText);

                if ("<%=EsContratista %>" == "True") {
                    $find("<%=pnlEdicion.ClientID %>").DisabledElement(true);
                }
            }
            else {
                radalert("Debe seleccionar un legajo para ver poder editar sus datos", 250, 100, "Selección Legajo");
            }
        }

        function requestStart1(sender, args) {
            if (args.get_eventTarget().indexOf("ExportExcel") > 0) {
                args.set_enableAjax(false);
            }
        }

        function ShowSueldos() {
            var Item = $find("<%=RadGrid1.ClientID %>").get_masterTableView().get_selectedItems();
            if (Item.length > 0) {
                var oWnd = radopen('ConsultaInformacionSueldos.aspx?IdLegajo=' + Item[0].getDataKeyValue("d.IdLegajos"), 'RadWindow1');
            }
            else {
                radalert("Debe seleccionar un legajo para ver su información de sueldos", 250, 100, "Selección Legajo");
            }
        }

        function ShowCredencial() {
            var grid = $find("<%= RadGrid1.ClientID%>");
            var MasterTable = grid.get_masterTableView();
            var Item = MasterTable.get_selectedItems();

            if (Item.length > 0) {

                var cellHabilitarCredencial = MasterTable.getCellByColumnUniqueName(Item[0], "HabCredColumn");

                if (cellHabilitarCredencial.innerText.trim() == 'NO') {
                    radalert("La impresión de la credencial no se encuentra habilitada para el legajo seleccionado.", 250, 100, "Impresión Credencial");
                    return;
                }

                var oWnd = radopen('ViewerCredenciales.aspx?IdLegajo=' + Item[0].getDataKeyValue("d.IdLegajos"), 'RadWindow2');
            }
            else {
                radalert("Debe seleccionar un legajo para ver su Credencial", 250, 100, "Selección Legajo");
            }
        }


        function ShowCargaFoto() {

            var grid = $find("<%= RadGrid1.ClientID%>");
            var MasterTable = grid.get_masterTableView();

            var Item = MasterTable.get_selectedItems();
            if (Item.length > 0) {

                //CargarGrillaCursos(MasterTable.getCellByColumnUniqueName(Item[0], "IdLegajosColumn").outerText);

                var cellApellido = MasterTable.getCellByColumnUniqueName(Item[0], "ApellidoColumn");
                var cellNombre = MasterTable.getCellByColumnUniqueName(Item[0], "NombreColumn");

                $find("<%=ServerControlWindowFoto.ClientID %>").set_CollectionDiv('divPricipalFoto');
                $find("<%=ServerControlWindowFoto.ClientID %>").ShowWindows('divPricipalFoto', "Edición: " + cellApellido.innerText + ", " + cellNombre.innerText);
            }
            else {
                radalert("Debe seleccionar un legajo para poder carga su foto", 250, 100, "Foto Legajo");
            }
        }

        function ValidarArchivo() {
            var archivo = $find("<%=RadUpload1.ClientID %>").getFileInputs()[0].value;

            if (archivo == "") {
                radalert("Debe seleccionar un archivo para poder completar la acción");
                return false;
            }
        }

        function lTrim0(sStr) {
            while (sStr.charAt(0) == '0')
                sStr = sStr.substr(1, sStr.length - 1);
            return sStr;
        }

        function onOff(validatorId, activar) {
            var validator = document.getElementById(validatorId);
            ValidatorEnable(validator, activar);
        }

        function toggleTbodyGroup(id) {
            if (document.getElementById) {
                var tbod = document.getElementById(id);
                if (tbod && typeof tbod.className == 'string') {
                    if (tbod.className == 'off') {
                        tbod.className = 'on';
                        document.getElementById('img' + id).src = "images/up.png";
                    } else {
                        tbod.className = 'off';
                        document.getElementById('img' + id).src = "images/down.png";
                    }
                }
            }
            return false;
        }


        function OnClientItemsRequesting(sender, eventArgs) {

            var cboEmpresaLegajo = $find("<%= cboEmpresaLegajo.ClientID%>")
            var context = eventArgs.get_context();
            if (cboEmpresaLegajo.get_value() != null) {
                context["IdEmpresa"] = cboEmpresaLegajo.get_value()
            }
            else {
                context["IdEmpresa"] = "0";
            }
        }

        function CargarPeriodos() {

            var contratosCombo = $find("<%= cboContratoLegajo.ClientID %>");
            var PeriodoLegajoCombo = $find("<%= cboPeriodoLegajo.ClientID %>");
            var EmpresaLegajoCombo = $find("<%= cboEmpresaLegajo.ClientID %>");

            if (contratosCombo.get_value() > 0) {
                PeriodoLegajoCombo.set_text("Sin Afectación");
                PeriodoLegajoCombo.set_enabled(true);
                PeriodoLegajoCombo.requestItems(EmpresaLegajoCombo.get_value() + "|" + contratosCombo.get_value(), false);
            }
            else {
                PeriodoLegajoCombo.set_text("Sin Afectación");
                PeriodoLegajoCombo.clearItems();
                PeriodoLegajoCombo.set_enabled(false);
            }
        }

        function CargarContratos(combo, eventarqs) {

            var contratosCombo = $find("<%= cboContratoLegajo.ClientID %>");
            var PeriodoLegajoCombo = $find("<%= cboPeriodoLegajo.ClientID %>");
            var txtFechaCredencial = $find("<%=txtFechaVenCredencial.ClientID %>");

            var item = eventarqs.get_item();
            contratosCombo.set_text("Loading...");
            if (item != null && item.get_index() >= 0) {

                PeriodoLegajoCombo.set_text("Sin Afectación");
                PeriodoLegajoCombo.clearItems();
                PeriodoLegajoCombo.set_enabled(false);


                txtFechaCredencial.set_selectedDate();
                txtFechaCredencial.set_enabled(false);

                $get("<%=lblTipo.ClientID %>").innerText = "";

                contratosCombo.requestItems(item.get_value(), false);
            }
            else {
                contratosCombo.set_text(" ");
                contratosCombo.clearItems();

                PeriodoLegajoCombo.set_text("Sin Afectación");
                PeriodoLegajoCombo.clearItems();
                PeriodoLegajoCombo.set_enabled(false);


            }
        }


        function ItemsLoaded(combo, eventarqs) {
            if (!EsEdicion) {
                if (combo.get_items().get_count() > 0) {
                    combo.set_text(combo.get_items().getItem(0).get_text());
                    //combo.get_items().getItem(0).highlight();
                }
                //combo.showDropDown();
            }
            else {
                EsEdicion = false;
            }
        }

        function SelectedContrato(sender, arg) {

            var txtFechaCredencial = $find("<%=txtFechaVenCredencial.ClientID %>");
            var fechaString = arg.get_item().get_attributes().getAttribute("FechaVencimiento");
            var tipo = arg.get_item().get_attributes().getAttribute("Tipo");

            if (fechaString != undefined) {

                txtFechaCredencial.set_enabled(true);

                dia = parseInt(lTrim0(fechaString.substr(0, 2)));
                mes = parseInt(lTrim0(fechaString.substr(3, 2))) - 1;
                año = parseInt(lTrim0(fechaString.substr(6, 4)));
                FechaContrato = new Date(año, mes, dia);


                if (txtFechaCredencial != null) {
                    txtFechaCredencial.set_selectedDate();
                    txtFechaCredencial.set_maxDate(FechaContrato);
                    txtFechaCredencial.set_selectedDate(FechaContrato);
                }

                $get("<%=lblTipo.ClientID %>").innerText = "Contrato: " + tipo;
            }
            else {

                txtFechaCredencial.set_selectedDate();
                txtFechaCredencial.set_enabled(false);

                $get("<%=lblTipo.ClientID %>").innerText = "";
            }
        }


        // Este metodo fue comentado hasta saber si la fecha de vencimiento
        // se calcula en base a la fecha cdo se esta afectado (DateTime.Now) o 
        // si es de acuerdo al periodo que se esta seleccionando. Ahora esta 
        // segun el prime caso.

        function SelectedPeriodo(sender, arg) {
            var contratosCombo = $find("<%= cboContratoLegajo.ClientID %>");
            var item = contratosCombo.get_selectedItem();
            var tipo = item.get_attributes().getAttribute("Tipo");

            if (tipo != "Auditable") {
                var periodo = arg.get_item().get_text();
                var pos1 = periodo.indexOf("/");
                ////esto es porque el objeto Date en el mes arranca desde 0
                mes = parseInt(lTrim0(periodo.substring(0, pos1))) - 1;
                dia = parseInt("1");
                año = parseInt(lTrim0(periodo.substring(pos1 + 1, periodo.length)));

                var FechaAux = new Date(año, mes, dia);

                $find("<%= txtFechaVenCredencial.ClientID%>").set_selectedDate(FechaAux);
            }

        }
    </script>
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
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Sunset" VisibleTitlebar="true"
        Style="z-index: 100000000" Title="Sub Contratistas">
        <Windows>
            <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close" Width="520" Height="280"
                Modal="true" NavigateUrl="ConsultaInformacionSueldos.aspx" VisibleTitlebar="true"
                Style="z-index: 100000000" Title="Información Sueldos" ReloadOnShow="true" VisibleStatusbar="false"
                ShowContentDuringLoad="false" Skin="Sunset">
            </telerik:RadWindow>
            <telerik:RadWindow ID="RadWindow2" runat="server" Behaviors="Close" Width="850" Height="440"
                Modal="true" NavigateUrl="ViewerCredenciales.aspx" VisibleTitlebar="true" Style="z-index: 100000000"
                Title="Impresion Credencial" ReloadOnShow="true" VisibleStatusbar="false" ShowContentDuringLoad="false"
                Skin="Sunset">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <asp:HiddenField ID="HiddenIdEmpresa" runat="server" />
    <asp:HiddenField ID="HiddenTipoUsuario" runat="server" />
    <asp:HiddenField ID="HiddenEsAdministrador" runat="server"></asp:HiddenField>
    <table cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td align="center" style="height: 35px; padding-left: 15px; padding-top: 10px">
                <asp:Label ID="lblTipoGestion" runat="server" Font-Bold="True" Font-Size="20pt" Font-Underline="false"
                    Font-Italic="True" ForeColor="black" Text="GESTION DE LEGAJOS" Font-Names="Arno Pro"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <table style="background-color: transparent; font-family: Arno Pro; font-size: 16px;
                width: 100%; vertical-align: middle;" border="0">
                <tr>
                    <td valign="middle" align="right" style="width: 240px">
                        <asp:Label ID="lblLegajo" runat="server" Font-Bold="True" ForeColor="#8C8C8C" Height="22px"
                            Text="Apellido:"></asp:Label>
                    </td>
                    <td valign="middle" align="left" style="width: 220px">
                        <telerik:RadTextBox ID="txtApellidoLegajo" runat="server" EmptyMessage="Ingrese apellido "
                            Skin="Sunset" Width="100%">
                        </telerik:RadTextBox>
                    </td>
                    <td valign="middle" align="left" style="width: 60px">
                        <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="#8C8C8C" Height="22px"
                            Text="NroDoc:"></asp:Label>
                    </td>
                    <td valign="middle" align="left" style="width: 220px">
                        <telerik:RadTextBox ID="txtNroDoc" runat="server" EmptyMessage="Ingrese Nro Doc"
                            Skin="Sunset" Width="100%">
                        </telerik:RadTextBox>
                    </td>
                    <td valign="middle" align="left">
                        <asp:ImageButton runat="server" Style="padding-left: 15px; padding-bottom: 15px;
                            border: 0px; vertical-align: middle;" ImageUrl="~/Images/Search.png" ID="imgBuscar"
                            OnClick="imgBuscar_Click" CausesValidation="false" Mensaje="Buscando Legajos.." />
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
    <table id="Table2" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
        border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #E0D6BE;
        font-family: Sans-Serif; font-size: 11px;" width="90%">
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpdPnlGrilla" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" AllowSorting="True"
                            ShowStatusBar="True" GridLines="None" Skin="Sunset" AllowAutomaticDeletes="True"
                            AllowAutomaticInserts="True" AllowAutomaticUpdates="True" AutoGenerateColumns="False"
                            PageSize="10" OnItemCommand="RadGrid1_ItemCommand" Width="100%">
                            <MasterTableView DataKeyNames="d.IdLegajos, dc.Codigo" ClientDataKeyNames="d.IdLegajos"
                                TableLayout="Fixed" CommandItemDisplay="Top" NoMasterRecordsText="No existen registros."
                                HorizontalAlign="NotSet">
                                <CommandItemTemplate>
                                    <div style="padding: 5px 5px;">
                                        <asp:LinkButton Mensaje="Buscar Legajo...." ID="btnEdit" runat="server" Visible='<%# RadGrid1.EditIndexes.Count == 0 %>'
                                            CausesValidation="false" OnClientClick="EditLegajo(); return false;">
                                            <img style="padding-right: 5px; border: 0px; vertical-align: middle;" alt="" src="Images/Edit.gif" /><span
                                                runat="server" id="lblEditar">Editar</span></asp:LinkButton>&nbsp;&nbsp;
                                        <asp:LinkButton Mensaje="Preparando para nuevo legajo...." ID="btnInsert" runat="server"
                                            CausesValidation="false" Visible='<%# !RadGrid1.MasterTableView.IsItemInserted %>'
                                            OnClientClick="InitInsert(); return false;">
                                <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/AddRecord.gif" />Insertar</asp:LinkButton>&nbsp;&nbsp;
                                        <asp:LinkButton Mensaje="Eliminando Legajo...." ID="btnDelete" OnClientClick="return blockConfirm('Esta seguro que desea eliminar el legajo seleccionado?', event, 330, 100,'','Legajos');"
                                            runat="server" OnClick="btnEliminar_Click" CausesValidation="false">
                                <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/delete_16x16.gif" />Eliminar</asp:LinkButton>&nbsp;&nbsp;
                                        <asp:LinkButton CausesValidation="false" Mensaje="Exportando Legajos...." ID="ExportExcel"
                                            runat="server" CommandName="ExportLegajos">
                                <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/Excel_16x16.gif" />Excel</asp:LinkButton>&nbsp;&nbsp;
                                        <asp:LinkButton CausesValidation="false" Mensaje="Cargan...." ID="btnSueldos" runat="server"
                                            OnClientClick="ShowSueldos(); return false;">
                                <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/notepad_16x16.gif" />Info. Sueldos</asp:LinkButton>&nbsp;&nbsp;
                                        <asp:LinkButton CausesValidation="false" Mensaje="Cargan...." ID="btnCredencial"
                                            runat="server" OnClientClick="ShowCredencial(); return false;">
                                <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/notepad_16x16.gif" />Credencial</asp:LinkButton>&nbsp;&nbsp;
                                        <asp:LinkButton CausesValidation="false" Mensaje="Cargan...." ID="btnCargarForo"
                                            runat="server" OnClientClick="ShowCargaFoto(); return false;">
                                <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/notepad_16x16.gif" />Foto</asp:LinkButton>&nbsp;&nbsp;
                                    </div>
                                </CommandItemTemplate>
                                <RowIndicatorColumn>
                                    <HeaderStyle Width="20px"></HeaderStyle>
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn>
                                    <HeaderStyle Width="20px"></HeaderStyle>
                                </ExpandCollapseColumn>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="d.IdLegajos" UniqueName="IdLegajosColumn" Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn DataField="d.Apellido" HeaderText="Apellido" SortExpression="Apellido"
                                        UniqueName="ApellidoColumn">
                                        <ItemTemplate>
                                            <asp:Label ID="ApellidoLabel" runat="server" Text='<%# Eval("d.Apellido") %>' Style="text-transform: capitalize"></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre"
                                        UniqueName="NombreColumn">
                                        <ItemTemplate>
                                            <asp:Label ID="NombreLabel" runat="server" Text='<%# Eval("d.Nombre").ToString().ToLower() %>'
                                                Style="text-transform: capitalize"></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="d.objTipoDocumento" Display="false" EditFormColumnIndex="1"
                                        HeaderText="Tipo Documento" SortExpression="TipoDocumento" UniqueName="TipoDocumentoColumn">
                                        <ItemTemplate>
                                            <asp:Label ID="EstadoCivilTextBoxccc" runat="server" Text='<%# Bind("d.objTipoDocumento.Descripcion") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="d.NroDoc" HeaderText="NroDoc" SortExpression="NroDoc"
                                        UniqueName="NroDocColumn">
                                        <ItemTemplate>
                                            <asp:Label ID="NroDocLabel" runat="server" Text='<%# Eval("d.NroDoc") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="d.CUIL" HeaderText="CUIL" SortExpression="CUIL"
                                        UniqueName="CUILColumn">
                                        <ItemTemplate>
                                            <asp:Label ID="CUILLabel" runat="server" Text='<%# string.Format("{0:##-########-#}", long.Parse(Eval("d.CUIL").ToString().Trim())) %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridDateTimeColumn DataField="d.FechaNacimiento" DataType="System.DateTime"
                                        HeaderText="Fecha Nacimiento" SortExpression="FechaNacimiento" UniqueName="FechaNacimientoColumn"
                                        Display="false" EditFormColumnIndex="0">
                                    </telerik:GridDateTimeColumn>
                                    <telerik:GridDateTimeColumn DataField="d.FechaUltimaVerificacion" DataType="System.DateTime"
                                        HeaderText="Fecha Verificación" SortExpression="FechaVerificacion" UniqueName="FechaVerificacionColumn"
                                        Display="false" EditFormColumnIndex="0">
                                    </telerik:GridDateTimeColumn>
                                    <telerik:GridDateTimeColumn DataField="d.FechaUltmaModificacion" DataType="System.DateTime"
                                        HeaderText="Fecha Ultima Modificación" SortExpression="FechaUltimaModificacion"
                                        UniqueName="FechaUltimaModificacionColumn" Display="false" EditFormColumnIndex="0">
                                    </telerik:GridDateTimeColumn>
                                    <telerik:GridBoundColumn DataField="d.CredVencimiento" UniqueName="CredVencimientoColumn"
                                        HeaderText="Vencimiento Credencial" Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="d.ObservacionBloqueo" UniqueName="ObservacionBloqueoColumn"
                                        HeaderText="Obs. Bloqueo" Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn DataField="d.CredVencimiento" HeaderText="Hab. Cred."
                                        UniqueName="HabCredColumn">
                                        <ItemTemplate>
                                            <asp:Label ID="lblHabCre" runat="server" Style="font-weight: bold" Text='<%# DateTime.Now < Convert.ToDateTime(Eval("d.CredVencimiento")) && Convert.ToDateTime(Eval("d.CredVencimiento")) <= Convert.ToDateTime(Eval("dc.FechaVencimiento")) ? "SÍ": "NO" %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="d.Direccion" HeaderText="Dirección" SortExpression="Direccion"
                                        UniqueName="DireccionColumn" Display="false" EditFormColumnIndex="0">
                                        <ItemTemplate>
                                            <asp:Label Width="90%" ID="lblDireccionTextBox" runat="server" Text='<%# Eval("d.Direccion") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="d.CodigoPostal" HeaderText="Codigo Postal" SortExpression="CodigoPostal"
                                        UniqueName="CodigoPostalColumn" Display="false" EditFormColumnIndex="1">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="d.TelefonoFijo" HeaderText="Telefono Fijo" SortExpression="TelefonoFijo"
                                        UniqueName="TelefonoFijoColumn" Display="false" EditFormColumnIndex="1">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="d.CorreoElectronico" HeaderText="Correo Electrónico"
                                        SortExpression="CorreoElectronico" UniqueName="CorreoElectronicoColumn" Display="false"
                                        EditFormColumnIndex="1">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn DataField="d.objEstadoCivil" Display="false" EditFormColumnIndex="1"
                                        HeaderText="Estado Civil" SortExpression="EstadoCivil" UniqueName="EstadoCivilColumn">
                                        <ItemTemplate>
                                            <asp:Label ID="EstadoCivilTextBoxGrilla" runat="server" Text='<%# Bind("d.objEstadoCivil.Descripcion") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="d.objNacionalidad" Display="false" EditFormColumnIndex="1"
                                        HeaderText="Nacionalidad" SortExpression="Nacionalidad" UniqueName="NacionalidadColumn">
                                        <ItemTemplate>
                                            <asp:Label ID="EstadoCivilTextBox" runat="server" Text='<%# Bind("d.objNacionalidad.Descripcion") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="d.objProvincia" Display="false" EditFormColumnIndex="1"
                                        HeaderText="Provincia" SortExpression="Provincia" UniqueName="ProvinciaColumn">
                                        <ItemTemplate>
                                            <asp:Label ID="EstadoCivildfdTextBoxccc" runat="server" Text='<%# Bind("d.objProvincia.Descripcion") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="d.objEstadoVerificacion" Display="false" EditFormColumnIndex="1"
                                        HeaderText="Estado Verificacion" SortExpression="EstadoVerificacion" UniqueName="EstadoVerificacionColumn">
                                        <ItemTemplate>
                                            <asp:Label ID="EstadoVerificacionTextBox" runat="server" Text='<%# Bind("d.objEstadoVerificacion.Descripcion") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="d.Observacion" UniqueName="ObservacionColumn"
                                        HeaderText="Observacion Auditoria" Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn DataField="d.objConvenio" Display="false" EditFormColumnIndex="1"
                                        HeaderText="Convenio" SortExpression="Convenio" UniqueName="ConvenioColumn">
                                        <ItemTemplate>
                                            <asp:Label ID="EstadoCiddvilTextBox" runat="server" Text='<%# Bind("d.objConvenio.Descripcion") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="d.FechaIngreos" Display="false" EditFormColumnIndex="2"
                                        HeaderText="Ingreso" UniqueName="FechaIngreosColumn">
                                        <ItemTemplate>
                                            <asp:Label ID="FechaIngreso" runat="server" Text='<%# Eval("d.FechaIngreos") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="d.GrupoSangre" HeaderText="Grupo Sangre" UniqueName="GrupoSangreColumn"
                                        Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="d.Funcion" UniqueName="FuncionColumn" HeaderText="Función"
                                        Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="d.AutorizadoCond" UniqueName="AutorizadoCondColumn"
                                        Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="d.EstudiosBasicos" UniqueName="EstudiosBasicosColumn"
                                        Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="d.ComplementarioRacs" UniqueName="ComplementarioRacsColumn"
                                        Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="d.AdicionalQuimicos" UniqueName="AdicionalQuimicosColumn"
                                        Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Estudios Básicos" DataField="DesEstudiosBasicos"
                                        UniqueName="DesEstudiosBasicosColumn" Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Comp. Racs" DataField="DesComplementarioRacs"
                                        UniqueName="DesComplementarioRacsColumn" Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Adicional Químicos" DataField="DesAdicionalQuimicos"
                                        UniqueName="DesAdicionalQuimicosColumn" Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="d.FechaUltimoExamen" UniqueName="FechaUltimoExamenColumn"
                                        HeaderText="Fecha Ult. Exámen" Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="d.RutaFoto" UniqueName="RutaFotoColumn" Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="d.EmpresaLegajo" UniqueName="EmpresaLegajoColumn"
                                        Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="d.objEmpresaLegajo.RazonSocial" UniqueName="EmpresaLegajoRazonSocial"
                                        HeaderText="" Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="dc.Contratista" UniqueName="ContratistaColumn"
                                        HeaderStyle-Width="140px" HeaderStyle-HorizontalAlign="Center" HeaderText="Contratista"
                                        Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="dc.SubContratista" UniqueName="SubContratistaColumn"
                                        HeaderStyle-Width="140px" HeaderStyle-HorizontalAlign="Center" HeaderText="Sub Contratista"
                                        Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="d.NroPoliza" HeaderText="Nro Poliza" UniqueName="NroPolizaColumn"
                                        Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="d.CompañiaSeguro" UniqueName="IdCompañiaColumn"
                                        Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="d.objCompañiaSeguro.Descripcion" UniqueName="objCompañiaSeguroDescripcion"
                                        HeaderText="Compañia Seguro" Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="d.FechaInicial" UniqueName="FechaInicialColumn"
                                        HeaderText="Inicio Seguro" Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="d.FechaUltimoPago" UniqueName="FechaUltimoPagoColumn"
                                        HeaderText="Ultimo Pago Seg." Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="d.FechaVencimiento" UniqueName="FechaVencimientoColumn"
                                        HeaderText="Vencimiento Seguro" Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="d.Descripcion" UniqueName="DescripcionColumn"
                                        Display="false" HeaderText="Descripción Seguro">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="dc.Codigo" UniqueName="ContratoColumn" Display="true"
                                        HeaderText="Contrato">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="dc.FechaVencimiento" UniqueName="FechaVencimientoContratoColumn"
                                        Display="false" HeaderText="Vencimiento Contrato">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="dc.Periodo" UniqueName="PeriodoColumn" Display="true"
                                        HeaderText="Periodo">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="dc.CategoriaContrato" UniqueName="CategoriaContratoColumn"
                                        HeaderStyle-Width="140px" HeaderStyle-HorizontalAlign="Center" Display="true"
                                        HeaderText="Categoria">
                                    </telerik:GridBoundColumn>
                                </Columns>
                                <EditFormSettings ColumnNumber="3" CaptionDataField="Apellido" CaptionFormatString="Editar el Legajo: {0}">
                                    <FormTableItemStyle Wrap="False" HorizontalAlign="Left"></FormTableItemStyle>
                                    <FormTableAlternatingItemStyle Wrap="False" HorizontalAlign="Left"></FormTableAlternatingItemStyle>
                                    <FormCaptionStyle HorizontalAlign="Center" Width="100%"></FormCaptionStyle>
                                    <FormMainTableStyle GridLines="Both" CellSpacing="0" CellPadding="3" BackColor="White"
                                        Width="100%" HorizontalAlign="Left" />
                                    <FormTableStyle CellSpacing="0" CellPadding="2" Width="100%" Height="110px" BackColor="White"
                                        HorizontalAlign="Left" />
                                    <FormStyle Width="100%" BackColor="#eef2ea"></FormStyle>
                                    <EditColumn ButtonType="ImageButton" InsertText="Insertar" UpdateText="Actualizar"
                                        UniqueName="EditCommandColumn1" CancelText="Cancelar">
                                    </EditColumn>
                                    <FormTableButtonRowStyle HorizontalAlign="Right"></FormTableButtonRowStyle>
                                    <PopUpSettings ScrollBars="Auto" Modal="true" Width="90%" />
                                </EditFormSettings>
                            </MasterTableView>
                            <ValidationSettings CommandsToValidate="PerformInsert,Update" />
                            <ClientSettings>
                                <Selecting AllowRowSelect="True" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <cc1:ServerControlWindow ID="ServerControlWindow1" runat="server" BackColor="WhiteSmoke"
        WindowColor="Rojo">
        <ContentControls>
            <div id="divPrincipal" style="height: 580px; width: 900px; overflow: auto">
                <cc1:ServerPanel runat="server" ID="pnlEdicion">
                    <BodyContent>
                        <table border="0" cellpadding="2" cellspacing="2" width="98%" style="padding-top: 0px;
                            text-align: left;">
                            <tr style="padding-top: 5px; padding-bottom: 5px">
                                <td colspan="5" style="background-color: #CACACA; padding-left: 10px; cursor: pointer"
                                    onclick="return toggleTbodyGroup('DP');">
                                    <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                        <tr>
                                            <td align="left" style="width: 95%">
                                                <asp:Label ID="Label22" runat="server" SkinID="lblConosudTitulo" Text="DATOS PRINCIPALES"></asp:Label>
                                            </td>
                                            <td align="center">
                                                <image id="imgDP" src="images/up.png"></image>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <table cellpadding="0" cellspacing="2" width="100%" border="0" style="text-align: left">
                                        <tbody id="DP">
                                            <tr>
                                                <td style="width: 130px">
                                                    <asp:Label ID="Label2" runat="server" SkinID="lblConosud" Text="Apellido:"></asp:Label>
                                                </td>
                                                <td align="left" style="padding-left: 5px; padding-right: 5px">
                                                    <asp:TextBox Width="250px" ID="txtApellido" runat="server" Style="text-transform: uppercase"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorApellido" ControlToValidate="txtApellido"
                                                        Display="Dynamic" ErrorMessage="*" runat="server">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label3" runat="server" SkinID="lblConosud" Text="Nombre:"></asp:Label>
                                                </td>
                                                <td style="padding-left: 5px; padding-right: 5px" colspan="2">
                                                    <asp:TextBox Width="250px" ID="txtNombre" runat="server" Style="text-transform: uppercase"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorNombre" ControlToValidate="txtNombre"
                                                        Display="Dynamic" ErrorMessage="*" runat="server">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label14" runat="server" SkinID="lblConosud" Text="Tipo Doc:"></asp:Label>
                                                </td>
                                                <td style="padding-left: 5px; padding-right: 5px">
                                                    <telerik:RadComboBox ID="cboTipoDoc" runat="server" Skin="Sunset" Width="95%" EmptyMessage="Seleccione Tipo Documento"
                                                        ZIndex="922000000" DataValueField="IdClasificacion" DataTextField="Descripcion"
                                                        AllowCustomText="true">
                                                    </telerik:RadComboBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label4" runat="server" SkinID="lblConosud" Text="Nro:"></asp:Label>
                                                </td>
                                                <td style="padding-left: 5px; padding-right: 5px">
                                                    <asp:UpdatePanel ID="upNroDoc" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox Width="100px" ID="txtNroDocEdit" runat="server"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorNroDoc" ControlToValidate="txtNroDocEdit"
                                                                Display="Dynamic" ErrorMessage="*" runat="server">
                                                            </asp:RequiredFieldValidator>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td valign="middle" rowspan="4" align="center">
                                                    <div style="border: medium double #800000">
                                                        <asp:Image ID="imgLegajo" runat="server" Width="90px" Height="90px" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label6" runat="server" SkinID="lblConosud" Text="CUIL:"></asp:Label>
                                                </td>
                                                <td style="padding-left: 5px; padding-right: 5px">
                                                    <telerik:RadMaskedTextBox runat="server" ID="txtCUIL" Mask="##-########-#" DisplayMask="##-########-#"
                                                        DisplayPromptChar=" " />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorCuil" ControlToValidate="txtCUIL"
                                                        Display="Dynamic" ErrorMessage="*" runat="server">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                                <td style="width: 180px">
                                                    <asp:Label ID="Label5" runat="server" SkinID="lblConosud" Text="Fecha Nacimiento:"></asp:Label>
                                                </td>
                                                <td style="padding-left: 5px; padding-right: 5px">
                                                    <telerik:RadDatePicker ID="txtFechaNacimiento" MinDate="1950/1/1" runat="server"
                                                        ZIndex="922000000" Width="125px">
                                                        <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                                    </telerik:RadDatePicker>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label7" runat="server" SkinID="lblConosud" Text="Direccion:"></asp:Label>
                                                </td>
                                                <td style="padding-left: 5px; padding-right: 5px">
                                                    <asp:TextBox Width="250px" ID="txtDireccion" MaxLength="50" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label8" runat="server" SkinID="lblConosud" Text="Codigo Postal:"></asp:Label>
                                                </td>
                                                <td style="padding-left: 5px; padding-right: 5px">
                                                    <asp:TextBox Width="80px" ID="txtCodigoPostal" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label9" runat="server" SkinID="lblConosud" Text="Telefono Fijo:"></asp:Label>
                                                </td>
                                                <td style="padding-left: 5px; padding-right: 5px">
                                                    <asp:TextBox Width="150px" ID="txtTelFijo" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label10" runat="server" SkinID="lblConosud" Text="Email:"></asp:Label>
                                                </td>
                                                <td style="padding-left: 5px; padding-right: 5px">
                                                    <asp:TextBox Width="220px" ID="txtEmail" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label11" runat="server" SkinID="lblConosud" Text="Estado Civil:"></asp:Label>
                                                </td>
                                                <td style="padding-left: 5px; padding-right: 5px">
                                                    <telerik:RadComboBox ID="cboEstadoCivil" runat="server" Skin="Sunset" Width="95%"
                                                        ZIndex="922000000" EmptyMessage="Seleccione Estado Civil" DataValueField="IdClasificacion"
                                                        DataTextField="Descripcion" AllowCustomText="true">
                                                    </telerik:RadComboBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label12" runat="server" SkinID="lblConosud" Text="Nacionalidad:"></asp:Label>
                                                </td>
                                                <td style="padding-left: 5px; padding-right: 5px" colspan="2">
                                                    <telerik:RadComboBox ID="cboNacionalidad" runat="server" Skin="Sunset" Width="95%"
                                                        ZIndex="922000000" DataValueField="IdClasificacion" DataTextField="Descripcion"
                                                        EmptyMessage="Seleccione la Nacionalidad" AllowCustomText="true">
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label13" runat="server" SkinID="lblConosud" Text="Provincia:"></asp:Label>
                                                </td>
                                                <td style="padding-left: 5px; padding-right: 5px">
                                                    <telerik:RadComboBox ID="cboProvincia" runat="server" Skin="Sunset" Width="95%" ZIndex="922000000"
                                                        DataValueField="IdClasificacion" DataTextField="Descripcion" EmptyMessage="Seleccione la Provincia"
                                                        AllowCustomText="true">
                                                    </telerik:RadComboBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label15" runat="server" SkinID="lblConosud" Text="Convenio:"></asp:Label>
                                                </td>
                                                <td style="padding-left: 5px; padding-right: 5px" colspan="2">
                                                    <telerik:RadComboBox ID="cboConvenio" runat="server" Skin="Sunset" Width="95%" ZIndex="922000000"
                                                        DataValueField="IdClasificacion" DataTextField="Descripcion" EmptyMessage="Seleccione el convenio"
                                                        AllowCustomText="true">
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label16" runat="server" SkinID="lblConosud" Text="F. Ingreso:"></asp:Label>
                                                </td>
                                                <td align="left" style="padding-left: 5px; padding-right: 5px">
                                                    <telerik:RadDatePicker ID="txtFechaIngreso" MinDate="1950/1/1" runat="server" ZIndex="922000000">
                                                    </telerik:RadDatePicker>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label26" runat="server" SkinID="lblConosud" Text="Grupo y Factor:"></asp:Label>
                                                </td>
                                                <td align="left" style="padding-left: 5px; padding-right: 5px" colspan="2">
                                                    <asp:TextBox Width="120px" ID="txtGrupoSangre" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr runat="server">
                                                <td>
                                                    <asp:Label ID="Label31" runat="server" SkinID="lblConosud" Text="Funcion :"></asp:Label>
                                                </td>
                                                <td colspan="3" align="left" style="padding-left: 5px; padding-right: 5px">
                                                    <asp:TextBox Width="240px" ID="txtFuncion" runat="server" MaxLength="35"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="rowEmpresaLegajo" runat="server">
                                                <td>
                                                    <asp:Label ID="Label42" runat="server" SkinID="lblConosud" Text="Empresa:"></asp:Label>
                                                </td>
                                                <td colspan="3" style="padding-left: 5px; padding-right: 5px">
                                                    <telerik:RadComboBox ID="cboEmpresaLegajo" runat="server" Skin="Sunset" Width="95%"
                                                        ZIndex="922000000" DataValueField="IdEmpresa" DataTextField="RazonSocial" AllowCustomText="true"
                                                        MarkFirstMatch="true" EmptyMessage="Seleccione la empresa">
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr id="HDCredencial" runat="server" style="padding-top: 5px; padding-bottom: 5px">
                                <td colspan="5" style="background-color: #CACACA; padding-left: 10px; cursor: pointer"
                                    onclick="return toggleTbodyGroup('C');">
                                    <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                        <tr>
                                            <td align="left" style="width: 95%">
                                                <asp:Label ID="Label18" runat="server" SkinID="lblConosudTitulo" Text="CREDENCIAL"></asp:Label>
                                            </td>
                                            <td align="center">
                                                <image id="imgC" src="images/up.png"></image>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:Panel ID="PnlCredencial" runat="server">
                                        <table cellpadding="0" cellspacing="2" width="100%" border="0" style="text-align: left">
                                            <tbody id="C">
                                                <tr id="rowContratoLegajo" runat="server">
                                                    <td width="10%">
                                                        <asp:Label ID="Label20" runat="server" SkinID="lblConosud" Text="Contrato:"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <telerik:RadComboBox ID="cboContratoLegajo" runat="server" Skin="Sunset" Width="50%"
                                                            OnClientSelectedIndexChanged="SelectedContrato" ZIndex="922000000" EmptyMessage="Debe Seleccionar la Empresa"
                                                            OnItemsRequested="cboContratos_ItemsRequested" OnClientItemsRequested="ItemsLoaded"
                                                            AllowCustomText="false">
                                                        </telerik:RadComboBox>
                                                        <asp:Label ID="lblTipo" runat="server" Text="" Style="padding-left: 10px; font-weight: bold;
                                                            color: Red"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="rowPeriodoLegajo" runat="server">
                                                    <td width="10%">
                                                        <asp:Label ID="Label21" runat="server" SkinID="lblConosud" Text="Periodo:"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <telerik:RadComboBox ID="cboPeriodoLegajo" runat="server" Skin="Sunset" Width="50%"
                                                            ZIndex="922000000" EmptyMessage="Debe Seleccionar el Contrato" OnItemsRequested="cboPeriodos_ItemsRequested"
                                                            OnClientItemsRequested="ItemsLoaded" AllowCustomText="false">
                                                        </telerik:RadComboBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="10%">
                                                        <asp:Label ID="Label30" runat="server" SkinID="lblConosud" Text="Venc. Cred:"></asp:Label>
                                                    </td>
                                                    <td align="left" width="50%">
                                                        <telerik:RadDatePicker ID="txtFechaVenCredencial" MinDate="1950/1/1" runat="server"
                                                            ZIndex="922000000">
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="10%">
                                                        <asp:Label ID="Label32" runat="server" SkinID="lblConosud" Text="Obs. Bloqueo:"></asp:Label>
                                                    </td>
                                                    <td align="left" width="50%">
                                                        <asp:TextBox Width="90%" ID="txtObsBloqueo" Rows="2" TextMode="MultiLine" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="10%">
                                                        <asp:Label ID="Label37" runat="server" SkinID="lblConosud" Text="Autorizado a Conducir:"></asp:Label>
                                                    </td>
                                                    <td align="left" width="50%">
                                                        <asp:CheckBox ID="chkAutorizadoConducir" SkinID="chkConosud" runat="server" Text="">
                                                        </asp:CheckBox>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr style="padding-top: 5px; padding-bottom: 5px">
                                <td colspan="5" style="background-color: #CACACA; padding-left: 10px; cursor: pointer"
                                    onclick="return toggleTbodyGroup('DMed');">
                                    <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                        <tr>
                                            <td align="left" style="width: 95%">
                                                <asp:Label ID="Label27" runat="server" SkinID="lblConosudTitulo" Text="DATOS MEDICOS"></asp:Label>
                                            </td>
                                            <td align="center">
                                                <image id="imgDMed" src="images/up.png"></image>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <table cellpadding="0" cellspacing="0" width="100%" border="0" style="text-align: left">
                                        <tbody id="DMed">
                                            <tr>
                                                <td style="width: 20%">
                                                    <asp:Label ID="Label17" runat="server" SkinID="lblConosud" Text="F. Último Examen Médico:"></asp:Label>
                                                </td>
                                                <td colspan="5" align="left" style="padding-left: 5px; padding-right: 5px">
                                                    <telerik:RadDatePicker ID="txtFechaUltExa" runat="server" ZIndex="922000000">
                                                    </telerik:RadDatePicker>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%">
                                                    <asp:Label ID="Label38" runat="server" SkinID="lblConosud" Text="Estudio Básico:"></asp:Label>
                                                </td>
                                                <td align="left" style="padding-right: 5px">
                                                    <asp:CheckBox ID="chkEstudosBasicos" SkinID="chkConosud" runat="server" Text="">
                                                    </asp:CheckBox>
                                                </td>
                                                <td style="width: 25%">
                                                    <asp:Label ID="Label39" runat="server" SkinID="lblConosud" Text="Complementario para RACs:"></asp:Label>
                                                </td>
                                                <td align="left" style="padding-right: 5px">
                                                    <asp:CheckBox ID="chkCompRacs" SkinID="chkConosud" runat="server" Text=""></asp:CheckBox>
                                                </td>
                                                <td style="width: 25%">
                                                    <asp:Label ID="Label40" runat="server" SkinID="lblConosud" Text="Adicional Productos Químicos:"></asp:Label>
                                                </td>
                                                <td align="left" style="padding-right: 5px">
                                                    <asp:CheckBox ID="chkAdicionalQuimicos" SkinID="chkConosud" runat="server" Text="">
                                                    </asp:CheckBox>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr id="HdDatosAuditoria" runat="server" style="padding-top: 5px; padding-bottom: 5px">
                                <td colspan="5" style="background-color: #CACACA; padding-left: 10px; cursor: pointer"
                                    onclick="return toggleTbodyGroup('DA');">
                                    <table cellpadding="0" cellspacing="0" width="100%" border="0" style="text-align: left">
                                        <tr>
                                            <td align="left" style="width: 95%">
                                                <asp:Label ID="Label25" runat="server" SkinID="lblConosudTitulo" Text="DATOS AUDITORIA"></asp:Label>
                                            </td>
                                            <td align="center">
                                                <image id="imgDA" src="images/up.png"></image>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="TrDatosAuditoria" runat="server">
                                <td colspan="5">
                                    <asp:Panel ID="PnlDtosAuditoria" runat="server">
                                        <table cellpadding="0" cellspacing="0" width="100%" border="0" style="text-align: left">
                                            <tbody id="DA">
                                                <tr>
                                                    <td style="display:none" >
                                                        <asp:Label ID="lblEstVer" runat="server" SkinID="lblConosud" Text="Estado Verificación.:"></asp:Label>
                                                    </td>
                                                    <td align="left" style="padding-left: 5px; padding-right: 5px;display:none">
                                                        <telerik:RadComboBox ID="cboEstadoVerificacion" runat="server" Skin="Sunset" Width="220px"
                                                            ZIndex="922000000" EmptyMessage="Seleccione tipo Verificacion" DataValueField="IdClasificacion"
                                                            DataTextField="Descripcion" AllowCustomText="true">
                                                        </telerik:RadComboBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblUltVer" runat="server" SkinID="lblConosud" Text="Ult. Verificación:"></asp:Label>
                                                    </td>
                                                    <td align="left" style="padding-left: 5px; padding-right: 5px" colspan="5">
                                                        <telerik:RadDatePicker ID="txtFechaVerificacion" MinDate="1950/1/1" runat="server"
                                                            ZIndex="922000000">
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblObs" runat="server" SkinID="lblConosud" Text="Observación:"></asp:Label>
                                                    </td>
                                                    <td align="left" style="padding-left: 5px; padding-right: 5px" colspan="3">
                                                        <asp:TextBox Width="600px" ID="txtObservacion" Rows="5" TextMode="MultiLine" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblUltMod" runat="server" SkinID="lblConosud" Text="Ult. Mod.:"></asp:Label>
                                                    </td>
                                                    <td align="left" style="padding-left: 5px; padding-right: 5px" colspan="3">
                                                        <asp:TextBox Width="150px" ID="txtFechaUltimaModificacion" Enabled="false" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr style="padding-top: 5px; padding-bottom: 5px">
                                <td colspan="5" style="background-color: #CACACA; padding-left: 10px; cursor: pointer"
                                    onclick="return toggleTbodyGroup('SEG');">
                                    <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                        <tr>
                                            <td align="left" style="width: 95%">
                                                <asp:Label ID="Label24" runat="server" SkinID="lblConosudTitulo" Text="DATOS SEGUROS"></asp:Label>
                                            </td>
                                            <td align="center">
                                                <image id="imgSEG" src="images/up.png"></image>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <table cellpadding="0" cellspacing="0" width="100%" border="0" style="text-align: left">
                                        <tbody id="SEG">
                                            <tr>
                                                <td style="width: 120px">
                                                    <asp:Label ID="Label19" runat="server" SkinID="lblConosud" Text="Compañia:"></asp:Label>
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
                                                    <asp:Label ID="Label35" runat="server" SkinID="lblConosud" Text="Fecha Ult. Pago:"></asp:Label>
                                                </td>
                                                <td style="padding-left: 5px; padding-right: 5px">
                                                    <telerik:RadDatePicker ID="txtFechaUltPago" MinDate="1950/1/1" runat="server" ZIndex="922000000">
                                                    </telerik:RadDatePicker>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label36" runat="server" SkinID="lblConosud" Text="Descipción:"></asp:Label>
                                                </td>
                                                <td style="padding-left: 5px; padding-right: 5px">
                                                    <asp:TextBox Width="90%" ID="txtDescripcion" runat="server" Rows="4" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" style="background-color: #CACACA; padding-left: 10px; cursor: pointer"
                                    onclick="return toggleTbodyGroup('RAC');" style="padding-top: 5px; padding-bottom: 5px">
                                    <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                        <tr>
                                            <td align="left" style="width: 95%">
                                                <asp:Label ID="Label23" runat="server" SkinID="lblConosudTitulo" Text="RAC"></asp:Label>
                                            </td>
                                            <td align="center">
                                                <image id="imgRAC" src="images/up.png"></image>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" align="center" style="padding-top: 5px">
                                    <table cellpadding="0" cellspacing="0" width="100%" border="0" style="text-align: left">
                                        <tbody id="RAC">
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdPnlCursos" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <telerik:RadGrid ID="gvRAC" runat="server" AutoGenerateColumns="False" Skin="Sunset"
                                                                GridLines="None" PageSize="50" Width="95%">
                                                                <MasterTableView ShowHeadersWhenNoRecords="true" NoMasterRecordsText="El legajo no posee cursos"
                                                                    ClientDataKeyNames="IdCursoLegajo" DataKeyNames="IdCursoLegajo">
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn DataField="Curso" HeaderText="Curso">
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="FechaCurso" HeaderText="Fecha" DataFormatString="{0:d}">
                                                                            <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="FechaVencimiento" HeaderText="Fecha Vencimiento"
                                                                            UniqueName="FechaVencimientoColumn" DataFormatString="{0:d}">
                                                                            <HeaderStyle Width="120px" HorizontalAlign="Center" />
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="Dictado" HeaderText="Dictado Por">
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="Aprobado" Display="false" UniqueName="AprobadoNoDisplayColumn">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="Aprobado" UniqueName="AprobadoColumn">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkAprobado" runat="server" Checked='<%# (DataBinder.Eval(Container.DataItem,"Aprobado").ToString()!="False"?true:false) %>' />
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn UniqueName="IdCursoLegajoColumn" Display="false">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="hiddenId" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"IdCursoLegajo").ToString()%>'></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                    </Columns>
                                                                </MasterTableView>
                                                                <ClientSettings>
                                                                    <ClientEvents OnCommand="function(){}" OnRowDataBound="gvRAC_RowDataBound" />
                                                                    <Selecting AllowRowSelect="false" />
                                                                </ClientSettings>
                                                            </telerik:RadGrid>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" align="center" style="padding-top: 10px">
                                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar Datos" Mensaje="Buscando Legajos solicitados..."
                                        CausesValidation="true" OnClientClick="return GuardarCambios(); " SkinID="btnConosudBasic" />
                                </td>
                            </tr>
                        </table>
                    </BodyContent>
                </cc1:ServerPanel>
            </div>
        </ContentControls>
    </cc1:ServerControlWindow>
    <cc1:ServerControlWindow ID="ServerControlWindowFoto" runat="server" BackColor="WhiteSmoke"
        WindowColor="Rojo">
        <ContentControls>
            <div id="divPricipalFoto" style="height: 340px; width: 550px">
                <table style="border-right: #843431 thin solid; border-top: #843431 thin solid; border-left: #843431 thin solid;
                    border-bottom: #843431 thin solid; background-color: #E0D6BE; font-family: Sans-Serif;
                    font-size: 11px;">
                    <tr>
                        <td valign="top" align="left">
                            <asp:Label ID="Lassbel5" SkinID="lblConosud" runat="server" Text="Archivo:"></asp:Label>
                        </td>
                        <td valign="top" align="left">
                            <asp:UpdatePanel runat="server" ID="UpArchivo" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <telerik:RadUpload ID="RadUpload1" runat="server" ControlObjectsVisibility="None"
                                        OverwriteExistingFiles="true" Skin="Sunset" InputSize="35" TargetFolder="~/ImagenesLegajos"
                                        Width="500px" Height="25px">
                                    </telerik:RadUpload>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="buttonSubmit" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td valign="top" align="left">
                            <asp:Button ID="buttonSubmit" SkinID="btnConosudBasic" OnClientClick="return ValidarArchivo();"
                                runat="server" OnClick="buttonSubmit_Click" Text="Subir Archivo" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="center" colspan="3">
                            <telerik:RadProgressManager ID="RadProgressManager1" runat="server" />
                            <telerik:RadProgressArea ID="RadProgressArea1" runat="server" Skin="Sunset" ProgressIndicators="RequestSize, FilesCountBar, FilesCount, FilesCountPercent, SelectedFilesCount, CurrentFileName, TimeElapsed, TimeEstimated, TransferSpeed">
                                <Localization Uploaded="Uploaded" CurrentFileName="Subiendo Foto: " />
                            </telerik:RadProgressArea>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentControls>
    </cc1:ServerControlWindow>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" ClientEvents-OnResponseEnd="ResponseEnd"
        ClientEvents-OnRequestStart="requestStart1" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <ClientEvents OnRequestStart="requestStart1"></ClientEvents>
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="gvRAC">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvRAC" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
