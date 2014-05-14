<%@ Page Language="C#" EnableEventValidation="false" Theme="MiTema" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="ABMRoles.aspx.cs" Inherits="PaginasDefinicion_ABMRoles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <table style="width: 100%;">
        <tr>
            <td align="center">
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                    AllowSorting="True" AutoGenerateColumns="False" 
                    DataKeyNames="IdSegRol" DataSourceID="LinqDataSource1" Width="463px" onrowdeleted="GridView1_RowDeleted" 
                    onrowupdated="GridView1_RowUpdated">
                    <Columns>
                        <asp:CommandField ButtonType="Image" CancelImageUrl="~/images/delete_16x16.gif" 
                            CancelText="" DeleteImageUrl="~/images/delete_16x16.gif" DeleteText="" 
                            EditImageUrl="~/images/edit_16x16.gif" EditText="" 
                            InsertImageUrl="~/images/ok_16x16.gif" InsertText="" 
                            NewImageUrl="~/images/add_16x16.gif" NewText="" 
                            SelectImageUrl="~/images/right_16x16Conosud.gif" SelectText="" 
                            ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" 
                            UpdateImageUrl="~/images/ok_16x16.gif" UpdateText="" />
                        <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" 
                            SortExpression="Descripcion" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" 
                    DataKeyNames="IdSegRol" DataSourceID="LinqDataSource1" Height="50px" Width="460px" 
                    oniteminserted="DetailsView1_ItemInserted">
                    <Fields>
                        <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" 
                            SortExpression="Descripcion" />
                        <asp:CommandField ButtonType="Image" CancelImageUrl="~/images/delete_16x16.gif" 
                            CancelText="" DeleteText="" EditText="" InsertImageUrl="~/images/ok_16x16.gif" 
                            InsertText="" NewImageUrl="~/images/add_16x16.gif" NewText="" SelectText="" 
                            ShowInsertButton="True" UpdateText="" />
                    </Fields>
                </asp:DetailsView>
                <asp:LinqDataSource ID="LinqDataSource1" runat="server" 
                    ContextTypeName="ConosudDataContext" EnableDelete="True" 
                    EnableInsert="True" EnableUpdate="True" OrderBy="Descripcion" 
                    TableName="SegRols">
                </asp:LinqDataSource>
            
            </td>
        </tr>
        <tr>
            <td >
                        <div >Páginas x Rol</div>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:DropDownList ID="CboMenus" runat="server" DataTextField="Descripcion"
                    DataSourceID="LinqDataSource3" DataValueField="IdSegMenu" Height="16px" 
                    Width="208px">
                </asp:DropDownList>
                <asp:ImageButton ID="ImageButton1" runat="server" 
                    ImageUrl="~/images/add_16x16.gif" onclick="ImageButton1_Click" />

            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:GridView ID="GridView2" runat="server" AllowPaging="True" 
                    AllowSorting="True" AutoGenerateColumns="False" 
                    DataKeyNames="IdSegRolMenu" DataSourceID="LinqDataSource2" Width="461px">
                    <Columns>
                        <asp:CommandField ButtonType="Image" CancelImageUrl="~/images/delete_16x16.gif" 
                            DeleteImageUrl="~/images/delete_16x16.gif" EditImageUrl="~/images/edit_16x16.gif" 
                            InsertImageUrl="~/images/ok_16x16.gif" NewImageUrl="~/images/add_16x16.gif" 
                            SelectImageUrl="~/images/right_16x16Conosud.gif" ShowDeleteButton="True" 
                            ShowEditButton="True" ShowSelectButton="True" 
                            UpdateImageUrl="~/images/ok_16x16.gif" />
                        <asp:TemplateField HeaderText="Menu" SortExpression="Menu">
                            <EditItemTemplate>
                                <asp:DropDownList ID="DropDownList1" runat="server" 
                                    DataSourceID="LinqDataSource3" DataTextField="Descripcion" 
                                    DataValueField="IdSegMenu" Height="19px" SelectedValue='<%# Bind("Menu") %>' 
                                    Width="200px" Enabled="False">
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("ObjSegMenu.Descripcion") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CheckBoxField DataField="Lectura" HeaderText="Lectura" 
                            SortExpression="Lectura" />
                        <asp:CheckBoxField DataField="Modificacion" HeaderText="Modificacion" 
                            SortExpression="Modificacion" />
                        <asp:CheckBoxField DataField="Creacion" HeaderText="Creacion" 
                            SortExpression="Creacion" />
                    </Columns>
                </asp:GridView>
                <asp:LinqDataSource ID="LinqDataSource3" runat="server" 
                    ContextTypeName="ConosudDataContext" 
                    TableName="SegMenus">
                </asp:LinqDataSource>
                <asp:LinqDataSource ID="LinqDataSource2" runat="server" 
                    ContextTypeName="ConosudDataContext" EnableDelete="True" 
                    EnableInsert="True" EnableUpdate="True" onselecting="LinqDataSource2_Selecting" 
                    TableName="SegRolMenus" Where="Rol == @Rol">
                    <WhereParameters>
                        <asp:ControlParameter ControlID="GridView1" Name="Rol" 
                            PropertyName="SelectedValue" Type="Int64" />
                    </WhereParameters>
                </asp:LinqDataSource>
        
            </td>
        </tr>
        <tr>
            <td >
                &nbsp;</td>
        </tr>
    </table>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>