<%@ Page Title="" Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="GestionHojadeRuta.aspx.cs" Inherits="GestionHojadeRuta" %>

<%@ Register Assembly="ControlsAjaxNotti" Namespace="ControlsAjaxNotti" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .lblBasic
        {
            font-size: 12px;
            font-weight: bold;
            text-transform: capitalize;
            font-family: Tahoma;
        }
        .HandCursor
        {
            cursor: hand;
        }
    </style>
    <script type="text/javascript">
        var idTimer;
        var accion = "Hide";

        function requestEnd(sender, args) {
            $find("<%=ServerControlWindow1.ClientID %>").CloseWindows();
        }

        function GuardarCambios() {

            var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");
            ajaxManager.ajaxRequest("Guardar");
            return false;
        }

        function LimpiarControles() {

            document.getElementById("<%= txtComentarioItem.ClientID%>").innerText = "";
            document.getElementById("<%= txtComentarioGral.ClientID%>").innerText = "";
            document.getElementById("<%= lblItem.ClientID%>").innerText = "";
            document.getElementById("<%= hdIdHoja.ClientID%>").value = "";
            document.getElementById("<%= hdIdPlantilla.ClientID%>").value = "";
        }

        function ClickEdit(imgEdit) {

            var grid = $find("<%= gvItemHoja.ClientID%>");
            var MasterTable = grid.get_masterTableView();
            var row = MasterTable.get_dataItems()[imgEdit.parentElement.parentElement.rowIndex - 1];
            var cell = MasterTable.getCellByColumnUniqueName(row, "columnDescripcionItem");
            var cellDocItem = MasterTable.getCellByColumnUniqueName(row, "imgComentarioItem");
            var cellDocGral = MasterTable.getCellByColumnUniqueName(row, "imgComentarioGral");
            var cellAprobacion = MasterTable.getCellByColumnUniqueName(row, "HojaFechaAprobacioncolumn");
            var cellchkAprobacion = MasterTable.getCellByColumnUniqueName(row, "chkAproboColumn");




            if (cellDocItem.all.length > 0)
                document.getElementById("<%= txtComentarioItem.ClientID%>").innerText = cellDocItem.all[0].getAttribute("coment");

            if (cellDocGral.all.length > 0)
                document.getElementById("<%= txtComentarioGral.ClientID%>").innerText = cellDocGral.all[0].getAttribute("coment");



            if (cellAprobacion.innerText.trim() != "") {
                dia = cellAprobacion.innerText.substr(0, 2);
                mes = parseInt(cellAprobacion.innerText.substr(3, 2)) - 1 + '';
                año = cellAprobacion.innerText.substr(6);
                FechaAprobacion = new Date(año, mes, dia);

                $find("<%= txtFechaAprobacion.ClientID%>").set_selectedDate(FechaAprobacion);


                /// Si esta aprobado, pero el auditor es distinto al usuario que esta legeado
                /// entonces oculto el boton de grabar.
                if (cellchkAprobacion.all.count > 0 && cellchkAprobacion.all[0].tagName == "IMG") {
                    document.getElementById("<%= btnAceptar.ClientID%>").style.display = "none";
                    document.getElementById("<%= lblGuardar.ClientID%>").style.display = "none";
                }
                // Si esta aprobado pero el auditor es el mismo que esta logeado entonces
                // lo dejo modificar.
                else {

                    document.getElementById("<%= btnAceptar.ClientID%>").style.display = "block";
                    document.getElementById("<%= lblGuardar.ClientID%>").style.display = "block";
                }


            }
            else {
                // Si no esta aprobado y el usuario que audito es el mismo usuario que esta legeado
                /// entonces lo dejo modificar.
                if (cellchkAprobacion.all[0].tagName != "IMG") {
                    document.getElementById("<%= btnAceptar.ClientID%>").style.display = "block";
                    document.getElementById("<%= lblGuardar.ClientID%>").style.display = "block";
                }
                // Si no esta aprobado y el usuario que audito es distinto al usuario que esta legeado
                /// entonces oculto el boton de grabar.
                else {

                    document.getElementById("<%= btnAceptar.ClientID%>").style.display = "none";
                    document.getElementById("<%= lblGuardar.ClientID%>").style.display = "none";
                }

            }

            document.getElementById("<%= lblItem.ClientID%>").innerText = cell.innerText;
            document.getElementById("<%= hdIdHoja.ClientID%>").value = row.getDataKeyValue("IdHoja");
            document.getElementById("<%= hdIdPlantilla.ClientID%>").value = row.getDataKeyValue("IdPlantilla");




            $find("<%=ServerControlWindow1.ClientID %>").set_CollectionDiv('divPrincipal');
            $find("<%=ServerControlWindow1.ClientID %>").ShowWindows('divPrincipal', "Edición Item");
            return false;
        }


        function ShowAvisoUltimoCertificado() {
            self.setTimeout("HideAviso()", 100)
        }

        function ShowAviso() {

            $("#ImgUltimoCertificado").fadeIn(700);
            idTimer = self.setTimeout("HideAviso()", 1400)

        }

        function HideAviso() {

            $("#ImgUltimoCertificado").fadeOut(500);
            idTimer = self.setTimeout("ShowAviso()", 10)
        }


        function Marcar(chk) {

            var grid = $find("<%= gvItemHoja.ClientID%>");
            var MasterTable = grid.get_masterTableView();
            var row = MasterTable.get_dataItems()[chk.parentElement.parentElement.parentElement.rowIndex - 1];
            var cell = MasterTable.getCellByColumnUniqueName(row, "HojaFechaAprobacioncolumn");
            var cellTerminoAprobacion = MasterTable.getCellByColumnUniqueName(row, "chkTerminoAuditoriaColumn");


            if (chk.checked) {

                cell.innerText = "<%= DateTime.Now.ToShortDateString() %>";
                cellTerminoAprobacion.all[0].checked = true;
                cellTerminoAprobacion.all[0].disabled = true;
            }
            else {
                cellTerminoAprobacion.all[0].checked = false;
                cellTerminoAprobacion.all[0].disabled = false;

                if (chk.parentElement.getAttribute("originalValue") != null) {
                    cell.innerText = chk.parentElement.getAttribute("originalValue").substring(0, chk.parentElement.getAttribute("originalValue").indexOf(" "));
                }
                else
                    cell.innerText = " ";

            }

        }

        function showToolTip(chk) {

            var tooltip = $find("<%= ToolTipComentario.ClientID %>");

            if (chk.getAttribute("coment") != "") {
                tooltip.set_text(chk.getAttribute("coment"));
            }
            else {
                tooltip.set_text("No hay comentario Ingresado");
            }

            tooltip.set_targetControlID(chk.id);
            tooltip.show();
        }

        function showToolTipAuditado(img) {

            var tooltip = $find("<%= ToolTipComentario.ClientID %>");

            if (img.getAttribute("coment") != "") {
                tooltip.set_title("Atención");
                tooltip.set_text("El item de la planilla fue auditado por: " + img.getAttribute("coment"));

            }

            tooltip.set_targetControlID(img.id);
            tooltip.show();
        }

        function ShowLegajos() {

            var doc = $find('<%=DockLegajos.ClientID %>');
            doc.set_closed(false);

        }

        function ShowReporteHistorico() {

            var idCabecera = "<%= IdCabecera %>";
            window.open('ReporteViewerPeriodoHistorico.aspx?Id=' + idCabecera + '&EsHistorico=true', 'mywindowsss');
        }

        function ShowEstimacion() {

            var doc = $find('<%=DockEstimacion.ClientID %>');
            doc.set_closed(false);

        }

        function CloseEstimacion() {
            var doc = $find('<%=DockEstimacion.ClientID %>');
            doc.set_closed(true);
        }

        function confirmCallBackFn(arg) {
            var idCabecera = "<%= IdCabecera %>";
            var idUsuario = "<%= IdUsuario %>";


            if (arg == true) {
                $.ajax({ type: "POST",
                    data: "{ IdHojaRuta: '" + idCabecera + "',IdUsuario:'" + idUsuario + "'}",
                    url: "GestionHojadeRuta.aspx/AprobarHojaDeRuta",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    cache: false,
                    success: function (AccionValida) { js_Aprobacion(AccionValida) },
                    error: function (msg) { alert("Existe un error con la aplicación: " + msg); }
                });
            }
        }

        function js_AprobacionAsync() {

            if (idTimer != undefined)
                clearTimeout(idTimer);

            if ("<%= TieneLegajos %>" == "False") {
                var resp = confirm('La hoja de ruta no posee legajos asociados, esta seguro de aprobar la hoja de todas formas?');

                confirmCallBackFn(resp);

                return false;
            }
            else {
                confirmCallBackFn(true);
            }

        }

        function js_Desaprobacion() {


            var idCabecera = "<%= IdCabecera %>";
            var idUsuario = "<%= IdUsuario %>";



            $.ajax({ type: "POST",
                data: "{ IdCabecera: '" + idCabecera + "',IdUsuario:'" + idUsuario + "'}",
                url: "GestionHojadeRuta.aspx/DesaprobarHojaDeRuta",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                cache: false,
                success: function (AccionValida) { js_Aprobacion(AccionValida) },
                error: function (msg) { alert("Existe un error con la aplicación: " + msg); }
            });


        }


        function js_Aprobacion(AccionValida) {
            var Resultado = AccionValida.d.split('|');

            if (Resultado[0] == "APROBADA") {
                radalert("La hoja de ruta se ha aprobado correctamente.", 250, 100, "Aprobación");
            }
            else if (Resultado[0] == "DESAPROBADA") {
                radalert("La hoja de Ruta se ha DESAPROBADO correctamente.", 300, 100, "Desaprobación");
                window.setTimeout(function () {
                    location.reload();
                }, 2500);
            }
            else if (Resultado[0] == "ITEMS") {
                radalert("La hoja de Ruta no se ha podido aprobar ya que posee items no aprobados", 250, 100, "Advertencia!");
            }
            else if (Resultado[0] == "HOJAS") {
                radalert("La hoja de Ruta no se puede aprobar ya que las hojas anteriores no estan aprobadas.", 250, 100, "Advertencia!");
            }
            else if (Resultado[0] == "SUBACTUAL") {
                radalert("La hoja de Ruta no se puede aprobar ya que las hojas del o los Sub-Contratistas no estan aprobadas, para el período actual.", 250, 100, "Advertencia!");
            }
            else {
                radalert("La hoja de Ruta no se puede aprobar ya que las hojas anteriores del contratista y sub contratista no estan aprobadas.", 250, 100, "Advertencia!");
            }


            if (Resultado[1] == "TRUE") {
                ShowAvisoUltimoCertificado();
            }


        }

        function OnInitialize(dock, args) {
            dock.get_element().style.zIndex = 900000009;
        }


        function ShowSueldo(obj) {
            var rowIndex = obj.parentElement.parentElement.rowIndex - 1;
            var Items = $find("<%=gvLegajos.ClientID %>").get_masterTableView().get_dataItems();
            var periodo = document.getElementById("<%=lblPeriodo.ClientID %>").innerText;
            if (Items.length > 0) {

                var url = 'ConsultaInformacionSueldos.aspx?IdLegajo=' + Items[rowIndex].getDataKeyValue("Legajos.IdLegajos") + "&PeriodoEspecifico=" + periodo;
                var name = "RadWindow1";
                var manager = $find("<%=radwindowsmanager.ClientID %>");
                var oWnd = manager.open(url, name);

                //var oWnd = radopen('ConsultaInformacionSueldos.aspx?IdLegajo=' + Items[rowIndex].getDataKeyValue("Legajos.IdLegajos") + "&PeriodoEspecifico=" + periodo, 'RadWindow1');
            }
            else {
                radalert("Debe seleccionar un legajo para ver su información de sueldos", 250, 100, "Selección Legajo");
            }

        }
        function requestStart(sender, args) {
            if (args.get_eventTarget().indexOf("ExportExcel") > 0) {
                args.set_enableAjax(false);
            }
        }




        function ImprimirCertificadoHoja() {
            window.print();

        }
    </script>
    <telerik:RadDock runat="server" ID="DockLegajos" Skin="Sunset" Title="Listado de Legajos"
        DockMode="Floating" OnClientInitialize="OnInitialize" Top="110px" Left="220px"
        Closed="true" Width="750" Height="350" ExpandedHeight="350">
        <ContentTemplate>
            <table width="99%">
                <tr>
                    <td align="center" valign="top">
                        <asp:UpdatePanel ID="upImpresionLegajos" runat="server" UpdateMode="Conditional">
                            <contenttemplate>
                                <asp:LinkButton ToolTip="Exportar Listado Legajos" Mensaje="Exportando Legajos...."
                                    ID="ExportExcel" runat="server" OnClick="ExportExcel_Click">
                                    <img tooltip="Exportar Listado Legajos" style="padding-right: 5px; border: 0px; vertical-align: middle;"
                                        alt="" src="Images/Excel_16x16.gif" />&nbsp;
                                    <asp:Label ID="lbl" runat="server" Text="Exportar Legajos"></asp:Label>
                                </asp:LinkButton>
                            </contenttemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td align="center" style="width: 95%">
                        <asp:UpdatePanel ID="upGrillaLegajos" runat="server" UpdateMode="Conditional">
                            <contenttemplate>
                                <telerik:RadGrid ID="gvLegajos" runat="server" AutoGenerateColumns="False" GridLines="None"
                                    Skin="Sunset" Width="100%" AllowAutomaticUpdates="false">
                                    <MasterTableView ShowHeadersWhenNoRecords="true" ShowHeader="true" AllowAutomaticUpdates="false"
                                        NoMasterRecordsText="No hay Legajos Asociados" NoDetailRecordsText="No hay Legajos Asociados"
                                        Width="100%" Height="100%" ShowFooter="false" TableLayout="Fixed" ClientDataKeyNames="IdLegajos">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="NombreCompleto" HeaderText="Nombre" UniqueName="ApellidoColumn"
                                                ReadOnly="true">
                                                <HeaderStyle Width="250px" HorizontalAlign="Center" />
                                                <ItemStyle Wrap="false" HorizontalAlign="Left" Width="100%" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="NroDoc" HeaderText="NroDocumento" UniqueName="columnNroDoc"
                                                ReadOnly="true">
                                                <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Convenio" HeaderText="Convenio" UniqueName="columnConvenio"
                                                ReadOnly="true">
                                                <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn HeaderText="Info Sueldos" UniqueName="ApellidoInfoSueldos"
                                                ReadOnly="true">
                                                <ItemTemplate>
                                                    <asp:Label onClick="ShowSueldo(this);" ID="lblInfoSueldos" Style="text-transform: capitalize"
                                                        runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="PeriodoColumn" Visible="true" HeaderText="Periodo"
                                                ReadOnly="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPeriodo" runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Wrap="false" HorizontalAlign="Center" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="ContratoColumn" Visible="true" HeaderText="Contrato"
                                                ReadOnly="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblContrato" runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Wrap="false" HorizontalAlign="Center" />
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                    <ClientSettings>
                                        <Scrolling AllowScroll="false" UseStaticHeaders="false" SaveScrollPosition="false" />
                                    </ClientSettings>
                                </telerik:RadGrid>
                            </contenttemplate>
                            <triggers>
                                <asp:AsyncPostBackTrigger ControlID="ExportExcel" EventName="Click" />
                            </triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </telerik:RadDock>
    <telerik:RadDock runat="server" ID="DockEstimacion" Skin="Sunset" Title="Estimación del Contrato"
        DockMode="Floating" OnClientInitialize="OnInitialize" Top="110px" Left="220px"
        Closed="true" Width="550" Height="300" ExpandedHeight="300">
        <ContentTemplate>
            <table width="100%" style="height: 100%">
                <tr>
                    <td align="center" valign="top">
                        <telerik:RadTextBox ID="txtEstimacion" runat="server" Width="95%" Height="100%" Text=""
                            Skin="Sunset" TextMode="MultiLine" Rows="14">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td align="center" valign="top">
                        <asp:UpdatePanel ID="upEstimacion" runat="server" UpdateMode="Conditional">
                            <contenttemplate>
                                <asp:Button ID="btnAplicarEstimacion" runat="server" SkinID="btnConosudBasic" Text="Guardar"
                                    OnClick="btnAplicarEstimacion_Click" OnClientClick="CloseEstimacion();" />
                            </contenttemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </telerik:RadDock>
    <telerik:RadWindowManager ID="radwindowsmanager" runat="server" Skin="Sunset" Style="z-index: 99900000009">
        <Windows>
            <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close" Width="520" Height="280"
                Modal="true" NavigateUrl="ConsultaInformacionSueldos.aspx" VisibleTitlebar="true"
                Style="z-index: 100000000" Title="Información Sueldos" ReloadOnShow="true" VisibleStatusbar="false"
                ShowContentDuringLoad="false" Skin="Sunset">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <table cellpadding="0" cellspacing="5" style="width: 80%" id="tblEncabezado" runat="server">
        <tr>
            <td align="center" style="height: 25px; background: url('images/sprite.gif') 0  -997px repeat-x">
                <asp:Label ID="lblTitulo" runat="server" Font-Bold="True" Font-Size="14pt" Font-Names="Sans-Serif"
                    ForeColor="#E0D6BE" Text="Gestión Contrato Nro: C2007/156" Width="90%"></asp:Label>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td align="center">
                <table id="Table1" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
                    border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #E0D6BE;
                    font-family: Sans-Serif; font-size: 11px;" width="85%">
                    <tr>
                        <td style="width: 70px" align="left">
                            <asp:Label ID="Label1" runat="server" Text="Servicio:"></asp:Label>
                        </td>
                        <td align="left" colspan="3">
                            <asp:Label ID="lblContrato" runat="server" CssClass="lblBasic"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 70px" align="left">
                            <asp:Label ID="Label5" runat="server" Text="Contratista:"></asp:Label>
                        </td>
                        <td align="left" style="width: 390px">
                            <asp:Label ID="lblContratista" runat="server" CssClass="lblBasic" Style="text-transform: capitalize"></asp:Label>
                        </td>
                        <td align="left" style="width: 84px">
                            <asp:Label ID="Label6" runat="server" Text="Sub Contratista:"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblSubContratista" runat="server" CssClass="lblBasic"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 75px">
                            <asp:Label ID="Label7" runat="server" Text="Período:"></asp:Label>
                        </td>
                        <td align="left" style="width: 350px">
                            <asp:Label ID="lblPeriodo" runat="server" CssClass="lblBasic"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="Label8" runat="server" Text="Estado:"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblEstado" runat="server" Font-Bold="true" Font-Size="17px" ForeColor="blue"></asp:Label>
                            <asp:Label ID="lblUsuarioAprobador" runat="server" Font-Size="10px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 70px" align="left">
                            <asp:Label ID="Label3" runat="server" Text="Fecha Inicial:"></asp:Label>
                        </td>
                        <td style="width: 390px" align="left">
                            <asp:Label ID="lblFechaIncial" runat="server" CssClass="lblBasic"></asp:Label>
                        </td>
                        <td style="width: 84px" align="left">
                            <asp:Label ID="Label9" runat="server" Text="Fecha Final:"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblFechaFinal" runat="server" CssClass="lblBasic"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4" id="tdUltimoCertificado" runat="server" style="height: 30px">
                            &nbsp;<img id="ImgUltimoCertificado" src="images/UltimoCertificado.gif" alt="Último Certificado" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table style="border-right: #843431 thin solid; border-top: #843431 thin solid; border-left: #843431 thin solid;
                    border-bottom: #843431 thin solid; background-color: #E0D6BE; font-family: Sans-Serif;
                    font-size: 11px; height: 100%" width="90%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="right" valign="top" style="padding-left: 10px; padding-top: 5px">
                            <table cellpadding="0" cellspacing="0">
                                <tr runat="server" id="trLegajos">
                                    <td>
                                        <asp:ImageButton runat="server" ID="imgLegajos" ImageUrl="Images/Legajos.gif" OnClientClick="ShowLegajos(); return false;"
                                            Style="cursor: hand;" ToolTip="Lista de Legajos Asociados" />
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
                                <tr runat="server" id="trDesaprobacion">
                                    <td>
                                        <asp:ImageButton runat="server" ID="imgDesaprobar" ImageUrl="Images/DesaprobarTab.gif"
                                            OnClientClick="js_Desaprobacion(); return false;" Style="cursor: hand;" ToolTip="Desaprobar Hoja de Ruta" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="padding-right: 10px; padding-top: 5px">
                            <asp:UpdatePanel ID="upGrilla" runat="server" UpdateMode="Conditional">
                                <contenttemplate>
                                    <telerik:RadGrid ID="gvItemHoja" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        Skin="Sunset" Height="100%" Width="100%" AllowAutomaticUpdates="True" OnItemDataBound="gvItemHoja_ItemDataBound"
                                        OnItemCommand="gvItemHoja_ItemCommand">
                                        <MasterTableView DataKeyNames="IdHoja,IdPlantilla" ClientDataKeyNames="IdHoja,IdPlantilla"
                                            ShowHeadersWhenNoRecords="true" ShowHeader="true" EditMode="PopUp" AllowAutomaticUpdates="true"
                                            NoMasterRecordsText="Por favor seleccione los valores de filtrado para obtener una hojas de ruta"
                                            NoDetailRecordsText="Por favor seleccione los valores de filtrado para obtener una hojas de ruta"
                                            Width="100%" Height="100%"  TableLayout="Fixed" CommandItemDisplay="Bottom">
                                            <CommandItemTemplate>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr  >
                                                        <td align="right" style="padding:5px;padding-right:40px">
                                                            <asp:Button ID="btnAplicar" runat="server" SkinID="btnConosudBasic" Text="Aplicar Cambios" OnClick="btnAplicar_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </CommandItemTemplate>
                                            <CommandItemStyle HorizontalAlign="Right" />
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="" UniqueName="imgEditColumn">
                                                    <ItemTemplate>
                                                        <asp:ImageButton Style="cursor: hand; text-align: center" ID="imgEdit" Mensaje="Editando Item Para Auditar..."
                                                            runat="server" ImageUrl="images/Edit.gif" OnClientClick="return ClickEdit(this);" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="Descripcion" HeaderText="Titulo" UniqueName="columnDescripcionItem"
                                                    ReadOnly="true">
                                                    <ItemStyle Wrap="True" HorizontalAlign="Left" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn DataField="DocComentario" HeaderText="Doc." UniqueName="imgComentarioDoc"
                                                    ReadOnly="true">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="NroDocTextBox" runat="server" Text='<%# Bind("DocComentario") %>'
                                                            Width="500px" Height="150px" TextMode="MultiLine"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Image ImageUrl="~/images/notepad_16x16.gif" ID="imgcomentariodoc" runat="server"
                                                            onclick="showToolTip(this);" coment='<% #Eval("DocComentario") %>' Style="cursor: hand;" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn DataField="HojaComentario" HeaderText="Item" UniqueName="imgComentarioItem">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtComentarioItemHoja" runat="server" Text='<%# Bind("HojaComentario") %>'
                                                            Width="500px" Height="150px" TextMode="MultiLine"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Image ImageUrl="~/images/notepad_16x16.gif" ID="imgcomentarioitem" runat="server"
                                                            onclick="showToolTip(this);" coment='<% #Eval("HojaComentario") %>' Style="cursor: hand;" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Gral" UniqueName="imgComentarioGral">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtComentarioGralEdit" runat="server" Width="500px" Height="150px"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Image ImageUrl="~/images/notepad_16x16.gif" ID="imgcomentariogral" runat="server"
                                                            onclick="showToolTip(this);" Style="cursor: hand;" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Aprobar" UniqueName="chkAproboColumn">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAprobo" runat="server" onclick="Marcar(this);" originalValue='<% #  Eval("HojaFechaAprobacion") %>' />
                                                        <asp:Image ImageUrl="~/images/AuditadoPor.gif" ID="imagenAuditadoPor" runat="server"
                                                            Visible="false" onclick="showToolTipAuditado(this);" coment='<% #Eval("AuditadoPor") %>'
                                                            Style="cursor: hand;" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridDateTimeColumn DataField="HojaFechaAprobacion" HeaderText="F. Aprobación"
                                                    UniqueName="HojaFechaAprobacioncolumn" DataFormatString="{0:dd/MM/yyyy}">
                                                    <HeaderStyle Width="75px" HorizontalAlign="Center" />
                                                </telerik:GridDateTimeColumn>
                                                <telerik:GridTemplateColumn HeaderText="Finalizado" UniqueName="chkTerminoAuditoriaColumn">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkTerminoAuditoria" runat="server" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                            <EditFormSettings ColumnNumber="1" CaptionDataField="Descripcion" CaptionFormatString="Edición del Item: {0}">
                                                <FormCaptionStyle HorizontalAlign="Left" Width="100%"></FormCaptionStyle>
                                                <FormMainTableStyle GridLines="Both" CellSpacing="0" CellPadding="0" BackColor="White"
                                                    Width="100%" />
                                                <FormTableStyle CellSpacing="0" CellPadding="2" BackColor="White" HorizontalAlign="Left" />
                                                <FormTableItemStyle Wrap="False" HorizontalAlign="Left"></FormTableItemStyle>
                                                <FormTableAlternatingItemStyle Wrap="False" HorizontalAlign="Left"></FormTableAlternatingItemStyle>
                                                <EditColumn ButtonType="ImageButton" InsertText="Insertar" UpdateText="Actualizar"
                                                    UniqueName="EditCommandColumn1" CancelText="Cancelar">
                                                </EditColumn>
                                                <FormTableButtonRowStyle HorizontalAlign="Right"></FormTableButtonRowStyle>
                                                <PopUpSettings ScrollBars="Auto" Modal="true" Width="60%" />
                                            </EditFormSettings>
                                        </MasterTableView>
                                        <ClientSettings>
                                            <Scrolling AllowScroll="false" UseStaticHeaders="false" SaveScrollPosition="false" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </contenttemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <telerik:RadToolTip runat="server" ID="ToolTipComentario" Skin="Hay" ShowCallout="true"
        ShowEvent="FromCode" RelativeTo="Element" Position="MiddleLeft" Width="220" Title="Comentario Ingresado"
        Animation="Fade">
    </telerik:RadToolTip>
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
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" ClientEvents-OnRequestStart="requestStart"
        ClientEvents-OnResponseEnd="requestEnd" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <ClientEvents OnRequestStart="requestStart"></ClientEvents>
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ExportExcel">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ExportExcel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <asp:UpdatePanel ID="upEdicion" runat="server" UpdateMode="Conditional">
        <contenttemplate>
            <cc1:ServerControlWindow ID="ServerControlWindow1" runat="server" onClientCanceled="LimpiarControles"
                BackColor="WhiteSmoke" WindowColor="Rojo">
                <ContentControls>
                    <div id="divPrincipal" style="height:410px">
                        <asp:HiddenField ID="hdIdHoja" runat="server" />
                        <asp:HiddenField ID="hdIdPlantilla" runat="server" />
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="text-align: left;">
                            <tr>
                                <td colspan="2" align="center" style="padding-bottom: 10px">
                                    <asp:Label ID="lblItem" Style="line-height: 15px" Width="500px" SkinID="lblConosud"
                                        runat="server" Text="Item:"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblInfoSueldosss" SkinID="lblConosud" runat="server" Text="Comentario Item:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtComentarioItem" runat="server" Width="500px" Height="150px" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" SkinID="lblConosud" Text="Comentario Gral:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtComentarioGral" runat="server" Width="500px" Height="150px" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label4" runat="server" SkinID="lblConosud" Text="Fecha Aprobación:"></asp:Label>
                                </td>
                                <td>
                                    <telerik:RadDatePicker ID="txtFechaAprobacion" MinDate="1950/1/1" runat="server"
                                        ZIndex="922000000">
                                        <DateInput InvalidStyleDuration="100">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td style="width: 83%">
                                                <asp:ImageButton ID="btnAceptar" ImageUrl="~/images/ok_16x16.gif" runat="server"
                                                    OnClientClick="return GuardarCambios(); " />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblGuardar" runat="server" SkinID="lblConosud" Text="Guardar Cambios"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentControls>
            </cc1:ServerControlWindow>
        </contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
