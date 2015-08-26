using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OdeToFood.Data;
using OdeToFood.Data.Models;

namespace OdeToFood.Tests
{
    [TestFixture]
    public class ModelTest
    {
        [Test]
        public void OrderTest()
        {
            var _db = new OdeToFoodContext();
            Order order = new Order()
            {
                Id = 152,
                PeopleCount = 1,
                TableId = 50,
                TimeFrom = DateTime.Now.AddDays(4),
                TimeTo = DateTime.Now.AddDays(4).AddHours(1),
                Table = _db.Tables.Find(50)
            };
            Order result =_db.Orders.Add(order);
            Assert.IsNotNull(result);
        }
    }
}
