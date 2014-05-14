<%@ Page Title="Gestión Instructivos" Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="GestionInstructivos.aspx.cs" Inherits="GestionInstructivos" %>

<%@ Register Assembly="ControlsAjaxNotti" Namespace="ControlsAjaxNotti" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        var isEdicion = false;

        function aa() {
            $find("<%=ServerControlWindow1.ClientID %>").HideWaiting();
        }

        function ValidarArchivo() {

            if (!isEdicion) {
                var archivo = $find("<%=RadUpload1.ClientID %>").getFileInputs()[0].value;

                if (archivo == "") {
                    radalert("Debe seleccionar un archivo para poder completar la acción", 300, 100);
                    return false;
                }
            }


            if (Page_ClientValidate()) {
                $find("<%=ServerControlWindow1.ClientID %>").ShowWaiting(null, "Preparando para subir archivo..");
                return true;
            }
            else
                return false;

        }

        function NuevoInstructivo() {
            isEdicion = false;
            $find("<%=pnlEdicion.ClientID %>").ClearElements();

            $get("<%=txtAlias.ClientID %>").value = "";
            $get("<%=idInstructivo.ClientID %>").value = "";
            $('#trArchivoActual').css("display", "none");
            $('#divPrincipal').css("height", "240px");
            

            $find("<%=ServerControlWindow1.ClientID %>").set_CollectionDiv('divPrincipal');
            $find("<%=ServerControlWindow1.ClientID %>").ShowWindows('divPrincipal', "Nuevo Instructivo");

        }

        function EditarInstructivo(sender, Id) {
            isEdicion = true;
            $find("<%=pnlEdicion.ClientID %>").ClearElements();
            $find("<%=GridInstructivos.ClientID %>").initEdit($find("<%=ServerControlWindow1.ClientID %>"), Id);

            $get("<%=idInstructivo.ClientID %>").value = Id;
            $('#trArchivoActual').css("display", "block");
            $('#divPrincipal').css("height", "265px");


            $find("<%=ServerControlWindow1.ClientID %>").set_CollectionDiv('divPrincipal');
            $find("<%=ServerControlWindow1.ClientID %>").ShowWindows('divPrincipal', "Edición Instructivo");
        }

        function EliminarInstructivo(sender, Id) {

            radconfirm("Esta seguro que desea eliminar el instructivo? También se eliminara el archivo físico", ConfirmDelete, 300, 100, null, "Eliminación");

            function ConfirmDelete(result) {

                if (result) {
                    var id = $find("<%=GridInstructivos.ClientID %>").get_KeyValueSelected();
                    var Archivo = $find("<%=GridInstructivos.ClientID %>").get_ItemsDataSelected()[0].NombreFisico

                    PageMethods.EliminarRegistro(id, Archivo, FillGrid);
                }
            }
        }

        function FillGrid(result) {

            $find("<%=ServerControlWindow1.ClientID %>").CloseWindows('divPrincipal');
            $find("<%=GridInstructivos.ClientID %>").set_ClientdataSource(result);
        }

    </script>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Sunset" VisibleTitlebar="true"
        Style="z-index: 900000000" Title="Información Requerida">
    </telerik:RadWindowManager>
    <table cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td align="left">
                <table cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td align="center" style="height: 35px; padding-left: 15px; padding-top: 15px">
                            <asp:Label ID="lblTipoGestion" runat="server" Font-Bold="True" Font-Size="20pt" Font-Underline="false"
                                Font-Italic="True" ForeColor="black" Text="GESTION DE INSTRUCTIVOS" Font-Names="Arno Pro"></asp:Label>
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
                                AllowPaging="false" PageSize="58" EmptyMessage="No Existen Instructivos">
                                <FunctionsGral>
                                    <cc1:FunctionGral Type="Add" Text="Crear Nuevo Ingreso" ClickFunction="NuevoInstructivo" />
                                </FunctionsGral>
                                <FunctionsColumns>
                                    <cc1:FunctionColumnRow Type="Delete" ClickFunction="EliminarInstructivo" Text="Eliminar Datos" />
                                    <cc1:FunctionColumnRow Type="Edit" ClickFunction="EditarInstructivo" Text="Ver Datos" />
                                </FunctionsColumns>
                                <Columns>
                                    <cc1:Column CssClass="tdSimple" Width="100px" HeaderName="Fecha" DataFieldName="Fecha" Align="Centrado"
                                        ExportToExcel="true" />
                                    <cc1:Column CssClass="tdSimple" Width="200px" HeaderName="Nombre Fisico" DataFieldName="NombreFisico"
                                        Align="Centrado" ExportToExcel="true" NameControlManger="txtArchivoActual" />
                                    <cc1:Column CssClass="tdSimple"  Width="200px" HeaderName="Alias" DataFieldName="NombreAlias" Align="Centrado"
                                        ExportToExcel="true" NameControlManger="txtAlias" />
                                    <cc1:Column CssClass="tdSimple" HeaderName="Descripcion" DataFieldName="Descripcion"
                                        Align="Centrado" ExportToExcel="true" NameControlManger="txtDescricpcion" />
                                </Columns>
                            </cc1:ClientControlGrid>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <cc1:ServerControlWindow ID="ServerControlWindow1" runat="server" BackColor="WhiteSmoke"
        WindowColor="Rojo">
        <ContentControls>
            <cc1:ServerPanel runat="server" ID="pnlEdicion">
                <BodyContent>
                    <div id="divPrincipal" style="height: 270px; width: 720px">
                        <table border="0" cellpadding="0" cellspacing="4" width="100%" style="padding-top: 0px;
                            text-align: left;">
                            <tr>
                                <td colspan="2" style="background-color: #CACACA; padding-left: 10px; height: 30px;">
                                    <asp:Label ID="lblTituloCaractesticas" runat="server" SkinID="lblConosud" Text="DATOS DEL ARCHIVO"></asp:Label>
                                </td>
                            </tr>
                            <tr style="padding-top: 5px">
                                <td style="width: 100px">
                                    <asp:Label ID="Label2" runat="server" SkinID="lblConosud" Text="Alias Archivo:"></asp:Label>
                                </td>
                                <td align="left" style="padding-left: 5px; padding-right: 5px">
                                    <asp:TextBox Width="550px" ID="txtAlias" runat="server" ViewStateMode="Disabled" ></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatortxtAlias" ControlToValidate="txtAlias"
                                        Display="Dynamic" ErrorMessage="*" runat="server">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="Label1" runat="server" SkinID="lblConosud" Text="Descripción:"></asp:Label>
                                </td>
                                <td align="left" style="padding-left: 5px; padding-right: 5px">
                                    <asp:TextBox Width="550px" ID="txtDescricpcion" TextMode="MultiLine" Rows="5" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatortxtDescricpcion" ControlToValidate="txtDescricpcion"
                                        Display="Dynamic" ErrorMessage="*" runat="server">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr style="display: none" id="trArchivoActual">
                                <td style="width: 100px">
                                    <asp:Label ID="Label4" runat="server" SkinID="lblConosud" Text="Archivo Actual:"></asp:Label>
                                </td>
                                <td align="left" style="padding-left: 5px; padding-right: 5px">
                                    <asp:TextBox Width="250px" ReadOnly="true" ID="txtArchivoActual" runat="server"></asp:TextBox>
                                    <asp:HiddenField ID="idInstructivo" runat="server" Value="" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="Label3" runat="server" SkinID="lblConosud" Text="Archivo:"></asp:Label>
                                </td>
                                <td align="left" style="padding-left: 5px; padding-right: 5px">
                                    <asp:UpdatePanel runat="server" ID="UpArchivo" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <telerik:RadUpload ID="RadUpload1" runat="server" ControlObjectsVisibility="None" 
                                                OverwriteExistingFiles="true" Skin="Vista" InputSize="75" TargetFolder="~/Instructivos"
                                                Width="500px" Height="25px">
                                            </telerik:RadUpload>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnAplicar" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <div style="position: absolute; top: 40px; left: 23%;">
                                        <telerik:RadProgressManager ID="RadProgressManager1" runat="server" />
                                        <telerik:RadProgressArea ID="RadProgressArea1" runat="server" Skin="Sunset"
                                          OnClientProgressUpdating="aa"   
                                         ProgressIndicators="RequestSize, FilesCountBar, FilesCount, FilesCountPercent, SelectedFilesCount, CurrentFileName, TimeElapsed, TimeEstimated, TransferSpeed">
                                            <Localization Uploaded="Uploaded" CurrentFileName="Subiendo Archivo: "  />
                                            
                                        </telerik:RadProgressArea>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center" style="padding-top: 5px">
                                    <asp:Button ID="btnAplicar" OnClick="btnAplicar_Click" OnClientClick="return ValidarArchivo();"
                                        runat="server" Text="Grabar" SkinID="btnConosudBasic" CausesValidation="true" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </BodyContent>
            </cc1:ServerPanel>
        </ContentControls>
    </cc1:ServerControlWindow>
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
