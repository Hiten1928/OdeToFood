using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OdeToFood.Data.Models;

namespace OdeToFood.Views.ViewModels
{
    public class RestaurantReviewViewModel
    {
        public List<Restaurant> Restaurants { get; set; }
        public Restaurant RestaurantFor { get; set; }

        public int Id { get; set; }
        [Range(1,10)]
        public int Rating { get; set; }
        [StringLength(1024)]
        public string Body { get; set; }
        public int RestaurantId { get; set; }
    }
}