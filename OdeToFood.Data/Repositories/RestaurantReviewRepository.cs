using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
