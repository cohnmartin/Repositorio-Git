<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true"
    CodeFile="GestionFormulasPlanTrabajo.aspx.cs" Inherits="GestionFormulasPlanTrabajo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="Scripts/Jquery-UI/css/start/jquery-ui-1.10.3.custom.min.css" rel="stylesheet"
        type="text/css" />
    <script src="Scripts/Jquery-UI/js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="Scripts/Jquery-UI/js/jquery-ui-1.10.3.custom.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.tmpl.1.1.1.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {

            PageMethods.GetDatos(callBackFunction, function () { });


            $(".txtEntrada").keypress(function (event) {
                // Backspace, tab, enter, end, home, left, right
                // We don't support the del key in Opera because del == . == 46.
                var controlKeys = [8, 9, 13, 35, 36, 37, 39];
                // IE doesn't support indexOf
                var isControlKey = controlKeys.join(",").match(new RegExp(event.which));
                // Some browsers just don't raise events for control keys. Easy.
                // e.g. Safari backspace.
                if (!event.which || // Control keys in most browsers. e.g. Firefox tab is 0
              (48 <= event.which && event.which <= 57)  // Always 1 through 9
              ||
              isControlKey) { // Opera assigns values for control keys.

                    if (event.which != 13)
                       return
                    else {
                        event.preventDefault();
                    }

                } else {
                    event.preventDefault();
                }
            });


            $(".txtEntrada").keyup(function (event) {
               
                        RealizarCalculos();
              });

        });

        function RealizarCalculos() {
            var A = parseInt($("#txtUOCRA").val());
            var B = parseInt($("#txtOTROS").val());
            var C = parseInt($("#txtINDEP").val());
            var D = parseInt($("#txtUOCRA").val()) + parseInt($("#txtOTROS").val()) + parseInt($("#txtINDEP").val());
            $("#lblTotal").text(D);

            var TotalMinutos = 0;
            $(".numericSimple").each(function () {

                if ($(this).val() != "") {
                    var resultado = eval($(this).val());
                    if (resultado != NaN) {
                        TotalMinutos += resultado;

                        var resultadoRedondeado = parseFloat(resultado).toFixed(1);
                        $(this).parents("tr:first").find("[id*=Calculo]").text(resultadoRedondeado);
                        
                    }
                }

            });

            $("#lblTotalMin").text(TotalMinutos);
            $("#lblTotalHs").text(parseFloat(TotalMinutos/60).toFixed(2));
            
        }

        function callBackFunction(datos) {


            var t = [{ "header": "Nro" },
                    { "header": "Items de la Hoja Ruta" },
                    { "header": "Formula" },
                    { "header": "Calculo"}];

            var templates = {
                th: '<th style="background-color:#CC3300;color:white" >#{header}</th>',
                td: '<tr>' +
                '<td align="center" style="width:40px" >#{Codigo}</td>' +
                '<td align="left" style="width:480px">#{Descripcion}</td>' +
                '<td  align="left" style="padding-left:5px" ><input type="text" class="numericSimple" style="width:90%" value="#{FornulaPlanTrabajo}" /></td>' +
                '<td  id="Calculo" style="width:60px;display:none">&nbsp;</td>' +
                '</tr>'

            };

            var table = '<table width="100%" id="tbl" border="1" cellpadding="0" cellspacing="0" style="font-size:14px; background-color: Transparent;"><thead>' +
            '<tr>';

            /// Genero la fila de encabezado
            $.each(t, function (key, val) {
                var td = $.tmpl(templates.th, val);

                if (val.header == "Calculo")
                    td = td.replace(" style=\"", " id='hCalculo' style=\"display:none;");

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

        function ShowSimulacion() {
            $("#tdSimulacion").toggle(300, function () {
                if ($("#tdSimulacion").css("display") == "none") {
                    $("#btnSimular").text("[ + ] Simular");
                    $("#tbl [id*=Calculo]").css("display", "none");

                }
                else {
                    $("#btnSimular").text("[ - ] Simular");
                    $("#tbl [id*=Calculo]").css("display", "block");
                }
                                
            });

            $("#tdSimulacionTotales").toggle(300);
        }
    </script>
    <style type="text/css">
        .rowAlt
        {
            background-color: #f1e9dc;
        }
        .rowSpl
        {
            background-color: White;
        }
    </style>
    <table cellpadding="0" cellspacing="5" style="width: 80%">
        <tr>
            <td align="center" style="height: 25px; background: url('images/sprite.gif') 0  -997px repeat-x">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="14pt" Font-Names="Sans-Serif"
                    ForeColor="#E0D6BE" Text="Gestión Plan de Trabajo" Width="468px"></asp:Label>
            </td>
        </tr>
    </table>
    <table cellpadding="0" cellspacing="2" style="width: 100%; font-size: 13px">
        <tr>
            <td>
                <div id="divtbl">
                </div>
            </td>
        </tr>
         <tr>
            <td id="tdSimulacionTotales" style="display: none">
              <table cellpadding="0" cellspacing="2" style="width: 100%;font-size:15px" border="0">
                    <tr>
                        <td align="right" style="width:94%;padding-right:5px">
                            TIEMPO TOTAL AUDITORIA (Hs.):
                        </td>
                        <td align="center">
                            <span id="lblTotalHs">&nbsp;</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width:94%;padding-right:5px">
                            TIEMPO TOTAL AUDITORIA (Min.):
                        </td>
                        <td align="center">
                            <span id="lblTotalMin">&nbsp;</span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
       
        <tr>
            <td id="tdSimulacion" style="display: none">
                <table cellpadding="0" cellspacing="2" style="width: 100%;background-color:#f1e9dc; color:Black" border="1">
                    <tr>
                        <td align="center">
                            &nbsp;
                        </td>
                        <td align="center" colspan="2">
                            Dependientes
                        </td>
                        <td align="center">
                            Independientes
                        </td>
                        <td align="center">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            Gremios
                        </td>
                        <td align="center">
                            UOCRA (A)
                        </td>
                        <td align="center">
                            Otros (B)
                        </td>
                        <td align="center">
                            Monotributo/RI (C)
                        </td>
                        <td align="center">
                            TOTAL (D)
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            Recursos
                        </td>
                        <td align="center">
                            <input class="txtEntrada" id="txtUOCRA" type="text" style="width: 120px" value="0" />
                        </td>
                        <td align="center">
                            <input class="txtEntrada" id="txtOTROS" type="text" style="width: 120px" value="0" />
                        </td>
                        <td align="center">
                            <input class="txtEntrada" id="txtINDEP" type="text" style="width: 120px" value="0" />
                        </td>
                        <td align="center">
                            <span id="lblTotal">&nbsp;</span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

         <tr>
            <td align="right">
                <button id="btnSimular" onclick="ShowSimulacion();return false;">
                    [ + ] Simular</button>
            </td>
        </tr>
    </table>
</asp:Content>
