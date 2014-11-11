<%@ Page Title="" Theme="MiTema" Language="C#" MasterPageFile="~/DefaultMasterPage.master"
    AutoEventWireup="true" CodeFile="GestionAltaRecorridos.aspx.cs" Inherits="GestionAltaRecorridos" %>
<%@ Register Assembly="ControlsAjaxNotti" Namespace="ControlsAjaxNotti" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.13/themes/base/jquery-ui.css" id="theme">
    <link href="UploadFiles/CSS/uploadify.css" rel="stylesheet" type="text/css" />
    <script src="UploadFiles/scripts/jquery.uploadify.js" type="text/javascript"></script>
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBRxC6Y4f-j6nECyHWigtBATtJyXyha-XU&libraries=adsense&sensor=true&language=es"></script>
    <script src="http://www.google-analytics.com/urchin.js" type="text/javascript"></script>
    <script type="text/javascript">
        var map;
        var directionsDisplay;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Metodos para la gesti&oacute;n de archivo fotocopia documento
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        $(window).load(
            function () {
                $("#<%=FileUpload2.ClientID %>").fileUpload({
                    'uploader': 'UploadFiles/scripts/uploader.swf',
                    'cancelImg': 'UploadFiles/images/cancel.png',
                    'buttonImg': 'Images/Procesar.png',
                    'wmode': 'transparent',
                    'buttonText': 'Seleccionar',
                    'script': 'UploadFiles/Upload.ashx',
                    'folder': 'ArchivosKML',
                    'fileDesc': 'Archivos kml',
                    'fileExt': '*.kml',
                    'multi': false,
                    'width': '45',
                    'auto': true,
                    'sizeLimit': 1024 * 1024 * 4.5,
                    'onComplete': TerminoUpload
                });
            }
        );


        function TerminoUpload(sender, arg, infoArchivo, DatosArchivo, aa) {

            window.setTimeout(function () {
                if (DatosArchivo.length > 150)
                    DibujarKML(DatosArchivo);
                else
                    alert(DatosArchivo);

            }, 200);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        function GuardarDatos() {
            PageMethods.GuardarAlta($('#txtEmpresa').val(), $('#txtHorarioSalida').val(), $('#txtHorarioLlegada').val(), $('#cboTipoUnidad').val(), $('#txtTurno').val(), $('#txtLinea').val(), $('#cboTipoRecorrido').val(), $('#cboTipoTurno').val(), function () { window.location.reload(); }, function () { });

        }

        var infoWindow;

        function DibujarKML(datos) {

            var flightPlanCoordinates = [];
            datos = datos.split('@');

            var posMed = parseInt(datos.length / 2);

            var mapOptions = {
                zoom: 13,
                center: new google.maps.LatLng(datos[posMed].split(',')[0], datos[posMed].split(',')[1]),
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };

            map = new google.maps.Map(document.getElementById("map"), mapOptions);

           

            for (var i = 0; i < datos.length - 1; i++) {
                flightPlanCoordinates.push(new google.maps.LatLng(datos[i].split(',')[0], datos[i].split(',')[1]));
            }



            var flightPath = new google.maps.Polyline({
                path: flightPlanCoordinates,
                geodesic: true,
                strokeColor: '#3399FF',
                strokeOpacity: 1.0,
                strokeWeight: 3,
                clickable: true
            });

            flightPath.setMap(map);


            google.maps.event.addListener(flightPath, 'click', showArrays);


            infoWindow = new google.maps.InfoWindow();

            /*  google.maps.event.addListener(map, 'click', function (event) {
            event.latLng;
            document.getElementById("puntos").innerHTML = document.getElementById("puntos").innerHTML + '{ "Geometry": { "Latitude": ' + event.latLng.lat() + ', "Longitude": ' + event.latLng.lng() + '} },'
            });*/



        }

        function showArrays(event) {
            var vertices = this.getPath();

            var contentString = '<b>Bermuda Triangle polygon</b><br>' + 'Clicked location: <br>' + event.latLng.lat() + ',' + event.latLng.lng() + '<br>';
            var distancia = 100000;
            var punto;
            // Iterate over the vertices.
            for (var i = 0; i < vertices.getLength(); i++) {
                var xy = vertices.getAt(i);

                var d = google.maps.geometry.spherical.computeDistanceBetween(event.latLng, xy);
                if (d < distancia) {
                    distancia = d;
                    punto = xy;
                }



                //contentString += '<br>' + 'Coordinate ' + i + ':<br>' + xy.lat() + ',' + xy.lng();
            }

            var marker = new google.maps.Marker({
                position: punto,
                title: '#A' ,
                map: map
            });

            //alert(distancia);

            // Replace the info window's content and position.
            //infoWindow.setContent(contentString);
            //infoWindow.setPosition(event.latLng);

            infoWindow.open(map);
        }

        jQuery(function () {

            // new up complex objects before passing them around
            directionsDisplay = new window.google.maps.DirectionsRenderer({ suppressMarkers: true });


            var mapOptions = {
                zoom: 8,
                center: new google.maps.LatLng(-32.948713, -68.805808),
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };

            map = new window.google.maps.Map(document.getElementById("map"), mapOptions);


            google.maps.event.addListener(map, 'click', function (event) {

                $("#newPoints").val($("#newPoints").val() + '{ "Geometry" :{"Latitude":' + event.latLng.k + ', "Longitude":' + event.latLng.B + '}},');

                createMarker(map, event.latLng, "marker", "some text for marker ", "change", 1120);

            });



            google.maps.event.addListener(directionsDisplay, 'click', function (event) {
                alert();
                createMarker(map, event.latLng, "marker", "some text for marker ", "change", 1120);

            });



        });

        function init(star, end, newPoints) {

            var stops = [
            { "Geometry": { "Latitude": -32.9223417, "Longitude": -68.8017413} },
            { "Geometry": { "Latitude": -32.9207926, "Longitude": -68.801119} },
            { "Geometry": { "Latitude": -32.920613, "Longitude": -68.807642} },
            { "Geometry": { "Latitude": -32.9203991, "Longitude": -68.810738} },
            { "Geometry": { "Latitude": -32.9205972, "Longitude": -68.8232479} },
            { "Geometry": { "Latitude": -32.918796, "Longitude": -68.8226792} },
            { "Geometry": { "Latitude": -32.9139865, "Longitude": -68.8265416} },
            { "Geometry": { "Latitude": -32.913257, "Longitude": -68.8317666} },
            { "Geometry": { "Latitude": -32.9123113, "Longitude": -68.8421413} },
            { "Geometry": { "Latitude": -32.911987, "Longitude": -68.8451561} },
            { "Geometry": { "Latitude": -32.8994069, "Longitude": -68.8417786} },
            { "Geometry": { "Latitude": -32.908724, "Longitude": -68.844206} },
            { "Geometry": { "Latitude": -32.899752, "Longitude": -68.841974} }

            /*  {"Geometry": { "Latitude": -32.94897471324162, "Longitude": -68.80584955215454} }, { "Geometry": { "Latitude": -32.95027115867484, "Longitude": -68.80475521087646} }, { "Geometry": { "Latitude": -32.95057726107188, "Longitude": -68.80496978759765} }, { "Geometry": { "Latitude": -32.95387229622379, "Longitude": -68.79909038543701} }, { "Geometry": { "Latitude": -32.95464652163358, "Longitude": -68.79703044891357} }, { "Geometry": { "Latitude": -32.95545675026478, "Longitude": -68.79516363143921} }
            , { "Geometry": { "Latitude": -32.955870864252276, "Longitude": -68.79404783248901} }, { "Geometry": { "Latitude": -32.956050913206816, "Longitude": -68.79338264465332} }, { "Geometry": { "Latitude": -32.9566090626348, "Longitude": -68.79258871078491} }, { "Geometry": { "Latitude": -32.95858956441623, "Longitude": -68.79376888275146} }, { "Geometry": { "Latitude": -32.95963381112011, "Longitude": -68.7943696975708} }
            , { "Geometry": { "Latitude": -32.96051600992581, "Longitude": -68.79477739334106} }, { "Geometry": { "Latitude": -32.961380196134236, "Longitude": -68.79520654678345} }, { "Geometry": { "Latitude": -32.96258644191746, "Longitude": -68.79580736160278} }, { "Geometry": { "Latitude": -32.962874498176085, "Longitude": -68.79473447799682} }, { "Geometry": { "Latitude": -32.96305453286081, "Longitude": -68.79376888275146} }, { "Geometry": { "Latitude": -32.96334258759333, "Longitude": -68.7924599647522} }
            , { "Geometry": { "Latitude": -32.96357663137198, "Longitude": -68.79121541976929} }, { "Geometry": { "Latitude": -32.96382867782485, "Longitude": -68.79003524780273} }, { "Geometry": { "Latitude": -32.96402671381911, "Longitude": -68.78919839859009} }, { "Geometry": { "Latitude": -32.964152736493425, "Longitude": -68.78827571868896} }, { "Geometry": { "Latitude": -32.964350771761296, "Longitude": -68.78754615783691} }, { "Geometry": { "Latitude": -32.96456680972907, "Longitude": -68.78645181655884} }
            , { "Geometry": { "Latitude": -32.964782847168564, "Longitude": -68.78522872924804} }, { "Geometry": { "Latitude": -32.96507089626612, "Longitude": -68.78370523452759} }, { "Geometry": { "Latitude": -32.96397270404651, "Longitude": -68.78340482711792} }, { "Geometry": { "Latitude": -32.962010326582806, "Longitude": -68.78278255462646} }, { "Geometry": { "Latitude": -32.962298384719695, "Longitude": -68.78166675567627} }, { "Geometry": { "Latitude": -32.9625504348191, "Longitude": -68.78037929534912} }, { "Geometry": { "Latitude": -32.96256843837012, "Longitude": -68.78014326095581} }, { "Geometry": { "Latitude": -32.9636486447176, "Longitude": -68.78005743026733} }, { "Geometry": { "Latitude": -32.96764529336791, "Longitude": -68.77969264984131} }
            , { "Geometry": { "Latitude": -32.96764529336791, "Longitude": -68.78078699111938} }, { "Geometry": { "Latitude": -32.96751927567569, "Longitude": -68.78173112869262} }, { "Geometry": { "Latitude": -32.96741126036783, "Longitude": -68.78310441970825} }, { "Geometry": { "Latitude": -32.96728524234176, "Longitude": -68.78462791442871} }, { "Geometry": { "Latitude": -32.96708721365184, "Longitude": -68.78582954406738} }, { "Geometry": { "Latitude": -32.96827537913246, "Longitude": -68.78628015518188} }, { "Geometry": { "Latitude": -32.969463528631635, "Longitude": -68.78658056259155} }, { "Geometry": { "Latitude": -32.97072366972737, "Longitude": -68.78692388534546} }, { "Geometry": { "Latitude": -32.97203779771984, "Longitude": -68.78726720809936} }
            , { "Geometry": { "Latitude": -32.97335190615994, "Longitude": -68.78767490386963} }, { "Geometry": { "Latitude": -32.97477400148347, "Longitude": -68.78806114196777} }, { "Geometry": { "Latitude": -32.97560204643937, "Longitude": -68.78827571868896} }, { "Geometry": { "Latitude": -32.976826098673875, "Longitude": -68.78864049911499} }, { "Geometry": { "Latitude": -32.97718611080798, "Longitude": -68.78694534301758} }, { "Geometry": { "Latitude": -32.97742011790806, "Longitude": -68.78621578216553} }, { "Geometry": { "Latitude": -32.97756412196912, "Longitude": -68.78561496734619} }, { "Geometry": { "Latitude": -32.9776901253299, "Longitude": -68.78479957580566} }, { "Geometry": { "Latitude": -32.9780501339414, "Longitude": -68.7834906578064} }
            , { "Geometry": { "Latitude": -32.978356140107174, "Longitude": -68.78278255462646} }, { "Geometry": { "Latitude": -32.978662145212475, "Longitude": -68.78233194351196} }, { "Geometry": { "Latitude": -32.9802821545721, "Longitude": -68.78336191177368} }, { "Geometry": { "Latitude": -32.980768151584016, "Longitude": -68.78164529800415} }, { "Geometry": { "Latitude": -32.981326144854314, "Longitude": -68.77982139587402} }, { "Geometry": { "Latitude": -32.98156014098269, "Longitude": -68.77896308898926} }, { "Geometry": { "Latitude": -32.98166813898661, "Longitude": -68.7784481048584} }, { "Geometry": { "Latitude": -32.98237012279206, "Longitude": -68.7774395942688} }, { "Geometry": { "Latitude": -32.9831080997488, "Longitude": -68.77692461013794} }
            , { "Geometry": { "Latitude": -32.98395406574445, "Longitude": -68.77741813659668} }, { "Geometry": { "Latitude": -32.987373845654915, "Longitude": -68.77913475036621} }, { "Geometry": { "Latitude": -32.9884177520759, "Longitude": -68.77967119216919} }, { "Geometry": { "Latitude": -32.988165775794286, "Longitude": -68.78145217895508} }, { "Geometry": { "Latitude": -32.98800379066198, "Longitude": -68.7823748588562} }, { "Geometry": { "Latitude": -32.989029691479395, "Longitude": -68.78263235092163} }, { "Geometry": { "Latitude": -32.98875971873612, "Longitude": -68.78409147262573} }, { "Geometry": { "Latitude": -32.9884717468998, "Longitude": -68.78572225570679} }, { "Geometry": { "Latitude": -32.98823776909101, "Longitude": -68.78666639328003} }
            , { "Geometry": { "Latitude": -32.988111780783214, "Longitude": -68.78733158111572} }, { "Geometry": { "Latitude": -32.98782380683299, "Longitude": -68.78861904144287} }, { "Geometry": { "Latitude": -32.98877771694469, "Longitude": -68.78894090652466} }, { "Geometry": { "Latitude": -32.989515640336364, "Longitude": -68.78909111022949} }, { "Geometry": { "Latitude": -32.990325549152516, "Longitude": -68.78939151763916} }, { "Geometry": { "Latitude": -32.99104546186103, "Longitude": -68.78956317901611} }, { "Geometry": { "Latitude": -32.991747371097986, "Longitude": -68.7898850440979} }, { "Geometry": { "Latitude": -32.9924132798292, "Longitude": -68.79005670547485} }, { "Geometry": { "Latitude": -32.99216131495424, "Longitude": -68.79149436950683} }
            , { "Geometry": { "Latitude": -32.991801363884996, "Longitude": -68.79288911819458} }, { "Geometry": { "Latitude": -32.990325549152516, "Longitude": -68.80067825317383} }, { "Geometry": { "Latitude": -32.9885977346938, "Longitude": -68.8089394569397} }, { "Geometry": { "Latitude": -32.99039754068761, "Longitude": -68.80072116851806} }, { "Geometry": { "Latitude": -32.99196334204783, "Longitude": -68.7929105758667} }, { "Geometry": { "Latitude": -32.996120679820784, "Longitude": -68.79415512084961} }, { "Geometry": { "Latitude": -33.00274322029944, "Longitude": -68.7960433959961} }, { "Geometry": { "Latitude": -33.00925729955151, "Longitude": -68.79791021347046} }, { "Geometry": { "Latitude": -33.01470932199389, "Longitude": -68.79947662353515} }
            , { "Geometry": { "Latitude": -33.017048370204506, "Longitude": -68.80014181137085} }, { "Geometry": { "Latitude": -33.029696148114546, "Longitude": -68.80374670028686} }, { "Geometry": { "Latitude": -33.030235842216655, "Longitude": -68.80499124526977} }, { "Geometry": { "Latitude": -33.03012790366064, "Longitude": -68.8198184967041} }
            , { "Geometry": { "Latitude": -33.03493104144184, "Longitude": -68.82104158401489} }*/
            //, { "Geometry": { "Latitude": -33.04149670506489, "Longitude": -68.82262945175171} }
            //, { "Geometry": { "Latitude": -33.04151469251227, "Longitude": -68.83258581161499} }, { "Geometry": { "Latitude": -33.036406109857474, "Longitude": -68.86704683303833} }, { "Geometry": { "Latitude": -33.039805868611616, "Longitude": -68.86702537536621} }
            //, { "Geometry": { "Latitude": -33.06892329348743, "Longitude": -68.87305498123169} }, { "Geometry": { "Latitude": -33.067808411930635, "Longitude": -68.87829065322876} }, { "Geometry": { "Latitude": -33.067125090446495, "Longitude": -68.8838267326355} }, { "Geometry": { "Latitude": -33.06716105486743, "Longitude": -68.89489889144897} }, { "Geometry": { "Latitude": -33.067844376072294, "Longitude": -68.91305208206177} }, { "Geometry": { "Latitude": -33.070110087365215, "Longitude": -68.9407753944397} }, { "Geometry": { "Latitude": -33.07081136713284, "Longitude": -68.94294261932373} }, { "Geometry": { "Latitude": -33.0741918168321, "Longitude": -68.9759874343872} }
                    ];





            var directionsService = new window.google.maps.DirectionsService();

            if (star != null) {
                eval('stops.splice(' + star + ', ' + end + ', ' + newPoints + ')');
            }

            window.tour = null;
            Tour_startUp(stops);

            window.tour.loadMap(map, directionsDisplay);
            window.tour.fitBounds(map);

            if (stops.length > 1)
                window.tour.calcRoute(directionsService, directionsDisplay);
        }

        function Tour_startUp(stops) {
            if (!window.tour) window.tour = {
                updateStops: function (newStops) {
                    stops = newStops;
                },
                // map: google map object
                // directionsDisplay: google directionsDisplay object (comes in empty)
                loadMap: function (map, directionsDisplay) {
                    var myOptions = {
                        zoom: 13,
                        center: new window.google.maps.LatLng(-32.912291, -68.834421), // default to London
                        mapTypeId: window.google.maps.MapTypeId.ROADMAP
                    };
                    map.setOptions(myOptions);
                    directionsDisplay.setMap(map);
                },
                fitBounds: function (map) {
                    var bounds = new window.google.maps.LatLngBounds();

                    // extend bounds for each record
                    jQuery.each(stops, function (key, val) {
                        var myLatlng = new window.google.maps.LatLng(val.Geometry.Latitude, val.Geometry.Longitude);
                        bounds.extend(myLatlng);
                    });
                    map.fitBounds(bounds);
                },
                calcRoute: function (directionsService, directionsDisplay) {
                    var batches = [];
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

                    // now we should have a 2 dimensional array with a list of a list of waypoints
                    var combinedResults;
                    var unsortedResults = [{}]; // to hold the counter and the results themselves as they come back, to later sort
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

                                    if (directionsResultsReturned == batches.length) // we've received all the results. put to map
                                    {
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
                                        var wp = combinedResults.Nb.waypoints;

                                        // alert(legs.length);
                                        for (var i = 0; i < legs.length; i++) {
                                            var markerletter = "A".charCodeAt(0);
                                            markerletter += i;
                                            markerletter = String.fromCharCode(markerletter);
                                            createMarker(directionsDisplay.getMap(), legs[i].start_location, "marker" + i, "some text for marker " + i + "<br>" + legs[i].start_address, markerletter, i);
                                        }
                                        var i = legs.length;
                                        var markerletter = "A".charCodeAt(0);
                                        markerletter += i;
                                        markerletter = String.fromCharCode(markerletter);
                                        createMarker(directionsDisplay.getMap(), legs[legs.length - 1].end_location, "marker" + i, "some text for the " + i + "marker<br>" + legs[legs.length - 1].end_address, markerletter, i);
                                    }
                                }
                            });
                        })(k);
                    }
                }
            };
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


        icons["change"] = new google.maps.MarkerImage("http://www.google.com/mapfiles/ms/micons/green.png",
        // This marker is 20 pixels wide by 34 pixels tall.
      new google.maps.Size(25, 34),
        // The origin for this image is 0,0.
      new google.maps.Point(0, 0),
        // The anchor for this image is at 6,20.
      new google.maps.Point(9, 34));

        function getMarkerImage(iconStr) {
            if ((typeof (iconStr) == "undefined") || (iconStr == null)) {
                iconStr = "red";
            }
            if (!icons[iconStr]) {
                icons[iconStr] = new google.maps.MarkerImage("http://www.google.com/mapfiles/marker" + iconStr + ".png",
                // This marker is 20 pixels wide by 34 pixels tall.
      new google.maps.Size(20, 34),
                // The origin for this image is 0,0.
      new google.maps.Point(0, 0),
                // The anchor for this image is at 6,20.
      new google.maps.Point(9, 34));
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

            google.maps.event.addListener(marker, 'click', function () {
                //infowindow.setContent(contentString);
                //infowindow.open(map, marker);
                marker.setIcon('http://www.google.com/mapfiles/ms/micons/yellow.png')
                var posMarker = pos;
                if ($('#txtstar').val() == '') {
                    //$('#txtstar').val(marker.position.k + ',' + marker.position.B);
                    $('#txtstar').val(posMarker);
                    //marker.setMap(null);
                }
                else
                //$('#txtend').val(marker.position.k + ',' + marker.position.B);
                    $('#txtend').val(posMarker);
            });
            return marker;
        }
        // { "Geometry" :{"Latitude":-32.89960561948946, "Longitude":-68.83769273757934}},{ "Geometry" :{"Latitude":-32.89994792981964, "Longitude":-68.83391618728637}},{ "Geometry" :{"Latitude":-32.900110076356356, "Longitude":-68.82883071899414}},{ "Geometry" :{"Latitude":-32.901155013583136, "Longitude":-68.82758617401123}},{ "Geometry" :{"Latitude":-32.90209184337356, "Longitude":-68.82758617401123}},{ "Geometry" :{"Latitude":-32.90205581164179, "Longitude":-68.82483959197998}},{ "Geometry" :{"Latitude":-32.902398112501665, "Longitude":-68.82056951522827}},{ "Geometry" :{"Latitude":-32.90618134966698, "Longitude":-68.82089138031006}},{ "Geometry" :{"Latitude":-32.91338706852444, "Longitude":-68.82365942001343}},{ "Geometry" :{"Latitude":-32.914521915783936, "Longitude":-68.82400274276733}}

        //{ "Geometry" :{"Latitude":-32.91536853839044, "Longitude":-68.79872560501098}},{ "Geometry" :{"Latitude":-32.91506231412712, "Longitude":-68.81112813949585}},{ "Geometry" :{"Latitude":-32.91999779965203, "Longitude":-68.81423950195312}}
    </script>
    <div id="DivDescripcionPto" style="display: inline; background-color: White;">
        <table border="0" cellpadding="0" cellspacing="0" style="text-align: left; width: 95%;
            margin-top: 5px; table-layout: fixed">
            <tr>
                <td align="center">
                    <asp:Label ID="Label2" runat="server" Text="ALTA DE RECORRIDOS"></asp:Label>
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="4" style="text-align: left; width: 95%;
            margin-top: 5px; border: 1px solid black;">
            <tr>
                <td rowspan="6" align="center" style="padding: 5px; width: 80px" valign="middle">
                    <img src="images/autobus.jpg" alt="" width="65px" />
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                    <asp:Label ID="Label6" runat="server" Style="font-size: x-small; font-weight: bold"
                        Text="Empresa:"></asp:Label>
                </td>
                <td style="width: 220px">
                    <input id="txtEmpresa" type="text" style="width: 95%" />
                </td>
                <td style="width: 100px">
                    <asp:Label ID="Label8" runat="server" Style="font-size: x-small; font-weight: bold"
                        Text="Recorrido:"></asp:Label>
                </td>
                <td>
                    <select id="cboTipoRecorrido">
                        <option value="IDA" selected="selected">IDA</option>
                        <option value="VUELTA">VUELTA</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" Style="font-size: x-small; font-weight: bold"
                        Text="Línea:"></asp:Label>
                </td>
                <td>
                    <input id="txtLinea" type="text" style="width: 95%" title="Formato de Información: Nro 1 (SAN JOSE - DORREGO)" />
                </td>
                <td>
                    <asp:Label ID="Label5" runat="server" Style="font-size: x-small; font-weight: bold"
                        Text="Turno:"></asp:Label>
                </td>
                <td>
                    <input id="txtTurno" type="text" style="width: 94%" title="Formato de información: 1 Y 2 o 1 " />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label10" runat="server" Style="font-size: x-small; font-weight: bold"
                        Text="Salida:"></asp:Label>
                </td>
                <td>
                    <input id="txtHorarioSalida" type="text" style="width: 95%" />
                </td>
                <td>
                    <asp:Label ID="Label9" runat="server" Style="font-size: x-small; font-weight: bold"
                        Text="LLegada:"></asp:Label>
                </td>
                <td>
                    <input id="txtHorarioLlegada" type="text" style="width: 94%" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label12" runat="server" Style="font-size: x-small; font-weight: bold"
                        Text="Tipo Unidad:"></asp:Label>
                </td>
                <td>
                    <select id="cboTipoUnidad">
                        <option value="Minubus" selected="selecMinu">Minubus</option>
                        <option value="Omnibus">Omnibus</option>
                    </select>
                </td>
                <td>
                    <asp:Label ID="Label1" runat="server" Style="font-size: x-small; font-weight: bold"
                        Text="Tipo:"></asp:Label>
                </td>
                <td>
                    <select id="cboTipoTurno">
                        <option value="TURNO" selected="selected">TURNO</option>
                        <option value="DIURNO">DIURNO</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" Style="font-size: x-small; font-weight: bold"
                        Text="Archivo Kml:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:FileUpload ID="FileUpload2" runat="server" Style="display: none" />
                </td>
            </tr>
        </table>
    </div>
    <div style="display: none">
        <input type="text" id="txtstar" value="" />
        <input type="text" id="txtend" value="" />
        <input type="text" id="newPoints" style="width: 600px;" value='' />
    </div>
    <div id="map" style="height: 450px; width: 800px; margin-top: 5px; margin-bottom: 5px">
    </div>
    <asp:Button ID="btnReemplazar" runat="server" Text="Guardar Recorrido" Mensaje="Buscando Legajos solicitados..."
        OnClientClick="GuardarDatos();return false;" SkinID="btnConosud" />
    <script src="http://www.google-analytics.com/urchin.js" type="text/javascript">
    </script>
    <script type="text/javascript">
        _uacct = "UA-162157-1";
        urchinTracker();
    </script>
</asp:Content>
