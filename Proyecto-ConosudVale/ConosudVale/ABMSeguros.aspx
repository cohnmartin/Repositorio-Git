<%@ Page Title="Gestión de Seguros" Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="ABMSeguros.aspx.cs" Inherits="ABMSeguros" %>

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
        var IdSeguros = "";

        function AplicarCambios() {

            var FechaInicio = "";
            var FechaVencimiento = "";
            var FechaUltPago = "";


            if ($find("<%= txtFechaInicio.ClientID %>").get_selectedDate() != null)
                FechaInicio = $find("<%= txtFechaInicio.ClientID %>").get_selectedDate().format("dd/MM/yyyy");

            if ($find("<%= txtFechaVenicimiento.ClientID %>").get_selectedDate() != null)
                FechaVencimiento = $find("<%= txtFechaVenicimiento.ClientID %>").get_selectedDate().format("dd/MM/yyyy");

            if ($find("<%= txtFechaUltPago.ClientID %>").get_selectedDate() != null)
                FechaUltPago = $find("<%= txtFechaUltPago.ClientID %>").get_selectedDate().format("dd/MM/yyyy");



            PageMethods.Aplicar(
                    $find("<%= cboCompañia.ClientID %>").get_value(),
                    $find("<%= cboTipoSeguro.ClientID %>").get_value(),
                    $get("<%= txtNroPoliza.ClientID %>").value,
                    $get("<%= txtDescripcion.ClientID %>").value,
                    FechaInicio,
                    FechaVencimiento,
                    FechaUltPago,
                    $find("<%= cboEmpresaContratista.ClientID %>").get_value(),
                    IdSeguros,
                    AplicarTerminado, ErrorAplicar);
        }


        function EditVehiculo() {



            var grid = $find("<%= gvVahiculos.ClientID%>");
            var MasterTable = grid.get_masterTableView();
            var Item = MasterTable.get_selectedItems();

            if (Item.length > 0) {
                IdSeguros = MasterTable.getCellByColumnUniqueName(Item[0], "IdSeguroColumn").outerText;



                var cellNroPolizaColumn = MasterTable.getCellByColumnUniqueName(Item[0], "NroPolizaColumn").outerText;
                var cellIdCompañiaColumn = MasterTable.getCellByColumnUniqueName(Item[0], "IdCompañiaColumn").outerText;
                var cellIdEmpresaContratistaColumn = MasterTable.getCellByColumnUniqueName(Item[0], "IdEmpresaContratistaColumn").outerText;
                var cellFechaInicialColumn = MasterTable.getCellByColumnUniqueName(Item[0], "FechaInicialColumn").outerText;
                var cellFechaUltimoPagoColumn = MasterTable.getCellByColumnUniqueName(Item[0], "FechaUltimoPagoColumn").outerText;
                var cellFechaVencimientoColumn = MasterTable.getCellByColumnUniqueName(Item[0], "FechaVencimientoColumn").outerText;
                var cellIdTipoSeguroColumn = MasterTable.getCellByColumnUniqueName(Item[0], "IdTipoSeguroColumn").outerText;
                var cellDescripcionColumn = MasterTable.getCellByColumnUniqueName(Item[0], "DescripcionColumn").outerText;


                var cellDescEmpresaColumn = MasterTable.getCellByColumnUniqueName(Item[0], "DescEmpresaColumn").outerText;
                var cellDescTipoSeguroColumn = MasterTable.getCellByColumnUniqueName(Item[0], "DescTipoSeguroColumn").outerText;
                var cellDescCompañiaColumn  = MasterTable.getCellByColumnUniqueName(Item[0], "DescCompañiaColumn").outerText;


                /// Controles Tipo TExt
                $get("<%= txtNroPoliza.ClientID %>").value = cellNroPolizaColumn;
                $get("<%= txtDescripcion.ClientID %>").value = cellDescripcionColumn;



                /// Controles Tipo Combos
                $find("<%= cboEmpresaContratista.ClientID%>").set_text(cellDescEmpresaColumn);
                $find("<%= cboEmpresaContratista.ClientID%>").set_value(cellIdEmpresaContratistaColumn);



                // Combo Compaia de Seguro
                if (cellDescCompañiaColumn != "") {
                    var itemTipoDoc = $find("<%= cboCompañia.ClientID%>").findItemByText(cellDescCompañiaColumn);
                    $find("<%= cboCompañia.ClientID%>").set_selectedItem(itemTipoDoc);
                    itemTipoDoc.select();
                }
                else {
                    $find("<%= cboCompañia.ClientID%>").clearSelection();
                }


                // Combo Tipo Seguro
                if (cellDescTipoSeguroColumn != "") {
                    var itemTipoDoc = $find("<%= cboTipoSeguro.ClientID%>").findItemByText(cellDescTipoSeguroColumn);
                    $find("<%= cboTipoSeguro.ClientID%>").set_selectedItem(itemTipoDoc);
                    itemTipoDoc.select();
                }
                else {
                    $find("<%= cboTipoSeguro.ClientID%>").clearSelection();
                }



                /// Controles Tipo Fecha
                if (cellFechaInicialColumn.trim() != "") {
                    dia = cellFechaInicialColumn.substr(0, 2);
                    mes = parseInt(cellFechaInicialColumn.substr(3, 2)) - 1 + '';
                    año = cellFechaInicialColumn.substr(6, 4);
                    Fecha = new Date(año, mes, dia);
                    $find("<%= txtFechaInicio.ClientID %>").set_selectedDate(Fecha);
                }

                /// Controles Tipo Fecha
                if (cellFechaUltimoPagoColumn.trim() != "") {
                    dia = cellFechaUltimoPagoColumn.substr(0, 2);
                    mes = parseInt(cellFechaUltimoPagoColumn.substr(3, 2)) - 1 + '';
                    año = cellFechaUltimoPagoColumn.substr(6, 4);
                    Fecha = new Date(año, mes, dia);
                    $find("<%= txtFechaUltPago.ClientID %>").set_selectedDate(Fecha);
                }


                /// Controles Tipo Fecha
                if (cellFechaVencimientoColumn.trim() != "") {
                    dia = cellFechaVencimientoColumn.substr(0, 2);
                    mes = parseInt(cellFechaVencimientoColumn.substr(3, 2)) - 1 + '';
                    año = cellFechaVencimientoColumn.substr(6, 4);
                    Fecha = new Date(año, mes, dia);
                    $find("<%= txtFechaVenicimiento.ClientID %>").set_selectedDate(Fecha);
                }


                $find("<%=ServerControlWindow1.ClientID %>").set_CollectionDiv('divPrincipal');
                $find("<%=ServerControlWindow1.ClientID %>").ShowWindows('divPrincipal', "Edición Seguro");


            }
            else {
                radalert("Debe seleccionar una registro para poder editar sus datos", 250, 100, "Selección Seguro");
            }



            
            return false;


        }



        function AplicarTerminado() {
            $find("<%=ServerControlWindow1.ClientID %>").CloseWindows();
            var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");
            ajaxManager.ajaxRequest("ActualizarGrilla");
        }

        function ErrorAplicar() {

        }





        function InitInsert() {

            IdSeguros = "-1";
            $find("<%=pnlEdicion.ClientID %>").ClearElements();
            $find("<%= gvVahiculos.ClientID%>").get_masterTableView().clearSelectedItems();
            $find("<%=ServerControlWindow1.ClientID %>").set_CollectionDiv('divPrincipal');
            $find("<%=ServerControlWindow1.ClientID %>").ShowWindows('divPrincipal', "Nuevo Seguro");
        }

        function requestStart1(sender, args) {
            if (args.get_eventTarget().indexOf("ExportExcel") > 0) {
                args.set_enableAjax(false);
            }
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
                            <asp:Label ID="lblTipoGestion" runat="server" Font-Bold="True" Font-Size="20pt" Font-Underline="false"
                                Font-Italic="True" ForeColor="black" Text="GESTION DE SEGUROS" Font-Names="Arno Pro"></asp:Label>
                        </td>
                    </tr>
                    <tr runat="server" id="trFiltro">
                        <td align="left">
                            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <table style="background-color: transparent; font-family: Sans-Serif; font-size: 11px;
                                        width: 100%; vertical-align: middle;" border="0">
                                        <tr>
                                            <td valign="middle" align="right" style="padding-left: 10px; width: 400px">
                                                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arno Pro"
                                                    ForeColor="#8C8C8C" Font-Size="16px" Text="Empresa Contratista:"></asp:Label>
                                            </td>
                                            <td valign="middle" align="left" width="310px">
                                                <telerik:RadComboBox ID="cboEmpresa" runat="server" Skin="Default" Width="100%" EmptyMessage="Seleccione Empresa"
                                                    ZIndex="922000000" AllowCustomText="true" EnableLoadOnDemand="true" OnItemsRequested="cboEmpresa_ItemsRequested">
                                                </telerik:RadComboBox>
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
            <td align="center">
                <asp:UpdatePanel ID="UpdPnlGrilla" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <telerik:RadGrid ID="gvVahiculos" runat="server" AllowPaging="True" AllowSorting="True"
                            ShowStatusBar="True" GridLines="None" Skin="Sunset" AutoGenerateColumns="False"
                            PageSize="20" Width="90%">
                            <MasterTableView TableLayout="Fixed" CommandItemDisplay="Top" NoMasterRecordsText="No existen registros."
                                HorizontalAlign="NotSet" DataKeyNames="IdSeguro" ClientDataKeyNames="IdSeguro">
                                <CommandItemTemplate>
                                    <div style="padding: 5px 5px;">
                                        <asp:LinkButton Mensaje="Buscar Legajo...." ID="btnEdit" runat="server" Visible='<%# gvVahiculos.EditIndexes.Count == 0 %>'
                                            CausesValidation="false" OnClientClick="EditVehiculo(); return false;">
                                <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/Edit.gif" />Editar</asp:LinkButton>&nbsp;&nbsp;
                                        <asp:LinkButton Mensaje="Preparando par nuevo legajo...." ID="btnInsert" runat="server"
                                            CausesValidation="false" Visible='<%# !gvVahiculos.MasterTableView.IsItemInserted %>'
                                            OnClientClick="InitInsert(); return false;">
                                <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/AddRecord.gif" />Insertar</asp:LinkButton>&nbsp;&nbsp;
                                        <asp:LinkButton Style="text-align: left" Mensaje="Eliminando Legajo...." ID="btnDelete"
                                            OnClientClick="return blockConfirm('Esta seguro que desea eliminar el registro seleccionado?', event, 330, 100,'','Eliminación');"
                                            runat="server" OnClick="btnEliminar_Click" CausesValidation="false">
                                <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/delete_16x16.gif" />Eliminar</asp:LinkButton>&nbsp;&nbsp;
                                    </div>
                                </CommandItemTemplate>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="DescEmpresa" HeaderText="Contratista" UniqueName="DescEmpresaColumn">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="DescTipoSeguro" HeaderText="Tipo Seguro" UniqueName="DescTipoSeguroColumn">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="DescCompañia" HeaderText="Compañia Seguro" UniqueName="DescCompañiaColumn">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="NroPoliza" HeaderText="Nro Poliza" UniqueName="NroPolizaColumn">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>


                                    <telerik:GridBoundColumn DataField="IdSeguro" UniqueName="IdSeguroColumn" Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Compañia" UniqueName="IdCompañiaColumn" Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="EmpresaContratista" UniqueName="IdEmpresaContratistaColumn"
                                        Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="FechaInicial" UniqueName="FechaInicialColumn"
                                        Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="FechaUltimoPago" UniqueName="FechaUltimoPagoColumn"
                                        Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="FechaVencimiento" UniqueName="FechaVencimientoColumn"
                                        Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="TipoSeguro" UniqueName="IdTipoSeguroColumn" Display="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Descripcion" UniqueName="DescripcionColumn" Display="false">
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings>
                                <Selecting AllowRowSelect="True" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <cc1:ServerPanel runat="server" ID="pnlEdicion">
        <BodyContent>
            <asp:UpdatePanel ID="upEdicion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <cc1:ServerControlWindow ID="ServerControlWindow1" runat="server" BackColor="WhiteSmoke"
                        WindowColor="Rojo">
                        <ContentControls>
                            <div id="divPrincipal" style="width: 900px; height: 220px">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" style="padding-top: 0px;
                                    text-align: left;">
                                    <tr>
                                        <td colspan="4" style="background-color: #CACACA; padding-left: 10px">
                                            <asp:Label ID="lblTituloCaractesticas" runat="server" SkinID="lblConosud" Text="DATOS DEL SEGURO"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" SkinID="lblConosud" Text="Compañia:"></asp:Label>
                                        </td>
                                        <td align="left" style="padding-left: 5px; padding-right: 5px">
                                            <telerik:RadComboBox ID="cboCompañia" runat="server" Skin="Sunset" Width="90%" EmptyMessage="Seleccione Tipo Compañia"
                                                ZIndex="922000000" AllowCustomText="true">
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" SkinID="lblConosud" Text="Tipo Seguro:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <telerik:RadComboBox ID="cboTipoSeguro" runat="server" Skin="Sunset" Width="90%"
                                                EmptyMessage="Seleccione Tipo Unidad" ZIndex="922000000" AllowCustomText="true">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label14" runat="server" SkinID="lblConosud" Text="Nro Poliza:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <asp:TextBox Width="100px" ID="txtNroPoliza" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" SkinID="lblConosud" Text="Empresa:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <telerik:RadComboBox ID="cboEmpresaContratista" runat="server" Skin="Default" Width="100%"
                                                EmptyMessage="Seleccione Empresa" ZIndex="922000000" AllowCustomText="true" EnableLoadOnDemand="true"
                                                OnItemsRequested="cboEmpresa_ItemsRequested">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" SkinID="lblConosud" Text="Fecha Inicial:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <telerik:RadDatePicker ID="txtFechaInicio" MinDate="1950/1/1" runat="server" ZIndex="922000000">
                                            </telerik:RadDatePicker>
                                        </td>
                                        <td style="width: 180px">
                                            <asp:Label ID="Label5" runat="server" SkinID="lblConosud" Text="Fecha Venc.:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <telerik:RadDatePicker ID="txtFechaVenicimiento" MinDate="1950/1/1" runat="server"
                                                ZIndex="922000000">
                                            </telerik:RadDatePicker>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label7" runat="server" SkinID="lblConosud" Text="Fecha Ult. Pago:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <telerik:RadDatePicker ID="txtFechaUltPago" MinDate="1950/1/1" runat="server" ZIndex="922000000">
                                            </telerik:RadDatePicker>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label8" runat="server" SkinID="lblConosud" Text="Descipción:"></asp:Label>
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px">
                                            <asp:TextBox Width="90%" ID="txtDescripcion" runat="server" Rows="4" TextMode="MultiLine"></asp:TextBox>
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
                        </ContentControls>
                    </cc1:ServerControlWindow>
                </ContentTemplate>
            </asp:UpdatePanel>
        </BodyContent>
    </cc1:ServerPanel>
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
