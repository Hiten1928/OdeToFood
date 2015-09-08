using System;
using System.Linq;
using System.Web.Mvc;
using NUnit.Framework;
using OdeToFood.Controllers;
using OdeToFood.Data;
using OdeToFood.Data.Models;
using OdeToFood.Data.Repositories;

namespace OdeToFood.Tests.Controllers_Tests
{
    [TestFixture]
    public class ControllerTest
    {
        private readonly DataContext _dataContext;
        private readonly OdeToFoodContext _context;

        public ControllerTest()
        {
            _context = new OdeToFoodContext();
            var restaurantRepository = new RestaurantRepository(_context);
            var restaurantReviewRepository = new RestaurantReviewRepository(_context);
            var orderRepository = new OrderRepository(_context);
            var tableRepository = new TableRepository(_context);
            _dataContext = new DataContext(restaurantRepository, restaurantReviewRepository, orderRepository, tableRepository);
        }

        [Test]
        public void TestPlaceOrder()
        {
            var controller = new BookTableController(_dataContext);
            var table = _context.Tables.FirstOrDefault();
            var restaurant = _context.Restaurants.FirstOrDefault();
            if (table != null && restaurant != null)
            {
                var result = controller.PlaceOrder(table.Id, restaurant.Id) as PartialViewResult;
                if (result != null) Assert.AreEqual("_PlaceOrder", result.ViewName);
                var orderToDelete = _dataContext.Order.GetAll().Last();
                _dataContext.Order.Delete(orderToDelete.Id);
            }
        }

        [Test]
        public void TestPlaceOrderPostInvalidModel()
        {
            var controller = new BookTableController(_dataContext);
            var order = _context.Orders.FirstOrDefault();
            var newOrder = new Order()
            {
                PeopleCount = 4,
                TableId = order.TableId,
                TimeFrom = order.TimeFrom,
                TimeTo = order.TimeTo,
                Table = _context.Tables.Find(order.TableId)
            };

            var result = controller.PlaceOrder(newOrder) as ContentResult;
            Assert.IsNotNull(result);
            _dataContext.Order.Delete(_dataContext.Order.GetAll().Last().Id);
        }

        [Test]
        public void IsTableAvialableTest()
        {
            var order = _context.Orders.FirstOrDefault();
            var controller = new BookTableController(_dataContext);
            bool result = order != null && controller.IsTableAvialable(order.TableId, order.TimeFrom);
            Assert.IsFalse(result);
        }

        [Test]
        public void GetAvialableTimeTest()
        {
            var controller = new BookTableController(_dataContext);
            DateTime timeToCheck = new DateTime(2015, 8, 26, 14, 0, 0);
            Table table = _context.Tables.FirstOrDefault();
            if (table != null)
            {
                var result = controller.GetAvialableTimes(table.Id, timeToCheck) as PartialViewResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("_ViewFreeTime", result.ViewName);
                Assert.IsNotNull(result.Model);
            }
        }
    }
}
