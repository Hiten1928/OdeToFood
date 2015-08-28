using OddToFood.Contracts;
using OdeToFood.Data.Models;
using OdeToFood.Data.Repositories;

namespace OdeToFood.Data
{
    public class DataContext 
    {
        public RestaurantReviewRepository RestaurantReview { get; set; }

        public RestaurantRepository Restaurant { get; set; }
        public OrderRepository Order { get; set; }

        public DataContext(RestaurantRepository restaurantRepository, RestaurantReviewRepository restaurantReviewRepository, OrderRepository orderRepositoty)
        {
            Restaurant = restaurantRepository;
            RestaurantReview = restaurantReviewRepository;
            Order = orderRepositoty;
        }
    }
}
