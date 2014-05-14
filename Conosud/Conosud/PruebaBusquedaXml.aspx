<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PruebaBusquedaXml.aspx.cs"
    Inherits="PruebaBusquedaXml" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="PruebaBusquedaXml.js" type="text/javascript"></script>
    <link href="ConosudSkin/ToolTip.ConosudSkin.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .MouseOverStyle
        {
            background-color: #D3DCA3;
            color: Black;
            filter: progid:dximagetransform.microsoft.gradient(gradienttype=0,startcolorstr=#D3DCA3,endcolorstr=#ffffff);
            cursor: hand;
            text-decoration: underline;
            font-family: Tahoma;
            font-size: 11px;
        }
        .MouseOutStyle
        {
            background-color: Transparent;
            color: Black;
            cursor: hand;
            text-decoration: underline;
            font-family: Tahoma;
            font-size: 11px;
        }
        ol
        {
            width: 30em; /* room for 3 columns */
        }
        ol li
        {
            float: left;
            width: 10em; /* accommodate the widest item */
            text-decoration: underline;
            font-family: Tahoma;
            font-size: 11px;
        }
        /* stop the floating after the list */br
        {
            clear: left;
        }
        /* separate the list from what follows it */div.wrapper
        {
            margin-bottom: 1em;
        }
        /* anchor styling */ol li a
        {
            display: block;
            width: 7em;
            text-decoration: none;
        }
        ol li a:hover
        {
            color: #FFF; /* white */
            background-color: #A52A2A; /* brown */
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadToolTip runat="server" ID="toolTipEdicion" TargetControlID="searchme" IsClientID="true"
         ManualClose="true" Skin="ConosudSkin" EnableEmbeddedSkins="false"
         Text="sds adas d" Width="300px" Height="180px" Title="Edición Legajo" Animation="Fade"
            Position="BottomCenter" RelativeTo="Element" ShowEvent="OnMouseOver" >
    </telerik:RadToolTip>
        
    <input type="text" id="searchme" onkeyup="return searchIndex();" />
    <input id="HiddenId" type="hidden" runat="server" />
    <asp:UpdatePanel ID="upToolTip" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
             <telerik:RadToolTip runat="server" ID="RadToolTip3" Skin="MySkin"
             Sticky="true"    
             ManualClose="true" 
             Position="MiddleRight"
             Width="570px"
             Height="290px"
             Animation="Fade"    
             OnClientHide="LimpiarTarget" 
             ShowDelay="0"
             EnableEmbeddedSkins="false"
             RelativeTo="Element" > 
                <table runat="server" id="tblEncabezdo" cellpadding="0" border="0" cellspacing="0"
                    style="background-image: url('Imagenes/TalCual-Femeninos/KenzoDEte.jpg'); background-position: center;
                    font-family: Sans-Serif; font-size: 11px; font-weight: bold; text-align: center;
                    color: White; border: 1px solid #9F9F9F; height: 100%; width: 100%">
                    <tr>
                        <td colspan="3" style="height: 30%" valign="bottom">
                            &nbsp;
                            <table width="100%" runat="server" id="tblProducto">
                                <tr>
                                    <td style="width: 77%;">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:Label ID="lblProducto" runat="server" Text="" Font-Names="Bookman Old Style"
                                            ForeColor="Black"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="Row0" style="display: none">
                        <td style="width: 77%">
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblCol0" runat="server" Text="50 ml" ForeColor="Black"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txtValor0" runat="server" Height="16px" CssClass="txtFormatSimple"
                                onkeydown="return ValidarNumerico();" onMousemove="CambiarCursor();" OnClick="SubirCantidad();"
                                Width="60px" MaxLength="3"></asp:TextBox>
                            <input id="IdPres0" type="hidden" runat="server" />
                        </td>
                    </tr>
                    <tr id="Row1" style="display: none">
                        <td style="width: 77%">
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblCol1" runat="server" Text="100 ml" ForeColor="Black"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txtValor1" runat="server" Height="16px" CssClass="txtFormatSimple"
                                onkeydown="return ValidarNumerico();" onMousemove="CambiarCursor();" OnClick="SubirCantidad();"
                                Width="60px" MaxLength="3"></asp:TextBox>
                            <input id="IdPres1" type="hidden" runat="server" />
                        </td>
                    </tr>
                    <tr id="Row2" style="display: none">
                        <td style="width: 77%">
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblCol2" runat="server" Text="Plum" ForeColor="Black"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txtValor2" runat="server" Height="16px" CssClass="txtFormatSimple"
                                onkeydown="return ValidarNumerico();" onMousemove="CambiarCursor();" OnClick="SubirCantidad();"
                                Width="60px" MaxLength="3"></asp:TextBox>
                            <input id="IdPres2" type="hidden" runat="server" />
                        </td>
                    </tr>
                    <tr id="Row3" style="display: none">
                        <td style="width: 77%">
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblCol3" runat="server" Text="Plum" ForeColor="Black"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txtValor3" runat="server" Height="16px" CssClass="txtFormatSimple"
                                onkeydown="return ValidarNumerico();" onMousemove="CambiarCursor();" OnClick="SubirCantidad();"
                                Width="60px" MaxLength="3"></asp:TextBox>
                            <input id="IdPres3" type="hidden" runat="server" />
                        </td>
                    </tr>
                    <tr id="Row4" style="display: none">
                        <td style="width: 77%">
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblCol4" runat="server" Text="Plum" ForeColor="Black"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txtValor4" runat="server" Height="16px" CssClass="txtFormatSimple"
                                onkeydown="return ValidarNumerico();" onMousemove="CambiarCursor();" OnClick="SubirCantidad();"
                                Width="60px" MaxLength="3"></asp:TextBox>
                            <input id="IdPres4" type="hidden" runat="server" />
                        </td>
                    </tr>
                    <tr id="Row5" style="display: none">
                        <td style="width: 77%">
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblCol5" runat="server" Text="Plum" ForeColor="Black"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txtValor5" runat="server" Height="16px" CssClass="txtFormatSimple"
                                onkeydown="return ValidarNumerico();" onMousemove="CambiarCursor();" OnClick="SubirCantidad();"
                                Width="60px" MaxLength="3"></asp:TextBox>
                            <input id="IdPres5" type="hidden" runat="server" />
                        </td>
                    </tr>
                    <tr id="Row6" style="display: none">
                        <td style="width: 77%">
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblCol6" runat="server" Text="Plum" ForeColor="Black"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txtValor6" runat="server" Height="16px" CssClass="txtFormatSimple"
                                onkeydown="return ValidarNumerico();" onMousemove="CambiarCursor();" OnClick="SubirCantidad();"
                                Width="60px" MaxLength="3"></asp:TextBox>
                            <input id="IdPres6" type="hidden" runat="server" />
                        </td>
                    </tr>
                    <tr id="Row7" style="display: none">
                        <td style="width: 77%">
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblCol7" runat="server" Text="Plum" ForeColor="Black"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txtValor7" runat="server" Height="16px" CssClass="txtFormatSimple"
                                onkeydown="return ValidarNumerico();" onMousemove="CambiarCursor();" OnClick="SubirCantidad();"
                                Width="60px" MaxLength="3"></asp:TextBox>
                            <input id="IdPres7" type="hidden" runat="server" />
                        </td>
                    </tr>
                    <tr id="Row8" style="display: none">
                        <td style="width: 77%">
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblCol8" runat="server" Text="Plum" ForeColor="Black"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txtValor8" runat="server" Height="16px" CssClass="txtFormatSimple"
                                onkeydown="return ValidarNumerico();" onMousemove="CambiarCursor();" OnClick="SubirCantidad();"
                                Width="60px" MaxLength="3"></asp:TextBox>
                            <input id="IdPres8" type="hidden" runat="server" />
                        </td>
                    </tr>
                    <tr id="Row9" style="display: none">
                        <td style="width: 77%">
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblCol9" runat="server" Text="Plum" ForeColor="Black"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txtValor9" runat="server" Height="16px" CssClass="txtFormatSimple"
                                onkeydown="return ValidarNumerico();" onMousemove="CambiarCursor();" OnClick="SubirCantidad();"
                                Width="60px" MaxLength="3"></asp:TextBox>
                            <input id="IdPres9" type="hidden" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 77%">
                            &nbsp;
                        </td>
                        <td colspan="2" align="center">
                            <asp:Button ID="btnSolicitar" runat="server" Text="Solicitar"  />
                        </td>
                    </tr>
                </table>
            </telerik:RadToolTip>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
    <div id="resultshere" class="wrapper">
    </div>
</body>
</html>
