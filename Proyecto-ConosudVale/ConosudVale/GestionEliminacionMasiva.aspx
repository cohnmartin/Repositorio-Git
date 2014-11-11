<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true"
    CodeFile="GestionEliminacionMasiva.aspx.cs" Inherits="GestionEliminacionMasiva" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">

        function ValidarArchivo() {
            var archivo = $find("<%=RadUpload1.ClientID %>").getFileInputs()[0].value;

            if (archivo == "") {
                radalert("Debe seleccionar un archivo para poder completar la acción");
                return false;
            }
        }

    </script>

    <telerik:RadWindowManager ID="RadWindowManager1" ShowContentDuringLoad="false" VisibleStatusbar="false"
        ReloadOnShow="false" runat="server" Skin="Sunset">
    </telerik:RadWindowManager>
    <table cellpadding="0" cellspacing="5" style="width: 80%">
        <tr>
            <td align="center" style="height: 25px; background: url('images/sprite.gif') 0  -997px repeat-x">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="14pt" Font-Names="Sans-Serif"
                    ForeColor="#E0D6BE" Text="Eliminación Masiva Legajos" Width="468px"></asp:Label>
            </td>
        </tr>
    </table>
    <table style="width: 100%">
        <tr>
            <td colspan="4" align="center">
                <table style="width: 90%" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
                    border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #E0D6BE;
                    font-family: Sans-Serif; font-size: 11px;">
                    <tr>
                        <td valign="top" align="left">
                            <asp:Label ID="Lassbel5" SkinID="lblConosud" runat="server" Text="Archivo:"></asp:Label>
                        </td>
                        <td valign="top" align="left">
                            <asp:UpdatePanel runat="server" ID="UpArchivo" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <telerik:RadUpload ID="RadUpload1" runat="server" ControlObjectsVisibility="None"
                                        OverwriteExistingFiles="true" Skin="Sunset" InputSize="85" TargetFolder="~/ArchivosEliminacionLegajos"
                                        Width="500px" Height="25px">
                                    </telerik:RadUpload>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="buttonSubmit" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td valign="top" align="left">
                            <asp:Button ID="buttonSubmit" SkinID="btnConosudBasic" OnClientClick="return ValidarArchivo();"
                                runat="server" OnClick="buttonSubmit_Click" Text="Procesar Archivo" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center">
                <telerik:RadProgressManager ID="RadProgressManager1" runat="server" />
                <telerik:RadProgressArea ID="RadProgressArea1" runat="server" Skin="Sunset" ProgressIndicators="RequestSize, FilesCountBar, FilesCount, FilesCountPercent, SelectedFilesCount, CurrentFileName, TimeElapsed, TimeEstimated, TransferSpeed">
                    <Localization Uploaded="Uploaded" CurrentFileName="Subiendo Archivo: " />
                </telerik:RadProgressArea>
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center">
                <asp:UpdatePanel runat="server" ID="upGrilla" UpdateMode="Conditional">
                    <ContentTemplate>
                        <telerik:RadGrid ID="gvArchivos" runat="server" Skin="Sunset" AutoGenerateColumns="False"
                            Width="90%" GridLines="None">
                            <MasterTableView Width="100%" DataKeyNames="Id" ClientDataKeyNames="Id">
                                <RowIndicatorColumn Visible="False">
                                    <HeaderStyle Width="20px"></HeaderStyle>
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn Visible="False" Resizable="False">
                                    <HeaderStyle Width="20px"></HeaderStyle>
                                </ExpandCollapseColumn>
                                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" NextPagesToolTip="Próxima Páginas"
                                    NextPageToolTip="Próxima Página" PagerTextFormat="Cambiar Página: {4} &amp;nbsp;Mostrando Página {0} de {1}, Registros {2} a {3} de {5}."
                                    PrevPagesToolTip="Páginas Previas" PrevPageToolTip="Página Previas"></PagerStyle>
                                <SortExpressions>
                                    <telerik:GridSortExpression FieldName="Empresa" SortOrder="Ascending" />
                                </SortExpressions>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="NombreLegajo" HeaderText="Nombre Legajo" UniqueName="NombreLegajo">
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Dni" HeaderText="Dni" UniqueName="DniColumn">
                                        <ItemStyle HorizontalAlign="Left" Width="110" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Contrato" HeaderText="Nro Contrato" UniqueName="ContratoColumn">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" Width="90" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Empresa" HeaderText="Empresa" UniqueName="Empresacolumn">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings>
                                <Selecting AllowRowSelect="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
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
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnEliminar" SkinID="btnConosudBasic" 
                    runat="server" Text="Realizar Eliminación" onclick="btnEliminar_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
