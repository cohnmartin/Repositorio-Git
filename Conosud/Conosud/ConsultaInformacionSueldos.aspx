<%@ Page Language="C#" Theme="MiTema" AutoEventWireup="true" CodeFile="ConsultaInformacionSueldos.aspx.cs" Inherits="ConsultaInformacionSueldos" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Consulta Información Sueldos</title>
    <style type="text/css">
        .style1
        {
            width: 70%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
     <telerik:RadScriptManager ID="RadScriptManager1" Runat="server">
        <Scripts>
            <asp:ScriptReference Path="~/FuncionesComunes.js" />
        </Scripts>
      </telerik:RadScriptManager>
    <div>
    
    
        <table cellpadding="0" cellspacing="0" style="width:70%">
        <tr>
            <td colspan="2" align="center">
            <table cellpadding="0" cellspacing="5" style="width: 80%">
        <tr>
            <td align="center" style="height: 25px; background: url('images/sprite.gif') 0  -997px repeat-x">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="14pt" Font-Names="Sans-Serif"
                    ForeColor="#E0D6BE" Text="Detalle Información Sueldos" 
                    Width="468px"></asp:Label>
            </td>
        </tr>
    </table>
            </td>
        </tr>
            <tr runat="server" id="TR_Periodos" style="padding-bottom:10px">
                <td align="right" style="width:40%">
                    <asp:Label ID="lblPeriodoDesc" SkinID="lblConosud" runat="server" Text="Período:" ></asp:Label>
                    </td>
                <td align="left" style="width:50%;padding-left:5px">
                    <telerik:RadComboBox ID="cboPeriodos" runat="server"  Width="208px"
                        Skin="Sunset" AutoPostBack="True"  EmptyMessage="Seleccione un Período"
                        AllowCustomText="true" MarkFirstMatch="true"
                        onselectedindexchanged="cboPeriodos_SelectedIndexChanged">
                    
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr >
                <td colspan="2">
                    <asp:UpdatePanel ID="upDatosSueldo" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table cellpadding="0" cellspacing="0" border="1" style="width: 100%" runat="server" id="Tbl_Datos">
                                <tr >
                                    <td>
                                        <asp:Label ID="lblPeriodo0" SkinID="lblConosud" runat="server" Text="Básico/ValorHora:"></asp:Label>
                                    </td>
                                    <td style="padding-left:5px">
                                        <asp:Label ID="lblBasico_ValorHora" SkinID="lblConosudNormal" runat="server" Text="0"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPeriodo4" SkinID="lblConosud" runat="server" Text="Básico Liquidado:"></asp:Label>
                                    </td>
                                    <td style="padding-left:5px">
                                        <asp:Label ID="lblBasicoLiquidado" SkinID="lblConosudNormal" runat="server" Text="0"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPeriodo1" SkinID="lblConosud" runat="server" Text="Horas Extras:"></asp:Label>
                                    </td>
                                    <td style="padding-left:5px">
                                        <asp:Label ID="lblHorasExtras" SkinID="lblConosudNormal" runat="server" Text="0"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPeriodo5" SkinID="lblConosud" runat="server" Text="Adic. Remunerativos:"></asp:Label>
                                    </td>
                                    <td style="padding-left:5px">
                                        <asp:Label ID="lblAdicionalesRemunerativos" SkinID="lblConosudNormal" runat="server" Text="0"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPeriodo2" SkinID="lblConosud" runat="server" Text="Vacaciones:"></asp:Label>
                                    </td>
                                    <td style="padding-left:5px">
                                        <asp:Label ID="lblVacaciones" SkinID="lblConosudNormal" runat="server" Text="0"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPeriodo6" SkinID="lblConosud" runat="server" Text="SAC:"></asp:Label>
                                    </td>
                                    <td style="padding-left:5px">
                                        <asp:Label ID="lblSAC" SkinID="lblConosudNormal" runat="server" Text="0"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPeriodo3" SkinID="lblConosud" runat="server" Text="Total Bruto:"></asp:Label>
                                    </td>
                                    <td style="padding-left:5px">
                                        <asp:Label ID="lblTotalBruto" SkinID="lblConosudNormal" runat="server" Text="0"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPeriodo7" SkinID="lblConosud" runat="server" Text="Asignación Familiar:"></asp:Label>
                                    </td>
                                    <td style="padding-left:5px">
                                        <asp:Label ID="lblAsignacionFamiliar" SkinID="lblConosudNormal" runat="server" Text="0"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPeriodo9" SkinID="lblConosud" runat="server" Text="Adic. NO Remunerativos:"></asp:Label>
                                    </td>
                                    <td style="padding-left:5px">
                                        <asp:Label ID="lblAdicionalesNORemunerativos" SkinID="lblConosudNormal" runat="server" Text="0"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPeriodo8" SkinID="lblConosud" runat="server" Text="Descuentos:"></asp:Label>
                                    </td>
                                    <td style="padding-left:5px">
                                        <asp:Label ID="lblDescuentos" SkinID="lblConosudNormal" runat="server" Text="0"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:Label ID="lblPeriodo10" SkinID="lblConosud" runat="server" Text="Total Neto:"></asp:Label>
                                    </td>
                                    <td colspan="2" style="padding-left: 5px">
                                        <asp:Label ID="lblTotalNeto" SkinID="lblConosudNormal" runat="server" Text="0"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table cellpadding="0" cellspacing="0" border="1" style="width: 100%" runat="server" id="Tbl_SinDatos" visible="false" >
                                <tr >
                                    <td colspan="2" align="center">
                                        El legajo no posee datos para este periodo.
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cboPeriodos" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                   
                </td>
            </tr>
            
        </table>
    
    </div>
    </form>
</body>
</html>
