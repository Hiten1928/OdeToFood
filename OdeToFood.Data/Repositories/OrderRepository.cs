using OdeToFood.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
