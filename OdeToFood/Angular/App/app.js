(function () {
    var app = angular.module("restaurantBooking", ["restaurant", "table", "order", "review"]);

    app.controller("MainController", function () {
        this.secondRowToggle = "avialable tables";
        var self = this;

        this.toggleHome = function() {
            self.secondRowToggle = "avialable tables";
        }
        this.toggleRestaurant = function () {
            self.secondRowToggle = "restaurants";
        }
        this.toggleOrder = function () {
            self.secondRowToggle = "orders";
        }
        this.toggleReview = function () {
            self.secondRowToggle = "reviews";
        }
    });
}());