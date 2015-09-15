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
    public class TableApiControllerTest
    {
        private readonly DataContext _dataContext;
        private readonly OdeToFoodContext _context;

        public TableApiControllerTest()
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
        public void TestGetTables()
        {
            TableController controller = new TableController(_dataContext);
            var response = controller.GetTables();

            Assert.IsNotNull(response);
            Assert.Greater(response.Count(), 0);
        }

        [Test]
        public void TestGetTable()
        {
            TableController controller = new TableController(_dataContext);
            Table table = _dataContext.Table.GetAll().FirstOrDefault();
            IHttpActionResult actionResult = controller.GetTable(table.Id);
            var contentResult = actionResult as OkNegotiatedContentResult<Table>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(table.Id, contentResult.Content.Id);
        }

        [Test]
        public void TestGetTablesNotFound()
        {
            TableController controller = new TableController(_dataContext);
            Table table = _dataContext.Table.GetAll().Last();
            IHttpActionResult actionResult = controller.GetTable(table.Id + 1);

            Assert.IsInstanceOf(typeof(NotFoundResult), actionResult);
        }

        public void TestGetTablesForTime()
        {
            TableController controller = new TableController(_dataContext);
            IEnumerable<Table> result = controller.Get(DateTime.Now,
                _dataContext.Restaurant.GetAll().First().Id);
            Assert.IsNotNull(result);
        }

        [Test]
        public void TestPutTable()
        {
            TableController controller = new TableController(_dataContext);
            Table table = new Table()
            {
                TableNumber = _dataContext.Restaurant.GetAll().First().Tables.First().TableNumber,
                RestaurantId = _dataContext.Restaurant.GetAll().First().Id
            };
            _dataContext.Table.Add(table);
            table.TableNumber = 8;
            IHttpActionResult actionResult = controller.PutTable(table.Id, table);

            Assert.IsInstanceOf(typeof(StatusCodeResult), actionResult);

        }

        [Test]
        public void TestPostTable()
        {
            TableController controller = new TableController(_dataContext);
            IHttpActionResult actionResult = controller.PostTable(new Table()
            {
                TableNumber = _dataContext.Restaurant.GetAll().First().Tables.First().TableNumber,
                RestaurantId = _dataContext.Restaurant.GetAll().First().Id
            });
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<Table>;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("DefaultApi", createdResult.RouteName);
        }

        [Test]
        public void TestDeleteTable()
        {
            TableController controller = new TableController(_dataContext);
            Table table = _dataContext.Table.GetAll().First();
            IHttpActionResult actionResult = controller.GetTable(table.Id);
            var contentResult = actionResult as OkNegotiatedContentResult<Table>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(table.Id, contentResult.Content.Id);
        }
    }
}