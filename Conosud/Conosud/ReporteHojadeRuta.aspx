<%@ Page Language="C#" EnableEventValidation="false" Theme="MiTema"  MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="ReporteHojadeRuta.aspx.cs" Inherits="ReporteHojadeRuta" Title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ajaxToolkit:CascadingDropDown ID="CascadingDropDown1" runat="server" Category="Empresas"
        LoadingText="Cargando..." PromptText="Seleccione una Empresa" ServiceMethod="GetEmpresas"
        TargetControlID="cboEmpresas" ServicePath="ServiceResumen.asmx">
    </ajaxToolkit:CascadingDropDown>
    <ajaxToolkit:CascadingDropDown ID="CascadingDropDown2" runat="server" Category="Contratos"
        LoadingText="Cargando..." ParentControlID="cboEmpresas" PromptText="Seleccione un Contrato"
        ServiceMethod="GetContratos" ServicePath="ServiceResumen.asmx" TargetControlID="DDLContratos">
    </ajaxToolkit:CascadingDropDown>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true">
    </ajaxToolkit:ToolkitScriptManager>
    <table cellpadding="0" cellspacing="5">
        <tr>
            <td align="center" style="width: 241px">
                <asp:Label ID="lblEncabezadoS" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Font-Bold="True" Font-Size="14pt" ForeColor="Maroon" Height="25px"
                    Style="background-image: url(images/FondoTitulos.gif); background-color: transparent"
                    Text="REPORTE RESUMEN HOJA DE RUTA" Width="368px"></asp:Label></td>
        </tr>
    </table>
    <table style="background-color: #f1dcdc; width: 150px; border-right: #843431 thin solid; border-top: #843431 thin solid; border-left: #843431 thin solid; border-bottom: #843431 thin solid;" id="tblFiltro" cellspacing="5">
        <tr>
            <td style="width: 309px; height: 26px; text-align: left">
                <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Maroon" Text="Empresa:" Width="94px"></asp:Label></td>
            <td style="width: 309px; height: 26px; text-align: left">
                <asp:DropDownList
                    ID="cboEmpresas" runat="server" AutoPostBack="True" Width="208px" Height="22px" OnSelectedIndexChanged="cboEmpresas_SelectedIndexChanged">
                </asp:DropDownList></td>
            <td style="width: 309px; height: 26px; text-align: left">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Maroon" Text="Contratos:" Width="93px"></asp:Label></td>
            <td style="width: 336px; height: 26px; text-align: left">
                <asp:DropDownList
                    ID="DDLContratos" runat="server" Width="164px" AutoPostBack="True" Height="22px" OnSelectedIndexChanged="DDLContratos_SelectedIndexChanged">
                </asp:DropDownList></td>
        </tr>
    </table>
    <br />
    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: #843431 thin solid; BORDER-TOP: #843431 thin solid; BORDER-LEFT: #843431 thin solid; BORDER-BOTTOM: #843431 thin solid; BACKGROUND-COLOR: #f1dcdc" id="Table1"><TBODY><TR><TD style="HEIGHT: 26px" align=right colSpan=2><asp:Label id="lblEmpresa" runat="server" ForeColor="Maroon" Width="111px" Text="Periodo Inicial:" Font-Bold="True" Height="6px"></asp:Label>&nbsp; </TD><TD align=left colSpan=2><cc2:ValidatorCalloutExtender id="ValidatorCalloutExtender1" runat="server" TargetControlID="RegularExpressionValidator1" Width="280px" HighlightCssClass="validatorCalloutHighlight">
                </cc2:ValidatorCalloutExtender> <cc2:MaskedEditExtender id="MaskedEditExtender1" runat="server" TargetControlID="txtPeriodo" ClearMaskOnLostFocus="False" Mask="9999/99">
                </cc2:MaskedEditExtender> &nbsp;<asp:TextBox id="txtPeriodo" runat="server" Width="85px" MaxLength="7" BorderStyle="Solid"></asp:TextBox>&nbsp;&nbsp;<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ErrorMessage="El Formato ingresado es Incorrecto. <p><b>Formato: yyyy/MM (Año/Mes)</b>" ControlToValidate="txtPeriodo" ValidationExpression="\d{4}\/\d{2}" Display="None" SetFocusOnError="True">Ingrese fecha valida</asp:RegularExpressionValidator> <cc2:MaskedEditValidator id="MaskedEditValidator1" runat="server" ControlToValidate="txtPeriodo" SetFocusOnError="True" ControlExtender="MaskedEditExtender1" InvalidValueBlurredMessage="*" InvalidValueMessage="Formato Incorrecto"></cc2:MaskedEditValidator></TD><TD align=left colSpan=1><asp:Label id="Label15" runat="server" ForeColor="Maroon" Width="99px" Text="Periodo Final:" Font-Bold="True" Height="6px"></asp:Label></TD><TD align=left colSpan=1><cc2:ValidatorCalloutExtender id="ValidatorCalloutExtender2" runat="server" TargetControlID="RegularExpressionValidator2" Width="280px" HighlightCssClass="validatorCalloutHighlight">
                </cc2:ValidatorCalloutExtender> <cc2:MaskedEditExtender id="MaskedEditExtender2" runat="server" TargetControlID="txtPeriodoFinal" ClearMaskOnLostFocus="False" Mask="9999/99">
                </cc2:MaskedEditExtender> <asp:TextBox id="txtPeriodoFinal" runat="server" Width="85px" MaxLength="7" BorderStyle="Solid"></asp:TextBox><asp:RegularExpressionValidator id="RegularExpressionValidator2" runat="server" ErrorMessage="El Formato ingresado es Incorrecto. <p><b>Formato: yyyy/MM (Año/Mes)</b>" ControlToValidate="txtPeriodoFinal" ValidationExpression="\d{4}\/\d{2}" Display="None" SetFocusOnError="True">Ingrese fecha valida</asp:RegularExpressionValidator> <cc2:MaskedEditValidator id="MaskedEditValidator2" runat="server" ControlToValidate="txtPeriodoFinal" SetFocusOnError="True" ControlExtender="MaskedEditExtender2" InvalidValueBlurredMessage="*" InvalidValueMessage="Formato Incorrecto"></cc2:MaskedEditValidator></TD></TR><TR><TD style="HEIGHT: 26px" align=center colSpan=6><asp:ImageButton id="ImageButton1" onclick="ImageButton1_Click" runat="server" Width="24px" Height="20px" ImageUrl="~/images/search_16x16.gif"></asp:ImageButton><asp:ImageButton id="ibExcel" onclick="ibExcel_Click" runat="server" Width="24px" Height="20px" ImageUrl="~/images/Excel.GIF"></asp:ImageButton></TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
&nbsp; <TABLE style="WIDTH: 951px"><TBODY><TR><TD style="HEIGHT: 26px"><DIV style="BACKGROUND-IMAGE: none; OVERFLOW: auto; WIDTH: 936px; HEIGHT: 300px; BACKGROUND-COLOR: transparent" align=center><asp:GridView id="dgConsulta" runat="server" __designer:wfdid="w1" OnRowDataBound="dgConsulta_RowDataBound" AutoGenerateColumns="False" OnRowCreated="dgConsulta_RowCreated" OnDataBinding="dgConsulta_DataBinding" OnDataBound="dgConsulta_DataBound"><Columns>
<asp:BoundField DataField="IdCabeceraHojadeRuta" Visible="False" HeaderText="IdCabeceraHojadeRuta"></asp:BoundField>
<asp:BoundField DataField="Codigo" HeaderText="C&#243;digo"></asp:BoundField>
<asp:BoundField DataField="Contratistas" HeaderText="Contratista"></asp:BoundField>
<asp:BoundField DataField="Servicios" HeaderText="Servicio"></asp:BoundField>
</Columns>
</asp:GridView></DIV>&nbsp; </TD></TR><TR><TD></TD></TR></TBODY></TABLE>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
</contenttemplate>
     <Triggers>
<asp:AsyncPostBackTrigger ControlID="cboEmpresas" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="DDLContratos" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="txtPeriodo" EventName="TextChanged"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="txtPeriodoFinal" EventName="TextChanged"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="ImageButton1" EventName="Click"></asp:AsyncPostBackTrigger>
</Triggers>
    </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="50">
        <ProgressTemplate>
            <div  style="left: 415px; width: 216px; line-height: 0pt; position: absolute; top: 390px;
                background-color: white; z-index: 20; border-right: #3399ff thin solid; border-top: #3399ff thin solid; vertical-align: middle; border-left: #3399ff thin solid; border-bottom: #3399ff thin solid; text-align: center;">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 127px; height: 62px">
                    <tr>
                        <td align="center" style="height: 31px; border-top-width: thin; border-left-width: thin; border-left-color: #6699ff; border-bottom-width: thin; border-bottom-color: #6699ff; border-top-color: #6699ff; border-right-width: thin; border-right-color: #6699ff;">
                            <asp:Label ID="labelprogess" runat="server" Font-Bold="True" Font-Names="Georgia" Font-Size="10pt"
                                ForeColor="Black" Height="21px" Style="vertical-align: middle" Text="Procesando Consulta..."
                                Width="179px"></asp:Label>
                            <img alt=" " src="images/indicator.gif""  /></td>
                    </tr>
                </table>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress> 
</asp:Content>

