<%@ Page Title="" Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="ABMClasificaciones.aspx.cs" Inherits="ABMClasificaciones" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadWindowManager ID="RadWindowManager1" ShowContentDuringLoad="false" VisibleStatusbar="false"
        ReloadOnShow="false" runat="server" Skin="Default">
    </telerik:RadWindowManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <table cellpadding="0" cellspacing="5" style="width: 80%">
        <tr>
            <td align="center" style="height: 25px; background: url('images/sprite.gif') 0  -997px repeat-x">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="14pt" Font-Names="Sans-Serif"
                    ForeColor="#E0D6BE" Text="Gestión de Parámetros" Width="378px"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <table style="border: thin solid #843431; background-color: #E0D6BE; font-family: Sans-Serif;
                font-size: 11px; width: 80%; vertical-align: middle">
                <tr>
                    <td valign="middle" align="right">
                        <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Maroon" Height="22px"
                            Text="Tipo Parámetro:"></asp:Label>
                    </td>
                    <td valign="middle" align="right" width="20%">
                        <telerik:RadComboBox runat="server" ID="cboTipoParametro" AllowCustomText="true" MarkFirstMatch="true"
                            DataTextField="Tipo" DataValueField="Tipo" Skin="Sunset" Width="255px"
                            EmptyMessage="Seleccione un tipo de Parámetro">
                        </telerik:RadComboBox>
                    </td>
                    <td valign="middle" align="left">
                        <asp:Button ID="btnBuscar" runat="server" CommandName="Buscar" SkinID="btnConosudBasic"
                            Text="Buscar" OnClick="btnBuscar_Click" Mensaje="Buscando Elementos..." />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="Table2" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
        border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #E0D6BE;
        font-family: Sans-Serif; font-size: 11px;" width="90%">
        <tr>
            <td align="left">
                <asp:UpdatePanel ID="updpnlGrilla" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" AllowSorting="True"
                            ShowStatusBar="True" GridLines="None" Skin="Sunset" 
                            AllowAutomaticDeletes="false"
                            AllowAutomaticInserts="false" 
                            AllowAutomaticUpdates="false" 
                            AutoGenerateColumns="False"
                            OnDataBound="RadGrid1_DataBound"
                            OnNeedDataSource ="RadGrid1_NeedDataSource"
                            onitemcommand="RadGrid1_ItemCommand"  
                            OnSelectedIndexChanged="RadGrid1_SelectedIndexChanged" 
                            OnItemDataBound = "RadGrid1_ItemDataBound">
                            <MasterTableView DataKeyNames="IdClasificacion" 
                                CommandItemDisplay="Top" EditMode="PopUp" NoMasterRecordsText="No existen registros."
                                HorizontalAlign="NotSet" GroupLoadMode="Client" PageSize="100" GroupsDefaultExpanded="false" >
                                <CommandItemTemplate>
                                    <div style="padding: 5px 5px;">
                                        <asp:LinkButton Mensaje="Editando Elemento..." ID="btnEdit" runat="server" CommandName="EditSelected"
                                            Visible='<%# RadGrid1.EditIndexes.Count == 0 %>'><img style="border:0px;vertical-align:middle;" alt="" src="Images/Edit.gif" />Editar</asp:LinkButton>&nbsp;&nbsp;
                                        <asp:LinkButton Mensaje="Preparando Para Nuevo Elemento..." ID="btnInsert" runat="server"
                                            CommandName="InitInsert" Visible='<%# !RadGrid1.MasterTableView.IsItemInserted %>'
                                            OnClick="LinkButton2_Click"><img style="border:0px;vertical-align:middle;" alt="" src="Images/AddRecord.gif" />Insertar</asp:LinkButton>&nbsp;&nbsp;
                                        <asp:LinkButton Mensaje="Eliminando Elemento..." ID="btnDelete" OnClientClick="return blockConfirm('Desea eliminar esta Clasificacion ?', event, 330, 100,'','Clasificaciones');"
                                            runat="server" CommandName="DeleteSelected"><img style="border:0px;vertical-align:middle;" alt="" src="Images/delete_16x16.gif" />Eliminar</asp:LinkButton>&nbsp;&nbsp;
                                    </div>
                                </CommandItemTemplate>
                                <GroupByExpressions>
                                    <telerik:GridGroupByExpression>
                                        <SelectFields>
                                            <telerik:GridGroupByField  FieldName="Tipo" />
                                        </SelectFields>
                                        <GroupByFields>
                                            <telerik:GridGroupByField  FieldName="Tipo" />
                                        </GroupByFields>
                                    </telerik:GridGroupByExpression>
                                </GroupByExpressions>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="Descripcion" HeaderText="Descripcion" SortExpression="Descripcion"
                                        UniqueName="Descripcion" EditFormColumnIndex="0">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Codigo" HeaderText="Codigo" SortExpression="Codigo"
                                        UniqueName="Codigo" EditFormColumnIndex="0">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridTemplateColumn HeaderText="Tipo" 
                                        UniqueName="TemplateColumnTipo">
                                        <ItemTemplate>
                                            <asp:Label ID="LabelTipo" runat="server" Text='<%# Eval("Tipo") %>'></asp:Label>                                         
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox ID="cboTipo" Runat="server" 
                                                DataTextField="Tipo" 
                                                DataValueField="Tipo" Skin="Sunset">
                                            </telerik:RadComboBox>
                                        </EditItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left"/>
                                    </telerik:GridTemplateColumn>      


                                    
                                </Columns>
                                <EditFormSettings ColumnNumber="1" CaptionDataField="Descripcion" CaptionFormatString="Editar el Parámetro: {0}">
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
                            </MasterTableView>
                            <ValidationSettings CommandsToValidate="PerformInsert,Update" />
                            <ClientSettings>
                                <Selecting AllowRowSelect="True" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
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
