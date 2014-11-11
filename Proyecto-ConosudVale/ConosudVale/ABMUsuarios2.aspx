<%@ Page Title="" Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="ABMUsuarios2.aspx.cs" Inherits="ABMUsuarios2" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Assembly="ControlsAjaxNotti" Namespace="ControlsAjaxNotti" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="ConosudSkin/ToolTip.ConosudSkin.css" rel="stylesheet" type="text/css" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8">
    <script src="Scripts/ui-select/angular.js" type="text/javascript"></script>
    <script src="Scripts/ui-select/angular-sanitize.js" type="text/javascript"></script>
    <script src="Scripts/ui-select/select.js" type="text/javascript"></script>
    <script src="Scripts/controllers/controller_Usuarios.js" type="text/javascript"></script>
    <script type="text/javascript">

        function ShowRolesUsuario(img) {
            /// ESTO ES PARA LA VERSION CON TELERIK
            var Items = $find("<%= grillaUsuarios.ClientID %>").get_masterTableView().get_dataItems();
            if (Items.length > 0)
                var oWnd = radopen('AsignacionRoles.aspx?IdUsuario=' + Items[img.parentElement.parentElement.rowIndex - 2].getDataKeyValue("IdSegUsuario"), 'RadWindow1');
            
            /// ESTO ES PARA LA VERSION COMPLETA EN ANGULARJS
            //var oWnd = radopen('AsignacionRoles.aspx?IdUsuario=' + img.value, 'RadWindow1');
            
        }

        function ShowSegcontexto(img) {
            /// ESTO ES PARA LA VERSION CON TELERIK
            var Items = $find("<%= grillaUsuarios.ClientID %>").get_masterTableView().get_dataItems();
            if (Items.length > 0)
                var oWnd = radopen('AsignacionSegContexto.aspx?IdUsuario=' + Items[img.parentElement.parentElement.rowIndex - 2].getDataKeyValue("IdSegUsuario"), 'RadWindow2');
            
            /// ESTO ES PARA LA VERSION COMPLETA EN ANGULARJS
            //var oWnd = radopen('AsignacionSegContexto.aspx?IdUsuario=' + img.value, 'RadWindow2');
        }

        var Constants = {
            usuarioEdicion: '',
            controlCombo: 'cboContratos'
        };
    </script>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Sunset" VisibleTitlebar="true"
        Style="z-index: 100000000" Title="Sub Contratistas">
        <Windows>
            <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close" Width="650" Height="440"
                Modal="true" NavigateUrl="AsignacionPaginaMenu.aspx" VisibleTitlebar="true" Style="z-index: 100000000"
                Title="Indique cuales son los roles habilitados" ReloadOnShow="true" VisibleStatusbar="false"
                ShowContentDuringLoad="false" Skin="Sunset">
            </telerik:RadWindow>
            <telerik:RadWindow ID="RadWindow2" runat="server" Behaviors="Close" Width="850" Height="540"
                Modal="true" NavigateUrl="AsignacionPaginaMenu.aspx" VisibleTitlebar="true" Style="z-index: 100000000"
                Title="Asignacion Contratos" ReloadOnShow="true" VisibleStatusbar="false" ShowContentDuringLoad="false"
                Skin="Sunset">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <cc1:ServerControlWindow ID="ServerControlContratos" runat="server" BackColor="WhiteSmoke"
        WindowColor="Rojo">
        <ContentControls>
        </ContentControls>
    </cc1:ServerControlWindow>
    <table cellpadding="0" cellspacing="5" style="width: 80%">
        <tr>
            <td align="center" style="height: 25px; background: url('images/sprite.gif') 0  -997px repeat-x">
                <asp:Label ID="lblEncabezado" runat="server" Font-Bold="True" Font-Size="14pt" Font-Names="Sans-Serif"
                    ForeColor="#E0D6BE" Text="Gestión de Usuarios" Width="378px"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdPnlGrilla" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <telerik:RadGrid ID="grillaUsuarios" runat="server" AllowPaging="True" AllowSorting="True"
                ShowStatusBar="True" GridLines="None" Skin="Sunset" AllowAutomaticDeletes="false"
                AllowAutomaticInserts="false" AllowAutomaticUpdates="false" AutoGenerateColumns="False"
                OnDataBound="grillaUsuarios_DataBound" OnNeedDataSource="grillaUsuarios_NeedDataSource"
                OnItemCommand="grillaUsuarios_ItemCommand" OnSelectedIndexChanged="grillaUsuarios_SelectedIndexChanged"
                OnItemDataBound="grillaUsuarios_ItemDataBound" PageSize="10" Width="80%">
                <MasterTableView DataKeyNames="IdSegUsuario" ClientDataKeyNames="IdSegUsuario" TableLayout="Fixed"
                    CommandItemDisplay="Top" EditMode="PopUp" NoMasterRecordsText="No existen registros."
                    HorizontalAlign="NotSet">
                    <CommandItemTemplate>
                        <div style="padding: 5px 5px;">
                            <asp:LinkButton Mensaje="Buscando Usuario...." ID="btnEdit" runat="server" CommandName="EditSelected"
                                Visible='<%# grillaUsuarios.EditIndexes.Count == 0 %>'>
                                <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/Edit.gif" />Editar</asp:LinkButton>&nbsp;&nbsp;
                            <asp:LinkButton Mensaje="Preparando Para Nuevo Usuario...." ID="btnInsert" runat="server"
                                CommandName="InitInsert" Visible='<%# !grillaUsuarios.MasterTableView.IsItemInserted %>'>
                                <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/AddRecord.gif" />Insertar</asp:LinkButton>&nbsp;&nbsp;
                            <asp:LinkButton Mensaje="Eliminando Usuario...." ID="btnDelete" OnClientClick="return blockConfirm('Esta seguro que desea eliminar el usuario seleccionado?', event, 330, 100,'','Legajos');"
                                runat="server" CommandName="DeleteSelected">
                                <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/delete_16x16.gif" />Eliminar</asp:LinkButton>&nbsp;&nbsp;
                        </div>
                    </CommandItemTemplate>
                    <RowIndicatorColumn>
                        <HeaderStyle Width="20px"></HeaderStyle>
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn>
                        <HeaderStyle Width="20px"></HeaderStyle>
                    </ExpandCollapseColumn>
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="Login" SortOrder="Ascending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn DataField="Login" HeaderText="Login" SortExpression="Login"
                            UniqueName="LoginColum" ShowSortIcon="true">
                            <EditItemTemplate>
                                <asp:TextBox ID="LoginTextBox" runat="server" Text='<%# Bind("Login") %>' Style="text-transform: uppercase"
                                    Width="180px"></asp:TextBox>
                                <span style="color: Red">*</span>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="LoginTextBox"
                                    ErrorMessage="Este campo es requerido" runat="server">
                                </asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="ApellidoLabel" runat="server" Text='<%# Eval("Login") %>' Style="text-transform: uppercase"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="180px" HorizontalAlign="Center" />
                            <ItemStyle Width="180px" HorizontalAlign="Left" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn DataField="Password" HeaderText="Password" SortExpression="Password"
                            UniqueName="PasswordColum">
                            <EditItemTemplate>
                                <asp:TextBox ID="PasswordTextBox" runat="server" Text='<%# Bind("Password")%>' Style="text-transform: capitalize"></asp:TextBox>
                                <span style="color: Red">*</span>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="PasswordTextBox"
                                    ErrorMessage="Este campo es requerido" runat="server">
                                </asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="NombreLabel" runat="server" Text='<%# Eval("Password")%>' Style="text-transform: capitalize"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="100px" HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Empresa Asiganada" UniqueName="TemplateColumnEmpresa">
                            <ItemTemplate>
                                <asp:Label ID="LabelTipo" runat="server" Text='<%# Eval("Empresa.RazonSocial") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="cboEmpresa" runat="server" AllowCustomText="true" MarkFirstMatch="true"
                                    DataTextField="RazonSocial" Width="80%" ZIndex="9000000" DataValueField="IdEmpresa"
                                    Skin="Sunset">
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Roles" UniqueName="MenuColumn">
                            <ItemTemplate>
                                <asp:ImageButton Style="cursor: hand; text-align: center" ID="imgSubContratistas"
                                    Mensaje="Cargando Opciones de Roles.." runat="server" ImageUrl="~/images/SubContratistas.gif"
                                    OnClientClick="ShowRolesUsuario(this); return false;" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="45px" />
                            <HeaderStyle HorizontalAlign="Center" Width="45px" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Contratos" UniqueName="MenuColumn22">
                            <ItemTemplate>
                                <asp:ImageButton Style="cursor: hand; text-align: center" ID="imgSubContratistasff"
                                    Mensaje="Cargando Conrtatos Asignados.." runat="server" ImageUrl="~/images/menusegu.gif"
                                    OnClientClick="ShowSegcontexto(this);return false;" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" Width="85px" />
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <EditFormSettings ColumnNumber="1" CaptionDataField="Login" CaptionFormatString="Editar el usuario: {0}">
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
                        <PopUpSettings ScrollBars="Auto" Modal="true" Width="60%" />
                    </EditFormSettings>
                </MasterTableView>
                <ClientSettings>
                    <Selecting AllowRowSelect="True" />
                </ClientSettings>
            </telerik:RadGrid>
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
    <div id="ng-app" ng-app="myApp" ng-controller="controller_Usuarios" style="display:none">
        <table id="tblContratosAsignados" width="95%" class="TSunset" border="0" cellpadding="0"
            cellspacing="0">
            <thead>
                <tr>
                    <th class="Theader">
                        <input id="filtroLogin" type="text" ng-model="textSearch1" style="width:98%" />
                    </th>
                    <th class="Theader">
                         <input id="filtroPas" type="text" ng-model="textSearch2" style="width:98%" />
                    </th>
                    <th class="Theader">
                         <input id="filtroEmp" type="text" ng-model="textSearch3" style="width:98%" />
                    </th>
                    <th class="Theader">
                        &nbsp;
                    </th>
                     <th class="Theader">
                        &nbsp;
                    </th>
                </tr>
                <tr>
                    <th class="Theader">
                        Login
                    </th>
                    <th class="Theader">
                        passsword
                    </th>
                    <th class="Theader">
                        Empresa
                    </th>
                    <th class="Theader">
                        &nbsp;
                    </th>
                    <th class="Theader">
                        &nbsp;
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr class="trDatos" ng-repeat="usuario in Usuarios  | filterMultiple : {Login:textSearch1,Password:textSearch2,Empresa:textSearch3} ">
                    <td class="tdSimple" align="left" style="width: 250px">
                        {{usuario.Login}}
                    </td>
                    <td class="tdSimple" align="center">
                        {{usuario.Password}}
                    </td>
                    <td class="tdSimple" align="center" style="width: 350px; vertical-align: bottom">
                        {{usuario.Empresa}}
                    </td>
                    <td class="tdSimple" align="center" style="width: 25px; vertical-align: bottom">
                        <img src="Images/SubContratistas.gif" style="cursor: pointer" title="Eliminar acceso al contrato"
                            onclick="ShowRolesUsuario(this); return false;" value="{{usuario.IdSegUsuario}}"  />
                    </td>
                    <td class="tdSimple" align="center" style="width: 25px; vertical-align: bottom">
                        <img src="Images/menusegu.gif" style="cursor: pointer" title="Eliminar acceso al contrato"
                            onclick="ShowSegcontexto(this); return false;" value="{{usuario.IdSegUsuario}}"  />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
