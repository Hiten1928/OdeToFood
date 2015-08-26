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
            var result = controller.PlaceOrder(50,9) as PartialViewResult;
            Assert.AreEqual("_PlaceOrder", result.ViewName);
        }

        [Test]
        public void TestPlaceOrderPostInvalidModel()
        {
            var controller = new BookTableController(dataContext);
            Order order = new Order()
            {
                Id = 150,
                PeopleCount = 2,
                TableId = 50,
                TimeFrom = DateTime.Now.AddDays(3),
                TimeTo = DateTime.Now.AddDays(3).AddHours(1),
                Table = context.Tables.Find(50)
            };

            var result = controller.PlaceOrder(order) as ContentResult;
            Assert.IsNotNull(result);
            StringAssert.AreEqualIgnoringCase("Table is not avialable at the specified time.", result.Content);
        }

        [Test]
        public void IsTableAvialableTest()
        {
            int tableId = 50;
            DateTime timeFrom = new DateTime(2015, 8, 26, 14, 0, 0);
            var controller = new BookTableController(dataContext);
            bool result = controller.IsTableAvialable(tableId, timeFrom);
            Assert.IsFalse(result);
        }

        [Test]
        public void GetAvialableTimeTest()
        {
            var controller = new BookTableController(dataContext);
            DateTime timeToCheck = new DateTime(2015, 8, 26, 14, 0, 0);
            var result = controller.GetAvialableTimes(50, timeToCheck) as ViewResult;
            Assert.AreEqual("_ViewFreeTime", result.ViewName);
            Assert.IsNotNull(result.Model);
        }
    }
}
