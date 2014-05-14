<%@ Page Language="C#" Theme="MiTema" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true" CodeFile="prueba upload.aspx.cs" Inherits="prueba_upload" Title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Assembly="FUA" Namespace="Subgurim.Controles" TagPrefix="cc1" %>
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
</script>
    <asp:Button ID="btnAnimacion" runat="server" OnClientClick="return false;" Text="Button"
        Width="115px" />&nbsp;<cc2:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"  EnablePartialRendering="true">
    </cc2:ToolkitScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
    <div style="width: 250px; display: none; background-color: #ffffff; border-right: #cccccc 1px solid; padding-right: 5px; border-top: #cccccc 1px solid; padding-left: 5px; font-size: 12px; z-index: 2; filter: progid:dximagetransform.microsoft.alpha(opacity=100); left: 32px; padding-bottom: 5px; border-left: #cccccc 1px solid; padding-top: 5px; border-bottom: #cccccc 1px solid; top: 936px;" id="divFile">
        <table style="width: 353px" bgcolor="#f1dddd">
        <tr>
            <td style="width: 403px; height: 26px;">
                <cc1:FileUploaderAJAX ID="FileUploaderAJAX1" runat="server" BackColor="#F1DDDD" text_Add="Agregar"
                    text_Delete="Eliminar" text_Uploading="Cargando..." text_X="Quitar" Width="336px" Height="25px" MaxFiles="4" showDeletedFilesOnPostBack="False" Directory_CreateIfNotExists="False" File_RenameIfAlreadyExists="False" />
            </td>
        </tr>
    </table>
    </div></contenttemplate>
    </asp:UpdatePanel>
    &nbsp;
    <cc2:AnimationExtender ID="AnimationExtender3" runat="server" TargetControlID="btnAnimacion">
        <Animations>
                <OnClick>
                    <Sequence>
                        
                        <ScriptAction Script="Cover($get('ctl00_ContentPlaceHolder1_btnAnimacion'), $get('flyEstimacion'));" />

                        <StyleAction AnimationTarget="flyEstimacion" Attribute="display" Value="block"/>
                        
                        <Parallel AnimationTarget="flyEstimacion" Duration=".3" Fps="25">
                            <Move Horizontal="10" Vertical="30" />
                            <Resize Height="66px" Width="350px" />
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
    <div id="flyestimacion" style="border-right: #d0d0d0 1px solid; border-top: #d0d0d0 1px solid;
        display: none; z-index: 2; left: 569px; overflow: hidden; border-left: #d0d0d0 1px solid;
        width: 100px; border-bottom: #d0d0d0 1px solid; position: absolute; top: 368px;
        height: 20px; background-color: aqua">
    </div>
</asp:Content>

