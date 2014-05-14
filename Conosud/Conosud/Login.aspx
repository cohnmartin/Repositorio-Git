<%@ Page Language="C#" Theme="MiTema" AutoEventWireup="true" CodeFile="Login.aspx.cs"
    Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Gstión Auditoria Conosud</title>
    <link href="App_Themes/MiTema/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">

        function ShowUsuarioSinPermisos() {

            alert("El usuario con el que ha ingresado al sistema no posee ningún ROL asigando, por favor tome contacto con el administrador del sistema.");

        }
    
    </script>

</head>
<body style="background-color: #E4E4E4;margin-top:2px">
    <form id="form1" runat="server">
    <div align="center">
        <table cellpadding="0" cellspacing="0" style="width: 980px">
            <tr>
                <td align="center" style="border: solid 1px black;background: transparent url('images/rotulo_new.jpg') no-repeat; height: 109px;">
                    &nbsp;
                </td>
            </tr>
            <tr style="padding-top:0px">
                <td align="center" style="height: 430px; border: solid 1px black; background-image: url('images/Fondo_new.jpg');">
                        <table border="0" cellpadding="0" cellspacing="0" style="background-color: Transparent;">
                            <tr>
                                <td style="width: 27px; height: 44px; background-repeat: no-repeat; background-color: Transparent;
                                    background-position: top; background-image: url('Images/ImagenesNewLogin/LoginConosud_r1_c1.png')">
                                    <div style="width: 100%; background-color: #EEEEEE; position:relative;left:15px;top:15px;z-index:-1">
                                    </div>
                                    
                                </td>
                                <td style="width: 350px; background-repeat: repeat-x; background-color: Transparent;
                                    background-position: top; background-image: url('Images/ImagenesNewLogin/LoginConosud_r1_c3.png');
                                    text-align: left; vertical-align: top">
                                    <div style="width: 100%;height:15px;background-color:  #EEEEEE; position:relative;top:35px;z-index:-1">
                                    </div>
                                        <asp:Label runat="server" ID="lblCabec" Style="color: White; font-size: 16px; font-family: Viner Hand ITC;
                                        font-weight: bold; text-align: left; top: -10px; position: relative; left: -15px;z-index:1000"
                                        Text="">Acceso Usuarios Conosud</asp:Label>
                                    
    
                                </td>
                                <td style="width: 27px; background-repeat: no-repeat; background-color: Transparent;
                                    background-position: top; background-image: url('Images/ImagenesNewLogin/LoginConosud_r1_c5.png')">
                                    <div style="width: 100%; background-color:  #EEEEEE; position:relative;left:-10px;top:25px;z-index:-1">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-repeat: repeat-y; background-color: Transparent;
                                    background-position: left top; background-image: url('Images/ImagenesNewLogin/LoginConosud_r3_c1.png')">
                                    <div style="height:140px;background-color:  #EEEEEE;position:relative;left:10px;z-index:-1">
                                    &nbsp;
                                    </div>
                                </td>
                                <td style="width: 350px; height: 140px;">
                                    <table border="0" cellpadding="4" width="100%" cellspacing="0" style="height:100%;font-family: Tahoma;background-color: #EEEEEE;
                                        font-size: 11px">
                                        <tr>
                                            <td align="right" style="color: Black;">
                                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Nombre de Usuario:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="UserName" runat="server" Font-Size="0.8em" Width="150px" Height="15px"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                    ErrorMessage="Nombre de Usuario es obligatorio" ToolTip="Nombre de Usuario es obligatorio"
                                                    ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="color: Black;">
                                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Clave:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Password" runat="server" Font-Size="0.8em" TextMode="Password" Height="15px"
                                                    Width="150px"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                    ErrorMessage="Clave es obligatoria" ToolTip="Clave es obligatoria" ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2" style="font-weight: bold; color: White; padding-top: 5px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2" style="padding-top: 20px">
                                                <asp:Button ID="LoginButton" runat="server" CommandName="Login" SkinID="btnConosudBasic"
                                                    Text="Ingresar" ValidationGroup="Login1" OnClick="LoginButton_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2" style="color: Red">
                                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="height:140px;background-repeat: repeat-y; background-color: Transparent;
                                    background-position: top; background-image: url('Images/ImagenesNewLogin/LoginConosud_r3_c5.png')">
                                    <div  style=" width: 100%;height:100%;background-color:  #EEEEEE; position:relative;right:10px;z-index:-1">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 37px; background-repeat: no-repeat; background-color: Transparent;
                                    background-position: left top; background-image: url('Images/ImagenesNewLogin/LoginConosud_r5_c1.png')">
                                   <div style="width: 100%; background-color:  #EEEEEE; position:relative;left:10px;top:-15px;z-index:-1">
                                    </div>
                                </td>
                                <td align="left" style="padding-top: 15px; font-weight: bold; color: White; height: 37px;
                                    background-repeat: repeat-x; background-color: Transparent; background-position: top;
                                    background-image: url('Images/ImagenesNewLogin/LoginConosud_r5_c3.png'); text-align: left;
                                    vertical-align: top">
                                     <div style="width: 100%; background-color:  #EEEEEE; position:relative;top:-15px;z-index:-1">
                                    </div>
                                </td>
                                <td style="height: 37px; background-repeat: no-repeat; background-color: Transparent;
                                    background-position: top; background-image: url('Images/ImagenesNewLogin/LoginConosud_r5_c5.png')">
                                    <div style="width: 100%; background-color:  #EEEEEE; position:relative;top:-15px;right:15px;z-index:-1">
                                    </div>
                                </td>
                            </tr>
                        </table>
                    
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
