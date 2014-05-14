<%@ Page Language="C#" Theme="MiTema" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="Plantilla.aspx.cs" Inherits="Plantilla" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <table cellpadding="0" cellspacing="5">
        <tr>
            <td align="center" style="width: 241px">
                <asp:Label ID="lblEncabezado" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Font-Bold="True" Font-Size="14pt" ForeColor="Maroon" Height="25px"
                    Style="background-image: url(images/FondoTitulos.gif); background-color: transparent"
                    Text="GESTION DE ITEMS PLANTILLA" Width="346px"></asp:Label></td>
        </tr>
    </table>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:GridView ID="gvItems" runat="server" DataSourceID="ODSSelItems" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="IdPlantilla">
            <Columns>
                <asp:CommandField SelectImageUrl="~/images/right_16x16Conosud.gif" ButtonType="Image"
                    ShowSelectButton="True"></asp:CommandField>
                <asp:BoundField ReadOnly="True" DataField="IdPlantilla" InsertVisible="False" Visible="False"
                    SortExpression="IdPlantilla" HeaderText="IdPlantilla"></asp:BoundField>
                <asp:BoundField DataField="Descripcion" SortExpression="Descripcion" HeaderText="Descripcion">
                    <ItemStyle Wrap="False"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Codigo" SortExpression="Codigo" HeaderText="Codigo"></asp:BoundField>
                <asp:BoundField DataField="IdCategoria" Visible="False" SortExpression="IdCategoria"
                    HeaderText="IdCategoria"></asp:BoundField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="ODSCatagorias" runat="server" OldValuesParameterFormatString="original_{0}"
            __designer:wfdid="w7" SelectMethod="GetData" TypeName="DSConosudTableAdapters.CategoriasItemsTableAdapter">
            <DeleteParameters>
                <asp:Parameter Type="Int64" Name="Original_IdCategoria"></asp:Parameter>
                <asp:Parameter Type="String" Name="Original_Nombre"></asp:Parameter>
                <asp:Parameter Type="String" Name="Original_Descripcion"></asp:Parameter>
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Type="String" Name="Nombre"></asp:Parameter>
                <asp:Parameter Type="String" Name="Descripcion"></asp:Parameter>
                <asp:Parameter Type="Int64" Name="Original_IdCategoria"></asp:Parameter>
                <asp:Parameter Type="String" Name="Original_Nombre"></asp:Parameter>
                <asp:Parameter Type="String" Name="Original_Descripcion"></asp:Parameter>
                <asp:Parameter Type="Int32" Name="IdCategoria"></asp:Parameter>
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Type="String" Name="Nombre"></asp:Parameter>
                <asp:Parameter Type="String" Name="Descripcion"></asp:Parameter>
            </InsertParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ODSSelItems" runat="server" __designer:wfdid="w118" SelectMethod="GetData"
            TypeName="DSConosudTableAdapters.PlantillaTableAdapter"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ODSItems" runat="server" __designer:wfdid="w1" SelectMethod="GetDataByIdPlantilla"
            TypeName="DSConosudTableAdapters.PlantillaTableAdapter" DeleteMethod="Delete"
            InsertMethod="Insert" UpdateMethod="Update">
            <DeleteParameters>
                <asp:Parameter Type="Int32" Name="IdPlantilla"></asp:Parameter>
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Type="String" Name="Descripcion"></asp:Parameter>
                <asp:Parameter Type="String" Name="Codigo"></asp:Parameter>
                <asp:Parameter Type="Int64" Name="IdCategoria"></asp:Parameter>
                <asp:Parameter Type="Int32" Name="IdPlantilla"></asp:Parameter>
            </UpdateParameters>
            <SelectParameters>
                <asp:ControlParameter PropertyName="SelectedValue" Type="Int32" Name="IdPlantilla"
                    ControlID="gvItems"></asp:ControlParameter>
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Type="String" Name="Descripcion"></asp:Parameter>
                <asp:Parameter Type="String" Name="Codigo"></asp:Parameter>
                <asp:Parameter Type="Int64" Name="IdCategoria"></asp:Parameter>
            </InsertParameters>
        </asp:ObjectDataSource>
        <asp:DetailsView ID="dvItems" runat="server" DataSourceID="ODSItems" DataKeyNames="IdPlantilla"
            __designer:wfdid="w125" Width="669px" Height="50px" AutoGenerateRows="False"
            OnItemInserted="dvItems_ItemInserted" OnItemUpdated="dvItems_ItemUpdated" OnItemDeleted="dvItems_ItemDeleted">
            <Fields>
                <asp:CommandField UpdateText="Actualizar" CancelImageUrl="~/images/delete_16x16.gif"
                    SelectText="Sel" NewText="Nuevo" EditImageUrl="~/images/edit_16x16.gif" CancelText="Cancelar"
                    InsertText="Insertar" ShowDeleteButton="True" InsertImageUrl="~/images/ok_16x16.gif"
                    DeleteText="Elimina" EditText="Editar" UpdateImageUrl="~/images/ok_16x16.gif"
                    DeleteImageUrl="~/images/delete_16x16.gif" ButtonType="Image" ShowInsertButton="True"
                    NewImageUrl="~/images/add_16x16.gif" ShowEditButton="True"></asp:CommandField>
                <asp:TemplateField SortExpression="Codigo" HeaderText="Codigo">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Width="95px" Text='<%# Bind("Codigo") %>'
                            __designer:wfdid="w127"></asp:TextBox>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Width="95px" Text='<%# Bind("Codigo") %>'
                            __designer:wfdid="w128"></asp:TextBox>
                    </InsertItemTemplate>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Codigo") %>' __designer:wfdid="w126"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="Descripcion" HeaderText="Descripcion">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Width="623px" Text='<%# Bind("Descripcion") %>'
                            __designer:wfdid="w130"></asp:TextBox>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Width="623px" Text='<%# Bind("Descripcion") %>'
                            __designer:wfdid="w131"></asp:TextBox>
                    </InsertItemTemplate>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("Descripcion") %>' __designer:wfdid="w129"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="IdCategoria" HeaderText="Categor&#237;a">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DropDownList2" runat="server" DataSourceID="ODSCatagorias"
                            __designer:wfdid="w20" Width="167px" DataTextField="Nombre" DataValueField="IdCategoria"
                            SelectedValue='<%# Bind("IdCategoria") %>'>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:DropDownList ID="DropDownList3" runat="server" DataSourceID="ODSCatagorias"
                            __designer:wfdid="w21" Width="175px" DataTextField="Nombre" DataValueField="IdCategoria"
                            SelectedValue='<%# Bind("IdCategoria") %>'>
                        </asp:DropDownList>
                    </InsertItemTemplate>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <asp:DropDownList ID="DropDownList2" runat="server" DataSourceID="ODSCatagorias"
                            __designer:wfdid="w19" Width="157px" DataTextField="Nombre" DataValueField="IdCategoria"
                            SelectedValue='<%# Bind("IdCategoria") %>' Enabled="False">
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
            </Fields>
        </asp:DetailsView>
        <table id="tblRoles" runat="server">
            <tbody>
                <tr>
                    <td style="height: 26px" align="center" colspan="2">
                        <asp:Label ID="Label1" runat="server" ForeColor="Maroon" __designer:wfdid="w6" Width="295px"
                            Height="22px" Font-Bold="True" Text="ROLES CON ACCESO DE ESCRITURA"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 87px; height: 26px" align="right">
                        <asp:DropDownList ID="cboRoles" runat="server" DataSourceID="ODSRoles" __designer:wfdid="w1"
                            Width="216px" Height="25px" DataValueField="RoleId" DataTextField="RoleName">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 100px; height: 26px" align="left">
                        <asp:ImageButton ID="ImageButton1" OnClick="ImageButton1_Click" runat="server" __designer:wfdid="w2"
                            ImageUrl="~/images/add_16x16.gif"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td valign="top" colspan="2">
                        <asp:GridView ID="grRolesPlantilla" runat="server" DataSourceID="ODSRolesPlantilla"
                            AutoGenerateColumns="False" DataKeyNames="IdRolPlanilla" __designer:wfdid="w3"
                            Width="361px">
                            <Columns>
                                <asp:CommandField ShowDeleteButton="True" DeleteImageUrl="~/images/delete_16x16.gif"
                                    ButtonType="Image"></asp:CommandField>
                                <asp:BoundField ReadOnly="True" DataField="IdRolPlanilla" InsertVisible="False" Visible="False"
                                    SortExpression="IdRolPlanilla" HeaderText="IdRolPlanilla"></asp:BoundField>
                                <asp:BoundField DataField="IdPlanilla" Visible="False" SortExpression="IdPlanilla"
                                    HeaderText="IdPlanilla"></asp:BoundField>
                                <asp:BoundField DataField="IdRol" Visible="False" SortExpression="IdRol" HeaderText="IdRol">
                                </asp:BoundField>
                                <asp:BoundField DataField="RoleName" HeaderText="Rol">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </tbody>
        </table>
        &nbsp;&nbsp;
        <asp:ObjectDataSource ID="ODSRoles" runat="server" OldValuesParameterFormatString="original_{0}"
            __designer:wfdid="w4" SelectMethod="GetData" TypeName="DSConosudTableAdapters.aspnet_RolesTableAdapter"
            DeleteMethod="Delete" InsertMethod="Insert" UpdateMethod="Update">
            <DeleteParameters>
                <asp:Parameter Type="Object" Name="Original_ApplicationId"></asp:Parameter>
                <asp:Parameter Type="Object" Name="Original_RoleId"></asp:Parameter>
                <asp:Parameter Type="String" Name="Original_RoleName"></asp:Parameter>
                <asp:Parameter Type="String" Name="Original_LoweredRoleName"></asp:Parameter>
                <asp:Parameter Type="String" Name="Original_Description"></asp:Parameter>
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Type="Object" Name="ApplicationId"></asp:Parameter>
                <asp:Parameter Type="Object" Name="RoleId"></asp:Parameter>
                <asp:Parameter Type="String" Name="RoleName"></asp:Parameter>
                <asp:Parameter Type="String" Name="LoweredRoleName"></asp:Parameter>
                <asp:Parameter Type="String" Name="Description"></asp:Parameter>
                <asp:Parameter Type="Object" Name="Original_ApplicationId"></asp:Parameter>
                <asp:Parameter Type="Object" Name="Original_RoleId"></asp:Parameter>
                <asp:Parameter Type="String" Name="Original_RoleName"></asp:Parameter>
                <asp:Parameter Type="String" Name="Original_LoweredRoleName"></asp:Parameter>
                <asp:Parameter Type="String" Name="Original_Description"></asp:Parameter>
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Type="Object" Name="ApplicationId"></asp:Parameter>
                <asp:Parameter Type="Object" Name="RoleId"></asp:Parameter>
                <asp:Parameter Type="String" Name="RoleName"></asp:Parameter>
                <asp:Parameter Type="String" Name="LoweredRoleName"></asp:Parameter>
                <asp:Parameter Type="String" Name="Description"></asp:Parameter>
            </InsertParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ODSRolesPlantilla" runat="server" __designer:wfdid="w5"
            SelectMethod="GetDataByPlantilla" TypeName="DSConosudTableAdapters.RolesPlanillaTableAdapter"
            DeleteMethod="Delete1" InsertMethod="Insert" UpdateMethod="Update" OnSelected="ODSRolesPlantilla_Selected">
            <DeleteParameters>
                <asp:Parameter Type="Int32" Name="IdRolPlanilla"></asp:Parameter>
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Type="Int64" Name="IdPlanilla"></asp:Parameter>
                <asp:Parameter Type="String" Name="IdRol"></asp:Parameter>
                <asp:Parameter Type="Int32" Name="IdRolPlanilla"></asp:Parameter>
            </UpdateParameters>
            <SelectParameters>
                <asp:ControlParameter PropertyName="SelectedValue" Type="Int64" Name="IdPlanilla"
                    ControlID="gvItems"></asp:ControlParameter>
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Type="Int64" Name="IdPlanilla"></asp:Parameter>
                <asp:Parameter Type="String" Name="IdRol"></asp:Parameter>
            </InsertParameters>
        </asp:ObjectDataSource>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
