<%@ Page Language="C#" EnableEventValidation="false" Theme="MiTema" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="ABMUsuarios.aspx.cs" Inherits="PaginasDefinicion_ABMUsuarios" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="width: 100%;">
        <tr>
            <td align="center" >
                <asp:UpdatePanel ID="upGrillaPrincipal" runat="server" UpdateMode="Conditional">
                     <ContentTemplate>                
                        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                            AllowSorting="True" AutoGenerateColumns="False" 
                            DataKeyNames="IdSegUsuario" DataSourceID="LinqDataSource1" Width="499px" onrowdeleted="GridView1_RowDeleted" 
                            onrowupdated="GridView1_RowUpdated"
                            onselectedindexchanged="GridView1_SelectedIndexChanged">
                            <Columns>
                                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" 
                                    ShowSelectButton="True" ButtonType="Image" 
                                    CancelImageUrl="~/images/delete_16x16.gif" CancelText="" 
                                    DeleteImageUrl="~/images/delete_16x16.gif" DeleteText="" 
                                    EditImageUrl="~/images/edit_16x16.gif" EditText="" 
                                    InsertImageUrl="~/images/ok_16x16.gif" InsertText="" 
                                    NewImageUrl="~/images/add_16x16.gif" NewText="" 
                                    SelectImageUrl="~/images/right_16x16Conosud.gif" SelectText="" 
                                    UpdateImageUrl="~/images/ok_16x16.gif" UpdateText="" />
                                <asp:BoundField DataField="Login" HeaderText="Login" />
                                <asp:TemplateField HeaderText="Password">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Height="22px" 
                                            Text='<%# Bind("Password") %>'  Width="197px"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server"  Text='********' ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Empresa">
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("ObjEmpresa.RazonSocial") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="DropDownList1" runat="server" 
                                            DataSourceID="LinqDataSource4" DataTextField="RazonSocial" 
                                            DataValueField="IdEmpresa" Height="16px" 
                                            SelectedValue='<%# Bind("IdEmpresa") %>' Width="247px" AppendDataBoundItems="True">
                                            <asp:ListItem Value="">(ninguno)</asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                     </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="upDetail" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" DataKeyNames="IdSegUsuario"
                            DataSourceID="LinqDataSource1" Height="50px" Width="499px" OnItemInserted="DetailsView1_ItemInserted"
                            OnItemCommand="DetailsView1_ItemCommand">
                            <FieldHeaderStyle BorderColor="black" BorderStyle="Solid" BorderWidth="1px" />
                            <Fields>
                                <asp:BoundField DataField="Login" HeaderText="Login" SortExpression="Login" Visible="false">
                                    <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" BackColor="#91A6BD" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Password" SortExpression="Password" Visible="false">
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Password") %>' TextMode="Password"
                                            Width="220px"></asp:TextBox>
                                    </InsertItemTemplate>
                                    <ItemStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" BackColor="#91A6BD" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Empresa" Visible="false">
                                    <InsertItemTemplate>
                                        <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="LinqDataSource4"
                                            DataTextField="RazonSocial" DataValueField="IdEmpresa" Height="16px" SelectedValue='<%# Bind("IdEmpresa") %>'
                                            Width="247px" AppendDataBoundItems="True">
                                            <asp:ListItem Value="">(ninguno)</asp:ListItem>
                                        </asp:DropDownList>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="New"
                                            ImageUrl="~/images/add_16x16.gif" Text="" />
                                    </ItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="True" CommandName="Insert"
                                            ImageUrl="~/images/ok_16x16.gif" Text="" />
                                        &nbsp;<asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False"
                                            CommandName="Cancel" ImageUrl="~/images/delete_16x16.gif" Text="" />
                                    </InsertItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" BackColor="#EAEAEA" />
                                </asp:TemplateField>
                            </Fields>
                            <InsertRowStyle BackColor="#E1F0FF" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                HorizontalAlign="Left" />
                        </asp:DetailsView>
                        <asp:LinqDataSource ID="LinqDataSource1" runat="server" ContextTypeName="ConosudDataContext"
                            EnableDelete="True" EnableInsert="True" EnableUpdate="True" OrderBy="Login" TableName="SegUsuarios">
                        </asp:LinqDataSource>
                        <asp:LinqDataSource ID="LinqDataSource4" runat="server" ContextTypeName="ConosudDataContext"
                            Select="new (IdEmpresa, RazonSocial)" TableName="Empresas">
                        </asp:LinqDataSource>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="left" >
                <div style="font-family: Verdana; font-size: 10px; font-weight: bold; font-style: normal; text-transform: capitalize">Roles asignado x usuario</div>
            </td>
        </tr>
        <tr>
            <td align="center" >
                 <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                      <ContentTemplate>                
                        <asp:DropDownList ID="CboRoles" runat="server" 
                            DataSourceID="LinqDataSource3" DataTextField="Descripcion" 
                            DataValueField="IdSegRol" Height="25px" Width="233px">
                        </asp:DropDownList>
                        <asp:ImageButton ID="ImageButton1" runat="server" 
                            ImageUrl="~/images/add_16x16.gif" onclick="ImageButton1_Click" />
                        <asp:LinqDataSource ID="LinqDataSource3" runat="server" 
                            ContextTypeName="ConosudDataContext" TableName="SegRols">
                        </asp:LinqDataSource>
                      </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center" >
             <asp:UpdatePanel ID="UpRoles" runat="server" UpdateMode="Conditional">
                  <ContentTemplate>                
                        <asp:GridView ID="GridView2" runat="server" AllowPaging="True" 
                            AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" 
                            DataKeyNames="IdSegUsuarioRol" DataSourceID="LinqDataSource2" ForeColor="#333333" 
                            BorderColor="#5D7B9D" BorderStyle="Solid" BorderWidth="1px"
                            GridLines="None" Width="497px">
                            <Columns>
                                <asp:CommandField ShowDeleteButton="True" ButtonType="Image" 
                                    CancelImageUrl="~/images/delete_16x16.gif" CancelText="" 
                                    DeleteImageUrl="~/images/delete_16x16.gif" />
                                <asp:TemplateField HeaderText="Rol Asignado" SortExpression="Rol">
                                    <EditItemTemplate>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("ObjSegRol.Descripcion") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        
                        <asp:LinqDataSource ID="LinqDataSource2" runat="server" 
                            ContextTypeName="ConosudDataContext" EnableDelete="True" 
                            TableName="SegUsuarioRols" onselecting="LinqDataSource2_Selecting" 
                            Where="Usuario == @Usuario">
                            <WhereParameters>
                                <asp:ControlParameter ControlID="GridView1" Name="Usuario" 
                                    PropertyName="SelectedValue" Type="Int64" />
                            </WhereParameters>
                        </asp:LinqDataSource>
                  </ContentTemplate>
                  <Triggers>
                      <asp:AsyncPostBackTrigger ControlID="GridView1" 
                          EventName="SelectedIndexChanged" />
                          
                      <asp:AsyncPostBackTrigger ControlID="ImageButton1" 
                          EventName="Click" />
                          
                  </Triggers>
            </asp:UpdatePanel>
            </td>
        </tr>
    </table>

</asp:Content>