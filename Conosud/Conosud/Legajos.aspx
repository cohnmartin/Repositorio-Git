<%@ Page Language="C#" Theme="MiTema" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="Legajos.aspx.cs" Inherits="Legajos" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <table cellpadding="0" cellspacing="5" >
        <tr>
            <td align="center" style="width: 241px">
    <asp:Label ID="lblEncabezado" runat="server" BorderStyle="Solid" 
        BorderWidth="1px" Font-Bold="True" Font-Size="14pt" ForeColor="Maroon" Text="GESTION DE LEGAJOS"
        Width="268px" style="background-image: url(images/FondoTitulos.gif); background-color: transparent" BorderColor="Black" Height="25px"></asp:Label></td>
        </tr>
    </table>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
<TABLE style="WIDTH: 100%"><TBODY><TR><TD style="WIDTH: 100%" vAlign=top align=left><asp:GridView id="gvLegajos" runat="server" Width="100%" DataSourceID="ObjectDataSource1" PageSize="2" DataKeyNames="IdLegajos" AutoGenerateColumns="False" AllowPaging="True"><Columns>
<asp:CommandField SelectImageUrl="~/images/right_16x16Conosud.gif" SelectText="" ShowSelectButton="True" ButtonType="Image"></asp:CommandField>
<asp:BoundField DataField="Apellido" HeaderText="Apellido" SortExpression="Apellido"></asp:BoundField>
<asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre"></asp:BoundField>
<asp:BoundField DataField="DescTipoDocumento" HeaderText="Tipo Documento" SortExpression="DescTipoDocumento"></asp:BoundField>
<asp:BoundField DataField="NroDoc" HeaderText="NroDoc" SortExpression="NroDoc"></asp:BoundField>
<asp:BoundField DataField="CUIL" HeaderText="CUIL" SortExpression="CUIL"></asp:BoundField>
<asp:BoundField DataField="TelefonoFijo" HeaderText="TelefonoFijo" SortExpression="TelefonoFijo"></asp:BoundField>
<asp:BoundField DataField="DescEstadoCivil" HeaderText="Estado Civil" SortExpression="DescEstadoCivil" Visible="False"></asp:BoundField>
<asp:BoundField DataField="DescNacionalidad" HeaderText="Nacionalidad" SortExpression="DescNacionalidad" Visible="False"></asp:BoundField>
<asp:BoundField DataField="DescProvincia" HeaderText="Provincia" SortExpression="DescProvincia" Visible="False"></asp:BoundField>
</Columns>
</asp:GridView> </TD><TD style="WIDTH: 443px" vAlign=top><asp:DetailsView id="dvLegajos" runat="server" Width="447px" DataSourceID="ObjectDataSource2" Height="50px" DataKeyNames="IdLegajos" AllowPaging="True" OnItemUpdating="dvLegajos_ItemUpdating" OnItemInserting="dvLegajos_ItemInserting" AutoGenerateRows="False" OnItemDeleted="dvLegajos_ItemDeleted" OnItemInserted="dvLegajos_ItemInserted" OnItemUpdated="dvLegajos_ItemUpdated"><FooterTemplate>
        <asp:ValidationSummary id="ValidationSummary1" runat="server" ForeColor="White" __designer:wfdid="w43" HeaderText="Errores detectados:"></asp:ValidationSummary> 
        </FooterTemplate>
        <Fields>
        <asp:CommandField CancelImageUrl="~/images/delete_16x16.gif" CancelText="" DeleteImageUrl="~/images/delete_16x16.gif" DeleteText="" EditImageUrl="~/images/edit_16x16.gif" EditText="" InsertImageUrl="~/images/ok_16x16.gif" InsertText="" NewImageUrl="~/images/add_16x16.gif" NewText="" SelectText="" ShowDeleteButton="True" ShowEditButton="True" ShowInsertButton="True" UpdateImageUrl="~/images/ok_16x16.gif" UpdateText="" ButtonType="Image"></asp:CommandField>
        <asp:TemplateField HeaderText="Apellido" SortExpression="Apellido"><EditItemTemplate>
        <asp:TextBox id="TxtEditApellido" runat="server" Text='<%# Bind("Apellido") %>' __designer:wfdid="w33"></asp:TextBox> <asp:RequiredFieldValidator id="ValidaApellido" runat="server" __designer:wfdid="w34" ErrorMessage="Ingrese Apellido" ControlToValidate="TxtEditApellido">*</asp:RequiredFieldValidator> 
        </EditItemTemplate>
        <InsertItemTemplate>
        <asp:TextBox id="TxtInsertApellido" runat="server" Text='<%# Bind("Apellido") %>' __designer:wfdid="w35"></asp:TextBox> <asp:RequiredFieldValidator id="ValidaApellido2" runat="server" __designer:wfdid="w36" ErrorMessage="Ingrese Apellido" ControlToValidate="TxtInsertApellido">*</asp:RequiredFieldValidator> 
        </InsertItemTemplate>
        <ItemTemplate>
        <asp:Label id="Label1" runat="server" Text='<%# Bind("Apellido") %>' __designer:wfdid="w42"></asp:Label> 
        </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Nombre" SortExpression="Nombre"><EditItemTemplate>
        <asp:TextBox id="TxteditNombre" runat="server" __designer:wfdid="w83" Text='<%# Bind("Nombre") %>'></asp:TextBox> <asp:RequiredFieldValidator id="ValidaNombre" runat="server" __designer:wfdid="w85" ErrorMessage="Ingrese Nombre" ControlToValidate="TxteditNombre">*</asp:RequiredFieldValidator>
        </EditItemTemplate>
        <InsertItemTemplate>
        <asp:TextBox id="TxtinsertNombre" runat="server" __designer:wfdid="w84" Text='<%# Bind("Nombre") %>'></asp:TextBox> <asp:RequiredFieldValidator id="ValidaNombre2" runat="server" __designer:wfdid="w86" ErrorMessage="Ingrese Nombre" ControlToValidate="TxtinsertNombre">*</asp:RequiredFieldValidator>
        </InsertItemTemplate>
        <ItemTemplate>
        <asp:Label id="Label2" runat="server" __designer:wfdid="w82" Text='<%# Bind("Nombre") %>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="TipoDoc" SortExpression="TipoDoc"><EditItemTemplate>
        <asp:DropDownList id="DDLTipoDocEdit" runat="server" DataSourceID="ObjectDataSource4" Width="152px" __designer:wfdid="w108" DataTextField="Descripcion" DataValueField="IdClasificacion" SelectedValue='<%# Bind("TipoDoc") %>'></asp:DropDownList>&nbsp; 
        </EditItemTemplate>
        <InsertItemTemplate>
        <asp:DropDownList id="DDLTipoDocInsert" runat="server" DataSourceID="ObjectDataSource4" Width="152px" __designer:wfdid="w109" DataTextField="Descripcion" DataValueField="IdClasificacion" SelectedValue='<%# Bind("TipoDoc") %>'></asp:DropDownList>&nbsp; 
        </InsertItemTemplate>
        <ItemTemplate>
        <asp:DropDownList id="DDLTipoDocItem" runat="server" DataSourceID="ObjectDataSource4" Width="152px" __designer:wfdid="w107" DataTextField="Descripcion" DataValueField="IdClasificacion" SelectedValue='<%# Eval("TipoDoc") %>' Enabled="False"></asp:DropDownList>&nbsp; 
        </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="NroDoc" SortExpression="NroDoc"><EditItemTemplate>
        &nbsp;<asp:TextBox id="TextBox1" runat="server" Text='<%# Bind("NroDoc") %>' __designer:wfdid="w38"></asp:TextBox> <asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" __designer:wfdid="w39" ErrorMessage="Ingrese Nro Documento" ControlToValidate="TextBox1">*</asp:RequiredFieldValidator>
        </EditItemTemplate>
        <InsertItemTemplate>
        <asp:TextBox id="TextBox1" runat="server" Text='<%# Bind("NroDoc") %>' __designer:wfdid="w40"></asp:TextBox> <asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" __designer:wfdid="w41" ErrorMessage="Ingrese Nro Documento" ControlToValidate="TextBox1">*</asp:RequiredFieldValidator>
        </InsertItemTemplate>
        <ItemTemplate>
        <asp:Label id="Label4" runat="server" Text='<%# Bind("NroDoc") %>' __designer:wfdid="w37"></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="FechaNacimiento" SortExpression="FechaNacimiento"><EditItemTemplate>
        <asp:TextBox id="TxtEditFechaNac" runat="server" Text='<%# Bind("FechaNacimiento") %>' __designer:wfdid="w13"></asp:TextBox><asp:ImageButton id="ImageEditFechaNac" runat="Server" Width="19px" __designer:wfdid="w14" ImageUrl="~/Images/Calendar_scheduleHS.png" AlternateText="Clic para mostar Calendario"></asp:ImageButton> <ajaxToolkit:MaskedEditExtender id="MaskedEditExtender3" runat="server" __designer:wfdid="w15" TargetControlID="TxtEditFechaNac" MaskType="Date" OnInvalidCssClass="MaskedEditError" OnFocusCssClass="MaskedEditFocus" Mask="99/99/9999" ErrorTooltipEnabled="True" DisplayMoney="Left" CultureName="es-AR" AcceptNegative="Left"></ajaxToolkit:MaskedEditExtender> <ajaxToolkit:CalendarExtender id="CalendarExtender3" runat="server" __designer:wfdid="w16" TargetControlID="TxtEditFechaNac" Format="dd/MM/yyyy" PopupButtonID="ImageEditFechaNac"></ajaxToolkit:CalendarExtender> 
        </EditItemTemplate>
        <InsertItemTemplate>
        <asp:TextBox id="TxtInsertFechaNac" runat="server" Text='<%# Bind("FechaNacimiento") %>' __designer:wfdid="w18"></asp:TextBox> <asp:ImageButton id="ImageInsertFechaNac" runat="Server" Width="19px" __designer:wfdid="w19" ImageUrl="~/Images/Calendar_scheduleHS.png" AlternateText="Clic para mostar Calendario"></asp:ImageButton><ajaxToolkit:MaskedEditExtender id="MaskedEditExtender4" runat="server" __designer:wfdid="w20" TargetControlID="TxtInsertFechaNac" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True" DisplayMoney="Left" CultureName="es-AR" AcceptNegative="Left"></ajaxToolkit:MaskedEditExtender><ajaxToolkit:CalendarExtender id="CalendarExtender4" runat="server" __designer:wfdid="w21" TargetControlID="TxtInsertFechaNac" Format="dd/MM/yyyy" PopupButtonID="ImageInsertFechaNac"></ajaxToolkit:CalendarExtender> 
        </InsertItemTemplate>
        <ItemTemplate>
        <asp:Label id="Label3" runat="server" Text='<%# Bind("FechaNacimiento") %>' __designer:wfdid="w12"></asp:Label> 
        </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Sexo" SortExpression="Sexo"><EditItemTemplate>
        <asp:RadioButtonList id="RadioButtonList1" runat="server" Width="140px" __designer:wfdid="w24" RepeatDirection="Horizontal" SelectedValue='<%# Bind("Sexo", "{0}") %>'><asp:ListItem Value="False">Masculino</asp:ListItem>
        <asp:ListItem Value="True">Femenino</asp:ListItem>
        </asp:RadioButtonList> <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" __designer:wfdid="w27" ErrorMessage="*" ControlToValidate="RadioButtonList1"></asp:RequiredFieldValidator> 
        </EditItemTemplate>
        <InsertItemTemplate>
        <asp:RadioButtonList id="RadioButtonList1" runat="server" Width="140px" __designer:wfdid="w25" RepeatDirection="Horizontal" SelectedValue='<%# Bind("Sexo", "{0}") %>'><asp:ListItem Value="False">Masculino</asp:ListItem>
        <asp:ListItem Value="True">Femenino</asp:ListItem>
        </asp:RadioButtonList> 
        </InsertItemTemplate>
        <ItemTemplate>
        <asp:RadioButtonList id="RadioButtonList1" runat="server" Width="140px" __designer:wfdid="w23" RepeatDirection="Horizontal" Enabled="False" SelectedValue='<%# Bind("Sexo", "{0}") %>'><asp:ListItem Value="False">Masculino</asp:ListItem>
        <asp:ListItem Value="True">Femenino</asp:ListItem>
        </asp:RadioButtonList>&nbsp; 
        </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="EstadoCivil" SortExpression="EstadoCivil"><EditItemTemplate>
        <asp:DropDownList id="DDLEstCivilEdit" runat="server" DataSourceID="ObjectDataSource5" Width="152px" __designer:wfdid="w34" SelectedValue='<%# Bind("EstadoCivil") %>' DataValueField="IdClasificacion" DataTextField="Descripcion"></asp:DropDownList>&nbsp; 
        </EditItemTemplate>
        <InsertItemTemplate>
        <asp:DropDownList id="DDLEstCivilInsert" runat="server" DataSourceID="ObjectDataSource5" Width="152px" __designer:wfdid="w35" SelectedValue='<%# Bind("EstadoCivil") %>' DataValueField="IdClasificacion" DataTextField="Descripcion"></asp:DropDownList>&nbsp; 
        </InsertItemTemplate>
        <ItemTemplate>
        <asp:DropDownList id="DDLEstCivilItem" runat="server" DataSourceID="ObjectDataSource5" Width="152px" __designer:wfdid="w33" Enabled="False" SelectedValue='<%# Eval("EstadoCivil") %>' DataValueField="IdClasificacion" DataTextField="Descripcion"></asp:DropDownList>&nbsp; 
        </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="CUIL" HeaderText="CUIL" SortExpression="CUIL"></asp:BoundField>
        <asp:TemplateField HeaderText="Nacionalidad" SortExpression="Nacionalidad"><EditItemTemplate>
        <asp:DropDownList id="DDLNacionalidadEdit" runat="server" DataSourceID="ObjectDataSource3" Width="152px" __designer:wfdid="w37" SelectedValue='<%# Bind("Nacionalidad") %>' DataValueField="IdClasificacion" DataTextField="Descripcion"></asp:DropDownList> 
        </EditItemTemplate>
        <InsertItemTemplate>
        <asp:DropDownList id="DDLNacionalidadInsert" runat="server" DataSourceID="ObjectDataSource3" Width="152px" __designer:wfdid="w38" SelectedValue='<%# Bind("Nacionalidad") %>' DataValueField="IdClasificacion" DataTextField="Descripcion"></asp:DropDownList> 
        </InsertItemTemplate>
        <ItemTemplate>
        <asp:DropDownList id="DDLNacionalidadItem" runat="server" DataSourceID="ObjectDataSource3" Width="152px" __designer:wfdid="w36" Enabled="False" SelectedValue='<%# Eval("Nacionalidad") %>' DataValueField="IdClasificacion" DataTextField="Descripcion"><asp:ListItem Selected="True"></asp:ListItem>
        </asp:DropDownList> 
        </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Direccion" HeaderText="Direccion" SortExpression="Direccion"></asp:BoundField>
        <asp:BoundField DataField="CodigoPostal" HeaderText="CodigoPostal" SortExpression="CodigoPostal"></asp:BoundField>
        <asp:TemplateField HeaderText="Provincia" SortExpression="Provincia"><EditItemTemplate>
        <asp:DropDownList id="DDLProvinciaEdit" runat="server" DataSourceID="ObjectDataSource6" Width="152px" __designer:wfdid="w40" SelectedValue='<%# Bind("Provincia") %>' DataValueField="IdClasificacion" DataTextField="Descripcion"></asp:DropDownList>&nbsp; 
        </EditItemTemplate>
        <InsertItemTemplate>
        &nbsp;<asp:DropDownList id="DDLProvinciaInsert" runat="server" DataSourceID="ObjectDataSource6" Width="152px" __designer:wfdid="w41" SelectedValue='<%# Bind("Provincia") %>' DataValueField="IdClasificacion" DataTextField="Descripcion"></asp:DropDownList> 
        </InsertItemTemplate>
        <ItemTemplate>
        <asp:DropDownList id="DDLProvinciaItem" runat="server" DataSourceID="ObjectDataSource6" Width="152px" __designer:wfdid="w39" Enabled="False" SelectedValue='<%# Eval("Provincia") %>' DataValueField="IdClasificacion" DataTextField="Descripcion"></asp:DropDownList>&nbsp; 
        </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="TelefonoFijo" HeaderText="TelefonoFijo" SortExpression="TelefonoFijo"></asp:BoundField>
        <asp:BoundField DataField="CorreoElectronico" HeaderText="CorreoElectronico" SortExpression="CorreoElectronico"></asp:BoundField>
        </Fields>
        </asp:DetailsView> </TD></TR></TBODY></TABLE><asp:ObjectDataSource id="ObjectDataSource1" runat="server" TypeName="DSConosudTableAdapters.LegajosTableAdapter" SelectMethod="GetLegajosCompleto" OldValuesParameterFormatString="original_{0}" UpdateMethod="Update" InsertMethod="Insert" DeleteMethod="Delete"><DeleteParameters>
<asp:Parameter Name="IdLegajos" Type="Int32"></asp:Parameter>
</DeleteParameters>
<UpdateParameters>
<asp:Parameter Name="Apellido" Type="String"></asp:Parameter>
<asp:Parameter Name="Nombre" Type="String"></asp:Parameter>
<asp:Parameter Name="TipoDoc" Type="Int64"></asp:Parameter>
<asp:Parameter Name="NroDoc" Type="String"></asp:Parameter>
<asp:Parameter Name="FechaNacimiento" Type="DateTime"></asp:Parameter>
<asp:Parameter Name="Sexo" Type="Boolean"></asp:Parameter>
<asp:Parameter Name="EstadoCivil" Type="Int64"></asp:Parameter>
<asp:Parameter Name="CUIL" Type="String"></asp:Parameter>
<asp:Parameter Name="Nacionalidad" Type="Int64"></asp:Parameter>
<asp:Parameter Name="Direccion" Type="String"></asp:Parameter>
<asp:Parameter Name="CodigoPostal" Type="String"></asp:Parameter>
<asp:Parameter Name="Provincia" Type="Int64"></asp:Parameter>
<asp:Parameter Name="TelefonoFijo" Type="String"></asp:Parameter>
<asp:Parameter Name="CorreoElectronico" Type="String"></asp:Parameter>
<asp:Parameter Name="IdLegajos" Type="Int32"></asp:Parameter>
</UpdateParameters>
<InsertParameters>
<asp:Parameter Name="Apellido" Type="String"></asp:Parameter>
<asp:Parameter Name="Nombre" Type="String"></asp:Parameter>
<asp:Parameter Name="TipoDoc" Type="Int64"></asp:Parameter>
<asp:Parameter Name="NroDoc" Type="String"></asp:Parameter>
<asp:Parameter Name="FechaNacimiento" Type="DateTime"></asp:Parameter>
<asp:Parameter Name="Sexo" Type="Boolean"></asp:Parameter>
<asp:Parameter Name="EstadoCivil" Type="Int64"></asp:Parameter>
<asp:Parameter Name="CUIL" Type="String"></asp:Parameter>
<asp:Parameter Name="Nacionalidad" Type="Int64"></asp:Parameter>
<asp:Parameter Name="Direccion" Type="String"></asp:Parameter>
<asp:Parameter Name="CodigoPostal" Type="String"></asp:Parameter>
<asp:Parameter Name="Provincia" Type="Int64"></asp:Parameter>
<asp:Parameter Name="TelefonoFijo" Type="String"></asp:Parameter>
<asp:Parameter Name="CorreoElectronico" Type="String"></asp:Parameter>
</InsertParameters>
</asp:ObjectDataSource> <asp:ObjectDataSource id="ObjectDataSource2" runat="server" TypeName="DSConosudTableAdapters.LegajosTableAdapter" SelectMethod="GetDataByIdLegajos" UpdateMethod="Update" InsertMethod="Insert" DeleteMethod="Delete"><DeleteParameters>
<asp:Parameter Type="Int32" Name="IdLegajos"></asp:Parameter>
</DeleteParameters>
<UpdateParameters>
<asp:Parameter Type="String" Name="Apellido"></asp:Parameter>
<asp:Parameter Type="String" Name="Nombre"></asp:Parameter>
<asp:Parameter Type="Int64" Name="TipoDoc"></asp:Parameter>
<asp:Parameter Type="String" Name="NroDoc"></asp:Parameter>
<asp:Parameter Type="DateTime" Name="FechaNacimiento"></asp:Parameter>
<asp:Parameter Type="Boolean" Name="Sexo"></asp:Parameter>
<asp:Parameter Type="Int64" Name="EstadoCivil"></asp:Parameter>
<asp:Parameter Type="String" Name="CUIL"></asp:Parameter>
<asp:Parameter Type="Int64" Name="Nacionalidad"></asp:Parameter>
<asp:Parameter Type="String" Name="Direccion"></asp:Parameter>
<asp:Parameter Type="String" Name="CodigoPostal"></asp:Parameter>
<asp:Parameter Type="Int64" Name="Provincia"></asp:Parameter>
<asp:Parameter Type="String" Name="TelefonoFijo"></asp:Parameter>
<asp:Parameter Type="String" Name="CorreoElectronico"></asp:Parameter>
<asp:Parameter Type="Int32" Name="IdLegajos"></asp:Parameter>
</UpdateParameters>
<SelectParameters>
<asp:ControlParameter PropertyName="SelectedValue" Type="Int32" Name="IdLegajos" ControlID="gvLegajos"></asp:ControlParameter>
</SelectParameters>
<InsertParameters>
<asp:Parameter Type="String" Name="Apellido"></asp:Parameter>
<asp:Parameter Type="String" Name="Nombre"></asp:Parameter>
<asp:Parameter Type="Int64" Name="TipoDoc"></asp:Parameter>
<asp:Parameter Type="String" Name="NroDoc"></asp:Parameter>
<asp:Parameter Type="DateTime" Name="FechaNacimiento"></asp:Parameter>
<asp:Parameter Type="Boolean" Name="Sexo"></asp:Parameter>
<asp:Parameter Type="Int64" Name="EstadoCivil"></asp:Parameter>
<asp:Parameter Type="String" Name="CUIL"></asp:Parameter>
<asp:Parameter Type="Int64" Name="Nacionalidad"></asp:Parameter>
<asp:Parameter Type="String" Name="Direccion"></asp:Parameter>
<asp:Parameter Type="String" Name="CodigoPostal"></asp:Parameter>
<asp:Parameter Type="Int64" Name="Provincia"></asp:Parameter>
<asp:Parameter Type="String" Name="TelefonoFijo"></asp:Parameter>
<asp:Parameter Type="String" Name="CorreoElectronico"></asp:Parameter>
</InsertParameters>
</asp:ObjectDataSource> <asp:ObjectDataSource id="ObjectDataSource3" runat="server" TypeName="DSConosudTableAdapters.ClasificacionTableAdapter" SelectMethod="GetDataNacionalidad" OldValuesParameterFormatString="original_{0}" __designer:wfdid="w5"><DeleteParameters>
<asp:Parameter Type="Int32" Name="IdClasificacion"></asp:Parameter>
</DeleteParameters>
<UpdateParameters>
<asp:Parameter Type="String" Name="Descripcion"></asp:Parameter>
<asp:Parameter Type="Int64" Name="IdPadre"></asp:Parameter>
<asp:Parameter Type="String" Name="Codigo"></asp:Parameter>
<asp:Parameter Type="String" Name="Tipo"></asp:Parameter>
<asp:Parameter Type="Int32" Name="IdClasificacion"></asp:Parameter>
</UpdateParameters>
<InsertParameters>
<asp:Parameter Type="String" Name="Descripcion"></asp:Parameter>
<asp:Parameter Type="Int64" Name="IdPadre"></asp:Parameter>
<asp:Parameter Type="String" Name="Codigo"></asp:Parameter>
<asp:Parameter Type="String" Name="Tipo"></asp:Parameter>
</InsertParameters>
</asp:ObjectDataSource> <asp:ObjectDataSource id="ObjectDataSource4" runat="server" TypeName="DSConosudTableAdapters.ClasificacionTableAdapter" SelectMethod="GetDataTipoDoc" OldValuesParameterFormatString="original_{0}" UpdateMethod="Update" InsertMethod="Insert" DeleteMethod="Delete" __designer:wfdid="w21"><DeleteParameters>
<asp:Parameter Type="Int32" Name="IdClasificacion"></asp:Parameter>
</DeleteParameters>
<UpdateParameters>
<asp:Parameter Type="String" Name="Descripcion"></asp:Parameter>
<asp:Parameter Type="Int64" Name="IdPadre"></asp:Parameter>
<asp:Parameter Type="String" Name="Codigo"></asp:Parameter>
<asp:Parameter Type="String" Name="Tipo"></asp:Parameter>
<asp:Parameter Type="Int32" Name="IdClasificacion"></asp:Parameter>
</UpdateParameters>
<InsertParameters>
<asp:Parameter Type="String" Name="Descripcion"></asp:Parameter>
<asp:Parameter Type="Int64" Name="IdPadre"></asp:Parameter>
<asp:Parameter Type="String" Name="Codigo"></asp:Parameter>
<asp:Parameter Type="String" Name="Tipo"></asp:Parameter>
</InsertParameters>
</asp:ObjectDataSource> <asp:ObjectDataSource id="ObjectDataSource5" runat="server" TypeName="DSConosudTableAdapters.ClasificacionTableAdapter" SelectMethod="GetDataEstCivil" OldValuesParameterFormatString="original_{0}" __designer:wfdid="w22"><InsertParameters>
<asp:Parameter Type="String" Name="Descripcion"></asp:Parameter>
<asp:Parameter Type="Int64" Name="IdPadre"></asp:Parameter>
<asp:Parameter Type="String" Name="Codigo"></asp:Parameter>
<asp:Parameter Type="String" Name="Tipo"></asp:Parameter>
</InsertParameters>
</asp:ObjectDataSource> <asp:ObjectDataSource id="ObjectDataSource6" runat="server" TypeName="DSConosudTableAdapters.ClasificacionTableAdapter" SelectMethod="GetDataProvincia" OldValuesParameterFormatString="original_{0}" __designer:wfdid="w39"><DeleteParameters>
<asp:Parameter Type="Int32" Name="IdClasificacion"></asp:Parameter>
</DeleteParameters>
<UpdateParameters>
<asp:Parameter Type="String" Name="Descripcion"></asp:Parameter>
<asp:Parameter Type="Int64" Name="IdPadre"></asp:Parameter>
<asp:Parameter Type="String" Name="Codigo"></asp:Parameter>
<asp:Parameter Type="String" Name="Tipo"></asp:Parameter>
<asp:Parameter Type="Int32" Name="IdClasificacion"></asp:Parameter>
</UpdateParameters>
<InsertParameters>
<asp:Parameter Type="String" Name="Descripcion"></asp:Parameter>
<asp:Parameter Type="Int64" Name="IdPadre"></asp:Parameter>
<asp:Parameter Type="String" Name="Codigo"></asp:Parameter>
<asp:Parameter Type="String" Name="Tipo"></asp:Parameter>
</InsertParameters>
</asp:ObjectDataSource>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox id="Date5" runat="server" Width="36px" ForeColor="Red" __designer:wfdid="w142" Visible="False"></asp:TextBox><ajaxToolkit:CalendarExtender id="calendarButtonExtender" runat="server" __designer:wfdid="w143" TargetControlID="Date5"></ajaxToolkit:CalendarExtender><BR />
</ContentTemplate>
</asp:UpdatePanel>

<asp:UpdateProgress ID="UpdateProgress2" runat="server"  DisplayAfter=0 >
    <ProgressTemplate>
        <div style="left: 415px; width: 216px; line-height: 0pt; position: absolute; top: 350px;
            background-color: white; z-index: 20; border-right: #3399ff thin solid; border-top: #3399ff thin solid; vertical-align: middle; border-left: #3399ff thin solid; border-bottom: #3399ff thin solid; text-align: center;">
            <table border="0" cellpadding="0" cellspacing="0" style="height: 62px">
                <tr>
                    <td>
                        <img src="images/indicator.gif" />                
                    </td>
                </tr>
                <tr>
                    <td align="center" style="height: 31px; border-top-width: thin; border-left-width: thin; border-left-color: #6699ff; border-bottom-width: thin; border-bottom-color: #6699ff; border-top-color: #6699ff; border-right-width: thin; border-right-color: #6699ff;">
                        <asp:Label ID="lbltitulopaciente" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="8pt"
                            ForeColor="Black" Height="21px" Style="vertical-align: middle" Text="Cargando .....">
                        </asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>

</asp:Content>

