<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/DefaultMasterPage.master"
    Theme="MiTema" AutoEventWireup="true" CodeFile="AsignacionLegajosContratos.aspx.cs"
    Inherits="AsignacionLegajosContratos" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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

        function ControlSeleccion() {

            var SelectedRow = $find("<%= gvLegajosAsociados.ClientID %>").get_masterTableView().get_selectedItems().length;
            if (SelectedRow > 0) {
                if (confirm('Esta seguro que desea desafectar el legajo del contrato?')) {
                    var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");
                    ajaxManager.ajaxRequest("DeleteSelected");
                }
            }
            else {
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
                var oWnd = radopen('CambioDeEncuadre.aspx?IdLegajo=' + SelectedRow[0].getDataKeyValue("Legajos.IdLegajos"), 'RadWindow2');
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

            if (periodosCombo.get_selectedIndex() >= 0) {
                var tooltip = $find("<%= ToolTipBusquedaUsuarios.ClientID %>");
                tooltip.show();
            }
            else {
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

            var oWnd = radopen('GestionCopiadoLegajos.aspx?Contrato=' + contrato + '&Contratista=' + contratista + '&Periodo=' + periodo + '&Empresa=' + empresa, 'RadWindow1');
        }

        function OnClientClose(oWnd, args) {

            var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");
            ajaxManager.ajaxRequest("ActualizarGrilla");

        }

        function OnClientSelectedIndexChangingPeriodos(sender, eventArgs) {

            var ajaxManager = $find("<%= RadAjaxManager1.ClientID %>");
            ajaxManager.ajaxRequest("CargaGrilla");
        }

        function requestStart1(sender, args) {
            if (args.get_eventTarget().indexOf("btnExportar") > 0) {
                args.set_enableAjax(false);
            }
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
            font-family: "Segoe UI" ,tahoma,verdana,sans-serif;
            font-size: 11px;
            list-style: circle;
        }
    </style>
    <table cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td align="center" style="height: 35px; padding-left: 15px; padding-top: 15px">
                <asp:Label ID="lblTipoGestion" runat="server" Font-Bold="True" Font-Size="20pt" Font-Underline="false"
                    Font-Italic="True" ForeColor="black" Text="ASIGNACION DE LEGAJOS" Font-Names="Arno Pro"></asp:Label>
            </td>
        </tr>
    </table>
    <telerik:radwindowmanager id="RadWindowManager1" runat="server" skin="Web20" visibletitlebar="true"
        style="z-index: 100000000" title="Sub Contratistas">
        <windows>
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

        </windows>
    </telerik:radwindowmanager>
    <telerik:radajaxmanager mensaje="Asignando Legajo..." id="RadAjaxManager1" runat="server"
        onajaxrequest="RadAjaxManager1_AjaxRequest" clientevents-onrequeststart="requestStart1">
        <clientevents onresponseend="ActualizarXML" onrequeststart="requestStart1" />
        <ajaxsettings>
            <telerik:AjaxSetting AjaxControlID="gvLegajosAsociados">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvLegajosAsociados" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </ajaxsettings>
    </telerik:radajaxmanager>
     <table style="background-color: transparent; font-family: Arno Pro; font-size: 16px;
                width: 90%; vertical-align: middle;" border="0">
        <tr >
            <td align="right" style="width:120px">
                <asp:Label ID="lblEmpresa" runat="server" Font-Bold="True" ForeColor="#8C8C8C" 
                    Text="Empresa:"></asp:Label>
            </td>
            <td id="Td1" align="left" style="width:280px" >
                <telerik:radcombobox id="cboEmpresas" runat="server" skin="Sunset" width="100%"
                    allowcustomtext="true" markfirstmatch="true" onclientselectedindexchanging="LoadContratos" />
            </td>
            <td align="left" style="width:80px" >
                <asp:Label ID="lblContr" runat="server" Font-Bold="True" ForeColor="#8C8C8C" Text="Contratos:" ></asp:Label>
            </td>
            <td align="left" style="width:280px" >
                <telerik:radcombobox id="cboContratos" runat="server" skin="Sunset" width="100%"
                    onitemsrequested="cboContratos_ItemsRequested" onclientselectedindexchanging="LoadContratistas"
                    onclientitemsrequested="ItemsLoaded" />
            </td>
        </tr>
        <tr>
            <td align="right" style="width:120px">
                <asp:Label ID="lblContratistas" runat="server" Font-Bold="True" ForeColor="#8C8C8C"
                    Height="20px" Text="Sub Contratistas:" ></asp:Label>
            </td>
            <td align="left" style="width:280px" >
                <telerik:radcombobox id="cboContratistas" runat="server" skin="Sunset" width="100%"
                    onitemsrequested="cboContratistas_ItemsRequested" onclientselectedindexchanging="LoadPeriodos"
                    onclientitemsrequested="ItemsLoaded" />
            </td>
            <td align="left" style="width:80px" >
                <asp:Label ID="lblPer" runat="server" Font-Bold="True" ForeColor="#8C8C8C" Text="Periodos:" Width="85px"></asp:Label>
            </td>
            <td align="left" style="width:280px" >
                <telerik:radcombobox id="cboPeriodos" runat="server" skin="Sunset" width="100%"
                    mensaje="Buscando Legajos Asignados..."  onclientselectedindexchanged="OnClientSelectedIndexChangingPeriodos"
                    allowcustomtext="true" markfirstmatch="true" autopostback="false" onitemsrequested="cboPriodos_ItemsRequested"
                    onclientitemsrequested="ItemsLoaded" />
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center">
                <div style="width: 95%; height: 8px; border-top-style: solid; border-top-width: 2px;
                    border-top-color: #808080;">
                    &nbsp;
                </div>
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
    <table style="background-color: transparent; font-family: Arno Pro; font-size: 16px;
                width: 90%; vertical-align: middle;" border="0">
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="upGrilla" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <telerik:radgrid id="gvLegajosAsociados" runat="server" autogeneratecolumns="false"
                            gridlines="None" skin="Sunset" height="100%" width="95%" allowautomaticupdates="True"
                            allowautomaticdeletes="True" OnItemCommand="gvLegajosAsociados_ItemCommand">
                            <mastertableview datakeynames="IdContEmpLegajos, Legajos.IdLegajos" clientdatakeynames="IdContEmpLegajos, Legajos.IdLegajos"
                                showheaderswhennorecords="true" showheader="true" editmode="PopUp" allowautomaticupdates="true"
                                nomasterrecordstext="Por favor seleccione los valores de filtrado para obtener los legajos asociados"
                                nodetailrecordstext="Por favor seleccione los valores de filtrado para obtener los legajos asociados"
                                width="100%" height="100%" showfooter="true" commanditemdisplay="Top">
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
                                      </asp:LinkButton>&nbsp;&nbsp;
                                    <asp:LinkButton CausesValidation="false" Mensaje="Exportando Legajos...." ID="btnExportar"
                                runat="server" CommandName="ExportLegajos">
                                <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/Excel_16x16.gif" />Exportar Excel</asp:LinkButton>

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
                            </mastertableview>
                            <clientsettings>
                                <Selecting AllowRowSelect="true" />
                                <Scrolling AllowScroll="false" UseStaticHeaders="false" SaveScrollPosition="false" />
                            </clientsettings>
                        </telerik:radgrid>
                        <telerik:radtooltip runat="server" id="ToolTipBusquedaUsuarios" skin="Black" showcallout="true"
                            showevent="FromCode" relativeto="Element" position="BottomCenter" width="700"
                            title="Busqueda de Legajos" animation="Fade" onclientshow="Setfocus" modal="true"
                            isclientid="true" targetcontrolid="btnAsignar" manualclose="true" onclienthide="Limpiar">
                            <table width="100%" style="height: 150px">
                                <tr>
                                    <td valign="top">
                                        Nro Documento:
                                        <asp:TextBox ID="txtNroDocToolTip" runat="server" onkeyup="searchIndex(this)"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left">
                                        <div id="resultshere">
                                        </div>
                                        <div id="DivError" style="text-align: center; width: 100%; font-family: Tahoma; font-size: 11px;
                                            font-weight: bold; color: Red; display: none">
                                            El legajo que intenta asignar esta vigente en otro contrato.
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </telerik:radtooltip>
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
                <asp:UpdatePanel ID="upAsignar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnCopiado" runat="server" Visible="false" SkinID="btnConosudBasic"
                            Mensaje="Cargando Copiado Legajos..." Text="Copiar Legajos" OnClientClick="ShowCopiado();" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <telerik:radtooltip runat="server" id="RadToolTipManager1" skin="Hay" showcallout="true"
                showevent="FromCode" relativeto="Element" position="BottomCenter" animation="Fade">
            </telerik:radtooltip>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
