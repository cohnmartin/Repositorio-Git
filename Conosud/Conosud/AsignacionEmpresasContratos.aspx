<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/DefaultMasterPage.master" Theme="MiTema" AutoEventWireup="true" CodeFile="AsignacionEmpresasContratos.aspx.cs" Inherits="AsignacionEmpresasContratos" Title="Untitled Page" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" namespace="System.Web.UI.WebControls" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">

        function ShowAsignarSubContratistas() {
            var Items = $find("ctl00_ContentPlaceHolder1_gvContratos").get_masterTableView().get_selectedItems();
            if (Items.length > 0) {

                var url = 'AsignacionSubContratista.aspx?IdContrato=' + Items[0].getDataKeyValue("Contrato.IdContrato");
                var name = "RadWindow1";
                var manager = $find("<%=RadWindowManager1.ClientID %>");
                var oWnd = manager.open(url, name);

                //var oWnd = radopen('AsignacionSubContratista.aspx?IdContrato=' + Items[0].getDataKeyValue("Contrato.IdContrato"), 'RadWindow1');
            }

        }

        function ShowEliminarSubContratistas() {
            var Items = $find("ctl00_ContentPlaceHolder1_gvContratos").get_masterTableView().get_selectedItems();
            if (Items.length > 0) {
                var url = 'EliminacionSubContratistas.aspx?IdContrato=' + Items[0].getDataKeyValue("Contrato.IdContrato");
                var name = "RadWindow2";
                var manager = $find("<%=RadWindowManager1.ClientID %>");
                var oWnd = manager.open(url, name);

                //var oWnd = radopen('EliminacionSubContratistas.aspx?IdContrato=' + Items[0].getDataKeyValue("Contrato.IdContrato"), 'RadWindow2');
            }

        }

        function OnClientClose(oWnd, args) {
            if (oWnd.argument != null && oWnd.argument.Elimino) {
                var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>")
                ajaxManager.ajaxRequest("Recargar");
            }
        }
        
 
    </script>

    <style type="text/css">
        .MouseOverStyle
        {
            color: white;
            border: 1px solid #99defd;
            background: #f6fbfd url(Images/item-over-red.gif) repeat-x left bottom;
            cursor: hand;
        }
        .MouseOutStyle
        {
            background-color: Transparent;
            color: Black;
            cursor: hand;
        }
    </style>
 
    <table cellpadding="0" cellspacing="5" style="width: 80%">
        <tr>
            <td align="center" style="height: 25px; background: url('images/sprite.gif') 0  -997px repeat-x">
                <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Size="14pt" Font-Names="Sans-Serif"
                    ForeColor="#E0D6BE" Text="Asignación de Sub Contratistas" Width="378px"></asp:Label>
            </td>
        </tr>
    </table>
    <table id="Table1" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
        border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #E0D6BE;
        font-family: Sans-Serif; font-size: 11px;" width="60%">
        <tr>
            <td align="right" style="width: 98px; height: 26px">
                <asp:Label ID="lblEmpresa" runat="server" Font-Bold="True" ForeColor="Maroon" Height="22px"
                    Text="Empresa:" Width="79px"></asp:Label>
            </td>
            <td id="Td1" align="left" style="width: 95px; height: 26px">
                <telerik:RadComboBox ID="cboEmpresas" runat="server" Skin="Sunset" Width="350px"
                    AllowCustomText="true" MarkFirstMatch="true" AutoPostBack="true" Mensaje="Buscando Contratos..." 
                    />
            </td>
        </tr>
    </table>
     <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest" >
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="gvContratos">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvContratos" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    
    <table style="border-right: #843431 thin solid; border-top: #843431 thin solid; border-left: #843431 thin solid;
        border-bottom: #843431 thin solid; background-color: #E0D6BE; font-family: Sans-Serif;
        font-size: 11px; height: 100%" width="90%">
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="upGrilla" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <telerik:RadGrid ID="gvContratos" runat="server" AutoGenerateColumns="false" GridLines="None"
                            Skin="Sunset" Height="100%" Width="95%" AllowAutomaticUpdates="True" AllowAutomaticDeletes="True"
                            DataSourceID="EntityDataSourceContratoEmpresas" OnDataBound="gvContratos_DataBound">
                            <MasterTableView DataKeyNames="IdContratoEmpresas,Contrato.IdContrato" ClientDataKeyNames="IdContratoEmpresas,Contrato.IdContrato" ShowHeadersWhenNoRecords="true"
                                ShowHeader="true" EditMode="PopUp" AllowAutomaticUpdates="true" DataSourceID="EntityDataSourceContratoEmpresas"
                                NoMasterRecordsText="Por favor seleccione los valores de filtrado para obtener los legajos asociados"
                                NoDetailRecordsText="La empresa seleccionado no posee sub-contratistas asignados"
                                Width="100%" Height="100%" ShowFooter="true" CommandItemDisplay="Top">
                                <CommandItemTemplate>
                                    <div style="padding: 5px 5px;">
                                        <asp:LinkButton Mensaje="Preparando para asiganar Sub Contratistas.." ID="btnAsignar" OnClientClick="ShowAsignarSubContratistas();" runat="server" AccessKey="i" Visible='<%# !gvContratos.MasterTableView.IsItemInserted %>'>
                                          <img style="border:0px;vertical-align:middle;padding-right:5px" alt="" src="Images/AddRecord.gif" />Asignar Sub Contratista
                                        </asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton Mensaje="Cargando Sub Contratista..." ID="btnEliminar" runat="server" OnClientClick="ShowEliminarSubContratistas();">
                                        <img style="border:0px;vertical-align:middle;padding-right:5px" alt="" src="Images/delete_16x16.gif" />Eliminar Sub Contratista
                                        </asp:LinkButton>
                                    </div>
                                </CommandItemTemplate>
                                <RowIndicatorColumn Visible="False">
                                    <HeaderStyle Width="20px"></HeaderStyle>
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn Visible="False" Resizable="False">
                                    <HeaderStyle Width="20px"></HeaderStyle>
                                </ExpandCollapseColumn>
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Sub Contratistas" UniqueName="SubContratistasColumn">
                                        <ItemTemplate>
                                            <asp:ImageButton Style="cursor: hand; text-align: center" ID="imgSubContratistas" Mensaje="Buscando Sub Contratistas Asignados.."
                                                runat="server" ImageUrl="~/images/SubContratistas.gif" OnClientClick="ShowSubContratistas(); return false;" />
                                        </ItemTemplate>
                                        <ItemStyle Width="25px" HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="Contrato.Codigo" HeaderText="Nro contrato" UniqueName="ConrtatoColumn"
                                        ReadOnly="true">
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Contrato.FechaInicio" HeaderText="Fecha Inicio"
                                        UniqueName="NombreColumn" ReadOnly="true">
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Contrato.FechaVencimiento" HeaderText="Fecha Vencimiento"
                                        UniqueName="NroDocColumn" ReadOnly="true">
                                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                </Columns>
                                <EditFormSettings ColumnNumber="1" CaptionDataField="Contrato.Codigo" CaptionFormatString="Edición del Item: {0}">
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
                            <ClientSettings>
                                <Selecting AllowRowSelect="true" />
                                <Scrolling AllowScroll="false" UseStaticHeaders="false" SaveScrollPosition="false" />
                            </ClientSettings>
                        </telerik:RadGrid>
                        <asp:EntityDataSource ID="EntityDataSourceContratoEmpresas" runat="server" ConnectionString="name=EntidadesConosud" ContextTypeName="Entidades.EntidadesConosud" 
                            EnableDelete="true" DefaultContainerName="EntidadesConosud" EntitySetName="ContratoEmpresas"
                            Include="Empresa,Contrato" EntityTypeFilter="" Select="" Where="it.Empresa.IdEmpresa == @Id && it.EsContratista = True">
                            <WhereParameters>
                                <asp:ControlParameter DbType="Int64" ControlID="cboEmpresas" PropertyName="SelectedValue"
                                    Name="Id" />
                            </WhereParameters>
                        </asp:EntityDataSource>
                        <telerik:RadToolTip ID="ToolTipSelFragancias" runat="server" Skin="Sunset" ManualClose="true"
                            RelativeTo="BrowserWindow" Position="Center" Title="Sub Contratistas Asignados"
                            ShowEvent="FromCode" Sticky="false" Animation="Fade" Width="710px">
                            <table width="100%" style="height: 100%">
                                <tr>
                                    <td id="tdSeleccionFragancias" valign="middle" align="left">
                                    </td>
                                </tr>
                            </table>
                        </telerik:RadToolTip>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="cboEmpresas"  EventName="SelectedIndexChanged"/>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
   

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Web20" VisibleTitlebar="true"
        style="z-index:100000000"  Title="Sub Contratistas">
        <Windows>
            <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close" Width="650" Height="480"
                Modal="true" NavigateUrl="AsignacionSubContratista.aspx" VisibleTitlebar="true" style="z-index:100000000"
                Title="Ingrese la definición para la Agenda" ReloadOnShow="true" 
                VisibleStatusbar="false" ShowContentDuringLoad="false" Skin="Sunset">
            </telerik:RadWindow>
             <telerik:RadWindow ID="RadWindow2" runat="server" Behaviors="Close" Width="650" Height="480"
                Modal="true" NavigateUrl="EliminacionSubContratistas.aspx" VisibleTitlebar="true" style="z-index:100000000"
                Title="Ingrese la definición para la Agenda" ReloadOnShow="true" OnClientClose="OnClientClose"
                VisibleStatusbar="false" ShowContentDuringLoad="false" Skin="Sunset">
            </telerik:RadWindow>
            
            
            
        </Windows>
    </telerik:RadWindowManager>
    
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="50">
        <ProgressTemplate>
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
        </ProgressTemplate>
    </asp:UpdateProgress>
    <script type="text/javascript">
    
    function ShowSubContratistas() {
        
        var cuits = event.srcElement.getAttribute("CUIT-SubContratista").split('|');
        var rzs = event.srcElement.getAttribute("Rz-SubContratista").split('|');
        var resultshere = document.getElementById("tdSeleccionFragancias");
        var tableContainer = document.createElement("table");
        var tableContainerTbody = document.createElement("tbody");
        resultshere.innerHTML = "";


        tableContainer.setAttribute("width", "100%");
        tableContainer.setAttribute("height", "100%");
        tableContainer.setAttribute("cellpadding", "0");
        tableContainer.setAttribute("cellspacing", "0");
        tableContainer.setAttribute("id", "tblSubCont");
        tableContainerTbody.setAttribute("id", "tblSubCont");
        tableContainer.bgColor = "transparent";

        for (var j = 0; j < cuits.length - 1; j++) {

            var trProductos = document.createElement("tr");
            trProductos.onmouseover = setStyle;
            trProductos.onmouseout = resetStyle;
            
            var tdProductosCuit = document.createElement("td");
            var tdProductosRz = document.createElement("td");
            tdProductosCuit.setAttribute("align", "center");
            tdProductosRz.innerHTML = rzs[j];

            trProductos.appendChild(tdProductosRz);
            tdProductosCuit.innerHTML = cuits[j];
            trProductos.appendChild(tdProductosCuit);


            tableContainerTbody.appendChild(trProductos);
        }


        tableContainer.appendChild(tableContainerTbody);
        resultshere.appendChild(tableContainer);

        var tooltip = $find("ctl00_ContentPlaceHolder1_ToolTipSelFragancias");
        tooltip.show();

    }
    function setStyle() {
        this.className = 'MouseOverStyle';
    }
    function resetStyle() {
        this.className = 'MouseOutStyle';
    }


</script>
</asp:Content>

