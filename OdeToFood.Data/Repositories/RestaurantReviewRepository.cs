using System.Data.Entity;
using OdeToFood.Data.Models;

namespace OdeToFood.Data.Repositories
{
    public class RestaurantReviewRepository : Repository<RestaurantReview>
    {
        public RestaurantReviewRepository(DbContext context) : base(context)
        {
        }
    }
}
