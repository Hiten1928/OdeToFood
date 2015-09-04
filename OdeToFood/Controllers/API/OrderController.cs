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
    public class OrderController : ApiController
    {
        private readonly DataContext _dataContext;

        public OrderController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        /// <summary>
        /// Gets all orders
        /// </summary>
        /// <returns>IEnumerable of all orders</returns>
        public IEnumerable<Order> GetOrders()
        {
            return _dataContext.Order.GetAll();
        }

        /// <summary>
        /// Gets an order spesified by id
        /// </summary>
        /// <param name="id">Id of the order that is being requested</param>
        /// <returns>Http result on an operation status</returns>
        [ResponseType(typeof(Order))]
        public IHttpActionResult GetOrder(int id)
        {
            var order = _dataContext.Order.Get(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        /// <summary>
        /// Updates an existing instance of order in the database
        /// </summary>
        /// <param name="id">Id of the order taht is beign updated</param>
        /// <param name="order">An updated instance of order</param>
        /// <returns>Http result on an operation status</returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != order.Id)
            {
                return BadRequest();
            }
            try
            {
                _dataContext.Order.Update(order, id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                throw;
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Creates a new instance of order and saves it to the database
        /// </summary>
        /// <param name="order">A new order instance</param>
        /// <returns></returns>
        [ResponseType(typeof(Order))]
        public IHttpActionResult PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _dataContext.Order.Add(order);
            return CreatedAtRoute("DefaultApi", new { id = order.Id }, order);
        }

        /// <summary>
        /// Deletes an order from teh database
        /// </summary>
        /// <param name="id">Id of an order that is being deleted</param>
        /// <returns>Http result on an operation status</returns>
        [ResponseType(typeof(Order))]
        public IHttpActionResult DeleteOrder(int id)
        {
            var order = _dataContext.Order.Get(id);
            if (order == null)
            {
                return NotFound();
            }
            _dataContext.Order.Delete(id);
            return Ok(order);
        }

        /// <summary>
        /// Checked whether order instance specified by id is already in the database
        /// </summary>
        /// <param name="id">Id of the order that is being inspected</param>
        /// <returns>True or false depending on whether order instance found or not</returns>
        private bool OrderExists(int id)
        {
            return _dataContext.Order.FindAll(e => e.Id == id).Count > 0;
        }
    }
}