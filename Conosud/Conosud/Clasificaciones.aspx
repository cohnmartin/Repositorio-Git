<%@ Page Language="C#" MasterPageFile="~/DefaultMasterPage.master" Theme="MiTema" AutoEventWireup="true" CodeFile="Clasificaciones.aspx.cs" Inherits="Clasificaciones" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table cellpadding="0" cellspacing="5">
        <tr>
            <td align="center" style="width: 241px">
                <asp:Label ID="lblEncabezado" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Font-Bold="True" Font-Size="14pt" ForeColor="Maroon" Height="25px"
                    Style="background-image: url(images/FondoTitulos.gif); background-color: transparent"
                    Text="GESTION DE PARAMETROS" Width="316px"></asp:Label></td>
        </tr>
    </table>
    <asp:UpdateProgress id="UpdateProgress1" runat="server">
        <progresstemplate>
<DIV class="progress"><IMG src="images/indicator.gif" /> Cargando ..... </DIV>
</progresstemplate>
    </asp:UpdateProgress>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
<asp:ObjectDataSource id="ObjectDataSource1" runat="server" SelectMethod="GetData" TypeName="DSConosudTableAdapters.ClasificacionTableAdapter"><DeleteParameters>
<asp:Parameter Type="Int32" Name="Original_IdClasificacion"></asp:Parameter>
<asp:Parameter Type="String" Name="Original_Descripcion"></asp:Parameter>
<asp:Parameter Type="Int32" Name="Original_IdPadre"></asp:Parameter>
<asp:Parameter Type="String" Name="Original_Codigo"></asp:Parameter>
</DeleteParameters>
<UpdateParameters>
<asp:Parameter Type="String" Name="Descripcion"></asp:Parameter>
<asp:Parameter Type="Int32" Name="IdPadre"></asp:Parameter>
<asp:Parameter Type="String" Name="Codigo"></asp:Parameter>
<asp:Parameter Type="Int32" Name="IdClasificacion"></asp:Parameter>
</UpdateParameters>
<InsertParameters>
<asp:Parameter Type="String" Name="Descripcion"></asp:Parameter>
<asp:Parameter Type="Int32" Name="IdPadre"></asp:Parameter>
<asp:Parameter Type="String" Name="Codigo"></asp:Parameter>
</InsertParameters>
</asp:ObjectDataSource> <asp:ObjectDataSource id="ObjectDataSource2" runat="server" SelectMethod="GetDataByIdClasificacion" TypeName="DSConosudTableAdapters.ClasificacionTableAdapter" DeleteMethod="Delete" InsertMethod="Insert" UpdateMethod="Update" __designer:wfdid="w15"><DeleteParameters>
<asp:Parameter Type="Int32" Name="IdClasificacion"></asp:Parameter>
</DeleteParameters>
<UpdateParameters>
<asp:Parameter Type="String" Name="Descripcion"></asp:Parameter>
<asp:Parameter Type="Int64" Name="IdPadre"></asp:Parameter>
<asp:Parameter Type="String" Name="Codigo"></asp:Parameter>
<asp:Parameter Type="String" Name="Tipo"></asp:Parameter>
<asp:Parameter Type="Int32" Name="IdClasificacion"></asp:Parameter>
</UpdateParameters>
<SelectParameters>
<asp:ControlParameter PropertyName="SelectedValue" Type="Int32" Name="IdClasificacion" ControlID="GridView1"></asp:ControlParameter>
</SelectParameters>
<InsertParameters>
<asp:Parameter Type="String" Name="Descripcion"></asp:Parameter>
<asp:Parameter Type="Int64" Name="IdPadre"></asp:Parameter>
<asp:Parameter Type="String" Name="Codigo"></asp:Parameter>
<asp:Parameter Type="String" Name="Tipo"></asp:Parameter>
</InsertParameters>
</asp:ObjectDataSource> <asp:ObjectDataSource id="ObjectDataSource3" runat="server" SelectMethod="GetDataTipo" TypeName="DSConosudTableAdapters.ClasificacionTableAdapter" InsertMethod="Insert" __designer:wfdid="w1" OldValuesParameterFormatString="original_{0}"><InsertParameters>
<asp:Parameter Type="String" Name="Descripcion"></asp:Parameter>
<asp:Parameter Type="Int64" Name="IdPadre"></asp:Parameter>
<asp:Parameter Type="String" Name="Codigo"></asp:Parameter>
<asp:Parameter Type="String" Name="Tipo"></asp:Parameter>
</InsertParameters>
</asp:ObjectDataSource><TABLE style="WIDTH: 197px"><TBODY><TR><TD style="HEIGHT: 492px" vAlign=top><asp:GridView id="GridView1" runat="server" Width="435px" PageSize="15" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="IdClasificacion" DataSourceID="ObjectDataSource1"><Columns>
<asp:CommandField SelectImageUrl="~/images/right_16x16Conosud.gif" UpdateText="Actualizar" SelectText="" NewText="Nuevo" CancelText="Cancelar" InsertText="Insertar" DeleteText="Eliminar" EditText="Editar" ButtonType="Image" ShowSelectButton="True"></asp:CommandField>
<asp:BoundField DataField="Codigo" SortExpression="Codigo" HeaderText="Codigo"></asp:BoundField>
<asp:BoundField DataField="Descripcion" SortExpression="Descripcion" HeaderText="Descripcion"></asp:BoundField>
</Columns>
<PagerTemplate>
&nbsp;
</PagerTemplate>
</asp:GridView> </TD><TD style="WIDTH: 3px; HEIGHT: 492px" vAlign=top><asp:DetailsView id="DetailsView1" runat="server" Width="277px" __designer:wfdid="w16" DataKeyNames="IdClasificacion" DataSourceID="ObjectDataSource2" OnItemDeleting="DetailsView1_ItemDeleting" AutoGenerateRows="False" Height="50px" OnItemDeleted="DetailsView1_ItemDeleted" OnItemInserted="DetailsView1_ItemInserted" OnItemUpdated="DetailsView1_ItemUpdated" OnItemInserting="DetailsView1_ItemInserting" OnItemUpdating="DetailsView1_ItemUpdating"><FooterTemplate>
<asp:ValidationSummary id="ValidationSummary1" runat="server" ForeColor="White" __designer:wfdid="w42" HeaderText="Errores detectados:"></asp:ValidationSummary>
</FooterTemplate>
<Fields>
<asp:CommandField UpdateText="" CancelImageUrl="~/images/delete_16x16.gif" SelectText="" NewText="" EditImageUrl="~/images/edit_16x16.gif" CancelText="" InsertText="" ShowDeleteButton="True" InsertImageUrl="~/images/ok_16x16.gif" DeleteText="" EditText="" UpdateImageUrl="~/images/ok_16x16.gif" DeleteImageUrl="~/images/delete_16x16.gif" ButtonType="Image" ShowInsertButton="True" NewImageUrl="~/images/add_16x16.gif" ShowEditButton="True"></asp:CommandField>
<asp:TemplateField SortExpression="Descripcion" HeaderText="Descripcion"><EditItemTemplate>
<asp:TextBox runat="server" Text='<%# Bind("Descripcion") %>' id="TextBox1"></asp:TextBox>
</EditItemTemplate>
<InsertItemTemplate>
<asp:TextBox runat="server" Text='<%# Bind("Descripcion") %>' id="TextBox1"></asp:TextBox>
</InsertItemTemplate>
<ItemTemplate>
<asp:Label runat="server" Text='<%# Bind("Descripcion") %>' id="Label1"></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField SortExpression="Codigo" HeaderText="Codigo"><EditItemTemplate>
<asp:TextBox id="TextBox2" runat="server" __designer:wfdid="w39" Text='<%# Bind("Codigo") %>'></asp:TextBox>
</EditItemTemplate>
<InsertItemTemplate>
<asp:TextBox id="TextBox2" runat="server" __designer:wfdid="w40" Text='<%# Bind("Codigo") %>'></asp:TextBox>
</InsertItemTemplate>
<ItemTemplate>
<asp:Label id="Label2" runat="server" __designer:wfdid="w38" Text='<%# Bind("Codigo") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField SortExpression="Tipo" HeaderText="Tipo"><EditItemTemplate>
<asp:DropDownList id="DDLTipoEdit" runat="server" DataSourceID="ObjectDataSource3" __designer:wfdid="w9" SelectedValue='<%# Bind("Tipo") %>' DataTextField="Tipo" DataValueField="Tipo"></asp:DropDownList>
</EditItemTemplate>
<InsertItemTemplate>
<asp:DropDownList id="DDLTipoInsert" runat="server" DataSourceID="ObjectDataSource3" __designer:wfdid="w10" SelectedValue='<%# Bind("Tipo") %>' DataTextField="Tipo" DataValueField="Tipo"></asp:DropDownList>
</InsertItemTemplate>
<ItemTemplate>
<asp:DropDownList id="DDLTipoItem" runat="server" DataSourceID="ObjectDataSource3" __designer:wfdid="w5" Enabled="False" SelectedValue='<%# Eval("Tipo") %>' DataTextField="Tipo" DataValueField="Tipo"></asp:DropDownList>
</ItemTemplate>
</asp:TemplateField>
</Fields>
</asp:DetailsView></TD></TR></TBODY></TABLE>
</ContentTemplate>
</asp:UpdatePanel>    
</asp:Content>

