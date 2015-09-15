using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using NUnit.Framework;
using OdeToFood.Controllers.API;
using OdeToFood.Data;
using OdeToFood.Data.Models;
using OdeToFood.Data.Repositories;

namespace OdeToFood.Tests.API_Tests
{
    [TestFixture]
    public class RestaurantReviewApiControllerTest
    {
        private readonly DataContext _dataContext;
        private readonly OdeToFoodContext _context;

        public RestaurantReviewApiControllerTest()
        {
            _context = new OdeToFoodContext();
            var restaurantRepository = new RestaurantRepository(_context);
            var restaurantReviewRepository = new RestaurantReviewRepository(_context);
            var orderRepository = new OrderRepository(_context);
            var tableRepository = new TableRepository(_context);
            _dataContext = new DataContext(restaurantRepository, restaurantReviewRepository, orderRepository,
                tableRepository);
        }

        [Test]
        public void TestGetRestaurantReviews()
        {
            RestaurantReviewController controller = new RestaurantReviewController(_dataContext);
            var response = controller.GetReviews();

            Assert.IsNotNull(response);
            Assert.Greater(response.Count(), 0);
        }

        [Test]
        public void TestGetRestaurantReview()
        {
            RestaurantReviewController controller = new RestaurantReviewController(_dataContext);
            RestaurantReview restaurantReview = _dataContext.RestaurantReview.GetAll().FirstOrDefault();
            IHttpActionResult actionResult = controller.GetRestaurantReview(restaurantReview.Id);
            var contentResult = actionResult as OkNegotiatedContentResult<RestaurantReview>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(restaurantReview.Id, contentResult.Content.Id);
        }

        [Test]
        public void TestGetRestaurantReviewsNotFound()
        {
            RestaurantReviewController controller = new RestaurantReviewController(_dataContext);
            RestaurantReview restaurantReview = _dataContext.RestaurantReview.GetAll().Last();
            IHttpActionResult actionResult = controller.GetRestaurantReview(restaurantReview.Id + 1);

            Assert.IsInstanceOf(typeof(NotFoundResult), actionResult);
        }

        [Test]
        public void TestPutRestaurantReview()
        {
            RestaurantReviewController controller = new RestaurantReviewController(_dataContext);
            RestaurantReview restaurantReview = _dataContext.RestaurantReview.GetAll().First();
            var tempRating = restaurantReview.Rating;
            restaurantReview.Rating = 10;
            IHttpActionResult actionResult = controller.PutRestaurantReview(restaurantReview);

            Assert.IsInstanceOf(typeof(StatusCodeResult), actionResult);
            restaurantReview.Rating = tempRating;
            controller.PutRestaurantReview(restaurantReview);
        }

        [Test]
        public void TestPostRestaurantReview()
        {
            RestaurantReviewController controller = new RestaurantReviewController(_dataContext);
            var review = new RestaurantReview()
            {
                ReviewerName = "John",
                Body = "Good",
                Rating = 6,
                RestaurantId = _dataContext.Restaurant.GetAll().First().Id
            };
            IHttpActionResult actionResult = controller.PostRestaurantReview(review);
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<RestaurantReview>;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("API Default", createdResult.RouteName);

            var reviewToDelete = _dataContext.RestaurantReview.GetAll().Last();
            _dataContext.RestaurantReview.Delete(reviewToDelete.Id);
        }

        [Test]
        public void TestDeleteRestaurantReview()
        {
            RestaurantReviewController controller = new RestaurantReviewController(_dataContext);
            var review = new RestaurantReview()
            {
                ReviewerName = "John",
                Body = "Good",
                Rating = 5,
                RestaurantId = _dataContext.Restaurant.GetAll().First().Id
            };
            _dataContext.RestaurantReview.Add(review);
            var reviewToDelete = _dataContext.RestaurantReview.GetAll().Last();
            IHttpActionResult actionResult = controller.DeleteRestaurantReview(reviewToDelete.Id);
            OkNegotiatedContentResult<RestaurantReview> result =
                actionResult as OkNegotiatedContentResult<RestaurantReview>;

            Assert.IsNotNull(result);
        }
    }
}
