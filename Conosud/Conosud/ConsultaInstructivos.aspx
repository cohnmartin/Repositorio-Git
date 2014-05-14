<%@ Page Title="Descarga de Instructivos" Language="C#" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="ConsultaInstructivos.aspx.cs" Inherits="ConsultaInstructivos" %>

<%@ Register Assembly="ControlsAjaxNotti" Namespace="ControlsAjaxNotti" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function DownLoad(sender, id) {
            var Archivo = $find("<%=GridInstructivos.ClientID %>").get_ItemDataByKey(id).NombreFisico
            window.open("Instructivos/" + Archivo, 'Download');
        }
    </script>

      <table cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td align="left">
                <table cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td align="center" style="height: 35px; padding-left: 15px; padding-top: 15px">
                            <asp:Label ID="lblTipoGestion" runat="server" Font-Bold="True" Font-Size="20pt" Font-Underline="false"
                                Font-Italic="True" ForeColor="black" Text="DESCARGA DE INSTRUCTIVOS" Font-Names="Arno Pro"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table id="Table2" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
                    border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #E0D6BE;
                    font-family: Sans-Serif; font-size: 11px;" width="90%">
                    <tr>
                        <td align="center">
                            <cc1:ClientControlGrid ID="GridInstructivos" runat="server" AllowMultiSelection="false"
                                TypeSkin="Sunset" PositionAdd="Top" AllowRowSelection="true" Width="100%" KeyName="IdInstructivo"
                                AllowPaging="false" PageSize="8" EmptyMessage="No Existen Instructivos">
                                <FunctionsGral>
                                </FunctionsGral>
                                <FunctionsColumns>
                                    <cc1:FunctionColumnRow Type="Custom" ClickFunction="DownLoad" Text="Descargar" ImgUrl="images/down.png" />
                                </FunctionsColumns>
                                <Columns>
                                    <cc1:Column CssClass="tdSimple" HeaderName="Fecha" DataFieldName="Fecha" Align="Centrado"
                                        ExportToExcel="true" />
                                    <cc1:Column CssClass="tdSimple" HeaderName="Nombre" DataFieldName="NombreAlias" Align="Centrado"
                                        ExportToExcel="true" NameControlManger="txtAlias" />
                                    <cc1:Column CssClass="tdSimple" HeaderName="Descripcion" DataFieldName="Descripcion"
                                        Align="Centrado" ExportToExcel="true" NameControlManger="txtDescricpcion" />
                                         <cc1:Column CssClass="tdSimple" HeaderName="Nombre Fisico" DataFieldName="NombreFisico"
                                        Align="Centrado" ExportToExcel="true" Display="false" />
                                </Columns>
                            </cc1:ClientControlGrid>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
