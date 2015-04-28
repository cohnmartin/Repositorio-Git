
var myAppModule = angular.module('myApp', []);



myAppModule.service('PageMethodsDomicilios', function ($http) {

    this.getDomicilios = function () {

        return $http({
            method: 'POST',
            url: 'ws_DomiciliosPersonalYPF.asmx/getDomicilios',
            data: {},
            contentType: 'application/json; charset=utf-8'
        });
    };



    this.EliminarRuta = function (id) {

        return $http({
            method: 'POST',
            url: 'ws_DomiciliosPersonalYPF.asmx/EliminarRuta',
            data: { idRecorrido: id },
            contentType: 'application/json; charset=utf-8'
        });
    };

    this.GrabarDomicilio = function (domicilio) {

        return $http({
            method: 'POST',
            url: 'ws_DomiciliosPersonalYPF.asmx/GrabarDomicilio',
            data: { domicilio: domicilio },
            contentType: 'application/json; charset=utf-8'
        });
    };

    this.GrabarReUbicacionDomicilio = function (domicilio) {

        return $http({
            method: 'POST',
            url: 'ws_DomiciliosPersonalYPF.asmx/GrabarReUbicacionDomicilio',
            data: { domicilio: domicilio },
            contentType: 'application/json; charset=utf-8'
        });
    };

});

myAppModule.controller('controller_domicilios', function ($scope, PageMethodsDomicilios) {
    $scope.Domicilios;
    $scope.Current;
    $scope.textSearch;
    $scope.textSearchTipo;
    $scope.searchDomicilio = null;
    $scope.Poblaciones;
    $scope.filteredDom;



    $scope.Filtrar = function () {
        //alert($scope.textSearch);
        //            PageMethods.filtrarVehiculos($scope.textSearch)
        //                        .then(function (response) {
        //                            $scope.Vehiculos = response.data.d;
        //                        });

    };


    $scope.BuscarDomicilios = function () {

        PageMethodsDomicilios.getDomicilios()
                    .then(function (response) {
                        $scope.Domicilios = response.data.d.Dom;
                        $scope.Poblaciones = response.data.d.Pob;
                    });

    };

    //    $scope.TransformarFecha = function (fecha) {

    //        if (fecha != "") {
    //            dia = fecha.substr(0, 2);
    //            mes = parseInt(fecha.substr(3, 2)) - 1 + '';
    //            año = fecha.substr(6);
    //            return new Date(año, mes, dia);
    //        }
    //        else
    //            null

    //    };

    //    $scope.getContextoClasificaciones = function () {

    //        PageMethods.getContextoClasificaciones()
    //                    .then(function (response) {
    //                        $scope.Clasificaciones = response.data.d;
    //                    });

    //    };

    //    $scope.GrabarVehiculo = function () {
    //        if (Page_ClientValidate()) {
    //            /// Codigo necesario para el control fecha de telerik, ya que no funciona el ng-model.
    //            $scope.Current.VtoTarjVerde = $find(Constants.controltxtVtoTarjVerde).get_selectedDate();
    //            $scope.Current.VtoRevTecnica = $find(Constants.controltxtVtoRevTecnica).get_selectedDate();
    //            $scope.Current.VelocimetroFecha = $find(Constants.controltxtVelocimetroFecha).get_selectedDate();

    //            PageMethods.GrabarVehiculo($scope.Current)
    //                    .then(function (response) {
    //                        if (response.data.d == true) {
    //                            $scope.BuscarVehiculos();
    //                            $find(Constants.controlPopUp).CloseWindows();
    //                        }
    //                    });
    //        }

    //    };

    //    $scope.BajaVehiculo = function (vehiculo) {


    //        PageMethods.BajaVehiculo(vehiculo.Id)
    //                    .then(function (response) {
    //                        if (response.data.d == true) {
    //                            $scope.BuscarVehiculos();
    //                        }
    //                    });
    //    };

    $scope.CargarTodos = function () {

        for (var i = 0; i < $scope.filteredDom.length; i++) {
            if ($scope.filteredDom[i].Latitud != null) {
                if ($scope.filteredDom[i].LatitudReposicion == null) {
                    MostrarUbicacionMapa($scope.filteredDom[i].Latitud + '', $scope.filteredDom[i].Longitud + '', $scope.filteredDom[i].Domicilio + ' ' + $scope.filteredDom[i].Distrito + ' ' + $scope.filteredDom[i].Poblacion);
                }
                else {
                    MostrarUbicacionMapa($scope.filteredDom[i].LatitudReposicion + '', $scope.filteredDom[i].LongitudReposicion + '', $scope.filteredDom[i].Domicilio + ' ' + $scope.filteredDom[i].Distrito + ' ' + $scope.filteredDom[i].Poblacion);
                }

            }
        }

    }

    $scope.MostrarUbicacion = function (domicilio) {

        LimpiarMarcadoresDomicilio();
        $scope.searchDomicilio = domicilio;
        MostrarUbicacionMapa(domicilio.Latitud + '', domicilio.Longitud + '', domicilio.Domicilio + ' ' + domicilio.Distrito + ' ' + domicilio.Poblacion);

    }

    $scope.MostrarReUbicacion = function (domicilio) {

        LimpiarMarcadoresDomicilio();
        $scope.searchDomicilio = domicilio;
        MostrarUbicacionMapa(domicilio.LatitudReposicion + '', domicilio.LongitudReposicion + '', domicilio.Domicilio + ' ' + domicilio.Distrito + ' ' + domicilio.Poblacion);

    }

    $scope.GrabarDomicilioUbicado = function (lat, lon) {

        $scope.searchDomicilio.Latitud = lat;
        $scope.searchDomicilio.Longitud = lon;

        PageMethodsDomicilios.GrabarDomicilio($scope.searchDomicilio)
                    .then(function (response) {
                        $scope.$digest();
                    });
    }

    $scope.GrabarReUbicacionDomicilio = function (lat, lon) {

        $scope.searchDomicilio.LatitudReposicion = lat;
        $scope.searchDomicilio.LongitudReposicion = lon;

        PageMethodsDomicilios.GrabarReUbicacionDomicilio($scope.searchDomicilio)
                    .then(function (response) {
                        $scope.MostrarReUbicacion($scope.searchDomicilio);
                        $scope.searchDomicilio = null;
                        $scope.$digest();
                    });
    }

    $scope.UbicarDomicilioMapa = function (domicilio) {

        $scope.searchDomicilio = domicilio;
        UbicarDomicilio(domicilio.Domicilio + ' ' + domicilio.Poblacion + ' ' + domicilio.Distrito);

    }

    $scope.EliminarRuta = function (idRecorrido) {

      

        PageMethodsDomicilios.EliminarRuta(idRecorrido)
                    .then(function (response) {
                        $scope.$digest();
                    });

        $scope.CancelarEdicion();

    }

    $scope.GrabarDomicilio = function () {

        $scope.Current.Latitud = null;
        $scope.Current.Longitud = null;

        PageMethodsDomicilios.GrabarDomicilio($scope.Current)
                    .then(function (response) {
                        $scope.$digest();
                    });

        $scope.CancelarEdicion();

    }

    $scope.CancelarEdicion = function () {
        $scope.Current = null;
        angular.element("#tblEdicion").css('display', 'none');
        $("#txtNombre").parentsUntil("tr").parent().find("span").css("display", "inline");

        $("#" + Constants.controlImgCancelar).css("display", "none");
        $("#txtNombre").css("display", "none");
        $("#txtDomicilio").css("display", "none");
        $("#txtPoblacion").css("display", "none");
        $("#txtDistrito").css("display", "none");
        $("#txtTipo").css("display", "none");
        $("#" + Constants.controlImgGrabar).css("display", "none");

    }
    $scope.Editar = function ($event, domicilio) {

        $("#" + Constants.controlImgCancelar).css("display", "inline");
        $("#txtNombre").css("display", "inline");
        $("#txtDomicilio").css("display", "inline");
        $("#txtPoblacion").css("display", "inline");
        $("#txtDistrito").css("display", "inline");
        $("#txtTipo").css("display", "inline");
        $("#" + Constants.controlImgGrabar).css("display", "inline");

        if ($("#txtNombre").val() != "") {
            $("#txtNombre").parentsUntil("tr").parent().find("span").css("display", "inline");
        }

        angular.element($event.target).parentsUntil("tr").parent().find("span").css("display", "none");


        $("#" + Constants.controlImgCancelar).appendTo(angular.element($event.target).parent().parent().parent().parent().children()[0]);
        $("#txtNombre").appendTo(angular.element($event.target).parent().parent().parent().parent().children()[1]);
        $("#txtDomicilio").appendTo(angular.element($event.target).parent().parent().parent().parent().children()[2]);
        $("#txtPoblacion").appendTo(angular.element($event.target).parent().parent().parent().parent().children()[3]);
        $("#txtDistrito").appendTo(angular.element($event.target).parent().parent().parent().parent().children()[4]);
        $("#txtTipo").appendTo(angular.element($event.target).parent().parent().parent().parent().children()[5]);
        $("#" + Constants.controlImgGrabar).appendTo(angular.element($event.target).parent().parent().parent().parent().children()[6]);

        $scope.Current = domicilio;



    };

    //    $scope.NuevoVehiculo = function () {

    //        $scope.Current = {};
    //        $find(Constants.controlPopUp).set_CollectionDiv('divPrincipal');
    //        $find(Constants.controlPopUp).ShowWindows('divPrincipal', "Nuevo Vehículo ");

    //    };

    $scope.BuscarDomicilios();
    //$scope.getContextoClasificaciones();

});


