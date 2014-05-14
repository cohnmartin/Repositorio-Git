<%@ Page EnableEventValidation="false" Language="C#" Theme="MiTema" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="ConsultaHojaRuta.aspx.cs" Inherits="ConsultaHojaRuta"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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
        function ModeDatailView(ctr) {
            ctr.DefaultMode = 'Insert';
        }

    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="5" cellpadding="0" __designer:dtid="1125899906842629">
                <tbody>
                    <tr __designer:dtid="1125899906842630">
                        <td style="width: 241px" align="center" __designer:dtid="1125899906842631">
                            <asp:Label Style="background-image: url(images/FondoTitulos.gif); background-color: transparent"
                                ID="lblEncabezadoS" runat="server" Width="314px" ForeColor="Maroon" Font-Size="14pt"
                                Height="25px" Font-Bold="True" __designer:dtid="1125899906842632" BorderColor="Black"
                                BorderStyle="Solid" BorderWidth="1px" __designer:wfdid="w11" Text="CONSULTA HOJA DE RUTA"></asp:Label>
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:Panel ID="Panel1" runat="server" Width="385px">
                <table style="border-right: #843431 thin solid; border-top: #843431 thin solid; border-left: #843431 thin solid;
                    width: 149%; border-bottom: #843431 thin solid; background-color: #f1dcdc">
                    <tbody>
                        <tr>
                            <td style="font-weight: bold; width: 102px; color: maroon; background-color: #f1dcdc">
                                Periodo Inicial:
                            </td>
                            <td>
                                <asp:TextBox ID="txtInicial" runat="server" Width="93px" __designer:wfdid="w2" MaxLength="7"></asp:TextBox>&nbsp;<asp:ImageButton
                                    ID="ImageEditFechaInicial" runat="Server" Width="19px" __designer:wfdid="w7"
                                    ImageUrl="~/Images/Calendar_scheduleHS.png" AlternateText="Clic para mostar Calendario">
                                </asp:ImageButton>&nbsp;&nbsp;
                            </td>
                            <td style="font-weight: bold; color: maroon">
                                Periodo Final:<asp:Label ID="lblIdCabecera" runat="server" __designer:wfdid="w3"></asp:Label>
                            </td>
                            <td>
                                &nbsp;<asp:TextBox ID="txtFinal" runat="server" Width="77px" __designer:wfdid="w4"></asp:TextBox>&nbsp;<asp:ImageButton
                                    ID="ImageEditFechaFinal" runat="server" Width="19px" __designer:wfdid="w8" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                    AlternateText="Clic para mostar Calendario"></asp:ImageButton>&nbsp;
                            </td>
                            <td style="width: 100px" align="left">
                                <asp:ImageButton ID="btnBuscar" OnClick="ImageButton1_Click" runat="server" Width="82px"
                                    Height="23px" __designer:wfdid="w5" ImageUrl="~/images/Buscar6.gif" name="btnBuscar">
                                </asp:ImageButton>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
            <ajaxToolkit:DropShadowExtender ID="DropShadowExtender1" runat="server" TrackPosition="True"
                Opacity="55" TargetControlID="Panel1">
            </ajaxToolkit:DropShadowExtender>
            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" __designer:wfdid="w8"
                TargetControlID="txtFinal" Mask="99/99/9999" CultureName="es-AR" MaskType="Date">
            </ajaxToolkit:MaskedEditExtender>
            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" __designer:wfdid="w9"
                TargetControlID="txtInicial" Mask="99/99/9999" CultureName="es-AR" MaskType="Date">
            </ajaxToolkit:MaskedEditExtender>
            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" __designer:wfdid="w7"
                TargetControlID="txtFinal" Format="dd/MM/yyyy" PopupButtonID="ImageEditFechaFinal">
            </ajaxToolkit:CalendarExtender>
            <ajaxToolkit:CalendarExtender ID="CalendarInicial" runat="server" __designer:wfdid="w6"
                TargetControlID="txtInicial" Format="dd/MM/yyyy" PopupButtonID="ImageEditFechaInicial">
            </ajaxToolkit:CalendarExtender>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <br />
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" OnSelected="SqlDataSource1_Selected"
                OnSelecting="SqlDataSource1_Selecting1" SelectCommand="SELECT DISTINCT Contrato.Codigo, &#13;&#10;&#9;CASE ContratoEmpresas.EsContratista WHEN 1 THEN Co.RazonSocial ELSE Co.RazonSocial END AS 'Contratista', &#13;&#10;&#9;CASE ContratoEmpresas.EsContratista WHEN 0 THEN Empresa.RazonSocial ELSE '' END AS 'Sub Contratista', &#13;&#10;&#9;Contrato.FechaInicio, &#13;&#10;&#9;Clasificacion.Descripcion, &#13;&#10;&#9;CabeceraHojasDeRuta.NroCarpeta, &#13;&#10;&#9;CabeceraHojasDeRuta.Periodo, &#13;&#10;&#9;CabeceraHojasDeRuta.IdCabeceraHojasDeRuta &#13;&#10;FROM ContratoEmpresas &#13;&#10;&#9;INNER JOIN Contrato ON ContratoEmpresas.IdContrato = Contrato.IdContrato &#13;&#10;&#9;INNER JOIN Empresa ON ContratoEmpresas.IdEmpresa = Empresa.IdEmpresa &#13;&#10;&#9;INNER JOIN CabeceraHojasDeRuta ON ContratoEmpresas.IdContratoEmpresas = CabeceraHojasDeRuta.IdContratoEmpresa &#13;&#10;&#9;INNER JOIN HojasDeRuta ON CabeceraHojasDeRuta.IdCabeceraHojasDeRuta = HojasDeRuta.IdCabeceraHojaDeRuta &#13;&#10;&#9;INNER JOIN Plantilla ON HojasDeRuta.IdPlanilla = Plantilla.IdPlantilla &#13;&#10;&#9;INNER JOIN Clasificacion ON CabeceraHojasDeRuta.IdEstado = Clasificacion.IdClasificacion&#13;&#10;     &#9;inner join (select CE.IdContrato , E.RazonSocial from ContratoEmpresas CE inner join Empresa E on E.IdEmpresa = CE.IdEmpresa and CE.EsContratista = 1) Co on  Contrato.IdContrato = Co.IdContrato &#13;&#10;&#13;&#10;WHERE (HojasDeRuta.DocFechaEntrega IS NOT NULL) AND (HojasDeRuta.DocFechaEntrega >= @PeriodoInicial) AND (HojasDeRuta.DocFechaEntrega <= @PeriodoFinal)&#13;&#10;"
                DeleteCommand="DELETE FROM [HojadeRuta] WHERE [IdHojadeRuta] = @IdHojadeRuta"
                InsertCommand="INSERT INTO [HojadeRuta] ([IdHojadeRuta], [IdContratoEmpresas], [Comentarios], [Controlado], [FechaControlado], [IdPlantilla], [IdEstado]) VALUES (@IdHojadeRuta, @IdContratoEmpresas, @Comentarios, @Controlado, @FechaControlado, @IdPlantilla, @IdEstado)"
                ConnectionString="<%$ ConnectionStrings:ConosudConnectionString %>" UpdateCommand="UPDATE [HojadeRuta] SET [IdContratoEmpresas] = @IdContratoEmpresas, [Comentarios] = @Comentarios, [Controlado] = @Controlado, [FechaControlado] = @FechaControlado, [IdPlantilla] = @IdPlantilla, [IdEstado] = @IdEstado WHERE [IdHojadeRuta] = @IdHojadeRuta">
                <DeleteParameters>
                    <asp:Parameter Type="Int64" Name="IdHojadeRuta"></asp:Parameter>
                </DeleteParameters>
                <UpdateParameters>
                    <asp:Parameter Type="Int64" Name="IdContratoEmpresas"></asp:Parameter>
                    <asp:Parameter Type="String" Name="Comentarios"></asp:Parameter>
                    <asp:Parameter Type="Boolean" Name="Controlado"></asp:Parameter>
                    <asp:Parameter Type="DateTime" Name="FechaControlado"></asp:Parameter>
                    <asp:Parameter Type="Int64" Name="IdPlantilla"></asp:Parameter>
                    <asp:Parameter Type="Int64" Name="IdEstado"></asp:Parameter>
                    <asp:Parameter Type="Int64" Name="IdHojadeRuta"></asp:Parameter>
                </UpdateParameters>
                <SelectParameters>
                    <asp:ControlParameter PropertyName="Text" Type="DateTime" DefaultValue="01/01/1900 00:00:00"
                        Name="PeriodoInicial" ControlID="txtInicial"></asp:ControlParameter>
                    <asp:ControlParameter PropertyName="Text" Type="DateTime" DefaultValue="01/01/1900 00:00:00"
                        Name="PeriodoFinal" ControlID="txtFinal"></asp:ControlParameter>
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Type="Int64" Name="IdHojadeRuta"></asp:Parameter>
                    <asp:Parameter Type="Int64" Name="IdContratoEmpresas"></asp:Parameter>
                    <asp:Parameter Type="String" Name="Comentarios"></asp:Parameter>
                    <asp:Parameter Type="Boolean" Name="Controlado"></asp:Parameter>
                    <asp:Parameter Type="DateTime" Name="FechaControlado"></asp:Parameter>
                    <asp:Parameter Type="Int64" Name="IdPlantilla"></asp:Parameter>
                    <asp:Parameter Type="Int64" Name="IdEstado"></asp:Parameter>
                </InsertParameters>
            </asp:SqlDataSource>
            <asp:ObjectDataSource ID="ODSVisorHoja" runat="server" __designer:wfdid="w1" UpdateMethod="Update"
                OldValuesParameterFormatString="original_{0}" TypeName="DSConosudTableAdapters.ContratoEmpresasTableAdapter"
                SelectMethod="GetHojasVisor" DeleteMethod="Delete" InsertMethod="Insert">
                <DeleteParameters>
                    <asp:Parameter Name="IdContratoEmpresas" Type="Int32"></asp:Parameter>
                </DeleteParameters>
                <UpdateParameters>
                    <asp:Parameter Name="IdContrato" Type="Int64"></asp:Parameter>
                    <asp:Parameter Name="IdEmpresa" Type="Int64"></asp:Parameter>
                    <asp:Parameter Name="EsContratista" Type="Boolean"></asp:Parameter>
                    <asp:Parameter Name="IdContratoEmpresas" Type="Int32"></asp:Parameter>
                </UpdateParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtInicial" PropertyName="Text" Name="PeriodoInicial"
                        Type="DateTime"></asp:ControlParameter>
                    <asp:ControlParameter ControlID="txtFinal" PropertyName="Text" Name="PeriodoFinal"
                        Type="DateTime"></asp:ControlParameter>
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="IdContrato" Type="Int64"></asp:Parameter>
                    <asp:Parameter Name="IdEmpresa" Type="Int64"></asp:Parameter>
                    <asp:Parameter Name="EsContratista" Type="Boolean"></asp:Parameter>
                </InsertParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gdHojasRuta" runat="server" Width="784px" DataSourceID="ODSVisorHoja"
                EmptyDataText="There are no data records to display." DataKeyNames="IdCabeceraHojasDeRuta"
                AutoGenerateColumns="False" OnRowCreated="gdHojasRuta_RowCreated" OnRowDataBound="gdHojasRuta_RowDataBound"
                OnSelectedIndexChanged="gdHojasRuta_SelectedIndexChanged">
                <Columns>
                    <asp:CommandField SelectImageUrl="~/images/right_16x16Conosud.gif" SelectText="sel"
                        ShowSelectButton="True" ButtonType="Image" Visible="False">
                        <ItemStyle Width="0px"></ItemStyle>
                    </asp:CommandField>
                    <asp:BoundField DataField="Codigo" HeaderText="Codigo" SortExpression="Codigo"></asp:BoundField>
                    <asp:BoundField DataField="Contratista" HeaderText="Contratista" SortExpression="Contratista">
                    </asp:BoundField>
                    <asp:BoundField DataField="Sub Contratista" HeaderText="Sub Contratista" SortExpression="Sub Contratista">
                    </asp:BoundField>
                    <asp:BoundField DataField="Descripcion" HeaderText="Estado" SortExpression="Descripcion">
                    </asp:BoundField>
                    <asp:BoundField DataField="NroCarpeta" HeaderText="NroCarpeta" SortExpression="NroCarpeta">
                    </asp:BoundField>
                    <asp:BoundField DataField="Periodo" DataFormatString="{0:yyyy/MM}" HeaderText="Periodo"
                        HtmlEncode="False" SortExpression="Periodo"></asp:BoundField>
                    <asp:TemplateField HeaderText="" InsertVisible="False" SortExpression="IdCabeceraHojasDeRuta"
                        Visible="False">
                        <EditItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("IdCabeceraHojasDeRuta") %>' ID="Label1"></asp:Label>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Bind("IdCabeceraHojasDeRuta") %>' ID="lblId"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            &nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="btnAuditores" runat="server" __designer:wfdid="w1" OnClick="btnAuditores_Click">Editar</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <table style="width: 785px">
                <tbody>
                    <tr>
                        <td style="width: 784px; height: 37px" align="right">
                            <ajaxToolkit:AnimationExtender ID="AEAuditores" runat="server" TargetControlID="lblIdCabecera">
                                <Animations>
            <OnClick>
                    <Sequence>
                        
                        
                        <ScriptAction Script="Cover($get('ctl00_ContentPlaceHolder1_lblIdCabecera'), $get('flyout'));" />
                        <StyleAction AnimationTarget="flyout" Attribute="display" Value="block"/>
                        
                        
                        <Parallel AnimationTarget="flyout" Duration=".3" Fps="25">
                            <Move Horizontal="-100" Vertical="50" />
                            <Resize Width="285" Height="281" />
                            <Color PropertyKey="backgroundColor" StartValue="#AAAAAA" EndValue="#FFFFFF" />
                        </Parallel>
                        
                        
                        
                        <ScriptAction Script="Cover($get('flyout'), $get('divAuditores'), true);" />
                        <StyleAction AnimationTarget="divAuditores" Attribute="display" Value="block"/>
                        <FadeIn AnimationTarget="divAuditores" Duration=".2"/>
                        <StyleAction AnimationTarget="flyout" Attribute="display" Value="none"/>
                        
                      


                    </Sequence>
                </OnClick></Animations>
                            </ajaxToolkit:AnimationExtender>
                            <ajaxToolkit:AnimationExtender ID="CloseAnimation" runat="server" TargetControlID="btnCerrar">
                                <Animations>
                <OnClick>
                    <Sequence AnimationTarget="divAuditores">
                        
                        <StyleAction Attribute="overflow" Value="hidden"/>
                        <Parallel Duration=".3" Fps="15">
                            <Scale ScaleFactor="0.05" Center="true" ScaleFont="true" FontUnit="px" />
                            <FadeOut />
                        </Parallel>
                        
                        
                        <StyleAction Attribute="display" Value="none"/>
                        <StyleAction Attribute="width" Value="285px"/>
                        <StyleAction Attribute="height" Value="281"/>
                        <StyleAction Attribute="fontSize" Value="13px"/>

                    </Sequence>
                </OnClick>
                                </Animations>
                            </ajaxToolkit:AnimationExtender>
                            <asp:ImageButton ID="igbExcel" OnClick="igbExcel_Click" runat="server" ImageUrl="~/images/Excel.GIF">
                            </asp:ImageButton>
                            <asp:ImageButton Style="cursor: hand" ID="btnEditar" OnClick="btnEditar_Click" runat="server"
                                Width="100px" Height="23px" Visible="False" ImageUrl="~/images/EditarHoja1.gif"
                                name="btnEditar"></asp:ImageButton>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div style="border-right: #d0d0d0 1px solid; border-top: #d0d0d0 1px solid; display: none;
                z-index: 2; left: 57px; overflow: hidden; border-left: #d0d0d0 1px solid; width: 100px;
                border-bottom: #d0d0d0 1px solid; position: absolute; top: 76px; height: 100px;
                background-color: aqua" id="flyout">
            </div>
            <div style="border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid;
                display: none; padding-left: 5px; font-size: 12px; z-index: 2; filter: progid:DXImageTransform.Microsoft.Alpha(opacity=100);
                padding-bottom: 5px; border-left: #cccccc 1px solid; width: 264px; padding-top: 5px;
                border-bottom: #cccccc 1px solid; height: 219px; background-color: #b5494a; text-align: center;
                opacity: 0" id="divAuditores">
                <table style="width: 273px; height: 300px">
                    <tbody>
                        <tr>
                            <td style="width: 150px" valign="top">
                                <asp:GridView ID="GridView1" runat="server" Width="275px" EmptyDataText="No Exiten Auditores Asociados"
                                    AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundField DataField="Auditor" HeaderText="Auditores Participantes" SortExpression="Auditor">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UserId" HeaderText="UserId" SortExpression="UserId" Visible="False">
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td valign="bottom" align="center">
                                <asp:Button ID="btnCerrar" runat="server" Width="66px" Text="Cerrar"></asp:Button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <asp:SqlDataSource ID="SDSAudirores" runat="server" SelectCommand="SELECT DISTINCT aspnet_Users.UserName AS Auditor, aspnet_Users.UserId FROM HojasDeRuta INNER JOIN aspnet_Users ON CONVERT (varchar(100), HojasDeRuta.AuditadoPor) = CONVERT (varchar(100), aspnet_Users.UserId) WHERE (HojasDeRuta.IdCabeceraHojaDeRuta = @Id)"
                ConnectionString="<%$ ConnectionStrings:ConosudConnectionString %>">
                <SelectParameters>
                    <asp:Parameter Name="Id"></asp:Parameter>
                </SelectParameters>
            </asp:SqlDataSource>
            &nbsp;&nbsp;
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gdHojasRuta" EventName="SelectedIndexChanged">
            </asp:AsyncPostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
    <asp:GridView ID="GridView2" runat="server" Visible="False">
    </asp:GridView>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <div style="left: 415px; width: 216px; line-height: 0pt; position: absolute; top: 350px;
                background-color: white; z-index: 20; border-right: #3399ff thin solid; border-top: #3399ff thin solid;
                vertical-align: middle; border-left: #3399ff thin solid; border-bottom: #3399ff thin solid;
                text-align: center;">
                <table border="0" cellpadding="0" cellspacing="0" style="height: 62px">
                    <tr>
                        <td>
                            <img src="images/indicator.gif" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="height: 31px; border-top-width: thin; border-left-width: thin;
                            border-left-color: #6699ff; border-bottom-width: thin; border-bottom-color: #6699ff;
                            border-top-color: #6699ff; border-right-width: thin; border-right-color: #6699ff;">
                            <asp:Label ID="lbltitulopaciente" runat="server" Font-Bold="True" Font-Names="Verdana"
                                Font-Size="8pt" ForeColor="Black" Height="21px" Style="vertical-align: middle"
                                Text="Prececando Consulta...">
                            </asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
