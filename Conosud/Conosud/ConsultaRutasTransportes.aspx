<%@ Page Title="" Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="ConsultaRutasTransportes.aspx.cs" Inherits="ConsultaRutasTransportes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBRxC6Y4f-j6nECyHWigtBATtJyXyha-XU&libraries=adsense&sensor=true&language=es"></script>
    <script src="http://www.google-analytics.com/urchin.js" type="text/javascript"></script>
    <script type="text/javascript">

        // DIRECCION DEL EJEMPLO
        ////http: //jafrancov.com/2011/06/geocode-gmaps-api-v3/
        var ubicacionOrigen;
        var pointMin;
        var destinoMin;
        var minDistance = 10000;
        var map;
        var directionsDisplay = new window.google.maps.DirectionsRenderer({ suppressMarkers: true });
        var directionsService = new window.google.maps.DirectionsService();
        var markerLocation;


        function load_map1() {
            PageMethods.CargarKML(DibujarKML, ErrorBusqueda);

        }

        function DibujarKML(datos, abordaje) {


            var mapOptions = {
                zoom: 13,
                center: abordaje, //new google.maps.LatLng(datos["Ruta"][posMed].Key, datos["Ruta"][posMed].Value),
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };

            map = new google.maps.Map($("#map_canvas").get(0), mapOptions);

            var flightPlanCoordinates = [];

            for (var i = 0; i < datos["Ruta"].length; i++) {
                flightPlanCoordinates.push(new google.maps.LatLng(datos["Ruta"][i].Key, datos["Ruta"][i].Value));
            }



            var flightPath = new google.maps.Polyline({
                path: flightPlanCoordinates,
                geodesic: true,
                strokeColor: '#FF0000',
                strokeOpacity: 1.0,
                strokeWeight: 2
            });

            flightPath.setMap(map);


            //            google.maps.event.addListener(map, 'click', function (event) {
            //                event.latLng;
            //                document.getElementById("puntos").innerHTML = document.getElementById("puntos").innerHTML + '{ "Geometry": { "Latitude": ' + event.latLng.lat() + ', "Longitude": ' + event.latLng.lng() + '} },'
            //            });

            $(".processMessageTooltip").css("display", "none");
            $("#DivBtnPaso2").css("display", "none");
            $("#DivDescripcionPto").css("display", "inline");
            //$("#divPaso1").css("display", "inline");

        }


        $(document).ready(function () {
            load_map();


            //            $('#search').live('click', function () {
            //                load_map1();
            //            });
        });


        function load_map() {
            //            var myLatlng = new google.maps.LatLng(-32.888714, -68.843712);
            //            var myOptions = {
            //                zoom: 14,
            //                center: myLatlng,
            //                mapTypeId: google.maps.MapTypeId.ROADMAP
            //            };



            //            map = new google.maps.Map($("#map_canvas").get(0), myOptions);

            $(document).on('click', '#search', function () {
                //$('#search').live('click', function () {
                $("#DivDescripcionPto").css("display", "none");
                // Obtenemos la dirección y la asignamos a una variable
                var address = $('#nro').val() + ' ' + $('#calle').val() + ',' + $('#cboDepartamentos').val() + ',Mendoza,Argentina';
                // Creamos el Objeto Geocoder
                var geocoder = new google.maps.Geocoder();
                // Hacemos la petición indicando la dirección e invocamos la función
                // geocodeResult enviando todo el resultado obtenido
                //var b = new google.maps.LatLngBounds(new google.maps.LatLng(-32.87724448503208, -68.84460210800171), new google.maps.LatLng(-32.899425455626385, -68.84187698364258))
                geocoder.geocode({ 'address': address }, geocodeResult);

            });

        }



        function geocodeResult(results, status) {
            // Verificamos el estatus
            if (status == 'OK') {
                // Si hay resultados encontrados, centramos y repintamos el mapa
                // esto para eliminar cualquier pin antes puesto
                var mapOptions = {
                    center: results[0].geometry.location,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                };
                map = new google.maps.Map($("#map_canvas").get(0), mapOptions);
                // fitBounds acercará el mapa con el zoom adecuado de acuerdo a lo buscado
                map.fitBounds(results[0].geometry.viewport);
                // Dibujamos un marcador con la ubicación del primer resultado obtenido
                var markerOptions = { position: results[0].geometry.location }

                markerLocation = new google.maps.Marker(markerOptions);
                markerLocation.setMap(map);

                //                document.getElementById("puntos").innerHTML = "";

                //                google.maps.event.addListener(map, 'click', function (event) {
                //                    event.latLng;
                //                    document.getElementById("puntos").innerHTML = document.getElementById("puntos").innerHTML + '{ "Geometry": { "Latitude": ' + event.latLng.lat() + ', "Longitude": ' + event.latLng.lng() + '} },'
                //                });

                $("#DivBtnPaso2").css("display", "inline");

                ubicacionOrigen = results[0].geometry.location;
                //ubicacionOrigen = new google.maps.LatLng(-32.888714, -68.843712);

            } else {
                // En caso de no haber resultados o que haya ocurrido un error
                // lanzamos un mensaje con el error
                alert("No se ha encontrado ninguna direcciòn con los datos ingresados, por favor vuelva a intentar");
            }
        }

        function IrPaso2() {
            markerLocation.setMap(null);
            //$("#divPaso1").css("display", "none");
            $(".processMessageTooltip").css("display", "inline");
            minDistance = 10000;
            PageMethods.BuscarLineaTransporte(ubicacionOrigen.lat(), ubicacionOrigen.lng(), BuscarPuntoMasCercano, ErrorBusqueda);

        }

        function ErrorBusqueda(err) {

        }

        function CargarDatosCabecera(datos) {

//            $("#<%= lblLinea.ClientID %>").text(datos["Linea"]);
//            $("#<%= lblEmpresa.ClientID %>").text(datos["Empresa"]);
//            $("#<%= lblTurno.ClientID %>").text(datos["Turno"]);
//            $("#<%= lblHorarios.ClientID %>").text(datos["Horarios"]);
//            $("#<%= lblTipoUnidad.ClientID %>").text(datos["TipoUnidad"]);
        }

        function BuscarPuntoMasCercano(datos) {

            /// Cargo los datos iniciales del resultado
            CargarDatosCabecera(datos);

            /// Indico las coordenadas del punto de partida
            var origin = ubicacionOrigen;



            var strops = " [";
            for (var i = 0; i < datos["Ruta"].length; i++) {

                if (i == datos["Ruta"].length - 1)
                    strops += '{ "Geometry": { "Latitude":' + datos["Ruta"][i].Key + ', "Longitude":' + datos["Ruta"][i].Value + '} }';
                else {
                    strops += '{ "Geometry": { "Latitude":' + datos["Ruta"][i].Key + ', "Longitude":' + datos["Ruta"][i].Value + '} },';
                }

            }
            strops += ']';


            var destinosPosibles = new Array();
            for (var i = 0; i < datos["PuntosCercanos"].length; i++) {
                destinosPosibles.push(new google.maps.LatLng(datos["PuntosCercanos"][i].Key, datos["PuntosCercanos"][i].Value));
            }



            var descDistancia;
            var descDuracion;
            var contador = 0;
            var cant = 0;
            var tam = 20;
            var idprocess = window.setInterval(function () {

                var service = new google.maps.DistanceMatrixService();
                var porsion = destinosPosibles.slice(cant, cant + tam);

                if (porsion.length > 0) {
                    service.getDistanceMatrix(
                {

                    origins: [origin], //array of origins
                    destinations: porsion, //aray of destionations
                    travelMode: google.maps.TravelMode.WALKING,
                    unitSystem: google.maps.UnitSystem.METRIC,
                    avoidHighways: false,
                    avoidTolls: false
                }, function (response, status) {
                    if (status == google.maps.DistanceMatrixStatus.OK) {

                        var origins = response.originAddresses;
                        var destinations = response.destinationAddresses;
                        for (var i = 0; i < origins.length; i++) {
                            var results = response.rows[i].elements;

                            for (var j = 0; j < results.length; j++) {

                                if (results[j].distance != null && results[j].distance.value < minDistance) {
                                    pointMin = porsion[j];
                                    minDistance = results[j].distance.value;
                                    descDistancia = results[j].distance.text;
                                    descDuracion = results[j].duration.text;
                                    destinoMin = destinations[j];
                                }
                            }
                        }
                    }
                    else {
                        //alert(status)
                    }
                });
                }

                if (porsion.length > 0) {
                    cant += tam;
                }
                else {

                    window.clearInterval(idprocess);

                    $("#<%= lblPtoCercano.ClientID%>").text(destinoMin);
                    $("#<%= lblDuracion.ClientID%>").text(descDuracion);
                    $("#<%= lblDistancia.ClientID%>").text(descDistancia);

                    if (datos["Ruta"].length < 150) {
                        // Esta llamada dibuja los puntos en cada cuadra
                        ShowRutaEncontrada(pointMin, eval(strops));
                    }
                    else {
                        /// Esta llamada dibuja la ruta con lineas cuando son muchos puntos
                        DibujarKML(datos, destinosPosibles[0]);
                        createMarker(map, destinosPosibles[0], 'Punto de Abordaje mas cercano', "Punto de Abordaje", "abordaje");
                        createMarker(map, ubicacionOrigen, "Ubicación Pasajero", "Ubicación Pasajero", "star");
                    }
                }

            }, 2000);
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

        function ShowRutaEncontrada(ptoMasCercano, stops) {

            var batches = [];
            var combinedResults;
            var unsortedResults = [{}]; // to hold the counter and the results themselves as they come back, to later sort



            //window.tour.loadMap(map, directionsDisplay);
            //window.tour.fitBounds(map);


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
                    $(".processMessageTooltip").css("display", "none");
                    $("#DivBtnPaso2").css("display", "none");
                    $("#DivDescripcionPto").css("display", "inline");
                    //$("#divPaso1").css("display", "inline");

                    dibujar(unsortedResults, directionsDisplay, combinedResults, stops, ptoMasCercano);

                }

            }, 2000);

        };

        function dibujar(unsortedResults, directionsDisplay, combinedResults, stops, ptoMasCercano) {

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
                    createMarker(directionsDisplay.getMap(), legs[0].start_location, 'Punto Salida Línea', "Dirección" + "<br>" + legs[0].start_address, markerletter);

                    $("#ctl00_ContentPlaceHolder1_lblPtoPartida").text(legs[0].start_address);

                    createMarker(directionsDisplay.getMap(), ptoMasCercano, 'Punto de Abordaje mas cercano', "Punto de Abordaje", "abordaje");
                }
            }

            var i = legs.length;
            var markerletter = "A".charCodeAt(0);
            markerletter += i;
            markerletter = String.fromCharCode(markerletter);

            var val = stops[stops.length - 1];
            var fin = new window.google.maps.LatLng(val.Geometry.Latitude, val.Geometry.Longitude)

            createMarker(directionsDisplay.getMap(), fin, 'Punto Llegada Línea', "Dirección" + "<br>" + legs[legs.length - 1].end_address, markerletter);

            createMarker(directionsDisplay.getMap(), ubicacionOrigen, "Ubicación Pasajero", "Ubicación Pasajero", "star");


        }

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

        var infowindow = new google.maps.InfoWindow(
        {
            size: new google.maps.Size(150, 50)
        });

        var icons = new Array();
        icons["red"] = new google.maps.MarkerImage("mapIcons/marker_red.png",
        // This marker is 20 pixels wide by 34 pixels tall.
      new google.maps.Size(20, 34),
        // The origin for this image is 0,0.
      new google.maps.Point(0, 0),
        // The anchor for this image is at 9,34.
      new google.maps.Point(9, 34));



        function getMarkerImage(iconStr) {
            if ((typeof (iconStr) == "undefined") || (iconStr == null)) {
                iconStr = "red";
            }
            if (!icons[iconStr]) {
                //icons[iconStr] = new google.maps.MarkerImage("http://www.google.com/mapfiles/marker" + iconStr + ".png",
                // This marker is 20 pixels wide by 34 pixels tall.
                //new google.maps.Size(20, 34),
                // The origin for this image is 0,0.
                //new google.maps.Point(0, 0),
                // The anchor for this image is at 6,20.
                //new google.maps.Point(9, 34));

                icons[iconStr] = new google.maps.MarkerImage("http://maps.google.com/mapfiles/ms/icons/purple-dot.png");

            }

            if (iconStr == "star") {
                icons[iconStr] = new google.maps.MarkerImage("http://maps.google.com/mapfiles/ms/micons/man.png");
            }

            if (iconStr == "abordaje") {
                icons[iconStr] = new google.maps.MarkerImage("http://maps.google.com/mapfiles/ms/micons/bus.png");
            }

            return icons[iconStr];

        }
        // Marker sizes are expressed as a Size of X,Y
        // where the origin of the image (0,0) is located
        // in the top left of the image.

        // Origins, anchor positions and coordinates of the marker
        // increase in the X direction to the right and in
        // the Y direction down.

        var iconImage = new google.maps.MarkerImage('mapIcons/marker_red.png',
        // This marker is 20 pixels wide by 34 pixels tall.
      new google.maps.Size(20, 34),
        // The origin for this image is 0,0.
      new google.maps.Point(0, 0),
        // The anchor for this image is at 9,34.
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




        function createMarker(map, latlng, label, html, color) {
            // alert("createMarker("+latlng+","+label+","+html+","+color+")");
            var contentString = '<b>' + label + '</b><br>' + html;
            var marker = new google.maps.Marker({
                position: latlng,
                map: map,
                shadow: iconShadow,
                icon: getMarkerImage(color),
                //shape: iconShape,
                title: label,
                zIndex: Math.round(latlng.lat() * -100000) << 5
            });
            marker.myname = label;

            google.maps.event.addListener(marker, 'click', function () {
                infowindow.setContent(contentString);
                infowindow.open(map, marker);
            });
            return marker;
        }
    </script>
    <div id="divPaso1">
        <div style="text-align: center; margin-top: 5px;">
            <center>
                <table border="0" cellpadding="0" cellspacing="0" width="80%" style="text-align: left;">
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" SkinID="lblConosud" Text="Calle Nro:"></asp:Label>
                            <input type="text" maxlength="100" style="width: 40px" id="nro" value="130" />
                        </td>
                        <td>
                            <asp:Label ID="Label1" runat="server" SkinID="lblConosud" Text="Nombre Calle:"></asp:Label>
                            <input type="text" maxlength="100" style="width: 220px" id="calle" value="Espejo" />
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" SkinID="lblConosud" Text="Departamento:"></asp:Label>
                            <select id="cboDepartamentos">
                                <option value="Mendoza" selected="selected">Capital</option>
                                <option value="General Alvear">General Alvear</option>
                                <option value="Godoy Cruz">Godoy Cruz</option>
                                <option value="Guaymallén">Guaymallén</option>
                                <option value="Junín">Junín</option>
                                <option value="La Paz">La Paz</option>
                                <option value="Las Heras">Las Heras</option>
                                <option value="Lavalle">Lavalle</option>
                                <option value="Luján de Cuyo">Luján de Cuyo</option>
                                <option value="Maipú">Maipú</option>
                                <option value="Malargüe">Malargüe</option>
                                <option value="Rivadavia">Rivadavia</option>
                                <option value="San Carlos">San Carlos</option>
                                <option value="San Martín">San Martín</option>
                                <option value="San Rafael">San Rafael</option>
                                <option value="Santa Rosa">Santa Rosa</option>
                                <option value="Tunuyán">Tunuyán</option>
                                <option value="Tupungato">Tupungato</option>
                            </select>
                        </td>
                        <td>
                            <input type="button" id="search" value="Ubicar" />
                        </td>
                    </tr>
                </table>
            </center>
        </div>
        <div id="DivDescripcionPto" style="display: none;">
            <table border="0" cellpadding="0" cellspacing="0" style="text-align: left;padding-left:2px; width: 95%;background-color:White;border:1px solid black">
                <tr>
                    <td colspan="4" align="center">
                        <asp:Label ID="Label13" runat="server" SkinID="lblConosudNormal" Text="LINEA MAS CERCANA:"></asp:Label>
                        <asp:Label ID="lblLinea" runat="server" SkinID="lblConosud" Text="Nro 2 (SAN JOSE - DORREGO)"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label6" runat="server" SkinID="lblConosud" Text="Empresa:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblEmpresa" runat="server" SkinID="lblConosudNormal" Text="Andesmar"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label8" runat="server" SkinID="lblConosud" Text="Turno:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblTurno" runat="server" SkinID="lblConosudNormal" Text="1 y 2"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label10" runat="server" SkinID="lblConosud" Text="Horario Salida/Llegada:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblHorarios" runat="server" SkinID="lblConosudNormal" Text="6.1/7.45 - 18.05/19.15"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label12" runat="server" SkinID="lblConosud" Text="Pto Partida:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblPtoPartida" runat="server" SkinID="lblConosudNormal" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label14" runat="server" SkinID="lblConosud" Text="Pto Mas Cercano:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblPtoCercano" runat="server" SkinID="lblConosudNormal" Text=""></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label16" runat="server" SkinID="lblConosud" Text="Tipo Unidad:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblTipoUnidad" runat="server" SkinID="lblConosudNormal" Text="Minibus"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label5" runat="server" SkinID="lblConosud" Text="Distnacia:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblDistancia" runat="server" SkinID="lblConosudNormal" Text=""></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label9" runat="server" SkinID="lblConosud" Text="Duración:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblDuracion" runat="server" SkinID="lblConosudNormal" Text="Minibus"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div id='map_canvas' style="width: 960px; height: 450px; border: 1px solid black;
            margin-top: 5px">
        </div>
        <div id='DivBtnPaso2' style="display: none; padding-top: 15px">
            <table border="0" cellpadding="0" cellspacing="0" width="80%" style="text-align: left;
                padding-top: 10px">
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" SkinID="lblConosud" Text="Si la dirección es correcta o aproximada por favor continue con el siguiente paso: "></asp:Label>
                        <asp:Button ID="btnPaso2" runat="server" Text="Buscar Linea Transporte" Mensaje="Buscando Legajos solicitados..."
                            OnClientClick="IrPaso2();return false;" SkinID="btnConosud" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="processMessageTooltip" style="display: none; left: 40%; top: 40%">
        <table border="0" cellpadding="0" cellspacing="0" style="height: 62px;">
            <tr>
                <td align="center">
                    <img alt="a" src="Images/loadingNew.gif" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <div id="divTituloCarga" style="font-weight: bold; font-family: Tahoma; font-size: 12px;
                        color: black; vertical-align: middle">
                        Buscar Recorrido mas cercano...
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
