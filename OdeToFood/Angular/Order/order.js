(function () {
    var app = angular.module("order", []);

    app.controller("OrderController", ["$http", function ($http) {

        var self = this;
        this.inputMode = "create"; //used to determine whether to show save or update button for the input form

        this.getOrders = function () {
            $http.get("/OdeToFood.Web/api/order").then(function (response) {
                self.orders = response.data;
            });
        }

        this.orders = this.getOrders();

        this.orderModel = {};

        this.createOrder = function () {
            if (!self.orderModel.peopleCount) {
                alert("Enter number of people");
            }
            else if (!self.orderModel.timeFrom) {
                alert("Enter time from");
            }
            else if (!self.orderModel.timeTo) {
                alert("Enter time to");
            }
            else if (!self.orderModel.tableId) {
                alert("Enter table id");
            }
            else {
                self.orders.push({
                    'id': self.orderModel.id,
                    'peopleCount': self.orderModel.peopleCount,
                    'timeFrom': self.orderModel.timeFrom,
                    'timeTo': self.orderModel.timeTo,
                    'tableId': self.orderModel.tableId
                });
                self.orderModel.id = '';
                self.orderModel.peopleCount = '';
                self.orderModel.timeFrom = '';
                self.orderModel.timeTo = '';
                self.orderModel.tableId = '';
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
        };

        this.updateOrder = function (id, num, tFrom, tTo, tId) {
            self.orders[key].id = id;
            self.orders[key].peopleCount = num;
            self.orders[key].timeFrom = tFrom;
            self.orders[key].timeTo = tTo;
            self.orders[key].tableId = tId;
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
            self.orderModel.tableId = '';
            self.inputMode = "create";
        };

        this.del = function (id) {
            self.orders.splice(id, 1);
        };
    }
    ]);

}());
