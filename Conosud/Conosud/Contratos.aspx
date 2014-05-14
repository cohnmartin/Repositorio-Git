<%@ Page Language="C#" Theme="MiTema" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="Contratos.aspx.cs" Inherits="Contratos" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function NoSubmit() {
            if (event.which || event.keyCode) {
                if ((event.which == 13) || (event.keyCode == 13)) {
                    event.keyCode = 9;
                }
            }
        }

        function rowBind() {
            var navRoot, node, dgRowid = null, tBody, rwVal, IdSelect;
            var Prev = 1, Curr = 1;
            if (document.all && document.getElementById) {
                navRoot = document.getElementById('ctl00_ContentPlaceHolder1_gvContratos');
                // Can Be Used For Both DataGrid & GridView,
                //Have To Give The Cleint ControlId.      


                if (navRoot != null) {
                    tBody = navRoot.childNodes[0];
                    for (i = 0; i < tBody.childNodes.length; i++) {
                        node = tBody.childNodes[i];
                        if (node.tagName == "TR") {
                            node.onmouseover = function() {
                                dtRowid = this.rowIndex;
                                if (Curr != null && Curr > 0 && Curr != dtRowid) {
                                    node.parentElement.children[dtRowid].style.background = "#CCCCCC";
                                    node.parentElement.children[dtRowid].style.cursor = 'hand';
                                }
                            }
                            node.onmouseout = function() {
                                dtRowid = this.rowIndex;
                                if (Curr != null && Curr > 0 && Curr != dtRowid) {
                                    node.parentElement.children[dtRowid].style.background = "#F1DCDC";
                                    node.parentElement.children[dtRowid].style.color = "#B5494A";
                                    node.parentElement.children[dtRowid].style.cursor = 'hand';
                                }
                            }

                            node.onmousedown = function() {
                                dtRowid = this.rowIndex;
                                rVal = dtRowid + 1;

                                if (rVal < 9) {
                                    rwVal = 'ctl00_ContentPlaceHolder1_gvContratos_ctl0' + rVal + '_lblIdContrato';
                                }
                                else {
                                    rwVal = 'ctl00_ContentPlaceHolder1_gvContratos_ctl' + rVal + '_lblIdContrato';
                                }

                                var ctrlText = document.getElementById('ctl00_ContentPlaceHolder1_txtIdContrato');

                                IdSelect = document.getElementById(rwVal).innerText;
                                ctrlText.value = IdSelect;

                                if (Prev != null) {
                                    node.parentElement.children[Prev].style.background = "#F1DCDC";
                                    node.parentElement.children[Prev].style.color = "#B5494A";
                                }
                                Curr = this.rowIndex;
                                if (Curr != null) {
                                    node.parentElement.children[Curr].style.background = "#B5494A";
                                    node.parentElement.children[Curr].style.color = "#F1DCDC";
                                    Prev = Curr;
                                    Editar();
                                }

                            }
                        }
                    }
                }
            }
        }    

    </script>

    <%--<asp:UpdateProgress id="UpdateProgress1" runat="server">
    <progresstemplate>
      <DIV class="progress"><IMG src="images/indicator.gif" /> Cargando ..... </DIV>
    </progresstemplate>
</asp:UpdateProgress>--%>
    <table cellpadding="0" cellspacing="5">
        <tr>
            <td align="center" style="width: 241px">
                <asp:Label ID="lblEncabezado" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Font-Bold="True" Font-Size="14pt" ForeColor="Maroon" Height="25px"
                    Style="background-image: url(images/FondoTitulos.gif); background-color: transparent"
                    Text="GESTION DE CONTRATOS" Width="296px"></asp:Label>
            </td>
        </tr>
    </table>
    <table style="width: 229px">
        <tbody>
            <tr>
                <td style="width: 392px" valign="top">
                    <asp:UpdatePanel ID="upGrilla" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gvContratos" runat="server" Width="525px" PageSize="5" AllowPaging="True"
                                AutoGenerateColumns="False" DataKeyNames="IdContrato" OnRowDataBound="gvContratos_RowDataBound"
                                OnSelectedIndexChanged="gvContratos_SelectedIndexChanged" OnPageIndexChanged="gvContratos_PageIndexChanged"
                                OnPageIndexChanging="gvContratos_PageIndexChanging">
                                <Columns>
                                    <asp:CommandField SelectImageUrl="~/images/right_16x16Conosud.gif" ButtonType="Image"
                                        ShowSelectButton="True"></asp:CommandField>
                                    <asp:BoundField DataField="Codigo" SortExpression="Codigo" HeaderText="C&#243;digo">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RazonSocial" SortExpression="RazonSocial" HeaderText="Empresa"
                                        Visible="False"></asp:BoundField>
                                    <asp:BoundField DataField="Servicio" SortExpression="Servicio" HeaderText="Servicio">
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HtmlEncode="False" DataFormatString="{0:d}" DataField="FechaInicio"
                                        SortExpression="FechaInicio" HeaderText="F. Inicio"></asp:BoundField>
                                    <asp:BoundField HtmlEncode="False" DataFormatString="{0:d}" DataField="FechaVencimiento"
                                        SortExpression="FechaVencimiento" HeaderText="F. Vencimiento"></asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label Style="display: none" ID="lblIdContrato" runat="server" Text='<%# Bind("IdContrato") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:TextBox Style="display: none" ID="txtIdContrato" runat="server" AutoPostBack="True"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Button ID="btnEditar" runat="server" Text="Editar" Style="display: none"></asp:Button>
                </td>
                <td style="width: 440px" valign="top">
                    <asp:UpdatePanel ID="upDetalle" runat="server">
                        <ContentTemplate>
                            <asp:DetailsView ID="dvContratos" runat="server" Width="432px" DataSourceID="ODSContratos"
                                Height="50px" DataKeyNames="IdContrato" HorizontalAlign="Left" OnDataBound="dvContratos_DataBound"
                                OnItemInserting="dvContratos_ItemInserting" AutoGenerateRows="False" OnItemDeleted="dvContratos_ItemDeleted"
                                OnItemUpdated="dvContratos_ItemUpdated" OnItemDeleting="dvContratos_ItemDeleting"
                                OnItemUpdating="dvContratos_ItemUpdating" OnItemCreated="dvContratos_ItemCreated"
                                __designer:wfdid="w2" EmptyDataText="Seleccione un Contrato">
                                <FooterTemplate>
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="White" Width="318px"
                                        DisplayMode="List" HeaderText="Escribir un valor en los siguientes campos:">
                                    </asp:ValidationSummary>
                                    &nbsp;&nbsp;&nbsp;
                                </FooterTemplate>
                                <Fields>
                                    <asp:CommandField CancelImageUrl="~/images/delete_16x16.gif" CancelText="" DeleteImageUrl="~/images/delete_16x16.gif"
                                        DeleteText="" EditImageUrl="~/images/edit_16x16.gif" EditText="" InsertImageUrl="~/images/ok_16x16.gif"
                                        InsertText="" NewImageUrl="~/images/add_16x16.gif" NewText="" ShowDeleteButton="True"
                                        ShowEditButton="True" ShowInsertButton="True" UpdateImageUrl="~/images/ok_16x16.gif"
                                        ButtonType="Image"></asp:CommandField>
                                    <asp:TemplateField HeaderText="C&#243;digo" SortExpression="Codigo">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditCodigo" runat="server" Text='<%# Bind("Codigo") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtEditCodigo"
                                                ErrorMessage="Código">*</asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:TextBox ID="txtInsertCodigo" runat="server" Text='<%# Bind("Codigo") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtInsertCodigo"
                                                ErrorMessage="Código">*</asp:RequiredFieldValidator>
                                        </InsertItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Codigo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Servicio" SortExpression="Servicio">
                                        <EditItemTemplate>
                                            <asp:TextBox Style="text-transform: capitalize;" ID="TextBox2" runat="server" Width="311px"
                                                Text='<%# Bind("Servicio") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:TextBox ID="TextBox2" runat="server" Width="305px" Text='<%# Bind("Servicio") %>'></asp:TextBox>
                                        </InsertItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Servicio") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Inicio" SortExpression="FechaInicio">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TxtEditFechaInicio" runat="server" Text='<%# Bind("FechaInicio") %>'></asp:TextBox>
                                            <asp:ImageButton ID="ImageEditFechaInicio" runat="Server" Width="19px" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                AlternateText="Clic para mostar Calendario"></asp:ImageButton>&nbsp;
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender3" runat="server" TargetControlID="TxtEditFechaInicio"
                                                MaskType="Date" OnInvalidCssClass="MaskedEditError" OnFocusCssClass="MaskedEditFocus"
                                                Mask="99/99/9999" ErrorTooltipEnabled="True" DisplayMoney="Left" CultureName="es-AR"
                                                AcceptNegative="Left">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="TxtEditFechaInicio"
                                                Format="dd/MM/yyyy" PopupButtonID="ImageEditFechaInicio">
                                            </ajaxToolkit:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Fecha Inicio"
                                                ControlToValidate="TxtEditFechaInicio">*</asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:TextBox ID="TxtInsertFechaInicio" runat="server" Text='<%# Bind("FechaInicio") %>'></asp:TextBox>
                                            <asp:ImageButton ID="ImageInsertFechaInicio" runat="Server" Width="19px" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                AlternateText="Clic para mostar Calendario"></asp:ImageButton>&nbsp;
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender4" runat="server" TargetControlID="TxtInsertFechaInicio"
                                                MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True" DisplayMoney="Left"
                                                CultureName="es-AR" AcceptNegative="Left">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="TxtInsertFechaInicio"
                                                Format="dd/MM/yyyy" PopupButtonID="Image">
                                            </ajaxToolkit:CalendarExtender>
                                            &nbsp;&nbsp;
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Fecha Inicio"
                                                ControlToValidate="TxtInsertFechaInicio">*</asp:RequiredFieldValidator>
                                        </InsertItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("FechaInicio") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vencimiento" SortExpression="FechaVencimiento">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TxtEditFechaVencimiento" runat="server" Text='<%# Bind("FechaVencimiento") %>'></asp:TextBox>
                                            <asp:ImageButton ID="ImageEditFechaVencimiento" runat="Server" Width="19px" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                AlternateText="Clic para mostar Calendario"></asp:ImageButton>&nbsp;
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="TxtEditFechaVencimiento"
                                                MaskType="Date" OnInvalidCssClass="MaskedEditError" OnFocusCssClass="MaskedEditFocus"
                                                Mask="99/99/9999" ErrorTooltipEnabled="True" DisplayMoney="Left" CultureName="es-AR"
                                                AcceptNegative="Left">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TxtEditFechaVencimiento"
                                                Format="dd/MM/yyyy" PopupButtonID="ImageEditFechaVencimiento">
                                            </ajaxToolkit:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Fecha Vencimiento"
                                                ControlToValidate="TxtEditFechaVencimiento">*</asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:TextBox ID="TxtInsertFechaVencimiento" runat="server" Text='<%# Bind("FechaVencimiento") %>'></asp:TextBox>
                                            <asp:ImageButton ID="ImageInsertFechaVencimiento" runat="Server" Width="19px" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                AlternateText="Clic para mostar Calendario"></asp:ImageButton>&nbsp;
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="TxtInsertFechaVencimiento"
                                                MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True" DisplayMoney="Left"
                                                CultureName="es-AR" AcceptNegative="Left">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="TxtInsertFechaVencimiento"
                                                Format="dd/MM/yyyy" PopupButtonID="ImageInsertFechaVencimiento">
                                            </ajaxToolkit:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Fecha Vencimiento"
                                                ControlToValidate="TxtInsertFechaVencimiento">*</asp:RequiredFieldValidator>&nbsp;
                                        </InsertItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("FechaVencimiento") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Prorroga" SortExpression="Prorroga">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TxtEditProrroga" runat="server" Text='<%# Bind("Prorroga") %>'></asp:TextBox>
                                            <asp:ImageButton ID="ImageEditProrroga" runat="Server" Width="19px" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                AlternateText="Clic para mostar Calendario"></asp:ImageButton>&nbsp;
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender5" runat="server" TargetControlID="TxtEditProrroga"
                                                MaskType="Date" OnInvalidCssClass="MaskedEditError" OnFocusCssClass="MaskedEditFocus"
                                                Mask="99/99/9999" ErrorTooltipEnabled="True" DisplayMoney="Left" CultureName="es-AR"
                                                AcceptNegative="Left">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="TxtEditProrroga"
                                                Format="dd/MM/yyyy" PopupButtonID="ImageEditProrroga">
                                            </ajaxToolkit:CalendarExtender>
                                            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="La prorroga debera ser mayor al Cencimiento"
                                                ControlToValidate="TxtEditProrroga" ControlToCompare="TxtEditFechaVencimiento"
                                                Type="Date" Operator="GreaterThanEqual">*</asp:CompareValidator>&nbsp;
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:TextBox ID="TxtInsertProrroga" runat="server" Text='<%# Bind("Prorroga") %>'></asp:TextBox>
                                            <asp:ImageButton ID="ImageInsertProrroga" runat="Server" Width="19px" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                AlternateText="Clic para mostar Calendario"></asp:ImageButton>&nbsp;
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender6" runat="server" TargetControlID="TxtInsertProrroga"
                                                MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True" DisplayMoney="Left"
                                                CultureName="es-AR" AcceptNegative="Left">
                                            </ajaxToolkit:MaskedEditExtender>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender6" runat="server" TargetControlID="TxtInsertProrroga"
                                                Format="dd/MM/yyyy" PopupButtonID="ImageInsertProrroga">
                                            </ajaxToolkit:CalendarExtender>
                                            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="La prorroga debera ser mayor al Cencimiento"
                                                ControlToValidate="TxtInsertProrroga" ControlToCompare="TxtInsertFechaVencimiento"
                                                Type="Date" Operator="GreaterThanEqual">*</asp:CompareValidator>&nbsp;&nbsp;&nbsp;
                                        </InsertItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("Prorroga") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tipo" SortExpression="TipoContrato">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="DDLEditTipoContrato" runat="server" Width="311px" DataSourceID="ODSTipoCon"
                                                SelectedValue='<%# Bind("TipoContrato") %>' DataValueField="IdClasificacion"
                                                DataTextField="Descripcion" AppendDataBoundItems="True">
                                                <asp:ListItem>(ninguno)</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DropDownList ID="DDLInsertTipoContrato" runat="server" Width="311px" DataSourceID="ODSTipoCon"
                                                SelectedValue='<%# Bind("TipoContrato") %>' DataValueField="IdClasificacion"
                                                DataTextField="Descripcion" AppendDataBoundItems="True">
                                                <asp:ListItem>(ninguno)</asp:ListItem>
                                            </asp:DropDownList>
                                        </InsertItemTemplate>
                                        <ItemTemplate>
                                            <asp:DropDownList ID="DDLItemTipoContrato" runat="server" Width="311px" DataSourceID="ODSTipoCon"
                                                SelectedValue='<%# Eval("TipoContrato") %>' DataValueField="IdClasificacion"
                                                DataTextField="Descripcion" AppendDataBoundItems="True" Enabled="False">
                                                <asp:ListItem>(ninguno) </asp:ListItem>
                                            </asp:DropDownList>
                                            &nbsp;
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Contratado Por" SortExpression="Contratadopor">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="DDLEditTContratadopor" runat="server" Width="311px" DataSourceID="ODSContrapor"
                                                SelectedValue='<%# Bind("Contratadopor") %>' DataValueField="IdClasificacion"
                                                DataTextField="Descripcion" AppendDataBoundItems="True">
                                                <asp:ListItem>(ninguno)</asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DropDownList ID="DDLInsertContratadopor" runat="server" Width="311px" DataSourceID="ODSContrapor"
                                                SelectedValue='<%# Bind("Contratadopor") %>' DataValueField="IdClasificacion"
                                                DataTextField="Descripcion" AppendDataBoundItems="True">
                                                <asp:ListItem>(ninguno)</asp:ListItem>
                                            </asp:DropDownList>
                                        </InsertItemTemplate>
                                        <ItemTemplate>
                                            <asp:DropDownList ID="DDLItemContratadopor" runat="server" Width="311px" DataSourceID="ODSContrapor"
                                                SelectedValue='<%# Eval("Contratadopor") %>' DataValueField="IdClasificacion"
                                                DataTextField="Descripcion" AppendDataBoundItems="True" Enabled="False">
                                                <asp:ListItem>(ninguno)</asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Contratista">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="DDLEditContratista" runat="server" Width="311px" DataSourceID="ODSEmpresa"
                                                DataValueField="IdEmpresa" DataTextField="RazonSocial">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DropDownList ID="DDLInsertContratista" runat="server" Width="311px" DataSourceID="ODSEmpresa"
                                                DataValueField="IdEmpresa" DataTextField="RazonSocial">
                                            </asp:DropDownList>
                                        </InsertItemTemplate>
                                        <ItemTemplate>
                                            <asp:DropDownList ID="DDLItemContratista" runat="server" Width="311px" DataSourceID="ODSEmpresa"
                                                DataValueField="IdEmpresa" DataTextField="RazonSocial" Enabled="False">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:TemplateField>
                                </Fields>
                            </asp:DetailsView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="dvContratos" EventName="ItemInserting"></asp:AsyncPostBackTrigger>
                            <asp:AsyncPostBackTrigger ControlID="dvContratos" EventName="ItemUpdating"></asp:AsyncPostBackTrigger>
                            <asp:AsyncPostBackTrigger ControlID="dvContratos" EventName="ItemDeleting"></asp:AsyncPostBackTrigger>
                            <asp:AsyncPostBackTrigger ControlID="btnEditar" EventName="Click"></asp:AsyncPostBackTrigger>
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </tbody>
    </table>
    <asp:ObjectDataSource ID="ODSSelContratos" runat="server" SelectMethod="GetDataByContratosCompleto"
        TypeName="DSConosudTableAdapters.ContratoTableAdapter" DeleteMethod="Delete"
        InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" UpdateMethod="Update">
        <DeleteParameters>
            <asp:Parameter Name="IdContrato" Type="Int64" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Codigo" Type="String" />
            <asp:Parameter Name="Servicio" Type="String" />
            <asp:Parameter Name="FechaInicio" Type="DateTime" />
            <asp:Parameter Name="FechaVencimiento" Type="DateTime" />
            <asp:Parameter Name="Prorroga" Type="DateTime" />
            <asp:Parameter Name="TipoContrato" Type="Int64" />
            <asp:Parameter Name="Contratadopor" Type="Int64" />
            <asp:Parameter Name="IdContrato" Type="Int64" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="Codigo" Type="String" />
            <asp:Parameter Name="Servicio" Type="String" />
            <asp:Parameter Name="FechaInicio" Type="DateTime" />
            <asp:Parameter Name="FechaVencimiento" Type="DateTime" />
            <asp:Parameter Name="Prorroga" Type="DateTime" />
            <asp:Parameter Name="TipoContrato" Type="Int64" />
            <asp:Parameter Name="Contratadopor" Type="Int64" />
        </InsertParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSContratos" runat="server" SelectMethod="GetDataByIdContrato"
        TypeName="DSConosudTableAdapters.ContratoTableAdapter" OnSelected="ODSContratos_Selected"
        OnUpdating="ODSContratos_Updating" DeleteMethod="Delete" InsertMethod="Insert"
        UpdateMethod="Update">
        <DeleteParameters>
            <asp:Parameter Name="IdContrato" Type="Int64" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Type="String" Name="Codigo"></asp:Parameter>
            <asp:Parameter Type="String" Name="Servicio"></asp:Parameter>
            <asp:Parameter Type="DateTime" Name="FechaInicio"></asp:Parameter>
            <asp:Parameter Type="DateTime" Name="FechaVencimiento"></asp:Parameter>
            <asp:Parameter Type="DateTime" Name="Prorroga"></asp:Parameter>
            <asp:Parameter Type="Int64" Name="TipoContrato"></asp:Parameter>
            <asp:Parameter Type="Int64" Name="Contratadopor"></asp:Parameter>
            <asp:Parameter Name="IdContrato" Type="Int64" />
        </UpdateParameters>
        <SelectParameters>
            <asp:ControlParameter PropertyName="SelectedValue" Type="Int32" Name="IdContrato"
                ControlID="gvContratos"></asp:ControlParameter>
        </SelectParameters>
        <InsertParameters>
            <asp:Parameter Type="String" Name="Codigo"></asp:Parameter>
            <asp:Parameter Type="String" Name="Servicio"></asp:Parameter>
            <asp:Parameter Type="DateTime" Name="FechaInicio"></asp:Parameter>
            <asp:Parameter Type="DateTime" Name="FechaVencimiento"></asp:Parameter>
            <asp:Parameter Type="DateTime" Name="Prorroga"></asp:Parameter>
            <asp:Parameter Type="Int64" Name="TipoContrato"></asp:Parameter>
            <asp:Parameter Type="Int64" Name="Contratadopor"></asp:Parameter>
        </InsertParameters>
    </asp:ObjectDataSource>
    &nbsp;
    <asp:TextBox ID="Date5" runat="server" Width="36px" ForeColor="Red" Visible="False"></asp:TextBox><ajaxToolkit:CalendarExtender
        ID="calendarButtonExtender" runat="server" TargetControlID="Date5">
    </ajaxToolkit:CalendarExtender>
    <asp:ObjectDataSource ID="ODSEmpresa" runat="server" SelectMethod="GetData" TypeName="DSConosudTableAdapters.EmpresaTableAdapter"
        OldValuesParameterFormatString="original_{0}"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSTipoCon" runat="server" SelectMethod="GetDataTipoContrato"
        TypeName="DSConosudTableAdapters.ClasificacionTableAdapter" OldValuesParameterFormatString="original_{0}">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSContrapor" runat="server" SelectMethod="GetDataContratadopor"
        TypeName="DSConosudTableAdapters.ClasificacionTableAdapter"></asp:ObjectDataSource>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="50">
        <ProgressTemplate>
            <div style="left: 415px; width: 216px; line-height: 0pt; position: absolute; top: 390px;
                background-color: white; z-index: 20; border-right: #3399ff thin solid; border-top: #3399ff thin solid;
                vertical-align: middle; border-left: #3399ff thin solid; border-bottom: #3399ff thin solid;
                text-align: center;">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 127px; height: 62px">
                    <tr>
                        <td align="center" style="height: 31px; border-top-width: thin; border-left-width: thin;
                            border-left-color: #6699ff; border-bottom-width: thin; border-bottom-color: #6699ff;
                            border-top-color: #6699ff; border-right-width: thin; border-right-color: #6699ff;">
                            <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Georgia" Font-Size="10pt"
                                ForeColor="Black" Height="21px" Style="vertical-align: middle" Text="Procesando Consulta..."
                                Width="179px"></asp:Label>
                            <img alt=" " src="images/indicator.gif"" />
                        </td>
                    </tr>
                </table>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
