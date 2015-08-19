using System.Data.Entity;
using OdeToFood.Data.Models;

namespace OdeToFood.Data
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
