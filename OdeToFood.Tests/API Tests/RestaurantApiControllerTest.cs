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
    public class RestaurantApiControllerTest
    {
        private readonly DataContext _dataContext;
        private readonly OdeToFoodContext _context;

        public RestaurantApiControllerTest()
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
        public void TestGetRestaurants()
        {
            RestaurantController controller = new RestaurantController(_dataContext);
            var response = controller.GetRestaurants();

            Assert.IsNotNull(response);
            Assert.Greater(response.Count(), 0);
        }

        [Test]
        public void TestGetRestaurant()
        {
            RestaurantController controller = new RestaurantController(_dataContext);
            Restaurant restaurant = _dataContext.Restaurant.GetAll().FirstOrDefault();
            IHttpActionResult actionResult = controller.GetRestaurant(restaurant.Id);
            var contentResult = actionResult as OkNegotiatedContentResult<Restaurant>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(restaurant.Id, contentResult.Content.Id);
        }

        [Test]
        public void TestGetRestaurantsNotFound()
        {
            RestaurantController controller = new RestaurantController(_dataContext);
            Restaurant restaurant = _dataContext.Restaurant.GetAll().Last();
            IHttpActionResult actionResult = controller.GetRestaurant(restaurant.Id + 1);

            Assert.IsInstanceOf(typeof(NotFoundResult), actionResult);
        }

        [Test]
        public void TestPutRestaurant()
        {
            RestaurantController controller = new RestaurantController(_dataContext);
//            Restaurant restaurant = new Restaurant()
//            {
//                Location = "NY",
//                Name = "Subway"
//            };
//            _dataContext.Restaurant.Add(restaurant);
            var restaurant = _dataContext.Restaurant.GetAll().FirstOrDefault();
            var tempName = restaurant.Name;
            restaurant.Name = "Chitos";
            IHttpActionResult actionResult = controller.PutRestaurant(restaurant.Id, restaurant);

            Assert.IsInstanceOf(typeof(StatusCodeResult), actionResult);

            restaurant.Name = tempName;
            controller.PutRestaurant(restaurant.Id, restaurant);
        }

        [Test]
        public void TestPostRestaurant()
        {
            RestaurantController controller = new RestaurantController(_dataContext);
            IHttpActionResult actionResult = controller.PostRestaurant(new Restaurant()
            {
                Name = "Mafia",
                Location = "Kharkiv"
            });
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<Restaurant>;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("DefaultApi", createdResult.RouteName);
            var mafiaRestaurant = _dataContext.Restaurant.Find(r => r.Name == "Mafia" && r.Location == "Kharkiv");
            _dataContext.Restaurant.Delete(mafiaRestaurant.Id);
        }

        [Test]
        public void TestDeleteRestaurant()
        {
            RestaurantController controller = new RestaurantController(_dataContext);
            Restaurant restaurant = new Restaurant() {Location = "Kharkiv", Name = "Favourite"};
            _dataContext.Restaurant.Add(restaurant);

            IHttpActionResult actionResult = controller.DeleteRestaurant(restaurant.Id);
            var contentResult = actionResult as OkNegotiatedContentResult<Restaurant>;

            Assert.IsNotNull(contentResult);
        }
    }
}
