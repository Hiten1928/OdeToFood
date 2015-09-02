using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OddToFood.Contracts;
using OdeToFood.Data;
using OdeToFood.Data.Models;
using OdeToFood.Data.Repositories;

namespace OdeToFood.Api.Controllers
{
    public class RestaurantsController : ApiController
    {


        public IEnumerable<Restaurant> Get()
        {
            var context = new OdeToFoodContext();
            var restaurantRepository = new RestaurantRepository(context);
            List<Restaurant> restaurants = restaurantRepository.GetAll().ToList();
            return restaurants;
        } 
    }
}
