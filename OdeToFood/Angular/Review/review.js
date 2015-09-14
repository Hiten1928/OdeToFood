(function () {
    var app = angular.module("review", []);

    app.controller("ReviewController", [
        "$http", "filterFilter", function ($http, filterFilter) {
            var self = this;
            this.inputMode = "create";

            this.getReviews = function () {
                $http.get("/OdeToFood.Web/api/restaurantreview").then(function (response) {
                    self.reviews = response.data;
                });
            };

            this.reviews = this.getReviews();

            this.reviewModel = {};

            this.createReview = function () {
                if (!self.reviewModel.rating) {
                    alert("Enter rating");
                }
                else if (!self.reviewModel.body) {
                    alert("Enter review body");
                }
                else if (!self.reviewModel.reviewerName) {
                    alert("Enter your name");
                }
                else if (!self.reviewModel.restaurantName) {
                    alert("Enter restaurant name");
                }
                else {
                    var nameAndLocationArr = self.reviewModel.restaurantName.split("_");

                    $http.get("/OdeToFood.Web/api/restaurant").then(function (response) {
                        var restaurants = response.data;
                        var targetRestaurant = filterFilter(restaurants, { name: nameAndLocationArr[0], location: nameAndLocationArr[1] });

                        var reviewToAdd = {
                            id: 1,
                            rating: self.reviewModel.rating,
                            body: self.reviewModel.body,
                            reviewerName: self.reviewModel.reviewerName,
                            restaurantId: targetRestaurant.id,
                            restaurant: targetRestaurant
                        };

                        $http.post("/OdeToFood.Web/api/restaurantreview", reviewToAdd).success(function(response) {
                            self.reviews.push(reviewToAdd);
                            alert("You have added review successfully");
                        }).error(function() {
                            alert("Something went wrong. Review has not been added.");
                        });

                        self.reviewModel.id = '';
                        self.reviewModel.body = '';
                        self.reviewModel.reviewerName = '';
                        self.reviewModel.restaurantName = '';


                    });

                }
            };
        }
    ]);
}());