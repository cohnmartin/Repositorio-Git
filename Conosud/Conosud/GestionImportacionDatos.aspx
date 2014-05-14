<%@ Page Title="" Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="GestionImportacionDatos.aspx.cs" Inherits="GestionImportacionDatos" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <telerik:RadWindowManager ID="RadWindowManager1" ShowContentDuringLoad="false" VisibleStatusbar="false"
        ReloadOnShow="false" runat="server" Skin="Sunset">
    </telerik:RadWindowManager>
    
    <table style="width: 100%">
        <tr >
            <td style="width: 318px">
                <asp:Label ID="Label3" runat="server" Text="1º Importación de Empresas:"></asp:Label>
            </td>
            <td>
                <asp:Button ID="btnEmpresas" SkinID="btnConosud" runat="server" Text="Button" OnClick="btnEmpresas_Click" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr >
            <td style="width: 318px">
                <asp:Label ID="Label1" runat="server" Text="2º Importación de Contratos:"></asp:Label>
            </td>
            <td>
                <asp:Button ID="btnContratos" SkinID="btnConosud" runat="server" OnClick="btnContratos_Click"
                    Text="Button" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr >
            <td style="width: 318px">
                <asp:Label ID="Label2" runat="server" Text="3º Importación de Legajos:"></asp:Label>
            </td>
            <td>
                <asp:Button ID="btnLegajo" runat="server" SkinID="btnConosud" OnClick="btnLegajo_Click"
                    Text="Button" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
       
        <tr >
            <td style="width: 318px">
                <asp:Label ID="Label4" runat="server" 
                    Text="Actualizacion Contratos Prorrogados"></asp:Label>
            </td>
            <td>
                <asp:Button ID="btnContProrr" runat="server" SkinID="btnConosud" OnClick="btnContProrr_Click"
                    Text="Button" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
       
        <tr >
            <td style="width: 318px">
                <asp:Label ID="Label5" runat="server" 
                    Text="Actualizacion Contratos Mes 11"></asp:Label>
            </td>
            <td>
                <asp:Button ID="Button1" runat="server" SkinID="btnConosud" OnClick="btnMesNoviembre_Click"
                    Text="Button" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        
        <tr >
            <td style="width: 318px">
                <asp:Label ID="Label6" runat="server" 
                    Text="Actualizacion Hoja"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label7" runat="server" 
                    Text="IdContratoEmpresa:"></asp:Label>
                <asp:TextBox ID="txtIdContratoEmpresa" runat="server" Width="75px"></asp:TextBox>
                <asp:Label ID="Label8" runat="server" 
                    Text="Mes/Año:"></asp:Label>
                <asp:TextBox ID="txtMesAño" runat="server" Width="75px"></asp:TextBox>
                <asp:Button ID="btnGenerar" runat="server" SkinID="btnConosud" OnClick="btnGenerar_Click"
                    Text="Button" ToolTip="Genera la cabecera para el mes/Año con la hojas y los legajos"/>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        
    </table>

   

</asp:Content>

