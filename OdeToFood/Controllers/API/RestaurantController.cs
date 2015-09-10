using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Castle.Components.DictionaryAdapter;
using OdeToFood.Data;
using OdeToFood.Data.Models;
using OdeToFood.Views.ViewModels;

namespace OdeToFood.Controllers.API
{
    [EnableCors(origins:"http://localhost:27680", headers:"*", methods:"*")]
    public class RestaurantController : ApiController
    {
        private readonly DataContext _dataContext;

        public RestaurantController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        /// <summary>
        /// Gets all the restaurants 
        /// </summary>
        /// <returns>IEnumerable of restaurants</returns>
        public IEnumerable<Restaurant> GetRestaurants()
        {
            return _dataContext.Restaurant.GetAll();
        }

        /// <summary>
        /// Gets a single restaurant specified by id
        /// </summary>
        /// <param name="id">Id of the restaurant</param>
        /// <returns>Restaurant instance</returns>
        [ResponseType(typeof(Restaurant))]
        public IHttpActionResult GetRestaurant(int id)
        {
            var restaurant = _dataContext.Restaurant.Get(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            return Ok(restaurant);
        }

        [ResponseType(typeof(Restaurant))]
        public Restaurant GetRestaurant(string restaurantName)
        {
            var restaurant = _dataContext.Restaurant.Find(r => r.Name == restaurantName);
            if (restaurant == null)
            {
                return null;
            }
            return restaurant;
        }

        /// <summary>
        /// Updates the instance of restaurant in the database if it's exist
        /// </summary>
        /// <param name="id">Id of the restaurant that is being updated</param>
        /// <param name="restaurant">Updated restaurant instance</param>
        /// <returns>Http result on an operation status</returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRestaurant([FromUri]int id, [FromBody]RestaurantViewModel restaurantViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != restaurantViewModel.Id)
            {
                return BadRequest();
            }
            try
            {
                List<Table> tables = new List<Table>();
                for (int i = 0; i < restaurantViewModel.TableCount; i++)
                {
                    Table table = new Table() {RestaurantId = restaurantViewModel.Id, TableNumber = i + 1};
                    tables.Add(table);
                }
                Restaurant restaurant = new Restaurant()
                {
                    Id = restaurantViewModel.Id,
                    Name = restaurantViewModel.Name,
                    Location = restaurantViewModel.Location,
                    Tables = tables
                };
                _dataContext.Restaurant.Update(restaurant, id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantExists(id))
                {
                    return NotFound();
                }
                throw;
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Creates a new instance of restaurant and saves it to the database
        /// </summary>
        /// <param name="restaurant">A new instance of restaurant that is being saved to the database</param>
        /// <returns></returns>
        [ResponseType(typeof(Restaurant))]
        public IHttpActionResult PostRestaurant(Restaurant restaurant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _dataContext.Restaurant.Add(restaurant);
            return CreatedAtRoute("DefaultApi", new { id = restaurant.Id }, restaurant);
        }

        /// <summary>
        /// Deletes the restaurant instance specified by id from the database
        /// </summary>
        /// <param name="id">Id of the restaurant that is being deleted</param>
        /// <returns>Http result on an operation status</returns>
        [ResponseType(typeof(Restaurant))]
        public IHttpActionResult DeleteRestaurant(int id)
        {
            var restaurant = _dataContext.Restaurant.Get(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            _dataContext.Restaurant.Delete(id);
            return Ok(restaurant);
        }

        /// <summary>
        /// Checked whether restaurant instance specified by id is alraedy in the database
        /// </summary>
        /// <param name="id">Id of the restaurant that is being inspected</param>
        /// <returns>True or false depending on whether restaurant instance found or not</returns>
        private bool RestaurantExists(int id)
        {
            return _dataContext.Restaurant.FindAll(e => e.Id == id).Count > 0;
        }
    }
}
