(function () {

    var app = angular.module("restaurantBooking", ['ngRoute', "restaurant", "table", "order", "review", "kendo"]);


    app.config([
        '$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
            var baseSiteUrlPath = $("base").first().attr("href");
            var baseTemplateUrl = baseSiteUrlPath + "Ng/templates/";

            $routeProvider.when('/angular/',
            {
                templateUrl: baseTemplateUrl + 'index.html'
            });

            $routeProvider.when('/angular/index',
            {
                templateUrl: baseTemplateUrl + 'index.html'
            });

            $routeProvider.when('/angular/restaurants',
            {
                templateUrl: baseTemplateUrl + 'restaurantsCrud.html',
                controller: 'RestaurantController'
            });

            $routeProvider.when('/angular/orders',
            {
                templateUrl: baseTemplateUrl + 'ordersCrud.html'
            });

            $routeProvider.when('/angular/reviews',
            {
                templateUrl: baseTemplateUrl + 'reviewsCrud.html'
            });

            $routeProvider.when('/angular/page-not-found',
            {
                templateUrl: baseTemplateUrl + 'page-not-found.html',
            });

            $routeProvider.otherwise({
                redirectTo: function () {

                    if (window.location.pathname == baseSiteUrlPath || window.location.pathname == baseSiteUrlPath + "angular") {
                        window.location = baseSiteUrlPath + "angular/index";
                    } else {
                        window.location = baseSiteUrlPath + "angular/page-not-found";
                    }
                },
            });
            $locationProvider.html5Mode(true);
        }
    ]);

    app.controller("MainController", function () {
        this.secondRowToggle = "avialable tables";
        var self = this;

        this.toggleHome = function () {
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