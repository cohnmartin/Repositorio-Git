<%@ Page Language="C#" EnableEventValidation ="false" MasterPageFile="~/DefaultMasterPage.master" Theme="MiTema" AutoEventWireup="true" CodeFile="AsignacionLegajosContratos.aspx.cs" Inherits="AsignacionLegajosContratos" Title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" namespace="System.Web.UI.WebControls" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script src="Legajos.js" type="text/javascript"></script>
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

    function ControlSeleccion()
    {

        var SelectedRow = $find("<%= gvLegajosAsociados.ClientID %>").get_masterTableView().get_selectedItems().length;
        if (SelectedRow>0)
        {
            if (confirm('Desea eliminar este legajo?')) {
                var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");
                ajaxManager.ajaxRequest("DeleteSelected");
            }
        }
        else
        {
                 var toolManegar = $find("<%=RadToolTipManager1.ClientID%>");
                 toolManegar.set_text("Debe seleccionar un legajo para poder realizar la eliminación");
                 toolManegar.set_targetControlID("<%= gvLegajosAsociados.ClientID %>");
                 toolManegar.show();
                 return false;
        }

         
    }

    function ControlSeleccionEncuadre() {
        var SelectedRow = $find("<%= gvLegajosAsociados.ClientID %>").get_masterTableView().get_selectedItems();
        if (SelectedRow.length > 0) {

            var url = 'CambioDeEncuadre.aspx?IdLegajo=' + SelectedRow[0].getDataKeyValue("Legajos.IdLegajos");
            var name = "RadWindow2";
            var manager = $find("<%=RadWindowManager1.ClientID %>");
            var oWnd = manager.open(url, name);

            //var oWnd = radopen('CambioDeEncuadre.aspx?IdLegajo=' + SelectedRow[0].getDataKeyValue("Legajos.IdLegajos"), 'RadWindow2');
        }
        else {
            var toolManegar = $find("<%=RadToolTipManager1.ClientID%>");
            toolManegar.set_text("Debe seleccionar un legajo para poder realizar el cambio de encuadre");
            toolManegar.set_targetControlID("<%= gvLegajosAsociados.ClientID %>");
            toolManegar.show();
            return false;
        }
    }
    
    
    function showToolTip() {
        var periodosCombo = $find("ctl00_ContentPlaceHolder1_cboPeriodos");

        if(periodosCombo.get_selectedIndex() >= 0)
        {
            var tooltip = $find("<%= ToolTipBusquedaUsuarios.ClientID %>");
            tooltip.show();
        }
        else
        {
             var toolManegar = $find("<%=RadToolTipManager1.ClientID%>");
             toolManegar.set_text("Debe seleccionar un perido para asiganar los legajos.");
             toolManegar.set_targetControlID("ctl00_ContentPlaceHolder1_cboPeriodos");
             toolManegar.show();
        
        }
        
    }

    function Setfocus() {
        document.getElementById("<%= txtNroDocToolTip.ClientID %>").focus();
    }

    function ClickLegajo(id, UltimoPeriodo) {

        var periodosCombo = $find("ctl00_ContentPlaceHolder1_cboPeriodos");
        var periodoSel = periodosCombo.get_text();

        if (UltimoPeriodo == "" || periodoSel > UltimoPeriodo) {

            $find("<%=RadAjaxManager1.ClientID%>").ajaxRequest(id);
            var tooltip = $find("<%= ToolTipBusquedaUsuarios.ClientID %>");
            tooltip.hide();
        }
        else {
            window.DivError.style.display = "block";
        }
    }
    
    function Limpiar() {
        document.getElementById("<%= txtNroDocToolTip.ClientID %>").value = "";
    }

    function ShowCopiado() {

        var empresa = $find("ctl00_ContentPlaceHolder1_cboEmpresas").get_value();
        var contrato = $find("ctl00_ContentPlaceHolder1_cboContratos").get_value();
        var contratista = $find("ctl00_ContentPlaceHolder1_cboContratistas").get_value();
        var periodo = $find("ctl00_ContentPlaceHolder1_cboPeriodos").get_value();

        var url = 'GestionCopiadoLegajos.aspx?Contrato=' + contrato + '&Contratista=' + contratista + '&Periodo=' + periodo + '&Empresa=' + empresa;
        var name = "RadWindow1";
        var manager = $find("<%=RadWindowManager1.ClientID %>");
        var oWnd = manager.open(url, name);

        //var oWnd = radopen('GestionCopiadoLegajos.aspx?Contrato=' + contrato + '&Contratista=' + contratista + '&Periodo=' + periodo + '&Empresa=' + empresa, 'RadWindow1');
    }
   
   function OnClientClose(oWnd, args) {

       var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");
       ajaxManager.ajaxRequest("ActualizarGrilla");

   }
   
   function OnClientSelectedIndexChangingPeriodos(sender, eventArgs) {

      
   }
    
</script>
    <style type="text/css">
    .MouseOverStyle
        {
	        color: #363636;
	        border: 1px solid #99defd;
	        background: #f6fbfd url(Imagenes/ItemHoveredBg.gif) repeat-x left bottom;
            cursor: hand;
        }
        .MouseOutStyle
        {
            background-color: Transparent;
            color: Black;
            cursor: hand;
        }
        
        li
        {
            font-family: "Segoe UI",tahoma,verdana,sans-serif;
            font-size: 11px;
            list-style: circle;
            
        }
</style>


    <table cellpadding="0" cellspacing="5" style="width: 80%">
        <tr>
            <td align="center" style="height: 25px; background: url('images/sprite.gif') 0  -997px repeat-x">
                <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Size="14pt" Font-Names="Sans-Serif"
                    ForeColor="#E0D6BE" Text="Asignación de Legajos" Width="378px"></asp:Label>
            </td>
        </tr>
    </table>
    
     <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Web20" VisibleTitlebar="true"
        style="z-index:100000000"  Title="Sub Contratistas">
        <Windows>
            <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close" Width="750" Height="500"
                Modal="true" NavigateUrl="GestionCopiadoLegajos.aspx" VisibleTitlebar="true" style="z-index:100000000"
                Title="Copiado de Legajos" ReloadOnShow="true" OnClientClose="OnClientClose"
                VisibleStatusbar="false" ShowContentDuringLoad="false" Skin="Sunset">
            </telerik:RadWindow>
                        <telerik:RadWindow ID="RadWindow2" runat="server" Behaviors="Close" Width="450" Height="200"
                Modal="true" NavigateUrl="CambioDeEncuadre.aspx" VisibleTitlebar="true" style="z-index:100000000"
                Title="Cambio de Encuadre" ReloadOnShow="true" OnClientClose="OnClientClose"
                VisibleStatusbar="false" ShowContentDuringLoad="false" Skin="Sunset">
            </telerik:RadWindow>

        </Windows>
        
    </telerik:RadWindowManager>
    
    <telerik:RadAjaxManager Mensaje="Asignando Legajo..." ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <ClientEvents OnResponseEnd="ActualizarXML" />
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="gvLegajosAsociados">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvLegajosAsociados" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    
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
                AllowCustomText="true" MarkFirstMatch="true" 
                    OnClientSelectedIndexChanging="LoadContratos"  />
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
                    Height="20px" Text="SubContratistas:" Width="85px"></asp:Label>
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
                    Mensaje="Buscando Legajos Asignados..." OnClientSelectedIndexChanging="OnClientSelectedIndexChangingPeriodos"
                    AllowCustomText="true" MarkFirstMatch="true" AutoPostBack="true" 
                    OnItemsRequested="cboPriodos_ItemsRequested"
                    OnClientItemsRequested="ItemsLoaded" />
                    
                
            </td>
        </tr>
    </table>
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
    <table style="border-right: #843431 thin solid; border-top: #843431 thin solid; border-left: #843431 thin solid;
        border-bottom: #843431 thin solid; background-color: #E0D6BE; font-family: Sans-Serif;
        font-size: 11px;height:100%" width="90%"  >
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="upGrilla" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <telerik:RadGrid ID="gvLegajosAsociados" runat="server" AutoGenerateColumns="false" GridLines="None"
                            Skin="Sunset" Height="100%" Width="95%" AllowAutomaticUpdates="True" 
                            AllowAutomaticDeletes="True" DataSourceID="EntityDataSourceContratoLegajos">
                            
                            
                            <MasterTableView DataKeyNames="IdContEmpLegajos, Legajos.IdLegajos" ClientDataKeyNames="IdContEmpLegajos, Legajos.IdLegajos" ShowHeadersWhenNoRecords="true"
                             ShowHeader ="true"  EditMode="PopUp" AllowAutomaticUpdates="true" DataSourceID="EntityDataSourceContratoLegajos"
                             NoMasterRecordsText="Por favor seleccione los valores de filtrado para obtener los legajos asociados"
                             NoDetailRecordsText="Por favor seleccione los valores de filtrado para obtener los legajos asociados" 
                              Width="100%" Height="100%" ShowFooter= "true" CommandItemDisplay="Top">
                              <CommandItemTemplate>
                                  <div style="padding: 5px 5px;">
                                      <asp:LinkButton ID="btnAsignar" runat="server" AccessKey="i"   OnClientClick="showToolTip(); return false;" onfocus="showToolTip(); return false;"   >
                                          <img style="border:0px;vertical-align:middle;padding-right:5px" alt="" src="Images/AddRecord.gif" />Asignar Legajo
                                      </asp:LinkButton>
                                      &nbsp;&nbsp;&nbsp;&nbsp;
                                      <asp:LinkButton Mensaje="Eliminando Legajo.." ID="btnEliminar" OnClientClick="ControlSeleccion(); return false;" runat="server" >
                                        <img style="border:0px;vertical-align:middle;padding-right:5px" alt="" src="Images/delete_16x16.gif" />Des Asignar Legajo
                                      </asp:LinkButton>
                                      &nbsp;&nbsp;&nbsp;&nbsp;
                                      <asp:LinkButton Mensaje="Buscando Encuadre.." ID="btnCambiarEncuadre" OnClientClick="ControlSeleccionEncuadre(); return false;" runat="server" >
                                        <img style="border:0px;vertical-align:middle;padding-right:5px" alt="" src="Images/anonymous_(edit)_16x16_1.gif" />Cambiar Encuadre
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
                                    <telerik:GridBoundColumn DataField="Legajos.Apellido" HeaderText="Apellido" UniqueName="ApellidoColumn" ReadOnly="true">
                                        <ItemStyle Wrap="false"  HorizontalAlign="Left"   />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Legajos.Nombre" HeaderText="Nombre" UniqueName="NombreColumn" ReadOnly="true">
                                        <ItemStyle Wrap="false"  HorizontalAlign="Left"   />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Legajos.NroDoc" HeaderText="Nro. Documento" UniqueName="NroDocColumn" ReadOnly="true">
                                        <ItemStyle Wrap="false"  HorizontalAlign="Left"   />
                                    </telerik:GridBoundColumn>
                                
                                </Columns>

                                <EditFormSettings ColumnNumber="1"  CaptionDataField="Plantilla.Descripcion" CaptionFormatString="Edición del Item: {0}">
                                    <FormCaptionStyle HorizontalAlign="Left" Width="100%" ></FormCaptionStyle>
                                    <FormMainTableStyle  GridLines="Both" CellSpacing="0" CellPadding="0" BackColor="White" Width="100%" />
                                    <FormTableStyle CellSpacing="0" CellPadding="2" BackColor="White" HorizontalAlign="Left" />
                                    <FormTableItemStyle Wrap="False" HorizontalAlign="Left" ></FormTableItemStyle>
                                    <FormTableAlternatingItemStyle Wrap="False" HorizontalAlign="Left" ></FormTableAlternatingItemStyle>
                                    <EditColumn ButtonType="ImageButton" InsertText="Insertar" UpdateText="Actualizar" UniqueName="EditCommandColumn1" CancelText="Cancelar"></EditColumn>
                                    <FormTableButtonRowStyle HorizontalAlign="Right" ></FormTableButtonRowStyle>
                                    <PopUpSettings ScrollBars="Auto" Modal="true" Width="50%"/>
                                </EditFormSettings>
                            </MasterTableView>
                            <ClientSettings>
                                <Selecting AllowRowSelect="true" />
                                <Scrolling AllowScroll="false" UseStaticHeaders="false" SaveScrollPosition="false" />
                            </ClientSettings>
                        </telerik:RadGrid>
                        <asp:EntityDataSource ID="EntityDataSourceContratoLegajos" runat="server"  ContextTypeName="Entidades.EntidadesConosud" 
                            ConnectionString="name=EntidadesConosud" EnableDelete="true" OrderBy="it.Legajos.Apellido"
                            DefaultContainerName="EntidadesConosud" EntitySetName="ContEmpLegajos" 
                            Include="Legajos,ContratoEmpresas" EntityTypeFilter="" Select="" 
                            Where="it.CabeceraHojasDeRuta.IdCabeceraHojasDeRuta == @Id && it.ContratoEmpresas.IdContratoEmpresas = @IdContratoEmpresa">
                            <WhereParameters>
                                <asp:ControlParameter DbType="Int64" ControlID="cboPeriodos" PropertyName="SelectedValue" Name="Id" />
                                <asp:ControlParameter DbType="Int64" ControlID="cboContratistas" PropertyName="SelectedValue" Name="IdContratoEmpresa" />
                            </WhereParameters>
                        </asp:EntityDataSource>
                        
                        
                        <telerik:RadToolTip runat="server" ID="ToolTipBusquedaUsuarios" Skin="Black"
                        ShowCallout="true" ShowEvent="FromCode" RelativeTo="Element" Position="BottomCenter" Width="700"
                        Title="Busqueda de Legajos" Animation="Fade" OnClientShow="Setfocus" Modal="true" IsClientID="true"
                        TargetControlID="btnAsignar" ManualClose="true" OnClientHide="Limpiar" >
                        <table width="100%" style="height: 150px">
                            <tr>
                                <td valign="top">
                                    Nro Documento: <asp:TextBox ID="txtNroDocToolTip" runat="server" onkeyup="searchIndex(this)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td  valign="top" align="left">
                                    <div id="resultshere">
                                        
                                    </div>
                                    <div id="DivError" style="text-align:center;width:100%;font-family:Tahoma;font-size:11px;font-weight:bold;color:Red;display:none">
                                        El legajo que intenta asignar esta vigente en otro contrato.
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </telerik:RadToolTip>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="cboPeriodos" EventName="SelectedIndexChanged" />
                    </Triggers>
                  </asp:UpdatePanel>
                
            </td>
        </tr>
        <tr>
            <td align="center">
                
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="upAsignar" runat="server"  UpdateMode="Conditional">
                    <ContentTemplate>
                         <asp:Button ID="btnCopiado" runat="server"  Enabled="false"
                          SkinID="btnConosudBasic" Mensaje="Cargando Copiado Legajos..."
                          Text="Copiar Legajos" OnClientClick="ShowCopiado();" />
                    </ContentTemplate>
                    
                </asp:UpdatePanel>
                
            </td>
        </tr>
    </table>
    
  
    
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <telerik:RadToolTip runat="server" ID="RadToolTipManager1" Skin="Hay" ShowCallout="true"
                ShowEvent="FromCode" RelativeTo="Element" Position="BottomCenter" Animation="Fade">
            </telerik:RadToolTip>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    
   
</asp:Content>

