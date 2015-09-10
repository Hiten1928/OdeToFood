(function () {
    var app = angular.module("order", []);

    app.controller("OrderController", ["$http", function ($http) {

        var self = this;
        this.getOrders = function () {
            $http.get("/OdeToFood.Web/api/order").then(function (response) {
                self.orders = response.data;
            });
        }
        this.orders = this.getOrders();
    }
    ]);

}());
