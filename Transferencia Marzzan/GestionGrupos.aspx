<%@ Page Language="C#" Theme="SkinMarzzan" AutoEventWireup="true" CodeFile="GestionGrupos.aspx.cs"
    Inherits="GestionGrupos" %>

<%@ Register Assembly="ControlsAjaxNotti" Namespace="ControlsAjaxNotti" TagPrefix="AjaxInfo" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Gestión de Grupos</title>
    <link href="Styles.css" type="text/css" rel="stylesheet" />
    <script src="Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="Scripts/jquery.autocomplete.js" type="text/javascript"></script>
    <script src="Scripts/jquery.tmpl.1.1.1.js" type="text/javascript"></script>
    <script src="Scripts/jquery.tablegroup.js" type="text/javascript"></script>
    <link href="Scripts/Modal-master/jquery.modal.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/Modal-master/jquery.modal.js" type="text/javascript"></script>
    <style type="text/css">
        .footer
        {
            background-color: white;
            color: Black;
            text-align: right;
            padding-right: 10px;
            font-weight: bold;
        }
        .rowAlt
        {
            background-color: #CCF2FF;
            color: Black;
        }
        .rowAlt2
        {
            background-color: #FAEBB8;
            color: Black;
        }
        .rowSpl
        {
            background-color: White;
            color: Black;
        }
        .ContentFiltro
        {
            font: 13px Verdana, Geneva, sans-serif;
            background-color: #F5F5F5;
            border: 3px solid #DBDBDB;
            margin-top: 0px;
            margin-right: 0px;
            padding: 0px;
            width: 70%;
            cursor: pointer;
            font-weight: bold;
            padding: 3px;
        }
        .DivGeneral
        {
            font: 13px Verdana, Geneva, sans-serif;
            background-color: #E0E0E0;
            border: 3px solid #C0C0C0;
            margin-top: 0px;
            margin-right: 0px;
            padding: 0px;
            float: none;
            width: 156px;
            text-align: center;
            cursor: pointer;
            font-weight: bold;
        }
    </style>
</head>
<script type="text/javascript">

    var ctrGrupo;
    var ctrClientes;

    jQuery(function () {

        options = {
            serviceUrl: 'ASHX/LoadGrupos.ashx',
            width: '384',
            minChars: 3,
            showOnFocus: true,
            showInit: false,
            zIndex: 922000000
        };
        ctrGrupo = $('#' + "<%= txtGrupo.ClientID %>").autocomplete(options);

        options = {
            serviceUrl: 'ASHX/LoadClientes.ashx',
            width: '384',
            minChars: 3,
            showOnFocus: true,
            showInit: false,
            zIndex: 922000000
        };
        ctrClientes = $('#' + "<%= txtResponsable.ClientID %>").autocomplete(options);
    });



    function callBackFunction(datos) {

        var t = [
            { "header": "Grupo" },
            { "header": "Responsable" },
            { "header": "Integrante"}];

        var templates = {
            th: '<th class="HeaderVista_Marzzan">#{header}</th>',
            td: '<tr style="cursor:pointer" key="#{idGrupo}" value="#{Nombre}" >' +
                '<td align="left" style="width:30%" filtro="Nombre">#{Nombre}</td>' +
                '<td  align="center" style="width:30%" filtro="Responsable" value="#{IdResponsable}">#{Responsable}</td>' +
                '<td align="left">#{Integrante}</td>' +
                '</tr>'
        };

        var table = '<table width="70%" id="tbl" border="1" cellpadding="0" cellspacing="0" style="font-size:13px; background-color: Transparent;"><thead><tr>';

        /// Genero la fila de encabezado
        $.each(t, function (key, val) {
            table += $.tmpl(templates.th, val);
        });

        table += '</tr></thead><tbody class="BodyDetalle">';

        /// Genero las filas del body
        row = 0;
        var totalRendicion = 0;
        for (var i = 0; i < datos.length; i++) {
            table += $.tmpl(templates.td, datos[row]);
            row++;
        }

        table += '</tbody></table>';

        /// Asigno la tabla generada al div para dibujarla
        $("#divtbl")[0].innerHTML = table;
        $("#tbl tbody tr:odd").addClass("rowAlt");
        $("#tbl tbody tr:even").addClass("rowSpl");

        $('#' + "tbl").tableGroup({ groupColumn: 1, groupClass: 'rgGroupItem', useNumChars: 0, showInit: false, editFunction: "EditGrupo", deleteFunction: "EliminarGrupo" });

    }

    function GrabarGrupo(GrabarIntegrantes, grupoIntegrante) {

        var idResponsable = ctrClientes.get_SelectedValue();
        var Nombre = $("#<%= txtNombre.ClientID %>").val();
        var idGrupo = $("#<%= hdIdGrupo.ClientID %>").val() == "" ? 0 : $("#<%= hdIdGrupo.ClientID %>").val();
        var totalIntegrantes = $("#tblIntegrantes");

        if (idResponsable == "")
            alert("Debe seleccionar el responsable del grupo.");
        else if (Nombre == "")
            alert("Debe ingresar el nombre del grupo.");
        else if ((totalIntegrantes.length == 0 || totalIntegrantes[0].rows.length == 1) && !grupoIntegrante)
            alert("Para poder grabar los datos debe existir al menos un grupo integrante definido.");
        else
            PageMethods.grabarGrupo(idGrupo, Nombre, idResponsable, function (idGrupo) {
                $("#<%= hdIdGrupo.ClientID %>").val(idGrupo);

                if (GrabarIntegrantes) {
                    PageMethods.insertarGrupoIntegrante(idGrupo, grupoIntegrante, callBackFunctionIntegrantes, ErroFunction);
                }

            }, ErroFunction);

    }

    function EliminarGrupoIntegrante(id, idGrupo) {

        PageMethods.eliminarGrupoIntegrante(id, idGrupo, callBackFunctionIntegrantes, ErroFunction);

    }

    function AgregarGrupoIntegrante() {

        var idGrupo = $("#<%= hdIdGrupo.ClientID %>").val();
        var GrupoIntegrante = ctrGrupo.get_SelectedText();

        // Si no exite la cabecera grabada, primero grabo la cabecera y luego los integrantes.
        if (idGrupo == "") {
            GrabarGrupo(true, GrupoIntegrante);
        }
        else {
            PageMethods.insertarGrupoIntegrante(idGrupo, GrupoIntegrante, callBackFunctionIntegrantes, ErroFunction);
        }

    }

    function ActualizarVista() {
        PageMethods.getData(callBackFunction, ErroFunction);

        // Limpio la variable del grupo que se esta editando.
        $("#<%= hdIdGrupo.ClientID %>").val("");
    }

    function NuevoGrupo() {

        // Limpio la variable del grupo que se esta editando.
        $("#<%= hdIdGrupo.ClientID %>").val("");
        $("#Titulo").text("NUEVO GRUPO");
        $("#txtNombre").val("");
        ctrClientes.Clear();

        $('#divGrupo').modal(
        {
            clickClose: false,
            showClose: true,
            zIndex: 999999,
            closeFunction: "ActualizarVista"
        });
    }

    function EliminarGrupo(img) {

        var idGrupo = $(img).parents("tr .group").next().attr("Key");
        
        PageMethods.eliminarGrupo(idGrupo, callBackFunction, ErroFunction);
    }

    function EditGrupo(img) {
        /// De esta forma se obtine el valor del agrupador, que esta puesto en una propiedad de cada una de los row y la propiedad a consultar se llama: key
        // Busco el primer padre del tipo TR y que tenga la clase de estilo grupo, luego voy a la siguente fila y ahi veo la propiedad key.
        //$(img).parents("tr .group").next().attr("Key")


        $("#Titulo").text("EDICION GRUPO: " + $(img).parents("tr .group").next().attr("value"))

        // Control de Texto simple
        $("#<%= txtNombre.ClientID %>").val($(img).parents("tr .group").next().find("td[filtro=Nombre]")[0].innerText);

        // Control de Texto oculto para almacenar el id del grupo
        $("#<%= hdIdGrupo.ClientID %>").val($(img).parents("tr .group").next().attr("Key"));

        /// Dado a que el control de clientes es un control de busqueda, esta es la forma en la que se setea el valor inicial.
        ctrClientes.set_Value($(img).parents("tr .group").next().find("td[filtro=Responsable]")[0].innerText, $(img).parents("tr .group").next().find("td[filtro=Responsable]").attr("Value"));

        /// Busco los integrantes del grupo para mostar en la pantalla de edicion.
        PageMethods.getDataGrupo($(img).parents("tr .group").next().attr("Key"), callBackFunctionIntegrantes, ErroFunction);


        $('#divGrupo').modal(
        {
            clickClose: false,
            showClose: true,
            zIndex: 999999,
            closeFunction: "ActualizarVista"
        });
    }

    function callBackFunctionIntegrantes(datos) {

        var t = [
            { "header": "Grupos Definidos" },
            { "header": "&nbsp;"}];

        var templates = {
            th: '<th class="HeaderVista_Marzzan">#{header}</th>',
            td: '<tr style="cursor:pointer" key="#{idGrupo}" >' +
                '<td align="left" style="width:98%" >#{Integrante}</td>' +
                '<td align="left"><img alt="Eliminar" src="Imagenes/delete.gif"  style="cursor:pointer;padding:3px"  onclick="EliminarGrupoIntegrante(#{id}, #{idGrupo});return false;" /></td>' +
                '</tr>'
        };

        var table = '<table width="95%" id="tblIntegrantes" border="1" cellpadding="0" cellspacing="0" style="font-size:13px; background-color: Transparent;"><thead><tr>';

        /// Genero la fila de encabezado
        $.each(t, function (key, val) {
            table += $.tmpl(templates.th, val);
        });

        table += '</tr></thead><tbody class="BodyDetalle">';

        /// Genero las filas del body
        row = 0;
        var totalRendicion = 0;
        for (var i = 0; i < datos.length; i++) {
            table += $.tmpl(templates.td, datos[row]);
            row++;
        }

        table += '</tbody></table>';

        /// Asigno la tabla generada al div para dibujarla
        $("#divIntegrantes")[0].innerHTML = table;
        $("#tblIntegrantes tbody tr:odd").addClass("rowAlt");
        $("#tblIntegrantes tbody tr:even").addClass("rowSpl");

        ctrGrupo.Clear();
    }

    $(document).ready(function () {

        window.setTimeout(function () {
            PageMethods.getData(callBackFunction, ErroFunction);
        });
    }, 300);

    function ErroFunction(datos) {
        alert(datos._message);
    }

    
</script>
<body style="background-image: url(Imagenes/repetido.jpg); margin-top: 1px; background-repeat: repeat-x;
    background-color: White;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        <Scripts>
            <asp:ScriptReference Path="~/FuncionesComunes.js" />
        </Scripts>
    </asp:ScriptManager>
    <table cellpadding="0" cellspacing="0" border="0" style="height: 100%; width: 100%;">
        <tr>
            <td>
                <div class="Header_panelContainerSimple">
                    <div class="CabeceraInicial">
                    </div>
                </div>
                <div class="CabeceraContent">
                    <table width="95%" border="0" cellspacing="0">
                        <tr>
                            <td valign="top" align="center" style="width: 100%;">
                                <div style="position: relative; top: -25px">
                                    <asp:Label Font-Names="Bookman Old Style" Font-Size="22pt" ForeColor="#5F5F5F" ID="lblTitulo"
                                        runat="server">Gestión de Grupos</asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="padding-top: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="padding-top: 10px">
                                <div style="width:70%;text-align:left;padding:5px">
                                    <button class="btnBasic_Marzzan" onclick="NuevoGrupo();return false;">
                                        <asp:LinkButton ID="LinkButton2" runat="server">
                                     <img style="border:0px;vertical-align:middle;" width="18" height="18" alt="" src="Imagenes/AddFormula.png" /><span style="padding:5px">Nuevo</span>
                                        </asp:LinkButton>
                                    </button>
                                </div>
                                <div id="divtbl">
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <div class="modal" id="divGrupo" style="width: 580px;">
        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%" id="tblPrincipal"
            runat="server">
            <tr>
                <td align="center" colspan="3">
                    <h4 id="Titulo">
                        Edición Grupo:
                    </h4>
                    <asp:HiddenField ID="hdIdGrupo" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" SkinID="lblBlack">Nombre:</asp:Label>
                </td>
                <td colspan="2">
                    <asp:TextBox Width="356px" ID="txtNombre" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" SkinID="lblBlack">Responsable:</asp:Label>
                </td>
                <td colspan="2" style="padding-bottom: 5px">
                    <asp:TextBox Width="356px" ID="txtResponsable" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr id="TR_TituloIntegrantes">
                <td colspan="3" align="center" style="background-color: #CCCCCC; padding-top: 5px">
                    <asp:Label ID="Label5" runat="server" SkinID="lblBlack">INTEGRANTES:</asp:Label>
                </td>
            </tr>
            <tr style="background-color: #CCCCCC" id="TR_TituloAdd">
                <td align="right">
                    <asp:Label ID="Label1" runat="server" SkinID="lblBlack">Grupo:</asp:Label>
                </td>
                <td align="left" style="padding: 5px">
                    <asp:TextBox Width="99%" ID="txtGrupo" runat="server"></asp:TextBox>
                </td>
                <td align="left">
                    <img src="Imagenes/AddFormula.png" width="16px" height="16px" style="cursor: pointer"
                        title="agregar grupo" onclick="AgregarGrupoIntegrante();return false;" />
                </td>
            </tr>
            <tr id="TR_TituloTabla">
                <td colspan="3" align="center" style="padding: 5px; background-color: #CCCCCC">
                    <div id="divIntegrantes">
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center" style="padding-top: 10px" colspan="3">
                    <button class="btnBasic_Marzzan" onclick="GrabarGrupo(false,null);return false;">
                        <asp:LinkButton ID="LinkButton1" runat="server">
                                     <img style="border:0px;vertical-align:middle;" width="18" height="18" alt="" src="Imagenes/AddFormula.png" /><span style="padding:5px">Grabar</span>
                        </asp:LinkButton>
                    </button>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
