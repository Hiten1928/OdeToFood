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
            context.Restaurants.AddOrUpdate(x => x.Id,
                new Restaurant() { Id = 1, Location = "London", Name = "McD"},
                new Restaurant() { Id = 2, Location = "NY", Name = "Botanical Garden" }
            );
            context.Tables.AddOrUpdate(x => x.Id,
                new Table() {Id = 1, RestaurantId = 1, TableNumber = 1},
                new Table() {Id = 2, RestaurantId = 1, TableNumber = 2},
                new Table() {Id = 3, RestaurantId = 2, TableNumber = 1},
                new Table() {Id = 4, RestaurantId = 2, TableNumber = 2}
                );
            context.Reviews.AddOrUpdate(x => x.Id,
                new RestaurantReview() {Id = 1, Body = "Good job", Rating = 9, RestaurantId = 1, ReviewerName = "John"},
                new RestaurantReview(){Id = 2,Body = "Bad service",Rating = 5,RestaurantId = 2,ReviewerName = "Diana"}
                );
            context.Orders.AddOrUpdate(
                new Order() { Id=1,PeopleCount = 2,TableId = 1,TimeFrom = DateTime.Now, TimeTo = DateTime.Now.AddHours(1)},
                new Order() { Id=2,PeopleCount = 3,TableId = 2, TimeFrom = DateTime.Now, TimeTo = DateTime.Now.AddHours(1)}
                );
        }
    }
}
