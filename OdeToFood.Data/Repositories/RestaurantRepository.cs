using System.Data.Entity;
using OdeToFood.Data.Models;

namespace OdeToFood.Data.Repositories
{
    public class RestaurantRepository : Repository<Restaurant>
    {
        public RestaurantRepository(DbContext context) : base(context)
        {

        }
    }
}
