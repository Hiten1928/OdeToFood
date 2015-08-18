using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace OdeToFood.Core
{
    public class OdeToFoodContext : DbContext
    {
        public OdeToFoodContext()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<OdeToFoodContext>());
        }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<RestaurantReview> Reviews { get; set; }
    }
}
