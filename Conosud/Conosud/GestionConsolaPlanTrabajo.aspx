<%@ Page Title="Consola Plan de Trabajo" Language="C#" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="GestionConsolaPlanTrabajo.aspx.cs" Inherits="GestionConsolaPlanTrabajo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="Scripts/Jquery-UI/css/blitzer/jquery-ui-1.10.3.custom.min.css" rel="stylesheet"
        type="text/css" />
    <script src="Scripts/Jquery-UI/js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="Scripts/Jquery-UI/js/jquery-ui-1.10.3.custom.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.tmpl.1.1.1.js" type="text/javascript"></script>
    <style type="text/css">
        .ui-dialog
        {
            z-index: 9999999999999999;
        }
        .ui-dialog .ui-dialog-title
        {
            text-align: left;
            font-size: medium;
        }
        .ruBrowse
        {
            background-position: 0 -46px !important;
            width: 122px !important;
        }
        .rowAlt
        {
            background-color: #f1e9dc;
        }
        .rowSpl
        {
            background-color: White;
        }
    </style>
    </style>
    <script type="text/javascript">

        function ValidarArchivo() {
            var archivo = $find("<%=RadUpload1.ClientID %>").getFileInputs()[0].value;
            var periodo = $find("<%=cboPeriodos.ClientID %>").get_selectedItem();

            if (archivo == "" || periodo == null) {
                return false;
            }
            else {
                $("#<%=buttonSubmit.ClientID %>").click();
            }
        }

        $(document).ready(function () {

            PageMethods.GetDatos(callBackFunction, function () { });

        });

        function TablaCalculo(datos) {

            var t = [{ "header": "Nro" },
                    { "header": "Items de la Hoja Ruta" },
                    { "header": "Calculo"}];

            var templates = {
                th: '<th style="background-color: #990033;color:white" >#{header}</th>',
                td: '<tr>' +
                '<td align="center" style="width:40px" >#{Codigo}</td>' +
                '<td align="left" >#{Descripcion}</td>' +
                '<td align="center"  style="width:60px" id="Calculo" >&nbsp;</td>' +
                '<td  id="Formula" style="display:none">#{FornulaPlanTrabajo}</td>' +
                '</tr>'

            };

            var table = '<table width="100%" id="tblcalculo" border="1" cellpadding="0" cellspacing="0" style="font-size:14px; background-color: Transparent;"><thead>' +
            '<tr>';

            /// Genero la fila de encabezado
            $.each(t, function (key, val) {
                var td = $.tmpl(templates.th, val);
                table += td;

            });

            table += '</tr></thead><tbody>';

            /// Genero las filas del body
            row = 0;
            for (var i = 0; i < datos.length; i++) {
                table += $.tmpl(templates.td, datos[row]);
                row++;
            }

            table += '</tbody></table>';

            /// Asigno la tabla generada al div para dibujarla
            $("#divtblCalculo")[0].innerHTML = table;
            $("#divtblCalculo tbody tr:odd").addClass("rowAlt");
            $("#divtblCalculo tbody tr:even").addClass("rowSpl");
        }

        function TablaDatosCalculo(datos) {
            var t = [{ "header": "Contrato" },
                    { "header": "Empresa" },
                    { "header": "UOCRA" },
                    { "header": "Otros" },
                    { "header": "Indep" },
                    { "header": "Tiempo (Hs)"}];

            var templates = {
                th: '<th style="background-color: #990033;color:white" >#{header}</th>',
                td: '<tr>' +
                '<td    align="center"  id="Contrato" style="width:100px">#{Contrato}</td>' +
                '<td    align="left"  id="Empresa" style="padding-left:3px">#{Empresa}</td>' +
                '<td    align="center"  id="Uocra" style="width:70px">#{UOCRA}</td>' +
                '<td    align="center"  id="Otros" style="width:70px" >#{OTROS}</td>' +
                '<td    align="center   id="Independientes" style="width:70px">#{INDEPENDIENTES}</td>' +
                '<td align="center"  style="width:100px" id="Calculo" >#{Resultado}</td>' +
                '</tr>'

            };

            var table = '<table width="100%" id="tbl" border="1" cellpadding="0" cellspacing="0" style="font-size:14px; background-color: Transparent;"><thead>' +
            '<tr>';

            /// Genero la fila de encabezado
            $.each(t, function (key, val) {
                var td = $.tmpl(templates.th, val);
                table += td;

            });

            table += '</tr></thead><tbody>';

            /// Genero las filas del body
            row = 0;
            for (var i = 0; i < datos.length; i++) {
                table += $.tmpl(templates.td, datos[row]);
                row++;
            }

            table += '</tbody></table>';

            /// Asigno la tabla generada al div para dibujarla
            $("#divtblCalculoContrato")[0].innerHTML = table;
            $("#divtblCalculoContrato tbody tr:odd").addClass("rowAlt");
            $("#divtblCalculoContrato tbody tr:even").addClass("rowSpl");
        }

        function Exportar(obj) {

            var periodo = $(obj).parents("tr:first").find("[id*=Periodo]").text();
            $("#<%=hdf_Periodo.ClientID %>").val(periodo);
            $("#<%=btnexportar.ClientID %>").click();

        }

        function TablaDatos(datos) {
            var t = [{ "header": "Periodo" },
                    { "header": "UOCRA" },
                    { "header": "Otros" },
                    { "header": "Independientes" },
                    { "header": "" },
                    { "header": ""}];

            var templates = {
                th: '<th style="background-color:#CC3300;color:white" >#{header}</th>',
                td: '<tr>' +
                '<td    align="center"  id="Periodo" >#{Periodo}</td>' +
                '<td    align="center"  id="Uocra" style="width:100px">#{UOCRA}</td>' +
                '<td    align="center"  id="Otros" style="width:100px" >#{OTROS}</td>' +
                '<td    align="center   id="Independientes" style="width:100px">#{INDEPENDIENTES}</td>' +
                '<td    align="center "style="width:25px;cursor:pointer"><img alt="a" title="Realizar Calculo" src="Images/notepad_16x16.gif" onclick="CalcularContrato(this);return false;" /></td>' +
                '<td    align="center "style="width:25px;cursor:pointer"><img alt="a" title="Realizar Calculo" src="Images/Excel_16x16.gif" onclick="Exportar(this);return false;" /></td>' +
                
                '</tr>'

            };

            var table = '<table width="70%" id="tbl" border="1" cellpadding="0" cellspacing="0" style="font-size:14px; background-color: Transparent;"><thead>' +
            '<tr>';

            /// Genero la fila de encabezado
            $.each(t, function (key, val) {
                var td = $.tmpl(templates.th, val);
                table += td;

            });

            table += '</tr></thead><tbody>';

            /// Genero las filas del body
            row = 0;
            for (var i = 0; i < datos.length; i++) {
                table += $.tmpl(templates.td, datos[row]);
                row++;
            }

            table += '</tbody></table>';

            /// Asigno la tabla generada al div para dibujarla
            $("#divtbl")[0].innerHTML = table;
            $("#tbl tbody tr:odd").addClass("rowAlt");
            $("#tbl tbody tr:even").addClass("rowSpl");
        }






        function callBackFunction(datos) {


            TablaCalculo(datos["Formulas"]);
            TablaDatos(datos["Datos"]);


        }

        function CalcularContrato(obj) {
            var periodo = $(obj).parents("tr:first").find("[id*=Periodo]").text();

            PageMethods.GetDatosCalculo(periodo, function (datos) {

                TablaDatosCalculo(datos["DatosCalculo"]);


                $("#dialog-modal-contrato").dialog({
                    autoOpen: true,
                    height: 690,
                    width: 850,
                    modal: true
                });



            }, function () { });


        }



        function Calcular(uocra, otros, independientes) {

            var A = parseInt(uocra);
            var B = parseInt(otros);
            var C = parseInt(independientes);
            var D = A + B + C;

            $("#lblTotal").text(D);

            var TotalMinutos = 0;
            $("#tblcalculo tr").each(function () {
                var formula = $(this).find("[id*=Formula]").text();

                if (formula != "") {
                    var resultado = eval(formula);
                    if (resultado != NaN) {
                        TotalMinutos += resultado;

                        var resultadoRedondeado = parseFloat(resultado).toFixed(1);
                        $(this).find("[id*=Calculo]").text(resultadoRedondeado);

                    }
                }

            });

            $("#lblTotalMin").text(parseFloat(TotalMinutos).toFixed(0));
            $("#lblTotalHs").text(parseFloat(TotalMinutos / 60).toFixed(0));

            $("#dialog-modal").dialog({
                autoOpen: true,
                height: 390,
                width: 850,
                modal: true
            });
        }


       
    </script>
    <telerik:RadWindowManager ID="RadWindowManager1" ShowContentDuringLoad="false" VisibleStatusbar="false"
        ReloadOnShow="false" runat="server" Skin="Sunset">
    </telerik:RadWindowManager>
    <table cellpadding="0" cellspacing="5" style="width: 80%">
        <tr>
            <td align="center" style="height: 25px; background: url('images/sprite.gif') 0  -997px repeat-x">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="14pt" Font-Names="Sans-Serif"
                    ForeColor="#E0D6BE" Text="Consola Plan de Trabajo" Width="468px"></asp:Label>
            </td>
        </tr>
    </table>
    <table style="width: 100%">
        <tr>
            <td colspan="4" align="center">
                <table style="width: 90%" cellpadding="5" style="border-right: #843431 thin solid;
                    border-top: #843431 thin solid; border-left: #843431 thin solid; border-bottom: #843431 thin solid;
                    background-color: #E0D6BE; font-family: Sans-Serif; font-size: 11px;">
                    <tr>
                        <td valign="top" align="left">
                            <asp:Label ID="Label2" SkinID="lblConosud" runat="server" Text="Período Informacion:"></asp:Label>
                        </td>
                        <td valign="top" align="left">
                            <telerik:RadComboBox ID="cboPeriodos" runat="server" Skin="Sunset" Width="200px"
                                OnClientSelectedIndexChanged="ValidarArchivo" AutoPostBack="false" Mensaje="Generando Informe..."
                                MarkFirstMatch="true" AllowCustomText="true" />
                        </td>
                        <td valign="top" align="left" style="display: none">
                            <asp:Button ID="button1" SkinID="btnConosudBasic" runat="server" OnClick="buttonSubmit_Click"
                                Text="Subir Archivo" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left">
                            <asp:Label ID="Lassbel5" SkinID="lblConosud" runat="server" Text="Origen Informacion:"></asp:Label>
                        </td>
                        <td valign="top" align="left">
                            <asp:UpdatePanel runat="server" ID="UpArchivo" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <telerik:RadUpload ID="RadUpload1" runat="server" ControlObjectsVisibility="None"
                                        OverwriteExistingFiles="true" Skin="Sunset" InputSize="55" TargetFolder="~/ArchivosPlanTrabajo"
                                        Width="500px" Height="25px" OnClientFileSelected="ValidarArchivo" Localization-Select="Seleccionar y Subir">
                                    </telerik:RadUpload>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="buttonSubmit" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td valign="top" align="left" style="display: none">
                            <asp:Button ID="buttonSubmit" SkinID="btnConosudBasic" runat="server" OnClick="buttonSubmit_Click"
                                Text="Subir Archivo" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center">
                <telerik:RadProgressManager ID="RadProgressManager1" runat="server" />
                <telerik:RadProgressArea ID="RadProgressArea1" runat="server" Skin="Sunset" ProgressIndicators="RequestSize, FilesCountBar, FilesCount, FilesCountPercent, SelectedFilesCount, CurrentFileName, TimeElapsed, TimeEstimated, TransferSpeed">
                    <Localization Uploaded="Uploaded" CurrentFileName="Subiendo Archivo: " />
                </telerik:RadProgressArea>
            </td>
        </tr>
        <tr>
            <td>
                <div id="divtbl">
                </div>
            </td>
        </tr>
    </table>
    <div id="Div1" style="text-align: left; font-size: 10px">
        <div id="dialog-modal" title="Resultado del Calculo" style="display: none">
            <div id="divtblCalculo">
            </div>
            <table cellpadding="0" cellspacing="2" style="width: 100%; font-size: 15px" border="0">
                <tr>
                    <td align="right" style="width: 94%; padding-right: 5px">
                        TIEMPO TOTAL AUDITORIA (Hs.):
                    </td>
                    <td align="center">
                        <span id="lblTotalHs">&nbsp;</span>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 94%; padding-right: 5px">
                        TIEMPO TOTAL AUDITORIA (Min.):
                    </td>
                    <td align="center">
                        <span id="lblTotalMin">&nbsp;</span>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <asp:Button ID="btnexportar" runat="server" Text="Exportar" OnClick="btnBuscar_Click" style="display:none" />
    <asp:HiddenField ID="hdf_Periodo" value="" runat="server" />

    <div id="Div2" style="text-align: left; font-size: 10px">
        <div id="dialog-modal-contrato" title="Resultado del Calculo por Contrato" style="display: none">
            <div id="divtblCalculoContrato">
            </div>
        </div>
    </div>
</asp:Content>
