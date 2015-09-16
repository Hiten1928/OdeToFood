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
                        //console.log(targetRestaurant[0].id);

                        var reviewToAdd = {
                            id: 1,
                            rating: self.reviewModel.rating,
                            body: self.reviewModel.body,
                            reviewerName: self.reviewModel.reviewerName,
                            restaurantId: targetRestaurant[0].id,
                            restaurant: targetRestaurant[0]
                        };
                        alert("restId: " + reviewToAdd.restaurantId);
                        alert("RestName: " + reviewToAdd.restaurant.name);

                        $http.post("/OdeToFood.Web/api/restaurantreview", reviewToAdd).success(function (response) {
                            self.reviews.push(reviewToAdd);
                            alert("You have added review successfully");
                        }).error(function () {
                            alert("Something went wrong. Review has not been added.");
                        });

                        self.reviewModel.id = '';
                        self.reviewModel.body = '';
                        self.reviewModel.reviewerName = '';
                        self.reviewModel.restaurantName = '';
                    });
                }
            };
            var key;
            this.edit = function (review, indx) {
                self.inputMode = "update";
                key = indx;
                self.reviewModel.id = review.id;
                self.reviewModel.rating = review.rating;
                self.reviewModel.body = review.body;
                self.reviewModel.reviewerName = review.reviewerName;
                self.reviewModel.restaurantName = review.restaurant.name + "_" + review.restaurant.location;

                alert(review.restaurant.name + "_" + review.restaurant.location);
            };
            this.updateReview = function (id, rating, body, reviewerName, restaurantName) {
                var review = {};
                review.id = id;
                review.rating = rating;
                review.body = body;
                review.reviewerName = reviewerName;

                var nameAndLocationArr = restaurantName.split("_");

                $http.get("/OdeToFood.Web/api/restaurant/").then(function(response) {
                    var restaurants = response.data;
                    var targetRestaurant = filterFilter(restaurants, { name: nameAndLocationArr[0], location: nameAndLocationArr[1] });

                    review.restaurant = targetRestaurant[0];
                    review.restaurantId = targetRestaurant[0].id;
                    console.log(review);

                    $http.put("/OdeToFood.Web/api/restaurantreview/", review).success(function (result) {
                        self.reviews[key].id = id;
                        self.reviews[key].rating = rating;
                        self.reviews[key].body = body;
                        self.reviews[key].reviewerName = reviewerName;
                        self.reviews[key].restaurant = targetRestaurant[0];
                        self.reviews[key].restaurantId = targetRestaurant[0].id;

                        alert("Changes to the review has been successfully saved.");
                    }).error(function() {
                        alert("Something went wrong while updating the review.");
                    });
                });
                self.reviewModel.id = '';
                self.reviewModel.rating = '';
                self.reviewModel.body = '';
                self.reviewModel.reviewerName = '';
                self.reviewModel.restaurantName = '';

                self.inputMode = "create";
            };
            this.cancelUpdate = function () {
                self.reviewModel.id = '';
                self.reviewModel.rating = '';
                self.reviewModel.body = '';
                self.reviewModel.reviewerName = '';
                self.reviewModel.restaurantName = '';
                self.inputMode = "create";
            };
            this.del = function (id) {
                var reviewToDelete = self.reviews[id];
                $http.delete("/OdeToFood.Web/api/review?id=" + reviewToDelete.id).then(function (response) {

                });
                self.reviews.splice(id, 1);
            };
        }
    ]);
}());