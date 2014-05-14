<%@ Page Language="C#" Theme="MiTema" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="ReporteSemaforoEstatico.aspx.cs" Inherits="ReporteSemaforoEstatico" Title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server" >
<script type="text/javascript" language="javascript">
    // Move an element directly on top of another element (and optionally
    // make it the same size)
    

    
    function Cover(bottom, top, ignoreSize) {
        var location = Sys.UI.DomElement.getLocation(bottom);
        top.style.position = 'absolute';
        top.style.top = location.y + 'px';
        top.style.left = location.x + 'px';
        if (!ignoreSize) {
            top.style.height = bottom.offsetHeight + 'px';
            top.style.width = bottom.offsetWidth + 'px';
        }
    }
    function NoSubmit()
    {
        if(event.which || event.keyCode)
        {
            if ((event.which == 13) || (event.keyCode == 13))
            {
                event.keyCode = 9;
            }
        }
    }
    function CargarDiv(pos)
    {
        document.getElementById("divDatosE").innerHTML = myArray[pos];
        var animBehavior = $find("dynamicAnimation"); 
        animBehavior.get_OnClickBehavior().play(); 
    }
</script>

    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" EnablePartialRendering="true" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <table cellpadding="0" cellspacing="5">
        <tr>
            <td align="center" style="width: 241px">
                <asp:Label ID="lblEncabezadoS" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Font-Bold="True" Font-Size="14pt" ForeColor="Maroon" Height="25px"
                    Style="background-image: url(images/FondoTitulos.gif); background-color: transparent"
                    Text="REPORTE SEMAFORO ESTATICO" Width="368px"></asp:Label></td>
        </tr>
    </table>
    <table id="Table1" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
        border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #f1dcdc">
        <tr>
            <td align="right" colspan="2">
                <asp:Label ID="lblEmpresa" runat="server" Font-Bold="True" ForeColor="Maroon" Height="6px"
                    Text="Periodo Inicial:" Width="111px"></asp:Label>&nbsp;
                    </td>
            <td align="left" colspan="2" >
                <cc2:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="RegularExpressionValidator1" HighlightCssClass="validatorCalloutHighlight" Width="280px">
                </cc2:ValidatorCalloutExtender>
                <cc2:MaskedEditExtender ID="MaskedEditExtender1" runat="server" ClearMaskOnLostFocus="False"
                    Mask="9999/99" TargetControlID="txtPeriodo">
                </cc2:MaskedEditExtender>
                &nbsp;<asp:TextBox ID="txtPeriodo" runat="server" Width="85px" MaxLength="7" BorderStyle="Solid"></asp:TextBox>&nbsp;&nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtPeriodo"
                    ErrorMessage="El Formato ingresado es Incorrecto. <p><b>Formato: yyyy/MM (Año/Mes)</b>" ValidationExpression="\d{4}\/\d{2}" Display="None" SetFocusOnError="True">Ingrese fecha valida</asp:RegularExpressionValidator>
                <cc2:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtender1"
                    ControlToValidate="txtPeriodo" InvalidValueBlurredMessage="*"
                    InvalidValueMessage="Formato Incorrecto" SetFocusOnError="True"></cc2:MaskedEditValidator></td>
            <td align="left" colspan="1">
                <asp:Label ID="Label15" runat="server" Font-Bold="True" ForeColor="Maroon" Height="6px"
                    Text="Periodo Final:" Width="99px"></asp:Label></td>
            <td align="left" colspan="1" >
                <cc2:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" TargetControlID="RegularExpressionValidator2" HighlightCssClass="validatorCalloutHighlight" Width="280px">
                </cc2:ValidatorCalloutExtender>
                <cc2:MaskedEditExtender ID="MaskedEditExtender2" runat="server" ClearMaskOnLostFocus="False"
                    Mask="9999/99" TargetControlID="txtPeriodoFinal">
                </cc2:MaskedEditExtender>
                <asp:TextBox ID="txtPeriodoFinal" runat="server" BorderStyle="Solid" MaxLength="7"
                    Width="85px"></asp:TextBox><asp:RegularExpressionValidator ID="RegularExpressionValidator2"
                        runat="server" ControlToValidate="txtPeriodoFinal" Display="None" ErrorMessage="El Formato ingresado es Incorrecto. <p><b>Formato: yyyy/MM (Año/Mes)</b>"
                        SetFocusOnError="True" ValidationExpression="\d{4}\/\d{2}">Ingrese fecha valida</asp:RegularExpressionValidator>
                <cc2:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlExtender="MaskedEditExtender2"
                    ControlToValidate="txtPeriodoFinal" InvalidValueBlurredMessage="*" InvalidValueMessage="Formato Incorrecto"
                    SetFocusOnError="True"></cc2:MaskedEditValidator></td>
        </tr>
        <tr>
            <td align="center" colspan="6">
                <asp:ImageButton ID="btnBuscar" runat="server" Height="23px" ImageUrl="~/images/Buscar6.gif"
                    name="btnBuscar" OnClick="btnBuscar_Click" Width="82px" /></td>
        </tr>
    </table>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<asp:UpdateProgress id="UpdateProgress1" runat="server" DisplayAfter="100"><ProgressTemplate>
<DIV class="progress"><IMG src="images/indicator.gif" /> <asp:Label id="Label8" runat="server" Width="226px" Text="Generando Datos Estadísticos..." Font-Bold="True"></asp:Label> </DIV>
</ProgressTemplate>
</asp:UpdateProgress> <ajaxToolkit:AnimationExtender id="AEDetalle" runat="server" TargetControlID="lblXContratos" __designer:wfdid="w24" BehaviorID="dynamicAnimation">
    <Animations>
            <OnClick>
                    <Sequence>
                        
                        
                        <ScriptAction Script="Cover($get('ctl00_ContentPlaceHolder1_lblXContratos'), $get('DivFly'));" />
                        
                        <StyleAction AnimationTarget="DivFly" Attribute="display" Value="block"/>
                        
                        
                        <Parallel AnimationTarget="DivFly" Duration=".3" Fps="25">
                            <Move Horizontal="100" Vertical="80" />
                            <Resize Width="320" Height="300" />
                            <Color PropertyKey="backgroundColor" StartValue="#AAAAAA" EndValue="#FFFFFF" />
                        </Parallel>
                        
                        
                        
                        <ScriptAction Script="Cover($get('DivFly'), $get('divDetalleE'), true);" />
                        <StyleAction AnimationTarget="divDetalleE" Attribute="display" Value="block"/>
                        <FadeIn AnimationTarget="divDetalleE" Duration=".2"/>
                        <StyleAction AnimationTarget="DivFly" Attribute="display" Value="none"/>
                        
                      


                    </Sequence>
                </OnClick></Animations>
</ajaxToolkit:AnimationExtender> <ajaxToolkit:AnimationExtender id="CloseAnimation" runat="server" TargetControlID="btnCerrar" __designer:wfdid="w27"><Animations>
                <OnClick>
                    <Sequence AnimationTarget="divDetalleE">
                        
                        <StyleAction Attribute="overflow" Value="hidden"/>
                        <Parallel Duration=".3" Fps="15">
                            <Scale ScaleFactor="0.05" Center="true" ScaleFont="true" FontUnit="px" />
                            <FadeOut />
                        </Parallel>
                        
                        
                        <StyleAction Attribute="display" Value="none"/>
                        <StyleAction Attribute="width" Value="345px"/>
                        <StyleAction Attribute="height" Value="300"/>
                        <StyleAction Attribute="fontSize" Value="13px"/>

                    </Sequence>
                </OnClick>
</Animations>
</ajaxToolkit:AnimationExtender> <TABLE style="WIDTH: 652px"><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 22px" align=left><TABLE style="BORDER-RIGHT: black thin solid; BORDER-TOP: black thin solid; BORDER-LEFT: black thin solid; WIDTH: 382px; BORDER-BOTTOM: black thin solid; HEIGHT: 29px; BACKGROUND-COLOR: gainsboro" id="Table2"><TBODY><TR><TD align=left colSpan=4><asp:Label id="lblXContratos" runat="server" ForeColor="Firebrick" Font-Size="13pt" Width="361px" Text="ESTADISTICA SEMAFORO POR CONTRATOS" Height="6px" Font-Bold="True" Font-Underline="True"></asp:Label></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE><DIV style="BORDER-RIGHT: #d0d0d0 1px solid; BORDER-TOP: #d0d0d0 1px solid; DISPLAY: none; Z-INDEX: 2; LEFT: 612px; OVERFLOW: hidden; BORDER-LEFT: #d0d0d0 1px solid; WIDTH: 395px; BORDER-BOTTOM: #d0d0d0 1px solid; POSITION: absolute; TOP: 415px; HEIGHT: 218px; BACKGROUND-COLOR: aqua" id="DivFly"></DIV><DIV style="OVERFLOW: auto; WIDTH: 656px; HEIGHT: 290px; TEXT-ALIGN: left"><asp:GridView id="gvDatosPorContratos" runat="server" Width="289px" Height="120px"></asp:GridView>&nbsp; </DIV><TABLE style="WIDTH: 654px"><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 18px" align=left><TABLE style="BORDER-RIGHT: black thin solid; BORDER-TOP: black thin solid; BORDER-LEFT: black thin solid; WIDTH: 316px; BORDER-BOTTOM: black thin solid; HEIGHT: 29px; BACKGROUND-COLOR: gainsboro" id="Table3"><TBODY><TR><TD align=left colSpan=4><asp:Label id="Label2" runat="server" ForeColor="Firebrick" Font-Size="13pt" Width="361px" Text="ESTADISTICA SEMAFORO POR LEGAJOS" Height="6px" Font-Bold="True" Font-Underline="True"></asp:Label></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE><DIV style="OVERFLOW: auto; WIDTH: 656px; HEIGHT: 256px; TEXT-ALIGN: left"><asp:GridView id="gvDatosPorLegajos" runat="server" Width="289px" Height="120px"></asp:GridView></DIV><DIV style="DISPLAY: none; FONT-SIZE: 13px; LEFT: 62px; WIDTH: 341px; POSITION: absolute; TOP: 402px; HEIGHT: 297px; BACKGROUND-COLOR: #b5494a" id="divDetalleE"><TABLE style="WIDTH: 280px; HEIGHT: 294px"><TBODY><TR><TD align=center><asp:Label id="Label1" runat="server" ForeColor="#FFE0C0" Font-Size="15pt" Width="244px" Text="Detalle Valor Estadistico" Font-Bold="True" __designer:wfdid="w25"></asp:Label></TD></TR><TR><TD vAlign=top><DIV style="OVERFLOW: auto; WIDTH: 340px; HEIGHT: 214px" id="divDatosE"></DIV></TD></TR><TR><TD align=center><asp:Button id="btnCerrar" runat="server" Width="84px" Text="Cerrar" __designer:wfdid="w26" OnClientClick="return false;"></asp:Button></TD></TR></TBODY></TABLE></DIV><BR /><BR />
</contenttemplate>
        <triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="txtPeriodo" EventName="Load"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="txtPeriodoFinal" EventName="TextChanged"></asp:AsyncPostBackTrigger>
</triggers>
    </asp:UpdatePanel>

</asp:Content>

