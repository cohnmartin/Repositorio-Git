<%@ Page Language="C#" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="height:90px">&nbsp;</div>
    <table border="0" cellpadding="0" cellspacing="0" style="background-color: Transparent;">
        <tr>
            <td style="width: 27px; height: 44px; background-repeat: no-repeat; background-color: Transparent;
                background-position: top; background-image: url('Images/ImagenesNewLogin/LoginConosud_r1_c1.png')">
            </td>
            <td style="width: 350px; background-repeat: repeat-x; background-color: Transparent;
                background-position: top; background-image: url('Images/ImagenesNewLogin/LoginConosud_r1_c3.png');
                text-align: left; vertical-align: top">
                <asp:Label runat="server" ID="lblCabec" Style="color: White; font-size: 16px; font-family: Viner Hand ITC;
                    font-weight: bold; text-align: left; top: 5px; position: relative; left: -15px"
                    Text="">Conosud S.R.L.</asp:Label>
            </td>
            <td style="width: 27px; background-repeat: no-repeat; background-color: Transparent;
                background-position: top; background-image: url('Images/ImagenesNewLogin/LoginConosud_r1_c5.png')">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="height: 140px; background-repeat: repeat-y; background-color: Transparent;
                background-position: left top; background-image: url('Images/ImagenesNewLogin/LoginConosud_r3_c1.png')">
                &nbsp;
            </td>
            <td style="width: 350px; height: 140px;">
                <table border="0" width="100%">
                    <tr align="center">
                        <td style="font-size: 30px;" align="center">
                            <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Verdama" Font-Size="1em"
                                Text="Bienvenido"></asp:Label>
                            <br />
                            <asp:Label ID="lblNombreUsu" runat="server" Font-Bold="True" Font-Size="XX-Large"
                                Text="Label"></asp:Label>
                            <br />
                            <asp:LinkButton ID="lbCambiarClave" runat="server" ForeColor="#804000" Font-Bold="True"
                                Font-Names="Sand Serif" PostBackUrl="~/CambiarClave.aspx" Font-Size="Small">Cambiar clave Usuario Actual</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="height: 140px; background-repeat: repeat-y; background-color: Transparent;
                background-position: top; background-image: url('Images/ImagenesNewLogin/LoginConosud_r3_c5.png')">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="height: 37px; background-repeat: no-repeat; background-color: Transparent;
                background-position: left top; background-image: url('Images/ImagenesNewLogin/LoginConosud_r5_c1.png')">
                &nbsp;
            </td>
            <td align="left" style="padding-top: 15px; font-weight: bold; color: White; height: 37px;
                background-repeat: repeat-x; background-color: Transparent; background-position: top;
                background-image: url('Images/ImagenesNewLogin/LoginConosud_r5_c3.png'); text-align: left;
                vertical-align: top">
            </td>
            <td style="height: 37px; background-repeat: no-repeat; background-color: Transparent;
                background-position: top; background-image: url('Images/ImagenesNewLogin/LoginConosud_r5_c5.png')">
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
