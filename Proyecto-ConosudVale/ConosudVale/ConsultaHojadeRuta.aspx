<%@ Page Title="" Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="ConsultaHojadeRuta.aspx.cs" Inherits="ConsultaHojadeRuta" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="FuncionesComunes.js"> </script>

    <script type="text/javascript">

        function ShowResult() {
            $("#divResult").fadeIn(1500);
        }
        function HideResult() {

            $("#divResult").fadeOut(500);
        }
        function GoToEditarHoja() {

            var grid = $find("<%= gvHojadeRuta.ClientID%>");
            var MasterTable = grid.get_masterTableView();
            var row = MasterTable.get_dataItems()[event.srcElement.parentElement.parentElement.rowIndex - 1];
            var idCabecera = row.getDataKeyValue("IdCabeceraHojasDeRuta")
            
            window.open('GestionHojadeRuta.aspx?IdCabecera=' + idCabecera + '', 'mywindow', 'width=800,height=600,toolbar=yes, location=yes,directories=yes,status=yes,menubar=yes,scrollbars=yes,copyhistory=yes, resizable=yes');
        
        }

    </script>

    <table cellpadding="0" cellspacing="5" style="width: 80%">
        <tr>
            <td align="center" style="height: 25px; background: url('images/sprite.gif') 0  -997px repeat-x">
                <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Size="14pt" Font-Names="Sans-Serif"
                    ForeColor="#E0D6BE" Text="Visor Hoja de Ruta" Width="378px"></asp:Label>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td align="center">
                <table id="Table1" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
                    border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #E0D6BE;
                    font-family: Sans-Serif; font-size: 11px;">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Periodo Inicial:"></asp:Label>
                        </td>
                        <td>
                            <telerik:RadDatePicker ID="txtInicial" MinDate="1950/1/1" runat="server">
                                <DateInput InvalidStyleDuration="100">
                                </DateInput>
                            </telerik:RadDatePicker>
                        </td>
                        <td>
                            <asp:Label ID="lblIdCabecera" runat="server" Text="Periodo Final:"></asp:Label>
                        </td>
                        <td>
                            <telerik:RadDatePicker ID="txtFinal" MinDate="1950/1/1" runat="server">
                                <DateInput InvalidStyleDuration="100">
                                </DateInput>
                            </telerik:RadDatePicker>
                        </td>
                        <td align="left">
                            <asp:Button ID="btnBuscar" Mensaje="Buscando hojas en el período seleccionado..."
                                runat="server" CommandName="Buscar" SkinID="btnConosudBasic" Text="Buscar" OnClick="btnBuscar_Click"
                                OnClientClick="HideResult();" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td height="25px">
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="upGrilla" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div id="divResult" style="display: none">
                            <telerik:RadGrid ID="gvHojadeRuta" runat="server" Skin="Sunset" AutoGenerateColumns="False"
                                Width="770px" GridLines="None" AllowPaging="True" OnNeedDataSource="gvHojadeRuta_NeedDataSource"
                                OnItemDataBound="gvHojadeRuta_ItemDataBound">
                                <MasterTableView Width="770px" ClientDataKeyNames="IdCabeceraHojasDeRuta">
                                    <RowIndicatorColumn Visible="False">
                                        <HeaderStyle Width="20px"></HeaderStyle>
                                    </RowIndicatorColumn>
                                    <ExpandCollapseColumn Visible="False" Resizable="False">
                                        <HeaderStyle Width="20px"></HeaderStyle>
                                    </ExpandCollapseColumn>
                                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" NextPagesToolTip="Próxima Páginas"
                                        NextPageToolTip="Próxima Página" PagerTextFormat="Cambiar Página: {4} &amp;nbsp;Mostrando Página {0} de {1}, Registros {2} a {3} de {5}."
                                        PrevPagesToolTip="Páginas Previas" PrevPageToolTip="Página Previas"></PagerStyle>
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="EsContratista" HeaderText="EsConrtatista" UniqueName="EsContratista" Visible="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="ConstratistaParaSubConstratista" HeaderText="ConstratistaParaSubConstratista" UniqueName="ConstratistaParaSubConstratista" Visible="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Codigo" HeaderText="Codigo" UniqueName="Codigo">
                                            <HeaderStyle />
                                            <ItemStyle HorizontalAlign="Left" Width="70" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn HeaderText="Contratista">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblContratista" Text='<%# Eval("Empresa.RazonSocial")%>'> </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Sub Contratista">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblSubContratista" Text='<%# Eval("Empresa.RazonSocial")%>'> </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="100" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="Estado" HeaderText="Estado" UniqueName="Estado">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" Width="90" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Periodo" DataFormatString="{0:yyyy/MM}" HeaderText="Periodo"
                                            UniqueName="Periodo">
                                            <ItemStyle HorizontalAlign="Left" Width="50" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridCheckBoxColumn DataField="EsFueraTermino.Value" HeaderText="F. Termino" UniqueName="EsFueraTerminoColumn">
                                            <ItemStyle HorizontalAlign="Center" Width="50" />
                                            <HeaderStyle HorizontalAlign="Center" Width="50" />
                                        </telerik:GridCheckBoxColumn>
                                        
                                        <telerik:GridTemplateColumn HeaderText="Auditar" UniqueName="AuditarColumn">
                                            <ItemTemplate>
                                                <asp:ImageButton Style="cursor: hand; text-align: center" ID="imgAuditar" runat="server"
                                                    ImageUrl="~/images/SubContratistas.gif" OnClientClick="GoToEditarHoja();return false;" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" Width="55px" />
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings>
                                    <Selecting AllowRowSelect="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
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
</asp:Content>
