(function() {
    var app = angular.module("review", []);

    app.controller("ReviewController", [
        "$http", function($http) {
            var self = this;
            this.getReviews = function () {
                $http.get("/OdeToFood.Web/api/restaurantreview").then(function (response) {
                    self.reviews = response.data;
                });
            }
            this.reviews = this.getReviews();
        }
    ]);
}());