using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using NUnit.Framework;
using OdeToFood.Controllers;
using OdeToFood.Data;
using OdeToFood.Data.Models;
using OdeToFood.Data.Repositories;

namespace OdeToFood.Tests
{
    [TestFixture]
    public class ControllerTest
    {
        private readonly DataContext dataContext;
        private RestaurantRepository restaurantRepository;
        private RestaurantReviewRepository restaurantReviewRepository;
        private OrderRepository orderRepository;
        private OdeToFoodContext context;

        public ControllerTest()
        {
            context = new OdeToFoodContext();
            restaurantRepository = new RestaurantRepository(context);
            restaurantReviewRepository = new RestaurantReviewRepository(context);
            orderRepository = new OrderRepository(context);
            dataContext = new DataContext(restaurantRepository, restaurantReviewRepository, orderRepository);
        }

        [Test]
        public void TestPlaceOrder()
        {
            var controller = new BookTableController(dataContext);
            var table = context.Tables.FirstOrDefault();
            var restaurant = context.Restaurants.FirstOrDefault();
            var result = controller.PlaceOrder(table.Id,restaurant.Id) as PartialViewResult;
            Assert.AreEqual("_PlaceOrder", result.ViewName);
        }

        [Test]
        public void TestPlaceOrderPostInvalidModel()
        {
            var controller = new BookTableController(dataContext);
            var tableId = context.Tables.FirstOrDefault().Id;
            var order = context.Orders.FirstOrDefault();

            Order newOrder = new Order()
            {
                PeopleCount = 2,
                TableId = order.TableId,
                TimeFrom = order.TimeFrom,
                TimeTo = order.TimeTo,
                Table = context.Tables.Find(order.TableId)
            };

            var result = controller.PlaceOrder(order) as ContentResult;
            Assert.IsNotNull(result);
            StringAssert.AreEqualIgnoringCase("Table is not avialable at the specified time.", result.Content);
        }

        [Test]
        public void IsTableAvialableTest()
        {
            var order = context.Orders.FirstOrDefault();
            var table = context.Tables.FirstOrDefault();
            int tableId = table.Id;
            DateTime timeFrom = new DateTime(2015, 8, 26, 14, 0, 0);
            var controller = new BookTableController(dataContext);
            bool result = controller.IsTableAvialable(order.TableId, order.TimeFrom);
            Assert.IsFalse(result);
        }

        [Test]
        public void GetAvialableTimeTest()
        {
            var controller = new BookTableController(dataContext);
            DateTime timeToCheck = new DateTime(2015, 8, 26, 14, 0, 0);
            Table table = context.Tables.FirstOrDefault();
            var result = controller.GetAvialableTimes(table.Id, timeToCheck) as PartialViewResult;
            Assert.AreEqual("_ViewFreeTime", result.ViewName);
            Assert.IsNotNull(result.Model);
        }
    }
}
