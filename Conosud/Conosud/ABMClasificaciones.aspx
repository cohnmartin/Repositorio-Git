<%@ Page Title="" Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true"
    CodeFile="ABMClasificaciones.aspx.cs" Inherits="ABMClasificaciones" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <telerik:RadWindowManager ID="RadWindowManager1" ShowContentDuringLoad="false" VisibleStatusbar="false"
        ReloadOnShow="false" runat="server" Skin="Default">
    </telerik:RadWindowManager>

    <asp:EntityDataSource ID="EntityDataSourceClasificaciones" runat="server" ConnectionString="name=EntidadesConosud"
        DefaultContainerName="EntidadesConosud" EnableDelete="True" EnableInsert="True"  ContextTypeName="Entidades.EntidadesConosud" 
        EnableUpdate="True" EntitySetName="Clasificacion" Where="it.Tipo = @TipoParametro "
        EntityTypeFilter="" Select="" OnSelecting="EntityDataSourceClasificaciones_Selecting">
        <WhereParameters>
            <asp:ControlParameter ControlID="cboTipoParametro" DbType="String"  Name="TipoParametro"
                PropertyName="Text" DefaultValue="" />
        </WhereParameters>
    </asp:EntityDataSource>
    
    <asp:EntityDataSource ID="EntityDataSourceTipos" runat="server" ConnectionString="name=EntidadesConosud" ContextTypeName="Entidades.EntidadesConosud" 
        DefaultContainerName="EntidadesConosud" EntitySetName="Clasificacion" Select="distinct it.[Tipo]">
    </asp:EntityDataSource>
    
     <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" >
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
                    <td valign="middle" align="right" >
                        <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Maroon" Height="22px"
                            Text="Tipo Parámetro:" ></asp:Label>
                    </td>
                    <td valign="middle" align="right" width="20%">
                        <telerik:RadComboBox runat="server" ID="cboTipoParametro" DataSourceID="EntityDataSourceTipos" 
                        DataTextField="Tipo" DataValueField="Tipo"  Skin="Sunset" Width="255px" AppendDataBoundItems="true" 
                        EmptyMessage="Seleccione un tipo de Parámetro">
                        <Items>
                            <telerik:RadComboBoxItem Text="" Value="" Selected="true" />
                        </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td valign="middle" align="left" >
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
            <td align="center">
                <asp:UpdatePanel ID="updpnlGrilla" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" AllowSorting="True"
                            ShowStatusBar="True" GridLines="None" Skin="Sunset" AllowAutomaticDeletes="True"
                            AllowAutomaticInserts="True" AllowAutomaticUpdates="True" AutoGenerateColumns="False"
                            DataSourceID="EntityDataSourceClasificaciones" OnSelectedIndexChanged="RadGrid1_SelectedIndexChanged">
                            <MasterTableView DataKeyNames="IdClasificacion" DataSourceID="EntityDataSourceClasificaciones"
                                CommandItemDisplay="Top" EditMode="PopUp" NoMasterRecordsText="No existen registros."
                                HorizontalAlign="NotSet">
                                <CommandItemTemplate>
                                    <div style="padding: 5px 5px;">
                                        <asp:LinkButton Mensaje="Editando Elemento..." ID="btnEdit" runat="server" CommandName="EditSelected" Visible='<%# RadGrid1.EditIndexes.Count == 0 %>'><img style="border:0px;vertical-align:middle;" alt="" src="Images/Edit.gif" />Editar</asp:LinkButton>&nbsp;&nbsp;
                                        <asp:LinkButton Mensaje="Preparando Para Nuevo Elemento..." ID="btnInsert" runat="server" CommandName="InitInsert" Visible='<%# !RadGrid1.MasterTableView.IsItemInserted %>'
                                            OnClick="LinkButton2_Click"><img style="border:0px;vertical-align:middle;" alt="" src="Images/AddRecord.gif" />Insertar</asp:LinkButton>&nbsp;&nbsp;
                                        <asp:LinkButton Mensaje="Eliminando Elemento..." ID="btnDelete" OnClientClick="return blockConfirm('Desea eliminar esta Clasificacion ?', event, 330, 100,'','Clasificaciones');"
                                            runat="server" CommandName="DeleteSelected"><img style="border:0px;vertical-align:middle;" alt="" src="Images/delete_16x16.gif" />Eliminar</asp:LinkButton>&nbsp;&nbsp;
                                    </div>
                                </CommandItemTemplate>
                                <RowIndicatorColumn>
                                    <HeaderStyle Width="20px"></HeaderStyle>
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn>
                                    <HeaderStyle Width="20px"></HeaderStyle>
                                </ExpandCollapseColumn>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="Descripcion" HeaderText="Descripcion" SortExpression="Descripcion"
                                        UniqueName="Descripcion">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Codigo" HeaderText="Codigo" SortExpression="Codigo"
                                        UniqueName="Codigo">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridDropDownColumn DataField="Tipo" DataSourceID="EntityDataSourceTipos"
                                        HeaderText="Tipo" ListTextField="Tipo" ListValueField="Tipo" UniqueName="Tipo">
                                    </telerik:GridDropDownColumn>
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
