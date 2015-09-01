using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OdeToFood.Data;
using OdeToFood.Data.Models;

namespace OdeToFood.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(DataContext dataContext)
            : base(dataContext)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets all the restaurants and returns it to the view
        /// </summary>
        /// <returns>Partial view and sends teh list of Restaurant to it</returns>
        public ActionResult GetRestaurants()
        {
            List<Restaurant> restaurants = DataContext.Restaurant.GetAll().ToList();

            return PartialView("_GetRestaurants", restaurants);
        }

    }
}