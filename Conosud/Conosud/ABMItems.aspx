<%@ Page Title="" Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true"
    CodeFile="ABMItems.aspx.cs" Inherits="ABMItems" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="ConosudSkin/ToolTip.ConosudSkin.css" rel="stylesheet" type="text/css" />
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="50">
        <ProgressTemplate >
            <div id="divBloq1"  >
            </div>
            <div  class="processMessageTooltip">
                <table border="0" cellpadding="0" cellspacing="0" style="height: 62px;">
                    <tr>
                        <td align="center">
                            <img alt="a" src="Images/LoadingSunset.gif" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <div id="divTituloCarga"  style="font-weight:bold;font-family:Tahoma;font-size:12px;color:White;vertical-align:middle" >
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ProgressTemplate>
        
    </asp:UpdateProgress>
    
    <telerik:RadWindowManager ID="RadWindowManager1" ShowContentDuringLoad="false" VisibleStatusbar="false"
        ReloadOnShow="false" runat="server" Skin="Default">
    </telerik:RadWindowManager>
    
    <asp:EntityDataSource ID="EntityDataSourceCat" runat="server" ConnectionString="name=EntidadesConosud" ContextTypeName="Entidades.EntidadesConosud" 
        DefaultContainerName="EntidadesConosud" EntitySetName="CategoriasItems" Select="it.[IdCategoria], it.[Nombre]">
    </asp:EntityDataSource>
    <asp:EntityDataSource ID="EntityDataSourceItems" runat="server" ConnectionString="name=EntidadesConosud" ContextTypeName="Entidades.EntidadesConosud" 
        DefaultContainerName="EntidadesConosud" EnableDelete="True" EnableInsert="True"
        EnableUpdate="True" EntitySetName="Plantilla" Include="CategoriasItems" Where="it.Descripcion LIKE @Apellido + '%'"
        EntityTypeFilter="" Select="" 
        OnSelecting="EntityDataSourceItems_Selecting" 
        onupdating="EntityDataSourceItems_Updating">
        <WhereParameters>
            <asp:ControlParameter ControlID="txtApellidoLegajo" DbType="String" Name="Apellido"
                PropertyName="Text" />
        </WhereParameters>
    </asp:EntityDataSource>
    
    <asp:EntityDataSource ID="EntityDataSourceRoles" runat="server" ConnectionString="name=EntidadesConosud" ContextTypeName="Entidades.EntidadesConosud" 
        DefaultContainerName="EntidadesConosud" EntitySetName="SegRol" Select="it.[IdSegRol], it.[Descripcion]">
    </asp:EntityDataSource>
    
    
    <table cellpadding="0" cellspacing="5" style="width: 80%">
        <tr>
            <td align="center" style="height: 25px; background: url('images/sprite.gif') 0  -997px repeat-x">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="14pt" Font-Names="Sans-Serif"
                    ForeColor="#E0D6BE" Text="Gestión de Items" Width="378px"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <table style="border: thin solid #843431; background-color: #E0D6BE; font-family: Sans-Serif;
                font-size: 11px; width: 80%; vertical-align: middle">
                <tr>
                    <td valign="middle" align="right" width="30%">
                        <asp:Label ID="lblLegajo" runat="server" Font-Bold="True" ForeColor="Maroon" Height="22px"
                            Text="Items:" Width="79px"></asp:Label>
                    </td>
                    <td valign="middle" align="right" width="20%">
                        <telerik:RadTextBox ID="txtApellidoLegajo" runat="server" EmptyMessage="Ingrese Items a buscar"
                            Skin="Sunset" Width="255px">
                        </telerik:RadTextBox>
                    </td>
                    <td valign="middle" align="left" width="50%">
                        <asp:Button ID="btnBuscar" Mensaje="Buscando Items...." runat="server" CommandName="Buscar" SkinID="btnConosudBasic"
                            Text="Buscar" OnClick="btnBuscar_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    
     <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"  >
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    
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
                            DataSourceID="EntityDataSourceItems" 
                            OnSelectedIndexChanged="RadGrid1_SelectedIndexChanged" 
                            OnItemDataBound = "RadGrid1_ItemDataBound"
                            OnItemCommand = "RadGrid1_ItemCommand">
                            <MasterTableView DataKeyNames="IdPlantilla" DataSourceID="EntityDataSourceItems"
                                CommandItemDisplay="Top" EditMode="PopUp" NoMasterRecordsText="No existen registros."
                                HorizontalAlign="NotSet">
                                <CommandItemTemplate>
                                    <div style="padding: 5px 5px;">
                                        <asp:LinkButton ID="btnEdit" runat="server" Mensaje="Recuperando Item para Edición..." CommandName="EditSelected" Visible='<%# RadGrid1.EditIndexes.Count == 0 %>'><img style="border:0px;vertical-align:middle;" alt="" src="Images/Edit.gif" />Editar</asp:LinkButton>&nbsp;&nbsp;
                                        <asp:LinkButton ID="btnInsert" runat="server" CommandName="InitInsert" Visible='<%# !RadGrid1.MasterTableView.IsItemInserted %>'
                                            OnClick="LinkButton2_Click"><img style="border:0px;vertical-align:middle;" alt="" src="Images/AddRecord.gif" />Insertar</asp:LinkButton>&nbsp;&nbsp;
                                        <asp:LinkButton ID="btnDelete" OnClientClick="return blockConfirm('Desea eliminar esta Item ?', event, 330, 100,'','Items');"
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
                                    <telerik:GridBoundColumn UniqueName="Codigo" HeaderText="Codigo" DataField="Codigo"
                                        MaxLength="10">
                                        <HeaderStyle HorizontalAlign="Center" Width="30px" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                    
                                    <telerik:GridBoundColumn UniqueName="Descripcion" HeaderText="Descripcion"
                                     DataField="Descripcion" MaxLength="250" ColumnEditorID="txtDescripcion" >
                                        <HeaderStyle HorizontalAlign="Center" Width="420px" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                    
                                    <telerik:GridDropDownColumn DataField="CategoriasItems.IdCategoria" DataSourceID="EntityDataSourceCat"
                                        HeaderText="Categoria" ListTextField="Nombre" ListValueField="IdCategoria" UniqueName="IdCategoria">
                                        <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Center" />
                                    </telerik:GridDropDownColumn>
                                    
                                    <telerik:GridTemplateColumn HeaderText="Rol con Acceso" UniqueName="TemplateColumn">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRol" runat="server" Text="" ></asp:Label>                                         
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox ID="cboRoles" Runat="server" 
                                                DataSourceID="EntityDataSourceRoles" DataTextField="Descripcion" 
                                                DataValueField="IdSegRol" Skin="Sunset">
                                            </telerik:RadComboBox>
                                        </EditItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="130px" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>       
                                    
                                </Columns>
                                <EditFormSettings ColumnNumber="1" CaptionDataField="Descripcion" CaptionFormatString="Editar el Item: {0}">
                                
                                    <FormTableItemStyle Wrap="False" HorizontalAlign="Left"></FormTableItemStyle>
                                    <FormMainTableStyle GridLines="Both" CellSpacing="0" CellPadding="3" BackColor="White" Width="100%" />
                                    <FormTableStyle CellSpacing="0" CellPadding="2" Width="100%" Height="110px" BackColor="White" />
                                    <FormStyle Width="100%" BackColor="#eef2ea"></FormStyle>
                                    <FormTableAlternatingItemStyle Wrap="False" HorizontalAlign="Left" ></FormTableAlternatingItemStyle>
                                    <EditColumn ButtonType="ImageButton" InsertText="Insertar" UpdateText="Actualizar" UniqueName="EditCommandColumn1" CancelText="Cancelar"></EditColumn>
                                    <FormTableButtonRowStyle HorizontalAlign="Right"></FormTableButtonRowStyle>
                                    <PopUpSettings ScrollBars="Auto" Modal="true" Width="60%" />
                                </EditFormSettings>
                            </MasterTableView>
                            <ValidationSettings CommandsToValidate="PerformInsert,Update" />
                            <ClientSettings>
                                <Selecting AllowRowSelect="True" />
                            </ClientSettings>
                        </telerik:RadGrid>
                        <telerik:GridTextBoxColumnEditor runat="server" ID="txtDescripcion" TextBoxStyle-Width="450px"></telerik:GridTextBoxColumnEditor>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

    
</asp:Content>
