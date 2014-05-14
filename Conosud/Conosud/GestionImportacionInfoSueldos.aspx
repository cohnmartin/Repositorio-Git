<%@ Page Title="Importación Info Sueldos" Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="GestionImportacionInfoSueldos.aspx.cs" Inherits="GestionImportacionInfoSueldos" %>

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
                    ForeColor="#E0D6BE" Text="Importación Información Sueldos" Width="468px"></asp:Label>
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
                                    <telerik:RadUpload ID="RadUpload1" runat="server" ControlObjectsVisibility="None" OverwriteExistingFiles="true"
                                        Skin="Sunset" InputSize="35" TargetFolder="~/ArchivosSueldos" Width="500px" Height="25px">
                                    </telerik:RadUpload>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="buttonSubmit" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td valign="top" align="left">
                            <asp:Button ID="buttonSubmit" SkinID="btnConosudBasic" OnClientClick="return ValidarArchivo();"
                                runat="server" OnClick="buttonSubmit_Click" Text="Subir Archivo" />
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
                            Width="90%" GridLines="None" AllowPaging="True" PageSize="20" OnNeedDataSource="gvArchivos_NeedDataSource">
                            <MasterTableView Width="100%" DataKeyNames="IdArchivoSueldos,Nombre" ClientDataKeyNames="IdArchivoSueldos,Nombre">
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
                                    <telerik:GridSortExpression FieldName="PeriodoNumerico" SortOrder="Descending" />
                                </SortExpressions>
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="" UniqueName="EliminarColumn">
                                        <ItemTemplate>
                                            <asp:ImageButton Style="cursor: hand; text-align: center" ID="imgEliminar" runat="server"
                                                ImageUrl="~/images/delete_16x16.gif" OnClick="imgEliminar_Click" Mensaje="Eliminando Registro e Información Asociada"
                                                OnClientClick="return blockConfirm('Esta seguro de eliminar el archivo con toda la información de sueldos asociada?', event, 330, 100,'','Eliminación Información Sueldos');" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" Width="5px" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="Nombre" HeaderText="Archivo" UniqueName="Nombre">
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="FechaCreacion" HeaderText="Fecha Creación" UniqueName="fechacreacioncolumn" DataFormatString="{0:d}">
                                        <ItemStyle HorizontalAlign="Center" Width="80" />
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Periodo" HeaderText="Periodo" UniqueName="Periodocolumn">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" Width="70" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Empresa" HeaderText="Empresa" UniqueName="Empresacolumn">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" Width="230" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="SegUsuario.Login" HeaderText="Usuario" UniqueName="UsuarioColum">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" Width="80" />
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
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
