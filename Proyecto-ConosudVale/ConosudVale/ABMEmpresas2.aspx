<%@ Page Title="" Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="ABMEmpresas2.aspx.cs" Inherits="ABMEmpresas2" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Assembly="ControlsAjaxNotti" Namespace="ControlsAjaxNotti" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .RadWindow.RadWindow_Sunset.rwNormalWindow.rwTransparentWindow
        {
            z-index: 999900000 !important;
        }
    </style>
    <script type="text/javascript">
        function ResponseEnd(sender, args) {

            if ($get("<%=txtCUIT.ClientID %>" + "_text").getAttribute("NroExistente") != null) {
                if ($get("<%=txtCUIT.ClientID %>" + "_text").getAttribute("NroExistente") == "True") {
                    radalert("El número de CUIT que intenta asignar ya existe.", 300, 100, 'Empresas');
                }
                else {
                    $find("<%=ServerControlWindow1.ClientID %>").CloseWindows();
                }
            }

        }

        function requestStart1(sender, args) {
            if (args.get_eventTarget().indexOf("ExportExcel") > 0) {
                args.set_enableAjax(false);
            }
        }

        function EditEmpresa() {
            LimpiarControles();
            var grid = $find("<%= RadGrid1.ClientID%>");
            var MasterTable = grid.get_masterTableView();


            var Item = MasterTable.get_selectedItems();
            if (Item.length > 0) {
                var cellRazonSocial = MasterTable.getCellByColumnUniqueName(Item[0], "RazonSocialColumn");
                var cellCUIT = MasterTable.getCellByColumnUniqueName(Item[0], "CUITColumn");
                var cellFechaAlta = MasterTable.getCellByColumnUniqueName(Item[0], "FechaAltaColumn");
                var cellRepresentanteTecnico = MasterTable.getCellByColumnUniqueName(Item[0], "RepresentanteTecnicoColumn");
                var cellPrestacionEmergencia = MasterTable.getCellByColumnUniqueName(Item[0], "PrestacionEmergenciaColumn");
                var cellDireccion = MasterTable.getCellByColumnUniqueName(Item[0], "DireccionColumn");
                var cellTelefono = MasterTable.getCellByColumnUniqueName(Item[0], "TelefonoColumn");
                var cellEmergencia = MasterTable.getCellByColumnUniqueName(Item[0], "EmergenciaColumn");
                var cellCorreoElectronico = MasterTable.getCellByColumnUniqueName(Item[0], "CorreoElectronicoColumn");

                var cellDescArt = MasterTable.getCellByColumnUniqueName(Item[0], "DescArtColumn");
                var cellDescVida = MasterTable.getCellByColumnUniqueName(Item[0], "DescVidaColumn");

                

                /// Controles Tipo TextBox
                document.getElementById("<%= txtRazonSocial.ClientID%>").innerText = cellRazonSocial.innerText;
                document.getElementById("<%= txtTecnico.ClientID%>").innerText = cellRepresentanteTecnico.innerText;
                document.getElementById("<%= txtPrestacionEmergencias.ClientID%>").innerText = cellPrestacionEmergencia.innerText;
                document.getElementById("<%= txtDireccion .ClientID%>").innerText = cellDireccion.innerText;
                document.getElementById("<%= txtTelefono.ClientID%>").innerText = cellTelefono.innerText;
                document.getElementById("<%= txtEmergencias.ClientID%>").innerText = cellEmergencia.innerText;
                document.getElementById("<%= txtEmail.ClientID%>").innerText = cellCorreoElectronico.innerText;

                document.getElementById("<%= lblSegArt.ClientID%>").innerText = cellDescArt.innerText;
                document.getElementById("<%= lblSegVida.ClientID%>").innerText = cellDescVida.innerText;


                /// Controles Tipo Telerik
                $find("<%= txtCUIT.ClientID%>").set_value(cellCUIT.innerText);


                /// Controles Tipo Fecha
                if (cellFechaAlta.outerText.trim() != "") {
                    dia = cellFechaAlta.outerText.substr(0, 2);
                    mes = parseInt(cellFechaAlta.outerText.substr(3, 2)) - 1 + '';
                    año = cellFechaAlta.outerText.substr(6);
                    Fecha = new Date(año, mes, dia);
                    $find("<%= txtFechaAlta.ClientID%>").set_selectedDate(Fecha);
                }
               

                $find("<%=ServerControlWindow1.ClientID %>").set_CollectionDiv('divPrincipal');
                $find("<%=ServerControlWindow1.ClientID %>").ShowWindows('divPrincipal', "Edición: " + cellRazonSocial.innerText);
            }
            else {
                radalert("Debe seleccionar una Empresa para poder editar sus datos", 250, 100, "Selección Empresa");
            }

        }

        function InitInsert() {
            LimpiarControles();
            $find("<%= RadGrid1.ClientID%>").get_masterTableView().clearSelectedItems();
            $find("<%=ServerControlWindow1.ClientID %>").set_CollectionDiv('divPrincipal');
            $find("<%=ServerControlWindow1.ClientID %>").ShowWindows('divPrincipal', "Nueva Empresa", $get("<%=txtRazonSocial.ClientID %>"));

        }

        function GuardarCambios() {
            if (Page_ClientValidate()) {
                var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");
                var Item = $find("<%= RadGrid1.ClientID%>").get_masterTableView().get_selectedItems();
                if (Item.length > 0) {
                    ajaxManager.ajaxRequest("Update");
                }
                else {
                    ajaxManager.ajaxRequest("Insert");
                }


            }
            return false;

        }

        function DeleteEmpresa() {

            radconfirm('Esta seguro de eliminar la empresa con toda la información relacionada?',DeleteConfirm, 330, 100, null, 'Empresas');
            function DeleteConfirm(result) {
                if (result) {
                    $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("delete");
                }
            }
        }

        function LimpiarControles() {

            document.getElementById("<%= txtRazonSocial.ClientID%>").innerText = "";
            document.getElementById("<%= txtTecnico.ClientID%>").innerText = "";
            document.getElementById("<%= txtPrestacionEmergencias.ClientID%>").innerText = "";
            document.getElementById("<%= txtDireccion .ClientID%>").innerText = "";
            document.getElementById("<%= txtTelefono.ClientID%>").innerText = "";
            document.getElementById("<%= txtEmergencias.ClientID%>").innerText = "";
            document.getElementById("<%= txtEmail.ClientID%>").innerText = "";

            /// Controles Tipo Telerik
            $find("<%= txtCUIT.ClientID%>").set_value("");


            /// Controles Tipo Fecha
            $find("<%= txtFechaAlta.ClientID%>").clear();

        }

        function OnClientItemsRequesting(sender, eventArgs) {
            var grid = $find("<%= RadGrid1.ClientID%>");
            var MasterTable = grid.get_masterTableView();
            var Items = MasterTable.get_selectedItems();
            var context = eventArgs.get_context();
            if (Items.length > 0) {
                context["IdEmpresa"] = Items[0].getDataKeyValue("IdEmpresa");
            }
            else
                context["IdEmpresa"] = "0";

        }

        
    </script>
    <telerik:radwindowmanager id="RadWindowManager1" showcontentduringload="false" visiblestatusbar="false"
        reloadonshow="false" runat="server" skin="Sunset">
    </telerik:radwindowmanager>
    <table cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td align="center" style="height: 35px; padding-left: 15px; padding-top: 10px">
                <asp:Label ID="lblTipoGestion" runat="server" Font-Bold="True" Font-Size="20pt" Font-Underline="false"
                    Font-Italic="True" ForeColor="black" Text="GESTION DE EMPRESAS" Font-Names="Arno Pro"></asp:Label>
            </td>
        </tr>
        <tr runat="server" id="trFiltro">
            <td align="left">
                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <table style="background-color: transparent; font-family: Sans-Serif; font-size: 11px;
                            width: 100%; vertical-align: middle;" border="0">
                            <tr>
                                <td valign="middle" align="right" style="padding-left: 10px; width: 340px">
                                    <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Arno Pro" ForeColor="#8C8C8C"
                                        Font-Size="16px" Text="Empresa:"></asp:Label>
                                </td>
                                <td valign="middle" align="left" width="310px">
                                    <telerik:radtextbox id="txtApellidoLegajo" runat="server" emptymessage="Ingrese Empresa a buscar"
                                        skin="Sunset" width="100%">
                                    </telerik:radtextbox>
                                </td>
                                <td valign="middle" align="left">
                                    <asp:ImageButton runat="server" Style="padding-left: 15px; padding-bottom: 15px;
                                        border: 0px; vertical-align: middle;" ImageUrl="~/Images/Search.png" ID="imgBuscar"
                                        OnClick="imgBuscar_Click" CausesValidation="false" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="center">
                                    <div style="width: 95%; height: 2px; border-top-style: solid; border-top-width: 2px;
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
    <table id="Table2" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
        border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #E0D6BE;
        font-family: Sans-Serif; font-size: 11px;" width="90%" >
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="updpnlGrilla" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <telerik:radgrid id="RadGrid1" runat="server" allowpaging="True" allowsorting="True"
                            showstatusbar="True" gridlines="None" skin="Sunset" autogeneratecolumns="False"
                            onitemcommand="RadGrid1_ItemCommand" PageSize="8">
                            <mastertableview datakeynames="IdEmpresa" clientdatakeynames="IdEmpresa" commanditemdisplay="Top"
                                editmode="PopUp" nomasterrecordstext="No existen registros." horizontalalign="NotSet">
                                <CommandItemTemplate>
                                    <div style="padding: 5px 5px;">
                                        <asp:LinkButton Mensaje="Editando Empresa..." ID="btnEdit" runat="server" Visible='<%# RadGrid1.EditIndexes.Count == 0 %>'
                                            CausesValidation="false" OnClientClick="EditEmpresa(); return false;">
                                            <img style="padding-right:5px;border:0px;vertical-align:middle;" alt="" src="Images/Edit.gif" />Editar</asp:LinkButton>&nbsp;&nbsp;
                                        <asp:LinkButton Mensaje="Preparando Para Nueva Empresa..." ID="btnInsert" runat="server"
                                            Visible='<%# !RadGrid1.MasterTableView.IsItemInserted %>' OnClientClick="InitInsert(); return false;">
                                            <img style="padding-right:5px;border:0px;vertical-align:middle;" alt="" src="Images/AddRecord.gif" />Insertar</asp:LinkButton>&nbsp;&nbsp;
                                        <asp:LinkButton Mensaje="Eliminando Empresa..." ID="btnDelete" OnClientClick="DeleteEmpresa();return false;"
                                            runat="server" CommandName="DeleteSelected">
                                            <img style="padding-right:5px;border:0px;vertical-align:middle;" alt="" src="Images/delete_16x16.gif" />Eliminar</asp:LinkButton>&nbsp;&nbsp;
                                        <asp:LinkButton CausesValidation="false" Mensaje="Exportando Empresas...." ID="ExportExcel"
                                            runat="server" CommandName="ExportEmpresas">
                                            <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/Excel_16x16.gif" />Excel</asp:LinkButton>&nbsp;&nbsp;
                                    </div>
                                </CommandItemTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn DataField="RazonSocial" HeaderText="RazonSocial" SortExpression="RazonSocial"
                                        UniqueName="RazonSocialColumn">
                                        <ItemTemplate>
                                            <asp:Label ID="RazonSocialLabel" runat="server" Text='<%# Eval("RazonSocial") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="CUIT" HeaderText="CUIT" SortExpression="CUIT"
                                        UniqueName="CUITColumn">
                                        <ItemTemplate>
                                            <asp:Label ID="CUITLabel" runat="server" Text='<%# string.Format("{0:##-########-#}", long.Parse(Eval("CUIT").ToString().Replace("-",""))) %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridDateTimeColumn DataField="FechaAlta" DataType="System.DateTime" DataFormatString="{0:dd/MM/yyyy}"
                                        HeaderText="FechaAlta" SortExpression="FechaAlta" UniqueName="FechaAltaColumn"
                                        EditFormColumnIndex="0" Display="false">
                                    </telerik:GridDateTimeColumn>
                                    <telerik:GridBoundColumn DataField="RepresentanteTecnico" HeaderText="Rep.Técnico"
                                        SortExpression="RepresentanteTecnico" UniqueName="RepresentanteTecnicoColumn"
                                        EditFormColumnIndex="0">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="PrestacionEmergencia" HeaderText="Prestacion Emergencia"
                                        SortExpression="PrestacionEmergencia" UniqueName="PrestacionEmergenciaColumn"
                                        EditFormColumnIndex="1" Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Direccion" HeaderText="Direccion" SortExpression="Direccion"
                                        UniqueName="DireccionColumn" EditFormColumnIndex="1" Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Telefono" HeaderText="Telefono" SortExpression="Telefono"
                                        UniqueName="TelefonoColumn" EditFormColumnIndex="1">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="CorreoElectronico" HeaderText="Correo Electronico"
                                        SortExpression="CorreoElectronico" UniqueName="CorreoElectronicoColumn" EditFormColumnIndex="1">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Emergencia" DataType="System.String" HeaderText="Emergencias"
                                        Display="false" SortExpression="Emergencia" UniqueName="EmergenciaColumn" EditFormColumnIndex="0"
                                        ItemStyle-Height="50px" ItemStyle-Width="150px" />

                                     <telerik:GridBoundColumn DataField="DescArt" HeaderText="Seguro ART" Display="false" UniqueName="DescArtColumn"  />
                                     <telerik:GridBoundColumn DataField="DescVida" HeaderText="Seguro Vida" Display="false" UniqueName="DescVidaColumn"  />

                                </Columns>
                                <EditFormSettings ColumnNumber="2" CaptionDataField="RazonSocial" CaptionFormatString="Editar el Empresa: {0}">
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
                                    <PopUpSettings ScrollBars="Auto" Modal="true" Width="60%" />
                                </EditFormSettings>
                            </mastertableview>
                            <validationsettings commandstovalidate="PerformInsert,Update" />
                            <clientsettings>
                                <Selecting AllowRowSelect="True" />
                            </clientsettings>
                        </telerik:radgrid>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="upEdicion" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <cc1:ServerControlWindow ID="ServerControlWindow1" runat="server" BackColor="WhiteSmoke">
                <ContentControls>
                    <div id="divPrincipal" style="height: 250px;">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="height: 250px;
                            text-align: left;">
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" SkinID="lblConosud" Text="Razon Social:"></asp:Label>
                                </td>
                                <td align="left" style="padding-left: 5px; padding-right: 5px">
                                    <asp:TextBox Width="250px" ID="txtRazonSocial" runat="server" Style="text-transform: uppercase"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtRazonSocial"
                                        Display="Dynamic" ErrorMessage="*" runat="server">
                                    </asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:Label ID="Label6" runat="server" SkinID="lblConosud" Text="CUIT:"></asp:Label>
                                </td>
                                <td style="padding-left: 5px; padding-right: 5px">
                                    <asp:UpdatePanel ID="upNroCUIT" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <telerik:radmaskedtextbox runat="server" id="txtCUIT" mask="##-########-#" displaymask="##-########-#"
                                                displaypromptchar=" " />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorCuit" ControlToValidate="txtCUIT"
                                                Display="Dynamic" ErrorMessage="*" runat="server">
                                            </asp:RequiredFieldValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" SkinID="lblConosud" Text="Fecha Alta:"></asp:Label>
                                </td>
                                <td style="padding-left: 5px; padding-right: 5px">
                                    <telerik:raddatepicker id="txtFechaAlta" mindate="1950/1/1" runat="server" zindex="922000000">
                                        <dateinput invalidstyleduration="100">
                                        </dateinput>
                                    </telerik:raddatepicker>
                                </td>
                                <td>
                                    <asp:Label ID="Label1" runat="server" SkinID="lblConosud" Text="Representante Técnico:"></asp:Label>
                                </td>
                                <td align="left" style="padding-left: 5px; padding-right: 5px">
                                    <asp:TextBox Width="250px" ID="txtTecnico" runat="server" Style="text-transform: uppercase"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" SkinID="lblConosud" Text="Contacto Emergencias:"></asp:Label>
                                </td>
                                <td style="padding-left: 5px; padding-right: 5px">
                                    <asp:TextBox Width="250px" ID="txtPrestacionEmergencias" runat="server" Style="text-transform: uppercase"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label9" runat="server" SkinID="lblConosud" Text="Direccion:"></asp:Label>
                                </td>
                                <td align="left" style="padding-left: 5px; padding-right: 5px">
                                    <asp:TextBox Width="250px" ID="txtDireccion" runat="server" Style="text-transform: uppercase"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label7" runat="server" SkinID="lblConosud" Text="Telefono:"></asp:Label>
                                </td>
                                <td style="padding-left: 5px; padding-right: 5px">
                                    <asp:TextBox Width="250px" ID="txtTelefono" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label8" runat="server" SkinID="lblConosud" Text="Emergencias:"></asp:Label>
                                </td>
                                <td style="padding-left: 5px; padding-right: 5px">
                                    <asp:TextBox Width="250px" ID="txtEmergencias" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label10" runat="server" SkinID="lblConosud" Text="Correo Electrónico:"></asp:Label>
                                </td>
                                <td style="padding-left: 5px; padding-right: 5px" colspan="3">
                                    <asp:TextBox Width="590px" ID="txtEmail" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label11" runat="server" SkinID="lblConosud" Text="Seg ART:"></asp:Label>
                                </td>
                                <td style="padding-left: 5px; padding-right: 5px" colspan="3">
                                    <asp:Label ID="lblSegArt" runat="server" SkinID="lblConosudNormal" Text="Sin Asignar"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label12" runat="server" SkinID="lblConosud" Text="Seg Vida:"></asp:Label>
                                </td>
                                <td style="padding-left: 5px; padding-right: 5px" colspan="3">
                                    <asp:Label ID="lblSegVida" runat="server" SkinID="lblConosudNormal" Text="Sin Asignar"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center" style="padding-top: 5px">
                                    <asp:Button ID="btnGuardar" runat="server" Text="Guardando Datos" Mensaje="Buscando Legajos solicitados..."
                                        CausesValidation="true" OnClientClick="return GuardarCambios(); " SkinID="btnConosudBasic" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentControls>
            </cc1:ServerControlWindow>
        </ContentTemplate>
    </asp:UpdatePanel>
    <telerik:radajaxmanager id="RadAjaxManager1" runat="server" clientevents-onresponseend="ResponseEnd"
        clientevents-onrequeststart="requestStart1" onajaxrequest="RadAjaxManager1_AjaxRequest">
        <clientevents onrequeststart="requestStart1"></clientevents>
        <ajaxsettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </ajaxsettings>
    </telerik:radajaxmanager>
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
