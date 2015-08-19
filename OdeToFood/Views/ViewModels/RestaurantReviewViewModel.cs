using System.Collections.Generic;
using OdeToFood.Data.Models;

namespace OdeToFood.Views.ViewModels
{
    public class RestaurantReviewViewModel
    {
        public List<Restaurant> Restaurants { get; set; }
        public Restaurant RestaurantFor { get; set; }

        public int Id { get; set; }
        public int Rating { get; set; }
        public string Body { get; set; }
        public int RestaurantId { get; set; }
    }
}