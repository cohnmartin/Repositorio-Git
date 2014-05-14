<%@ Page Title="" Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="ABMLegajos.aspx.cs" Inherits="ABMLegajos" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <link href="ConosudSkin/ToolTip.ConosudSkin.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .MyImageButton
        {
            cursor: hand;
        }
    </style>
    <script type="text/javascript">
        function CrearLegajo() {

            document.getElementById("ctl00_ContentPlaceHolder1_HiddenId").value = "0";

            $find('<%= txtEmail.ClientID %>').clear();
            $find('<%= txtTelefono.ClientID %>').clear();
            $find('<%= txtCodigoPostal.ClientID %>').clear();
            $find('<%= txtDireccion.ClientID %>').clear();
            $find('<%= txtCuil.ClientID %>').clear();
            $find('<%= txtFechaNacimiento.ClientID %>').clear();
            $find('<%= txtNroDocumento.ClientID %>').clear();
            $find('<%= txtNombre.ClientID %>').clear();
            $find('<%= txtApellido.ClientID %>').clear();


            var tooltipo = $find("ctl00_ContentPlaceHolder1_toolTipEdicion");
            tooltipo.set_title("Cración Nuevo Legajo");
            tooltipo.show();
            
        }
        function PopupOpening(sender, args) {
            Telerik.Web.UI.Calendar.Popup.zIndex = 100100;
        }

        function HideTooTil() {
            var tooltipo = $find("ctl00_ContentPlaceHolder1_toolTipEdicion");
            tooltipo.hide();



        }
        function ControlarCarga() {
            return true;
        }

        function ShowToolTip() {

            var tooltipo = $find("ctl00_ContentPlaceHolder1_toolTipEdicion");
            tooltipo.show();
            return true;
        }
    </script>
    <table cellpadding="0" cellspacing="5" style="width: 80%">
        <tr>
            <td align="center"  style="height: 25px; background: url('images/sprite.gif') 0  -997px repeat-x" >
                <asp:Label ID="lblEncabezado" runat="server" 
                    Font-Bold="True" Font-Size="14pt" Font-Names="Sans-Serif" ForeColor="#E0D6BE" 
                    Text="Gestión de Legajos" Width="378px"></asp:Label>
            </td>
        </tr>
     </table>
    <table id="Table1" style="border: thin solid #843431; background-color: #E0D6BE;
        font-family: Sans-Serif; font-size: 11px; width: 394px;">
        <tr>
            <td align="right" style="width: 98px; height: 26px">
                <asp:Label ID="lblLegajo" runat="server" Font-Bold="True" ForeColor="Maroon" Height="22px"
                    Text="Legajo:" Width="79px"></asp:Label>
            </td>
            <td id="Td1" valign="middle" align="left" >
                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <telerik:RadTextBox ID="txtApellidoLegajo" runat="server" EmptyMessage="Ingrese apellido del legajo para buscar"
                            Skin="Sunset" Width="255px" OnTextChanged="txtApellidoLegajo_TextChanged" AutoPostBack="True">
                        </telerik:RadTextBox>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="50" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <div id="divBloq1" class="progressBackgroundFilterBlack">
                                </div>
                                <div class="processMessage">
                                    <table border="0" cellpadding="0" cellspacing="0" style="height: 62px;">
                                        <tr>
                                            <td align="center">
                                                <img alt="a" src="Images/LoadingSunset.gif" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lbltitulobusqueda" runat="server" Font-Bold="True" Font-Names="Thomas"
                                                    Font-Size="12px" ForeColor="white" Height="21px" Style="vertical-align: middle"
                                                    Text="Buscando legajos...">
                                                </asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    
    </table>
    <table id="Table2" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
        border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #E0D6BE;
        font-family: Sans-Serif; font-size: 11px;" width="90%" >
        <tr>
            <td align="center">
                <table width="100%" >
                    <tr>
                        <td colspan="2">
                            <asp:UpdatePanel ID="upGrilla" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvLegajos" runat="server" Skin="Sunset" AutoGenerateColumns="False"
                                        Width="770px" GridLines="None" OnItemCommand="gvLegajos_ItemCommand">
                                        <MasterTableView Width="770px" DataKeyNames="IdLegajos">
                                            <RowIndicatorColumn Visible="False">
                                                <HeaderStyle Width="20px"></HeaderStyle>
                                            </RowIndicatorColumn>
                                            <ExpandCollapseColumn Visible="False" Resizable="False">
                                                <HeaderStyle Width="20px"></HeaderStyle>
                                            </ExpandCollapseColumn>
                                            <EditFormSettings>
                                                <PopUpSettings ScrollBars="None"></PopUpSettings>
                                            </EditFormSettings>
                                            <Columns>
                                                
                                                <telerik:GridTemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:ImageButton Style="cursor: hand;" CausesValidation="false" ID="ImageButton1" runat="server" ImageUrl="images/Edit.gif" OnClientClick="return ShowToolTip(); " />
                                                        <itemstyle cssclass="MyImageButton" width="50" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="Apellido" HeaderText="Apellido" UniqueName="columnApellido">
                                                    <HeaderStyle />
                                                    <ItemStyle HorizontalAlign="Left" Width="290" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Nombre" HeaderText="Nombre" UniqueName="columnNombre">
                                                    <HeaderStyle />
                                                    <ItemStyle HorizontalAlign="Left" Width="290" />
                                                </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn DataField="objTipoDocumento.Descripcion" HeaderText="Tipo Doc." UniqueName="columnTipoDoc">
                                                    <HeaderStyle />
                                                    <ItemStyle HorizontalAlign="Left" Width="290" />
                                                </telerik:GridBoundColumn>
                                                
                                                 <telerik:GridBoundColumn DataField="NroDoc" HeaderText="Nro" UniqueName="columnNroDoc">
                                                    <HeaderStyle />
                                                    <ItemStyle HorizontalAlign="Left" Width="290" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="CUIL" HeaderText="CIUL" UniqueName="CUIL">
                                                    <ItemStyle HorizontalAlign="Left" Width="100" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="TelefonoFijo" HeaderText="Telefono" UniqueName="columnTelefonoFijo">
                                                    <ItemStyle HorizontalAlign="Left" Width="60" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridButtonColumn  ConfirmText="Desea eliminar este legajo?" ConfirmDialogType="RadWindow"
                                                    ConfirmTitle="Delete" ButtonType="ImageButton" CommandName="Delete" Text="Delete" 
                                                    UniqueName="DeleteColumn">
                                                    <ItemStyle HorizontalAlign="Right" CssClass="MyImageButton" />
                                                </telerik:GridButtonColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings>
                                            <Selecting AllowRowSelect="false" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="txtApellidoLegajo" EventName="TextChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <div style="width: 770px; background: url('images/sprite.gif') 0  -997px repeat-x;">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" style="border: 1px solid gray">
                                    <tr style="height: 25px; background: url('Imagenes/sprite_Sunset.gif') 0 -300px repeat-x;">
                                        <td style="width: 5%" align="right" valign="bottom">
                                            <asp:Image ID="Image2" runat="server" ImageUrl="Images/AddRecord.gif" />
                                        </td>
                                        <td style="vertical-align: middle" align="left">
                                            <asp:UpdatePanel ID="upEditar" UpdateMode="Conditional" runat="server">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnNueva" runat="server" BackColor="Transparent" Style="cursor: hand;"
                                                        BorderStyle="None" Height="18px" Font-Bold="true" Font-Names="Verdana" Font-Size="11px"
                                                        ForeColor="black" Text="Agregar Nuevo Legajo" Width="167px" CausesValidation="false"
                                                        OnClientClick="CrearLegajo(); return false;"  />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>


    <telerik:RadToolTip runat="server" ID="toolTipEdicion" 
        Skin="ConosudSkin" EnableEmbeddedSkins="false" Modal="true"
        Sticky="true" Width="400px" Height="280px" Title="Edición Legajo" Animation="None"
        Position="Center" RelativeTo="BrowserWindow" ShowEvent="FromCode" 
        VisibleOnPageLoad="false" >
        <asp:UpdatePanel ID="upToolTip" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <input id="HiddenId" type="hidden" runat="server" />
                <table width="100%" border="0" style="height: 100%; background-color: #f4ede1">
                    <tr>
                        <td class="style2">
                            <asp:Label ID="label2" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                font-size: 11px">Apellido:</asp:Label>
                        </td>
                        <td align="left">
                            <telerik:RadTextBox Width="256px" ID="txtApellido" runat="server" EmptyMessage="Ingrese Apellido del Legajo"
                                InvalidStyleDuration="100" MaxLength="250" Skin="Sunset">
                            </telerik:RadTextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtApellido"
                                ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="label9" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                font-size: 11px">Nombre:</asp:Label>
                        </td>
                        <td align="left">
                            <telerik:RadTextBox Width="256px" ID="txtNombre" runat="server" EmptyMessage="Ingrese Nombre del Legajo"
                                InvalidStyleDuration="100" MaxLength="250" Skin="Sunset">
                            </telerik:RadTextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtNombre"
                                ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="label3" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                font-size: 11px">Tipo Documento:</asp:Label>
                        </td>
                        <td align="left">
                            <telerik:RadComboBox ID="cboTipoDoc" runat="server" Skin="Sunset" ZIndex="100000">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="label4" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                font-size: 11px">Nro. Documento:</asp:Label>
                        </td>
                        <td align="left">
                            <telerik:RadTextBox Width="256px" ID="txtNroDocumento" runat="server" EmptyMessage="Ingrese el nro. Documento"
                                InvalidStyleDuration="100" MaxLength="10" Skin="Sunset">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="lblfechanacimiento" runat="server" Style="width: 100px; color: #0066CC;
                                font-family: Sans-Serif; font-size: 11px">Fecha Nacimiento:</asp:Label>
                        </td>
                        <td align="left">
                            <telerik:RadDatePicker ClientEvents-OnPopupOpening="PopupOpening" ID="txtFechaNacimiento"
                                runat="server" Skin="Sunset" MinDate="1900-01-01">
                                <DateInput ID="DateInput2" runat="server" DateFormat="d/MM/yyyy" DisplayDateFormat="d/MM/yyyy">
                                </DateInput>
                                <Calendar Skin="Sunset">
                                </Calendar>
                                <DatePopupButton CssClass="radPopupImage_Sunset"></DatePopupButton>
                            </telerik:RadDatePicker>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="label6" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                font-size: 11px">Sexo:</asp:Label>
                        </td>
                        <td align="left">
                            <telerik:RadComboBox ID="cboSexo" runat="server" Skin="Sunset" ZIndex="10000000">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="label7" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                font-size: 11px">Estado Civil:</asp:Label>
                        </td>
                        <td align="left">
                            <telerik:RadComboBox ID="cboEstadoCivil" runat="server" Skin="Sunset" ZIndex="10000000">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="label1" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                font-size: 11px">CUIL:</asp:Label>
                        </td>
                        <td align="left">
                            <telerik:RadMaskedTextBox Width="256px" ID="txtCuil" runat="server" EmptyMessage="Ingrese Cuit de la empresa"
                                InvalidStyleDuration="100" Mask="##-########-#" Skin="Sunset" DisplayMask="##-########-#">
                            </telerik:RadMaskedTextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCuil"
                                ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="label5" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                font-size: 11px">Nacionalidad:</asp:Label>
                        </td>
                        <td align="left">
                            <telerik:RadComboBox ID="cboNacionalidad" runat="server" Skin="Sunset" ZIndex="10000000">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="label10" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                font-size: 11px">Dirección:</asp:Label>
                        </td>
                        <td align="left">
                            <telerik:RadTextBox Width="256px" ID="txtDireccion" runat="server" EmptyMessage="Ingrese direccion completa"
                                InvalidStyleDuration="100" MaxLength="100" Skin="Sunset">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="label11" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                font-size: 11px">Código Postal:</asp:Label>
                        </td>
                        <td align="left">
                            <telerik:RadTextBox Width="256px" ID="txtCodigoPostal" runat="server" EmptyMessage="Ingrese el código postal"
                                InvalidStyleDuration="100" MaxLength="100" Skin="Sunset">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="label12" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                font-size: 11px">Provincia:</asp:Label>
                        </td>
                        <td align="left">
                            <telerik:RadComboBox ID="cboProvincia" runat="server" Skin="Sunset" ZIndex="10000000">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="label13" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                font-size: 11px">Teléfono:</asp:Label>
                        </td>
                        <td align="left">
                            <telerik:RadTextBox Width="256px" ID="txtTelefono" runat="server" EmptyMessage="Ingrese el teléfono del legajo"
                                InvalidStyleDuration="100" MaxLength="100" Skin="Sunset">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            <asp:Label ID="label8" runat="server" Style="width: 100px; color: #0066CC; font-family: Sans-Serif;
                                font-size: 11px">Correo Electrónico:</asp:Label>
                        </td>
                        <td align="left">
                            <telerik:RadTextBox Width="256px" ID="txtEmail" runat="server" EmptyMessage="Ingrese Correo Electrónico"
                                InvalidStyleDuration="100" MaxLength="100" Skin="Sunset">
                            </telerik:RadTextBox>
                            <asp:RegularExpressionValidator ID="regEmail" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                Display="Dynamic" ForeColor="Red" font-name="Arial" Font-Size="11" runat="server">*</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" SkinID="btnConosud"
                              OnClientClick="return ControlarCarga();" OnClick="btnAceptar_Click" CausesValidation="true" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" SkinID="btnConosud" CausesValidation="false"
                                OnClientClick="HideTooTil(); return false;" />
                        </td>
                    </tr>
                </table>
                <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="50" >
                    <ProgressTemplate>
                        <div id="divBloq1" class="progressBackgroundFilterBlackToolTip">
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
                                        <asp:Label ID="lbltituloCarga" runat="server" Font-Bold="True" Font-Names="Thomas"
                                            Font-Size="12px" ForeColor="black" Height="21px" Style="vertical-align: middle"
                                            Text="Cargando datos para la edición del legajo..">
                                        </asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </ContentTemplate>
        </asp:UpdatePanel>
    </telerik:RadToolTip>
</asp:Content>

