using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using NUnit.Framework;
using OdeToFood.Controllers;
using OdeToFood.Data;

namespace OdeToFood.Tests
{
    [TestFixture]
    public class OrderModelTest
    {
        private readonly OdeToFoodContext _context = new OdeToFoodContext();
        
        public OrderModelTest(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [Test]
        public void IsOrderModelValid()
        {
            Order order = new Order()
            {
                TableNumber = 1,
                PeopleCount = 2,
                RestaurantId = 1,
                Time = new DateTime(2015, 9, 20, 20, 0, 0)
            };

            _context.Set<Order>().Add(order);
            var result = _context.SaveChanges();
            Assert.AreNotEqual(0,result);
        }
    }
}
