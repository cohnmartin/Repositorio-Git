<%@ Page Title="" Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="ABMContratos.aspx.cs" Inherits="ABMContratos" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        var currentTextBox = null;
        var currentDatePicker = null;

        //This method is called to handle the onclick and onfocus client side events for the texbox
        function showPopup(sender, e) {
            //this is a reference to the texbox which raised the event
            currentTextBox = sender;

            //this gets a reference to the datepicker, which will be shown, to facilitate
            //the selection of a date
            var datePicker = $find("<%= RadDatePicker1.ClientID %>");

            //this variable is used to store a reference to the date picker, which is currently 
            //active
            currentDatePicker = datePicker;

            //this method first parses the date, that the user entered or selected, and then
            //sets it as a selected date to the picker
            datePicker.set_selectedDate(currentDatePicker.get_dateInput().parseDate(sender.value));

            //the code lines below show the calendar, which is used to select a date. The showPopup
            //function takes three arguments - the x and y coordinates where to show the calendar, as 
            //well as its height, derived from the offsetHeight property of the textbox 
            var position = datePicker.getElementPosition(sender);
            datePicker.showPopup(position.x, position.y + sender.offsetHeight);
        }

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

        function requestStart(sender, args) {
            if (args.get_eventTarget().indexOf("ExportExcel") > 0) {
                args.set_enableAjax(false);
            }
        }

        var options, a;
        jQuery(function () {
            options = { serviceUrl: 'LoadEmpresas.ashx', width: '484', params: { Tipo: 'E'} };
            a = $('#<%=txtEmpresa.ClientID%>' + '_text').autocomplete(options);


        });
    
    </script>
    <telerik:RadWindowManager ID="RadWindowManager1" ShowContentDuringLoad="false" VisibleStatusbar="false"
        ReloadOnShow="false" runat="server" Skin="Default">
    </telerik:RadWindowManager>
    <table cellpadding="0" cellspacing="5" style="width: 80%">
        <tr>
            <td align="center" style="height: 25px; background: url('images/sprite.gif') 0  -997px repeat-x">
                <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Size="14pt" Font-Names="Sans-Serif"
                    ForeColor="#E0D6BE" Text="Gestión de Contratos" Width="378px"></asp:Label>
            </td>
        </tr>
    </table>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" ClientEvents-OnRequestStart="requestStart">
        <ClientEvents OnRequestStart="requestStart"></ClientEvents>
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <table style="border: thin solid #843431; background-color: #E0D6BE; font-family: Sans-Serif;
        font-size: 11px; width: 80%;">
        <tr >
            <td valign="middle" align="right" width="20%">
                <asp:Label ID="Label6" runat="server" Font-Bold="True" ForeColor="Maroon"
                    Text="Empresa:" Width="79px"></asp:Label>
            </td>
            <td valign="middle" align="center" width="30%">
                <telerik:RadTextBox ID="txtEmpresa" runat="server" EmptyMessage="Ingrese Nombre Empresa a buscar"
                    Skin="Sunset" Width="255px">
                </telerik:RadTextBox>
            </td>
            <td valign="middle" align="right" width="20%">
                <asp:Label ID="lblContratos" runat="server" Font-Bold="True" ForeColor="Maroon" 
                    Text="Contrato:" Width="79px"></asp:Label>
            </td>
            <td valign="middle" align="center" width="30%">
                <telerik:RadTextBox ID="txtCodigo" runat="server" EmptyMessage="Ingrese Nro Contrato a buscar"
                    Skin="Sunset" Width="255px">
                </telerik:RadTextBox>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
                    <contenttemplate>
                        <asp:Button ID="btnBuscar" runat="server" CommandName="Buscar" SkinID="btnConosudBasic"
                            Text="Buscar" OnClick="btnBuscar_Click" Mensaje="Buscando Contratos..." />
                            </contenttemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <table id="Table2" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
        border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #E0D6BE;
        font-family: Sans-Serif; font-size: 11px;" width="98%">
        <tr>
            <td>
                <asp:UpdatePanel ID="updpnlGrilla" UpdateMode="Conditional" runat="server">
                    <contenttemplate>
                        <telerik:raddatepicker id="RadDatePicker1" style="display: none;" mindate="01/01/1900"
                            maxdate="12/31/2100" runat="server">
                            <clientevents ondateselected="dateSelected" />
                        </telerik:raddatepicker>
                        <telerik:radgrid id="RadGrid1" runat="server" allowpaging="True" allowsorting="True"
                            showstatusbar="True" gridlines="None" skin="Sunset" allowautomaticdeletes="false"
                            allowautomaticinserts="false" allowautomaticupdates="false" autogeneratecolumns="False"
                            pagesize="10" onselectedindexchanged="RadGrid1_SelectedIndexChanged" onitemcommand="RadGrid1_ItemCommand"
                            onitemdatabound="RadGrid1_ItemDataBound">
                            <pagerstyle nextpagestooltip="Prox. Pag." nextpagetooltip="Prox. Pag." pagertextformat="Cambiar Página: {4} &amp;nbsp;Mostrando Pag. {0} de {1}, del {2} al {3} de {5}."
                                prevpagestooltip="Prev. Pag." prevpagetooltip="Prev. Pag." />
                            <mastertableview datakeynames="IdContrato" commanditemdisplay="Top" editmode="PopUp"
                                nomasterrecordstext="No existen registros." horizontalalign="NotSet">
                                <CommandItemTemplate>
                                    <div style="padding: 5px 5px;">
                                        <asp:LinkButton Mensaje="Editanto Contrato..." ID="btnEdit" runat="server" CommandName="EditSelected" Visible='<%# RadGrid1.EditIndexes.Count == 0 %>'><img style="border:0px;vertical-align:middle;" alt="" src="Images/Edit.gif" />Editar</asp:LinkButton>&nbsp;&nbsp;
                                        <asp:LinkButton Mensaje="Preparando Para Nuevo Contrato..." ID="btnInsert" runat="server" CommandName="InitInsert" Visible='<%# !RadGrid1.MasterTableView.IsItemInserted %>'
                                            OnClick="LinkButton2_Click"><img style="border:0px;vertical-align:middle;" alt="" src="Images/AddRecord.gif" />Insertar</asp:LinkButton>&nbsp;&nbsp;
                                        <asp:LinkButton Mensaje="Eliminando Contrato..." ID="btnDelete" OnClientClick="return blockConfirm('Desea eliminar esta Contrato ?', event, 330, 100,'','Contratos');"
                                            runat="server" OnClick="LinkButtonEliminar_OnClick"  ><img style="border:0px;vertical-align:middle;" alt="" src="Images/delete_16x16.gif" />Eliminar</asp:LinkButton>&nbsp;&nbsp;
                                        <asp:LinkButton Mensaje="Exportando Contratos...." ID="ExportExcel" runat="server" CommandName="ExportContratos">
                                            <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/Excel_16x16.gif" />Excel</asp:LinkButton>&nbsp;&nbsp;                                            
                                    </div>
                                </CommandItemTemplate>
                                <RowIndicatorColumn>
                                    <HeaderStyle Width="20px"></HeaderStyle>
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn>
                                    <HeaderStyle Width="20px"></HeaderStyle>
                                </ExpandCollapseColumn>
                                <Columns>
                                    
                                    <telerik:GridBoundColumn DataField="Codigo" HeaderText="Nro Contrato" SortExpression="Codigo"
                                        UniqueName="Codigo" EditFormColumnIndex="0">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="Servicio" HeaderText="Servicio" SortExpression="Servicio"
                                        UniqueName="Servicio" EditFormColumnIndex="0">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>

                                    <telerik:GridTemplateColumn HeaderText="Fecha Inicio" SortExpression="FechaInicio" EditFormColumnIndex="0" >
                                        <ItemTemplate>
                                            <asp:Label ID="LabelFechaInicio" runat="server" Text='<%# Eval("FechaInicio", "{0:d}") %>'></asp:Label>                                         
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBoxFechaInicio" Text='<%# Bind("FechaInicio", "{0:d}") %>'
                                                onclick="showPopup(this, event);" onfocus="showPopup(this, event);" onblur="parseDate(this, event)"
                                                runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorFechaInicio" ControlToValidate="TextBoxFechaInicio"
                                                Display="Dynamic" ErrorMessage="*" runat="server">
                                            </asp:RequiredFieldValidator>

                                        </EditItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Fecha Vencimiento" SortExpression="FechaVencimiento" EditFormColumnIndex="0">
                                        <ItemTemplate>
                                            <asp:Label ID="LabelFechaVencimiento" runat="server" Text='<%# Eval("FechaVencimiento", "{0:d}") %>'></asp:Label>                                         
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBoxFechaVencimiento" Text='<%# Bind("FechaVencimiento", "{0:d}") %>'
                                                onclick="showPopup(this, event);" onfocus="showPopup(this, event);" onblur="parseDate(this, event)"
                                                runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorFechaVencimiento" ControlToValidate="TextBoxFechaVencimiento"
                                                    Display="Dynamic" ErrorMessage="*" runat="server">
                                                </asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Prorroga" SortExpression="Prorroga" EditFormColumnIndex="0">
                                        <ItemTemplate>
                                            <asp:Label ID="LabelProrroga" runat="server" Text='<%# Eval("Prorroga", "{0:d}") %>'></asp:Label>                                         
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBoxProrroga" Text='<%# Bind("Prorroga", "{0:d}") %>'
                                                onclick="showPopup(this, event);" onfocus="showPopup(this, event);" onblur="parseDate(this, event)"
                                                runat="server"></asp:TextBox>
                                        </EditItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>
                                   
                                    
                                    <telerik:GridTemplateColumn HeaderText="Contratista" 
                                        UniqueName="TemplateColumnContratista">
                                        <ItemTemplate>
                                            <asp:Label ID="LabelContratista" runat="server" Text='<%# Eval("Contratista") %>'></asp:Label>                                         
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox ID="cboContratista" Runat="server" 
                                                DataTextField="RazonSocial" ZIndex="900000000"
                                                DataValueField="IdEmpresa" Skin="Sunset">
                                            </telerik:RadComboBox>
                                        </EditItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left"/>
                                    </telerik:GridTemplateColumn>      
                                    
                                    
                                    <telerik:GridTemplateColumn HeaderText="Tipo Contrato" 
                                        UniqueName="TemplateColumnTipoContrato" Visible="false" >
                                        <ItemTemplate>
                                            <asp:Label ID="LabelContratista" runat="server" Text='<%# Eval("TipoContrato") %>'></asp:Label>                                         
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox ID="cboTipoContrato" Runat="server" 
                                                DataTextField="Descripcion" 
                                                DataValueField="IdClasificacion" Skin="Sunset">
                                            </telerik:RadComboBox>
                                        </EditItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left"/>
                                    </telerik:GridTemplateColumn>      
                                    
                                    

                                    <telerik:GridTemplateColumn HeaderText="Contratado Por" 
                                        UniqueName="TemplateColumnContratadopor" Visible="false" >
                                        <ItemTemplate>
                                            <asp:Label ID="LabelContratista" runat="server" Text='<%# Eval("Contratadopor") %>'></asp:Label>                                         
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox ID="cboContratadoPor" Runat="server" 
                                                DataTextField="Descripcion" 
                                                DataValueField="IdClasificacion" Skin="Sunset">
                                            </telerik:RadComboBox>
                                        </EditItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left"/>
                                    </telerik:GridTemplateColumn>      

                                    <telerik:GridTemplateColumn HeaderText="Clasificación" 
                                        UniqueName="TemplateColumnCategoria"  >
                                        <ItemTemplate>
                                            <asp:Label ID="LabelCategoria" runat="server" Text='<%# Eval("Categoria") %>'></asp:Label>                                         
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox ID="cboCategoria" Runat="server" 
                                                DataTextField="Descripcion" 
                                                DataValueField="IdClasificacion" Skin="Sunset">
                                            </telerik:RadComboBox>
                                        </EditItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left"/>
                                    </telerik:GridTemplateColumn>      
                                    

                                    <telerik:GridTemplateColumn HeaderText="Area" 
                                        UniqueName="TemplateColumnArea" Visible="false" >
                                        <ItemTemplate>
                                            <asp:Label ID="LabelArea" runat="server" Text='<%# Eval("Area") %>'></asp:Label>                                         
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadComboBox ID="cboArea" Runat="server" 
                                                DataTextField="Descripcion" 
                                                DataValueField="IdClasificacion" Skin="Sunset">
                                            </telerik:RadComboBox>
                                        </EditItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left"/>
                                    </telerik:GridTemplateColumn>      


                                 
                                 <telerik:GridBoundColumn DataField="GestorNombre" ReadOnly="true" HeaderText="Gestor Nombre" Visible="false" UniqueName="GestorNombreAlt">
                                 </telerik:GridBoundColumn>

                                 <telerik:GridBoundColumn DataField="GestorEmail"  ReadOnly="true" HeaderText="Gestor Email" Visible="false" UniqueName="GestorEmailAlt">
                                 </telerik:GridBoundColumn>


                                 <telerik:GridBoundColumn DataField="FiscalNombre"  ReadOnly="true" HeaderText="Fiscales Nombre" Visible="false" UniqueName="FiscalNombreAlt">
                                 </telerik:GridBoundColumn>

                                 <telerik:GridBoundColumn DataField="FiscalEmail"  ReadOnly="true" HeaderText="Fiscales Email" Visible="false" UniqueName="FiscalEmailAlt">
                                 </telerik:GridBoundColumn>


                                 <telerik:GridTemplateColumn HeaderText="Gestor" 
                                        UniqueName="TemplateColumnCategoria" Display="false" >
                                        <EditItemTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0" style="height: 62px;">
                                                <tr>
                                                     <td align="right" style="padding-right:5px">
                                                        <asp:Label ID="Label1" runat="server" Text='Nombre:'></asp:Label> 
                                                    </td>
                                                    <td align="center">
                                                        <asp:TextBox ID="txtNombreGestor" Width="300px" Text='<%# Bind("GestorNombre") %>' runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" style="padding-right:5px">
                                                        <asp:Label ID="Label2" runat="server" Text='Email:'></asp:Label> 
                                                    </td>

                                                    <td align="center">
                                                        <asp:TextBox ID="txtEmailGestor" Width="300px" Text='<%# Bind("GestorEmail") %>' runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            
                                        </EditItemTemplate>
         
                                    </telerik:GridTemplateColumn>      

                                    <telerik:GridTemplateColumn HeaderText="Fiscales" 
                                        UniqueName="TemplateColumnCategoria" Display="false" >
                                        <EditItemTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0" style="height: 62px;">
                                                <tr>
                                                     <td align="right" style="padding-right:5px">
                                                        <asp:Label ID="Label3" runat="server" Text='Nombres:'></asp:Label> 
                                                    </td>
                                                    <td align="center">
                                                        <asp:TextBox ID="txtNombreFiscales" Width="300px" Text='<%# Bind("FiscalNombre") %>' runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" style="padding-right:5px">
                                                        <asp:Label ID="Label5" runat="server" Text='Emails:'></asp:Label> 
                                                    </td>

                                                    <td align="center">
                                                        <asp:TextBox ID="txtEmailFiscales" Width="300px" Text='<%# Bind("FiscalEmail") %>' runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            
                                        </EditItemTemplate>
         
                                    </telerik:GridTemplateColumn>     

                                    <telerik:GridTemplateColumn HeaderText="SubContratistas" 
                                        UniqueName="TemplateColumnSubContratistas" Visible="false" >
                                        <ItemTemplate>
                                            <asp:Label ID="LabelSubContratistas" runat="server" Text='<%# Eval("SubContratistas") %>'></asp:Label>       
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left"/>
                                    </telerik:GridTemplateColumn>      
                                                                        
                                </Columns>
                                <EditFormSettings ColumnNumber="2" CaptionDataField="Codigo" CaptionFormatString="Editar el Contrato: {0}">
                                    <FormTableItemStyle Wrap="False"></FormTableItemStyle>
                                    <FormMainTableStyle  GridLines="Both" HorizontalAlign="Right" CellSpacing="0" CellPadding="0" BackColor="White"
                                        Width="100%" />
                                    <FormTableStyle CellSpacing="0" CellPadding="2" Width="100%" Height="110px" BackColor="White"  />
                                    <FormStyle Width="100%" BackColor="#eef2ea"></FormStyle>
                                    <FormTableAlternatingItemStyle Wrap="False"></FormTableAlternatingItemStyle>
                                    <EditColumn ButtonType="ImageButton" InsertText="Insertar" UpdateText="Actualizar"
                                        UniqueName="EditCommandColumn1" CancelText="Cancelar">
                                    </EditColumn>
                                    <FormTableButtonRowStyle HorizontalAlign="Right"></FormTableButtonRowStyle>
                                    <PopUpSettings ScrollBars="Auto" Modal="true" Width="60%" />
                                </EditFormSettings>
                            </mastertableview>
                            <validationsettings commandstovalidate="PerformInsert,Update" />
                            <clientsettings>
                                <Selecting AllowRowSelect="True" />
                            </clientsettings>
                        </telerik:radgrid>
                    </contenttemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
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
