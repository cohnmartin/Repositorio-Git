<%@ Page Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="ConsultDocumentacion.aspx.cs" Inherits="ConsultDocumentacion" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .HandCursor
        {
            cursor: hand;
        }
    </style>
    <script type="text/javascript">
        var contratosCombo = $find("ctl00_ContentPlaceHolder1_cboContratos");

        function LoadContratos(combo, eventarqs) {
            var contratosCombo = $find("ctl00_ContentPlaceHolder1_cboContratos");
            var contratistasCombo = $find("ctl00_ContentPlaceHolder1_cboContratistas");
            var periodosCombo = $find("ctl00_ContentPlaceHolder1_cboPeriodos");

            var item = eventarqs.get_item();
            contratosCombo.set_text("Loading...");
            contratistasCombo.clearSelection();
            periodosCombo.clearSelection();

            if (item.get_index() > 0) {
                contratosCombo.requestItems(item.get_value(), false);
            }
            else {
                contratosCombo.set_text(" ");
                contratosCombo.clearItems();

                contratistasCombo.set_text(" ");
                contratistasCombo.clearItems();

                periodosCombo.set_text(" ");
                periodosCombo.clearItems();
            }
        }

        function LoadContratistas(combo, eventarqs) {
            var contratistasCombo = $find("ctl00_ContentPlaceHolder1_cboContratistas");
            var periodosCombo = $find("ctl00_ContentPlaceHolder1_cboPeriodos");

            var item = eventarqs.get_item();

            if (item.get_index() > 0) {
                periodosCombo.clearSelection();
                contratistasCombo.clearSelection();
                contratistasCombo.set_text("Loading...");
                contratistasCombo.requestItems(item.get_value(), false);

            }
            else {
                contratistasCombo.set_text(" ");
                contratistasCombo.clearItems();

                periodosCombo.set_text(" ");
                periodosCombo.clearItems();


            }


        }

        function LoadPeriodos(combo, eventarqs) {
            var periodosCombo = $find("ctl00_ContentPlaceHolder1_cboPeriodos");

            var item = eventarqs.get_item();

            if (item.get_index() > 0) {
                periodosCombo.set_text("Loading...");
                periodosCombo.requestItems(item.get_value(), false);
            }
            else {

                periodosCombo.set_text(" ");
                periodosCombo.clearItems();


            }


        }
        function ItemsLoaded(combo, eventarqs) {
            var contratosCombo = $find("ctl00_ContentPlaceHolder1_cboContratos");
            //var citiesCombo = $find("RadComboBox3");

            if (combo.get_items().get_count() > 0) {
                combo.set_text(combo.get_items().getItem(0).get_text());
                combo.get_items().getItem(0).highlight();
            }
            combo.showDropDown();
        }


        function Marcar(chk) {

            var grid = $find("<%= gvItemHoja.ClientID%>");
            var MasterTable = grid.get_masterTableView();
            var row = MasterTable.get_dataItems()[chk.parentElement.parentElement.parentElement.rowIndex - 1];
            var cell = MasterTable.getCellByColumnUniqueName(row, "FechaRecepcion");


            if (chk.checked) {
                cell.innerText = "<%= DateTime.Now.ToShortDateString() %>";
            }
            else {
                if (chk.parentElement.getAttribute("originalValue") != null) {
                    cell.innerText = chk.parentElement.getAttribute("originalValue").substring(0, chk.parentElement.getAttribute("originalValue").indexOf(" "));
                }
                else
                    cell.innerText = " ";

            }

        }

        function showToolTip(chk) {

            var tooltip = $find("<%= ToolTipComentario.ClientID %>");

            if (chk.getAttribute("coment") != "") {
                tooltip.set_text(chk.getAttribute("coment"));
            }
            else {
                tooltip.set_text("No hay comentario Ingresado");
            }

            tooltip.set_targetControlID(chk.id);
            tooltip.show();
        }


    </script>
    <script type="text/javascript">
        var currentTextBox = null;
        var currentDatePicker = null;

        
        //this handler is used to set the text of the TextBox to the value of selected from the popup 
        function dateSelected(sender, args) {
            if (currentTextBox != null) {
                //currentTextBox is the currently selected TextBox. Its value is set to the newly selected
                //value of the picker
                currentTextBox.value = args.get_newValue();
            }
        }

        //this function is used to parse the date entered or selected by the user
        function parseDate(sender, e) {
            if (currentDatePicker != null) {
                var date = currentDatePicker.get_dateInput().parseDate(sender.value);
                var dateInput = currentDatePicker.get_dateInput();

                if (date == null) {
                    date = currentDatePicker.get_selectedDate();
                }

                var formattedDate = dateInput.get_dateFormatInfo().FormatDate(date, dateInput.get_displayDateFormat());
                sender.value = formattedDate;
            }
        }

        function CargarGrilla() {
            var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");
            ajaxManager.ajaxRequest("ActualizarGrilla");
        }

    
    </script>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
    </telerik:RadAjaxManager>
    <table cellpadding="0" cellspacing="5" style="width: 80%">
        <tr>
            <td align="center" style="height: 25px; background: url('images/sprite.gif') 0  -997px repeat-x">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="14pt" Font-Names="Sans-Serif"
                    ForeColor="#E0D6BE" Text="Recepción de Documentación" Width="378px"></asp:Label>
            </td>
        </tr>
    </table>
    <table id="Table1" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
        border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #E0D6BE;
        font-family: Sans-Serif; font-size: 11px;">
        <tr>
            <td align="right" style="width: 98px; height: 26px">
                <asp:Label ID="lblEmpresa" runat="server" Font-Bold="True" ForeColor="Maroon" Height="22px"
                    Text="Empresa:" Width="79px"></asp:Label>
            </td>
            <td id="Td1" align="left" style="width: 95px; height: 26px">
                <telerik:RadComboBox ID="cboEmpresas" runat="server" Skin="Sunset" Width="200px"
                    AllowCustomText="true" MarkFirstMatch="true" OnClientSelectedIndexChanging="LoadContratos"
                    OnItemsRequested="cboEmpresas_ItemsRequested" />
            </td>
            <td align="right" style="width: 34px">
                <asp:Label ID="lblContr" runat="server" Font-Bold="True" ForeColor="Maroon" Height="22px"
                    Text="Contratos:" Width="85px"></asp:Label>
            </td>
            <td align="left" style="width: 299px">
                <telerik:RadComboBox ID="cboContratos" runat="server" Skin="Sunset" Width="200px"
                    OnItemsRequested="cboContratos_ItemsRequested" OnClientSelectedIndexChanging="LoadContratistas"
                    OnClientItemsRequested="ItemsLoaded" />
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 98px; height: 26px">
                <asp:Label ID="lblContratistas" runat="server" Font-Bold="True" ForeColor="Maroon"
                    Height="20px" Text="Contratistas:" Width="85px"></asp:Label>
            </td>
            <td align="left" style="width: 95px; height: 26px">
                <telerik:RadComboBox ID="cboContratistas" runat="server" Skin="Sunset" Width="200px"
                    OnItemsRequested="cboContratistas_ItemsRequested" OnClientSelectedIndexChanging="LoadPeriodos"
                    OnClientItemsRequested="ItemsLoaded" />
            </td>
            <td align="right" style="width: 34px; height: 26px">
                <asp:Label ID="lblPer" runat="server" Font-Bold="True" ForeColor="Maroon" Height="19px"
                    Text="Periodos:" Width="85px"></asp:Label>
            </td>
            <td align="left" style="width: 299px; height: 26px">
                <telerik:RadComboBox ID="cboPeriodos" runat="server" Skin="Sunset" Width="200px"
                    OnItemsRequested="cboPriodos_ItemsRequested" OnClientItemsRequested="ItemsLoaded"
                    OnClientSelectedIndexChanged="CargarGrilla" Mensaje="Buscando Item Hoja de Ruta..." />
            </td>
        </tr>
        <tr>
            <td align="left" style="height: 26px" colspan="4">
                <asp:UpdatePanel ID="upFueraTermino" runat="server" UpdateMode="Conditional">
                    <contenttemplate>
                        <asp:CheckBox ID="chkFueraTermino" runat="server" Text="Si la documentación es presentada fuera de termino marque aqui." />
                    </contenttemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <table style="border-right: #843431 thin solid; border-top: #843431 thin solid; border-left: #843431 thin solid;
        border-bottom: #843431 thin solid; background-color: #E0D6BE; font-family: Sans-Serif;
        font-size: 11px; height: 100%" width="90%">
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="upGrilla" runat="server" UpdateMode="Conditional">
                    <contenttemplate>
                        
                        <telerik:RadGrid ID="gvItemHoja" runat="server" GridLines="None" Skin="Sunset" Height="100%"
                            Width="95%" AllowAutomaticDeletes="false" AllowAutomaticInserts="false" AllowAutomaticUpdates="false"
                            AutoGenerateColumns="False" 
                            OnItemCommand="gvItemHoja_ItemCommand" 
                            OnItemDataBound="gvItemHoja_ItemDataBound"
                            OnDataBound="gvItemHoja_DataBound" >
                            <MasterTableView DataKeyNames="IdHojaDeRuta" ShowHeadersWhenNoRecords="true" ShowHeader="true"
                                EditMode="PopUp" NoMasterRecordsText="Por favor seleccione los valores de filtrado para obtener una hojas de ruta"
                                NoDetailRecordsText="Por favor seleccione los valores de filtrado para obtener una hojas de ruta"
                                Width="100%" Height="100%" ShowFooter="true" TableLayout="Fixed">
                                <RowIndicatorColumn Visible="False">
                                    <HeaderStyle Width="20px"></HeaderStyle>
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn Visible="False" Resizable="False">
                                    <HeaderStyle Width="20px"></HeaderStyle>
                                </ExpandCollapseColumn>
                                <Columns>
                                    <telerik:GridTemplateColumn DataField="" HeaderText="" UniqueName="imgEdit">
                                        <ItemTemplate>
                                            <asp:LinkButton Mensaje="Editando Item..." ID="btnEditSelected" runat="server" CommandName="Edit">
                                                <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/Edit.gif" />
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle Width="25px" HorizontalAlign="Center" />
                                        <ItemStyle CssClass="HandCursor" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="IdHojaDeRuta" HeaderText="Id" SortExpression="Codigo"
                                        UniqueName="Id" Display="false" ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Plantilla.Descripcion" HeaderText="Titulo" UniqueName="column"
                                        ReadOnly="true">
                                        <ItemStyle Wrap="true" HorizontalAlign="Left" />
                                        <HeaderStyle Width="350px" HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn HeaderText="Fecha Recepción" SortExpression="FechaRecepcion"
                                        UniqueName="FechaRecepcion">
                                        <ItemTemplate>
                                            <asp:Label ID="LabelFechaInicio" runat="server" Text='<%# Eval("DocFechaEntrega", "{0:d}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>

                                        <telerik:RadDatePicker ID="TextBoxFechaEntrega"  MinDate="01/01/1900"
                            MaxDate="12/31/2100" runat="server" SelectedDate='<%# Bind("DocFechaEntrega") %>'>
                            
                        </telerik:RadDatePicker>


                                            
                                        </EditItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="DocComentario" HeaderText="Comentario" UniqueName="imgComentario">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBoxDocComentario" runat="server" Text='<%# Bind("DocComentario") %>'
                                                Width="300px" Height="150px" TextMode="MultiLine"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Image ImageUrl="~/images/notepad_16x16.gif" ID="imgcomentario" runat="server"
                                                onclick="showToolTip(this);" coment='<% #Eval("DocComentario") %>' Style="cursor: hand;" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="80px" HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Presento" UniqueName="chkPresentoColumn">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkPresento" runat="server" onclick="Marcar(this);" originalValue='<% #  Eval("DocFechaEntrega") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Button ID="btnAplicar" runat="server" SkinID="btnConosudAplicar" Text="" OnClick="btnAplicar_Click"
                                                Mensaje="Aplicando Cambios.." CausesValidation="false" />
                                        </FooterTemplate>
                                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <EditFormSettings ColumnNumber="1" CaptionDataField="Plantilla.Descripcion" CaptionFormatString="Edición del Item: {0}">
                                    <FormCaptionStyle HorizontalAlign="Left" Width="100%"></FormCaptionStyle>
                                    <FormMainTableStyle GridLines="Both" CellSpacing="0" CellPadding="0" BackColor="White"
                                        Width="100%" />
                                    <FormTableStyle CellSpacing="0" CellPadding="2" BackColor="White" HorizontalAlign="Left" />
                                    <FormTableItemStyle Wrap="False" HorizontalAlign="Left"></FormTableItemStyle>
                                    <FormTableAlternatingItemStyle Wrap="False" HorizontalAlign="Left"></FormTableAlternatingItemStyle>
                                    <EditColumn ButtonType="ImageButton" InsertText="Insertar" UpdateText="Actualizar"
                                        UniqueName="EditCommandColumn1" CancelText="Cancelar">
                                    </EditColumn>
                                    <FormTableButtonRowStyle HorizontalAlign="Right"></FormTableButtonRowStyle>
                                    <PopUpSettings ScrollBars="Auto" Modal="true" Width="50%" />
                                </EditFormSettings>
                            </MasterTableView>
                            <ValidationSettings CommandsToValidate="PerformInsert,Update" />
                            <ClientSettings>
                                <Scrolling AllowScroll="false" UseStaticHeaders="false" SaveScrollPosition="false" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </contenttemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <telerik:RadToolTip runat="server" ID="ToolTipComentario" Skin="Hay" ShowCallout="true"
        ShowEvent="FromCode" RelativeTo="Element" Position="MiddleLeft" Width="220" Title="Comentario Ingresado"
        Animation="Fade">
    </telerik:RadToolTip>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="50">
        <progresstemplate>
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
        </progresstemplate>
    </asp:UpdateProgress>
</asp:Content>
