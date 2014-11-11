
var myAppModule = angular.module('myApp', []);

myAppModule.service('PageMethods', function ($http) {



    this.BajaVehiculo = function (Id) {

        return $http({
            method: 'POST',
            url: 'ws_VehiculosYPF.asmx/BajaVehiculo',
            data: { Id: Id },
            contentType: 'application/json; charset=utf-8'
        });
    };


    this.filtrarVehiculos = function (nroPatente) {

        return $http({
            method: 'POST',
            url: 'ws_VehiculosYPF.asmx/filtrarVehiculos',
            data: { nroPatente: nroPatente },
            contentType: 'application/json; charset=utf-8'
        });
    };


    this.getContextoClasificaciones = function () {

        return $http({
            method: 'POST',
            url: 'ws_VehiculosYPF.asmx/getContextoClasificaciones',
            data: {},
            contentType: 'application/json; charset=utf-8'
        });
    };

    this.getVehiculos = function () {

        return $http({
            method: 'POST',
            url: 'ws_VehiculosYPF.asmx/getVehiculos',
            data: {},
            contentType: 'application/json; charset=utf-8'
        });
    };

    this.GrabarVehiculo = function (vehiculo) {

        return $http({
            method: 'POST',
            url: 'ws_VehiculosYPF.asmx/GrabarVehiculo',
            data: { vehiculo: vehiculo },
            contentType: 'application/json; charset=utf-8'
        });
    };

});

myAppModule.controller('controller_vehiculos', function ($scope, PageMethods) {
    $scope.Vehiculos;
    $scope.Current;
    $scope.Clasificaciones;
    $scope.textSearch;



    $scope.Filtrar = function () {

        PageMethods.filtrarVehiculos($scope.textSearch)
                    .then(function (response) {
                        $scope.Vehiculos = response.data.d;
                    });

    };


    $scope.BuscarVehiculos = function () {

        PageMethods.getVehiculos()
                    .then(function (response) {
                        $scope.Vehiculos = response.data.d;
                    });

    };

    $scope.TransformarFecha = function (fecha) {

        if (fecha != "") {
            dia = fecha.substr(0, 2);
            mes = parseInt(fecha.substr(3, 2)) - 1 + '';
            año = fecha.substr(6);
            return new Date(año, mes, dia);
        }
        else
            null

    };

    $scope.getContextoClasificaciones = function () {

        PageMethods.getContextoClasificaciones()
                    .then(function (response) {
                        $scope.Clasificaciones = response.data.d;
                    });

    };

    $scope.GrabarVehiculo = function () {
        if (Page_ClientValidate()) {
            /// Codigo necesario para el control fecha de telerik, ya que no funciona el ng-model.
            $scope.Current.VtoTarjVerde = $find(Constants.controltxtVtoTarjVerde).get_selectedDate();
            $scope.Current.VtoRevTecnica = $find(Constants.controltxtVtoRevTecnica).get_selectedDate();
            $scope.Current.VelocimetroFecha = $find(Constants.controltxtVelocimetroFecha).get_selectedDate();

            PageMethods.GrabarVehiculo($scope.Current)
                    .then(function (response) {
                        if (response.data.d == true) {
                            $scope.BuscarVehiculos();
                            $find(Constants.controlPopUp).CloseWindows();
                        }
                    });
        }

    };

    $scope.BajaVehiculo = function (vehiculo) {


        PageMethods.BajaVehiculo(vehiculo.Id)
                    .then(function (response) {
                        if (response.data.d == true) {
                            $scope.BuscarVehiculos();
                        }
                    });
    };

    $scope.Editar = function (vehiculo) {

        $scope.Current = vehiculo;

        /// Codigo necesario para el control fecha de telerik, ya que no funciona el ng-model.
        $find(Constants.controltxtVtoTarjVerde).set_selectedDate($scope.TransformarFecha($scope.Current.VtoTarjVerde));
        $find(Constants.controltxtVtoRevTecnica).set_selectedDate($scope.TransformarFecha($scope.Current.VtoRevTecnica));
        $find(Constants.controltxtVelocimetroFecha).set_selectedDate($scope.TransformarFecha($scope.Current.VelocimetroFecha));



        $find(Constants.controlPopUp).set_CollectionDiv('divPrincipal');
        $find(Constants.controlPopUp).ShowWindows('divPrincipal', "Edición Vehículo " + vehiculo.Patente);

    };

    $scope.NuevoVehiculo = function () {

        $scope.Current = {};
        $find(Constants.controlPopUp).set_CollectionDiv('divPrincipal');
        $find(Constants.controlPopUp).ShowWindows('divPrincipal', "Nuevo Vehículo ");

    };

    $scope.BuscarVehiculos();
    $scope.getContextoClasificaciones();

});


