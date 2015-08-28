using System.Collections.Generic;
using OdeToFood.Data.Models;

namespace OdeToFood.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OdeToFoodContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "OdeToFood.Data.OdeToFoodContext";
        }

        protected override void Seed(OdeToFoodContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            Restaurant restaurant = new Restaurant()
            {
                Location = "London",
                Name = "Zomato"
            };
            context.Restaurants.AddOrUpdate(restaurant);
            Table table1 = new Table()
            {
                TableNumber = 1,
                RestaurantId = restaurant.Id,
                Restaurant = restaurant
            };
            Table table2 = new Table()
            {
                TableNumber = 2,
                RestaurantId = restaurant.Id,
                Restaurant = restaurant
            };
            context.Tables.AddOrUpdate(table1);
            context.Tables.AddOrUpdate(table2);
            Order order = new Order()
            {
                PeopleCount = 2,
                Table = table1,
                TableId = table1.Id,
                TimeFrom = new DateTime(),
                TimeTo = new DateTime().AddHours(1)
            };
        }
    }
}
