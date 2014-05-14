<%@ Page Title="" Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="ConsultaDatosSueldos.aspx.cs" Inherits="ConsultaDatosSueldos" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        function requestStart1(sender, args) {
            if (args.get_eventTarget().indexOf("ExportExcel") > 0) {
                args.set_enableAjax(false);
            }
        }

    </script>

    <telerik:RadWindowManager ID="RadWindowManager1" ShowContentDuringLoad="false" VisibleStatusbar="false"
        ReloadOnShow="false" runat="server" Skin="Sunset">
    </telerik:RadWindowManager>
    <table cellpadding="0" cellspacing="5" style="width: 80%">
        <tr>
            <td align="center" style="height: 25px; background: url('images/sprite.gif') 0  -997px repeat-x">
                <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Size="14pt" Font-Names="Sans-Serif"
                    ForeColor="#E0D6BE" Text="Consulta Datos de Sueldos" Width="378px"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <table style="border: thin solid #843431; background-color: #E0D6BE; font-family: Sans-Serif;
                font-size: 11px; width: 80%;">
                <tr>
                    <td valign="middle" align="right" width="40%">
                        <asp:Label ID="lblLegajo" runat="server" Font-Bold="True" ForeColor="Maroon" Height="22px"
                            Text="Período:" Width="79px"></asp:Label>
                    </td>
                    <td valign="middle" align="center" width="30%">
                        <telerik:RadDatePicker ID="txtPeriodo" runat="server">
                            <DateInput DisplayDateFormat="MMMM yyyy">
                            </DateInput>
                        </telerik:RadDatePicker>
                    </td>
                    <td>
                        <asp:Button ID="btnBuscar" runat="server" CommandName="Buscar" SkinID="btnConosudBasic"
                            Text="Buscar" OnClick="btnBuscar_Click" Mensaje="Buscando Datos..." />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="Table2" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
        border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #E0D6BE;
        font-family: Sans-Serif; font-size: 11px;" width="90%">
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="upGrilla" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <telerik:RadGrid ID="gvDatosSueldos" runat="server" AllowPaging="True" AllowSorting="True"
                            ShowStatusBar="True" GridLines="None" Skin="Sunset" AutoGenerateColumns="False"
                            OnItemCommand="gvDatosSueldos_ItemCommand" OnNeedDataSource="gvDatosSueldos_NeedDataSource">
                            <MasterTableView CommandItemDisplay="Top" NoMasterRecordsText="No existen registros para el período seleccionado."
                                HorizontalAlign="NotSet">
                                <CommandItemTemplate>
                                    <div style="padding: 5px 5px;">
                                        <asp:LinkButton CausesValidation="false" Mensaje="Exportando Datos Sueldos...." ID="ExportExcel"
                                            runat="server" CommandName="ExportEmpresas">
                                            <img style="padding-right: 5px;border:0px;vertical-align:middle;" alt="" src="Images/Excel_16x16.gif" />Exportar Datos</asp:LinkButton>&nbsp;&nbsp;
                                    </div>
                                </CommandItemTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn DataField="RazonSocial" HeaderText="RazonSocial" SortExpression="RazonSocial"
                                        UniqueName="RazonSocialColumn">
                                        <ItemTemplate>
                                            <asp:Label ID="RazonSocialLabel" runat="server" Text='<%# Eval("RazonSocial") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn DataField="CUIT" HeaderText="CUIT" SortExpression="CUIT"
                                        UniqueName="CUITColumn">
                                        <ItemTemplate>
                                            <asp:Label ID="CUITLabel" runat="server" Text='<%# string.Format("{0:##-########-#}", long.Parse(Eval("CUIT").ToString().Replace("-",""))) %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="Contrato" HeaderText="Contrato" UniqueName="ContratoColumn"
                                        Display="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="MesLiquidacion" HeaderText="Mes Liquidacion"
                                        UniqueName="ContratoColumn" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="NombreLegajo" HeaderText="Apellido y Nombre"
                                        UniqueName="ContratoColumn" Display="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="FechaNacimiento" HeaderText="Fecha Nacimiento"
                                        UniqueName="ContratoColumn" Visible="false" DataFormatString="{0:MMM-yy}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="FECHADEINGRESO" HeaderText="FECHA DE INGRESO"
                                        UniqueName="ContratoColumn" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="CATEGORIACCT" HeaderText="CATEGORIA SEGÚN CCT"
                                        UniqueName="ContratoColumn" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ENCUADREGREMIAL" HeaderText="ENCUADRE GREMIAL"
                                        UniqueName="ContratoColumn" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="FUNCION" HeaderText="FUNCION " UniqueName="ContratoColumn"
                                        Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="BASICO" HeaderText="BASICO/VALOR HORA SEGÚN CCT"
                                        UniqueName="ContratoColumn" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="BASICOLIQUIDADO" HeaderText="BASICO LIQUIDADO"
                                        UniqueName="ContratoColumn" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="HORASEXTRAS" HeaderText="HORAS EXTRAS" UniqueName="ContratoColumn"
                                        Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ADICIONALES" HeaderText="ADICIONALES REMUNERATIVOS (SIN HORAS EXTRAS)"
                                        UniqueName="ContratoColumn" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="VACACIONES" HeaderText="VACACIONES" UniqueName="ContratoColumn"
                                        Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="SAC" HeaderText="SAC" UniqueName="ContratoColumn"
                                        Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="TOTALBRUTO" HeaderText="TOTAL BRUTO" UniqueName="ContratoColumn"
                                        Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ASIGNFLIARES" HeaderText="ASIGN FLIARES" UniqueName="ContratoColumn"
                                        Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ADICIONALESNOREMUNERATIVOS" HeaderText="ADICIONALES NO REMUNERATIVOS"
                                        UniqueName="ContratoColumn" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="DESCUENTOS" HeaderText="DESCUENTOS" UniqueName="ContratoColumn"
                                        Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="TOTALNETO" HeaderText="TOTAL NETO" UniqueName="ContratoColumn"
                                        Visible="false">
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                            <ValidationSettings CommandsToValidate="PerformInsert,Update" />
                            <ClientSettings>
                                <Selecting AllowRowSelect="True" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" ClientEvents-OnRequestStart="requestStart1">
        <ClientEvents OnRequestStart="requestStart1"></ClientEvents>
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="gvDatosSueldos">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvDatosSueldos" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
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
</asp:Content>
