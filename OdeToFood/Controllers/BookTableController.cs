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
            var db = new OdeToFoodContext();
            Order order = new Order();
            order.TimeFrom = DateTime.Now;
            order.TimeTo = DateTime.Now;
            order.TableId = id;


            return PartialView("_PlaceOrder", order);
        }

        [HttpPost]
        public ActionResult PlaceOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                var ordersForTable = DataContext.Order.FindAll(o => o.TableId == order.TableId );
                bool isAvialable = true;
                DateTime timeFromCeil = RoundUp(order.TimeFrom, TimeSpan.FromMinutes(60));
                order.TimeFrom = timeFromCeil;
                foreach (var o in ordersForTable)
                {
                    if (order.TimeFrom == o.TimeFrom)
                    {
                        isAvialable = false;
                    }
                }
                if (!isAvialable)
                {
                    return Content("Table is not avialable at the specified time.");
                }

                DataContext.Order.Add(order);
                return Content("You have placed your order successfully.");
            }
            return Content("Wrong input.");
        }

        DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime(((dt.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
        }
    }
}