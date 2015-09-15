
angular.module("kendo", ["kendo.directives"])
    .controller("KendoController", function ($scope) {
        $scope.getType = function (x) {
            return typeof x;
        };
        $scope.isDate = function (x) {
            return x instanceof Date;
        };
        $scope.datePickerOptions = {
            format: "M/d/yyyy h:mm:ss"
        }
    })
