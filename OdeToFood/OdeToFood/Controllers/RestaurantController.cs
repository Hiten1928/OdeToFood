using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OdeToFood.Core;
using Castle.Windsor;
using OdeToFood.Contracts;

namespace OdeToFood.Controllers
{
    public class RestaurantController : Controller
    {
        WindsorContainer container = new WindsorContainer();
        private readonly IRestaurantManager _manager;
        OdeToFoodContext _db = new OdeToFoodContext();

        public RestaurantController(IRestaurantManager manager)
        {
            _manager = manager;
        }

        public ActionResult Index()
        {            
            return View(_db.Restaurants.ToList());
        }

        public ActionResult Details(int? id)
        {
            Restaurant restaurant = (Restaurant)_manager.GetEntityById(id.Value);
            return View(restaurant);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Location")] Restaurant restaurant)
        {
            _manager.Create(restaurant);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            Restaurant medicine = (Restaurant)_manager.GetEntityById(id.Value);
            return View(medicine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Location")] Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                _manager.ValidateEntity(restaurant);
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
            _manager.DeleteEntity(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
