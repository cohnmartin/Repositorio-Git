<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PruebaGrilla.aspx.cs" Inherits="PruebaGrilla" %>

<%@ Register Assembly="ControlsAjaxNotti" Namespace="ControlsAjaxNotti" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Grilla JavaScript</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7">
</head>
<script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
<script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>
<script type="text/javascript">
    var map;
    var gdir;
    var geocoder = null;
    var addressMarker;
    var directionsService =  new google.maps.DirectionsService()
    
    function initialize() {
        var origin = new google.maps.LatLng(53.348172, -6.297285);
        var destination = new google.maps.LatLng(53.355502, -6.30557);
        directionsDisplay = new google.maps.DirectionsRenderer();

        map = new google.maps.Map(
      document.getElementById('google_map'), {
          center: origin,
          zoom: 10,
          mapTypeId: google.maps.MapTypeId.ROADMAP
      });

        directionsDisplay.setPanel(document.getElementById("direcciones"));
        directionsDisplay.setMap(map);
        directionsService.route({
            origin: origin,
            destination: destination,
            travelMode: google.maps.DirectionsTravelMode.WALKING
        }, function (result, status) {
            if (status == google.maps.DirectionsStatus.OK) {
                directionsDisplay.setDirections(result);
            }
        });
    }



    function load() {
        if (google.maps.GBrowserIsCompatible()) {

            map = new google.maps.Map(document.getElementById("google_map"));

            //map = new google.maps.Map2(document.getElementById("google_map"));
            map.setMapTypeId(google.maps.MapTypeId['HYBRID']);

            // Centramos el mapa en las coordenadas con zoom 15   
            map.setCenter(new google.maps.LatLng(40.396764, -3.713379), 15);
            // Creamos el punto.   
            var point = new google.maps.LatLng(40.396764, -3.713379);
            // Pintamos el punto   
            //map.addOverlay(new GMarker(point));

            var mark = new google.maps.Marker({ position: point, map: map })


            // Controles que se van a ver en el mapa   
            //            map.addControl(new GLargeMapControl());
            //            var mapControl = new GMapTypeControl();
            //            map.addControl(mapControl);


            // Asociamos el div 'direcciones' a las direcciones que devolveremos a Google   
            gdir = new GDirections(map, document.getElementById("direcciones"));

            // Para recoger los errores si los hubiera   
            GEvent.addListener(gdir, "error", handleErrors);
        }
    }
    // Esta función calcula la ruta con el API de Google Maps   
    function setDirections(Address) {
        gdir.load("from: " + Address + " to: @40.396764, -3.713379",
                { "locale": "es" });
        //Con la opción locale:es hace que la ruta la escriba en castellano.   
    }
    // Se han producido errores   
    function handleErrors() {
        if (gdir.getStatus().code == G_GEO_UNKNOWN_ADDRESS)
            alert("Direccion desconocida");
        else if (gdir.getStatus().code == G_GEO_SERVER_ERROR)
            alert("Error de Servidor");
        else if (gdir.getStatus().code == G_GEO_MISSING_QUERY)
            alert("Falta la direccion inicial");
        else if (gdir.getStatus().code == G_GEO_BAD_KEY)
            alert("Clave de Google Maps incorrecta");
        else if (gdir.getStatus().code == G_GEO_BAD_REQUEST)
            alert("No se ha encontrado la direccion de llegada");
        else alert("Error desconocido");
    }
    function onGDirectionsLoad() {
    }   


</script>
<body onload="initialize()">
    <div id="google_map" style="width: 600px; height: 400px;">
    </div>
    <form action="#" onsubmit="setDirections(this.from.value); return false">
    <input type="text" size="43" id="fromAddress" name="from" value="" />
    <input type="submit" value="Calcula la ruta">
    </form>
    <div id="direcciones" style="width: 500px;">
    </div>
</body>
</html>
