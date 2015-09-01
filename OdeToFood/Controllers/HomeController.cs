using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Castle.Core.Internal;
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

        public ActionResult GetRestaurants()
        {
            List<Restaurant> restaurants = DataContext.Restaurant.GetAll().ToList();

            return PartialView("_GetRestaurants", restaurants);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}