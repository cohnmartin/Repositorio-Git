<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true"
    CodeFile="ReporteRecursosAfectados.aspx.cs" Inherits="ReporteRecursosAfectados" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Assembly="ControlsAjaxNotti" Namespace="ControlsAjaxNotti" TagPrefix="AjaxInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadScriptBlock runat="server">
        <script type="text/javascript">

            function BuscarDatos() {
                $find("<%=GridRecursosAfectados.ClientID %>").ShowWaiting("Buscando Recursos...");
                PageMethods.GetData($get("<%=cboPeriodos.ClientID %>").value, 0, $find("<%=GridRecursosAfectados.ClientID %>").get_pageSize(), FillGrid, Error);

            }


            function FillGrid(result) {
                $find("<%=GridRecursosAfectados.ClientID %>").set_ClientdataSource(result);
            }

            function Error(err) {
                alert(err.get_message());
            }
        </script>
    </telerik:RadScriptBlock>
    <table cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td align="left">
                <table cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td align="center" style="height: 35px; padding-left: 15px; padding-top: 15px">
                            <asp:Label ID="lblTipoGestion" runat="server" Font-Bold="True" Font-Size="20pt" Font-Underline="false"
                                Font-Italic="True" ForeColor="black" Text="REPORTE PERSONAL AFECTADO" Font-Names="Arno Pro"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <table style="background-color: transparent; font-family: Sans-Serif; font-size: 11px;
                                width: 100%; vertical-align: middle;" border="0">
                                <tr>
                                    <td valign="middle" align="right" style="padding-left: 10px; width: 340px">
                                        <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Names="Arno Pro" ForeColor="#8C8C8C"
                                            Font-Size="16px" Text="Periodo:"></asp:Label>
                                    </td>
                                    <td valign="middle" align="left" width="310px">
                                        <asp:DropDownList ID="cboPeriodos" runat="server" Width="208px" Height="22px">
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="middle" align="left">
                                        <asp:ImageButton runat="server" Style="padding-left: 15px; padding-bottom: 15px;
                                            border: 0px; vertical-align: middle;" ImageUrl="~/Images/Search.png" ID="imgBuscar"
                                            OnClientClick="BuscarDatos();return false;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center">
                                        <div style="width: 95%; height: 8px; border-top-style: solid; border-top-width: 2px;
                                            border-top-color: #808080;">
                                            &nbsp;
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table id="Table3" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
                    border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #E0D6BE;
                    font-family: Sans-Serif; font-size: 11px;" width="98%">
                    <tr>
                        <td align="center">
                            <AjaxInfo:ClientControlGrid ID="GridRecursosAfectados" runat="server" AllowMultiSelection="false"
                                TypeSkin="Sunset" PositionAdd="Top" AllowRowSelection="false" Width="100%" KeyName="IdLegajos"
                                AllowPaging="false" PageSize="8">
                                <FunctionsGral>
                                    <AjaxInfo:FunctionGral Type="Excel" Text="Exportar Datos" />
                                </FunctionsGral>
                                <Columns>
                                    <AjaxInfo:Column Capitalice="true" HeaderName="Periodo Consulta" DataFieldName="PeriodoConsulta"
                                        Align="Derecha" ExportToExcel="true" />
                                    <AjaxInfo:Column Capitalice="true" HeaderName="Apellido" DataFieldName="Apellido"
                                        Align="Derecha" ExportToExcel="true" />
                                    <AjaxInfo:Column Capitalice="true" HeaderName="Nombre" DataFieldName="Nombre" Align="Derecha"
                                        ExportToExcel="true" />
                                    <AjaxInfo:Column HeaderName="CUIL" DataFieldName="CUIL" Align="Derecha" ExportToExcel="true" />
                                    <AjaxInfo:Column Capitalice="true" HeaderName="Emp Contratista" DataFieldName="EmpContratista"
                                        Align="Derecha" ExportToExcel="true" />
                                        <AjaxInfo:Column Capitalice="true" HeaderName="Sub Contratista" DataFieldName="EmpSubContratista"
                                        Align="Derecha" ExportToExcel="true" />
                                    <AjaxInfo:Column Capitalice="true" HeaderName="Empresa Asignada" DataFieldName="RazonSocial"
                                        Align="Derecha" ExportToExcel="true" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="CUIT Empresa" DataFieldName="CUITEmpresa"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Es Contratista" DataFieldName="EsContratista"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Nro Contrato" DataFieldName="Codigo"
                                        ExportToExcel="true" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Inicio Contrato" DataFieldName="FechaInicioContrato"
                                        ExportToExcel="true" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Afectacion" DataFieldName="PeriodoAfectacion"
                                        ExportToExcel="true" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Periodo Baja" DataFieldName="PeriodoBaja"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Tramite Baja" DataFieldName="FechaTramiteBaja"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Vencimiento Contrato" DataFieldName="FechaVencimientoContrato"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Prorroga Contrato" DataFieldName="Prorroga"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Fiscal Nombre" DataFieldName="FiscalNombre"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Fiscal Email" DataFieldName="FiscalEmail"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Gestor Email" DataFieldName="GestorEmail"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Gestor Nombre" DataFieldName="GestorNombre"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Categoria" DataFieldName="DescCategoria"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Contratado Por" DataFieldName="DescContratadoPor"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Tipo Contrato" DataFieldName="DescTipoContrato"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Area" DataFieldName="DescArea" ExportToExcel="true"
                                        Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Servicio" DataFieldName="Servicio"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Tipo Documento" DataFieldName="DescTipoDocumento"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="NroDoc" DataFieldName="NroDoc" ExportToExcel="true"
                                        Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Nacimiento" DataFieldName="FechaNacimiento"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Sexo" DataFieldName="Sexo" ExportToExcel="true"
                                        Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Estado Civil" DataFieldName="DescEstadoCicil"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Nacionalida" DataFieldName="DescNacionalida"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Direccion" DataFieldName="Direccion"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Codigo Postal" DataFieldName="CodigoPostal"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Provincia" DataFieldName="DescProvincia"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Telefono Fijo" DataFieldName="TelefonoFijo"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Correo Electronico" DataFieldName="CorreoElectronico"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Convenio" DataFieldName="DescConvenio"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Ingreso Empresa" DataFieldName="FechaIngreos"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Grupo Sangre" DataFieldName="GrupoSangre"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Autorizado Conducir" DataFieldName="AutorizadoCond"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Cred. Vencimiento" DataFieldName="CredVencimiento"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Ultimo Examen Medico" DataFieldName="FechaUltimoExamen"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Habilitar Credencial" DataFieldName="HabilitarCredencial"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Estado Verificacion" DataFieldName="DescEstadoVerificacion"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Ultima Verificacion" DataFieldName="FechaUltimaVerificacion"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Observacion Auditoria" DataFieldName="Observacion"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Ultima Modificacion" DataFieldName="FechaUltmaModificacion"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Funcion" DataFieldName="Funcion" ExportToExcel="true"
                                        Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Nro Poliza" DataFieldName="NroPoliza"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Desc. Seguro" DataFieldName="DescDescSeguro"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Compañia Seguro" DataFieldName="DescCompañiaSeguro"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Inicio Seguro" DataFieldName="FechaInicial"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Vencimiento Seguro" DataFieldName="FechaVencimiento"
                                        ExportToExcel="true" Display="false" />
                                    <AjaxInfo:Column Align="Centrado" HeaderName="Ultimo Pago Seguro" DataFieldName="FechaUltimoPago"
                                        ExportToExcel="true" Display="false" />
                                </Columns>
                            </AjaxInfo:ClientControlGrid>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
