<%@ Page Language="C#" Theme="MiTema" EnableEventValidation="false" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="AprobacionHojaSinLegajos.aspx.cs" Inherits="AprobacionHojaSinLegajos" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript" language="javascript">
</script>
    <ajaxToolkit:CascadingDropDown ID="CascadingDropDown4" runat="server" Category="Periodos"
        LoadingText="Cargando..." ParentControlID="cboContratistasAjax" PromptText="Seleccione un Periodo sin Legajos"
        ServiceMethod="GetPeriodosSinLegajos" ServicePath="ServiceResumen.asmx" TargetControlID="cboPriodosAjax">
    </ajaxToolkit:CascadingDropDown>
    <ajaxToolkit:CascadingDropDown ID="CascadingDropDown1" runat="server" Category="Empresas"
        LoadingText="Cargando" PromptText="Seleccione un Empresa" ServiceMethod="GetEmpresas"
        ServicePath="ServiceResumen.asmx" TargetControlID="cboEmpresasAjax" >
    </ajaxToolkit:CascadingDropDown>
    <ajaxToolkit:CascadingDropDown ID="CascadingDropDown3" runat="server" Category="Contratista"
        LoadingText="Cargando..." ParentControlID="cboContratosAjax" PromptText="Seleccione un Contratista o Sub Contratista"
        SelectedValue="IdContratoEmpresas" ServiceMethod="GetSubContratistas" ServicePath="ServiceResumen.asmx"
        TargetControlID="cboContratistasAjax">
    </ajaxToolkit:CascadingDropDown>
    <ajaxToolkit:CascadingDropDown ID="CascadingDropDown2" runat="server" Category="Contratos"
        LoadingText="Cargando..." ParentControlID="cboEmpresasAjax" PromptText="Seleccione un Contrato"
        ServiceMethod="GetContratos" ServicePath="ServiceResumen.asmx" TargetControlID="cboContratosAjax" BehaviorID="CascadeEmpresas">
    </ajaxToolkit:CascadingDropDown>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true">
    </ajaxToolkit:ToolkitScriptManager>
    <table cellpadding="0" cellspacing="5">
        <tr>
            <td align="center" style="width: 241px">
                <asp:Label ID="lblEncabezadoS" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Font-Bold="True" Font-Size="14pt" ForeColor="Maroon" Height="25px"
                    Style="background-image: url(images/FondoTitulos.gif); background-color: transparent"
                    Text="APROBACION HOJAS DE RUTAS SIN LEGAJOS" Width="476px"></asp:Label></td>
        </tr>
    </table>
    <table id="Table2" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
        border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #f1dcdc">
        <tr>
            <td align="right" style="width: 98px; height: 26px">
                <asp:Label ID="lblEmpresa" runat="server" Font-Bold="True" ForeColor="Maroon" Height="22px"
                    Text="Empresa:" Width="79px"></asp:Label></td>
            <td id="Td2" align="left" style="width: 95px; height: 26px">
                <asp:DropDownList ID="cboEmpresasAjax" runat="server" AutoPostBack="True"
                    Width="350px">
                </asp:DropDownList></td>
            <td align="right" style="width: 34px">
                <asp:Label ID="lblContr" runat="server" Font-Bold="True" ForeColor="Maroon" Height="22px"
                    Text="Contratos:" Width="85px"></asp:Label></td>
            <td align="left" style="width: 299px">
                &nbsp;<asp:DropDownList ID="cboContratosAjax" runat="server" AutoPostBack="True"
                    Height="23px" Width="236px">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td align="right" style="width: 98px; height: 26px">
                <asp:Label ID="lblContratistas" runat="server" Font-Bold="True" ForeColor="Maroon"
                    Height="20px" Text="Contratistas:" Width="85px"></asp:Label></td>
            <td align="left" style="width: 95px; height: 26px">
                <asp:DropDownList ID="cboContratistasAjax" runat="server" AutoPostBack="True"
                    Width="350px">
                </asp:DropDownList></td>
            <td align="right" style="width: 34px; height: 26px">
                <asp:Label ID="lblPer" runat="server" Font-Bold="True" ForeColor="Maroon" Height="19px"
                    Text="Periodos:" Width="85px"></asp:Label></td>
            <td align="left" style="width: 299px; height: 26px">
                &nbsp;<asp:DropDownList ID="cboPriodosAjax" runat="server" AutoPostBack="True"
                    Width="232px" OnSelectedIndexChanged="cboPriodosAjax_SelectedIndexChanged">
                </asp:DropDownList></td>
        </tr>
    </table>
    <br />
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: #843431 thin solid; BORDER-TOP: #843431 thin solid; BORDER-LEFT: #843431 thin solid; WIDTH: 316px; BORDER-BOTTOM: #843431 thin solid; BACKGROUND-COLOR: #f1dcdc" id="tblDescipcionContrato" runat="server"><TBODY><TR><TD align=left><asp:Label id="lblNroCarp" runat="server" ForeColor="Maroon" Text="Contratista:" Font-Bold="True"></asp:Label></TD><TD align=left><asp:Label id="lblContratista" runat="server" ForeColor="#404040" Font-Bold="False"></asp:Label></TD></TR><TR><TD style="WIDTH: 31px" align=left><asp:Label id="Label1" runat="server" ForeColor="Maroon" Text="Contrato:" Font-Bold="True"></asp:Label></TD><TD align=left><asp:Label id="lblContrato" runat="server" ForeColor="#404040" Font-Bold="False"></asp:Label></TD></TR><TR><TD style="WIDTH: 31px" align=left><asp:Label id="Label2" runat="server" ForeColor="Maroon" Text="Periodo:" Font-Bold="True"></asp:Label></TD><TD align=left><asp:Label id="lblPeriodo" runat="server" ForeColor="#404040" Font-Bold="False"></asp:Label></TD></TR><TR><TD style="WIDTH: 31px" align=left><asp:Label id="Label3" runat="server" ForeColor="Maroon" Width="127px" Text="Fecha Aprobacion:" Font-Bold="True"></asp:Label></TD><TD align=left><asp:Label id="lblFechaAprobacion" runat="server" ForeColor="#404040" Font-Bold="True"></asp:Label></TD></TR><TR><TD align=center colSpan=2><asp:ImageButton id="btnAprobar" onclick="btnAprobar_Click" runat="server" name="btnAprobar" ImageUrl="~/images/HojaAprobada4.gif"></asp:ImageButton></TD></TR></TBODY></TABLE>
<HR />
&nbsp;&nbsp; <TABLE style="WIDTH: 337px"><TBODY><TR><TD style="WIDTH: 405px" align=left><asp:Label style="TEXT-INDENT: 5pt" id="LabelCabeceraE" runat="server" ForeColor="Firebrick" Font-Size="13pt" BackColor="#E0E0E0" Width="209px" Text=" Aprobaciónes Anteriores" Height="19px" Font-Bold="True" BorderStyle="Solid" BorderWidth="1px" BorderColor="Black" Font-Underline="True" __designer:wfdid="w51"></asp:Label></TD></TR><TR><TD style="WIDTH: 405px"><DIV style="OVERFLOW: auto; WIDTH: 384px; HEIGHT: 224px"><asp:GridView id="gvAprobacionesAnteriores" runat="server" DataSourceID="ObjectDataSource1" __designer:wfdid="w73" EmptyDataText="No existen aprobaciones anteriores" AutoGenerateColumns="False" DataKeyNames="IdCabeceraHojasDeRuta"><Columns>
<asp:BoundField DataField="RazonSocial" HeaderText="Contratista"></asp:BoundField>
<asp:BoundField DataField="Codigo" HeaderText="Contrato"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:yyyy/MM}" DataField="Periodo" SortExpression="Periodo" HeaderText="Periodo"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:d}" DataField="FechaAprobacionSinLegajos" HeaderText="Fecha Aprobaci&#243;n"></asp:BoundField>
</Columns>
</asp:GridView></DIV></TD></TR></TBODY></TABLE> <asp:ObjectDataSource id="ObjectDataSource1" runat="server" __designer:wfdid="w1" InsertMethod="Insert" TypeName="DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter" DeleteMethod="Delete" SelectMethod="GetHojasAprobadasSinLegajos" OldValuesParameterFormatString="original_{0}" UpdateMethod="Update"><DeleteParameters>
<asp:Parameter Type="Int64" Name="Original_IdCabeceraHojasDeRuta"></asp:Parameter>
</DeleteParameters>
<UpdateParameters>
<asp:Parameter Type="Int64" Name="IdEstado"></asp:Parameter>
<asp:Parameter Type="DateTime" Name="Periodo"></asp:Parameter>
<asp:Parameter Type="Int64" Name="NroCarpeta"></asp:Parameter>
<asp:Parameter Type="Int64" Name="IdContratoEmpresa"></asp:Parameter>
<asp:Parameter Type="String" Name="Estimacion"></asp:Parameter>
<asp:Parameter Type="DateTime" Name="FechaEstimacion"></asp:Parameter>
<asp:Parameter Type="DateTime" Name="FechaAprobacion"></asp:Parameter>
<asp:Parameter Type="DateTime" Name="FechaAprobacionSinLegajos"></asp:Parameter>
<asp:Parameter Type="Int64" Name="Original_IdCabeceraHojasDeRuta"></asp:Parameter>
<asp:Parameter Type="Int32" Name="IdCabeceraHojasDeRuta"></asp:Parameter>
</UpdateParameters>
<InsertParameters>
<asp:Parameter Type="Int64" Name="IdEstado"></asp:Parameter>
<asp:Parameter Type="DateTime" Name="Periodo"></asp:Parameter>
<asp:Parameter Type="Int64" Name="NroCarpeta"></asp:Parameter>
<asp:Parameter Type="Int64" Name="IdContratoEmpresa"></asp:Parameter>
<asp:Parameter Type="String" Name="Estimacion"></asp:Parameter>
<asp:Parameter Type="DateTime" Name="FechaEstimacion"></asp:Parameter>
<asp:Parameter Type="DateTime" Name="FechaAprobacion"></asp:Parameter>
<asp:Parameter Type="DateTime" Name="FechaAprobacionSinLegajos"></asp:Parameter>
</InsertParameters>
</asp:ObjectDataSource>
</contenttemplate>
        <triggers>
<asp:AsyncPostBackTrigger ControlID="cboContratistasAjax" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="cboContratosAjax" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="cboEmpresasAjax" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="cboPriodosAjax" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
</triggers>
    </asp:UpdatePanel>
</asp:Content>

