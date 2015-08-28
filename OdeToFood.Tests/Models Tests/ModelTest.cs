using System;
using NUnit.Framework;
using OdeToFood.Data;
using OdeToFood.Data.Models;

namespace OdeToFood.Tests.Models_Tests
{
    [TestFixture]
    public class ModelTest
    {
        [Test]
        public void OrderTest()
        {
            var db = new OdeToFoodContext();
            Order order = new Order()
            {
                PeopleCount = 1,
                TableId = 50,
                TimeFrom = DateTime.Now.AddDays(4),
                TimeTo = DateTime.Now.AddDays(4).AddHours(1),
                Table = db.Tables.Find(50)
            };
            Order result = db.Orders.Add(order);
            Assert.IsNotNull(result);
        }
    }
}
