<%@ Page Language="C#" Theme="SkinMarzzan" AutoEventWireup="true" CodeFile="GestionSolicitudAltaConsultor.aspx.cs"
    Inherits="GestionSolicitudAltaConsultor" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Solicitud Alta Nuevo Consultor</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7">
    <link href="Styles.css" type="text/css" rel="stylesheet" />
    <link href="UploadFiles/CSS/uploadify.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.3.1.js" type="text/javascript"></script>
    <script src="Scripts/jquery.tmpl.1.1.1.js" type="text/javascript"></script>
    <script src="Scripts/jquery.uploadify.js" type="text/javascript"></script>
    <script src="Scripts/jquery.autocomplete.js" type="text/javascript"></script>
    <style type="text/css">
        .style1
        {
            width: 90%;
        }
        .style2
        {
            text-align: left;
            width: 150px;
        }
        .style3
        {
            text-align: left;
            height: 32px;
        }
        .style4
        {
            height: 32px;
        }
        .style5
        {
            text-align: left;
            height: 26px;
        }
        .style6
        {
            height: 26px;
        }
        .style7
        {
            text-align: left;
            height: 27px;
        }
        .style8
        {
            height: 27px;
        }
    </style>
</head>
<script type="text/javascript">


    var ctrDepartamento;
    var ctrlocalidad;

    function CargarDepartamentos(sender, arg) {
        //debugger;

        $('#' + "<%= txtdepartamento.ClientID %>").val("");
        $('#' + "<%= txtdepartamento.ClientID %>").html("");
        $('#' + "<%= txtNuevo_Depart_loc.ClientID %>").val("");
        $('#' + "<%= txtNuevo_Depart_loc.ClientID %>").html("");

        jQuery(function () {
            var empresa = arg.get_item().get_text();

            //            if (empresa == "NEUQU&eacute;N") { empresa = "NEUQUEN"; }
            //if (empresa == "C&oacute;RDOBA") { empresa = "C&Oacute;RDOBA"; }
            //            if (empresa == "ENTRE R&iacute;OS") { empresa = "ENTRE RIOS"; }
            //            if (empresa == "TUCUM&aacute;N") { empresa = "TUCUMAN"; }
            //            if (empresa == "R&iacute;O NEGRO") { empresa = "RIO NEGRO"; }

            if (ctrDepartamento == null) {
                options = {
                    serviceUrl: 'ASHX/LoadDepartamentos.ashx',
                    width: '384',
                    minChars: 2,
                    showInit: true,
                    showOnFocus: true,
                    params: { Empresa: encodeURIComponent(empresa) },
                    zIndex: 922000000,
                    onSelect: showLocalidades,
                    onBlurFunction: showLocalidades
                };

                ctrDepartamento = $('#' + "<%= txtdepartamento.ClientID %>").autocomplete(options);

            }
            else {
                ctrDepartamento.changeParams({ Empresa: encodeURIComponent(empresa) });
            }

            $("#<%= txtdepartamento.ClientID %>").focus();
        });

    }

    function showLocalidades(value, data, obj, aditionalData) {
        if (value == undefined || value == '' || value == 'NO ESTA EN LA LISTA') {

            $("#<%= txtNuevo_Depart_loc.ClientID %>").removeAttr("disabled");
            $("#<%= txtNuevo_Depart_loc.ClientID %>").css("background-color", "white");
            $("#<%= txtNuevo_Depart_loc.ClientID %>").focus();

            $("#<%= txtCodigoPostal.ClientID %>").val("");
            $("#<%= txtCodigoPostal.ClientID %>").attr("disabled", false);
            $("#<%= txtCodigoPostal.ClientID %>").css("background-color", "#fff");


            var val = document.getElementById("<%=RequiredFieldValidator12.ClientID %>");
            ValidatorEnable(val, true);

        }
        else {
            $("#<%= txtNuevo_Depart_loc.ClientID %>").val("");
            $("#<%= txtNuevo_Depart_loc.ClientID %>").attr("disabled", true);
            $("#<%= txtNuevo_Depart_loc.ClientID %>").css("background-color", "#CCCCCC");
            $find("<%= txtDireccion.ClientID %>").focus();

            $("#<%= txtCodigoPostal.ClientID %>").val(aditionalData);
            $("#<%= txtCodigoPostal.ClientID %>").attr("disabled", true);
            $("#<%= txtCodigoPostal.ClientID %>").css("background-color", "#fff");



            var val = document.getElementById("<%=RequiredFieldValidator12.ClientID %>");
            ValidatorEnable(val, false);
        }

        return;

        //        $('#' + "<%= txtNuevo_Depart_loc.ClientID %>").val("");
        //        $('#' + "<%= txtNuevo_Depart_loc.ClientID %>").html("");

        //        if (ctrlocalidad == null) {
        //            options = {
        //                serviceUrl: 'ASHX/LoadLocalidades.ashx',
        //                width: '384',
        //                minChars: 2,
        //                showInit: true,
        //                showOnFocus: true,
        //                params: { Departamento: data },
        //                zIndex: 922000000
        //            };

        //            ctrlocalidad = $('#' + "<%= txtNuevo_Depart_loc.ClientID %>").autocomplete(options);

        //        }
        //        else {
        //            ctrlocalidad.changeParams({ Departamento: data });
        //        }

        //        $("#<%= txtNuevo_Depart_loc.ClientID %>").focus();


    }

    function CalcularCUIT() {
        //debugger;
        var suma = 0;
        var digitoVerificador = "";
        var tipoCuit = "";
        var DNI = $find("<%= txtDni.ClientID %>").get_value();




        if (DNI.length == 8 || DNI.length == 7) {
            if (DNI.length == 7) DNI = "0" + DNI;

            // Reglas para el tipo de CUIT/CUIL
            //        27 significa que es mujer 
            //        20 hombre
            //        23 puede ser ambos (se usa cuando hay otro n&uacute;mero igual)
            //        30 empresas
            if ($find("<%= cboTipoPersona.ClientID %>").get_value() == "PJ") {
                tipoCuit = "30";
            }
            else {
                if ($find("<%= cboSexo.ClientID %>").get_value() == "Masculino") {
                    tipoCuit = "20";
                }
                else {
                    tipoCuit = "27";
                }
            }


            var datos = (tipoCuit + DNI).split('');
            suma += datos[0] * 5;
            suma += datos[1] * 4;
            suma += datos[2] * 3;
            suma += datos[3] * 2;
            suma += datos[4] * 7;
            suma += datos[5] * 6;
            suma += datos[6] * 5;
            suma += datos[7] * 4;
            suma += datos[8] * 3;
            suma += datos[9] * 2;

            var res = suma % 11;

            // Reglas para el D&iacute;gito Verificador
            //        Si res = 0, entonces Y = 0.
            //        Si res = 1, entonces Y = 9.
            //        Sino, Y = 11 – res.
            if (res == 0)
                digitoVerificador = 0;
            else if (res == 1)
                digitoVerificador = 9;
            else
                digitoVerificador = 11 - res;


            $find("<%= txtCuit.ClientID %>").set_value(tipoCuit + "-" + DNI + "-" + digitoVerificador);
        }
        else {
            $find("<%= txtCuit.ClientID %>").set_value("");
        }

    }

    function ShowNuevaDireccion() {
        oWnd = radopen("GestionDireccionesEntrega.aspx?SolicitudAlta=true", "RadWindow1");
    }

    function OnClientClose(oWnd) {
        if (oWnd.argument) {
            $find("<%=RadAjaxManager1.ClientID%>").ajaxRequest("");
        }
    }

    function ConfirmacionEnvio() {
        radalert("La solicitud de alta ha sido enviada al personal de asistencia correctamente.");
        setTimeout("ReLoadPage()", 8000);
    }

    function AvisoClienteExistente() {
        radalert("La solicitud no se ha completado ya que el revendedor ya existe en la base de datos de clientes. Por favor comun&iacute;quese con Asistencia Comercial.", 300, 150, "Cliente Existente");
        setTimeout("ReLoadPage()", 12000);
    }

    function AvisoClienteExistenteSolicitud() {
        radalert("La solicitud no se ha completado ya que el revendedor posee una SOLICITUD DE ALTA PREVIA. Por favor comun&iacute;quese con Asistencia Comercial.", 300, 150, "Cliente Existente");
        setTimeout("ReLoadPage()", 12000);
    }

    function AvisoError(catchError) {
        radalert('A ocurrido un inconveniente con el alta, por favor intentetelo nuevamente. De persistir el error tome contacto con el personal de asistencia. ' + catchError, 300, 150, 'Error Solicitud Alta');
    }

    function ReLoadPage() {
        window.location.reload();

    }

    function AvisoOk() {
        radalert("La solicitud se ha enviado correcteamente.", 300, 150, "Solicitud Correcta");
        setTimeout("ReLoadPage()", 4000);
    }

    function CheckIfShow(sender, args) {
        var summaryElem = document.getElementById("ValidationSummary1");

        //check if summary is visible
        if (summaryElem.style.display == "none") {
            //API: if there are no errors, do not show the tooltip
            args.set_cancel(true);
        }
    }

    function HideToolTip() {

        $('#<%=btnAlta.ClientID%>').removeAttr("disabled");

    }

    function ShowTooltip() {

        window.setTimeout(function () {
            var tooltip = $find("RadToolTip1");
            //API: show the tooltip
            tooltip.show();
            $('#<%=btnAlta.ClientID%>').attr("disabled", "disabled");
        }, 10);
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Metodos para la gesti&oacute;n de archivo fotocopia documento
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    $(window).load(
            function () {
                $("#<%=FileUpload2.ClientID %>").fileUpload({
                    'uploader': 'UploadFiles/scripts/uploader.swf',
                    'cancelImg': 'UploadFiles/images/cancel.png',
                    'buttonImg': 'Imagenes/adjuntar.png',
                    'wmode': 'transparent',
                    'buttonText': 'Adjuntar',
                    'script': 'UploadFiles/Upload.ashx',
                    'folder': 'ImagenesDni',
                    'fileDesc': 'Archivos Imagen',
                    'fileExt': '*.jpg;*.png',
                    'multi': false,
                    'width': '45',
                    'auto': true,
                    'sizeLimit': 1024 * 1024 * 1.5,
                    'onComplete': TerminoUpload
                });
            }
        );


    function TerminoUpload(sender, arg, infoArchivo, DatosArchivo, aa) {

        if (DatosArchivo.split('|').length > 1) {
            $find("<% =txtArchivoDNI.ClientID %>").set_value(infoArchivo.name);
        }
    }

    function CambiarEtiqueta() {
        valueSelected = $find("<%= cboTipoPresentador.ClientID %>").get_value();
        
        if (valueSelected == "Referente") {
            $("#<%= lblTipoReferente.ClientID %>").text("Referente:");

            $find("<%= cboConsultores.ClientID %>").set_emptyMessage("Seleccione un Referente")
            $find("<%= cboConsultores.ClientID %>").clearSelection();

        }
        else {
            $("#<%= lblTipoReferente.ClientID %>").text("Patrocinador:");


            $find("<%= cboConsultores.ClientID %>").set_emptyMessage("Seleccione un Patrocinador")
            $find("<%= cboConsultores.ClientID %>").clearSelection();
        }
    }

    function ValidarControlIB() {
        var validator = document.getElementById("<%=RFV_IB.ClientID %>");
        valueSelected = $find("<%= cboIB.ClientID %>").get_value();
        if (valueSelected != "3") {
            ValidatorEnable(validator, true);
        }
        else {
            ValidatorEnable(validator, false);
        }
    }

    function ValidarControlesConyuge() {
        var validator = document.getElementById("<%=RFV_NombresConyuge.ClientID %>");
        var validator1 = document.getElementById("<%=RFV_ApellidoConyuge.ClientID %>");

        valueSelected = $find("<%= cboEstadoCivil.ClientID %>").get_value();
        if (valueSelected != "Casado/a") {
            ValidatorEnable(validator, false);
            ValidatorEnable(validator1, false);
        }
        else {
            ValidatorEnable(validator, true);
            ValidatorEnable(validator1, true);
        }
    }

</script>
<body style="background-image: url(Imagenes/repetido.jpg); margin-top: 1px; background-repeat: repeat-x;
    background-color: White;">
    <form id="form1" runat="server" defaultbutton="btnOculto">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:Button ID="btnOculto" Style="display: none" runat="server" OnClientClick="return false;" />
    <telerik:RadWindowManager ID="RadWindowManager1" ShowContentDuringLoad="true" VisibleStatusbar="false"
        ReloadOnShow="false" runat="server" Skin="WebBlue">
        <Windows>
            <telerik:RadWindow runat="server" ID="RadWindow1" Behaviors="Close" Width="500" Height="290px"
                ReloadOnShow="true" Modal="true" OnClientClose="OnClientClose" NavigateUrl="BusquedaLider.aspx">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <table cellpadding="0" cellspacing="0" border="0" style="height: 100%; width: 100%"
        id="tblPrincipal" runat="server">
        <tr>
            <td>
                <div class="Header_panelContainerSimple">
                    <div class="CabeceraInicial">
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td align="center">
                <div class="PageContent">
                    <table class="style1" border="0">
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="upDatos" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <telerik:RadToolTip runat="server" ID="ToolTipDireccion" Skin="Sunset" Sticky="true"
                                            ManualClose="true" Width="350px" Animation="Fade" Position="Center" VisibleOnPageLoad="true"
                                            RelativeTo="BrowserWindow" ShowEvent="FromCode" Modal="true" Title="Notificaci&oacute;n de Direcci&oacute;n"
                                            Text="Usted no tiene asigando un asistente por lo que no podr&aacute; solicitar el alta, por favor comunique dicha situaci&oacute;n al personal administrativo.">
                                        </telerik:RadToolTip>
                                        <table border="0" style="width: 100%">
                                            <tr>
                                                <td class="style2" colspan="4" style="border-bottom: 1px solid #0066CC">
                                                    <asp:Label ID="label13" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 15px" Font-Bold="True" Font-Names="Tahoma">Datos Personales</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="label1" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">Apellido:</asp:Label><br />
                                                    <asp:Label ID="label38" runat="server" Style="width: 70px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">Nombres:</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <telerik:RadTextBox ID="txtApellido" runat="server" EmptyMessage="Ingrese Apellido"
                                                        InvalidStyleDuration="100" MaxLength="15" Skin="WebBlue" Width="280px" Style="text-transform: uppercase;">
                                                    </telerik:RadTextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtApellido"
                                                        Text="*" ErrorMessage="Debe Ingresar el Apellido del revendedor"></asp:RequiredFieldValidator>
                                                    <telerik:RadTextBox ID="txtNombres" runat="server" EmptyMessage="Ingrese Nombres"
                                                        InvalidStyleDuration="100" MaxLength="24" Skin="WebBlue" Width="280px" Style="text-transform: uppercase;">
                                                    </telerik:RadTextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="txtNombres"
                                                        Text="*" ErrorMessage="Debe Ingresar el/los Nombre/s del revendedor"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="label2" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">Fecha Nacimiento</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <telerik:RadDatePicker ID="DpFechaNacimiento" runat="server" Culture="Spanish (Argentina)"
                                                        Skin="Web20" Width="138px" MinDate="01/01/1940">
                                                        <DateInput ID="DateInput1" runat="server" InvalidStyleDuration="100" LabelCssClass="radLabelCss_Web20"
                                                            SelectionOnFocus="SelectAll" ShowButton="True" Skin="WebBlue">
                                                        </DateInput>
                                                        <Calendar ID="Calendar1" runat="server" Skin="WebBlue">
                                                        </Calendar>
                                                    </telerik:RadDatePicker>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DpFechaNacimiento"
                                                        Text="*" ErrorMessage="Debe Ingresar la fecha de Nacimiento"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="label9" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">DNI:</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <telerik:RadMaskedTextBox ID="txtDni" runat="server" DisplayMask="##.###.###" EmptyMessage="Ingrese su n&uacute;mero de documento "
                                                        InvalidStyleDuration="100" Mask="########" Skin="WebBlue" Width="256px" onblur="CalcularCUIT();">
                                                    </telerik:RadMaskedTextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtDni"
                                                        Text="*" ErrorMessage="Debe Ingresar el DNI"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="label7" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">Telefono Fijo</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <telerik:RadMaskedTextBox ID="txtTelFijo" runat="server" EmptyMessage="Ingrese su tel. con el c&oacute;digo de &aacute;rea "
                                                        InvalidStyleDuration="100" Mask="(####) - #######" Skin="WebBlue" Width="256px">
                                                    </telerik:RadMaskedTextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="txtTelFijo"
                                                        Text="*" ErrorMessage="Debe Ingresar el tel&eacute;fono fijo"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="label8" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">Tel&eacute;fono Celular:</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <telerik:RadMaskedTextBox ID="txtTelCel" runat="server" EmptyMessage="Ingrese su tel. con el c&oacute;digo de &aacute;rea "
                                                        InvalidStyleDuration="100" Mask="(####) - 15#######" Skin="WebBlue" Width="256px">
                                                    </telerik:RadMaskedTextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtTelCel"
                                                        Text="*" ErrorMessage="Debe Ingresar un tel&eacute;fono celular"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="label4" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">E-mail:</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <telerik:RadTextBox ID="txtEmail" runat="server" EmptyMessage="Ingrese su correo electr&oacute;nico"
                                                        InvalidStyleDuration="100" MaxLength="50" Skin="WebBlue" Width="256px">
                                                    </telerik:RadTextBox>
                                                    <asp:RequiredFieldValidator Enabled="false" ID="RequiredFieldValidator7" runat="server"
                                                        ControlToValidate="txtEmail" Text="*" ErrorMessage="Debe Ingresar un correo electr&oacute;nico"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="regEmail" runat="server" ControlToValidate="txtEmail"
                                                        Display="Dynamic" font-name="Arial" Font-Size="11" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="label22" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">Nacionalidad:</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <telerik:RadTextBox ID="txtNacionalidad" runat="server" EmptyMessage="Ingrese su nacionalidad"
                                                        InvalidStyleDuration="100" MaxLength="50" Skin="WebBlue" Width="256px">
                                                    </telerik:RadTextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtNacionalidad"
                                                        Text="*" ErrorMessage="Debe Ingresar la nacionalidad"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="label23" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">Sexo:</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <telerik:RadComboBox ID="cboSexo" runat="server" Skin="WebBlue" Width="156px" OnClientSelectedIndexChanged="CalcularCUIT">
                                                        <Items>
                                                            <telerik:RadComboBoxItem Text="Femenino" Value="Femenino" Selected="true" />
                                                            <telerik:RadComboBoxItem Text="Masculino" Value="Masculino" />
                                                        </Items>
                                                        <CollapseAnimation Duration="200" Type="OutQuint" />
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="label24" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">Estado Civil:</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <telerik:RadComboBox ID="cboEstadoCivil" runat="server" Skin="WebBlue" Width="156px" OnClientSelectedIndexChanged="ValidarControlesConyuge">
                                                        <Items>
                                                            <telerik:RadComboBoxItem Text="Casado/a" Value="Casado/a" Selected="true" />
                                                            <telerik:RadComboBoxItem Text="Soltero/a" Value="Soltero/a" />
                                                            <telerik:RadComboBoxItem Text="Divorciado/a" Value="Divorciado/a" />
                                                            <telerik:RadComboBoxItem Text="Viudo/a" Value="Viudo/a" />
                                                        </Items>
                                                        <CollapseAnimation Duration="200" Type="OutQuint" />
                                                    </telerik:RadComboBox>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="label36" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">Tipo Persona:</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <telerik:RadComboBox ID="cboTipoPersona" runat="server" Skin="WebBlue" Width="156px"
                                                        OnClientSelectedIndexChanged="CalcularCUIT">
                                                        <Items>
                                                            <telerik:RadComboBoxItem Text="Persona F&iacute;sica" Value="PF" Selected="true" />
                                                            <telerik:RadComboBoxItem Text="Persona Jur&iacute;dica" Value="PJ" />
                                                        </Items>
                                                        <CollapseAnimation Duration="200" Type="OutQuint" />
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="label25" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">Profesi&oacute;n:</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <telerik:RadTextBox ID="txtPrefesion" runat="server" EmptyMessage="Ingrese su profesi&oacute;n"
                                                        InvalidStyleDuration="100" MaxLength="40" Skin="WebBlue" Width="256px">
                                                    </telerik:RadTextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtPrefesion"
                                                        Text="*" ErrorMessage="Debe Ingresar la profesi&oacute;n"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="label39" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">Fotocopia Dni:</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td align="left" style="width: 170px">
                                                                <telerik:RadTextBox ID="txtArchivoDNI" runat="server" ReadOnly="true" InvalidStyleDuration="100"
                                                                    MaxLength="100" Skin="WebBlue" Width="90%">
                                                                </telerik:RadTextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="txtArchivoDNI"
                                                                    Text="*" ErrorMessage="Debe Ingresar la imagen del documento"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td align="left">
                                                                <asp:FileUpload ID="FileUpload2" runat="server" Style="display: none" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="label11" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">Apellido Conyuge:</asp:Label><br />
                                                </td>
                                                <td align="left">
                                                    <telerik:RadTextBox ID="txtApellidoConyuge" runat="server" EmptyMessage="Ingrese Apellido"
                                                        InvalidStyleDuration="100" MaxLength="30" Skin="WebBlue" Width="90%" Style="text-transform: uppercase;">
                                                    </telerik:RadTextBox>
                                                    <asp:RequiredFieldValidator ID="RFV_ApellidoConyuge" runat="server" ControlToValidate="txtApellidoConyuge"
                                                        Text="*" ErrorMessage="Debe Ingresar el Apellido del Conyuge"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="label40" runat="server" Style="width: 70px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">Nombres Conyuge:</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <telerik:RadTextBox ID="txtNombresConyuge" runat="server" EmptyMessage="Ingrese Nombres"
                                                        InvalidStyleDuration="100" MaxLength="30" Skin="WebBlue" Width="90%" Style="text-transform: uppercase;">
                                                    </telerik:RadTextBox>
                                                    <asp:RequiredFieldValidator ID="RFV_NombresConyuge" runat="server" ControlToValidate="txtNombresConyuge"
                                                        Text="*" ErrorMessage="Debe Ingresar el/los Nombre/s del Conyuge"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="4" style="border-bottom: 1px solid #0066CC;padding-top:15px">
                                                    <asp:Label ID="label14" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 15px" Font-Bold="True" Font-Names="Tahoma">Direcci&oacute;n Principal de Entrega</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="label3" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">Provincia:</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:LinqDataSource ID="LinqDataSource2" runat="server" ContextTypeName="CommonMarzzan.Marzzan_InfolegacyDataContext"
                                                        TableName="Clasificaciones" Where="Tipo == @Tipo" OrderBy="Descripcion">
                                                        <WhereParameters>
                                                            <asp:Parameter DefaultValue="Provincias" Name="Tipo" Type="String" />
                                                        </WhereParameters>
                                                    </asp:LinqDataSource>
                                                    <telerik:RadComboBox ID="cboProvincias" runat="server" DataSourceID="LinqDataSource2"
                                                        DataTextField="Descripcion" DataValueField="IdClasificacion" EmptyMessage="Seleccione una Provincia"
                                                        Skin="WebBlue" Width="256px" OnClientSelectedIndexChanged="CargarDepartamentos">
                                                        <CollapseAnimation Duration="200" Type="OutQuint" />
                                                    </telerik:RadComboBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="cboProvincias"
                                                        Text="*" ErrorMessage="Debe Ingresar la provincia de residencia"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblDepartamento" runat="server" Style="width: 100px; color: #0066CC;
                                                        font-family: Sans-Serif; font-size: 11px">Departamento/Localidad:</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox Width="256px" ID="txtdepartamento" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtdepartamento"
                                                        Text="*" ErrorMessage="Debe Ingresar el departamento"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="label5" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">Nuevo Dpto/Loc:</asp:Label>
                                                </td>
                                                <td align="left" colspan="3">
                                                    <asp:TextBox Width="325px" Enabled="false" ID="txtNuevo_Depart_loc" Style="background-color: #CCCCCC"
                                                        runat="server" MaxLength="25"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtNuevo_Depart_loc"
                                                        Text="*" ErrorMessage="Debe Ingresar la localidad"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="label6" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">Calle y N&uacute;mero:</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <telerik:RadTextBox ID="txtDireccion" runat="server" EmptyMessage="Ingrese su calle y numero "
                                                        InvalidStyleDuration="100" MaxLength="30" Skin="WebBlue" Width="256px">
                                                    </telerik:RadTextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtDireccion"
                                                        Text="*" ErrorMessage="Debe Ingresar la direcci&oacute;n"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="style2">
                                                    <asp:Label ID="label37" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">C&oacute;digo Postal:</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox Width="256px" ID="txtCodigoPostal" runat="server" MaxLength="5"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="txtCodigoPostal"
                                                        Text="*" ErrorMessage="Debe Ingresar el c&oacute;digo postal de la direccion principal"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="label26" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">Entre Calle:</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <telerik:RadTextBox ID="txtCalleEntre" runat="server" EmptyMessage="Ingrese el nombre de la calle"
                                                        InvalidStyleDuration="100" MaxLength="100" Skin="WebBlue" Width="256px">
                                                    </telerik:RadTextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtCalleEntre"
                                                        Text="*" ErrorMessage="Debe Ingresar entre que calle se encuentra"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="label27" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">y calle:</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <telerik:RadTextBox ID="txtCalleFinal" runat="server" EmptyMessage="Ingrese el nombre de la calle"
                                                        InvalidStyleDuration="100" MaxLength="100" Skin="WebBlue" Width="256px">
                                                    </telerik:RadTextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtCalleFinal"
                                                        Text="*" ErrorMessage="Debe Ingresar entre que calle se encuentra"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="4" style="border-bottom: 1px solid #0066CC;padding-top:15px">
                                                    <asp:Label ID="label16" runat="server" Font-Bold="True" Font-Names="Tahoma" Style="width: 100px;
                                                        color: #0066CC; font-family: Sans-Serif; font-size: 15px">Direcciones Alternativas de Entrega</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="label21" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">Lista Direcciones:</asp:Label>
                                                </td>
                                                <td colspan="3" align="left">
                                                    <asp:UpdatePanel ID="upDirecciones" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <telerik:RadGrid ID="gvDirEntrega" runat="server" Skin="Vista" AutoGenerateColumns="False"
                                                                Height="250px" Width="90%" AllowAutomaticInserts="True" AllowAutomaticUpdates="True"
                                                                GridLines="None">
                                                                <MasterTableView EditMode="PopUp" TableLayout="Fixed" CommandItemDisplay="Bottom"
                                                                    DataKeyNames="IdDireccion" ClientDataKeyNames="IdDireccion" NoDetailRecordsText="No posee direcciones de entrega"
                                                                    NoMasterRecordsText="">
                                                                    <CommandItemTemplate>
                                                                        <div style="padding: 5px 5px;">
                                                                            <asp:LinkButton ID="LinkButton2" runat="server" OnClientClick="ShowNuevaDireccion(); return false;"
                                                                                CommandName="InitInsert" CausesValidation="true">
                                                                                  <img style="padding-right:5px;border:0px;vertical-align:middle;" alt="sssa" src="Imagenes/AddRecord.gif" />Agregar Direcci&oacute;n
                                                                            </asp:LinkButton>
                                                                        </div>
                                                                    </CommandItemTemplate>
                                                                    <Columns>
                                                                        <telerik:GridTemplateColumn HeaderText="Direcciones de Entrega">
                                                                            <ItemTemplate>
                                                                                <table width="100%" border="1" cellpadding="0" cellspacing="0">
                                                                                    <tr>
                                                                                        <td style="width: 100px">
                                                                                            <asp:Label ID="label8" runat="server" SkinID="lblBlue">Provincia:</asp:Label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblProvincia" runat="server" SkinID="lblBlack"><%# Eval("Provincia")%></asp:Label>
                                                                                        </td>
                                                                                        <td style="width: 100px">
                                                                                            <asp:Label ID="label19" runat="server" SkinID="lblBlue">Dpto/loc:</asp:Label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="label20" runat="server" SkinID="lblBlack"><%# Eval("Departamento")%></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 100px">
                                                                                            <asp:Label ID="label13" runat="server" SkinID="lblBlue">Nuevo Dpto/Loc:</asp:Label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="label14" runat="server" SkinID="lblBlack"><%# Eval("Localidad")%></asp:Label>
                                                                                        </td>
                                                                                        <td style="width: 100px">
                                                                                            <asp:Label ID="label15" runat="server" SkinID="lblBlue">Calle:</asp:Label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="label16" runat="server" SkinID="lblBlack"><%# Eval("Calle")%></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 100px">
                                                                                            <asp:Label ID="label17" runat="server" SkinID="lblBlue">Codigo Posta:</asp:Label>
                                                                                        </td>
                                                                                        <td align="left" colspan="3">
                                                                                            <asp:Label ID="label18" runat="server" SkinID="lblBlack"><%# Eval("CodigoPostal")%></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                    </Columns>
                                                                    <EditFormSettings ColumnNumber="1" CaptionFormatString="Edici&oacute;n">
                                                                        <FormTableItemStyle Wrap="False"></FormTableItemStyle>
                                                                        <FormMainTableStyle GridLines="Both" CellSpacing="0" CellPadding="3" BackColor="White"
                                                                            Width="100%" />
                                                                        <FormTableStyle CellSpacing="0" CellPadding="2" Width="100%" Height="110px" BackColor="White" />
                                                                        <FormStyle Width="100%" BackColor="#eef2ea"></FormStyle>
                                                                        <FormTableAlternatingItemStyle Wrap="False"></FormTableAlternatingItemStyle>
                                                                        <EditColumn ButtonType="ImageButton" InsertText="Insertar" UpdateText="Actualizar"
                                                                            UniqueName="EditCommandColumn1" CancelText="Cancelar">
                                                                        </EditColumn>
                                                                        <FormTableButtonRowStyle HorizontalAlign="Right"></FormTableButtonRowStyle>
                                                                        <PopUpSettings ScrollBars="Auto" Modal="true" Width="45%" />
                                                                    </EditFormSettings>
                                                                    <RowIndicatorColumn>
                                                                        <HeaderStyle Width="20px"></HeaderStyle>
                                                                    </RowIndicatorColumn>
                                                                    <ExpandCollapseColumn>
                                                                        <HeaderStyle Width="20px"></HeaderStyle>
                                                                    </ExpandCollapseColumn>
                                                                </MasterTableView>
                                                                <ValidationSettings CommandsToValidate="PerformInsert,Update" />
                                                                <ClientSettings>
                                                                    <Selecting AllowRowSelect="true" />
                                                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                                                                </ClientSettings>
                                                            </telerik:RadGrid>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2" colspan="4" style="border-bottom: 1px solid #0066CC;padding-top:15px">
                                                    <asp:Label ID="label15" runat="server" Font-Bold="True" Font-Names="Tahoma" Style="width: 100px;
                                                        color: #0066CC; font-family: Sans-Serif; font-size: 15px">Contacto Entregas</asp:Label>
                                                </td>
                                            
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="label28" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">L&iacute;der:</asp:Label>
                                                </td>
                                                <td align="left" colspan="3">
                                                    <asp:Label ID="lblCoordinadora" runat="server" Style="width: 100px; color: black;
                                                        font-family: Sans-Serif; font-size: 11px; text-transform: capitalize" Text="Cortes Carolina Andrea"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="label10" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">Como Nos Comocio?</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:LinqDataSource ID="LinqDataSource1" runat="server" ContextTypeName="CommonMarzzan.Marzzan_InfolegacyDataContext"
                                                        Select="new (IdClasificacion, Descripcion)" TableName="Clasificaciones" Where="Tipo == @Tipo">
                                                        <WhereParameters>
                                                            <asp:Parameter DefaultValue="ComoConocio" Name="Tipo" Type="String" />
                                                        </WhereParameters>
                                                    </asp:LinqDataSource>
                                                    <telerik:RadComboBox ID="cboComo" runat="server" DataSourceID="LinqDataSource1" DataTextField="Descripcion"
                                                        DataValueField="IdClasificacion" EmptyMessage="Seleccione una forma" Skin="WebBlue"
                                                        Width="256px">
                                                        <CollapseAnimation Duration="200" Type="OutQuint" />
                                                    </telerik:RadComboBox>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="label30" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">Tipo Presentador:</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <telerik:RadComboBox ID="cboTipoPresentador" runat="server" EmptyMessage="Seleccione Tipo Presentador"
                                                        Skin="WebBlue" Width="256px" OnClientSelectedIndexChanged="CambiarEtiqueta">
                                                        <Items>
                                                            <telerik:RadComboBoxItem runat="server" Text="Referente" Value="Referente" Selected="true" />
                                                            <telerik:RadComboBoxItem runat="server" Text="Patrocinador" Value="Patrocinador" />
                                                        </Items>
                                                        <CollapseAnimation Duration="200" Type="OutQuint" />
                                                    </telerik:RadComboBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="cboTipoPresentador"
                                                        Text="*" ErrorMessage="Debe Ingresar el tipo de incorporaci&oacute;n"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                
                                                <td class="style2">
                                                    <asp:Label ID="label3544" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">Grupo Alta:</asp:Label>
                                                </td>
                                                <td align="left" >
                                                    <telerik:RadComboBox ID="cboGrupos" runat="server" EmptyMessage="Seleccione el grupo para el alta de la solicitud"
                                                        Skin="WebBlue" Width="356px">
                                                        <CollapseAnimation Duration="200" Type="OutQuint" />
                                                    </telerik:RadComboBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="cboGrupos"
                                                        Text="*" ErrorMessage="Debe Ingresar el grupo para el alta"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblTipoReferente" runat="server" Style="width: 100px; color: #0066CC;
                                                        font-family: Sans-Serif; font-size: 11px">Referente:</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <telerik:RadComboBox Skin="WebBlue" ID="cboConsultores" runat="server" Width="256px"
                                                        AllowCustomText="true" MarkFirstMatch="true" AutoPostBack="false" EmptyMessage="Seleccione un referente" >
                                                        <CollapseAnimation Duration="200" Type="OutQuint" />
                                                    </telerik:RadComboBox>
                                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="cboConsultores"
                                                            Text="*" ErrorMessage="Debe Ingresar el referente/patrocinado"></asp:RequiredFieldValidator>

                                                    <div style="display: none">
                                                        <telerik:RadTextBox ID="txtPresento" runat="server" InvalidStyleDuration="100" MaxLength="40"
                                                            Skin="WebBlue" Width="256px">
                                                        </telerik:RadTextBox>
                                                    </div>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="label12" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">Comentario:</asp:Label>
                                                </td>
                                                <td align="left" colspan="3">
                                                    <telerik:RadTextBox ID="txtComentario" runat="server" EmptyMessage="Deje su pregunta o comentario "
                                                        InvalidStyleDuration="100" MaxLength="255" Rows="6" Skin="WebBlue" TextMode="MultiLine"
                                                        Width="92%">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="4" style="border-bottom: 1px solid #0066CC;padding-top:15px">
                                                    <asp:Label ID="label31" runat="server" Font-Bold="True" Font-Names="Tahoma" Style="color: #0066CC; font-family: Sans-Serif; font-size: 15px">Datos Complementarios</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <asp:Label ID="label32" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">Condici&oacute;n IVA:</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <telerik:RadComboBox ID="cboCondicionIVA" runat="server" Skin="WebBlue" Width="205px">
                                                        <Items>
                                                            <telerik:RadComboBoxItem Text="Responsable Inscripto" Value="Inscripto" />
                                                            <telerik:RadComboBoxItem Text="Monotributista" Value="Monotributista" />
                                                            <telerik:RadComboBoxItem Text="Consumidor Final" Value="ConsumidorFinal" Selected="true" />
                                                        </Items>
                                                        <CollapseAnimation Duration="200" Type="OutQuint" />
                                                    </telerik:RadComboBox>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="label33" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">CUIT/CUIL:</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <telerik:RadTextBox ReadOnly="true" ID="txtCuit" runat="server" EmptyMessage="C&aacute;lculo Autom&aacute;tico..."
                                                        InvalidStyleDuration="100" MaxLength="100" Skin="WebBlue" Width="256px">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>

                                             <tr>
                                                <td class="style2">
                                                    <asp:Label ID="label41" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">Condici&oacute;n Ingresos Brutos:</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <telerik:RadComboBox ID="cboIB" runat="server" Skin="WebBlue" Width="205px" OnClientSelectedIndexChanged="ValidarControlIB" >
                                                        <Items>
                                                            <telerik:RadComboBoxItem Text="Contribuyente Local" Value="1" />
                                                            <telerik:RadComboBoxItem Text="Convenio Multilateral" Value="2" />
                                                            <telerik:RadComboBoxItem Text="No Inscripto" Value="3" Selected="true" />
                                                            <telerik:RadComboBoxItem Text="Excento" Value="5" />
                                                        </Items>
                                                        <CollapseAnimation Duration="200" Type="OutQuint" />
                                                    </telerik:RadComboBox>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="label42" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                                        font-size: 11px">Nro Ingresos Brutos:</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <telerik:RadTextBox ID="txtNroIb" runat="server"
                                                        InvalidStyleDuration="100" MaxLength="14" Skin="WebBlue" Width="256px">
                                                    </telerik:RadTextBox>
                                                     <asp:RequiredFieldValidator ID="RFV_IB" runat="server" ControlToValidate="txtNroIb" Enabled="false"
                                                        Text="*" ErrorMessage="Debe Ingresar el nro de IB"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="middle" align="center" style="height: 22px;">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 50%" align="center">
                                            <asp:UpdatePanel ID="upAlta" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnAlta" runat="server" SkinID="btnBasic" Text="Solicitar Alta" OnClientClick="ShowTooltip()"
                                                        OnClick="btnAlta_Click" />
                                                    <telerik:RadToolTip runat="server" ID="RadToolTip1" Position="Center" HideEvent="ManualClose"
                                                        ShowEvent="FromCode" Width="400px" RelativeTo="BrowserWindow" OnClientBeforeShow="CheckIfShow"
                                                        Skin="Sunset" OnClientHide="HideToolTip" TargetControlID="btnAlta">
                                                        <table border="0">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:Label ID="label34" runat="server" Style="width: 100%; color: black; font-family: Sans-Serif;
                                                                        font-size: 13px">Los datos marcados con * son obligatorios o tiene un formato incorecto</asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </telerik:RadToolTip>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
    </telerik:RadAjaxManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="50">
        <ProgressTemplate>
            <div id="divBloq1" class="progressBackgroundFilterBlack">
            </div>
            <div class="processMessage">
                <table border="0" cellpadding="0" cellspacing="0" style="height: 62px;">
                    <tr>
                        <td align="center">
                            <img alt="a" src="Imagenes/waiting.gif" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lbltitulopaciente1" runat="server" Font-Bold="True" Font-Names="Thomas"
                                Font-Size="12px" ForeColor="Black" Height="21px" Style="vertical-align: middle"
                                Text="Realizando Solicitud....">
                            </asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    </form>
</body>
</html>
