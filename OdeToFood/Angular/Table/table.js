(function () {
    var app = angular.module("table", []);

    app.controller("TableController", ["$http", "$filter", function ($http, $filter) {
        var self = this;

        this.getAvialableTables = function (restId) {
            var date = jQuery("#dateAvialableTables").val();
            if (date == "") {
                jQuery("#tableListErrorMessage").text("Please, enter a date.");
                return;
            }
            else if (restId == null || restId.isUndefined) {
                jQuery("#tableListErrorMessage").text("Please, select a restaurant from the list above.");
                return;
            }

            $http.get("/OdeToFood.Web/api/table?restaurantId=" + restId + "&dateTime=" + date).then(function (response) {
                jQuery("#tableListErrorMessage").text("");
                self.avialableTables = response.data;
            });
        };
    }]);


}());