using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using AutoMapper.Internal;
using NUnit.Framework;
using OdeToFood.Controllers.API;
using OdeToFood.Data;
using OdeToFood.Data.Models;
using OdeToFood.Data.Repositories;

namespace OdeToFood.Tests.API_Tests
{
    [TestFixture]
    public class OrderApiControllerTest
    {
        private readonly DataContext _dataContext;
        private readonly OdeToFoodContext _context;

        public OrderApiControllerTest()
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
        public void TestGetOrders()
        {
            OrderController controller = new OrderController(_dataContext);
            var response = controller.GetOrders();

            Assert.IsNotNull(response);
            Assert.Greater(response.Count(), 0);
        }

        [Test]
        public void TestGetOrder()
        {
            OrderController controller = new OrderController(_dataContext);
//            controller.PostOrder(new Order()
//            {
//                PeopleCount = 2,
//                TableId = _dataContext.Table.GetAll().First().Id,
//                TimeFrom = DateTime.Now.AddHours(6),
//                TimeTo = DateTime.Now.AddHours(7)
//            });
            Order order = _dataContext.Order.GetAll().FirstOrDefault();
            IHttpActionResult actionResult = controller.GetOrder(order.Id);
            var contentResult = actionResult as OkNegotiatedContentResult<Order>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(order.Id, contentResult.Content.Id);
        }

        [Test]
        public void TestGetOrderNotFound()
        {
            OrderController controller = new OrderController(_dataContext);
            Order order = _dataContext.Order.GetAll().Last();
            IHttpActionResult actionResult = controller.GetOrder(order.Id + 1);

            Assert.IsInstanceOf(typeof (NotFoundResult), actionResult);
        }

        [Test]
        public void TestPutOrder()
        {
            OrderController controller = new OrderController(_dataContext);
//            Order order = new Order()
//            {
//                PeopleCount = 2,
//                TableId = _dataContext.Table.GetAll().First().Id,
//                TimeFrom = DateTime.Now.AddHours(5),
//                TimeTo = DateTime.Now.AddHours(6)
//            };
//            _dataContext.Order.Add(order);
            Order order = _dataContext.Order.GetAll().First();
            var tempPeopleCount = order.PeopleCount;
            order.PeopleCount = 4;
            IHttpActionResult actionResult = controller.PutOrder(order);

            Assert.IsInstanceOf(typeof (StatusCodeResult), actionResult);

            order.PeopleCount = tempPeopleCount;
            controller.PutOrder(order);
        }

        [Test]
        public void TestPostOrder()
        {
            OrderController controller = new OrderController(_dataContext);
            var order = new Order()
            {
                PeopleCount = 2,
                TableId = _dataContext.Table.GetAll().First().Id,
                TimeFrom = DateTime.Now.AddHours(6),
                TimeTo = DateTime.Now.AddHours(7)
            };
            IHttpActionResult actionResult = controller.PostOrder(order);
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<Order>;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("API Default", createdResult.RouteName);
            var orderToDelete = _dataContext.Order.GetAll().Last();
            _dataContext.Order.Delete(orderToDelete.Id);
        }

        [Test]
        public void TestDeleteOrder()
        {
            OrderController controller = new OrderController(_dataContext);
            var order = new Order()
            {
                PeopleCount = 2,
                TableId = _dataContext.Table.GetAll().First().Id,
                TimeFrom = DateTime.Now.AddHours(6),
                TimeTo = DateTime.Now.AddHours(7)
            };
            controller.PostOrder(order);
            Order orderToTest = _dataContext.Order.GetAll().Last();
            IHttpActionResult actionResult = controller.DeleteOrder(order.Id);
            var contentResult = actionResult as OkNegotiatedContentResult<Order>;

            Assert.IsNotNull(contentResult);
        }

        private DateTime RoundUp(DateTime dateTime, TimeSpan interval)
        {
            return new DateTime(((dateTime.Ticks + interval.Ticks - 1) / interval.Ticks) * interval.Ticks);
        }
    }
}