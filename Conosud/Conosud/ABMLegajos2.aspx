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
    </style>

    <script type="text/javascript">
        function ResponseEnd(sender, args) {

            if ($get("<%=txtNroDocEdit.ClientID %>").getAttribute("NroExistente") != null) {
                if ($get("<%=txtNroDocEdit.ClientID %>").getAttribute("NroExistente") == "True") {
                    radalert("El número de documento que intenta asignar ya existe.",300,100,'Legajos');
                }
                else {
                    $find("<%=ServerControlWindow1.ClientID %>").CloseWindows();
                }
            }

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

        function LimpiarControles() {
            /// Controles Tipo TextBox
            document.getElementById("<%= txtApellido.ClientID%>").innerText = "";
            document.getElementById("<%= txtNombre.ClientID%>").innerText = "";
            document.getElementById("<%= txtNroDocEdit.ClientID%>").innerText = "";
            document.getElementById("<%= txtDireccion.ClientID%>").innerText = "";
            document.getElementById("<%= txtCodigoPostal.ClientID%>").innerText = "";
            document.getElementById("<%= txtTelFijo.ClientID%>").innerText = "";
            document.getElementById("<%= txtEmail.ClientID%>").innerText = "";

            /// Controles Tipo Telerik
            $find("<%= txtCUIL.ClientID%>").set_value("");

            /// Controles Tipo Fecha
            $find("<%= txtFechaNacimiento.ClientID%>").clear();
            $find("<%= txtFechaIngreso.ClientID%>").clear();

            /// Controles Tipo Combos
            $find("<%= cboTipoDoc.ClientID%>").clearSelection();
            $find("<%= cboEstadoCivil.ClientID%>").clearSelection();
            $find("<%= cboNacionalidad.ClientID%>").clearSelection();
            $find("<%= cboConvenio.ClientID%>").clearSelection();
            $find("<%= cboProvincia.ClientID%>").clearSelection();
        }
        function InitInsert() {


            LimpiarControles();
            $find("<%= RadGrid1.ClientID%>").get_masterTableView().clearSelectedItems();
            $find("<%=ServerControlWindow1.ClientID %>").set_CollectionDiv('divPrincipal');
            $find("<%=ServerControlWindow1.ClientID %>").ShowWindows('divPrincipal', "Nuevo Legajo", $get("<%=txtApellido.ClientID %>"));
        }
        function EditLegajo() {
            LimpiarControles();
            var grid = $find("<%= RadGrid1.ClientID%>");
            var MasterTable = grid.get_masterTableView();


            var Item = MasterTable.get_selectedItems();
            if (Item.length > 0) {
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
                var cellTipoDocumento = MasterTable.getCellByColumnUniqueName(Item[0], "TipoDocumentoColumn");
                var cellConvenio = MasterTable.getCellByColumnUniqueName(Item[0], "ConvenioColumn");
                var cellFechaIngreos = MasterTable.getCellByColumnUniqueName(Item[0], "FechaIngreosColumn");

                /// Controles Tipo TextBox
                document.getElementById("<%= txtApellido.ClientID%>").innerText = cellApellido.innerText;
                document.getElementById("<%= txtNombre.ClientID%>").innerText = cellNombre.innerText;
                document.getElementById("<%= txtNroDocEdit.ClientID%>").innerText = cellNroDoc.innerText;
                document.getElementById("<%= txtDireccion.ClientID%>").innerText = cellDireccion.innerText;
                document.getElementById("<%= txtCodigoPostal.ClientID%>").innerText = cellCodigoPostal.innerText;
                document.getElementById("<%= txtTelFijo.ClientID%>").innerText = cellTelefonoFijo.innerText;
                document.getElementById("<%= txtEmail.ClientID%>").innerText = cellCorreoElectronico.innerText;

                /// Controles Tipo Telerik
                $find("<%= txtCUIL.ClientID%>").set_value(cellCUIL.innerText);


                /// Controles Tipo Fecha
                if (cellFechaNacimiento.outerText.trim() != "") {
                    dia = cellFechaNacimiento.outerText.substr(0, 2);
                    mes = parseInt(cellFechaNacimiento.outerText.substr(3, 2)) - 1 + '';
                    año = cellFechaNacimiento.outerText.substr(6);
                    Fecha = new Date(año, mes, dia);
                    $find("<%= txtFechaNacimiento.ClientID%>").set_selectedDate(Fecha);
                }

                if (cellFechaIngreos.outerText.trim() != "") {
                    dia = cellFechaIngreos.outerText.substr(0, 2);
                    mes = parseInt(cellFechaIngreos.outerText.substr(3, 2)) - 1 + '';
                    año = cellFechaIngreos.outerText.substr(6);
                    Fecha = new Date(año, mes, dia);
                    $find("<%= txtFechaIngreso.ClientID%>").set_selectedDate(Fecha);
                }


                /// Controles Tipo Combos
                if (cellTipoDocumento.outerText != "") {
                    var itemTipoDoc = $find("<%= cboTipoDoc.ClientID%>").findItemByText(cellTipoDocumento.outerText.trim());
                    $find("<%= cboTipoDoc.ClientID%>").set_selectedItem(itemTipoDoc);
                    itemTipoDoc.select();
                }
                else {
                    $find("<%= cboTipoDoc.ClientID%>").clearSelection();
                }


                if (cellEstadoCivil.outerText != "") {
                    var itemEstadoCivial = $find("<%= cboEstadoCivil.ClientID%>").findItemByText(cellEstadoCivil.outerText.trim());
                    $find("<%= cboEstadoCivil.ClientID%>").set_selectedItem(itemEstadoCivial);
                    itemEstadoCivial.select();
                }
                else {
                    $find("<%= cboEstadoCivil.ClientID%>").clearSelection();
                }

                if (cellNacionalidad.outerText != "") {
                    var itemNacionalidad = $find("<%= cboNacionalidad.ClientID%>").findItemByText(cellNacionalidad.outerText.trim());
                    $find("<%= cboNacionalidad.ClientID%>").set_selectedItem(itemNacionalidad);
                    itemNacionalidad.select();
                }
                else {
                    $find("<%= cboNacionalidad.ClientID%>").clearSelection();
                }

                if (cellConvenio.outerText != "") {
                    var itemConvenio = $find("<%= cboConvenio.ClientID%>").findItemByText(cellConvenio.outerText.trim());
                    $find("<%= cboConvenio.ClientID%>").set_selectedItem(itemConvenio);
                    itemConvenio.select();
                }
                else {
                    $find("<%= cboConvenio.ClientID%>").clearSelection();
                }

                if (cellProvincia.outerText != "") {
                    var itemProvincia = $find("<%= cboProvincia.ClientID%>").findItemByText(cellProvincia.outerText.trim());
                    $find("<%= cboProvincia.ClientID%>").set_selectedItem(itemProvincia);
                    itemProvincia.select();
                }
                else {
                    $find("<%= cboProvincia.ClientID%>").clearSelection();
                }


                $find("<%=ServerControlWindow1.ClientID %>").set_CollectionDiv('divPrincipal');
                $find("<%=ServerControlWindow1.ClientID %>").ShowWindows('divPrincipal', "Edición: " + cellApellido.innerText + ", " + cellNombre.innerText);
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

                var url = 'ConsultaInformacionSueldos.aspx?IdLegajo=' + Item[0].getDataKeyValue("IdLegajos")
                var name = "RadWindow1";
                var manager = $find("<%=RadWindowManager1.ClientID %>");
                var oWnd = manager.open(url, name);

                //var oWnd = radopen('ConsultaInformacionSueldos.aspx?IdLegajo=' + Item[0].getDataKeyValue("IdLegajos"), 'RadWindow1');
            }
            else {
                radalert("Debe seleccionar un legajo para ver su información de sueldos", 250, 100, "Selección Legajo");
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
    <asp:EntityDataSource ID="EntityDataSourceLegajos" runat="server" ConnectionString="name=EntidadesConosud" ContextTypeName="Entidades.EntidadesConosud" 
        DefaultContainerName="EntidadesConosud" EnableDelete="True" EnableInsert="True"
        EnableUpdate="True" EntitySetName="Legajos" OrderBy="it.Apellido" Include="objEstadoCivil,objNacionalidad,objProvincia,objTipoDocumento,objConvenio"
        Where="it.Apellido LIKE @Apellido + '%' or it.NroDoc LIKE @NroDoc + '%' " EntityTypeFilter=""
        OnSelecting="EntityDataSourceLegajos_Selecting" Select="" AutoPage="true">
        <WhereParameters>
            <asp:ControlParameter ControlID="txtApellidoLegajo" DbType="String" Name="Apellido"
                PropertyName="Text" />
            <asp:ControlParameter ControlID="txtNroDoc" DbType="String" Name="NroDoc" PropertyName="Text" />
        </WhereParameters>
    </asp:EntityDataSource>
    <asp:EntityDataSource ID="EntityDataSourceEstadoCivil" runat="server" ConnectionString="name=EntidadesConosud" ContextTypeName="Entidades.EntidadesConosud" 
        DefaultContainerName="EntidadesConosud" EntitySetName="Clasificacion" Select="it.[IdClasificacion], it.[Descripcion]"
        Where="it.Tipo == @Tipo">
        <WhereParameters>
            <asp:Parameter DefaultValue="Estado Civil" Name="Tipo" DbType="String" />
        </WhereParameters>
    </asp:EntityDataSource>
    <asp:EntityDataSource ID="EntityDataSourceNacionalidad" runat="server" ConnectionString="name=EntidadesConosud" ContextTypeName="Entidades.EntidadesConosud" 
        DefaultContainerName="EntidadesConosud" EntitySetName="Clasificacion" Select="it.[IdClasificacion], it.[Descripcion]"
        Where="it.Tipo == @Tipo">
        <WhereParameters>
            <asp:Parameter DefaultValue="Nacionalidad" Name="Tipo" DbType="String" />
        </WhereParameters>
    </asp:EntityDataSource>
    <asp:EntityDataSource ID="EntityDataSourceProvincia" runat="server" ConnectionString="name=EntidadesConosud" ContextTypeName="Entidades.EntidadesConosud" 
        DefaultContainerName="EntidadesConosud" EntitySetName="Clasificacion" Select="it.[IdClasificacion], it.[Descripcion]"
        Where="it.Tipo == @Tipo">
        <WhereParameters>
            <asp:Parameter DefaultValue="Provincia" Name="Tipo" DbType="String" />
        </WhereParameters>
    </asp:EntityDataSource>
    <asp:EntityDataSource ID="EntityDataSourceTipoDocumento" runat="server" ConnectionString="name=EntidadesConosud" ContextTypeName="Entidades.EntidadesConosud" 
        DefaultContainerName="EntidadesConosud" EntitySetName="Clasificacion" Select="it.[IdClasificacion], it.[Descripcion]"
        Where="it.Tipo == @Tipo">
        <WhereParameters>
            <asp:Parameter DefaultValue="Tipo Documento" Name="Tipo" DbType="String" />
        </WhereParameters>
    </asp:EntityDataSource>
    <asp:EntityDataSource ID="EntityDataSourceConvenio" runat="server" ConnectionString="name=EntidadesConosud" ContextTypeName="Entidades.EntidadesConosud" 
        DefaultContainerName="EntidadesConosud" EntitySetName="Clasificacion" Select="it.[IdClasificacion], it.[Descripcion]"
        Where="it.Tipo == @Tipo">
        <WhereParameters>
            <asp:Parameter DefaultValue="Convenio" Name="Tipo" DbType="String" />
        </WhereParameters>
    </asp:EntityDataSource>
    <table cellpadding="0" cellspacing="5" style="width: 80%">
        <tr>
            <td align="center" style="height: 25px; background: url('images/sprite.gif') 0  -997px repeat-x">
                <asp:Label ID="lblEncabezado" runat="server" Font-Bold="True" Font-Size="14pt" Font-Names="Sans-Serif"
                    ForeColor="#E0D6BE" Text="Gestión de Legajos" Width="378px"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <table style="border: thin solid #843431; background-color: #E0D6BE; font-family: Sans-Serif;
                font-size: 11px; width: 80%; vertical-align: middle">
                <tr>
                    <td valign="middle" align="right">
                        <asp:Label ID="lblLegajo" runat="server" Font-Bold="True" ForeColor="Maroon" Height="22px"
                            Text="Legajo:"></asp:Label>
                    </td>
                    <td valign="middle" align="left">
                        <telerik:RadTextBox ID="txtApellidoLegajo" runat="server" EmptyMessage="Ingrese apellido del legajo a buscar"
                            Skin="Sunset" Width="210px">
                        </telerik:RadTextBox>
                    </td>
                    <td valign="middle" align="left">
                        <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Maroon" Height="22px"
                            Text="NroDoc:"></asp:Label>
                    </td>
                    <td valign="middle" align="left">
                        <telerik:RadTextBox ID="txtNroDoc" runat="server" EmptyMessage="Ingrese Nro Doc del legajo a buscar"
                            Skin="Sunset" Width="210px">
                        </telerik:RadTextBox>
                    </td>
                    <td valign="middle" align="left">
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar Legajo" CommandName="Buscar" SkinID="btnConosudBasic"
                            Mensaje="Buscando Legajos solicitados..." OnClick="btnBuscar_Click" CausesValidation="false" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdPnlGrilla" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" AllowSorting="True"
                ShowStatusBar="True" GridLines="None" Skin="Sunset" AllowAutomaticDeletes="True"
                AllowAutomaticInserts="True" AllowAutomaticUpdates="True" AutoGenerateColumns="False"
                PageSize="10" DataSourceID="EntityDataSourceLegajos" OnItemCommand="RadGrid1_ItemCommand">
                <MasterTableView DataKeyNames="IdLegajos" ClientDataKeyNames="IdLegajos" DataSourceID="EntityDataSourceLegajos"
                    TableLayout="Fixed" CommandItemDisplay="Top" NoMasterRecordsText="No existen registros."
                    HorizontalAlign="NotSet">
                    <CommandItemTemplate>
                        <div style="padding: 5px 5px;">
                            <asp:LinkButton Mensaje="Buscar Legajo...." ID="btnEdit" runat="server" Visible='<%# RadGrid1.EditIndexes.Count == 0 %>'
                                CausesValidation="false" OnClientClick="EditLegajo(); return false;">
                                <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/Edit.gif" />Editar</asp:LinkButton>&nbsp;&nbsp;
                            <asp:LinkButton Mensaje="Preparando par nuevo legajo...." ID="btnInsert" runat="server"
                                CausesValidation="false" Visible='<%# !RadGrid1.MasterTableView.IsItemInserted %>'
                                OnClientClick="InitInsert(); return false;">
                                <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/AddRecord.gif" />Insertar</asp:LinkButton>&nbsp;&nbsp;
                            <asp:LinkButton Mensaje="Eliminando Legajo...." ID="btnDelete" OnClientClick="return blockConfirm('Esta seguro que desea eliminar el legajo seleccionado?', event, 330, 100,'','Legajos');"
                                runat="server" OnClick="btnEliminar_Click" CausesValidation="false">
                                <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/delete_16x16.gif" />Eliminar</asp:LinkButton>&nbsp;&nbsp;
                            <asp:LinkButton CausesValidation="false" Mensaje="Exportando Legajos...." ID="ExportExcel" runat="server"
                                CommandName="ExportLegajos">
                                <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/Excel_16x16.gif" />Excel</asp:LinkButton>&nbsp;&nbsp;
                            <asp:LinkButton CausesValidation="false" Mensaje="Cargan...." ID="btnSueldos" runat="server" OnClientClick="ShowSueldos(); return false;">
                                <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/notepad_16x16.gif" />Info. Sueldos</asp:LinkButton>&nbsp;&nbsp;
                        </div>
                    </CommandItemTemplate>
                    <RowIndicatorColumn>
                        <HeaderStyle Width="20px"></HeaderStyle>
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn>
                        <HeaderStyle Width="20px"></HeaderStyle>
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridTemplateColumn DataField="Apellido" HeaderText="Apellido" SortExpression="Apellido"
                            UniqueName="ApellidoColumn">
                            <ItemTemplate>
                                <asp:Label ID="ApellidoLabel" runat="server" Text='<%# Eval("Apellido") %>' Style="text-transform: capitalize"></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre"
                            UniqueName="NombreColumn">
                            <ItemTemplate>
                                <asp:Label ID="NombreLabel" runat="server" Text='<%# Eval("Nombre").ToString().ToLower() %>'
                                    Style="text-transform: capitalize"></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="NroDoc" HeaderText="NroDoc" SortExpression="NroDoc"
                            UniqueName="NroDocColumn">
                            <ItemTemplate>
                                <asp:Label ID="NroDocLabel" runat="server" Text='<%# Eval("NroDoc") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridDateTimeColumn DataField="FechaNacimiento" DataType="System.DateTime"
                            HeaderText="FechaNacimiento" SortExpression="FechaNacimiento" UniqueName="FechaNacimientoColumn"
                            Display="false" EditFormColumnIndex="0">
                        </telerik:GridDateTimeColumn>
                        <telerik:GridTemplateColumn DataField="CUIL" HeaderText="CUIL" SortExpression="CUIL"
                            UniqueName="CUILColumn">
                            <ItemTemplate>
                              <asp:Label ID="CUILLabel" runat="server"  Text='<%# string.Format("{0:##-########-#}", long.Parse(Eval("CUIL").ToString().Replace("-",""))) %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="Direccion" HeaderText="Direccion" SortExpression="Direccion"
                            UniqueName="DireccionColumn" Display="false" EditFormColumnIndex="0">
                            <ItemTemplate>
                                <asp:TextBox Width="90%" ID="DireccionTextBox" runat="server" Text='<%# Bind("Direccion") %>'></asp:TextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="CodigoPostal" HeaderText="CodigoPostal" SortExpression="CodigoPostal"
                            UniqueName="CodigoPostalColumn" Display="false" EditFormColumnIndex="1">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TelefonoFijo" HeaderText="TelefonoFijo" SortExpression="TelefonoFijo"
                            UniqueName="TelefonoFijoColumn" Display="false" EditFormColumnIndex="1">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CorreoElectronico" HeaderText="CorreoElectronico"
                            SortExpression="CorreoElectronico" UniqueName="CorreoElectronicoColumn" Display="false"
                            EditFormColumnIndex="1">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn DataField="objEstadoCivil" Display="false" EditFormColumnIndex="1"
                            HeaderText="Estado Civil" SortExpression="EstadoCivil" UniqueName="EstadoCivilColumn">
                            <ItemTemplate>
                                <asp:Label ID="EstadoCivilTextBoxGrilla" runat="server" Text='<%# Bind("objEstadoCivil.Descripcion") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="objNacionalidad" Display="false" EditFormColumnIndex="1"
                            HeaderText="Nacionalidad" SortExpression="Nacionalidad" UniqueName="NacionalidadColumn">
                            <ItemTemplate>
                                <asp:Label ID="EstadoCivilTextBox" runat="server" Text='<%# Bind("objNacionalidad.Descripcion") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="objProvincia" Display="false" EditFormColumnIndex="1"
                            HeaderText="Provincia" SortExpression="Provincia" UniqueName="ProvinciaColumn">
                            <ItemTemplate>
                                <asp:Label ID="EstadoCivildfdTextBoxccc" runat="server" Text='<%# Bind("objProvincia.Descripcion") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="objTipoDocumento" Display="false" EditFormColumnIndex="1"
                            HeaderText="Tipo Documento" SortExpression="TipoDocumento" UniqueName="TipoDocumentoColumn">
                            <ItemTemplate>
                                <asp:Label ID="EstadoCivilTextBoxccc" runat="server" Text='<%# Bind("objTipoDocumento.Descripcion") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="objConvenio" Display="false" EditFormColumnIndex="1"
                            HeaderText="Convenio" SortExpression="Convenio" UniqueName="ConvenioColumn">
                            <ItemTemplate>
                                <asp:Label ID="EstadoCiddvilTextBox" runat="server" Text='<%# Bind("objConvenio.Descripcion") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="FechaIngreos" Display="false" EditFormColumnIndex="2"
                            HeaderText="Ingreso" UniqueName="FechaIngreosColumn">
                            <ItemTemplate>
                                <asp:Label ID="FechaIngreso" runat="server" Text='<%# Eval("FechaIngreos") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
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
    <asp:UpdatePanel ID="upEdicion" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <cc1:ServerControlWindow ID="ServerControlWindow1" runat="server" BackColor="WhiteSmoke" WindowColor="Rojo">
                <ContentControls>
                    <div id="divPrincipal" style="height:260px">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="text-align: left;">
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" SkinID="lblConosud" Text="Apellido:"></asp:Label>
                                </td>
                                <td align="left" style="padding-left: 5px; padding-right: 5px">
                                    <asp:TextBox Width="250px" ID="txtApellido" runat="server" Style="text-transform: uppercase"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtApellido"
                                        Display="Dynamic" ErrorMessage="*" runat="server">
                                    </asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" SkinID="lblConosud" Text="Nombre:"></asp:Label>
                                </td>
                                <td style="padding-left: 5px; padding-right: 5px">
                                    <asp:TextBox Width="250px" ID="txtNombre" runat="server" Style="text-transform: uppercase"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txtNombre"
                                        Display="Dynamic" ErrorMessage="*" runat="server">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label14" runat="server" SkinID="lblConosud" Text="TipoDocumento:"></asp:Label>
                                </td>
                                <td style="padding-left: 5px; padding-right: 5px">
                                    <telerik:RadComboBox ID="cboTipoDoc" runat="server" Skin="Sunset" Width="150px" EmptyMessage="Seleccione Tipo Documento"
                                        ZIndex="922000000" DataSourceID="EntityDataSourceTipoDocumento" DataValueField="IdClasificacion"
                                        DataTextField="Descripcion" AllowCustomText="true">
                                    </telerik:RadComboBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" SkinID="lblConosud" Text="Nro:"></asp:Label>
                                </td>
                                <td style="padding-left: 5px; padding-right: 5px">
                                    <asp:UpdatePanel ID="upNroDoc" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:TextBox Width="100px" ID="txtNroDocEdit" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtNroDocEdit"
                                                Display="Dynamic" ErrorMessage="*" runat="server">
                                            </asp:RequiredFieldValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
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
                                <td>
                                    <asp:Label ID="Label5" runat="server" SkinID="lblConosud" Text="Fecha Nacimiento:"></asp:Label>
                                </td>
                                <td style="padding-left: 5px; padding-right: 5px">
                                    <telerik:RadDatePicker ID="txtFechaNacimiento" MinDate="1950/1/1" runat="server"
                                        ZIndex="922000000">
                                        <DateInput InvalidStyleDuration="100">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label7" runat="server" SkinID="lblConosud" Text="Direccion:"></asp:Label>
                                </td>
                                <td style="padding-left: 5px; padding-right: 5px">
                                    <asp:TextBox Width="250px" ID="txtDireccion" runat="server"></asp:TextBox>
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
                                    <telerik:RadComboBox ID="cboEstadoCivil" runat="server" Skin="Sunset" Width="150px"
                                        DataSourceID="EntityDataSourceEstadoCivil" ZIndex="922000000" EmptyMessage="Seleccione Estado Civil"
                                        DataValueField="IdClasificacion" DataTextField="Descripcion" AllowCustomText="true">
                                    </telerik:RadComboBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label12" runat="server" SkinID="lblConosud" Text="Nacionalidad:"></asp:Label>
                                </td>
                                <td style="padding-left: 5px; padding-right: 5px">
                                    <telerik:RadComboBox ID="cboNacionalidad" runat="server" Skin="Sunset" Width="150px"
                                        DataSourceID="EntityDataSourceNacionalidad" ZIndex="922000000" DataValueField="IdClasificacion"
                                        DataTextField="Descripcion" EmptyMessage="Seleccione la Nacionalidad" AllowCustomText="true">
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label13" runat="server" SkinID="lblConosud" Text="Provincia:"></asp:Label>
                                </td>
                                <td style="padding-left: 5px; padding-right: 5px">
                                    <telerik:RadComboBox ID="cboProvincia" runat="server" Skin="Sunset" Width="150px"
                                        ZIndex="922000000" DataSourceID="EntityDataSourceProvincia" DataValueField="IdClasificacion"
                                        DataTextField="Descripcion" EmptyMessage="Seleccione la Provincia" AllowCustomText="true">
                                    </telerik:RadComboBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label15" runat="server" SkinID="lblConosud" Text="Convenio:"></asp:Label>
                                </td>
                                <td style="padding-left: 5px; padding-right: 5px">
                                    <telerik:RadComboBox ID="cboConvenio" runat="server" Skin="Sunset" Width="150px"
                                        ZIndex="922000000" DataSourceID="EntityDataSourceConvenio" DataValueField="IdClasificacion"
                                        DataTextField="Descripcion" EmptyMessage="Seleccione el convenio" AllowCustomText="true">
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label16" runat="server" SkinID="lblConosud" Text="Fecha Ingreos:"></asp:Label>
                                </td>
                                <td colspan="3" align="left" style="padding-left: 5px; padding-right: 5px">
                                    <telerik:RadDatePicker ID="txtFechaIngreso" MinDate="1950/1/1" runat="server" ZIndex="922000000">
                                        <DateInput InvalidStyleDuration="100">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
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
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" ClientEvents-OnResponseEnd="ResponseEnd"
        ClientEvents-OnRequestStart="requestStart1" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <ClientEvents OnRequestStart="requestStart1"></ClientEvents>
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
