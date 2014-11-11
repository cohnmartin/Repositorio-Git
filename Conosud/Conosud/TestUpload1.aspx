<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestUpload1.aspx.cs" Inherits="TestUpload1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="UploadFiles/CSS/uploadify.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.3.1.js" type="text/javascript"></script>
    <script src="UploadFiles/scripts/jquery.uploadify.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:FileUpload ID="FileUpload2" runat="server" Style="display: none" />
    </form>
</body>
<script type="text/javascript">

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
                    'folder': 'UploadFiles',
                    'fileDesc': 'Archivos Imagen',
                    'fileExt': '*.jpg;*.png',
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
</html>
