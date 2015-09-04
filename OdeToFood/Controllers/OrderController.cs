using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using log4net;
using OdeToFood.Data;
using OdeToFood.Data.Models;

namespace OdeToFood.Controllers
{
    public class OrderController : Controller
    {
        private readonly DataContext _dataContext;
        readonly ILog _logger = LogManager.GetLogger(typeof(OrderController));

        public OrderController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        /// <summary>
        /// Gets all the Orders and returns it to the view
        /// </summary>
        /// <returns>View and sends the list of orders to it</returns>
        public ActionResult Index()
        {
            IEnumerable<Order> orders;
            try
            {
                orders = _dataContext.Order.GetAll().ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Content("Exception occured while connecting to the database.");
            }

            return View(orders);
        }

        /// <summary>
        /// Gets the Order spesified by id and deletes it from the database
        /// </summary>
        /// <param name="id">Id of the order to delete</param>
        /// <returns>Redirects to Index action</returns>
        public ActionResult Delete(int id)
        {
            try
            {
                _dataContext.Order.Delete(id);
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
            }
            return RedirectToAction("Index");
        }
    }
}
