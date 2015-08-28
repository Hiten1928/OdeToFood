using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using OdeToFood.Data.Models;

namespace OdeToFood.Data
{
    public class OdeToFoodContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<RestaurantReview> Reviews { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Table> Tables { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
