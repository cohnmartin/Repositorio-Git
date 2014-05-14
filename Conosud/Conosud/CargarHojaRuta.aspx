<%@ Page Language="C#" EnableEventValidation="false" Theme="MiTema" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="CargarHojaRuta.aspx.cs" Inherits="CargarHojaRuta" Title="Untitled Page" %>

<%@ Register Assembly="FUA" Namespace="Subgurim.Controles" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

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

    <cc2:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true">
    </cc2:ToolkitScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
            <cc2:AnimationExtender id="AnimationExtender1" runat="server" TargetControlID="ctl00$ContentPlaceHolder1$gdItemHoja$ctl03$btnEditComentHoja">
                <Animations>
                <OnClick>
                    <Sequence>
                        
                       
                        <ScriptAction Script="Cover($get('ctl00_ContentPlaceHolder1_LoginName1'), $get('flyout'));" />
                        
                        <StyleAction AnimationTarget="flyout" Attribute="display" Value="block"/>                        
                                                                    
                        <Parallel AnimationTarget="flyout" Duration=".3" Fps="25">
                            <Move Horizontal="-100" Vertical="25" />
                            <Resize Width="729px" Height="480px" />
                            <Color PropertyKey="backgroundColor" StartValue="#AAAAAA" EndValue="#FFFFFF" />
                        </Parallel>
                        
                        
                        <ScriptAction Script="Cover($get('flyout'), $get('info'), true);" />
                        <StyleAction AnimationTarget="info" Attribute="display" Value="block"/>
                        <FadeIn AnimationTarget="info" Duration=".2"/>
                        
                        <StyleAction AnimationTarget="flyout" Attribute="display" Value="none"/>
                        
                        
                        
                        <Parallel AnimationTarget="info" Duration=".5">
                            <Color PropertyKey="color" StartValue="#666666" EndValue="#FF0000" />
                            <Color PropertyKey="borderColor" StartValue="#666666" EndValue="#FF0000" />
                        </Parallel>


                    </Sequence>
                </OnClick></Animations>
            </cc2:AnimationExtender> 
            <cc2:AnimationExtender id="AnimationExtender2" runat="server" TargetControlID="Label52">
                <Animations>
                <OnClick>
                    <Sequence>
                        
                        <ScriptAction Script="Cover($get('ctl00$ContentPlaceHolder1$gdItemHoja$ctl03$btnEditComentDoc'), $get('flyout1'));" />
                        
                        <StyleAction AnimationTarget="flyout1" Attribute="display" Value="block"/>                        
                                                
                        <Parallel AnimationTarget="flyout1" Duration=".3" Fps="25">
                            <Move Horizontal="-100" Vertical="-25" />
                            <Resize Height="280px" Width="581px" />
                            <Color PropertyKey="backgroundColor" StartValue="#AAAAAA" EndValue="#FFFFFF" />
                        </Parallel>
                        
                        
                        <ScriptAction Script="Cover($get('flyout1'), $get('info1'), true);" />
                        <StyleAction AnimationTarget="info1" Attribute="display" Value="block"/>
                        <FadeIn AnimationTarget="info1" Duration=".2"/>
                        
                        <StyleAction AnimationTarget="flyout1" Attribute="display" Value="none"/>
                        
                        
                        
                        <Parallel AnimationTarget="info1" Duration=".5">
                            <Color PropertyKey="color" StartValue="#666666" EndValue="#FF0000" />
                            <Color PropertyKey="borderColor" StartValue="#666666" EndValue="#FF0000" />
                        </Parallel>


                    </Sequence>
                </OnClick></Animations>
            </cc2:AnimationExtender> 
            <cc2:AnimationExtender id="AnimationExtender3" runat="server" TargetControlID="BtnEstimacion">
    <Animations>
                <OnClick>
                    <Sequence>
                        
                        <ScriptAction Script="Cover($get('ctl00_ContentPlaceHolder1_BtnEstimacion'), $get('flyEstimacion'));" />

                        <StyleAction AnimationTarget="flyEstimacion" Attribute="display" Value="block"/>
                        
                        <Parallel AnimationTarget="flyEstimacion" Duration=".3" Fps="25">
                            <Move Horizontal="80" Vertical="30" />
                            <Resize Height="300px" Width="580px" />
                            <Color PropertyKey="backgroundColor" StartValue="#AAAAAA" EndValue="#FFFFFF" />
                        </Parallel>
                        
                        
                        <ScriptAction Script="Cover($get('flyEstimacion'), $get('DivEstimacion'), true);" />
                        <StyleAction AnimationTarget="DivEstimacion" Attribute="display" Value="block"/>
                        <FadeIn AnimationTarget="DivEstimacion" Duration=".02"/>
                        
                        <StyleAction AnimationTarget="flyEstimacion" Attribute="display" Value="none"/>
                        
                        
                        
                        <Parallel AnimationTarget="DivEstimacion" Duration=".5">
                            <Color PropertyKey="color" StartValue="#666666" EndValue="#FF0000" />
                            <Color PropertyKey="borderColor" StartValue="#666666" EndValue="#FF0000" />
                        </Parallel>  


                    </Sequence>
                </OnClick></Animations>
</cc2:AnimationExtender>
            <cc2:AnimationExtender id="AnimationExtender4" runat="server" TargetControlID="btnComentario"><Animations>
                <OnClick>
                    <Sequence>
                        
                        <ScriptAction Script="Cover($get('ctl00_ContentPlaceHolder1_btnComentario'), $get('flyEstimacion'));" />

                        <StyleAction AnimationTarget="flyEstimacion" Attribute="display" Value="block"/>
                        
                        <Parallel AnimationTarget="flyEstimacion" Duration=".3" Fps="25">
                            <Move Horizontal="150" Vertical="50" />
                            <Resize Height="420px" Width="550px" />
                            <Color PropertyKey="backgroundColor" StartValue="#AAAAAA" EndValue="#FFFFFF" />
                        </Parallel>
                        
                        
                        <ScriptAction Script="Cover($get('flyEstimacion'), $get('divFile'), true);" />
                        <StyleAction AnimationTarget="divFile" Attribute="display" Value="block"/>
                        <FadeIn AnimationTarget="divFile" Duration=".02"/>
                        
                        <StyleAction AnimationTarget="flyEstimacion" Attribute="display" Value="none"/>
                        
                        
                        
                        <Parallel AnimationTarget="divFile" Duration=".5">
                            <Color PropertyKey="color" StartValue="#666666" EndValue="#FF0000" />
                            <Color PropertyKey="borderColor" StartValue="#666666" EndValue="#FF0000" />
                        </Parallel>  


                    </Sequence>
                </OnClick></Animations>
</cc2:AnimationExtender> 
            <ajaxToolkit:AnimationExtender id="CloseAnimationDoc" runat="server" TargetControlID="btnCerrarComentDoc" BehaviorID="DynamicAnimmationCom"><Animations>
                <OnClick>
                    <Sequence AnimationTarget="info1">
                        
                        <StyleAction Attribute="overflow" Value="hidden"/>
                        <Parallel Duration=".3" Fps="15">
                            <Scale ScaleFactor="0.05" Center="true" ScaleFont="true" FontUnit="px" />
                            <FadeOut />
                        </Parallel>
                        
                        
                        <StyleAction Attribute="display" Value="none"/>
                        <StyleAction Attribute="width" Value="575px"/>
                        <StyleAction Attribute="height" Value="322px"/>
                        <StyleAction Attribute="fontSize" Value="13px"/>

                    </Sequence>
                </OnClick></Animations>
</ajaxToolkit:AnimationExtender> 
            <ajaxToolkit:AnimationExtender id="CloseAprobacion" runat="server" TargetControlID="btnCerrarComentDoc" BehaviorID="DynamicAnimmationAprobacion"><Animations>
                <OnClick>
                    <Sequence AnimationTarget="info">
                        
                        <StyleAction Attribute="overflow" Value="hidden"/>
                        <Parallel Duration=".3" Fps="15">
                            <Scale ScaleFactor="0.05" Center="true" ScaleFont="true" FontUnit="px" />
                            <FadeOut />
                        </Parallel>
                        
                        
                        <StyleAction Attribute="display" Value="none"/>
                        <StyleAction Attribute="width" Value="729px"/>
                        <StyleAction Attribute="height" Value="480px"/>
                        <StyleAction Attribute="fontSize" Value="13px"/>

                    </Sequence>
                </OnClick></Animations>
</ajaxToolkit:AnimationExtender>

 <asp:TextBox id="fechaPrueba" runat="server" Visible="False">
    </asp:TextBox> <asp:ImageButton id="Image12" runat="Server" Width="19px" Visible="False" AlternateText="Clic para mostar Calendario" ImageUrl="~/Images/Calendar_scheduleHS.png">
    </asp:ImageButton> <cc2:CalendarExtender id="CalendarExtender21" runat="server" TargetControlID="fechaPrueba" PopupButtonID="Image1">
    </cc2:CalendarExtender> 
    
     <table style="BORDER-RIGHT: #843431 thin solid; BORDER-TOP: #843431 thin solid; BORDER-LEFT: #843431 thin solid; WIDTH: 789px; BORDER-BOTTOM: #843431 thin solid; HEIGHT: 89px; BACKGROUND-COLOR: #f1dcdc" id="tblEncabezado">
            <tbody>
            <tr>
                <td style="WIDTH: 128px; HEIGHT: 26px" align="right">
                    <asp:Label id="Label10" runat="server" Width="119px" ForeColor="Maroon" Height="22px" Font-Bold="True" Text="Contratista:"></asp:Label>
                </td>
                <td style="HEIGHT: 26px" align="left" colspan="5">
                    <asp:Label id="lblContratista" runat="server" Width="100%" Text="" Font-Size="12px" Font-Names="Verdana"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="WIDTH: 128px; HEIGHT: 15px" align="right">
                    <asp:Label id="Label34" runat="server" Width="93px" ForeColor="Maroon" Height="25px" Font-Bold="True" Text="Contrato:   "></asp:Label>
                </td>
                <td style="HEIGHT: 15px" align="left" colspan="5">
                    <asp:Label id="lblDescContrato" runat="server" Font-Size="12px" Font-Names="Verdana" Width="100%" Height="21px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="WIDTH: 128px; HEIGHT: 15px" align="right">
                    <asp:Label id="Label52" runat="server" Width="129px" ForeColor="Maroon" Height="19px" Font-Bold="True" Text="Sub Contratista:"></asp:Label> 
                </td>
                <td style="HEIGHT: 15px" align="left" colspan="5">
                    <asp:Label id="lblSubCon" runat="server" Width="246px" Text="" Font-Size="12px" Font-Names="Verdana"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="WIDTH: 128px; HEIGHT: 14px" align="right">
                    <asp:Label id="Label4" runat="server" Width="87px" ForeColor="Maroon" Height="25px" Font-Bold="True" Text="Fecha:"></asp:Label>
                </td>
                <td style="WIDTH: 112px; HEIGHT: 14px" align="left">
                    <asp:Label id="lblFechaHojaRuta" runat="server" Width="90px" Text="" Font-Size="12px" Font-Names="Verdana"></asp:Label>
                </td>
                <td style="WIDTH: 82px; HEIGHT: 14px" align="left">
                    <asp:Label id="Label5" runat="server" Width="129px" ForeColor="Maroon" Height="20px" Font-Bold="True" Text="Carpeta Asignada:"></asp:Label>
                </td>
                <td align="left">
                    <asp:Label id="lblNroCarpeta" runat="server" Width="84px" ForeColor="Black" Height="25px" Font-Bold="True" Text="" Font-Size="12px" Font-Names="Verdana"></asp:Label>
                </td>
                <td>
                    <asp:Label id="Label6" runat="server" Width="55px" ForeColor="Maroon" Height="24px" Font-Bold="True" Text="Estado:"></asp:Label>
                </td>
                <td>
                    <asp:Label id="lblEstadoDoc" runat="server"
                         Width="200px" 
                         ForeColor="White" 
                         BackColor="#FF8080" 
                         Height="21px" 
                         Font-Bold="True" 
                         BorderStyle="None" 
                         Font-Size="14px" Font-Names="Verdana"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="WIDTH: 128px; HEIGHT: 14px" align="right">
                    <asp:Label id="Label9" runat="server" Width="129px" ForeColor="Maroon" Height="20px" Font-Bold="True" Text="Auditor:"></asp:Label>
                </td>
                <td style="HEIGHT: 14px" align="left" colSpan=1>
                    <asp:Label id="lblUsuario" runat="server" Width="84px" ForeColor="Black" 
                        Font-Bold="True"  Font-Size="12px" Font-Names="Verdana"></asp:Label>
                </td>
                <td style="HEIGHT: 14px" align="left" colSpan=1>
                    <asp:Label id="Label7" runat="server" Width="129px" ForeColor="Maroon" Height="20px" Font-Bold="True" Text="Publicar Hoja:"></asp:Label>
                </td>
                <td style="HEIGHT: 14px" align="left">
                    <asp:UpdatePanel id="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkPublicar" runat="server" AutoPostBack="True" 
                            oncheckedchanged="chkPublicar_CheckedChanged" />
                        </contenttemplate>
                    </asp:UpdatePanel>
                </td>
                                <td>
                                </td>
                                <td>
                        <IMG id="imgUltimoCertificado" src="images/UltimoCertificado.gif" runat="server" /></td>
            </tr>
            
            </tbody>
       </table>
    
    <table id="tblPrincipal">
    <TBODY>
    <tr>
        <td vAlign=top align=left colSpan=6>
        <DIV style="BORDER-RIGHT: #d0d0d0 1px solid; BORDER-TOP: #d0d0d0 1px solid; DISPLAY: none; Z-INDEX: 2; LEFT: 612px; OVERFLOW: hidden; BORDER-LEFT: #d0d0d0 1px solid; WIDTH: 100px; BORDER-BOTTOM: #d0d0d0 1px solid; POSITION: absolute; TOP: 593px; HEIGHT: 20px; BACKGROUND-COLOR: aqua" id="Div1"></DIV>
        </td></tr><tr><td style="WIDTH: 100%" vAlign=top align=left colSpan=5><DIV style="BORDER-RIGHT: #d0d0d0 1px solid; BORDER-TOP: #d0d0d0 1px solid; DISPLAY: none; Z-INDEX: 2; LEFT: 741px; OVERFLOW: hidden; BORDER-LEFT: #d0d0d0 1px solid; WIDTH: 100px; BORDER-BOTTOM: #d0d0d0 1px solid; POSITION: absolute; TOP: 589px; HEIGHT: 20px; BACKGROUND-COLOR: aqua" id="flyestimacion"></DIV><DIV style="BORDER-RIGHT: #cccccc 1px solid; PADDING-RIGHT: 5px; BORDER-TOP: #cccccc 1px solid; DISPLAY: none; PADDING-LEFT: 5px; FONT-SIZE: 12px; Z-INDEX: 2; FILTER: progid:dximagetransform.microsoft.alpha(opacity=100); LEFT: 333px; PADDING-BOTTOM: 5px; BORDER-LEFT: #cccccc 1px solid; WIDTH: 260px; PADDING-TOP: 5px; BORDER-BOTTOM: #cccccc 1px solid; POSITION: absolute; TOP: 2289px; HEIGHT: 125px; BACKGROUND-COLOR: #ffffff" id="divFile"><table style="WIDTH: 276px" bgColor=#f1dddd><TBODY><tr><td style="HEIGHT: 9px" align=center colSpan=2><asp:Label id="lblTituloComentario" runat="server" Width="562px" ForeColor="Black" Font-Size="11pt" Height="22px" Font-Bold="True" Text="COMENTARIO CONTRATO: " Font-Underline="True" Font-Strikeout="False"></asp:Label></td></tr><tr><td style="WIDTH: 403px; HEIGHT: 9px"><asp:Label id="Label11" runat="server" Width="74px" ForeColor="Maroon" Height="22px" Font-Bold="True" Text="Comentario:"></asp:Label></td><td style="WIDTH: 197px; HEIGHT: 9px"><asp:TextBox id="TextBox5" runat="server" Width="480px" Height="128px" TextMode="MultiLine"></asp:TextBox></td></tr><tr><td style="WIDTH: 403px; HEIGHT: 14px"><asp:Label id="Label12" runat="server" Width="75px" ForeColor="Maroon" Height="22px" Font-Bold="True" Text="Documentos:"></asp:Label></td><td style="WIDTH: 197px; HEIGHT: 14px">&nbsp; 
        <asp:UpdatePanel id="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <asp:GridView id="gvArchivos" runat="server" Width="484px" EmptyDataText="No Hay Archivos Asociados" AutoGenerateColumns="False"><Columns>
        <asp:TemplateField HeaderText="Eliminar"><EditItemTemplate>
        <asp:TextBox id="TextBox1" runat="server" Text='<%# Bind("Eliminar") %>'></asp:TextBox>
        </EditItemTemplate>
        <ItemTemplate>
        &nbsp;<asp:CheckBox id="CheckBox2" runat="server" Checked='<%# Bind("Eliminar") %>'></asp:CheckBox>
        </ItemTemplate>
        </asp:TemplateField>
        <asp:HyperLinkField Target="_blank" HeaderText="Archivo" DataTextField="NombreArchivo" DataNavigateUrlFormatString="~/Documentos/{0}" DataNavigateUrlFields="NombreArchivo"></asp:HyperLinkField>
        <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="FechaAdjunto" HeaderText="Fecha Creaci&#243;n"></asp:BoundField>
        </Columns>
        </asp:GridView> 
</ContentTemplate>
</asp:UpdatePanel> </td></tr><tr><td style="WIDTH: 403px; HEIGHT: 11px"><asp:Label id="Label8" runat="server" Width="74px" ForeColor="Maroon" Height="20px" Font-Bold="True" Text="Doc. Nuevos"></asp:Label> </td><td style="WIDTH: 197px; HEIGHT: 11px">&nbsp; </td></tr><tr><td style="HEIGHT: 22px" id="TdArchivos" align=left colSpan=2 runat="server"><cc1:FileUploaderAJAX id="FileUploaderAJAX1" runat="server" Width="568px" BackColor="#F1DDDD" Height="25px" text_Add="Agregar Documento" text_Delete="Eliminar" text_Uploading="Cargando..." text_X="Eliminar Documento" MaxFiles="4" showDeletedFilesOnPostBack="False" Directory_CreateIfNotExists="False" File_RenameIfAlreadyExists="False"></cc1:FileUploaderAJAX>&nbsp; </td></tr><tr><td style="HEIGHT: 22px" align=right colSpan=2><asp:Button id="Button3" onclick="Button3_Click" runat="server" Width="61px" Text="Grabar"></asp:Button> <asp:Button id="Button4" onclick="Button4_Click" runat="server" Width="66px" Text="Cancelar"></asp:Button> </td></tr></TBODY></table></DIV><table><TBODY><tr><td style="WIDTH: 355px"><asp:ImageButton id="btnEstimacion" runat="server" Height="23px" ImageUrl="~/Images/Estimacion4.gif" BorderStyle="Solid" OnClientClick="return false;" name="btnEstimacion"></asp:ImageButton> <asp:ImageButton id="btnComentario" runat="server" Height="23px" ImageUrl="~/Images/Comentario.gif" BorderStyle="Solid" OnClientClick="return false;"></asp:ImageButton> <asp:ImageButton id="btnAprobar" onclick="btnAprobar_Click" runat="server" Height="23px" ImageUrl="~/Images/HojaAprobada4.gif" name="btnAprobar"></asp:ImageButton></td></tr></TBODY></table><asp:GridView id="gdItemHoja" runat="server" Width="100%" DataSourceID="SqlDataSource2" Height="163px" OnSelectedIndexChanged="gdItemHoja_SelectedIndexChanged" OnSelectedIndexChanging="gdItemHoja_SelectedIndexChanging" PageSize="8" OnPageIndexChanging="gdItemHoja_PageIndexChanging" EmptyDataText="There are no data records to display." DataKeyNames="IdHojaDeRuta" AutoGenerateColumns="False"><Columns>
<asp:CommandField SelectImageUrl="~/Images/right_16x16Conosud.gif" ShowSelectButton="True" ButtonType="Image"></asp:CommandField>
<asp:BoundField DataField="Titulo" HeaderText="Titulo" SortExpression="Titulo"></asp:BoundField>
<asp:BoundField DataField="DocFechaEntrega" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Doc" HtmlEncode="False" SortExpression="DocFechaEntrega"></asp:BoundField>
<asp:TemplateField HeaderText="Comentario" SortExpression="DocComentario"><EditItemTemplate>
    <asp:TextBox id="TextBox1" runat="server" Text='<%# Bind("DocComentario") %>'></asp:TextBox> 
    
</EditItemTemplate>
<ItemTemplate>
    <asp:Label id="lblComentDoc" runat="server" Visible="False" Text='<%# Bind("DocComentario") %>'></asp:Label> <asp:ImageButton id="btnEditComentDoc" runat="server" Width="20px" ImageUrl="~/Images/notepad_16x16.gif" Height="20px" OnClientClick="return false;"></asp:ImageButton> 
    
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
    <asp:Label id="Label3" runat="server" Visible="False" Text='<%# Bind("HojaComentario") %>'></asp:Label> <asp:ImageButton id="btnEditComentHoja" runat="server" Width="20px" ImageUrl="~/Images/notepad.gif" Height="20px" OnClientClick="return false;"></asp:ImageButton> 
    
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
</asp:GridView> <table style="WIDTH: 100%"><TBODY><TR style="WIDTH: 100%">
     <td style="WIDTH: 100%;" align=right>
             <table cellspacing ="5px">
                <tr>
                    <td align="center" class="tdBotones">
                          <asp:Button id="btnAprobarItems"  runat="server" style=" cursor : hand;"
                            BorderColor="#6699FF" BorderStyle="Solid" BorderWidth="0px" 
                              BackColor="Transparent" Font-Bold="true" ForeColor="white"
                            Font-Names="Verdana" Font-Size="0.78em" 
                            Text="Aplicar Cambios" onclick="btnAprobarItems_Click"    />
                    </td>   
                </tr>
            </table>  
        
     </td>
        
    </tr></TBODY></table><asp:SqlDataSource id="SqlDataSource2" runat="server" OnSelected="SqlDataSource2_Selected" SelectCommand="SELECT Plantilla.Descripcion AS 'Titulo', HojasDeRuta.IdHojaDeRuta, HojasDeRuta.IdCabeceraHojaDeRuta, HojasDeRuta.IdPlanilla, HojasDeRuta.HojaComentario, HojasDeRuta.HojaFechaControlado, HojasDeRuta.HojaAprobado, HojasDeRuta.HojaFechaAprobacion, HojasDeRuta.DocComentario, HojasDeRuta.DocFechaEntrega, HojasDeRuta.AuditadoPor FROM ContratoEmpresas INNER JOIN Contrato ON ContratoEmpresas.IdContrato = Contrato.IdContrato INNER JOIN Empresa ON ContratoEmpresas.IdEmpresa = Empresa.IdEmpresa INNER JOIN CabeceraHojasDeRuta ON ContratoEmpresas.IdContratoEmpresas = CabeceraHojasDeRuta.IdContratoEmpresa INNER JOIN HojasDeRuta ON CabeceraHojasDeRuta.IdCabeceraHojasDeRuta = HojasDeRuta.IdCabeceraHojaDeRuta INNER JOIN Plantilla ON HojasDeRuta.IdPlanilla = Plantilla.IdPlantilla INNER JOIN Clasificacion ON CabeceraHojasDeRuta.IdEstado = Clasificacion.IdClasificacion WHERE (HojasDeRuta.IdCabeceraHojaDeRuta = @Id)" ConnectionString="<%$ ConnectionStrings:ConosudConnectionString %>"><SelectParameters>

<asp:QueryStringParameter QueryStringField="id" Name="id"></asp:QueryStringParameter>
</SelectParameters>
</asp:SqlDataSource> <cc2:ConfirmButtonExtender id="ConfirmButtonExtender1" runat="server" TargetControlID="btnAprobar" ConfirmText="Esta Seguro de Aprobar la Planilla"></cc2:ConfirmButtonExtender> <asp:ObjectDataSource id="ObjectDataSource2" runat="server" SelectMethod="GetDataById2" TypeName="DSConosudtableAdapters.CabeceraHojasDeRutatableAdapter" UpdateMethod="UpdateEstimacion"><UpdateParameters>
<asp:Parameter Type="String" Name="Estimacion"></asp:Parameter>
<asp:Parameter Type="DateTime" Name="FechaEstimacion"></asp:Parameter>
<asp:Parameter Type="Int32" Name="IdCabeceraHojasDeRuta"></asp:Parameter>
</UpdateParameters>
<SelectParameters>
<asp:QueryStringParameter Type="Int32" Name="idcabecera" QueryStringField="Id"></asp:QueryStringParameter>
</SelectParameters>
</asp:ObjectDataSource> <DIV style="DISPLAY: none; WIDTH: 100px; HEIGHT: 100px" id="DivEstimacion"><asp:DetailsView id="DetailsView2" runat="server" Width="125px" DataSourceID="ObjectDataSource2" BackColor="#F1DCDC" Height="50px" DataKeyNames="IdCabeceraHojasDeRuta" DefaultMode="Edit" AutoGenerateRows="False"><Fields>
    <asp:BoundField ReadOnly="True" DataField="IdCabeceraHojasDeRuta" InsertVisible="False" Visible="False" SortExpression="IdCabeceraHojasDeRuta" HeaderText="IdCabeceraHojasDeRuta"></asp:BoundField>
    <asp:BoundField DataField="IdEstado" Visible="False" SortExpression="IdEstado" HeaderText="IdEstado"></asp:BoundField>
    <asp:BoundField DataField="Periodo" Visible="False" SortExpression="Periodo" HeaderText="Periodo"></asp:BoundField>
    <asp:BoundField DataField="NroCarpeta" Visible="False" SortExpression="NroCarpeta" HeaderText="NroCarpeta"></asp:BoundField>
    <asp:BoundField DataField="IdContratoEmpresa" Visible="False" SortExpression="IdContratoEmpresa" HeaderText="IdContratoEmpresa"></asp:BoundField>
    <asp:TemplateField SortExpression="FechaEstimacion" HeaderText="FechaEstimacion"><EditItemTemplate>
    <asp:TextBox id="TxFechaEstimacion" runat="server" Text='<%# Bind("FechaEstimacion") %>'></asp:TextBox> <asp:ImageButton id="ImageEditFechaEstimacion" runat="Server" ImageUrl="~/Images/Calendar_scheduleHS.png" AlternateText="Clic para mostar Calendario" Width="19px"></asp:ImageButton> <BR /><ajaxToolkit:MaskedEditExtender id="MaskedEditExtender1" runat="server" TargetControlID="TxFechaEstimacion" MaskType="Date" CultureName="es-AR" Mask="99/99/9999"></ajaxToolkit:MaskedEditExtender><ajaxToolkit:CalendarExtender id="CalendarInicial" runat="server" TargetControlID="TxfechaEstimacion" PopupButtonID="ImageEditFechaEstimacion" Format="dd/MM/yyyy" Enabled="True"></ajaxToolkit:CalendarExtender>&nbsp; 
    </EditItemTemplate>
    <InsertItemTemplate>
    <asp:TextBox id="Txfecha" runat="server" Text='<%# Bind("FechaEstimacion") %>'></asp:TextBox>&nbsp;&nbsp; 
    </InsertItemTemplate>
    <HeaderStyle ForeColor="Maroon" Font-Bold="True"></HeaderStyle>
    <ItemTemplate>
    <asp:Label id="Label2" runat="server" Text='<%# Bind("FechaEstimacion") %>'></asp:Label> 
    </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField SortExpression="Estimacion" HeaderText="Estimacion"><EditItemTemplate>
    <asp:TextBox id="TextBox1" runat="server" Width="453px" Height="182px" Text='<%# Bind("Estimacion") %>' TextMode="MultiLine"></asp:TextBox>
    </EditItemTemplate>
    <InsertItemTemplate>
    <asp:TextBox id="TextBox1" runat="server" Text='<%# Bind("Estimacion") %>'></asp:TextBox>
    </InsertItemTemplate>

    <HeaderStyle ForeColor="Maroon" Font-Bold="True"></HeaderStyle>
    <ItemTemplate>
    <asp:Label id="Label1" runat="server" Text='<%# Bind("Estimacion") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    <asp:CheckBoxField DataField="Publicar" Visible="False" SortExpression="Publicar" HeaderText="Publicar"></asp:CheckBoxField>
    <asp:CommandField CancelImageUrl="~/Images/delete_16x16.gif" UpdateImageUrl="~/Images/ok_16x16.gif" ButtonType="Image" ShowEditButton="True"></asp:CommandField>
    </Fields>
</asp:DetailsView> </DIV><DIV style="BORDER-RIGHT: #d0d0d0 1px solid; BORDER-TOP: #d0d0d0 1px solid; DISPLAY: none; Z-INDEX: 2; OVERFLOW: hidden; BORDER-LEFT: #d0d0d0 1px solid; BORDER-BOTTOM: #d0d0d0 1px solid; BACKGROUND-COLOR: #ffffff" id="flyout"></DIV><DIV style="BORDER-RIGHT: #cccccc 1px solid; PADDING-RIGHT: 5px; BORDER-TOP: #cccccc 1px solid; DISPLAY: none; PADDING-LEFT: 5px; FONT-SIZE: 12px; Z-INDEX: 2; FILTER: progid:dximagetransform.microsoft.alpha(opacity=100); LEFT: 189px; PADDING-BOTTOM: 5px; BORDER-LEFT: #cccccc 1px solid; WIDTH: 250px; PADDING-TOP: 5px; BORDER-BOTTOM: #cccccc 1px solid; TOP: 455px; HEIGHT: 457px; BACKGROUND-COLOR: #ffffff; opacity: 0" id="info"><DIV style="Z-INDEX: 2"><asp:DetailsView id="DetailsView1" runat="server" Width="595px" DataSourceID="ObjectDataSource1" BackColor="#F1DCDC" DataKeyNames="IdHojadeRuta" DefaultMode="Edit" AutoGenerateRows="False" OnDataBound="DetailsView1_DataBound" OnItemUpdating="DetailsView1_ItemUpdating" OnItemUpdated="DetailsView1_ItemUpdated"><Fields>
    <asp:BoundField ReadOnly="True" DataField="IdHojadeRuta" Visible="False" SortExpression="IdHojadeRuta" HeaderText="IdHojadeRuta"></asp:BoundField>
    <asp:BoundField DataField="IdContratoEmpresas" Visible="False" SortExpression="IdContratoEmpresas" HeaderText="IdContratoEmpresas"></asp:BoundField>
    <asp:TemplateField SortExpression="HojaFechaControlado" HeaderText="Ultimo Control"><EditItemTemplate>
    <asp:TextBox id="txtFechaUltimoControl" runat="server" Width="90px" Text='<%# Bind("HojaFechaControlado") %>'></asp:TextBox><asp:ImageButton id="Image2" runat="Server" Width="19px" ImageUrl="~/Images/Calendar_scheduleHS.png" AlternateText="Clic para mostar Calendario"></asp:ImageButton>&nbsp;&nbsp; <ajaxToolkit:MaskedEditExtender id="MaskedEditExtender3" runat="server" TargetControlID="txtFechaUltimoControl" MaskType="Date" CultureName="es-AR" OnInvalidCssClass="MaskedEditError" OnFocusCssClass="MaskedEditFocus" Mask="99/99/9999" ErrorTooltipEnabled="True" DisplayMoney="Left" AcceptNegative="Left"></ajaxToolkit:MaskedEditExtender><ajaxToolkit:MaskedEditValidator id="MaskedEditValidator3" runat="server" TooltipMessage="Debe ingresar la Fecha del último control" InvalidValueMessage="La Fecha Ingrersada es Inválida" InvalidValueBlurredMessage="*" Display="Dynamic" ControlToValidate="txtFechaUltimoControl" ControlExtender="MaskedEditExtender3"></ajaxToolkit:MaskedEditValidator>&nbsp; <ajaxToolkit:CalendarExtender id="CalendarExtender2" runat="server" TargetControlID="txtFechaUltimoControl" PopupButtonID="Image2" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender>
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
    <asp:TextBox id="txtFechaAprobacion" runat="server" Width="84px" Text='<%# Bind("HojaFechaAprobacion", "{0:d}") %>' Enabled="False"></asp:TextBox> <asp:ImageButton id="Image1" runat="Server" Visible="False" Width="19px" AlternateText="Clic para mostar Calendario" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> <ajaxToolkit:MaskedEditExtender id="MaskedEditExtender2" runat="server" TargetControlID="txtFechaAprobacion" MaskType="Date" CultureName="es-AR" OnInvalidCssClass="MaskedEditError" OnFocusCssClass="MaskedEditFocus" Mask="99/99/9999" ErrorTooltipEnabled="True" DisplayMoney="Left" AcceptNegative="Left"></ajaxToolkit:MaskedEditExtender> <ajaxToolkit:MaskedEditValidator id="MaskedEditValidator2" runat="server" TooltipMessage="Debe ingresar la Fecha de aprobación del Item" InvalidValueMessage="La Fecha Ingrersada es Inválida" InvalidValueBlurredMessage="*" Display="Dynamic" ControlToValidate="txtFechaAprobacion" ControlExtender="MaskedEditExtender2"></ajaxToolkit:MaskedEditValidator> <ajaxToolkit:CalendarExtender id="CalendarExtender1" runat="server" TargetControlID="txtFechaAprobacion" PopupButtonID="Image1" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender> 
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
    <asp:BoundField DataField="IdPlantilla" Visible="False" SortExpression="IdPlantilla" HeaderText="IdPlantilla"></asp:BoundField>
    <asp:BoundField DataField="IdEstado" Visible="False" SortExpression="IdEstado" HeaderText="IdEstado"></asp:BoundField>
    <asp:TemplateField ShowHeader="False"><EditItemTemplate>
    <asp:ImageButton id="ImageButton1" runat="server" ImageUrl="~/Images/ok_16x16.gif" Text="Update" CausesValidation="True" CommandName="Update"></asp:ImageButton>&nbsp;<asp:ImageButton id="btnCancelar" runat="server" ImageUrl="~/Images/delete_16x16.gif" Text="Cancel" OnClientClick="CerrarDiv();return false;" CausesValidation="False" CommandName="Cancel"></asp:ImageButton>
    </EditItemTemplate>
    <ItemTemplate>
    <asp:Button id="Button1" runat="server" Text="Edit" CausesValidation="False" CommandName="Edit"></asp:Button> 
    </ItemTemplate>
    </asp:TemplateField>
    <asp:BoundField DataField="AuditadoPor" Visible="False" SortExpression="AuditadoPor" HeaderText="AuditadoPor"></asp:BoundField>
    <asp:TemplateField InsertVisible="False" ShowHeader="False"><EditItemTemplate>
    &nbsp;<asp:ImageButton id="btnCerrar" runat="server" ImageUrl="~/Images/delete_16x16.gif" Text="Cancel" OnClientClick="CerrarDiv();return false;" CausesValidation="False" CommandName="Cancel"></asp:ImageButton>
    </EditItemTemplate>
    </asp:TemplateField>
    </Fields>
</asp:DetailsView> <asp:ObjectDataSource id="ObjectDataSource1" runat="server"  SelectMethod="GetDataById" TypeName="DSConosudtableAdapters.HojasDeRutatableAdapter" UpdateMethod="UpdateHoja" OnUpdating="ObjectDataSource1_Updating" InsertMethod="Insert" DeleteMethod="Delete"><DeleteParameters>
<asp:Parameter Type="Int64" Name="Original_IdHojaDeRuta"></asp:Parameter>
</DeleteParameters>
<UpdateParameters>
<asp:Parameter Type="String" Name="HojaComentario"></asp:Parameter>
<asp:Parameter Type="Boolean" Name="HojaAprobado"></asp:Parameter>
<asp:Parameter Type="DateTime" Name="HojaFechaAprobacion"></asp:Parameter>
<asp:Parameter Type="String" Name="AuditadoPor"></asp:Parameter>
<asp:Parameter Type="DateTime" Name="HojaFechaControlado"></asp:Parameter>
<asp:Parameter Type="Int32" Name="IdHojaDeRuta"></asp:Parameter>
</UpdateParameters>
<SelectParameters>
<asp:ControlParameter PropertyName="SelectedValue" Type="Int32" Name="Id" ControlID="gdItemHoja"></asp:ControlParameter>
</SelectParameters>
<InsertParameters>
<asp:Parameter Type="Int64" Name="IdCabeceraHojaDeRuta"></asp:Parameter>
<asp:Parameter Type="Int64" Name="IdPlanilla"></asp:Parameter>
<asp:Parameter Type="String" Name="HojaComentario"></asp:Parameter>
<asp:Parameter Type="DateTime" Name="HojaFechaControlado"></asp:Parameter>
<asp:Parameter Type="Boolean" Name="HojaAprobado"></asp:Parameter>
<asp:Parameter Type="DateTime" Name="HojaFechaAprobacion"></asp:Parameter>
<asp:Parameter Type="String" Name="DocComentario"></asp:Parameter>
<asp:Parameter Type="DateTime" Name="DocFechaEntrega"></asp:Parameter>
<asp:Parameter Type="Int64" Name="AuditadoPor"></asp:Parameter>
</InsertParameters>
</asp:ObjectDataSource>&nbsp;&nbsp; </DIV></DIV><DIV style="BORDER-RIGHT: #d0d0d0 1px solid; BORDER-TOP: #d0d0d0 1px solid; DISPLAY: none; Z-INDEX: 2; OVERFLOW: hidden; BORDER-LEFT: #d0d0d0 1px solid; BORDER-BOTTOM: #d0d0d0 1px solid; BACKGROUND-COLOR: #ffffff" id="flyout1"></DIV><DIV style="BORDER-RIGHT: #cccccc 1px solid; PADDING-RIGHT: 5px; BORDER-TOP: #cccccc 1px solid; DISPLAY: none; PADDING-LEFT: 5px; FONT-SIZE: 12px; Z-INDEX: 2; LEFT: 32px; PADDING-BOTTOM: 5px; BORDER-LEFT: #cccccc 1px solid; WIDTH: 250px; PADDING-TOP: 5px; BORDER-BOTTOM: #cccccc 1px solid; TOP: 936px; HEIGHT: 322px; BACKGROUND-COLOR: #f1dcdc" id="info1"><DIV style="WIDTH: 577px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; TEXT-ALIGN: right; BORDER-BOTTOM-STYLE: none"><asp:ImageButton id="btnCerrarComentDoc" runat="server" Width="16px" BackColor="#E0E0E0" Height="16px" ImageUrl="~/Images/delete_16x16.gif" BorderStyle="Solid" OnClientClick="CerrarDivCom();return false;" BorderWidth="1px" BorderColor="Black"></asp:ImageButton> </DIV><DIV><asp:TextBox id="txtComentDoc" runat="server" Width="571px" Height="280px" TextMode="MultiLine" ReadOnly="True"></asp:TextBox> </DIV></DIV></td></tr></TBODY></table><asp:SqlDataSource id="SqlDataSource1" runat="server" SelectCommand="SELECT DISTINCT CASE ContratoEmpresas.EsContratista WHEN 1 THEN Empresa.RazonSocial ELSE '' END AS 'Contratista', CASE ContratoEmpresas.EsContratista WHEN 0 THEN Empresa.RazonSocial ELSE '' END AS 'Sub Contratista', Contrato.Codigo, Contrato.Servicio, Clasificacion.Descripcion AS 'Estado', CabeceraHojasDeRuta.NroCarpeta, CabeceraHojasDeRuta.Periodo, Contrato.FechaInicio, Contrato.FechaVencimiento, ContratoEmpresas.EsContratista, ContratoEmpresas.IdContratoEmpresas FROM ContratoEmpresas INNER JOIN Contrato ON ContratoEmpresas.IdContrato = Contrato.IdContrato INNER JOIN Empresa ON ContratoEmpresas.IdEmpresa = Empresa.IdEmpresa INNER JOIN CabeceraHojasDeRuta ON ContratoEmpresas.IdContratoEmpresas = CabeceraHojasDeRuta.IdContratoEmpresa INNER JOIN Clasificacion ON CabeceraHojasDeRuta.IdEstado = Clasificacion.IdClasificacion WHERE (CabeceraHojasDeRuta.IdCabeceraHojasDeRuta = @id)" ConnectionString="<%$ ConnectionStrings:ConosudConnectionString %>"><SelectParameters>
<asp:QueryStringParameter Name="id" QueryStringField="id"></asp:QueryStringParameter>
</SelectParameters>
</asp:SqlDataSource> <asp:GridView id="gdEncezado" runat="server" Width="754px" DataSourceID="SqlDataSource1" Height="1px" Visible="False" AutoGenerateColumns="False"><Columns>
<asp:BoundField DataField="Contratista" SortExpression="Contratista" HeaderText="Contratista"></asp:BoundField>
<asp:BoundField DataField="Sub Contratista" SortExpression="Sub Contratista" HeaderText="Sub Contratista"></asp:BoundField>
<asp:BoundField DataField="Codigo" SortExpression="Codigo" HeaderText="Codigo"></asp:BoundField>
<asp:BoundField DataField="Servicio" SortExpression="Servicio" HeaderText="Servicio"></asp:BoundField>
<asp:BoundField DataField="Estado" SortExpression="Estado" HeaderText="Estado"></asp:BoundField>
<asp:BoundField DataField="NroCarpeta" SortExpression="NroCarpeta" HeaderText="NroCarpeta"></asp:BoundField>
<asp:BoundField DataField="Periodo" SortExpression="Periodo" HeaderText="Periodo"></asp:BoundField>
<asp:BoundField DataField="FechaInicio" SortExpression="FechaInicio" HeaderText="FechaInicio"></asp:BoundField>
<asp:BoundField DataField="FechaVencimiento" SortExpression="FechaVencimiento" HeaderText="FechaVencimiento"></asp:BoundField>
<asp:BoundField DataField="EsContratista" SortExpression="EsContratista" HeaderText="EsContratista"></asp:BoundField>
<asp:BoundField DataField="IdContratoEmpresas" InsertVisible="False" SortExpression="IdContratoEmpresas" HeaderText="IdContratoEmpresas"></asp:BoundField>
</Columns>
</asp:GridView> </P>
</contenttemplate>

    <Triggers>
        
<asp:AsyncPostBackTrigger ControlID="gdItemHoja" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="gdItemHoja" EventName="SelectedIndexChanging"></asp:AsyncPostBackTrigger>
        
<asp:AsyncPostBackTrigger ControlID="gdItemHoja" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="gdItemHoja" EventName="SelectedIndexChanging"></asp:AsyncPostBackTrigger>
        
<asp:AsyncPostBackTrigger ControlID="gdItemHoja" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="gdItemHoja" EventName="SelectedIndexChanging"></asp:AsyncPostBackTrigger>
        
<asp:AsyncPostBackTrigger ControlID="gdItemHoja" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="gdItemHoja" EventName="SelectedIndexChanging"></asp:AsyncPostBackTrigger>
        
<asp:AsyncPostBackTrigger ControlID="gdItemHoja" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="gdItemHoja" EventName="SelectedIndexChanging"></asp:AsyncPostBackTrigger>
        
<asp:AsyncPostBackTrigger ControlID="gdItemHoja" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="gdItemHoja" EventName="SelectedIndexChanging"></asp:AsyncPostBackTrigger>
        
    </Triggers>
        <triggers>
<asp:AsyncPostBackTrigger ControlID="gdItemHoja" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="gdItemHoja" EventName="SelectedIndexChanging"></asp:AsyncPostBackTrigger>
</triggers>
    </asp:UpdatePanel>




       <DIV style="display:none;PADDING-RIGHT: 5px; PADDING-LEFT: 5px; Z-INDEX: 10; LEFT: 30px; FLOAT: left; PADDING-BOTTOM: 5px; WIDTH: 223px; PADDING-TOP: 5px; TOP: 30px" id="divLegajos">
        <asp:Panel style="Z-INDEX: 20" id="Panel6" runat="server" Width="249px">
            <asp:Panel id="Panel7" runat="server" Width="106.8%" BorderStyle="Solid" BorderWidth="2px" BorderColor="black" HorizontalAlign="Left">
                <DIV style="FONT-WEIGHT: bold; BACKGROUND-IMAGE: url(images/header-opened.png); WIDTH: 100%; CURSOR: move; HEIGHT: 21px; BACKGROUND-COLOR: #fff; TEXT-ALIGN: left">
                    <asp:Label id="lbltitulo" runat="server" ForeColor="Maroon" Width="101px" Height="20px" Font-Bold="True" Text="Total Legajos:" Font-Underline="True"></asp:Label>
                    <asp:Label id="lblTotalLegajos" runat="server" ForeColor="Black" Width="1px" Height="20px" Font-Bold="True" Text="0"></asp:Label>
                </DIV>
            </asp:Panel> 
        <asp:Panel style="OVERFLOW: scroll" id="Panel8" runat="server" Width="107%" ForeColor="whitesmoke" BackColor="#F1DCDC" Height="347px" BorderStyle="Solid" BorderWidth="2px" BorderColor="black">
        <DIV>
            <asp:GridView id="gvLegajos" runat="server" Width="277px" DataSourceID="ODSContLegajos" Height="35px" DataKeyNames="IdCabeceraHojasDeRuta" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField ReadOnly="True" DataField="IdCabeceraHojasDeRuta" InsertVisible="False" Visible="False" SortExpression="IdCabeceraHojasDeRuta" HeaderText="IdCabeceraHojasDeRuta"></asp:BoundField>
                <asp:BoundField DataField="" SortExpression="" HeaderText="N&#186; Legajo">
                <ItemStyle Height="5px"></ItemStyle>
                <HeaderStyle Height="30px"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Nombre" Visible="False" SortExpression="Nombre" HeaderText="Nombre"></asp:BoundField>
                <asp:BoundField DataField="Apellido" Visible="False" SortExpression="Apellido" HeaderText="Apellido"></asp:BoundField>
                <asp:BoundField ReadOnly="True" DataField="NombreCompleto" SortExpression="NombreCompleto" HeaderText="Nombre y Apellido"></asp:BoundField>
                </Columns>
            </asp:GridView>
         </DIV>
         </asp:Panel> 
         </asp:Panel>
      </DIV>
            <asp:ObjectDataSource id="ODSContLegajos" runat="server" TypeName="DSConosudtableAdapters.ConsultaCabeceraLegajostableAdapter" SelectMethod="GetDatalegajos" OnSelected="ODSContLegajos_Selected" OnSelecting="ODSContLegajos_Selecting" OldValuesParameterFormatString="original_{0}"><SelectParameters>
            <asp:QueryStringParameter Type="Int32" Name="cabecera" QueryStringField="Id"></asp:QueryStringParameter>
            </SelectParameters>
            </asp:ObjectDataSource>


<asp:UpdateProgress ID="UpdateProgress2" runat="server"  DisplayAfter=0 >
    <ProgressTemplate>
        <div style="left: 415px; width: 216px; line-height: 0pt; position: absolute; top: 350px;
            background-color: white; z-index: 20; border-right: #3399ff thin solid; border-top: #3399ff thin solid; vertical-align: middle; border-left: #3399ff thin solid; border-bottom: #3399ff thin solid; text-align: center;">
            <table border="0" cellpadding="0" cellspacing="0" style="height: 62px">
                <tr>
                    <td>
                        <img src="images/indicator.gif" />                
                    </td>
                </tr>
                <tr>
                    <td align="center" style="height: 31px; border-top-width: thin; border-left-width: thin; border-left-color: #6699ff; border-bottom-width: thin; border-bottom-color: #6699ff; border-top-color: #6699ff; border-right-width: thin; border-right-color: #6699ff;">
                        <asp:Label ID="lbltitulopaciente" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="8pt"
                            ForeColor="Black" Height="21px" Style="vertical-align: middle" Text="Prececando Pedido...">
                        </asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
</asp:Content>

