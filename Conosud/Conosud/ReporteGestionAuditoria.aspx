<%@ Page Language="C#" EnableEventValidation="false"  Theme="MiTema" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="ReporteGestionAuditoria.aspx.cs" Inherits="ReporteGestionAuditoria" Title="Untitled Page" %>
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
                event.returnValue=false; 
                event.cancel = true;
            }
        }
    }
</script>

    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" EnablePartialRendering="true" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <div id="flyout" style="border-right: #d0d0d0 1px solid; border-top: #d0d0d0 1px solid;
        display: none; z-index: 2; left: 100px; overflow: hidden; border-left: #d0d0d0 1px solid;
        width: 100px; border-bottom: #d0d0d0 1px solid; position: absolute; top: 287px;
        height: 100px; background-color: aqua">
    </div>
    <table cellpadding="0" cellspacing="5">
        <tr>
            <td align="center" style="width: 241px">
                <asp:Label ID="lblEncabezadoS" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Font-Bold="True" Font-Size="14pt" ForeColor="Maroon" Height="25px"
                    Style="background-image: url(images/FondoTitulos.gif); background-color: transparent"
                    Text="REPORTE GESTION AUDITORIA" Width="368px"></asp:Label></td>
        </tr>
    </table>
    <table id="Table1" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
        border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #f1dcdc">
        <tr>
            <td align="right" colspan="2" style="height: 26px">
                <asp:Label ID="lblEmpresa" runat="server" Font-Bold="True" ForeColor="Maroon" Height="6px"
                    Text="Periodo a Consultar:" Width="157px"></asp:Label>&nbsp;
                    </td>
            <td align="left" colspan="2">
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
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<asp:UpdateProgress id="UpdateProgress1" runat="server" DisplayAfter="100" __designer:wfdid="w15"><ProgressTemplate>
<DIV class="progress"><IMG src="images/indicator.gif" /> <asp:Label id="Label8" runat="server" Width="226px" Text="Generando Datos Gestión...." Font-Bold="True" __designer:wfdid="w16"></asp:Label> </DIV>
</ProgressTemplate>
</asp:UpdateProgress> <ajaxToolkit:AnimationExtender id="AEDetalle" runat="server" TargetControlID="Label1" __designer:wfdid="w17"><Animations>
            <OnClick>
                    <Sequence>
                        
                        
                        <ScriptAction Script="Cover($get('lblEmpresa'), $get('flyout'));" />
                        <StyleAction AnimationTarget="flyout" Attribute="display" Value="block"/>
                        
                        
                        <Parallel AnimationTarget="flyout" Duration=".3" Fps="25">
                            <Move Horizontal="-300" Vertical="-100" />
                            <Resize Width="285" Height="281" />
                            <Color PropertyKey="backgroundColor" StartValue="#AAAAAA" EndValue="#FFFFFF" />
                        </Parallel>
                        
                        
                        
                        <ScriptAction Script="Cover($get('flyout'), $get('divDetalle'), true);" />
                        <StyleAction AnimationTarget="divDetalle" Attribute="display" Value="block"/>
                        <FadeIn AnimationTarget="divDetalle" Duration=".2"/>
                        <StyleAction AnimationTarget="flyout" Attribute="display" Value="none"/>
                        
                      


                    </Sequence>
                </OnClick></Animations>
</ajaxToolkit:AnimationExtender> <ajaxToolkit:AnimationExtender id="AEDetalleMA" runat="server" TargetControlID="Label1" __designer:wfdid="w18"><Animations>
            <OnClick>
                    <Sequence>
                        
                        
                        <ScriptAction Script="Cover($get('lblEmpresa'), $get('flyout'));" />
                        <StyleAction AnimationTarget="flyout" Attribute="display" Value="block"/>
                        
                        
                        <Parallel AnimationTarget="flyout" Duration=".3" Fps="25">
                            <Move Horizontal="-300" Vertical="-100" />
                            <Resize Width="285" Height="281" />
                            <Color PropertyKey="backgroundColor" StartValue="#AAAAAA" EndValue="#FFFFFF" />
                        </Parallel>
                        
                        
                        
                        <ScriptAction Script="Cover($get('flyout'), $get('divDetalleMA'), true);" />
                        <StyleAction AnimationTarget="divDetalleMA" Attribute="display" Value="block"/>
                        <FadeIn AnimationTarget="divDetalleMA" Duration=".2"/>
                        <StyleAction AnimationTarget="flyout" Attribute="display" Value="none"/>
                        
                      


                    </Sequence>
                </OnClick></Animations>
</ajaxToolkit:AnimationExtender> <ajaxToolkit:AnimationExtender id="AEDetalleOM" runat="server" TargetControlID="Label1" __designer:wfdid="w19"><Animations>
            <OnClick>
                    <Sequence>
                        
                        
                        <ScriptAction Script="Cover($get('lblEmpresa'), $get('flyout'));" />
                        <StyleAction AnimationTarget="flyout" Attribute="display" Value="block"/>
                        
                        
                        <Parallel AnimationTarget="flyout" Duration=".3" Fps="25">
                            <Move Horizontal="-300" Vertical="-100" />
                            <Resize Width="285" Height="281" />
                            <Color PropertyKey="backgroundColor" StartValue="#AAAAAA" EndValue="#FFFFFF" />
                        </Parallel>
                        
                        
                        
                        <ScriptAction Script="Cover($get('flyout'), $get('divDetalleOM'), true);" />
                        <StyleAction AnimationTarget="divDetalleOM" Attribute="display" Value="block"/>
                        <FadeIn AnimationTarget="divDetalleOM" Duration=".2"/>
                        <StyleAction AnimationTarget="flyout" Attribute="display" Value="none"/>
                        
                      


                    </Sequence>
                </OnClick></Animations>
</ajaxToolkit:AnimationExtender> <ajaxToolkit:AnimationExtender id="CloseAnimation" runat="server" TargetControlID="btnCerrar" __designer:wfdid="w20"><Animations>
                <OnClick>
                    <Sequence AnimationTarget="divDetalle">
                        
                        <StyleAction Attribute="overflow" Value="hidden"/>
                        <Parallel Duration=".3" Fps="15">
                            <Scale ScaleFactor="0.05" Center="true" ScaleFont="true" FontUnit="px" />
                            <FadeOut />
                        </Parallel>
                        
                        
                        <StyleAction Attribute="display" Value="none"/>
                        <StyleAction Attribute="width" Value="250px"/>
                        <StyleAction Attribute="height" Value=""/>
                        <StyleAction Attribute="fontSize" Value="12px"/>

                    </Sequence>
                </OnClick>
</Animations>
</ajaxToolkit:AnimationExtender> <ajaxToolkit:AnimationExtender id="CloseAnimationMA" runat="server" TargetControlID="btnCerrarMA" __designer:wfdid="w21"><Animations>
                <OnClick>
                    <Sequence AnimationTarget="divDetalleMA">
                        
                        <StyleAction Attribute="overflow" Value="hidden"/>
                        <Parallel Duration=".3" Fps="15">
                            <Scale ScaleFactor="0.05" Center="true" ScaleFont="true" FontUnit="px" />
                            <FadeOut />
                        </Parallel>
                        
                        
                        <StyleAction Attribute="display" Value="none"/>
                        <StyleAction Attribute="width" Value="250px"/>
                        <StyleAction Attribute="height" Value=""/>
                        <StyleAction Attribute="fontSize" Value="12px"/>

                    </Sequence>
                </OnClick></Animations>
</ajaxToolkit:AnimationExtender> <ajaxToolkit:AnimationExtender id="CloseAnimationOM" runat="server" TargetControlID="btnCerrarOM" __designer:wfdid="w22"><Animations>
                <OnClick>
                    <Sequence AnimationTarget="divDetalleOM">
                        
                        <StyleAction Attribute="overflow" Value="hidden"/>
                        <Parallel Duration=".3" Fps="15">
                            <Scale ScaleFactor="0.05" Center="true" ScaleFont="true" FontUnit="px" />
                            <FadeOut />
                        </Parallel>
                        
                        
                        <StyleAction Attribute="display" Value="none"/>
                        <StyleAction Attribute="width" Value="250px"/>
                        <StyleAction Attribute="height" Value=""/>
                        <StyleAction Attribute="fontSize" Value="12px"/>

                    </Sequence>
                </OnClick></Animations>
</ajaxToolkit:AnimationExtender> <TABLE style="WIDTH: 790px"><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 13px" align=left><TABLE style="BORDER-RIGHT: black thin solid; BORDER-TOP: black thin solid; BORDER-LEFT: black thin solid; WIDTH: 382px; BORDER-BOTTOM: black thin solid; HEIGHT: 29px; BACKGROUND-COLOR: gainsboro" id="Table2"><TBODY><TR><TD align=left colSpan=4><asp:Label id="Label1" runat="server" ForeColor="Firebrick" Font-Size="13pt" Width="393px" Text="Documentación Presentada para el Mes de Consulta" Height="6px" Font-Bold="True" Font-Underline="True" __designer:wfdid="w23"></asp:Label></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE><DIV style="OVERFLOW: auto; WIDTH: 792px; HEIGHT: 272px; TEXT-ALIGN: left"><asp:GridView id="gvAuditoriaActual" runat="server" Width="774px" Height="120px" OnSelectedIndexChanged="gvAuditoriaActual_SelectedIndexChanged" OnRowDataBound="gvAuditoriaActual_RowDataBound" EmptyDataText="Sin Datos Generados" AutoGenerateColumns="False" __designer:wfdid="w24"><Columns>
<asp:BoundField DataField="CantidadHojas" HeaderText="Cantidad Hojas"></asp:BoundField>
<asp:BoundField DataField="CantidadLegajos" HeaderText="Cantidad Legajos"></asp:BoundField>
<asp:BoundField DataField="Nombre" HeaderText="Categoria"></asp:BoundField>
<asp:BoundField DataField="ItemsControlados" HeaderText="Items Controlados"></asp:BoundField>
<asp:BoundField DataField="TotalItems" HeaderText="Total Items"></asp:BoundField>
<asp:BoundField DataField="Porcentaje" HeaderText="Porcentaje Cumplido"></asp:BoundField>
<asp:BoundField DataField="Auditor" HeaderText="Auditor"></asp:BoundField>
<asp:TemplateField ShowHeader="False"><ItemTemplate>
<asp:Button id="btnComponentes" runat="server" Text="Detalle" CausesValidation="false" CommandName="" OnClientClick="return false;" Enabled="False"></asp:Button>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView> <DIV style="BORDER-RIGHT: #cccccc 1px solid; PADDING-RIGHT: 5px; BORDER-TOP: #cccccc 1px solid; DISPLAY: none; PADDING-LEFT: 5px; FONT-SIZE: 12px; Z-INDEX: 2; FILTER: progid:DXImageTransform.Microsoft.Alpha(opacity=100); PADDING-BOTTOM: 5px; VERTICAL-ALIGN: top; BORDER-LEFT: #cccccc 1px solid; WIDTH: 264px; PADDING-TOP: 5px; BORDER-BOTTOM: #cccccc 1px solid; HEIGHT: 219px; BACKGROUND-COLOR: #b5494a; TEXT-ALIGN: center; opacity: 0" id="divDetalle"><TABLE id="tblDetalle" height=281 width=285><TBODY><TR><TD style="WIDTH: 150px; HEIGHT: 153px" vAlign=top align=center><asp:GridView id="gvDetalle" runat="server" Width="275px" AutoGenerateColumns="False" __designer:wfdid="w25"><Columns>
<asp:BoundField DataField="RazonSocial" HeaderText="Empresa"></asp:BoundField>
<asp:BoundField DataField="Codigo" HeaderText="Contrato"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:yyyy/MM}" DataField="Periodo" HeaderText="Periodo"></asp:BoundField>
</Columns>
</asp:GridView></TD></TR><TR><TD vAlign=bottom align=center><asp:Button id="btnCerrar" runat="server" Width="66px" Text="Cerrar" OnClientClick="return false;" __designer:wfdid="w26"></asp:Button> </TD></TR></TBODY></TABLE></DIV></DIV><TABLE style="WIDTH: 794px"><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 13px" align=left><TABLE style="BORDER-RIGHT: black thin solid; BORDER-TOP: black thin solid; BORDER-LEFT: black thin solid; WIDTH: 382px; BORDER-BOTTOM: black thin solid; HEIGHT: 29px; BACKGROUND-COLOR: gainsboro" id="Table3"><TBODY><TR><TD align=left colSpan=4><asp:Label id="Label2" runat="server" ForeColor="Firebrick" Font-Size="13pt" Width="417px" Text="Documentación Presentada para el Meses de Anteriores" Height="6px" Font-Bold="True" Font-Underline="True" __designer:wfdid="w27"></asp:Label></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE><DIV style="OVERFLOW: auto; WIDTH: 792px; HEIGHT: 272px; TEXT-ALIGN: left"><asp:GridView id="gvAuditoriaMesesAnterior" runat="server" Width="786px" Height="120px" OnSelectedIndexChanged="gvAuditoriaMesesAnterior_SelectedIndexChanged" OnRowDataBound="gvAuditoriaMesesAnterior_RowDataBound" EmptyDataText="Sin Datos Generados" AutoGenerateColumns="False" __designer:wfdid="w28"><Columns>
<asp:BoundField DataField="CantidadHojas" HeaderText="Cantidad Hojas"></asp:BoundField>
<asp:BoundField DataField="CantidadLegajos" HeaderText="Cantidad Legajos"></asp:BoundField>
<asp:BoundField DataField="Nombre" HeaderText="Categoria"></asp:BoundField>
<asp:BoundField DataField="ItemsControlados" HeaderText="Items Controlados"></asp:BoundField>
<asp:BoundField DataField="TotalItems" HeaderText="Total Items"></asp:BoundField>
<asp:BoundField DataField="Porcentaje" HeaderText="Porcentaje Cumplido"></asp:BoundField>
<asp:BoundField DataField="Auditor" HeaderText="Auditor"></asp:BoundField>
<asp:TemplateField ShowHeader="False"><ItemTemplate>
<asp:Button id="btnDetalleMA" runat="server" Text="Detalle" OnClientClick="return false;" CausesValidation="false" CommandName="" __designer:wfdid="w13" Enabled="False"></asp:Button> 
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView> <DIV style="BORDER-RIGHT: #cccccc 1px solid; PADDING-RIGHT: 5px; BORDER-TOP: #cccccc 1px solid; DISPLAY: none; PADDING-LEFT: 5px; FONT-SIZE: 12px; Z-INDEX: 2; FILTER: progid:DXImageTransform.Microsoft.Alpha(opacity=100); PADDING-BOTTOM: 5px; VERTICAL-ALIGN: top; BORDER-LEFT: #cccccc 1px solid; WIDTH: 264px; PADDING-TOP: 5px; BORDER-BOTTOM: #cccccc 1px solid; HEIGHT: 219px; BACKGROUND-COLOR: #b5494a; TEXT-ALIGN: center; opacity: 0" id="divDetalleMA"><TABLE id="Table5" height=281 width=285><TBODY><TR><TD style="WIDTH: 150px; HEIGHT: 153px" vAlign=top align=center><asp:GridView id="gvDetalleMA" runat="server" Width="275px" AutoGenerateColumns="False" __designer:wfdid="w29"><Columns>
<asp:BoundField DataField="RazonSocial" HeaderText="Empresa"></asp:BoundField>
<asp:BoundField DataField="Codigo" HeaderText="Contrato"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:yyyy/MM}" DataField="Periodo" HeaderText="Periodo"></asp:BoundField>
</Columns>
</asp:GridView></TD></TR><TR><TD vAlign=bottom align=center><asp:Button id="btnCerrarMA" runat="server" Width="66px" Text="Cerrar" OnClientClick="return false;" __designer:wfdid="w30"></asp:Button> </TD></TR></TBODY></TABLE></DIV></DIV><TABLE style="WIDTH: 792px"><TBODY><TR><TD style="WIDTH: 105px; HEIGHT: 13px" align=left><TABLE style="BORDER-RIGHT: black thin solid; BORDER-TOP: black thin solid; BORDER-LEFT: black thin solid; WIDTH: 382px; BORDER-BOTTOM: black thin solid; HEIGHT: 29px; BACKGROUND-COLOR: gainsboro" id="Table4"><TBODY><TR><TD align=left colSpan=4><asp:Label id="Label3" runat="server" ForeColor="Firebrick" Font-Size="13pt" Width="361px" Text="Documentación Presentada en Otros Meses" Height="6px" Font-Bold="True" Font-Underline="True" __designer:wfdid="w31"></asp:Label></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE><DIV style="OVERFLOW: auto; WIDTH: 792px; HEIGHT: 272px; TEXT-ALIGN: left"><asp:GridView id="gvAuditoriaDesplazada" runat="server" Width="789px" Height="120px" OnSelectedIndexChanged="gvAuditoriaDesplazada_SelectedIndexChanged" OnRowDataBound="gvAuditoriaDesplazada_RowDataBound" EmptyDataText="Sin Datos Generados" AutoGenerateColumns="False" __designer:wfdid="w32"><Columns>
<asp:BoundField DataField="CantidadHojas" HeaderText="Cantidad Hojas"></asp:BoundField>
<asp:BoundField DataField="CantidadLegajos" HeaderText="Cantidad Legajos"></asp:BoundField>
<asp:BoundField DataField="Nombre" HeaderText="Categoria"></asp:BoundField>
<asp:BoundField DataField="ItemsControlados" HeaderText="Items Controlados"></asp:BoundField>
<asp:BoundField DataField="TotalItems" HeaderText="Total Items"></asp:BoundField>
<asp:BoundField DataField="Porcentaje" HeaderText="Porcentaje Cumplido"></asp:BoundField>
<asp:BoundField DataField="Auditor" HeaderText="Auditor"></asp:BoundField>
<asp:TemplateField ShowHeader="False"><ItemTemplate>
<asp:Button id="btnDetalleOM" runat="server" Text="Detalle" OnClientClick="return false;" CausesValidation="false" CommandName="" __designer:wfdid="w14" Enabled="False"></asp:Button> 
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView> <DIV style="BORDER-RIGHT: #cccccc 1px solid; PADDING-RIGHT: 5px; BORDER-TOP: #cccccc 1px solid; DISPLAY: none; PADDING-LEFT: 5px; FONT-SIZE: 12px; Z-INDEX: 2; FILTER: progid:DXImageTransform.Microsoft.Alpha(opacity=100); PADDING-BOTTOM: 5px; VERTICAL-ALIGN: top; BORDER-LEFT: #cccccc 1px solid; WIDTH: 264px; PADDING-TOP: 5px; BORDER-BOTTOM: #cccccc 1px solid; HEIGHT: 219px; BACKGROUND-COLOR: #b5494a; TEXT-ALIGN: center; opacity: 0" id="divDetalleOM"><TABLE id="Table6" height=281 width=285><TBODY><TR><TD style="WIDTH: 150px; HEIGHT: 153px" vAlign=top align=center><asp:GridView id="gvDetalleOM" runat="server" Width="275px" AutoGenerateColumns="False" __designer:wfdid="w33"><Columns>
<asp:BoundField DataField="RazonSocial" HeaderText="Empresa"></asp:BoundField>
<asp:BoundField DataField="Codigo" HeaderText="Contrato"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:yyyy/MM}" DataField="Periodo" HeaderText="Periodo"></asp:BoundField>
</Columns>
</asp:GridView></TD></TR><TR><TD vAlign=bottom align=center><asp:Button id="btnCerrarOM" runat="server" Width="66px" Text="Cerrar" OnClientClick="return false;" __designer:wfdid="w34"></asp:Button> </TD></TR></TBODY></TABLE></DIV></DIV>&nbsp; 
</contenttemplate>
        <triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="txtPeriodo" EventName="Load"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="gvAuditoriaActual" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="gvAuditoriaDesplazada" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="gvAuditoriaMesesAnterior" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
</triggers>
    </asp:UpdatePanel>
</asp:Content>

