using OdeToFood.Data.Models;
using OdeToFood.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using OdeToFood.Data;

namespace OdeToFood.Controllers
{
    public class BookTableController : BaseController
    {

        public BookTableController(DataContext dataContext)
            : base(dataContext)
        {
        }

        public ActionResult Index(int id)
        {
            TempData["RestaurantId"] = id;
            Restaurant restaurant = DataContext.Restaurant.Get(id);

            return View(restaurant);
        }

        [HttpGet]
        public ActionResult PlaceOrder(int id, int restaurantId)
        {
            Order order = new Order();
            order.TableNumber = id;
            order.TimeFrom = DateTime.Now;
            order.TimeTo = DateTime.Now;
            order.RestaurantId = restaurantId;



            return PartialView("_PlaceOrder", order);
        }

        [HttpPost]
        public ActionResult PlaceOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                DataContext.Order.Add(order);
                return Content("Order has been placed!");
            }
            return View("_PlaceOrder", order);
        }


    }
}