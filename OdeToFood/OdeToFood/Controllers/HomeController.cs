using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OdeToFood.Core;
using System.Data.Entity;
using OdeToFood.Contracts;

namespace OdeToFood.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //private IRestaurantManager restaurantManager;

        //public HomeController(IRestaurantManager contactManager)
        //{
        //    this.restaurantManager = contactManager;
        //}

        //public ActionResult Index()
        //{
        //    OdeToFoodContext context = new OdeToFoodContext();
        //    List<Restaurant> restaurants = context.Restaurants.ToList();
        //    return View(restaurants);
        //}

        //[HttpGet]
        //public ActionResult Edit(int id)
        //{
        //    Restaurant restaurant = (Restaurant)restaurantManager.GetEntityById(id);
        //    return View(restaurant);
        //}

        //[HttpPost]
        //public ActionResult Edit(Restaurant restaurant)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        restaurantManager.ValidateEntity(restaurant);
        //        return RedirectToAction("Index");
        //    }
        //    return View(restaurant);
        //}

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}