<%@ Page Title="" Language="C#" Theme="MiTema" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="ConsultaRecorridosTransportes.aspx.cs" Inherits="ConsultaRecorridosTransportes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBRxC6Y4f-j6nECyHWigtBATtJyXyha-XU&libraries=adsense&sensor=true&language=es&libraries=geometry"></script>
    <script src="http://www.google-analytics.com/urchin.js" type="text/javascript"></script>
    <script type="text/javascript">
        var map;
        var directionsDisplay;
        var directionsService;
        var flightPath;
        var markerInicial;
        var markerFinal;

        jQuery(function () {


            directionsDisplay = new window.google.maps.DirectionsRenderer({ suppressMarkers: true });
            directionsService = new window.google.maps.DirectionsService();

            var mapOptions = {
                zoom: 8,
                center: new google.maps.LatLng(-32.948713, -68.805808),
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };

            map = new window.google.maps.Map(document.getElementById("map"),mapOptions);

        });


        function ShowRutaEncontrada(stops) {

            var batches = [];
            var combinedResults;
            var unsortedResults = [{}]; // to hold the counter and the results themselves as they come back, to later sort


            if (stops.length > 1) {
                directionsDisplay.setMap(map);
                armarRutaConsula(stops, batches);
            }

            var cant = 0;
            var aa = window.setInterval(function () {
                var porsion = batches.slice(cant, cant + 5);
                if (porsion.length > 0) {
                    calculo(directionsService, directionsDisplay, porsion, unsortedResults);
                    cant += 5;
                }
                else {
                    window.clearInterval(aa);
                    dibujar(unsortedResults, directionsDisplay, combinedResults, stops);
                }

            }, 2000);

        };

        function calculo(directionsService, directionsDisplay, batches, unsortedResults) {


            var directionsResultsReturned = 0;


            for (var k = 0; k < batches.length; k++) {
                var lastIndex = batches[k].length - 1;
                var start = batches[k][0].location;
                var end = batches[k][lastIndex].location;

                // trim first and last entry from array
                var waypts = [];
                waypts = batches[k];
                waypts.splice(0, 1);
                waypts.splice(waypts.length - 1, 1);

                var request = {
                    origin: start,
                    destination: end,
                    waypoints: waypts,
                    travelMode: window.google.maps.TravelMode.WALKING
                };



                (function (kk) {
                    directionsService.route(request, function (result, status) {
                        if (status == window.google.maps.DirectionsStatus.OK) {

                            var unsortedResult = { order: kk, result: result };
                            unsortedResults.push(unsortedResult);

                            directionsResultsReturned++;


                        }
                        else {
                            var aa = 11;
                        }
                    });
                })(k);
            }
        }


        function armarRutaConsula(stops, batches) {

            var itemsPerBatch = 10; // google API max = 10 - 1 start, 1 stop, and 8 waypoints
            var itemsCounter = 0;
            var wayptsExist = stops.length > 0;

            while (wayptsExist) {
                var subBatch = [];
                var subitemsCounter = 0;

                for (var j = itemsCounter; j < stops.length; j++) {
                    subitemsCounter++;
                    subBatch.push({
                        location: new window.google.maps.LatLng(stops[j].Geometry.Latitude, stops[j].Geometry.Longitude),
                        stopover: true
                    });
                    if (subitemsCounter == itemsPerBatch)
                        break;
                }

                itemsCounter += subitemsCounter;
                batches.push(subBatch);
                wayptsExist = itemsCounter < stops.length;
                // If it runs again there are still points. Minus 1 before continuing to
                // start up with end of previous tour leg
                itemsCounter--;
            }

        }


        function dibujar(unsortedResults, directionsDisplay, combinedResults, stops) {

            // sort the returned values into their correct order
            unsortedResults.sort(function (a, b) { return parseFloat(a.order) - parseFloat(b.order); });
            var count = 0;
            for (var key in unsortedResults) {
                if (unsortedResults[key].result != null) {
                    if (unsortedResults.hasOwnProperty(key)) {
                        if (count == 0) // first results. new up the combinedResults object
                            combinedResults = unsortedResults[key].result;
                        else {
                            // only building up legs, overview_path, and bounds in my consolidated object. This is not a complete
                            // directionResults object, but enough to draw a path on the map, which is all I need
                            combinedResults.routes[0].legs = combinedResults.routes[0].legs.concat(unsortedResults[key].result.routes[0].legs);
                            combinedResults.routes[0].overview_path = combinedResults.routes[0].overview_path.concat(unsortedResults[key].result.routes[0].overview_path);

                            combinedResults.routes[0].bounds = combinedResults.routes[0].bounds.extend(unsortedResults[key].result.routes[0].bounds.getNorthEast());
                            combinedResults.routes[0].bounds = combinedResults.routes[0].bounds.extend(unsortedResults[key].result.routes[0].bounds.getSouthWest());
                        }
                        count++;
                    }
                }
            }
            directionsDisplay.setDirections(combinedResults);
            var legs = combinedResults.routes[0].legs;

            for (var i = 0; i < legs.length; i++) {
                var markerletter = "A".charCodeAt(0);
                markerletter += i;
                markerletter = String.fromCharCode(markerletter);
                if (i == 0) {
                    markerInicial = createMarker(directionsDisplay.getMap(), legs[0].start_location, 'Punto Salida Línea', "Dirección" + "<br>" + legs[0].start_address, "change");
                }
            }

            var i = legs.length;
            var markerletter = "A".charCodeAt(0);
            markerletter += i;
            markerletter = String.fromCharCode(markerletter);

            var val = stops[stops.length - 1];
            var fin = new window.google.maps.LatLng(val.Geometry.Latitude, val.Geometry.Longitude)

            markerFinal = createMarker(directionsDisplay.getMap(), fin, 'Punto Llegada Línea', "Dirección" + "<br>" + legs[legs.length - 1].end_address, "change");

        }

        function init(newPoints) {

            CargarCabecera(newPoints);
            
            if (flightPath != null)
                flightPath.setMap(null);

            if (markerInicial != null)
                markerInicial.setMap(null);

            if (markerFinal  != null)
                markerFinal.setMap(null);
            

            if (newPoints["InfoRecorrido"]["recorrido"].length < 150) {

                var stops = "[]";

                if (newPoints != undefined) {
                    stops = " [";
                    for (var i = 0; i < newPoints["InfoRecorrido"]["recorrido"].length; i++) {

                        if (i == newPoints["InfoRecorrido"]["recorrido"].length - 1)
                            stops += '{ "Geometry": { "Latitude":' + newPoints["InfoRecorrido"]["recorrido"][i].Latitud.replace(',', '.') + ', "Longitude":' + newPoints["InfoRecorrido"]["recorrido"][i].Longitud.replace(',', '.') + '} }';
                        else {
                            stops += '{ "Geometry": { "Latitude":' + newPoints["InfoRecorrido"]["recorrido"][i].Latitud.replace(',', '.') + ', "Longitude":' + newPoints["InfoRecorrido"]["recorrido"][i].Longitud.replace(',', '.') + '} },';
                        }

                    }
                    stops += ']';

                    stops = eval(stops);
                    ShowRutaEncontrada(stops);
                }
            }
            else {
                ShowRutaEncontradaLines(newPoints);
            }

            $("#map").css("display", "block");
        }


        function ShowRutaEncontradaLines(newPoints) {

            var mapOptions = {
                zoom: 13,
                center: new google.maps.LatLng(newPoints["InfoRecorrido"]["recorrido"][0].Latitud.replace(',', '.'), newPoints["InfoRecorrido"]["recorrido"][0].Longitud.replace(',', '.')),
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };

            map = new google.maps.Map(document.getElementById("map"), mapOptions);

            var flightPlanCoordinates = [];

            for (var i = 0; i < newPoints["InfoRecorrido"]["recorrido"].length; i++) {
                flightPlanCoordinates.push(new google.maps.LatLng(newPoints["InfoRecorrido"]["recorrido"][i].Latitud.replace(',', '.'), newPoints["InfoRecorrido"]["recorrido"][i].Longitud.replace(',', '.')));
            }



            flightPath = new google.maps.Polyline({
                path: flightPlanCoordinates,
                geodesic: true,
                strokeColor: '#FF0000',
                strokeOpacity: 1.0,
                strokeWeight: 2
            });

            flightPath.setMap(map);

            markerInicial = createMarker(map, flightPlanCoordinates[0], 'Punto Salida Línea', "", "change");
            markerFinal  = createMarker(map, flightPlanCoordinates[flightPlanCoordinates.length - 1], 'Punto Llegada Línea', "", "change");

            var path = flightPath.getPath();
            var distance = google.maps.geometry.spherical.computeLength(path.getArray()) / 1000;
            $("#<%= lblTotalKm.ClientID %>").text(distance.toFixed(2) + " km.");
        }

        function CargarCabecera(datos) {

            $("#<%= lblEmpresa.ClientID %>").text(datos["InfoRecorrido"]["Empresa"]);
            $("#<%= lblHorarios.ClientID %>").text(datos["InfoRecorrido"]["Horario"]);
            $("#<%= lblRecorrido.ClientID %>").text(datos["InfoRecorrido"]["TipoRecorrido"]);
            

        }

        var infowindow = new google.maps.InfoWindow({size: new google.maps.Size(150, 50)});

        var icons = new Array();

        icons["change"] = new google.maps.MarkerImage("http://www.google.com/mapfiles/ms/micons/green.png",
        // This marker is 20 pixels wide by 34 pixels tall.
        new google.maps.Size(25, 34),
        // The origin for this image is 0,0.
        new google.maps.Point(0, 0),
        // The anchor for this image is at 6,20.
        new google.maps.Point(9, 34));

        var iconShadow = new google.maps.MarkerImage('http://www.google.com/mapfiles/shadow50.png',
        // The shadow image is larger in the horizontal dimension
        // while the position and offset are the same as for the main image.
        new google.maps.Size(37, 34),
        new google.maps.Point(0, 0),
        new google.maps.Point(9, 34));

        // Shapes define the clickable region of the icon.
        // The type defines an HTML &lt;area&gt; element 'poly' which
        // traces out a polygon as a series of X,Y points. The final
        // coordinate closes the poly by connecting to the first
        // coordinate.
        var iconShape = {
            coord: [9, 0, 6, 1, 4, 2, 2, 4, 0, 8, 0, 12, 1, 14, 2, 16, 5, 19, 7, 23, 8, 26, 9, 30, 9, 34, 11, 34, 11, 30, 12, 26, 13, 24, 14, 21, 16, 18, 18, 16, 20, 12, 20, 8, 18, 4, 16, 2, 15, 1, 13, 0],
            type: 'poly'
        };

        function getMarkerImage(iconStr) {
            
            return icons[iconStr];
        }
       
        function createMarker(map, latlng, label, html, color, pos) {

            var contentString = '<b>' + label + '</b><br>' + html;
            var marker = new google.maps.Marker({
                position: latlng,
                map: map,
                shadow: iconShadow,
                icon: getMarkerImage(color),
                shape: iconShape,
                title: label,
                zIndex: Math.round(latlng.lat() * -100000) << 5
            });
            marker.myname = label;

            return marker;
        }


        function CargarMapa(combo, eventarqs) {
            var id = eventarqs.get_item().get_value();
            PageMethods.GetRecorrido(id, init, function () {
                alert("Error en el llamado de recuperacion del recorrido.");
            });
        }
    </script>
    <div id="DivDescripcionPto" style="display: inline; background-color: White;">
        <table border="0" cellpadding="0" cellspacing="0" style="text-align: left; width: 95%;
            margin-top: 5px; table-layout: fixed">
            <tr>
                <td align="center">
                    <telerik:RadComboBox ID="cboRecorridos" runat="server" Skin="Sunset" Width="90%"
                        EmptyMessage="Seleccione un recorrido" AllowCustomText="true" MarkFirstMatch="true"
                        OnClientSelectedIndexChanged="CargarMapa" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" style="border: solid 1px black;
            text-align: left; width: 95%; margin-top: 5px; table-layout: fixed">
            <tr>
                <td rowspan="3" align="center" style="padding: 5px; width: 80px" valign="middle">
                    <img src="images/autobus.jpg" alt="" width="65px" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label Style="width: 90px" ID="Label6" runat="server" SkinID="lblConosud" Text="Empresa:"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblEmpresa" runat="server" SkinID="lblConosudNormal" Text=""></asp:Label>
                </td>
                <td>
                    <asp:Label Style="width: 60px" ID="Label8" runat="server" SkinID="lblConosud" Text="Recorrido:"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblRecorrido" runat="server" SkinID="lblConosudNormal" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label Style="width: 90px" ID="Label10" runat="server" SkinID="lblConosud" Text="Horario Salida/Llegada:"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblHorarios" runat="server" SkinID="lblConosudNormal" Text=""></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label12" runat="server" SkinID="lblConosud" Text="Total Km:"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblTotalKm" runat="server" SkinID="lblConosudNormal" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div style="display: none">
        <input type="text" id="txtstar" value="" />
        <input type="text" id="txtend" value="" />
        <input type="text" id="newPoints" style="width: 600px;" value='' />
    </div>
    <div id="map" style="height: 450px; width: 800px; margin-top: 5px;">
    </div>
    <script src="http://www.google-analytics.com/urchin.js" type="text/javascript">
    </script>
    <script type="text/javascript">
        _uacct = "UA-162157-1";
        urchinTracker();
    </script>
</asp:Content>
