(function () {
    var app = angular.module("restaurant", []);

    app.controller("RestaurantController", ["$http", function ($http) {
        var self = this;
        this.inputMode = "create";
        this.restaurantModel = {};
        this.order = {};

        this.getAllRestaurants = function () {
            self.currentRestaurant = null;
            self.restaurants = [];
            $http.get("/OdeToFood.Web/api/restaurant").then(function (response) {
                self.restaurants = response.data;
            });
        };

        function roundHour(date) {

            date.setHours(date.getHours()+1);
            date.setMinutes(0);

            return date;
        }

        this.restaurants = this.getAllRestaurants();

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

        this.createRestaurant = function () {
            if (!self.restaurantModel.name) {
                alert("Enter restaurant name");
            }
            else if (!self.restaurantModel.location) {
                alert("Enter trestaurant location");
            }
            else {
                var tablesForRestaurant = [];
                for (var i = 0; i < self.restaurantModel.tableCount; i++) {
                    var table = {};
                    table.tableNumber = i + 1;
                    tablesForRestaurant.push(table);
                }

                var restaurant = { name: self.restaurantModel.name, location: self.restaurantModel.location, tables: tablesForRestaurant };

                $http.post("/OdeToFood.Web/api/restaurant", restaurant).success(function (response) {
                    alert("Successfully inserted a new record.");
                    self.restaurants.push({
                        'id': 1,
                        'name': self.restaurantModel.name,
                        'location': self.restaurantModel.location,
                        'tables': tablesForRestaurant
                    });
                }).error(function (response) {
                    alert("Sorry. There was an error.");
                });

                self.restaurantModel.id = '';
                self.restaurantModel.peopleCount = '';
                self.restaurantModel.timeFrom = '';
                self.restaurantModel.timeTo = '';
                self.restaurantModel.restaurantName = '';
            }
        };

        var key;
        this.edit = function (restaurant, indx) {
            self.inputMode = "update";
            key = indx;
            self.restaurantModel.id = restaurant.id;
            self.restaurantModel.name = restaurant.name;
            self.restaurantModel.location = restaurant.location;
            self.restaurantModel.tableCount = restaurant.tables.length;
        };

        this.updateRestaurant = function (id, name, location, tableCount) {
            var restaurant = {};
            restaurant.id = id;
            restaurant.name = name;
            restaurant.location = location;
            restaurant.tableCount = tableCount;

            $http.put("/OdeToFood.Web/api/restaurant/", restaurant).success(function (response) {
                alert("Successfully modified restaurant record.");
                self.restaurants[key].id = id;
                self.restaurants[key].name = name;
                self.restaurants[key].location = location;

                self.restaurantModel.id = '';
                self.restaurantModel.name = '';
                self.restaurantModel.location = '';

            }).error(function (response) {
                alert(response.message);
            });
        };

        this.cancelUpdate = function () {
            self.restaurantModel.id = '';
            self.restaurantModel.name = '';
            self.restaurantModel.location = '';
            self.restaurantModel.tableCount = '';
            self.inputMode = "create";
        };

        this.del = function (id) {
            var restaurantToDelete = self.restaurants[id];

            $http.delete("/OdeToFood.Web/api/restaurant?id=" + restaurantToDelete.id).then(function (response) {

            });
            self.restaurants.splice(id, 1);
        };


        this.placeOrder = function () {
            self.order.timeFrom = new Date(jQuery("#timeFrom").val());
            self.order.timeFrom = roundHour(self.order.timeFrom);
            self.order.timeTo = new Date(self.order.timeFrom);
            self.order.timeTo.setHours(self.order.timeFrom.getHours() + 1);
            console.log(self.order);
            $http.post("/OdeToFood.Web/api/order", self.order).success(function (response) {
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
        this.setTableId = function (tableId) {
            self.order.tableId = tableId;
        };
        this.setTableNumberForm = function (tableNum) {
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
            }).error(function () {
                jQuery("#editResponceMessage").text("Something went wrong.");
            });
        };
    }]);
}());


