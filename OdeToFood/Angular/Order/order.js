(function () {
    var app = angular.module("order", []);

    app.controller("OrderController", ["$http", "filterFilter", function ($http, filterFilter) {

        var self = this;
        this.inputMode = "create"; //used to determine whether to show save or update button for the input form

        this.getOrders = function () {
            //self.restaurant = null;
            $http.get("/OdeToFood.Web/api/order").then(function (response) {
                self.orders = response.data;
                jQuery.each(self.orders, function (key, value) {
                    var restaurantId = value.table.restaurantId;
                    $http.get("/OdeToFood.Web/api/restaurant?id=" + restaurantId).then(function (result) {
                        value.restaurantName = result.data.name;
                    });
                });
            });
        }

        this.orders = this.getOrders();

        this.orderModel = {};

        this.createOrder = function () {
            var time = self.orderModel.timeFrom;
            if (!self.orderModel.peopleCount) {
                alert("Enter number of people");
            }
            else if (!self.orderModel.timeFrom) {
                alert("Enter time from");
            }
            else if (!self.orderModel.timeTo) {
                alert("Enter time to");
            }
            else if (!self.orderModel.restaurantName) {
                alert("Enter restaurant name");
            }
            else {
                var nameAndLocationArr = self.orderModel.restaurantName.split("_");

                $http.get("/OdeToFood.Web/api/restaurant").then(function (response) {
                    var restaurants = response.data;
                    var targetRestaurant = filterFilter(restaurants, { name: nameAndLocationArr[0], location: nameAndLocationArr[1] });

                    $http.get("/OdeToFood.Web/api/table?dateTime=" + time + "&restaurantId=" + targetRestaurant[0].id).then(function (response) {
                        var targetTable = response.data[0];

                        if (targetTable) {
                            console.log(targetTable.id);
                            self.orderModel.tableId = targetTable.id;

                            self.orders.push({
                                'id': 1,
                                'peopleCount': self.orderModel.peopleCount,
                                'timeFrom': self.orderModel.timeFrom,
                                'timeTo': self.orderModel.timeTo,
                                'tableId': self.orderModel.tableId,
                                'restaurantName': self.orderModel.restaurantName.split("_")[0]
                            });


                            $http.post("/OdeToFood.Web/api/order", self.orders[self.orders.length - 1]).then(function (response) {

                            });

                            self.orderModel.id = '';
                            self.orderModel.peopleCount = '';
                            self.orderModel.timeFrom = '';
                            self.orderModel.timeTo = '';
                            self.orderModel.restaurantName = '';

                        } else {
                            alert("There are no avialable tables at the restaurant");
                        }
                    });
                });
            }
        };

        var key;
        this.edit = function (order, indx) {
            self.inputMode = "update";
            key = indx;
            self.orderModel.id = order.id;
            self.orderModel.peopleCount = order.peopleCount;
            self.orderModel.timeFrom = order.timeFrom;
            self.orderModel.timeTo = order.timeTo;
            self.orderModel.tableId = order.tableId;

            $http.get("/OdeToFood.Web/api/table?id=" + self.orderModel.tableId).then(function (r) {
                var table = r.data;

                $http.get("/OdeToFood.Web/api/restaurant?id=" + table.restaurantId).then(function (response) {
                    self.orderModel.restaurantName = response.data.name + "_" + response.data.location;
                });
            });


        };

        this.updateOrder = function (id, num, tFrom, tTo, tTableId) {
            var order = {};
            order.id = id;
            order.peopleCount = num;
            order.timeFrom = tFrom;
            order.timeTo = tTo;
            order.tableId = tTableId;

            $http.put("/OdeToFood.Web/api/order/", order).success(function(response) {
                alert(response);
                self.orders[key].id = id;
                self.orders[key].peopleCount = num;
                self.orders[key].timeFrom = tFrom;
                self.orders[key].timeTo = tTo;
                self.orders[key].tableId = tTableId;
            }).error(function(response) {
                alert(response.message);
            });

            self.orderModel.id = '';
            self.orderModel.peopleCount = '';
            self.orderModel.timeFrom = '';
            self.orderModel.timeTo = '';
            self.orderModel.tableId = '';
        };

        this.cancelUpdate = function () {
            self.orderModel.id = '';
            self.orderModel.peopleCount = '';
            self.orderModel.timeFrom = '';
            self.orderModel.timeTo = '';
            self.orderModel.restaurantName = '';
            self.inputMode = "create";
        };

        this.del = function (id) {
            var orderToDelete = self.orders[id];
            $http.delete("/OdeToFood.Web/api/order?id=" + orderToDelete.id).then(function (response) {

            });
            self.orders.splice(id, 1);
        };
    }
    ]);

}());
