<%@ Page Language="C#" Theme="MiTema" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="publicaciones.aspx.cs" Inherits="publicaciones" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script type="text/javascript" language="javascript">
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

    <table cellpadding="0" cellspacing="5">
        <tr>
            <td align="center" style="width: 241px">
                <asp:Label ID="lblEncabezadoS" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Font-Bold="True" Font-Size="14pt" ForeColor="Maroon" Height="25px"
                    Style="background-image: url(images/FondoTitulos.gif); background-color: transparent"
                    Text="PUBLICACION DE HOJAS DE RUTA" Width="368px"></asp:Label></td>
        </tr>
    </table>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>

    <asp:UpdateProgress id="UpdateProgress1" runat="server" __designer:wfdid="w4" DisplayAfter="200">
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

 

<TABLE style="WIDTH: 765px"><TBODY><TR><TD style="HEIGHT: 27px" align=center colSpan=3>
<TABLE style="BORDER-RIGHT: #843431 thin solid; BORDER-TOP: #843431 thin solid; BORDER-LEFT: #843431 thin solid; WIDTH: 279px; BORDER-BOTTOM: #843431 thin solid; BACKGROUND-COLOR: #f1dcdc" cellSpacing=0 cellPadding=0>
<TBODY><TR><TD>
<asp:Label style="TEXT-ALIGN: center" id="LbPeriodo" runat="server" Width="73px" Text="Periodo:" Height="22px" Font-Bold="True" ForeColor="Maroon"></asp:Label></TD>
<TD align=left>
             <table >
                <tr>
                <td>
                    <asp:TextBox id="TxPeriodo" runat="server" Width="61px">
                    </asp:TextBox>                
                </td>
                    <td align="center" class="tdBotones">
                          <asp:Button id="btnBuscar"  runat="server" style=" cursor : hand;"
                            BorderColor="#6699FF" BorderStyle="Solid" BorderWidth="0px" 
                              BackColor="Transparent" Font-Bold="true" ForeColor="white"
                            Font-Names="Verdana" Font-Size="0.78em" 
                            Text="Buscar" onclick="btnBuscar_Click"  />
                    </td>   
                </tr>
            </table>  
<ajaxToolkit:ValidatorCalloutExtender id="ValidatorCalloutExtender1" runat="server" __designer:wfdid="w12" TargetControlID="RegularExpressionValidator1"></ajaxToolkit:ValidatorCalloutExtender><ajaxToolkit:MaskedEditExtender id="MaskedEditExtender1" runat="server" __designer:wfdid="w13" TargetControlID="TxPeriodo" ClearMaskOnLostFocus="False" Mask="9999/99"></ajaxToolkit:MaskedEditExtender><ajaxToolkit:MaskedEditValidator id="MaskedEditValidator1" runat="server" __designer:wfdid="w14" ControlToValidate="TxPeriodo" ControlExtender="MaskedEditExtender1"></ajaxToolkit:MaskedEditValidator><asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" __designer:dtid="4222124650659853" __designer:wfdid="w15" ControlToValidate="TxPeriodo" ErrorMessage="El Formato ingresado es Incorrecto. <p><b>Formato: yyyy/MM (Año/Mes)</b>" ValidationExpression="\d{4}\/\d{2}" Display="None" SetFocusOnError="True">Ingrese fecha valida</asp:RegularExpressionValidator></TD></TR></TBODY></TABLE>

</TD></TR><TR><TD style="HEIGHT: 201px" colSpan=3>&nbsp;
<DIV style="OVERFLOW: auto; WIDTH: 772px; HEIGHT: 280px">
 
                <asp:GridView id="GridView1" runat="server" Width="97%" Height="150px" 
                    AutoGenerateColumns="False" DataKeyNames="IdCabeceraHojasDeRuta"><Columns>
                        <asp:TemplateField HeaderText="IdCabeceraHojasDeRuta" InsertVisible="False" 
                            SortExpression="IdCabeceraHojasDeRuta" Visible="False">
                            <EditItemTemplate>
                                <asp:Label ID="Label1" runat="server" 
                                    Text='<%# Eval("IdCabeceraHojasDeRuta") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblId" runat="server" 
                                    Text='<%# Bind("IdCabeceraHojasDeRuta") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Height="8px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="RazonSocial" SortExpression="RazonSocial">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Eval("ObjContratoEmpresa.ObjEmpresa.RazonSocial") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("ObjContratoEmpresa.ObjEmpresa.RazonSocial") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Height="5px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IdContrato" SortExpression="IdContrato" 
                            Visible="False">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Eval("ObjContratoEmpresa.IdContrato") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("ObjContratoEmpresa.IdContrato") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="N° Contrato" SortExpression="Codigo">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox3" runat="server" Text='<%# Eval("ObjContratoEmpresa.ObjContrato.Codigo") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("ObjContratoEmpresa.ObjContrato.Codigo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Servicio" SortExpression="Servicio">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox4" runat="server" Text='<%# Eval("ObjContratoEmpresa.ObjContrato.Servicio") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Eval("ObjContratoEmpresa.ObjContrato.Servicio") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Descripcion" SortExpression="Descripcion">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox5" runat="server" Text='<%# Eval("ObjEstado.Descripcion") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("ObjEstado.Descripcion") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
<asp:TemplateField SortExpression="Publicar" HeaderText="Publicar"><EditItemTemplate>
<asp:CheckBox id="chkPublicarE" runat="server" Height="5px" Checked='<%# Bind("Publicar") %>'></asp:CheckBox>
</EditItemTemplate>

<ItemStyle Height="5px"></ItemStyle>
<ItemTemplate>
<asp:CheckBox id="chkPublicar" runat="server" Height="5px" Checked='<%# Bind("Publicar") %>'></asp:CheckBox> 
</ItemTemplate>
</asp:TemplateField>
</Columns>

<RowStyle Height="5px"></RowStyle>

<EditRowStyle Height="5px"></EditRowStyle>
</asp:GridView></DIV> </TD></TR><TR>
    <TD colSpan=3>
        <table >
                <tr>
                    <td align="center" class="tdBotones">
                          <asp:Button id="Button1"  runat="server" style=" cursor : hand;"
                            BorderColor="#6699FF" BorderStyle="Solid" BorderWidth="0px" 
                              BackColor="Transparent" Font-Bold="true" ForeColor="white"
                            Font-Names="Verdana" Font-Size="0.78em" 
                            Text="Guardar Cambios" onclick="Button1_Click"  />
                    </td>   
               </tr>
        </table>  
    </TD>

</TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>

