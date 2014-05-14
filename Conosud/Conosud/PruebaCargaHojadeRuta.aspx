<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PruebaCargaHojadeRuta.aspx.cs" Inherits="PruebaCargaHojadeRuta" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/tr/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
            body {
                /* Style */
                color:#000000;
                font-family:arial,verdana,sans-serif;
                font-size:small;
            } 
            .modalBackground {
	            background-color:Gray;
	            filter:alpha(opacity=70);
	            opacity:0.7;
            }
        .style1
        {}
        </style>        
</head>
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
    function Fecha()
    {
        chk = window.document.getElementById("ctl00_ContentPlaceHolder1_DetailsView1_chkAprobado");
        if(chk.checked)
            window.document.getElementById("ctl00_ContentPlaceHolder1_DetailsView1_txtFechaAprobacion").innerText = '<%= FechaActual%>';
        else
            window.document.getElementById("ctl00_ContentPlaceHolder1_DetailsView1_txtFechaAprobacion").innerText = '';
            
    }
    
    
    function AbrirDivComentario()
    {
        var animBehavior = $find("AbrirComentarioItem"); 
        alert(animBehavior);
        animBehavior.get_OnClickBehavior().play(); 
        //window.event.returnValue = true;
        
    }
    
    function CerrarDiv()
    {
        var animBehavior = $find("DynamicAnimmationAprobacion"); 
        animBehavior.get_OnClickBehavior().play(); 
    }
    
    function CerrarDivCom()
    {
        var animBehavior = $find("DynamicAnimmationCom"); 
        animBehavior.get_OnClickBehavior().play(); 
    }
    
    function highlightRow(obj, newColor)
    {
         obj.style.cursor = "hand";
         // light-yellow, can be changed to whatever desired.
         obj.style.backgroundColor = "#ffffcc";
    }
  
   function dehighlightRow(obj, originalColor)
   {
     obj.style.backgroundColor = originalColor;
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
</script>
<body>
    <form id="form1" runat="server">
        <cc2:ToolkitScriptManager ID="ToolkitScriptManager1" EnableScriptGlobalization="true" runat="server" EnablePartialRendering="true">
        </cc2:ToolkitScriptManager>    
        
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
            
            <asp:GridView id="gdItemHoja" runat="server" Width="100%" Height="163px" 
                            PageSize="8"                            
                            EmptyDataText="There are no data records to display." 
                            DataKeyNames="IdHojaDeRuta" AutoGenerateColumns="False"><Columns>
            <asp:CommandField SelectImageUrl="~/images/right_16x16Conosud.gif" ShowSelectButton="True" ButtonType="Image"></asp:CommandField>
                                <asp:TemplateField HeaderText="Titulo" SortExpression="Titulo">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox6" runat="server" Text='<%# Eval("ObjPlantilla.Descripcion") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("ObjPlantilla.Descripcion") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
            <asp:BoundField DataField="DocFechaEntrega" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Doc" HtmlEncode="False" SortExpression="DocFechaEntrega"></asp:BoundField>
            <asp:TemplateField HeaderText="Comentario" SortExpression="DocComentario"><EditItemTemplate>
                <asp:TextBox id="TextBox1" runat="server" Text='<%# Bind("DocComentario") %>'></asp:TextBox> 
                
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label id="lblComentDoc" runat="server" Visible="False" Text='<%# Bind("DocComentario") %>'></asp:Label> 
                <asp:ImageButton id="btnEditComentDoc" runat="server" Width="20px" 
                    ImageUrl="~/images/notepad_16x16.gif" Height="20px" 
                    onclick="btnEditComentDoc_Click"></asp:ImageButton> 
                
            </ItemTemplate>

            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Controlado"><EditItemTemplate>
                <asp:TextBox id="TextBox2" runat="server" Text='<%# Bind("HojaFechaControlado") %>'></asp:TextBox> 
                
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label id="Label2" runat="server" Text='<%# Eval("HojaFechaControlado", "{0:d}") %>'></asp:Label>&nbsp; 
                
            </ItemTemplate>

            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Comentario" SortExpression="HojaComentario"><EditItemTemplate>
                <asp:TextBox id="TextBox3" runat="server" Text='<%# Bind("HojaComentario") %>'></asp:TextBox> 
                
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label id="Label3" runat="server" Visible="False" Text='<%# Bind("HojaComentario") %>'></asp:Label>
                <asp:ImageButton id="btnEditComentHoja" 
                runat="server" Width="20px" ImageUrl="~/images/notepad.gif" 
                Height="20px" OnClientClick="return false;"></asp:ImageButton> 
                
            </ItemTemplate>

            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Aprobado" SortExpression="HojaAprobado"><EditItemTemplate>
                    &nbsp;
                
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label id="Label4" runat="server" Visible="False" Text='<%# Bind("HojaAprobado") %>'></asp:Label> <asp:CheckBox id="chkAprobo" runat="server" Checked='<%# Bind("HojaAprobado") %>'></asp:CheckBox> 
                <asp:TextBox id="txtIdHojaDeRuta" runat="server" Text='<%# Bind("IdHojaDeRuta") %>' Visible="False"></asp:TextBox> 
                
            </ItemTemplate>

            <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField Visible="False"><EditItemTemplate>
                        <asp:TextBox runat="server" Text='<%# Bind("IdPlanilla") %>' id="TextBox4"></asp:TextBox>
                    
            </EditItemTemplate>
            <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Bind("IdPlanilla") %>' id="lblIdPlanilla"></asp:Label>
                    
            </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField Visible="False">
            <EditItemTemplate>
            <asp:TextBox runat="server" Text='<%# Bind("AuditadoPor") %>' id="TextBox5"></asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate>
            <asp:Label runat="server" Text='<%# Bind("AuditadoPor") %>' id="lblAuditadoPor"></asp:Label>
            </ItemTemplate>
            </asp:TemplateField>
            </Columns>
        </asp:GridView> 
   
        <asp:UpdatePanel id="upComentariosRecepcion" runat="server" UpdateMode="Conditional" >
        <contenttemplate>
        <asp:Panel ID="pnlAprobacion" runat="server" Width="0px" Height="0px" HorizontalAlign="Center" style="padding:0px;">
            <asp:DetailsView id="DetailsView1" runat="server" Width="100%" Height="100%" 
                BackColor="#F1DCDC" DataKeyNames="IdHojadeRuta" DefaultMode="Edit" 
                AutoGenerateRows="False" ><Fields>
                    <asp:TemplateField HeaderText="IdHojadeRuta" SortExpression="IdHojadeRuta" 
                        Visible="False">
                        <EditItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("IdHojadeRuta") %>'></asp:Label>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("IdHojadeRuta") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("IdHojadeRuta") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ReadOnly="True" DataField="IdHojadeRuta" Visible="False" SortExpression="IdHojadeRuta" HeaderText="IdHojadeRuta"></asp:BoundField>
                    <asp:TemplateField SortExpression="HojaFechaControlado" HeaderText="Ultimo Control"><EditItemTemplate>
                    <asp:TextBox id="txtFechaUltimoControl" runat="server" Width="90px" Text='<%# Bind("HojaFechaControlado") %>'></asp:TextBox><asp:ImageButton id="Image2" runat="Server" Width="19px" ImageUrl="~/Images/Calendar_scheduleHS.png" AlternateText="Clic para mostar Calendario"></asp:ImageButton>&nbsp;&nbsp; <ajaxToolkit:MaskedEditExtender id="MaskedEditExtender3" runat="server" TargetControlID="txtFechaUltimoControl" MaskType="Date" CultureName="es-AR" OnInvalidCssClass="MaskedEditError" OnFocusCssClass="MaskedEditFocus" Mask="99/99/9999" ErrorTooltipEnabled="True" DisplayMoney="Left" AcceptNegative="Left"></ajaxToolkit:MaskedEditExtender><ajaxToolkit:MaskedEditValidator id="MaskedEditValidator3" runat="server" TooltipMessage="Debe ingresar la Fecha del último control" InvalidValueMessage="La Fecha Ingrersada es Inválida" InvalidValueBlurredMessage="*" Display="Dynamic" ControlToValidate="txtFechaUltimoControl" ControlExtender="MaskedEditExtender3">&nbsp;</ajaxToolkit:MaskedEditValidator>&nbsp; <ajaxToolkit:CalendarExtender id="CalendarExtender2" runat="server" TargetControlID="txtFechaUltimoControl" PopupButtonID="Image2" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                    <asp:TextBox id="TextBox4" runat="server" Text='<%# Bind("HojaFechaControlado") %>'></asp:TextBox> 
                    </InsertItemTemplate>
                    <HeaderStyle ForeColor="Maroon" Font-Bold="True"></HeaderStyle>
                    <ItemTemplate>
                    <asp:Label id="Label4" runat="server" Text='<%# Bind("HojaFechaControlado") %>'></asp:Label> 
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="HojaComentario" HeaderText="Aprobado"><EditItemTemplate>
                    <asp:CheckBox id="chkAprobado" onclick="Fecha()" runat="server" Checked='<%# Bind("HojaAprobado") %>'></asp:CheckBox> 
                    </EditItemTemplate>
                    <InsertItemTemplate>
                    <asp:TextBox id="TextBox3" runat="server" Text='<%# Bind("HojaAprobado") %>'></asp:TextBox> 
                    </InsertItemTemplate>
                    <HeaderStyle ForeColor="Maroon" Font-Bold="True"></HeaderStyle>
                    <ItemTemplate>
                    <asp:Label id="Label3" runat="server" Text='<%# Bind("HojaAprobado") %>'></asp:Label> 
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="FechaControlado" HeaderText="Fecha Aprobaci&#243;n"><EditItemTemplate>
                    <asp:TextBox id="txtFechaAprobacion" runat="server" Width="84px" Text='<%# Bind("HojaFechaAprobacion", "{0:d}") %>' Enabled="False"></asp:TextBox> <asp:ImageButton id="Image1" runat="Server" Visible="False" Width="19px" AlternateText="Clic para mostar Calendario" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> <ajaxToolkit:MaskedEditExtender id="MaskedEditExtender2" runat="server" TargetControlID="txtFechaAprobacion" MaskType="Date" CultureName="es-AR" OnInvalidCssClass="MaskedEditError" OnFocusCssClass="MaskedEditFocus" Mask="99/99/9999" ErrorTooltipEnabled="True" DisplayMoney="Left" AcceptNegative="Left"></ajaxToolkit:MaskedEditExtender> <ajaxToolkit:MaskedEditValidator id="MaskedEditValidator2" runat="server" TooltipMessage="Debe ingresar la Fecha de aprobación del Item" InvalidValueMessage="La Fecha Ingrersada es Inválida" InvalidValueBlurredMessage="*" Display="Dynamic" ControlToValidate="txtFechaAprobacion" ControlExtender="MaskedEditExtender2">&nbsp;</ajaxToolkit:MaskedEditValidator> <ajaxToolkit:CalendarExtender id="CalendarExtender1" runat="server" TargetControlID="txtFechaAprobacion" PopupButtonID="Image1" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender> 
                    </EditItemTemplate>
                    <InsertItemTemplate>
                    <asp:TextBox id="TextBox2" runat="server" Text='<%# Bind("HojaFechaAprobacion") %>'></asp:TextBox> 
                    </InsertItemTemplate>
                    <HeaderStyle ForeColor="Maroon" Font-Bold="True"></HeaderStyle>
                    <ItemTemplate>
                    <asp:Label id="Label2" runat="server" Text='<%# Eval("HojaFechaAprobacion") %>'></asp:Label> 
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Comentarios" HeaderText="Comentarios"><EditItemTemplate>
                    <asp:TextBox id="TextBox1" runat="server" Width="638px" Text='<%# Bind("HojaComentario") %>' Height="255px" TextMode="MultiLine"></asp:TextBox> 
                    </EditItemTemplate>
                    <InsertItemTemplate>
                    <asp:TextBox id="TextBox1" runat="server" Text='<%# Bind("HojaComentario") %>'></asp:TextBox> 
                    </InsertItemTemplate>
                    <HeaderStyle ForeColor="Maroon" Font-Bold="True"></HeaderStyle>
                    <ItemTemplate>
                    <asp:Label id="Label1" runat="server" Text='<%# Eval("HojaComentario") %>'></asp:Label> 
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="IdPlantilla" Visible="False" 
                        SortExpression="IdPlantilla" HeaderText="IdPlantilla"></asp:BoundField>
                    <asp:BoundField DataField="IdEstado" Visible="False" SortExpression="IdEstado" HeaderText="IdEstado"></asp:BoundField>
                    <asp:TemplateField ShowHeader="False"><EditItemTemplate>
                    <asp:ImageButton id="ImageButton1" runat="server" ImageUrl="~/images/ok_16x16.gif" Text="Update" CausesValidation="True" CommandName="Update"></asp:ImageButton>&nbsp;<asp:ImageButton id="btnCancelar" runat="server" ImageUrl="~/images/delete_16x16.gif" Text="Cancel" CausesValidation="False" CommandName="Cancel"></asp:ImageButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                    <asp:Button id="Button1" runat="server" Text="Edit" CausesValidation="False" CommandName="Edit"></asp:Button> 
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="AuditadoPor" Visible="False" SortExpression="AuditadoPor" HeaderText="AuditadoPor"></asp:BoundField>
                    <asp:TemplateField InsertVisible="False" ShowHeader="False"><EditItemTemplate>
                    &nbsp;<asp:ImageButton id="btnCerrar" runat="server" ImageUrl="~/images/delete_16x16.gif" Text="Cancel" CausesValidation="False" CommandName="Cancel"></asp:ImageButton>
                    </EditItemTemplate>
                    </asp:TemplateField>
                    </Fields>
            </asp:DetailsView> 
        </asp:Panel>
            <asp:Panel ID="pnl" runat="server" Width="0px" Height="0px" HorizontalAlign="Center" style="padding:0px;">
                <table style="width:100%;height:100%;background-color:Black" border="1">
                    <tr>
                        <td align="right" style="height:20px">
                            <asp:ImageButton id="btnCerrarComentDoc" runat="server" Width="16px" BackColor="#E0E0E0" Height="16px" ImageUrl="~/images/delete_16x16.gif" BorderStyle="Solid"  BorderWidth="1px" BorderColor="Black"></asp:ImageButton>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:TextBox id="txtComentDoc" runat="server" Width="99%" Height="350px" TextMode="MultiLine" ReadOnly="True"></asp:TextBox>                 
                        </td>
                    </tr>

                </table>
            </asp:Panel>
                  
            <asp:Button ID="btnShow" runat="server" onclick="btnShow_Click" Text="Button"  />
            <ajaxToolkit:ModalPopupExtender ID="mdlPopupFadeIn" runat="server"  BehaviorID="mdlPopupFadeIn"       
                PopupControlID="pnlAprobacion" 
                BackgroundCssClass="modalBackground"
                OkControlID="btnCerrarComentDoc"
                TargetControlID="btnShow">
            </ajaxToolkit:ModalPopupExtender>
            
            <asp:Button ID="btnOtro" runat="server" onclick="btnOtro_Click" Text="Button" />
            
  

                          
                          
       </contenttemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gdItemHoja" EventName="RowCommand" />
            </triggers>
    </asp:UpdatePanel>     
    </form>
</body>
</html>
