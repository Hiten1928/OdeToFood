﻿(function () {
    var app = angular.module("restaurant", []);

    app.controller("RestaurantController", ["$http", function ($http) {
        var self = this;
        this.selected = 0;
        this.currentRestaurant = null;
        this.order = {};
        this.submitStatus = null;
        this.restaurantById = null;//restaurant we received by id
        this.restaurantVm = {};

        var roundHour = function (date) {
            date.setHours(date.getHours() + 1);
            date.setMinutes(0);
            date.setSeconds(0);
            return date;
        }

        this.getAllRestaurants = function () {
            self.currentRestaurant = null;
            self.restaurants = [];
            $http.get("/OdeToFood.Web/api/restaurant").then(function (response) {
                self.restaurants = response.data;
            });
        };
        this.getRestaurant = function (restaurantName) {
            $http.get("/OdeToFood.Web/api/restaurant?restaurantName=" + restaurantName).then(function (response) {
                self.currentRestaurant = response.data;
            });
        };
        this.getRestaurantById = function (restaurantId) {
            alert(restaurantId);
            $http.get("/OdeToFood.Web/api/restaurant?id=" + restaurantId).then(function (response) {
                self.restaurantById = response.data;
            });
        };

        


        this.select = function (index) {
            self.selected = index;
        };
        this.placeOrder = function () {
            
            self.order.timeFrom = new Date(jQuery("#timeFrom").val());
            
            self.order.timeFrom = roundHour(self.order.timeFrom);
            
            self.order.timeTo = new Date(self.order.timeFrom);
            self.order.timeTo.setHours(self.order.timeFrom.getHours() + 1);
            console.log(self.order);
            $http.post("/OdeToFood.Web/api/order",self.order).success(function(response) {
                if (response.statusCode == 409) {
                    self.submitStatus = "Table is already taken for the specified time. Try to pick a different one.";
                    $("#submitStatus").html(self.submitStatus);
                } else {
                    self.submitStatus = "You have placed your order successfully";
                    $("#submitStatus").html(self.submitStatus);
                }
            }).error(function (response) {
                    self.submitStatus = "Sorry. The table is already taken.";
                    $("#submitStatus").html(self.submitStatus);

            });
        };
        this.setTableId = function(tableId) {
            self.order.tableId = tableId;
        };
        this.setTableNumberForm = function(tableNum){
            jQuery("#tableNumber").val(tableNum);
        };

        self.restaurants = this.getAllRestaurants();
        self.tabActive = 0;

        this.putRestaurant = function () {
            self.restaurantVm.name = jQuery("#restaurantmodelName").val();
            self.restaurantVm.location = jQuery("#restaurantModelLocation").val();
            self.restaurantVm.id = jQuery("#restaurantModelId").val();
            self.restaurantVm.tableCount = jQuery("#restaurantModelTableCount").val();
            console.log(self.restaurantVm);
            console.log(self.restaurantVm.id);
            $http.put("/OdeToFood.Web/api/restaurant?id=" + self.restaurantVm.id, self.restaurantVm).success(function (response) {
                jQuery("#editResponceMessage").text("You have updated restaurant successfully.");
            }).error(function() {
                jQuery("#editResponceMessage").text("Something went wrong.");
            });
        };

    }]);

}());


