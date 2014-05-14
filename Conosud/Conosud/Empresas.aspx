<%@ Page Language="C#" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="Empresas.aspx.cs" Inherits="Empresas" Title="Untitled Page" Theme="MiTema" %>
<%@ Register Assembly="Validators" Namespace="Sample.Web.UI.Compatibility" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script runat="server">
    public int GetTipo(object Tipo)
    {
        if (Tipo == DBNull.Value || Tipo == null)
            return 0;
        else
            return (int)Tipo;
    }
</script>


    <asp:UpdateProgress id="UpdateProgress1" runat="server">
        <progresstemplate>
<DIV class="progress"><IMG src="images/indicator.gif" /> Cargando ..... </DIV>
</progresstemplate>
    </asp:UpdateProgress>
    <table cellpadding="0" cellspacing="5">
        <tr>
            <td align="center" style="width: 241px">
                <asp:Label ID="lblEncabezado" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Font-Bold="True" Font-Size="14pt" ForeColor="Maroon" Height="25px"
                    Style="background-image: url(images/FondoTitulos.gif); background-color: transparent"
                    Text="GESTION DE EMPRESAS" Width="278px"></asp:Label></td>
        </tr>
    </table>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
<TABLE style="WIDTH: 255px"><TBODY><TR><TD style="WIDTH: 57%" vAlign=top>
    <asp:GridView id="gvEmpresa" runat="server" Width="493px" 
        DataSourceID="ODSSelEmpresa" AllowPaging="True" AllowSorting="True" 
        AutoGenerateColumns="False" DataKeyNames="IdEmpresa"><Columns>
<asp:CommandField SelectImageUrl="~/images/right_16x16Conosud.gif" SelectText="" ShowSelectButton="True" ButtonType="Image"></asp:CommandField>
<asp:BoundField DataField="RazonSocial" HeaderText="Razon Social" SortExpression="RazonSocial"></asp:BoundField>
<asp:BoundField DataField="CUIT" HeaderText="CUIT" SortExpression="CUIT"></asp:BoundField>
<asp:BoundField DataField="Telefono" HeaderText="Telefono" SortExpression="Telefono"></asp:BoundField>
<asp:BoundField DataField="FechaAlta" HeaderText="Fecha Alta" Visible="False"></asp:BoundField>
<asp:BoundField DataField="RepresentanteTecnico" HeaderText="Representante Tecnico" Visible="False"></asp:BoundField>
<asp:BoundField DataField="PrestacionEmergencia" HeaderText="Prestacion Emergencia" Visible="False"></asp:BoundField>
<asp:BoundField DataField="Direccion" HeaderText="Direcci&#243;n" Visible="False"></asp:BoundField>
<asp:BoundField DataField="CorreoElectronico" HeaderText="Correo Electronico" Visible="False"></asp:BoundField>
</Columns>
</asp:GridView></TD><TD vAlign=top width="50%"><asp:DetailsView id="dvEmpresas" runat="server" Width="397px" DataSourceID="ODSEmpresa" Height="50px" DataKeyNames="IdEmpresa" OnItemUpdating="dvEmpresas_ItemUpdating" OnItemInserting="dvEmpresas_ItemInserting" OnItemDeleting="dvEmpresas_ItemDeleting" OnItemUpdated="DetailsView1_ItemUpdated" OnItemInserted="DetailsView1_ItemInserted" OnItemDeleted="DetailsView1_ItemDeleted" AutoGenerateRows="False"><FooterTemplate>
<asp:ValidationSummary id="ValidationSummary1" runat="server" ForeColor="White" __designer:wfdid="w21"></asp:ValidationSummary> 
</FooterTemplate>
<Fields>
<asp:CommandField UpdateText="" CancelImageUrl="~/images/delete_16x16.gif" SelectText="" NewText="" EditImageUrl="~/images/edit_16x16.gif" CancelText="" InsertText="" ShowDeleteButton="True" InsertImageUrl="~/images/add_16x16.gif" DeleteText="" EditText="" UpdateImageUrl="~/images/ok_16x16.gif" DeleteImageUrl="~/images/delete_16x16.gif" ButtonType="Image" ShowInsertButton="True" NewImageUrl="~/images/add_16x16.gif" ShowEditButton="True"></asp:CommandField>
<asp:TemplateField SortExpression="RazonSocial" HeaderText="RazonSocial"><EditItemTemplate>
<asp:TextBox id="TxtEditRazon" runat="server" Text='<%# Bind("RazonSocial") %>' __designer:wfdid="w23"></asp:TextBox> <asp:RequiredFieldValidator id="ValidaRazonEdit" runat="server" __designer:wfdid="w24" ErrorMessage="Ingrese Razon Social" ControlToValidate="TxtEditRazon">*</asp:RequiredFieldValidator> 
</EditItemTemplate>
<InsertItemTemplate>
<asp:TextBox id="TxtInsertRazon" runat="server" Text='<%# Bind("RazonSocial") %>' __designer:wfdid="w25"></asp:TextBox> <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" __designer:wfdid="w26" ErrorMessage="Ingrese Razon Social" ControlToValidate="TxtInsertRazon">*</asp:RequiredFieldValidator> 
</InsertItemTemplate>
<ItemTemplate>
<asp:Label id="Label2" runat="server" Text='<%# Bind("RazonSocial") %>' __designer:wfdid="w22"></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField SortExpression="CUIT" HeaderText="CUIT"><EditItemTemplate>
<asp:TextBox id="Txeditcuit" runat="server" Text='<%# Bind("CUIT") %>' __designer:wfdid="w113"></asp:TextBox> <asp:RequiredFieldValidator id="ValidarCuit" runat="server" ControlToValidate="Txeditcuit" __designer:wfdid="w114" ErrorMessage="Ingrese Cuit">*</asp:RequiredFieldValidator> <asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ControlToValidate="Txeditcuit" __designer:wfdid="w115" ErrorMessage="Ingrese CUIT valido" ValidationExpression="\d{2}\-\d{8}\-\d{1}">*</asp:RegularExpressionValidator> 
</EditItemTemplate>
<InsertItemTemplate>
<asp:TextBox id="TxInsertcuit" runat="server" Text='<%# Bind("CUIT") %>' __designer:wfdid="w116"></asp:TextBox> <asp:RequiredFieldValidator id="ValidarCuit1" runat="server" ControlToValidate="TxInsertcuit" __designer:wfdid="w117" ErrorMessage="IngreseCuit">*</asp:RequiredFieldValidator>&nbsp;<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ControlToValidate="TxInsertcuit" __designer:wfdid="w119" ErrorMessage="Ingrese CUIT valido" ValidationExpression="\d{2}\-\d{8}\-\d{1}">*</asp:RegularExpressionValidator>
</InsertItemTemplate>
<ItemTemplate>
<asp:Label id="Label3" runat="server" Text='<%# Bind("CUIT") %>' __designer:wfdid="w112"></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField SortExpression="FechaAlta" HeaderText="FechaAlta"><EditItemTemplate>
<asp:TextBox id="TxtEditFechaAlta" runat="server" Text='<%# Bind("FechaAlta") %>' __designer:wfdid="w91"></asp:TextBox> <asp:ImageButton id="Image1" runat="Server" Width="19px" ImageUrl="~/Images/Calendar_scheduleHS.png" AlternateText="Clic para mostar Calendario" __designer:wfdid="w92"></asp:ImageButton>&nbsp; <ajaxToolkit:MaskedEditExtender id="MaskedEditExtender2" runat="server" TargetControlID="TxtEditFechaAlta" MaskType="Date" OnInvalidCssClass="MaskedEditError" OnFocusCssClass="MaskedEditFocus" Mask="99/99/9999" ErrorTooltipEnabled="True" DisplayMoney="Left" CultureName="en-US" AcceptNegative="Left" __designer:wfdid="w93">
                        </ajaxToolkit:MaskedEditExtender><ajaxToolkit:CalendarExtender id="CalendarExtender1" runat="server" TargetControlID="TxtEditFechaAlta" PopupButtonID="Image1" __designer:wfdid="w95" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender> 
</EditItemTemplate>
<InsertItemTemplate>
<asp:TextBox id="TxtInsertFechaAlta" runat="server" Text='<%# Bind("FechaAlta") %>' __designer:wfdid="w96"></asp:TextBox> <asp:ImageButton id="Image1" runat="Server" Width="19px" ImageUrl="~/Images/Calendar_scheduleHS.png" AlternateText="Clic para mostar Calendario" __designer:wfdid="w97"></asp:ImageButton>&nbsp; <ajaxToolkit:MaskedEditExtender id="MaskedEditExtender1" runat="server" TargetControlID="TxtInsertFechaAlta" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True" DisplayMoney="Left" CultureName="en-US" AcceptNegative="Left" __designer:wfdid="w98">
                        </ajaxToolkit:MaskedEditExtender><ajaxToolkit:CalendarExtender id="CalendarExtender2" runat="server" TargetControlID="TxtInsertFechaAlta" PopupButtonID="Image1" __designer:wfdid="w100" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender> &nbsp; 
</InsertItemTemplate>
<ItemTemplate>
<asp:Label id="Label5" runat="server" Text='<%# Bind("FechaAlta") %>' __designer:wfdid="w90"></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField SortExpression="RepresentanteTecnico" HeaderText="RepresentanteTecnico"><EditItemTemplate>
                        <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("RepresentanteTecnico") %>'></asp:TextBox>
                    
</EditItemTemplate>
<InsertItemTemplate>
                        <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("RepresentanteTecnico") %>'></asp:TextBox>
                    
</InsertItemTemplate>
<ItemTemplate>
                        <asp:Label ID="Label6" runat="server" Text='<%# Bind("RepresentanteTecnico") %>'></asp:Label>
                    
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField SortExpression="PrestacionEmergencia" HeaderText="PrestacionEmergencia"><EditItemTemplate>
                        <asp:TextBox ID="TextBox7" runat="server" Text='<%# Bind("PrestacionEmergencia") %>'></asp:TextBox>
                    
</EditItemTemplate>
<InsertItemTemplate>
                        <asp:TextBox ID="TextBox7" runat="server" Text='<%# Bind("PrestacionEmergencia") %>'></asp:TextBox>
                    
</InsertItemTemplate>
<ItemTemplate>
                        <asp:Label ID="Label7" runat="server" Text='<%# Bind("PrestacionEmergencia") %>'></asp:Label>
                    
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField SortExpression="Direccion" HeaderText="Direccion"><EditItemTemplate>
                        <asp:TextBox ID="TextBox8" runat="server" Text='<%# Bind("Direccion") %>'></asp:TextBox>
                    
</EditItemTemplate>
<InsertItemTemplate>
                        <asp:TextBox ID="TextBox8" runat="server" Text='<%# Bind("Direccion") %>'></asp:TextBox>
                    
</InsertItemTemplate>
<ItemTemplate>
                        <asp:Label ID="Label8" runat="server" Text='<%# Bind("Direccion") %>'></asp:Label>
                    
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField SortExpression="Telefono" HeaderText="Telefono"><EditItemTemplate>
                        <asp:TextBox ID="TextBox9" runat="server" Text='<%# Bind("Telefono") %>'></asp:TextBox>
                    
</EditItemTemplate>
<InsertItemTemplate>
                        <asp:TextBox ID="TextBox9" runat="server" Text='<%# Bind("Telefono") %>'></asp:TextBox>
                    
</InsertItemTemplate>
<ItemTemplate>
                        <asp:Label ID="Label9" runat="server" Text='<%# Bind("Telefono") %>'></asp:Label>
                    
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField SortExpression="CorreoElectronico" HeaderText="CorreoElectronico"><EditItemTemplate>
<asp:TextBox id="TextBox10" runat="server" Text='<%# Bind("CorreoElectronico") %>'></asp:TextBox> 
</EditItemTemplate>
<InsertItemTemplate>
<asp:TextBox id="TextBox10" runat="server" Text='<%# Bind("CorreoElectronico") %>'></asp:TextBox> 
</InsertItemTemplate>
<ItemTemplate>
<asp:Label id="Label10" runat="server" Text='<%# Bind("CorreoElectronico") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
</Fields>
</asp:DetailsView></TD></TR>
<tr>
<td colspan="2">
<asp:ObjectDataSource id="ODSSelEmpresa" runat="server" SelectMethod="GetData" TypeName="DSConosudTableAdapters.EmpresaTableAdapter" OnSelected="ODSSelEmpresa_Selected"><DeleteParameters>
<asp:Parameter Type="Int32" Name="IdEmpresa"></asp:Parameter>
</DeleteParameters>
<UpdateParameters>
<asp:Parameter Type="String" Name="Codigo"></asp:Parameter>
<asp:Parameter Type="String" Name="RazonSocial"></asp:Parameter>
<asp:Parameter Type="String" Name="CUIT"></asp:Parameter>
<asp:Parameter Type="DateTime" Name="FechaAlta"></asp:Parameter>
<asp:Parameter Type="String" Name="RepresentanteTecnico"></asp:Parameter>
<asp:Parameter Type="String" Name="PrestacionEmergencia"></asp:Parameter>
<asp:Parameter Type="String" Name="Direccion"></asp:Parameter>
<asp:Parameter Type="String" Name="Telefono"></asp:Parameter>
<asp:Parameter Type="String" Name="CorreoElectronico"></asp:Parameter>
<asp:Parameter Type="Int32" Name="IdEmpresa"></asp:Parameter>
</UpdateParameters>
<InsertParameters>
<asp:Parameter Type="String" Name="Codigo"></asp:Parameter>
<asp:Parameter Type="String" Name="RazonSocial"></asp:Parameter>
<asp:Parameter Type="String" Name="CUIT"></asp:Parameter>
<asp:Parameter Type="DateTime" Name="FechaAlta"></asp:Parameter>
<asp:Parameter Type="String" Name="RepresentanteTecnico"></asp:Parameter>
<asp:Parameter Type="String" Name="PrestacionEmergencia"></asp:Parameter>
<asp:Parameter Type="String" Name="Direccion"></asp:Parameter>
<asp:Parameter Type="String" Name="Telefono"></asp:Parameter>
<asp:Parameter Type="String" Name="CorreoElectronico"></asp:Parameter>
</InsertParameters>
</asp:ObjectDataSource>&nbsp;&nbsp; <asp:TextBox id="Date5" runat="server" Width="36px" ForeColor="Red" Visible="False"></asp:TextBox> <ajaxToolkit:CalendarExtender id="calendarButtonExtender" runat="server" TargetControlID="Date5"></ajaxToolkit:CalendarExtender> 
<asp:ObjectDataSource id="ODSEmpresa" runat="server" SelectMethod="GetDataByIdEmpresa" TypeName="DSConosudTableAdapters.EmpresaTableAdapter" 
UpdateMethod="Update" 
InsertMethod="Insert"
DeleteMethod="Delete"><DeleteParameters>
<asp:Parameter Type="Int64" Name="Original_IdEmpresa"></asp:Parameter>
</DeleteParameters>
<UpdateParameters>
<asp:Parameter Type="String" Name="RazonSocial"></asp:Parameter>
<asp:Parameter Type="String" Name="CUIT"></asp:Parameter>
<asp:Parameter Type="DateTime" Name="FechaAlta"></asp:Parameter>
<asp:Parameter Type="String" Name="RepresentanteTecnico"></asp:Parameter>
<asp:Parameter Type="String" Name="PrestacionEmergencia"></asp:Parameter>
<asp:Parameter Type="String" Name="Direccion"></asp:Parameter>
<asp:Parameter Type="String" Name="Telefono"></asp:Parameter>
<asp:Parameter Type="String" Name="CorreoElectronico"></asp:Parameter>
<asp:Parameter Type="Int32" Name="IdEmpresa"></asp:Parameter>
</UpdateParameters>
<SelectParameters>
<asp:ControlParameter PropertyName="SelectedValue" Type="Int32" Name="IdEmpresa" ControlID="gvEmpresa"></asp:ControlParameter>
</SelectParameters>
<InsertParameters>
<asp:Parameter Type="String" Name="RazonSocial"></asp:Parameter>
<asp:Parameter Type="String" Name="CUIT"></asp:Parameter>
<asp:Parameter Type="DateTime" Name="FechaAlta"></asp:Parameter>
<asp:Parameter Type="String" Name="RepresentanteTecnico"></asp:Parameter>
<asp:Parameter Type="String" Name="PrestacionEmergencia"></asp:Parameter>
<asp:Parameter Type="String" Name="Direccion"></asp:Parameter>
<asp:Parameter Type="String" Name="Telefono"></asp:Parameter>
<asp:Parameter Type="String" Name="CorreoElectronico"></asp:Parameter>
</InsertParameters>
</asp:ObjectDataSource> <asp:ObjectDataSource id="ODClasificaciones" runat="server" SelectMethod="GetData" TypeName="DSConosudTableAdapters.ClasificacionTableAdapter"></asp:ObjectDataSource>&nbsp;&nbsp; 
</td>
</tr>
</TBODY></TABLE>

</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>