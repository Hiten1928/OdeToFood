using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web.Http;
using OdeToFood.Data;
using OdeToFood.Data.Models;
using OdeToFood.Data.Repositories;

namespace OdeToFood.Controllers.API
{
    public class RestaurantController : ApiController
    {
        private readonly DataContext _dataContext;

        public RestaurantController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<Restaurant> Get()
        {
            return _dataContext.Restaurant.GetAll();
        } 
    }
}
