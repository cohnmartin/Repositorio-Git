<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="ReporteComentariosHojas.aspx.cs" Inherits="ReporteComentariosHojas" Title="Untitled Page" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript" language="javascript">
    function AbrirDocumento(nombre)
    {
        if (nombre != "Resumn.pdf")
            window.open('Documentos\\' + nombre);
    }

</script>

    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" EnablePartialRendering="true" runat="server"> </ajaxToolkit:ToolkitScriptManager>
    <table cellpadding="0" cellspacing="5">
        <tr>
            <td align="center" style="width: 241px">
                <asp:Label ID="lblEncabezadoS" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Font-Bold="True" Font-Size="14pt" ForeColor="Maroon" Height="25px"
                    Style="background-image: url(images/FondoTitulos.gif); background-color: transparent"
                    Text="INFORME POR PERIODO" Width="368px"></asp:Label></td>
        </tr>
    </table>
    
    <table id="tblEncabezado" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
        border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #f1dcdc">
        <tr>
            <td align="right" style="width: 98px; height: 26px">
                <asp:Label ID="Label3" runat="server" Font-Bold="True" ForeColor="Maroon" Height="22px"
                    Text="Empresa:" Width="79px"></asp:Label></td>
            <td id="PrimerFila" align="left" style="width: 95px; height: 26px">
                <asp:DropDownList ID="cboEmpresasAjax" runat="server" Width="350px" OnSelectedIndexChanged="cbo_SelectedIndexChanged" AutoPostBack="True">
                </asp:DropDownList></td>
            <td align="right" style="width: 34px">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Maroon" Height="22px"
                    Text="Contratos:" Width="85px"></asp:Label></td>
            <td align="left" style="width: 312px">
                &nbsp;<asp:DropDownList ID="cboContratosAjax" runat="server" Height="23px" Width="236px" OnSelectedIndexChanged="cbo_SelectedIndexChanged" AutoPostBack="True">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td align="right" style="width: 98px; height: 26px">
                <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="Maroon" Height="20px"
                    Text="Contratistas:" Width="85px"></asp:Label></td>
            <td align="left" style="width: 95px; height: 26px">
                <asp:DropDownList ID="cboContratistasAjax" runat="server" Width="350px" OnSelectedIndexChanged="cbo_SelectedIndexChanged" AutoPostBack="True">
                </asp:DropDownList></td>
            <td align="right" style="width: 34px; height: 26px">
                <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="Maroon" Height="19px"
                    Text="Periodos:" Width="85px"></asp:Label></td>
            <td align="left" style="width: 312px; height: 26px">
                &nbsp;<asp:DropDownList ID="cboPriodosAjax" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbo_SelectedIndexChanged"
                    Width="168px">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td align="center" colspan="5" style="height: 26px">
                &nbsp; &nbsp;
                <asp:ImageButton ID="btnBusquedaPrincipal" runat="server" Height="20px" ImageUrl="~/images/search_16x16.gif"
                    OnClick="btnBusquedaPrincipal_Click" Width="24px" /></td>
        </tr>
    </table>
    &nbsp;
    <ajaxToolkit:CascadingDropDown ID="CascadingDropDown2" runat="server" Category="Contratos"
        LoadingText="Cargando..." ParentControlID="cboEmpresasAjax" PromptText="Seleccione un Contrato"
        ServiceMethod="GetContratos" ServicePath="ServiceResumen.asmx" TargetControlID="cboContratosAjax">
    </ajaxToolkit:CascadingDropDown>
    <ajaxToolkit:CascadingDropDown ID="CascadingDropDown3" runat="server" Category="Contratista"
        LoadingText="Cargando..." ParentControlID="cboContratosAjax" PromptText="Seleccione un Contratista o Sub Contratista"
        SelectedValue="IdContratoEmpresas" ServiceMethod="GetSubContratistas" ServicePath="ServiceResumen.asmx"
        TargetControlID="cboContratistasAjax">
    </ajaxToolkit:CascadingDropDown>
    <ajaxToolkit:CascadingDropDown ID="CascadingDropDown1" runat="server" Category="Empresas"
        LoadingText="Cargando" PromptText="Seleccione un Empresa" ServiceMethod="GetEmpresas"
        ServicePath="ServiceResumen.asmx" TargetControlID="cboEmpresasAjax">
    </ajaxToolkit:CascadingDropDown>
    <ajaxToolkit:CascadingDropDown ID="CascadingDropDown4" runat="server" Category="Periodos"
        LoadingText="Cargando..." ParentControlID="cboContratistasAjax" PromptText="Seleccione un Periodo"
        ServiceMethod="GetPeriodos" ServicePath="ServiceResumen.asmx" TargetControlID="cboPriodosAjax">
    </ajaxToolkit:CascadingDropDown>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<asp:UpdateProgress id="UpdateProgress1" runat="server"><ProgressTemplate>
<DIV class="progress"><IMG src="images/indicator.gif" />&nbsp;<asp:Label id="Label8" runat="server" Text="Generando Reporte..." Font-Bold="True" __designer:wfdid="w7"></asp:Label> </DIV>
</ProgressTemplate>
</asp:UpdateProgress> 
<TABLE style="BORDER-RIGHT: black thin solid; BORDER-TOP: black thin solid; BORDER-LEFT: black thin solid; BORDER-BOTTOM: black thin solid; BACKGROUND-COLOR: gainsboro" id="tblDetalleResumen" runat="server" visible="false"><TBODY><TR><TD style="WIDTH: 100px" align=right><asp:Label id="lblDescipcion" runat="server" ForeColor="Maroon" Width="106px" Text="Empresa:" Height="22px" Font-Bold="True"></asp:Label></TD><TD style="WIDTH: 100px" align=left><asp:Label id="lblEmpresa" runat="server" ForeColor="LightCoral" Width="106px" Height="22px" Font-Bold="True" __designer:wfdid="w4"></asp:Label></TD></TR><TR><TD style="WIDTH: 100px" align=right><asp:Label id="Label2" runat="server" ForeColor="Maroon" Width="106px" Text="Nro Contrato:" Height="24px" Font-Bold="True" __designer:wfdid="w1"></asp:Label></TD><TD style="WIDTH: 100px" align=left><asp:Label id="lblNroCont" runat="server" ForeColor="LightCoral" Width="106px" Height="22px" Font-Bold="True" __designer:wfdid="w4"></asp:Label></TD></TR><TR><TD style="WIDTH: 100px" align=right><asp:Label id="Label6" runat="server" ForeColor="Maroon" Width="106px" Text="Período:" Height="22px" Font-Bold="True" __designer:wfdid="w2"></asp:Label></TD><TD style="WIDTH: 100px" align=left><asp:Label id="lblPeriodo" runat="server" ForeColor="LightCoral" Width="106px" Height="22px" Font-Bold="True" __designer:wfdid="w4"></asp:Label></TD></TR><TR><TD style="WIDTH: 100px" align=right><asp:Label id="Label7" runat="server" ForeColor="Maroon" Width="106px" Text="Informe:" Height="24px" Font-Bold="True" __designer:wfdid="w2"></asp:Label></TD><TD style="WIDTH: 100px; BACKGROUND-COLOR: gainsboro" align=left>&nbsp;<IMG style="CURSOR: hand" id="imgReporte" onclick="AbrirDocumento()" height=35 src="images/AcrobatReader.png" width=32 name="imgReporte" runat="server" /></TD></TR></TBODY></TABLE>&nbsp;&nbsp;&nbsp; 

    <table runat="server" style="BORDER-RIGHT: #843431 thin solid; BORDER-TOP: #843431 thin solid; BORDER-LEFT: #843431 thin solid; WIDTH: 449px; BORDER-BOTTOM: #843431 thin solid; HEIGHT: 89px; BACKGROUND-COLOR: #f1dcdc" id="tblDetalleEnAuditoria">
    <tbody>
    <tr>
        <td align="center" valign="middle">
            <asp:Label id="Label10" runat="server" Width="320px" ForeColor="Maroon" 
                 Font-Bold="True" 
                
                Text="La hoja de ruta solicitada se encuentra en proceso de auditoria, consulte la misma en otro momento, Gracias."></asp:Label>
        </td>
    </tr>
    
    </tbody>
    </table>
    
    
</contenttemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="cboEmpresasAjax" EventName="SelectedIndexChanged" />
        <asp:AsyncPostBackTrigger ControlID="cboContratosAjax" EventName="SelectedIndexChanged" />
        <asp:AsyncPostBackTrigger ControlID="cboContratistasAjax" EventName="SelectedIndexChanged" />
        <asp:AsyncPostBackTrigger ControlID="cboPriodosAjax" EventName="SelectedIndexChanged" />
        <asp:AsyncPostBackTrigger ControlID="btnBusquedaPrincipal" EventName="Click" />
        
        
    </Triggers>
    </asp:UpdatePanel>
    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;
    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;<br />
    <br />
    &nbsp;
</asp:Content>

