using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using OdeToFood.Data;
using OdeToFood.Data.Models;

namespace OdeToFood.Controllers.API
{
    public class RestaurantReviewController : ApiController
    {
        private readonly DataContext _dataContext;

        public RestaurantReviewController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        /// <summary>
        /// Gets all RestaurantReviews
        /// </summary>
        /// <returns>IEnumerable of all restaurant reviews</returns>
        public IEnumerable<RestaurantReview> GetReviews()
        {
            return _dataContext.RestaurantReview.GetAll();
        }

        /// <summary>
        /// Gets an RestaurantReview spesified by id
        /// </summary>
        /// <param name="id">Id of the RestaurantReview that is being requested</param>
        /// <returns>Http result on an operation status</returns>
        [ResponseType(typeof(RestaurantReview))]
        public IHttpActionResult GetRestaurantReview(int id)
        {
            var restaurantReview = _dataContext.RestaurantReview.Get(id);
            if (restaurantReview == null)
            {
                return NotFound();
            }
            return Ok(restaurantReview);
        }

        /// <summary>
        /// Updates an existing instance of RestaurantReview in the database
        /// </summary>
        /// <param name="id">Id of the RestaurantReview that is beign updated</param>
        /// <param name="restaurantReview">An updated instance of RestaurantReview</param>
        /// <returns>Http result on an operation status</returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRestaurantReview(int id, RestaurantReview restaurantReview)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != restaurantReview.Id)
            {
                return BadRequest();
            }
            try
            {
                _dataContext.RestaurantReview.Update(restaurantReview, id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantReviewExists(id))
                {
                    return NotFound();
                }
                throw;
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Creates a new instance of RestaurantReview and saves it to the database
        /// </summary>
        /// <param name="restaurantReview">A new RestaurantReview instance</param>
        /// <returns></returns>
        [ResponseType(typeof(RestaurantReview))]
        public IHttpActionResult PostRestaurantReview(RestaurantReview restaurantReview)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _dataContext.RestaurantReview.Add(restaurantReview);
            return CreatedAtRoute("DefaultApi", new { id = restaurantReview.Id }, restaurantReview);
        }

        /// <summary>
        /// Deletes a RestaurantReview from teh database
        /// </summary>
        /// <param name="id">Id of a RestaurantReview that is being deleted</param>
        /// <returns>Http result on an operation status</returns>
        [ResponseType(typeof(RestaurantReview))]
        public IHttpActionResult DeleteRestaurantReview(int id)
        {
            var restaurantReview = _dataContext.RestaurantReview.Get(id);
            if (restaurantReview == null)
            {
                return NotFound();
            }
            _dataContext.RestaurantReview.Delete(id);
            return Ok(restaurantReview);
        }

        /// <summary>
        /// Checked whether RestaurantReview instance specified by id is already in the database
        /// </summary>
        /// <param name="id">Id of the RestaurantReview that is being inspected</param>
        /// <returns>True or false depending on whether RestaurantReview instance found or not</returns>
        private bool RestaurantReviewExists(int id)
        {
            return _dataContext.RestaurantReview.FindAll(e => e.Id == id).Count > 0;
        }
    }
}