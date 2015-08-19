using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Castle.Windsor;
using OddToFood.Contracts;
using OdeToFood.Data;
using OdeToFood.Data.Models;

namespace OdeToFood.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly DataContext _dataContext;

        public RestaurantController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public ActionResult Index()
        {
            List<Restaurant> restaurantList = _dataContext.Restaurant.GetAll().ToList();
            return View(restaurantList);
        }

        public ActionResult Details(int? id)
        {
            Restaurant restaurant = _dataContext.Restaurant.Get(id.Value);
            return View(restaurant);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Restaurant restaurant)
        {
            _dataContext.Restaurant.Add(restaurant);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            Restaurant restaurant = _dataContext.Restaurant.Get(id.Value);
            return View(restaurant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                _dataContext.Restaurant.Update(restaurant, restaurant.Id);
                return RedirectToAction("Index");
            }
            return View(restaurant);
        }

        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Restaurant restaurant = _db.Restaurants.Find(id);
        //    if (restaurant == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(restaurant);
        //}


        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    _manager.DeleteEntity(id);
        //    return RedirectToAction("Index");
        //}

        public ActionResult Delete(int id)
        {
            _dataContext.Restaurant.Delete(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            OdeToFoodContext _db = new OdeToFoodContext();
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
