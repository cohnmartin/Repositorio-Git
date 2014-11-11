<%@ Page Title="Gestión de Cursos" Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="ABMCursos.aspx.cs" Inherits="ABMCursos" %>

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
    </style>
    <script type="text/javascript">
        var IdCursoLegajo = "";

        function EliminarLegajo() {
            var grid = $find("<%= gvVahiculos.ClientID%>");
            var MasterTable = grid.get_masterTableView();
            var Item = MasterTable.get_selectedItems();
            
            if (Item.length > 0) {
                var esAprobado = MasterTable.getCellByColumnUniqueName(Item[0], "EsAprobadoColumn").innerText;
                if ("<%=EsContratista%>" == "True" && esAprobado.trim() != "" && esAprobado == "True") {
                    radalert('El curso no puede ser eliminado ya que el mismo esta aprobado por el auditor, tome contacto con la administración para realizar la operación.', 400, 100, 'Eliminación Curso');
                    return false;
                }
                else {
                    blockConfirm('Esta seguro que desea eliminar el curso del legajo seleccionado?', event, 330, 100, '', 'Eliminación Curso');
                    return false;
                }
            }
            else {
                radalert('Debe seleccionar un curso para poder realizar la eliminación', 300, 100, 'Eliminación');
                return false;
            }
        }




        function AplicarCambios() {

            var IdLegajo = $find("<%= cboLegajos.ClientID %>").get_value();
            var IdCurso = $find("<%= cboCursos.ClientID %>").get_value();
            var IdInstituto = $find("<%= cboInstitutos.ClientID %>").get_value();
            var FechaCurso = $find("<%= txtFechaInicial.ClientID %>").get_selectedDate().format("dd/MM/yyyy");
            var FechaVen = $find("<%= txtFechaVen.ClientID %>").get_selectedDate().format("dd/MM/yyyy");
            var Obs = $find("<%= txtObs.ClientID %>").get_value();

            PageMethods.Aplicar(IdLegajo, IdCurso, IdInstituto, FechaCurso, FechaVen, Obs, IdCursoLegajo, AplicarTerminado, ErrorAplicar);
        }

        function AplicarTerminado() {
            $find("<%=ServerControlWindow1.ClientID %>").CloseWindows();
            var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");
            ajaxManager.ajaxRequest("ActualizarGrilla");
        }

        function ErrorAplicar() {

        }


        function InitInsert() {
            IdCursoLegajo = "-1";
            $find("<%=pnlEdicion.ClientID %>").ClearElements();
            $find("<%= cboLegajos.ClientID%>").set_enabled(true);
            $find("<%= gvVahiculos.ClientID%>").get_masterTableView().clearSelectedItems();
            $find("<%=ServerControlWindow1.ClientID %>").set_CollectionDiv('divPrincipal');
            $find("<%=ServerControlWindow1.ClientID %>").ShowWindows('divPrincipal', "Alta Nuevo Curso");
        }
        function EditLegajo() {

            var grid = $find("<%= gvVahiculos.ClientID%>");
            var MasterTable = grid.get_masterTableView();
            var Item = MasterTable.get_selectedItems();

            if (Item.length > 0) {
                IdCursoLegajo = MasterTable.getCellByColumnUniqueName(Item[0], "IdCursoLegajoColumn").outerText;
                var cellDescLegajo = MasterTable.getCellByColumnUniqueName(Item[0], "DesCompletaLegajoColumn");
                var cellCurso = MasterTable.getCellByColumnUniqueName(Item[0], "CursoColumn");
                var cellInstituto = MasterTable.getCellByColumnUniqueName(Item[0], "InstitutoColumn");
                var cellFechaCurso = MasterTable.getCellByColumnUniqueName(Item[0], "FechaCursoColumn");
                var cellFechaVencimiento = MasterTable.getCellByColumnUniqueName(Item[0], "FechaVenColumn");
                var cellObs = MasterTable.getCellByColumnUniqueName(Item[0], "ObsColumn");
                var cellIdCurso = MasterTable.getCellByColumnUniqueName(Item[0], "IdCursoColumn");

                /// Controles Tipo Telerik
                $find("<%= txtObs.ClientID %>").set_value(cellObs.innerText);


                /// Controles Tipo Fecha
                if (cellFechaCurso.outerText.trim() != "") {
                    dia = cellFechaCurso.outerText.substr(0, 2);
                    mes = parseInt(cellFechaCurso.outerText.substr(3, 2)) - 1 + '';
                    año = cellFechaCurso.outerText.substr(6);
                    Fecha = new Date(año, mes, dia);
                    $find("<%= txtFechaInicial.ClientID %>").set_selectedDate(Fecha);
                }


                if (cellFechaVencimiento.outerText.trim() != "") {
                    dia = cellFechaVencimiento.outerText.substr(0, 2);
                    mes = parseInt(cellFechaVencimiento.outerText.substr(3, 2)) - 1 + '';
                    año = cellFechaVencimiento.outerText.substr(6);
                    Fecha = new Date(año, mes, dia);
                    $find("<%= txtFechaVen.ClientID  %>").set_selectedDate(Fecha);
                }



                /// Controles Tipo Combos
                // Combo Curso

                if (cellCurso.outerText != "") {
                    var itemTipoDoc = $find("<%= cboCursos.ClientID%>").findItemByValue(cellIdCurso.outerText);

                    $find("<%= cboCursos.ClientID%>").set_selectedItem(itemTipoDoc);
                    itemTipoDoc.select();
                }
                else {
                    $find("<%= cboCursos.ClientID%>").clearSelection();
                }




                // Combo Instituto
                if (cellInstituto.outerText != "") {
                    var itemTipoDoc = $find("<%= cboInstitutos.ClientID%>").findItemByText(cellInstituto.outerText);
                    $find("<%= cboInstitutos.ClientID%>").set_selectedItem(itemTipoDoc);
                    itemTipoDoc.select();
                }
                else {
                    $find("<%= cboInstitutos.ClientID%>").clearSelection();
                }


                var esAprobado = MasterTable.getCellByColumnUniqueName(Item[0], "EsAprobadoColumn").innerText;
                /// Si el usuario logeado es contratista y el curso esta aprobado no se puede modificar.
                if ("<%=EsContratista%>" == "True" && esAprobado.trim() != "" && esAprobado == "True") {
                    $find("<%= pnlEdicion.ClientID%>").DisabledElement(true);
                }
                else {
                    $find("<%= pnlEdicion.ClientID%>").DisabledElement(false);
                }

                // Combo Legajo
                $find("<%= cboLegajos.ClientID%>").set_text(cellDescLegajo.outerText);
                $find("<%= cboLegajos.ClientID%>").set_enabled(false);

                $find("<%=ServerControlWindow1.ClientID %>").set_CollectionDiv('divPrincipal');
                $find("<%=ServerControlWindow1.ClientID %>").ShowWindows('divPrincipal', "Edición Curso");
                return;




            }
            else {
                radalert("Debe seleccionar una registro para poder editar sus datos", 250, 100, "Selección Curso");
            }



            return false;


        }
        function requestStart1(sender, args) {
            if (args.get_eventTarget().indexOf("ExportExcel") > 0) {
                args.set_enableAjax(false);
            }
        }

        function ItemsLoaded(combo, eventarqs) {
            combo.showDropDown();
        }

    </script>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Sunset" VisibleTitlebar="true"
        Style="z-index: 100000000" Title="Sub Contratistas">
        <Windows>
            <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close" Width="520" Height="280"
                Modal="true" NavigateUrl="ConsultaInformacionSueldos.aspx" VisibleTitlebar="true"
                Style="z-index: 100000000" Title="Información Sueldos" ReloadOnShow="true" VisibleStatusbar="false"
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
                            <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Size="20pt" Font-Underline="false"
                                Font-Italic="True" ForeColor="black" Text="GESTION DE CURSOS" Font-Names="Arno Pro"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <table style="font-family: Sans-Serif; font-size: 11px; width: 100%; "
                                        border="0">
                                        <tr>
                                            <td valign="middle" align="right" style="padding-left: 10px; width: 400px">
                                                <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Names="Arno Pro"
                                                    ForeColor="#8C8C8C" Font-Size="16px" Text="Apellido Legajo:"></asp:Label>
                                            </td>
                                            <td valign="middle" align="left" width="310px">
                                                <telerik:RadTextBox ID="txApellido" runat="server" EmptyMessage="Ingrese apellido Legajo"
                                                    Skin="Sunset" Width="100%">
                                                </telerik:RadTextBox>
                                            </td>
                                            <td valign="middle" align="left">
                                                <asp:ImageButton runat="server" Style="padding-left: 15px; padding-bottom: 15px;
                                                    border: 0px; vertical-align: middle;" ImageUrl="~/Images/Search.png" ID="imgBuscar"
                                                    OnClick="imgBuscar_Click" />
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
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" style="background-color: #f3f1ee">
                <table style="background-color: #f3f1ee; width: 100%;vertical-align:top">
                    <tr>
                        <td align="center">
                            <asp:UpdatePanel ID="UpdPnlGrilla" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvVahiculos" runat="server" AllowPaging="True" ShowStatusBar="True"
                                        GridLines="None" Skin="Sunset" AutoGenerateColumns="False" PageSize="20" Width="90%">
                                        <MasterTableView TableLayout="Fixed" CommandItemDisplay="Top" NoMasterRecordsText="No existen registros."
                                            HorizontalAlign="NotSet" DataKeyNames="IdCursoLegajo" ClientDataKeyNames="IdCursoLegajo">
                                            <CommandItemTemplate>
                                                <div style="padding: 5px 5px;">
                                                    <asp:LinkButton Mensaje="Buscar Legajo...." ID="btnEdit" runat="server" Visible='<%# gvVahiculos.EditIndexes.Count == 0 %>'
                                                        CausesValidation="false" OnClientClick="EditLegajo(); return false;">
                                <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/Edit.gif" />Editar</asp:LinkButton>&nbsp;&nbsp;
                                                    <asp:LinkButton Mensaje="Preparando par nuevo legajo...." ID="btnInsert" runat="server"
                                                        CausesValidation="false" Visible='<%# !gvVahiculos.MasterTableView.IsItemInserted %>'
                                                        OnClientClick="InitInsert(); return false;">
                                <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/AddRecord.gif" />Insertar</asp:LinkButton>&nbsp;&nbsp;
                                                    <asp:LinkButton Mensaje="Eliminando Legajo...." ID="btnDelete" OnClientClick="return EliminarLegajo();"
                                                        runat="server" OnClick="btnEliminar_Click" CausesValidation="false">
                                <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/delete_16x16.gif" />Eliminar</asp:LinkButton>&nbsp;&nbsp;
                                                </div>
                                            </CommandItemTemplate>
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="Legajo" HeaderText="Nombre Legajo" SortExpression="Apellido"
                                                    UniqueName="LegajoColumn">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Curso" HeaderText="Curso" SortExpression="Nombre"
                                                    UniqueName="CursoColumn">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="IdCurso" UniqueName="IdCursoColumn" Display="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Instituto" HeaderText="Instituto Dictador" SortExpression="NroDoc"
                                                    UniqueName="InstitutoColumn">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FechaCurso" HeaderText="Fecha Curso" DataFormatString="{0:d}"
                                                    UniqueName="FechaCursoColumn">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FechaVen" HeaderText="Fecha Curso" DataFormatString="{0:d}"
                                                    UniqueName="FechaVenColumn" Display="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Obs" UniqueName="ObsColumn" Display="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="DesCompletaLegajo" UniqueName="DesCompletaLegajoColumn"
                                                    Display="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="IdCursoLegajo" UniqueName="IdCursoLegajoColumn"
                                                    Display="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="EsAprobado" UniqueName="EsAprobadoColumn" Display="false">
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings>
                                            <ClientEvents />
                                            <Selecting AllowRowSelect="True" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </asp:UpdatePanel>
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
                    <div id="divPrincipal" style="height: 300px; width: 600px">
                        <cc1:ServerPanel runat="server" ID="pnlEdicion">
                            <BodyContent>
                                <table border="0" cellpadding="2" cellspacing="2" width="100%" style="padding-top: 0px;
                                    text-align: left;">
                                    <tr>
                                        <td colspan="2" style="background-color: #CACACA; padding-left: 10px">
                                            <asp:Label ID="Label22" runat="server" SkinID="lblConosud" Text="DATOS DEL CURSO"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" SkinID="lblConosud" Width="120px" Text="Nombre Legajo:"></asp:Label>
                                        </td>
                                        <td align="left" style="padding-left: 5px; padding-right: 5px">
                                            <telerik:RadComboBox ID="cboLegajos" runat="server" Skin="Sunset" Width="100%" EmptyMessage="Ingrese Nombre Legajo"
                                                OnItemsRequested="cboLegajos_ItemsRequested" OnClientItemsRequested="ItemsLoaded"
                                                EnableLoadOnDemand="true" ZIndex="922000000" AllowCustomText="true">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label14" runat="server" SkinID="lblConosud" Width="80px" Text="Curso:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <telerik:RadComboBox ID="cboCursos" runat="server" Skin="Sunset" Width="100%" EmptyMessage="Seleccione un curso"
                                                ZIndex="922000000" AllowCustomText="true" MarkFirstMatch="true">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" SkinID="lblConosud" Width="120px" Text="Inst. Dictador:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <telerik:RadComboBox ID="cboInstitutos" runat="server" Skin="Sunset" Width="100%"
                                                EmptyMessage="Seleccione un instituto" ZIndex="922000000" AllowCustomText="true"
                                                MarkFirstMatch="true">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" SkinID="lblConosud" Width="80px" Text="Fecha Curso:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <telerik:RadDatePicker ID="txtFechaInicial" MinDate="1950/1/1" runat="server" ZIndex="922000000">
                                            </telerik:RadDatePicker>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" SkinID="lblConosud" Width="120px" Text="Fecha Vencimiento:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <telerik:RadDatePicker ID="txtFechaVen" MinDate="1950/1/1" runat="server" ZIndex="922000000">
                                            </telerik:RadDatePicker>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label9" runat="server" SkinID="lblConosud" Width="80px" Text="Observación:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <telerik:RadTextBox Width="100%" Rows="5" TextMode="MultiLine" ID="txtObs" runat="server">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center" style="padding-top: 5px">
                                            <asp:Button ID="btnAplicar" OnClientClick="AplicarCambios();return false;" runat="server"
                                                Text="Grabar" SkinID="btnConosudBasic" />
                                        </td>
                                    </tr>
                                </table>
                            </BodyContent>
                        </cc1:ServerPanel>
                    </div>
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
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" ClientEvents-OnRequestStart="requestStart1"
        OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <ClientEvents OnRequestStart="requestStart1"></ClientEvents>
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="gvVahiculos">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvVahiculos" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
