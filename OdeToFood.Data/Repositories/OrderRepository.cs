using System.Data.Entity;
using OdeToFood.Data.Models;

namespace OdeToFood.Data.Repositories
{
    public class OrderRepository : Repository<Order>
    {
        public OrderRepository(DbContext context)
            : base(context)
        {

        }
    }
}
