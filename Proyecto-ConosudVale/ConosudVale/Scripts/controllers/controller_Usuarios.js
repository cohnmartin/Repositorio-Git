var myAppModule = angular.module('myApp', ['ngSanitize', 'ui.select']);

myAppModule.filter('filterMultiple', ['$filter', function ($filter) {
    return function (items, keyObj) {
        var filterObj = {
            data: items,
            filteredData: [],
            applyFilter: function (obj, key) {
                var fData = [];
                if (this.filteredData.length == 0)
                    this.filteredData = this.data;
                if (obj) {
                    var fObj = {};
                    if (angular.isString(obj)) {
                        fObj[key] = obj;
                        fData = fData.concat($filter('filter')(this.filteredData, fObj));
                    } else if (angular.isArray(obj)) {
                        if (obj.length > 0) {
                            for (var i = 0; i < obj.length; i++) {
                                if (angular.isString(obj[i])) {
                                    fObj[key] = obj[i];
                                    fData = fData.concat($filter('filter')(this.filteredData, fObj));
                                }
                            }

                        }
                    }
                    if (fData.length > 0) {
                        this.filteredData = fData;
                    }
                }
            }
        };

        if (keyObj) {
            angular.forEach(keyObj, function (obj, key) {
                filterObj.applyFilter(obj, key);
            });
        }

        return filterObj.filteredData;
    }
} ]);

myAppModule.service('pageMethods_Usuarios', function ($http) {


    this.getContratosAsignados = function (idUsuario) {
        return $http({
            method: 'POST',
            url: 'ws_Usuarios.asmx/getContratosAsignados',
            data: { idUsuario: idUsuario },
            contentType: 'application/json; charset=utf-8'
        });
    };


    this.getContratosDisponibles = function (idUsuario) {
        return $http({
            method: 'POST',
            url: 'ws_Usuarios.asmx/getContratosDisponibles',
            data: { idUsuario: idUsuario },
            contentType: 'application/json; charset=utf-8'
        });
    };


    this.agregarContrato = function (datos) {
        return $http({
            method: 'POST',
            url: 'ws_Usuarios.asmx/agregarContrato',
            data: { idUsuario: datos.idUsuario, idContrato: datos.contrato.IdContrato },
            contentType: 'application/json; charset=utf-8'
        });
    };

    this.eliminarContrato = function (idContexto) {
        return $http({
            method: 'POST',
            url: 'ws_Usuarios.asmx/eliminarContrato',
            data: { idSegUsuario: idContexto },
            contentType: 'application/json; charset=utf-8'
        });
    };

    this.getUsuarios = function () {
        return $http({
            method: 'POST',
            url: 'ws_Usuarios.asmx/getUsuarios',
            data: { },
            contentType: 'application/json; charset=utf-8'
        });
    };


});

myAppModule.controller('controller_Usuarios', function ($scope, pageMethods_Usuarios) {
    $scope.NewContrato = new Object();
    $scope.NewContrato.idUsuario = Constants.usuarioEdicion;
    $scope.NewContrato.contrato;
    $scope.textSearch = '';
    $scope.Usuarios = '';
    $scope.ContratosAsignados;
    $scope.contratosDisponibles;
    $scope.textSearch1;
    $scope.textSearch2;
    $scope.textSearch3;

    $scope.getFilter = function () {
        return { Login: $scope.textSearch };
    }

    $scope.showControl = function () {
        $find(Constants.controlCombo);
    }

    $scope.EliminarContrato = function (idSegContexto) {

        pageMethods_Usuarios.eliminarContrato(idSegContexto).then(function (response) {
            $scope.ContratosAsignados = response.data.d;
        });
    }


    $scope.AgregarContrato = function () {

        pageMethods_Usuarios.agregarContrato($scope.NewContrato).then(function (response) {
            $scope.ContratosAsignados = response.data.d;
            $scope.NewContrato.contrato = new Object();
        });
    }



    //$scope.getContratosAsignados = function ($event) {
    $scope.getContratosAsignados = function () {

        /// por ahora desde aca cargo todos los contratos posiblres de seleccionar.
        // Cargo los contratos ya asignados.
        pageMethods_Usuarios.getContratosDisponibles($scope.NewContrato.idUsuario)
                    .then(function (response) {
                        $scope.contratosDisponibles = response.data.d;
                    });

        // Cargo los contratos ya asignados.
        pageMethods_Usuarios.getContratosAsignados($scope.NewContrato.idUsuario)
                    .then(function (response) {
                        $scope.ContratosAsignados = response.data.d;
                    });
    };

    $scope.getUsuarios = function () {

        /// por ahora desde aca cargo todos los contratos posiblres de seleccionar.
        // Cargo los contratos ya asignados.
        pageMethods_Usuarios.getUsuarios()
                    .then(
                    function (response) {
                        $scope.Usuarios = response.data.d;
                    }, function (data) {
                        alert(data.data);
                    });
    };

    if ($scope.NewContrato.idUsuario != '')
        $scope.getContratosAsignados();

    //$scope.getUsuarios();
});

