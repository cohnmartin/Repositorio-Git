<%@ Page Language="C#" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="Estimaciones.aspx.cs" Inherits="Estimaciones" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table id="tblEncabezado" style="border-right: #843431 thin solid; border-top: #843431 thin solid;
        border-left: #843431 thin solid; border-bottom: #843431 thin solid; background-color: #f1dcdc">
        <tr>
            <td align="right" style="width: 98px; height: 26px">
                <asp:Label ID="Label3" runat="server" Font-Bold="True" ForeColor="Maroon" Height="22px"
                    Text="Empresa:" Width="79px"></asp:Label></td>
            <td id="PrimerFila" align="left" style="width: 95px; height: 26px">
                <asp:DropDownList ID="cboEmpresas" runat="server" AutoPostBack="True" DataSourceID="ODSEmpresas"
                    DataTextField="RazonSocial" DataValueField="IdEmpresa" Width="215px">
                </asp:DropDownList></td>
            <td align="right" style="width: 34px">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Maroon" Height="22px"
                    Text="Contratos:" Width="85px"></asp:Label></td>
            <td align="left" style="width: 312px">
                <asp:DropDownList ID="cboContratos" runat="server" AutoPostBack="True" DataSourceID="ODSContratos"
                    DataTextField="Codigo" DataValueField="IdContrato" Height="22px" OnDataBound="cboContratos_DataBound"
                    OnSelectedIndexChanged="cboContratos_SelectedIndexChanged" Width="190px">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td align="right" style="width: 98px; height: 26px">
                <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="Maroon" Height="20px"
                    Text="Contratistas:" Width="85px"></asp:Label></td>
            <td align="left" style="width: 95px; height: 26px">
                <asp:DropDownList ID="cboContratistas" runat="server" AutoPostBack="True" DataSourceID="ODSContratistas"
                    DataTextField="RazonSocial" DataValueField="IdContratoEmpresas" Height="21px" OnDataBound="cboContratistas_DataBound"
                    Width="214px">
                </asp:DropDownList></td>
            <td align="right" style="width: 34px; height: 26px">
                <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="Maroon" Height="19px"
                    Text="Periodos:" Width="85px"></asp:Label></td>
            <td align="left" style="width: 312px; height: 26px">
                <asp:DropDownList ID="cboPeriodos" runat="server" AutoPostBack="True" DataSourceID="ODSPeriodos"
                    DataTextField="CustomPer" DataValueField="IdCabeceraHojasDeRuta" Height="22px"
                    OnDataBound="cboPeriodos_DataBound" OnSelectedIndexChanged="cboPeriodos_SelectedIndexChanged"
                    Width="188px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="5">
                <asp:ImageButton ID="btnBusquedaPrincipal" runat="server" Height="20px" ImageUrl="~/images/search_16x16.gif"
                    OnClick="btnBusquedaPrincipal_Click" Width="24px" /></td>
        </tr>
    </table>
    <asp:ObjectDataSource ID="ODSEmpresas" runat="server" DeleteMethod="Delete" InsertMethod="Insert"
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="DSConosudTableAdapters.EmpresaTableAdapter"
        UpdateMethod="Update">
        <DeleteParameters>
            <asp:Parameter Name="IdEmpresa" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Codigo" Type="String" />
            <asp:Parameter Name="RazonSocial" Type="String" />
            <asp:Parameter Name="CUIT" Type="String" />
            <asp:Parameter Name="FechaAlta" Type="DateTime" />
            <asp:Parameter Name="RepresentanteTecnico" Type="String" />
            <asp:Parameter Name="PrestacionEmergencia" Type="String" />
            <asp:Parameter Name="Direccion" Type="String" />
            <asp:Parameter Name="Telefono" Type="String" />
            <asp:Parameter Name="CorreoElectronico" Type="String" />
            <asp:Parameter Name="IdEmpresa" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="Codigo" Type="String" />
            <asp:Parameter Name="RazonSocial" Type="String" />
            <asp:Parameter Name="CUIT" Type="String" />
            <asp:Parameter Name="FechaAlta" Type="DateTime" />
            <asp:Parameter Name="RepresentanteTecnico" Type="String" />
            <asp:Parameter Name="PrestacionEmergencia" Type="String" />
            <asp:Parameter Name="Direccion" Type="String" />
            <asp:Parameter Name="Telefono" Type="String" />
            <asp:Parameter Name="CorreoElectronico" Type="String" />
        </InsertParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSPeriodos" runat="server" DeleteMethod="Delete" InsertMethod="Insert"
        OldValuesParameterFormatString="original_{0}" OnSelected="ODSPeriodos_Selected"
        SelectMethod="GetPeriodosDisponiblesByContrato" TypeName="DSConosudTableAdapters.CabeceraHojasDeRutaTableAdapter"
        UpdateMethod="Update">
        <DeleteParameters>
            <asp:Parameter Name="IdCabeceraHojasDeRuta" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="IdEstado" Type="Int64" />
            <asp:Parameter Name="Periodo" Type="DateTime" />
            <asp:Parameter Name="NroCarpeta" Type="Int64" />
            <asp:Parameter Name="IdContratoEmpresa" Type="Int64" />
            <asp:Parameter Name="IdCabeceraHojasDeRuta" Type="Int32" />
        </UpdateParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="cboContratos" Name="IdContrato" PropertyName="SelectedValue"
                Type="Int64" />
        </SelectParameters>
        <InsertParameters>
            <asp:Parameter Name="IdEstado" Type="Int64" />
            <asp:Parameter Name="Periodo" Type="DateTime" />
            <asp:Parameter Name="NroCarpeta" Type="Int64" />
            <asp:Parameter Name="IdContratoEmpresa" Type="Int64" />
        </InsertParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSContratos" runat="server" DeleteMethod="Delete" InsertMethod="Insert"
        OldValuesParameterFormatString="original_{0}" OnSelected="ODSContratos_Selected"
        SelectMethod="GetDataByIdEmpresa" TypeName="DSConosudTableAdapters.ContratoTableAdapter"
        UpdateMethod="Update">
        <DeleteParameters>
            <asp:Parameter Name="IdContrato" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Codigo" Type="String" />
            <asp:Parameter Name="Servicio" Type="String" />
            <asp:Parameter Name="FechaInicio" Type="DateTime" />
            <asp:Parameter Name="FechaVencimiento" Type="DateTime" />
            <asp:Parameter Name="Prorroga" Type="DateTime" />
            <asp:Parameter Name="TipoContrato" Type="Int64" />
            <asp:Parameter Name="Contratadopor" Type="Int64" />
            <asp:Parameter Name="IdContrato" Type="Int32" />
        </UpdateParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="cboEmpresas" DefaultValue="" Name="IdEmpresa" PropertyName="SelectedValue"
                Type="Int64" />
        </SelectParameters>
        <InsertParameters>
            <asp:Parameter Name="Codigo" Type="String" />
            <asp:Parameter Name="Servicio" Type="String" />
            <asp:Parameter Name="FechaInicio" Type="DateTime" />
            <asp:Parameter Name="FechaVencimiento" Type="DateTime" />
            <asp:Parameter Name="Prorroga" Type="DateTime" />
            <asp:Parameter Name="TipoContrato" Type="Int64" />
            <asp:Parameter Name="Contratadopor" Type="Int64" />
        </InsertParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSContratistas" runat="server" DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="GetContEmpresaExtend"
        TypeName="DSConosudTableAdapters.ContratoEmpresasTableAdapter" UpdateMethod="Update">
        <DeleteParameters>
            <asp:Parameter Name="IdContratoEmpresas" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="IdContrato" Type="Int64" />
            <asp:Parameter Name="IdEmpresa" Type="Int64" />
            <asp:Parameter Name="EsContratista" Type="Boolean" />
            <asp:Parameter Name="IdContratoEmpresas" Type="Int32" />
        </UpdateParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="cboContratos" DefaultValue="" Name="IdContrato"
                PropertyName="SelectedValue" Type="Int64" />
        </SelectParameters>
        <InsertParameters>
            <asp:Parameter Name="IdContrato" Type="Int64" />
            <asp:Parameter Name="IdEmpresa" Type="Int64" />
            <asp:Parameter Name="EsContratista" Type="Boolean" />
        </InsertParameters>
    </asp:ObjectDataSource>
    <br />
    <asp:ObjectDataSource ID="ObjectDataItem" runat="server" DeleteMethod="Delete" InsertMethod="Insert"
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetDataByCabecera"
        TypeName="DSConosudTableAdapters.EstimacionesTableAdapter" UpdateMethod="Update">
        <DeleteParameters>
            <asp:Parameter Name="Original_IdEstimacion" Type="Int64" />
            <asp:Parameter Name="Original_IdCabecera" Type="Int64" />
            <asp:Parameter Name="Original_FechaEstimacion" Type="DateTime" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="IdCabecera" Type="Int64" />
            <asp:Parameter Name="Estimacion" Type="String" />
            <asp:Parameter Name="FechaEstimacion" Type="DateTime" />
            <asp:Parameter Name="Original_IdEstimacion" Type="Int64" />
            <asp:Parameter Name="Original_IdCabecera" Type="Int64" />
            <asp:Parameter Name="Original_FechaEstimacion" Type="DateTime" />
            <asp:Parameter Name="IdEstimacion" Type="Int32" />
        </UpdateParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="cboPeriodos" Name="IdCabecera" PropertyName="SelectedValue"
                Type="Int64" />
        </SelectParameters>
        <InsertParameters>
            <asp:Parameter Name="IdCabecera" Type="Int64" />
            <asp:Parameter Name="Estimacion" Type="String" />
            <asp:Parameter Name="FechaEstimacion" Type="DateTime" />
        </InsertParameters>
    </asp:ObjectDataSource>
    <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" DataKeyNames="IdEstimacion"
        DataSourceID="ObjectDataItem" Height="50px" Width="125px">
        <Fields>
            <asp:BoundField DataField="IdEstimacion" HeaderText="IdEstimacion" InsertVisible="False"
                ReadOnly="True" SortExpression="IdEstimacion" />
            <asp:BoundField DataField="IdCabecera" HeaderText="IdCabecera" SortExpression="IdCabecera" />
            <asp:BoundField DataField="Estimacion" HeaderText="Estimacion" SortExpression="Estimacion" />
            <asp:BoundField DataField="FechaEstimacion" HeaderText="FechaEstimacion" SortExpression="FechaEstimacion" />
        </Fields>
    </asp:DetailsView>
    <br />
    &nbsp;
</asp:Content>

