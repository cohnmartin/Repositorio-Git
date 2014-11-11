<%@ Page Language="C#" Theme="MiTema" AutoEventWireup="true" CodeFile="SelContratoLegajos.aspx.cs" Inherits="SelContratoLegajos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" Content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <base target="_self">
</head>
<script type="text/javascript" language="javascript">
	function DevolverValor()
	{   
    	var empcont = document.forms[0].item('txtSeleccionado').value;
    	var periodo = document.forms[0].item('txtPeriodo').value;
	    returnValue = empcont + '/' + periodo; 
    }		
</script>
<body>
    <form id="form1" runat="server">
        <cc2:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering =true>
        </cc2:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

                <div id="CopiarLegajos">
                    <table id="" bgcolor="#f1dcdc" style="width: 433px; height: 227px">
                        <tr>
                            <td style="height: 24px">
                                <asp:Label ID="Label1" runat="server" Text="Codigos Contratos" Font-Bold="True"></asp:Label></td>
                            <td style="width: 491px; height: 24px;">
                                &nbsp;&nbsp;
                                <div style="width: 269px; height: 11px">
                                    <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="Periodos:"></asp:Label>
                                <asp:DropDownList ID="DDLPeriodosDesde" runat="server" DataSourceID="ODSPeriodos"
                                    DataTextField="Periodo" DataTextFormatString="{0:yyyy/MM}" DataValueField="IdCabeceraHojasDeRuta" Width="120px" AutoPostBack="True" OnSelectedIndexChanged="DDLPeriodosDesde_SelectedIndexChanged" OnDataBound="DDLPeriodosDesde_DataBound">
                                </asp:DropDownList></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 249px; height: 112px">
                                &nbsp;<asp:GridView ID="gvContratos" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    DataKeyNames="IdContratoEmpresas" DataSourceID="ODSContratosNoVig" Width="226px" Height="208px" OnSelectedIndexChanged="gvContratos_SelectedIndexChanged">
                                    <Columns>
                                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/images/right_16x16Conosud.gif"
                                            ShowSelectButton="True" />
                                        <asp:BoundField DataField="Codigo" HeaderText="Codigo" SortExpression="Codigo" />
                                        <asp:BoundField DataField="Servicio" HeaderText="Servicio" SortExpression="Servicio" />
                                    </Columns>
                                </asp:GridView>
                                <input id="txtSeleccionado" type="text" runat="server" style="width: 0px; height: 0px" />
                                <input id="txtPeriodo" type="text" runat="server" style="width: 0px; height: 0px" /></td>
                            <td style="width: 491px; height: 112px">
                                <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Legajos Asignados:"></asp:Label>
                                <asp:GridView ID="gvLegajos" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    DataKeyNames="IdContEmpLegajos" DataSourceID="ODSContEmpLeg" Width="266px">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Legajos" SortExpression="IdLegajos">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="DropDownList2" runat="server" DataSourceID="ODSLegajos" DataTextField="NombreCompleto"
                                                    DataValueField="IdLegajos" Enabled="False" SelectedValue='<%# Eval("IdLegajos") %>'
                                                    Width="248px">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 249px">
                                <asp:ObjectDataSource ID="ODSContratosNoVig" runat="server" OldValuesParameterFormatString="original_{0}"
                        SelectMethod="GetContratosVigxEmpresa" TypeName="DSConosudTableAdapters.ContratosVigentesTableAdapter">
                                    <SelectParameters>
                                        <asp:QueryStringParameter Name="IdEmpresa" QueryStringField="IdEmp" Type="Int64" />
                                    </SelectParameters>
                    </asp:ObjectDataSource>
                                <asp:ObjectDataSource ID="ODSContEmpLeg" runat="server" DeleteMethod="Delete" InsertMethod="Insert"
                                    OldValuesParameterFormatString="original_{0}" SelectMethod="GetDataByIdContratoEmpresas"
                                    TypeName="DSConosudTableAdapters.ContEmpLegajosTableAdapter" UpdateMethod="Update">
                                    <DeleteParameters>
                                        <asp:Parameter Name="IdContEmpLegajos" Type="Int32" />
                                    </DeleteParameters>
                                    <UpdateParameters>
                                        <asp:Parameter Name="IdContratoEmpresas" Type="Int64" />
                                        <asp:Parameter Name="IdLegajos" Type="Int64" />
                                        <asp:Parameter Name="IdCabecerHojaRuta" Type="Int64" />
                                        <asp:Parameter Name="IdContEmpLegajos" Type="Int32" />
                                    </UpdateParameters>
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="gvContratos" Name="IdContratoEmpresas" PropertyName="SelectedValue"
                                            Type="Int64" />
                                        <asp:ControlParameter ControlID="DDLPeriodosDesde" Name="IdCabecerHojaRuta" PropertyName="SelectedValue"
                                            Type="Int64" />
                                    </SelectParameters>
                                    <InsertParameters>
                                        <asp:Parameter Name="IdContratoEmpresas" Type="Int64" />
                                        <asp:Parameter Name="IdLegajos" Type="Int64" />
                                        <asp:Parameter Name="IdCabecerHojaRuta" Type="Int64" />
                                    </InsertParameters>
                                </asp:ObjectDataSource>
                                <asp:ObjectDataSource ID="ODSPeriodos" runat="server" DeleteMethod="Delete" InsertMethod="Insert"
                                    OldValuesParameterFormatString="original_{0}" SelectMethod="GetDataByPeriodosDispXContEmp"
                                    TypeName="DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter" UpdateMethod="Update" OnSelected="ODSPeriodos_Selected">
                                    <DeleteParameters>
                                        <asp:Parameter Name="IdCabeceraHojasDeRuta" Type="Int32" />
                                    </DeleteParameters>
                                    <UpdateParameters>
                                        <asp:Parameter Name="IdEstado" Type="Int64" />
                                        <asp:Parameter Name="Periodo" Type="DateTime" />
                                        <asp:Parameter Name="NroCarpeta" Type="Int64" />
                                        <asp:Parameter Name="IdContratoEmpresa" Type="Int64" />
                                        <asp:Parameter Name="Estimacion" Type="String" />
                                        <asp:Parameter Name="FechaEstimacion" Type="DateTime" />
                                        <asp:Parameter Name="FechaAprobacion" Type="DateTime" />
                                        <asp:Parameter Name="IdCabeceraHojasDeRuta" Type="Int32" />
                                    </UpdateParameters>
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="gvContratos" Name="IdContratoEmpresas" PropertyName="SelectedValue"
                                            Type="Int32" />
                                    </SelectParameters>
                                    <InsertParameters>
                                        <asp:Parameter Name="IdEstado" Type="Int64" />
                                        <asp:Parameter Name="Periodo" Type="DateTime" />
                                        <asp:Parameter Name="NroCarpeta" Type="Int64" />
                                        <asp:Parameter Name="IdContratoEmpresa" Type="Int64" />
                                        <asp:Parameter Name="Estimacion" Type="String" />
                                        <asp:Parameter Name="FechaEstimacion" Type="DateTime" />
                                        <asp:Parameter Name="FechaAprobacion" Type="DateTime" />
                                    </InsertParameters>
                                </asp:ObjectDataSource><asp:ObjectDataSource ID="ODSLegajos" runat="server"
                                    OldValuesParameterFormatString="original_{0}" SelectMethod="GetData"
                                    TypeName="DSConosudTableAdapters.LegajosTableAdapter" OnSelected="ODSPeriodos_Selected">
                                </asp:ObjectDataSource>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 491px; text-align: right">
                                &nbsp;<asp:ImageButton ID="ibCancelar" runat="server" Height="20px" ImageUrl="~/images/delete_16x16.gif"
                                    name="btnEditar" OnClick="ibCancelar_Click" Width="20px" />
                                <asp:ImageButton ID="ibOk" runat="server" Height="20px" ImageUrl="~/images/ok_20x20.gif"
                                    name="btnEditar" OnClick="ibOk_Click" Width="20px" />
                            </td>
                        </tr>
                    </table>
                </div>
                </ContentTemplate>
            </asp:UpdatePanel>
    </form>
</body>
</html>
