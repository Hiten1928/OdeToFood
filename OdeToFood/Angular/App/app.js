(function () {
    var app = angular.module("restaurantBooking", ["restaurant", "table", "order", "review", "kendo"]);

    app.controller("MainController", function () {
        this.secondRowToggle = "avialable tables";
        var self = this;

        this.toggleHome = function() {
            self.secondRowToggle = "avialable tables";
            jQuery("#submitStatus").html("");
        }
        this.toggleRestaurant = function () {
            self.secondRowToggle = "restaurants";
            jQuery("#submitStatus").html("");
        }
        this.toggleOrder = function () {
            self.secondRowToggle = "orders";
            jQuery("#submitStatus").html("");
        }
        this.toggleReview = function () {
            self.secondRowToggle = "reviews";
            jQuery("#submitStatus").html("");
        }
    });
}());