(function () {
    var app = angular.module("table", []);

    app.controller("TableController", ["$http","$filter", function ($http,$filter) {
        var self = this;

        this.getAvialableTables = function (restId) {
            var date = new Date(jQuery("#dateAvialableTables").val());
//            $http.get("/OdeToFood.Web/api/table", { dateTime: date, restaurantId: restId }).then(function (response) {
//                self.avialableTables = response.data;
            //            });
            $http({
                method: "GET",
                url: "/OdeToFood.Web/api/table",
                data: {
                    dateTime:$filter('date')(date),
                    restaurantId : restId
                }
            }).then(function(response) {
                self.avialableTables = response.data;
                alert(self.avialableTables);
            });
        };
    }]);


}());