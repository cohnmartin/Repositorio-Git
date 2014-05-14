<%@ Page Title="" Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="ABMRoles2.aspx.cs" Inherits="ABMRoles2" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="ConosudSkin/ToolTip.ConosudSkin.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        function ShowOpcionesMenu(img) {
            var Items = $find("<%= grillaRoles.ClientID %>").get_masterTableView().get_dataItems();
            if (Items.length > 0) {

                var url = 'AsignacionPaginaMenu.aspx?IdRol=' + Items[img.parentElement.parentElement.rowIndex - 2].getDataKeyValue("IdSegRol");
                var name = "RadWindow1";
                var manager = $find("<%=RadWindowManager1.ClientID %>");
                var oWnd = manager.open(url, name);

                //var oWnd = radopen('AsignacionPaginaMenu.aspx?IdRol=' + Items[img.parentElement.parentElement.rowIndex-2].getDataKeyValue("IdSegRol"), 'RadWindow1');
            }
        }

 
    </script>
    <style type="text/css">
        .MyImageButton
        {
            cursor: hand;
        }
    </style>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Sunset" VisibleTitlebar="true"
        Style="z-index: 100000000" Title="Opciones del Menu">
        <Windows>
            <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close" Width="650" Height="440"
                Modal="true" NavigateUrl="AsignacionPaginaMenu.aspx" VisibleTitlebar="true" Style="z-index: 100000000"
                Title="Indique cuales son las opciones de menu habilitadas" ReloadOnShow="true"
                VisibleStatusbar="false" ShowContentDuringLoad="false" Skin="Sunset">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <asp:EntityDataSource ID="EntityDataSourceRol" runat="server" ConnectionString="name=EntidadesConosud"
        ContextTypeName="Entidades.EntidadesConosud" DefaultContainerName="EntidadesConosud"
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" EntitySetName="SegRol">
    </asp:EntityDataSource>
    <table cellpadding="0" cellspacing="5" style="width: 80%">
        <tr>
            <td align="center" style="height: 25px; background: url('images/sprite.gif') 0  -997px repeat-x">
                <asp:Label ID="lblEncabezado" runat="server" Font-Bold="True" Font-Size="14pt" Font-Names="Sans-Serif"
                    ForeColor="#E0D6BE" Text="Gestión de Roles" Width="378px"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdPnlGrilla" UpdateMode="Conditional" runat="server">
        <contenttemplate>
            <telerik:RadGrid ID="grillaRoles" runat="server" AllowPaging="True" AllowSorting="True"
                ShowStatusBar="True" GridLines="None" Skin="Sunset" AllowAutomaticDeletes="True"
                AllowAutomaticInserts="True" AllowAutomaticUpdates="True" 
                AutoGenerateColumns="False" PageSize="10" 
                DataSourceID="EntityDataSourceRol" 
                Width="70%">
                
                <MasterTableView DataKeyNames="IdSegRol" ClientDataKeyNames="IdSegRol" DataSourceID="EntityDataSourceRol" TableLayout="Fixed"
                    CommandItemDisplay="Top" EditMode="PopUp" NoMasterRecordsText="No existen registros."
                    HorizontalAlign="NotSet">
                    <CommandItemTemplate>
                        <div style="padding: 5px 5px;">
                            <asp:LinkButton Mensaje="Buscando Rol...."  ID="btnEdit" runat="server" CommandName="EditSelected" Visible='<%# grillaRoles.EditIndexes.Count == 0 %>'>
                                <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/Edit.gif" />Editar</asp:LinkButton>&nbsp;&nbsp;
                            <asp:LinkButton Mensaje="Preparando Para Nuevo Rol...." ID="btnInsert" runat="server" CommandName="InitInsert" Visible='<%# !grillaRoles.MasterTableView.IsItemInserted %>'>
                                <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/AddRecord.gif" />Insertar</asp:LinkButton>&nbsp;&nbsp;
                            <asp:LinkButton Mensaje="Eliminando Rol...." ID="btnDelete" OnClientClick="return blockConfirm('Esta seguro que desea eliminar el rol seleccionado?', event, 330, 100,'','Legajos');"
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
                        <telerik:GridSortExpression FieldName="Descripcion" SortOrder="Ascending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn DataField="Descripcion" HeaderText="Nombre del Rol" SortExpression="Descripcion"
                            UniqueName="DescripcionColum" ShowSortIcon="true" >
                            <EditItemTemplate>
                                <asp:TextBox ID="LoginTextBox" runat="server" Text='<%# Bind("Descripcion") %>' style="text-transform:uppercase" Width="250px"></asp:TextBox>
                                <span style="color: Red">*</span>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="LoginTextBox"
                                    ErrorMessage="Este campo es requerido" runat="server">
                                </asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="ApellidoLabel" runat="server" Text='<%# Eval("Descripcion") %>' style="text-transform:uppercase"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="250px" HorizontalAlign="Center" />
                            <ItemStyle Width="250px" HorizontalAlign="Left" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Acceso al Menu" UniqueName="MenuColumn">
                            <ItemTemplate>
                                <asp:ImageButton Style="cursor: hand; text-align: center" ID="imgSubContratistas"
                                    Mensaje="Cargando Opciones del Menu.." runat="server" ImageUrl="~/images/SubContratistas.gif"
                                    OnClientClick="ShowOpcionesMenu(this); return false;" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" Width="45px" />
                        </telerik:GridTemplateColumn>
                                    
                               
                    </Columns>
                    
                    <EditFormSettings ColumnNumber="1" CaptionDataField="Descripcion" CaptionFormatString="Editando el Rol: {0}">
                        <FormTableItemStyle Wrap="False" HorizontalAlign="Left" ></FormTableItemStyle>
                        <FormTableAlternatingItemStyle Wrap="False" HorizontalAlign="Left" ></FormTableAlternatingItemStyle>
                        
                        <FormCaptionStyle HorizontalAlign="Center" Width="100%"></FormCaptionStyle>
                        <FormMainTableStyle GridLines="Both" CellSpacing="0" CellPadding="3" BackColor="White"
                            Width="100%" HorizontalAlign="Left" />
                        <FormTableStyle CellSpacing="0" CellPadding="2" Width="100%" 
                            Height="110px" BackColor="White" HorizontalAlign="Left" />
                        <FormStyle Width="100%" BackColor="#eef2ea"  ></FormStyle>
                        
                        <EditColumn ButtonType="ImageButton" InsertText="Insertar" UpdateText="Actualizar"
                            UniqueName="EditCommandColumn1" CancelText="Cancelar">
                        </EditColumn>
                        <FormTableButtonRowStyle HorizontalAlign="Right"></FormTableButtonRowStyle>
                        <PopUpSettings ScrollBars="Auto" Modal="true" Width="60%"  />
                    </EditFormSettings>
                </MasterTableView>
                <ClientSettings>
                    <Selecting AllowRowSelect="True" />
                </ClientSettings>
            </telerik:RadGrid>
           

        </contenttemplate>
    </asp:UpdatePanel>
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
</asp:Content>
