<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="true"
    CodeFile="TestUpLoadFile.aspx.cs" Inherits="TestUpLoadFile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="UploadFiles/CSS/uploadify.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.3.1.js" type="text/javascript"></script>
    <script src="UploadFiles/scripts/jquery.uploadify.js" type="text/javascript"></script>

<asp:FileUpload ID="FileUpload2" runat="server" Style="display: none" />


<script type = "text/javascript">
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Metodos para la gesti&oacute;n de archivo fotocopia documento
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    $(window).load(
            function () {
                $("#<%=FileUpload2.ClientID %>").fileUpload({
                    'uploader': 'UploadFiles/scripts/uploader.swf',
                    'cancelImg': 'UploadFiles/images/cancel.png',
                    'buttonImg': 'Images/adjuntar.png',
                    'wmode': 'transparent',
                    'buttonText': 'Adjuntar',
                    'script': 'UploadFiles/Upload.ashx',
                    'folder': 'ArchivosKML',
                    'fileDesc': 'Archivos kml',
                    'fileExt': '*.kml',
                    'multi': false,
                    'width': '45',
                    'auto': true,
                    'sizeLimit': 1024 * 1024 * 1.5,
                    'onComplete': TerminoUpload
                });
            }
        );


    function TerminoUpload(sender, arg, infoArchivo, DatosArchivo, aa) {

        if (DatosArchivo.split('|').length > 1) {

        }
    }

</script>  

</asp:Content>
