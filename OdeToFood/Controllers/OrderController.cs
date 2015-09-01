using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OdeToFood.Data;
using OdeToFood.Data.Models;

namespace OdeToFood.Controllers
{
    public class OrderController : Controller
    {
        private readonly DataContext _dataContext;
        readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public OrderController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public ActionResult Index()
        {
            IEnumerable<Order> orders = new List<Order>();
            try
            {
                orders = _dataContext.Order.GetAll().ToList();
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot connect to the database.");
                return Content("Exception occured while connecting to the database.");
            }

            return View(orders);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                _dataContext.Order.Delete(id);
            }
            catch(Exception ex)
            {
                _logger.Error("Cannot delete Order entity spesified by Id.");
            }
            return RedirectToAction("Index");
        }
    }
}
