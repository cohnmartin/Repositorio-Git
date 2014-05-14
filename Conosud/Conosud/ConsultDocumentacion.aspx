<%@ Page Theme="MiTema" EnableEventValidation="false" Language="C#" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="ConsultDocumentacion.aspx.cs" Inherits="ConsultDocumentacion"
    Title="Untitled Page" %>

<%@ Register Assembly="ControlsAjaxNotti" Namespace="ControlsAjaxNotti" TagPrefix="ControlsAjax" %>
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






        function CheckPresentoChange(chk, id) {

            var item = $find("<%= gridDoc.ClientID %>").get_ItemDataByKey(id);

            if (!chk.checked) {
                if (item.FechaEntregaOriginal != undefined)
                    item.FechaEntrega = item.FechaEntregaOriginal.format("dd/MM/yyyy");
                else
                    item.FechaEntrega = item.FechaEntregaOriginal;
            }
            else
                item.FechaEntrega = new Date().format("dd/MM/yyyy");

            $find("<%= gridDoc.ClientID %>").updateRow(item);

        }



        function AplicarCambios(sender, id) {

            var items = $find("<%= gridDoc.ClientID %>").get_ItemsData();

            var ExisteCambio = false;
            jQuery.each(items, function () {
                if ($(this).attr("Presento") == true) {
                    ExisteCambio = true;
                    // break each
                    return false;
                }
            });


            if (ExisteCambio) {
                PageMethods.UpdateData(items, $get("<%= chkFueraTermino.ClientID %>").checked, returnFunction, errorFunction);
            }
            else
                alert("Debe realizar algún cambio para guardar los datos");
        }

        function returnFunction(data) {

            if (data["check"] == undefined) {

                $find("<%=ServerControlWindow1.ClientID %>").CloseWindows();
                $find("<%=gridDoc.ClientID %>").set_ClientdataSource(data);
            }
            else {
                $find("<%=gridDoc.ClientID %>").set_ClientdataSource(data["Datos"]);
                $get("<%=chkFueraTermino.ClientID %>").checked = data["check"];
            }

        }

        function errorFunction(error) {

            alert(error);
        }


        function InitEdit(sender, Id) {

            $find("<%=pnlEdicion.ClientID %>").ClearElements();

            $find("<%=gridDoc.ClientID %>").initEdit($find("<%=ServerControlWindow1.ClientID %>"), Id);
            $get("<%=lblTitulo.ClientID %>").innerText = $find("<%=gridDoc.ClientID %>").get_ItemDataByKey(Id).Titulo;


            $find("<%=ServerControlWindow1.ClientID %>").set_CollectionDiv('divPrincipal');
            $find("<%=ServerControlWindow1.ClientID %>").ShowWindows('divPrincipal', "Carga Datos Recepción");

        }

        function AplicarCambiosItem() {

            var id = $find("<%=gridDoc.ClientID %>").get_KeyValueSelected();
            var item = $find("<%=gridDoc.ClientID %>").getValuesEdit($find("<%=ServerControlWindow1.ClientID %>"));

            $find("<%=ServerControlWindow1.ClientID %>").ShowWaiting(false, "Grabando Datos...");
            PageMethods.UpdateDataItem(item, id, returnFunction, errorFunction);


        }

        function ClientSelectedChange(sender, args) {

            var id = $find("<%=cboPeriodos.ClientID %>").get_value();
            $find("<%=gridDoc.ClientID %>").ShowWaiting("Buscando Items...");
            PageMethods.GetData(id, returnFunction, errorFunction);
            

        }
    </script>
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
                    AutoPostBack="false" OnItemsRequested="cboPriodos_ItemsRequested" OnClientItemsRequested="ItemsLoaded"
                    Mensaje="Buscando Item Hoja de Ruta..." 
                    OnClientSelectedIndexChanged="ClientSelectedChange" />
            </td>
        </tr>
        <tr>
            <td align="left" style="height: 26px" colspan="4">
                <asp:UpdatePanel ID="upFueraTermino" runat="server" UpdateMode="Conditional">
                    <contenttemplate>
                <asp:CheckBox ID="chkFueraTermino" runat="server" 
                    Text="Si la documentación es presentada fuera de termino marque aqui." />
                    </contenttemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <table style="border-right: #843431 thin solid; border-top: #843431 thin solid; border-left: #843431 thin solid;
        border-bottom: #843431 thin solid; background-color: #E0D6BE; font-family: Sans-Serif;
        font-size: 11px; height: 100%;" width="90%">
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="upPeriodos" runat="server" UpdateMode="Conditional">
                    <contenttemplate>
                        <ControlsAjax:ClientControlGrid ID="gridDoc" runat="server" AllowMultiSelection="false"
                    TypeSkin="Sunset" PositionAdd="Botton" AllowRowSelection="true" Width="95%" KeyName="IdHoja"
                    AllowPaging="false" PageSize="50" EmptyMessage="Debe Seleccionar la empresa, contrato y periodo para ver los datos">
                    <FunctionsGral>
                        <ControlsAjax:FunctionGral ClickFunction="AplicarCambios" Text="Guardar Cambios"
                            Type="Custom" ImgUrl="images/notepad.gif" />
                    </FunctionsGral>
                    <FunctionsColumns>
                        <ControlsAjax:FunctionColumnRow Type="Edit" Text="Editar Datos" ClickFunction="InitEdit" />
                    </FunctionsColumns>
                    <Columns>
                        <ControlsAjax:Column HeaderName="Titulo" DataFieldName="Titulo" Align="Derecha" Width="55%" />
                        <ControlsAjax:Column HeaderName="Fecha Recepción" DataFieldName="FechaEntrega" Align="Centrado"
                            AllowClientChange="true" Display="true" NameControlManger="txtFecha" />
                        <ControlsAjax:Column HeaderName="Comentario" DataFieldName="Comentario" Align="Centrado"
                            ImgUrl="images/notepad_16x16.gif" Display="true" NameControlManger="txtComentario" />
                        <ControlsAjax:Column HeaderName="Presento" DataFieldName="Presento" Align="Centrado"
                            Enabled="true" onClientClick="CheckPresentoChange" Display="true" DataType="Bool" />
                        <ControlsAjax:Column HeaderName="FechaEntregaOriginal" DataFieldName="FechaEntregaOriginal"
                            Display="false" />
                    </Columns>
                </ControlsAjax:ClientControlGrid>
                    </contenttemplate>
                    <triggers>
                                    <asp:AsyncPostBackTrigger ControlID="cboPeriodos" EventName="SelectedIndexChanged" />
                                </triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <ControlsAjax:ServerControlWindow ID="ServerControlWindow1" runat="server" BackColor="WhiteSmoke"
        WindowColor="Rojo">
        <ContentControls>
            <ControlsAjax:ServerPanel runat="server" ID="pnlEdicion">
                <BodyContent>
                    <div id="divPrincipal" style="height: 190px; width: 700px">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="padding-top: 0px;
                            text-align: left;">
                            <tr>
                                <td colspan="2" style="background-color: #CACACA; padding-left: 10px;">
                                    <asp:Label ID="lblTitulo" runat="server" SkinID="lblConosud" Style="font: 13px/16px 'segoe ui' ,arial,sans-serif"
                                        Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="padding-top: 5px">
                                    <asp:Label ID="Label2" runat="server" SkinID="lblConosud" Text="Fecha Recepción:"></asp:Label>
                                </td>
                                <td align="left" style="padding-left: 5px; padding-right: 5px">
                                    <telerik:RadDatePicker ID="txtFecha" MinDate="1950/1/1" runat="server">
                                        <DateInput InvalidStyleDuration="100">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" valign="top">
                                    <asp:Label ID="Label14" runat="server" SkinID="lblConosud" Text="Comentario:"></asp:Label>
                                </td>
                                <td style="padding-left: 5px; padding-right: 5px">
                                    <asp:TextBox Width="100%" ID="txtComentario" runat="server" TextMode="MultiLine"
                                        Rows="5"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center" style="padding-top: 5px">
                                    <asp:Button ID="btnAplicar" OnClientClick="AplicarCambiosItem();return false;" runat="server"
                                        Text="Grabar" SkinID="btnConosudBasic" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </BodyContent>
            </ControlsAjax:ServerPanel>
        </ContentControls>
    </ControlsAjax:ServerControlWindow>
</asp:Content>
