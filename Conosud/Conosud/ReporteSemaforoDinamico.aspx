<%@ Page Language="C#" Theme="MiTema" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="ReporteSemaforoDinamico.aspx.cs" Inherits="ReporteSemaforoDinamico" Title="Untitled Page" %>
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
                //event.returnValue=false; 
                //event.cancel = true;
            }
        }
    }
    
    function CargarDiv(pos)
    {
        alert(pos);
        document.getElementById("divDatosE").innerHTML = myArrayE[pos];
        var animBehavior = $find("dynamicAnimationE"); 
        animBehavior.get_OnClickBehavior().play(); 
    }
    
    
    function CargarDivC(pos)
    {
        document.getElementById("divDatosC").innerHTML = myArrayC[pos];
        var animBehavior = $find("dynamicAnimationC"); 
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
                    Text="REPORTE SEMAFORO DINAMICO" Width="368px"></asp:Label></td>
        </tr>
    </table>
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
    <asp:Panel ID="Panel1" runat="server" Height="50px" Width="125px">
    <table id="Table1" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
        border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #f1dcdc">
        <tr>
            <td align="right" colspan="2" style="height: 26px" valign="top">
                <asp:Label ID="lblEmpresa" runat="server" Font-Bold="True" ForeColor="Maroon" Height="6px"
                    Text="Periodo a Consultar:" Width="157px"></asp:Label>&nbsp;
                    </td>
            <td align="left" colspan="2" valign="top">
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
        </tr>
        <tr>
            <td align="center" colspan="4" style="height: 26px">
                <asp:ImageButton ID="btnBuscar" runat="server" Height="23px" ImageUrl="~/images/Buscar6.gif"
                    name="btnBuscar" OnClick="btnBuscar_Click" Width="82px" /></td>
        </tr>
    </table>
    </asp:Panel>
    <br />
    <cc2:DropShadowExtender ID="DropShadowExtender1" runat="server" Opacity="55" TargetControlID="Panel1"
        TrackPosition="True">
    </cc2:DropShadowExtender>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<asp:UpdateProgress id="UpdateProgress1" runat="server" __designer:wfdid="w48" DisplayAfter="100"><ProgressTemplate>
<DIV class="progress"><IMG src="images/indicator.gif" /> <asp:Label id="Label8" runat="server" Width="226px" Text="Generando Datos Estadísticos..." Font-Bold="True" __designer:wfdid="w49"></asp:Label> </DIV>
</ProgressTemplate>
</asp:UpdateProgress> 

<cc2:AnimationExtender id="AnimationExtender1" runat="server" TargetControlID="Label9" BehaviorID="dynamicAnimationE"><Animations>
                <OnClick>
                    <Sequence>
                        
               
                       
                        
                        <ScriptAction Script="Cover($get('ctl00_ContentPlaceHolder1_LabelCabeceraE'), $get('DivFly'));" />
                        
                        <StyleAction AnimationTarget="DivFly" Attribute="display" Value="block"/>
                                                                     
                        <Parallel AnimationTarget="DivFly" Duration=".3" Fps="25">
                            <Move Horizontal="55" Vertical="25" />
                            <Resize Width="350" Height="300" />
                            <Color PropertyKey="backgroundColor" StartValue="#AAAAAA" EndValue="#FFFFFF" />
                        </Parallel>
                        
                        
                        <ScriptAction Script="Cover($get('DivFly'), $get('divDetalleE'), true);" />
                        <StyleAction AnimationTarget="divDetalleE" Attribute="display" Value="block"/>
                        <FadeIn AnimationTarget="divDetalleE" Duration=".2"/>
                        
                        <StyleAction AnimationTarget="DivFly" Attribute="display" Value="none"/>
                        
                        
                        
                        <Parallel AnimationTarget="divDetalleE" Duration=".5">
                            <Color PropertyKey="color" StartValue="#666666" EndValue="#FF0000" />
                            <Color PropertyKey="borderColor" StartValue="#666666" EndValue="#FF0000" />
                        </Parallel>


                    </Sequence>
                </OnClick></Animations>
</cc2:AnimationExtender> <cc2:AnimationExtender id="AnimationExtender2" runat="server" TargetControlID="Label5" BehaviorID="dynamicAnimationC"><Animations>
                    <OnClick>
                        <Sequence>

                        <ScriptAction Script="Cover($get('ctl00_ContentPlaceHolder1_LabelCabeceraC'), $get('DivFly'));" />
                        
                        <StyleAction AnimationTarget="DivFly" Attribute="display" Value="block"/>
                                                                     
                        <Parallel AnimationTarget="DivFly" Duration=".3" Fps="25">
                            <Move Horizontal="55" Vertical="25" />
                            <Resize Width="350" Height="300" />
                            <Color PropertyKey="backgroundColor" StartValue="#AAAAAA" EndValue="#FFFFFF" />
                        </Parallel>
                        
                        
                        <ScriptAction Script="Cover($get('DivFly'), $get('divDetalleC'), true);" />
                        <StyleAction AnimationTarget="divDetalleC" Attribute="display" Value="block"/>
                        <FadeIn AnimationTarget="divDetalleC" Duration=".2"/>
                        
                        <StyleAction AnimationTarget="DivFly" Attribute="display" Value="none"/>
                        
                        
                        
                        <Parallel AnimationTarget="divDetalleC" Duration=".5">
                            <Color PropertyKey="color" StartValue="#666666" EndValue="#FF0000" />
                            <Color PropertyKey="borderColor" StartValue="#666666" EndValue="#FF0000" />
                        </Parallel>


                    </Sequence>
                </OnClick></Animations>
</cc2:AnimationExtender> <ajaxToolkit:AnimationExtender id="CloseAnimation" runat="server" TargetControlID="btnCerrar" __designer:wfdid="w5"><Animations>
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
</ajaxToolkit:AnimationExtender> <ajaxToolkit:AnimationExtender id="AnimationExtender3" runat="server" TargetControlID="btnCerrarC"><Animations>
                <OnClick>
                    <Sequence AnimationTarget="divDetalleC">
                        
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
</ajaxToolkit:AnimationExtender> <TABLE style="WIDTH: 610px"><TBODY><TR><TD align=left>&nbsp;<asp:Label style="TEXT-INDENT: 5pt" id="LabelCabeceraE" runat="server" ForeColor="Firebrick" Font-Size="13pt" BackColor="#E0E0E0" Width="347px" Height="19px" Text="  ESTADISTICA SEMAFORO POR EMPRESA" Font-Bold="True" BorderStyle="Solid" __designer:wfdid="w51" Font-Underline="True" BorderColor="Black" BorderWidth="1px"></asp:Label></TD></TR><TR><TD align=left><asp:Panel id="Panel2" runat="server" Width="385px" Height="19px" __designer:wfdid="w12" HorizontalAlign="Left"><TABLE style="BORDER-RIGHT: black thin solid; BORDER-TOP: black thin solid; BORDER-LEFT: black thin solid; BORDER-BOTTOM: black thin solid; BACKGROUND-COLOR: gainsboro" id="Table4"><TBODY><TR><TD style="WIDTH: 171px" align=right colSpan=2><asp:Label style="BORDER-LEFT-WIDTH: thin; BORDER-LEFT-COLOR: black; BORDER-BOTTOM-WIDTH: thin; BORDER-BOTTOM-COLOR: black" id="Label13" runat="server" ForeColor="Maroon" Font-Size="10pt" Width="153px" Text="Total Empresas:" Height="1px" Font-Bold="True" __designer:wfdid="w52"></asp:Label></TD><TD style="WIDTH: 83px" align=left colSpan=1><asp:LinkButton style="LEFT: 517px; TOP: 525px" id="lblTotalEmpresas" onclick="lblTotalEmpresas_Click" runat="server" Font-Size="11pt" Width="1px" __designer:wfdid="w53" OnClientClick="CargarDiv(0);return false;">0</asp:LinkButton> </TD><TD align=right colSpan=1></TD></TR><TR><TD style="BORDER-TOP-WIDTH: thin; BORDER-LEFT-WIDTH: thin; BORDER-LEFT-COLOR: gray; BORDER-BOTTOM-WIDTH: thin; BORDER-BOTTOM-COLOR: gray; WIDTH: 171px; BORDER-TOP-COLOR: gray; HEIGHT: 28px; BORDER-RIGHT-WIDTH: thin; BORDER-RIGHT-COLOR: gray" align=right colSpan=2></TD><TD align=right colSpan=1><asp:Label id="Label17" runat="server" ForeColor="Maroon" Font-Size="10pt" Width="152px" Text="Empresas Aprobadas:" Height="1px" Font-Bold="True" __designer:wfdid="w54"></asp:Label></TD><TD style="HEIGHT: 28px" align=left colSpan=1><asp:LinkButton style="LEFT: 517px; TOP: 525px" id="lblTotalAprobadasE" onclick="lblTotalEmpresas_Click" runat="server" Font-Size="11pt" Width="1px" Height="8px" __designer:wfdid="w55" OnClientClick="CargarDiv(1);return false;">0</asp:LinkButton> </TD></TR><TR><TD style="WIDTH: 171px" align=right colSpan=2></TD><TD align=right colSpan=1><asp:Label id="Label21" runat="server" ForeColor="Maroon" Font-Size="10pt" Width="176px" Text="Empresas No Aprobadas:" Height="1px" Font-Bold="True" __designer:wfdid="w56"></asp:Label></TD><TD align=left colSpan=1><asp:LinkButton style="LEFT: 517px; TOP: 525px" id="lblTotalNoAprobadasE" onclick="lblTotalEmpresas_Click" runat="server" Font-Size="11pt" Width="1px" Height="12px" __designer:wfdid="w1" OnClientClick="CargarDiv(2);return false;">0</asp:LinkButton></TD></TR><TR><TD style="WIDTH: 171px" align=right colSpan=2></TD><TD style="WIDTH: 83px" align=right colSpan=1><asp:Label id="Label29" runat="server" ForeColor="Maroon" Font-Size="10pt" Width="191px" Text="Empresas Mixtas:" Height="15px" Font-Bold="True" __designer:wfdid="w58"></asp:Label></TD><TD align=left colSpan=1><asp:LinkButton style="LEFT: 517px; TOP: 525px" id="lblTotalMixtas" onclick="lblTotalEmpresas_Click" runat="server" Font-Size="11pt" Width="1px" Height="13px" __designer:wfdid="w2" OnClientClick="CargarDiv(3);return false;">0</asp:LinkButton></TD></TR></TBODY></TABLE></asp:Panel></TD></TR></TBODY></TABLE><cc2:DropShadowExtender id="DropShadowExtender2" runat="server" TrackPosition="True" TargetControlID="Panel2" Opacity="55" __designer:wfdid="w13"></cc2:DropShadowExtender> <DIV style="BORDER-RIGHT: #d0d0d0 1px solid; BORDER-TOP: #d0d0d0 1px solid; DISPLAY: none; Z-INDEX: 10; LEFT: 843px; OVERFLOW: hidden; BORDER-LEFT: #d0d0d0 1px solid; WIDTH: 100px; BORDER-BOTTOM: #d0d0d0 1px solid; POSITION: absolute; TOP: 798px; HEIGHT: 20px; BACKGROUND-COLOR: aqua" id="DivFly"></DIV><TABLE><TBODY><TR><TD align=left><asp:Label style="TEXT-INDENT: 5pt" id="LabelCabeceraC" runat="server" ForeColor="Firebrick" Font-Size="13pt" BackColor="#E0E0E0" Width="369px" Height="6px" Text="ESTADISTICA SEMAFORO POR CONTRATOS" Font-Bold="True" BorderStyle="Solid" __designer:wfdid="w60" Font-Underline="True" BorderColor="Black" BorderWidth="1px"></asp:Label></TD></TR><TR><TD style="WIDTH: 100px"><asp:Panel id="Panel3" runat="server" Width="125px" Height="50px" __designer:wfdid="w15" HorizontalAlign="Left"><TABLE style="BORDER-RIGHT: black thin solid; BORDER-TOP: black thin solid; BORDER-LEFT: black thin solid; BORDER-BOTTOM: black thin solid; BACKGROUND-COLOR: gainsboro" id="Table2"><TBODY><TR><TD align=right colSpan=2><asp:Label id="Label1" runat="server" ForeColor="Maroon" Width="113px" Text="Total Contratos:" Height="6px" Font-Bold="True" __designer:wfdid="w61"></asp:Label></TD><TD align=left colSpan=1><asp:Label id="lblTotalContratos" runat="server" ForeColor="DodgerBlue" Width="67px" Height="6px" Font-Bold="True" __designer:wfdid="w62">0</asp:Label></TD><TD align=right colSpan=1></TD><TD align=right colSpan=1></TD><TD align=left colSpan=1></TD></TR><TR><TD style="BORDER-TOP-WIDTH: thin; BORDER-LEFT-WIDTH: thin; BORDER-LEFT-COLOR: gray; BORDER-BOTTOM-WIDTH: thin; BORDER-BOTTOM-COLOR: gray; WIDTH: 159px; BORDER-TOP-COLOR: gray; HEIGHT: 26px; BORDER-RIGHT-WIDTH: thin; BORDER-RIGHT-COLOR: gray" align=right colSpan=2></TD><TD align=right colSpan=1><asp:Label id="label122" runat="server" ForeColor="Maroon" Width="109px" Text="Total Hojas:" Height="6px" Font-Bold="True" __designer:wfdid="w63"></asp:Label></TD><TD align=left colSpan=1><asp:LinkButton style="LEFT: 517px; TOP: 525px" id="lblTotalHojas" onclick="lblTotalEmpresas_Click" runat="server" Width="1px" __designer:wfdid="w1" OnClientClick="CargarDivC(0);return false;">0</asp:LinkButton></TD><TD align=right colSpan=1></TD><TD align=left colSpan=1></TD></TR><TR><TD align=right colSpan=2></TD><TD align=right colSpan=1></TD><TD align=right colSpan=1><asp:Label id="Label2" runat="server" ForeColor="Maroon" Width="170px" Text="Total Hojas Aprobadas:" Height="12px" Font-Bold="True" __designer:wfdid="w65"></asp:Label></TD><TD align=left colSpan=1><asp:LinkButton style="LEFT: 517px; TOP: 525px" id="lblAprobadas" onclick="lblTotalEmpresas_Click" runat="server" Width="1px" __designer:wfdid="w2" OnClientClick="CargarDivC(1);return false;">0</asp:LinkButton></TD><TD align=left colSpan=1></TD></TR><TR><TD align=right colSpan=2></TD><TD align=right colSpan=1></TD><TD align=right colSpan=1></TD><TD align=right colSpan=1><asp:Label id="Label5" runat="server" ForeColor="Maroon" Width="134px" Text="Hojas En Termino:" Height="27px" Font-Bold="True" __designer:wfdid="w67"></asp:Label></TD><TD align=left colSpan=1><asp:LinkButton style="LEFT: 517px; TOP: 525px" id="lblEnTermino" onclick="lblTotalEmpresas_Click" runat="server" Width="1px" __designer:wfdid="w4" OnClientClick="CargarDivC(3);return false;">0</asp:LinkButton></TD></TR><TR><TD align=right colSpan=2></TD><TD align=right colSpan=1></TD><TD align=right colSpan=1></TD><TD align=right colSpan=1><asp:Label id="Label6" runat="server" ForeColor="Maroon" Width="148px" Text="Hojas Fuera Termino:" Height="27px" Font-Bold="True" __designer:wfdid="w69"></asp:Label></TD><TD align=left colSpan=1><asp:LinkButton style="LEFT: 517px; TOP: 525px" id="lblFueraTermino" onclick="lblTotalEmpresas_Click" runat="server" Width="1px" __designer:wfdid="w5" OnClientClick="CargarDivC(4);return false;">0</asp:LinkButton></TD></TR><TR><TD align=right colSpan=2></TD><TD align=right colSpan=1></TD><TD align=right colSpan=1><asp:Label id="label111" runat="server" ForeColor="Maroon" Width="191px" Text="Total Hojas NO Aprobadas:" Height="27px" Font-Bold="True" __designer:wfdid="w71"></asp:Label></TD><TD align=left colSpan=1><asp:LinkButton style="LEFT: 517px; TOP: 525px" id="lblNoAprobadas" onclick="lblTotalEmpresas_Click" runat="server" Width="1px" __designer:wfdid="w3" OnClientClick="CargarDivC(2);return false;">0</asp:LinkButton></TD><TD align=left colSpan=1></TD></TR><TR><TD align=right colSpan=2></TD><TD align=right colSpan=1></TD><TD align=right colSpan=1></TD><TD align=right colSpan=1><asp:Label id="Label7" runat="server" ForeColor="Maroon" Width="138px" Text="Con Presentaciones:" Height="27px" Font-Bold="True" __designer:wfdid="w73"></asp:Label></TD><TD align=left colSpan=1><asp:LinkButton style="LEFT: 517px; TOP: 525px" id="lblConPresentaciones" onclick="lblTotalEmpresas_Click" runat="server" Width="1px" __designer:wfdid="w6" OnClientClick="CargarDivC(5);return false;">0</asp:LinkButton></TD></TR><TR><TD align=right colSpan=2></TD><TD align=right colSpan=1></TD><TD align=right colSpan=1></TD><TD align=right colSpan=1><asp:Label id="Label9" runat="server" ForeColor="Maroon" Width="132px" Text="Sin Presentaciones:" Height="27px" Font-Bold="True" __designer:wfdid="w75"></asp:Label></TD><TD align=left colSpan=1><asp:LinkButton style="LEFT: 517px; TOP: 525px" id="lblSinPresentaciones" onclick="lblTotalEmpresas_Click" runat="server" Width="9px" __designer:wfdid="w7" OnClientClick="CargarDivC(6);return false;">0</asp:LinkButton></TD></TR></TBODY></TABLE></asp:Panel> <cc2:DropShadowExtender id="DropShadowExtender3" runat="server" TrackPosition="True" TargetControlID="Panel3" Opacity="55" __designer:wfdid="w17"></cc2:DropShadowExtender></TD></TR></TBODY></TABLE><TABLE><TBODY><TR><TD align=left><asp:Label style="TEXT-INDENT: 5pt" id="Label4" runat="server" ForeColor="Firebrick" Font-Size="13pt" BackColor="#E0E0E0" Width="361px" Height="6px" Text="ESTADISTICA SEMAFORO POR LEGAJOS" Font-Bold="True" BorderStyle="Solid" __designer:wfdid="w77" Font-Underline="True" BorderColor="#404040" BorderWidth="1px"></asp:Label></TD></TR><TR><TD style="WIDTH: 100px"><asp:Panel id="Panel4" runat="server" Width="125px" Height="50px" __designer:wfdid="w2"><TABLE style="BORDER-RIGHT: black thin solid; BORDER-TOP: black thin solid; BORDER-LEFT: black thin solid; BORDER-BOTTOM: black thin solid; BACKGROUND-COLOR: gainsboro" id="Table3"><TBODY><TR><TD style="WIDTH: 97px" align=right colSpan=2><asp:Label id="Label10" runat="server" ForeColor="Maroon" Width="113px" Height="6px" Text="Total Contratos:" Font-Bold="True" __designer:wfdid="w78"></asp:Label></TD><TD align=left colSpan=1><asp:Label id="lblTotalContratosL" runat="server" ForeColor="DodgerBlue" Width="67px" Height="6px" Font-Bold="True" __designer:wfdid="w79">0</asp:Label></TD><TD align=right colSpan=1></TD><TD align=right colSpan=1></TD><TD style="WIDTH: 36px" align=left colSpan=1></TD></TR><TR><TD style="BORDER-TOP-WIDTH: thin; BORDER-LEFT-WIDTH: thin; BORDER-LEFT-COLOR: gray; BORDER-BOTTOM-WIDTH: thin; BORDER-BOTTOM-COLOR: gray; WIDTH: 97px; BORDER-TOP-COLOR: gray; HEIGHT: 26px; BORDER-RIGHT-WIDTH: thin; BORDER-RIGHT-COLOR: gray" align=right colSpan=2></TD><TD align=right colSpan=1><asp:Label id="Label12" runat="server" ForeColor="Maroon" Width="109px" Height="6px" Text="Total Hojas:" Font-Bold="True" __designer:wfdid="w80"></asp:Label></TD><TD align=left colSpan=1><asp:Label id="lblTotalHojasL" runat="server" ForeColor="DodgerBlue" Width="25px" Height="6px" Font-Bold="True" __designer:wfdid="w81">0</asp:Label></TD><TD align=right colSpan=1></TD><TD style="WIDTH: 36px" align=left colSpan=1></TD></TR><TR><TD style="WIDTH: 97px" align=right colSpan=2></TD><TD align=right colSpan=1></TD><TD align=right colSpan=1><asp:Label id="Label14" runat="server" ForeColor="Maroon" Width="170px" Height="12px" Text="Total Hojas Aprobadas:" Font-Bold="True" __designer:wfdid="w82"></asp:Label></TD><TD align=left colSpan=1><asp:Label id="lblAprobadasL" runat="server" ForeColor="DodgerBlue" Width="57px" Height="6px" Font-Bold="True" __designer:wfdid="w83">0</asp:Label></TD><TD style="WIDTH: 36px" align=left colSpan=1></TD></TR><TR><TD style="WIDTH: 97px" align=right colSpan=2></TD><TD align=right colSpan=1></TD><TD align=right colSpan=1></TD><TD align=right colSpan=1><asp:Label id="Label16" runat="server" ForeColor="Maroon" Width="134px" Height="27px" Text="Hojas En Termino:" Font-Bold="True" __designer:wfdid="w84"></asp:Label></TD><TD style="WIDTH: 36px" align=left colSpan=1><asp:Label id="lblEnTerminoL" runat="server" ForeColor="DodgerBlue" Width="13px" Height="6px" Font-Bold="True" __designer:wfdid="w85">0</asp:Label></TD></TR><TR><TD style="WIDTH: 97px" align=right colSpan=2></TD><TD align=right colSpan=1></TD><TD align=right colSpan=1></TD><TD align=right colSpan=1><asp:Label id="Label18" runat="server" ForeColor="Maroon" Width="148px" Height="27px" Text="Hojas Fuera Termino:" Font-Bold="True" __designer:wfdid="w86"></asp:Label></TD><TD style="WIDTH: 36px" align=left colSpan=1><asp:Label id="lblFueraTerminoL" runat="server" ForeColor="DodgerBlue" Width="17px" Height="6px" Font-Bold="True" __designer:wfdid="w87">0</asp:Label></TD></TR><TR><TD style="WIDTH: 97px" align=right colSpan=2></TD><TD align=right colSpan=1></TD><TD align=right colSpan=1><asp:Label id="Label20" runat="server" ForeColor="Maroon" Width="191px" Height="27px" Text="Total Hojas NO Aprobadas:" Font-Bold="True" __designer:wfdid="w88"></asp:Label></TD><TD align=left colSpan=1><asp:Label id="lblNoAprobadasL" runat="server" ForeColor="DodgerBlue" Width="47px" Height="6px" Font-Bold="True" __designer:wfdid="w89">0</asp:Label></TD><TD style="WIDTH: 36px" align=left colSpan=1></TD></TR><TR><TD style="WIDTH: 97px" align=right colSpan=2></TD><TD align=right colSpan=1></TD><TD align=right colSpan=1></TD><TD align=right colSpan=1><asp:Label id="Label22" runat="server" ForeColor="Maroon" Width="138px" Height="27px" Text="Con Presentaciones:" Font-Bold="True" __designer:wfdid="w90"></asp:Label></TD><TD style="WIDTH: 36px" align=left colSpan=1><asp:Label id="lblConPresentacionesL" runat="server" ForeColor="DodgerBlue" Width="9px" Height="6px" Font-Bold="True" __designer:wfdid="w91">0</asp:Label></TD></TR><TR><TD style="WIDTH: 97px" align=right colSpan=2></TD><TD align=right colSpan=1></TD><TD align=right colSpan=1></TD><TD align=right colSpan=1><asp:Label id="Label24" runat="server" ForeColor="Maroon" Width="132px" Height="27px" Text="Sin Presentaciones:" Font-Bold="True" __designer:wfdid="w92"></asp:Label></TD><TD style="WIDTH: 36px" align=left colSpan=1><asp:Label id="lblSinPresentacionesL" runat="server" ForeColor="DodgerBlue" Width="15px" Height="6px" Font-Bold="True" __designer:wfdid="w93">0</asp:Label></TD></TR></TBODY></TABLE></asp:Panel> <cc2:DropShadowExtender id="DropShadowExtender4" runat="server" TrackPosition="True" TargetControlID="Panel4" __designer:wfdid="w1" Radius="55"></cc2:DropShadowExtender></TD></TR></TBODY></TABLE>&nbsp;<BR />&nbsp;<BR /><DIV style="DISPLAY: none; FONT-SIZE: 13px; Z-INDEX: 2; LEFT: 31px; WIDTH: 341px; POSITION: absolute; TOP: 455px; HEIGHT: 297px; BACKGROUND-COLOR: #b5494a" id="divDetalleE" __designer:dtid="1407374883553282"><TABLE style="WIDTH: 280px; HEIGHT: 294px" __designer:dtid="1407374883553283"><TBODY><TR __designer:dtid="1407374883553284"><TD align=center __designer:dtid="1407374883553285"><asp:Label id="LabelA" runat="server" ForeColor="#FFE0C0" Font-Size="15pt" __designer:dtid="1407374883553286" Width="244px" Text="Detalle Valor Estadistico" Font-Bold="True" __designer:wfdid="w6"></asp:Label></TD></TR><TR __designer:dtid="1407374883553287"><TD vAlign=top __designer:dtid="1407374883553288"><DIV style="OVERFLOW: auto; WIDTH: 340px; HEIGHT: 214px" id="divDatosE" __designer:dtid="1407374883553289"></DIV></TD></TR><TR __designer:dtid="1407374883553290"><TD align=center __designer:dtid="1407374883553291"><asp:Button id="btnCerrar" runat="server" __designer:dtid="1407374883553292" Width="84px" Text="Cerrar" __designer:wfdid="w7" OnClientClick="return false;"></asp:Button></TD></TR></TBODY></TABLE></DIV><DIV style="DISPLAY: none; FONT-SIZE: 13px; Z-INDEX: 10; LEFT: 642px; WIDTH: 341px; POSITION: absolute; TOP: 457px; HEIGHT: 297px; BACKGROUND-COLOR: #b5494a" id="divDetalleC" __designer:dtid="1407374883553282"><TABLE style="WIDTH: 280px; HEIGHT: 294px" __designer:dtid="1407374883553283"><TBODY><TR __designer:dtid="1407374883553284"><TD align=center __designer:dtid="1407374883553285"><asp:Label id="Label3" runat="server" ForeColor="#FFE0C0" Font-Size="15pt" __designer:dtid="1407374883553286" Width="244px" Text="Detalle Valor Estadistico" Font-Bold="True" __designer:wfdid="w8"></asp:Label></TD></TR><TR __designer:dtid="1407374883553287"><TD vAlign=top __designer:dtid="1407374883553288"><DIV style="OVERFLOW: auto; WIDTH: 340px; HEIGHT: 214px" id="divDatosC" __designer:dtid="1407374883553289"></DIV></TD></TR><TR __designer:dtid="1407374883553290"><TD align=center __designer:dtid="1407374883553291"><asp:Button id="btnCerrarC" runat="server" __designer:dtid="1407374883553292" Width="84px" Text="Cerrar" __designer:wfdid="w9" OnClientClick="return false;"></asp:Button></TD></TR></TBODY></TABLE></DIV>
</contenttemplate>
        <triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="lblTotalEmpresas" EventName="Click"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="txtPeriodo" EventName="Load"></asp:AsyncPostBackTrigger>
</triggers>
    </asp:UpdatePanel>
</asp:Content>

