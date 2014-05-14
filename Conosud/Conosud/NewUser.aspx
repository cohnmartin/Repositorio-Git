<%@ Page Language="C#" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="NewUser.aspx.cs" Inherits="NewUser" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
        <div style="text-align: center">
            <table style="width: 100%">
                <tr>
                    <td align="center" style="height: 76px">
                        &nbsp; &nbsp; &nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center" style="height: 308px">
                        <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" BackColor="#F1DCDC" BorderColor="#F1DCDC"
                            BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ContinueDestinationPageUrl="~/default.aspx" AnswerLabelText="Respuesta de Seguridad:" AnswerRequiredErrorMessage="Se requiere Respuesta de seguridad" CompleteSuccessText="Su cuenta ha sido exitosamente creada" ConfirmPasswordCompareErrorMessage="La contraseña y la contraseña de la confirmación deben ser iguales." ConfirmPasswordLabelText="Confirmar Clave:" ConfirmPasswordRequiredErrorMessage="Se requiere confimacion de Clave" ContinueButtonText="Continuar" CreateUserButtonText="Crear Usuario" DuplicateUserNameErrorMessage="Por favor ingrese un usuario distinto" EmailRegularExpressionErrorMessage="Por favor ingrese un nuevo e-mail" EmailRequiredErrorMessage="Se requiere e-mail" FinishCompleteButtonText="Final" FinishPreviousButtonText="Previo" InvalidPasswordErrorMessage="Mínimo de la longitud de la contraseña: {0}. caracteres No-alfanuméricos requeridos: {1}." PasswordLabelText="Clave:" PasswordRegularExpressionErrorMessage="Por favor ingrese una Clave distinta" PasswordRequiredErrorMessage="Se requiere Clave" QuestionLabelText="Pregunta para Seguridad:" QuestionRequiredErrorMessage="Se requiere Pregunta de Seguridad" RequireEmail="False" StartNextButtonText="Proximo" StepNextButtonText="Proximo" StepPreviousButtonText="Previo" UnknownErrorMessage="Su cuenta no se ha creado intentelo nuevamente" UserNameLabelText="Nombre de Usuario:" UserNameRequiredErrorMessage="Se requiere Nombre de Usuario">
                            <WizardSteps>
                                <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
                                    <CustomNavigationTemplate>
                                        <table border="0" cellspacing="5" style="width: 100%; height: 100%;">
                                            <tr align="right">
                                                <td align="right" colspan="0">
                                                    <asp:Button ID="StepNextButton" runat="server" BackColor="White" BorderColor="#CC9966"
                                                        BorderStyle="Solid" BorderWidth="1px" CommandName="MoveNext" Font-Names="Verdana"
                                                        ForeColor="#990000" Text="Crear Usuario" ValidationGroup="CreateUserWizard1" />
                                                </td>
                                            </tr>
                                        </table>
                                    </CustomNavigationTemplate>
                                    <ContentTemplate>
                                        <table border="0" style="font-size: 100%; font-family: Verdana; background-color: #f1dcdc">
                                            <tr>
                                                <td align="center" colspan="2" style="font-weight: bold; color: white; background-color: #990000">
                                                    Sign Up for Your New Account</td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Nombre de Usuario:</asp:Label></td>
                                                <td>
                                                    <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                        ErrorMessage="Se requiere Nombre de Usuario" ToolTip="Se requiere Nombre de Usuario"
                                                        ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Clave:</asp:Label></td>
                                                <td>
                                                    <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                        ErrorMessage="Se requiere Clave" ToolTip="Se requiere Clave" ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">Confirmar Clave:</asp:Label></td>
                                                <td>
                                                    <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPassword"
                                                        ErrorMessage="Se requiere confimacion de Clave" ToolTip="Se requiere confimacion de Clave"
                                                        ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" style="height: 14px">
                                                    <asp:Label ID="QuestionLabel" runat="server" AssociatedControlID="Question">Pregunta para Seguridad:</asp:Label></td>
                                                <td style="height: 14px">
                                                    <asp:TextBox ID="Question" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="QuestionRequired" runat="server" ControlToValidate="Question"
                                                        ErrorMessage="Se requiere Pregunta de Seguridad" ToolTip="Se requiere Pregunta de Seguridad"
                                                        ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="AnswerLabel" runat="server" AssociatedControlID="Answer">Respuesta de Seguridad:</asp:Label></td>
                                                <td>
                                                    <asp:TextBox ID="Answer" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="AnswerRequired" runat="server" ControlToValidate="Answer"
                                                        ErrorMessage="Se requiere Respuesta de seguridad" ToolTip="Se requiere Respuesta de seguridad"
                                                        ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password"
                                                        ControlToValidate="ConfirmPassword" Display="Dynamic" ErrorMessage="La contraseña y la contraseña de la confirmación deben ser iguales."
                                                        ValidationGroup="CreateUserWizard1"></asp:CompareValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2" style="color: red">
                                                    <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:CreateUserWizardStep>
                                <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
                                    <ContentTemplate>
                                        <table border="0" style="font-size: 100%; font-family: Verdana; background-color: #f1dcdc">
                                            <tr>
                                                <td align="center" colspan="2" style="font-weight: bold; color: white; background-color: #990000">
                                                    Complete</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Su cuenta ha sido exitosamente creada</td>
                                            </tr>
                                            <tr>
                                                <td align="right" colspan="2">
                                                    <asp:Button ID="ContinueButton" runat="server" BackColor="White" BorderColor="#CC9966"
                                                        BorderStyle="Solid" BorderWidth="1px" CausesValidation="False" CommandName="Continue"
                                                        Font-Names="Verdana" ForeColor="#990000" Text="Continuar" ValidationGroup="CreateUserWizard1" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:CompleteWizardStep>
                            </WizardSteps>
                            <SideBarStyle BackColor="#990000" Font-Size="0.9em" VerticalAlign="Top" />
                            <TitleTextStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <SideBarButtonStyle ForeColor="White" />
                            <NavigationButtonStyle BackColor="White" BorderColor="#CC9966" BorderStyle="Solid"
                                BorderWidth="1px" Font-Names="Verdana" ForeColor="#990000" />
                            <HeaderStyle BackColor="#FFCC66" BorderStyle="Solid" Font-Bold="True" Font-Size="0.9em"
                                ForeColor="#333333" HorizontalAlign="Center" BorderColor="#FFFBD6" BorderWidth="2px" />
                            <CreateUserButtonStyle BackColor="White" BorderColor="#CC9966" BorderStyle="Solid"
                                BorderWidth="1px" Font-Names="Verdana" ForeColor="#990000" />
                            <ContinueButtonStyle BackColor="White" BorderColor="#CC9966" BorderStyle="Solid"
                                BorderWidth="1px" Font-Names="Verdana" ForeColor="#990000" />
                        </asp:CreateUserWizard>
                        &nbsp; &nbsp;&nbsp;</td>
                </tr>
            </table>
        </div>
    
    </div>
</asp:Content>

