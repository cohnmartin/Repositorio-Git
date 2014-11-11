var myAppModule = angular.module('myApp', ['ngSanitize', 'ui.select']);

myAppModule.service('pageMethods_HojaRuta', function ($http) {

    this.getItemsHojaRuta = function (idCab) {
        return $http({
            method: 'POST',
            url: 'ws_HojaRuta.asmx/getItemsHojaRuta',
            data: { IdCab: idCab },
            contentType: 'application/json; charset=utf-8'
        });
    };


});

myAppModule.controller('controller_HojaRuta', function ($scope, pageMethods_HojaRuta) {
    $scope.ItemsHoja = new Object();
    $scope.idCab = Constants.idCabeceraHojaRuta;

    $scope.getItemsHojaRuta = function () {

        // Cargo los contratos ya asignados.
        pageMethods_HojaRuta.getItemsHojaRuta($scope.idCab)
                    .then(function (response) {
                        $scope.ItemsHoja = response.data.d;
                    });
    };



    $scope.getItemsHojaRuta();

    


});

