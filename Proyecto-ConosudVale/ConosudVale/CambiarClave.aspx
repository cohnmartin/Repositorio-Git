<%@ Page MasterPageFile="~/DefaultMasterPage.master" Theme="MiTema" Language="C#" AutoEventWireup="true" CodeFile="CambiarClave.aspx.cs" Inherits="CambiarClave" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server" >

    <div style="height: 444px">
        <div style="text-align: center">
            <table >
                <tr>
                    <td align=center style="width: 300px">
                        <table style="width:100%; height: 170px;border:1px solid black" border="0">
                            <tr>
                                <td style="width: 141px">
                                    <asp:Label ID="Label1"  SkinID="lblConosud" runat="server" 
                                        Text="Clave Actual:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtClave" runat="server" TextMode="Password"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 141px">
                                    <asp:Label ID="Label2" SkinID="lblConosud"  runat="server" Text="Nueva Clave:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNuevaClave" runat="server" TextMode="Password"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 141px">
                                    <asp:Label ID="Label3"  SkinID="lblConosud"  runat="server" Text="Confirmación:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConfClave" runat="server" TextMode="Password"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan='2'>
                                    <asp:Label ID="Label4" runat="server" ForeColor="#FF3300"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 50%" align="right">
                                    <asp:Button ID="btnCambiar" runat="server" SkinID="btnConosudBasic"
                                        Text="Cambiar Clave" OnClick="btnCambiar_Click" Mensaje="Buscando Elementos..." />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btnCancelar" runat="server" SkinID="btnConosudBasic"
                                        Text="Cancelar" OnClick="btnCancelar_Click" Mensaje="Buscando Elementos..." />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 179px; text-align: center;">
                    </td>
                </tr>
            </table>
        </div>
    
    </div>

</asp:Content>