<%@ Page Language="C#" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="Imagenes.aspx.cs" Inherits="Imagenes" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <ajaxToolkit:SlideShowExtender id="SlideShowExtender1" runat="server" Loop="True" NextButtonID="Button2" PlayButtonID="Button1" PlayButtonText="Continuar Secuencia" PlayInterval="100" PreviousButtonID="Button3" SlideShowServiceMethod="GetSlides" StopButtonText="Para Secuencia" TargetControlID="Image1" UseContextKey="True">
    </ajaxToolkit:SlideShowExtender>
    <table >
        <tr>
            <td >
                <asp:Button ID="Button3" runat="server" Text="Imagen Anterior" />
                <asp:Button ID="Button1" runat="server" Text="Para Secuencia" />
                <asp:Button ID="Button2" runat="server" Text="Imagen Siguiente" /></td>
        </tr>
        <tr>
            <td >
                <asp:Image ID="Image1" runat="server" AlternateText="No se puede mostrar" Height="317px"
                    Width="444px" /></td>
        </tr>
    </table>
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/output.swf">Acceder a SWF</asp:HyperLink>
</asp:Content>

