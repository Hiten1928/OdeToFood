using OdeToFood.Data.Models;
using OdeToFood.Views.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using OdeToFood.Data;
using OdeToFood.Views.ViewModels;

namespace OdeToFood.Controllers
{
    [Authorize]
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
            order.PeopleCount = 2;
            return PartialView("_PlaceOrder", order);
        }

        [HttpPost]
        public ActionResult PlaceOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                var ordersForTable = DataContext.Order.FindAll(o => o.TableId == order.TableId);
                bool isAvialable = true;
                DateTime timeFromCeil = RoundUp(order.TimeFrom, TimeSpan.FromMinutes(60));
                foreach (var o in ordersForTable)
                {
                    if (timeFromCeil.Hour == o.TimeFrom.Hour && timeFromCeil.Day == o.TimeFrom.Day && timeFromCeil.Month == o.TimeFrom.Month)
                    {
                        isAvialable = false;
                    }
                }
                if (!isAvialable)
                {
                    return Content("Table is not avialable at the specified time.");
                }
                order.TimeFrom = timeFromCeil;
                order.TimeTo = timeFromCeil.AddHours(1);
                DataContext.Order.Add(order);
                return Content("You have placed your order successfully.");
            }
            return Content("Model State is not valid.");
        }

        [HttpPost]
        public bool IsTableAvialable(int tableId, DateTime time)
        {
            var ordersForTable = DataContext.Order.FindAll(o => o.TableId == tableId);
            bool isAvialable = true;
            DateTime timeFromCeil = RoundUp(time, TimeSpan.FromMinutes(60));
            foreach (var o in ordersForTable)
            {
                if (timeFromCeil.Hour == o.TimeFrom.Hour && timeFromCeil.Day == o.TimeFrom.Day && timeFromCeil.Month == o.TimeFrom.Month)
                {
                    isAvialable = false;
                }
            }
            return isAvialable;
        }

        [HttpPost]
        public ActionResult GetAvialableTimes(int tableId, DateTime date)
        {
            var tableOrders = DataContext.Order.FindAll(o => o.TableId == tableId);
            List<DateTime> takenTimesForTheDate = new List<DateTime>();
            foreach (var t in tableOrders)
            {
                if (t.TimeFrom.Date == date.Date)
                {
                    takenTimesForTheDate.Add(t.TimeFrom);
                }
            }
            List<DateTime> freeTimes = new List<DateTime>();
            for (int i = 0; i < 24; i++)
            {
                if (takenTimesForTheDate.Any(o => o.Hour == i))
                {
                    continue;
                }
                freeTimes.Add(new DateTime(date.Year, date.Month, date.Day, i, 0, 0, 0));
            }
            return PartialView("_ViewFreeTime", freeTimes);
        }

        public ActionResult GetAvialableTables(DateTime time, int restaurantId)
        {
            var context = new OdeToFoodContext();
            var ordersForTheRestaurant =
                DataContext.Order.GetAll().Where(t => t.Table.RestaurantId == restaurantId).ToList();
            var ordersForGivenTime = new List<Order>();
            var timeCeil = RoundUp(time, TimeSpan.FromMinutes(60));
            foreach (Order o in ordersForTheRestaurant)
            {
                if (o.TimeFrom == timeCeil)
                {
                    ordersForGivenTime.Add(o);
                }
            }
            var allTablesForRestaurant = context.Tables.Where(t => t.RestaurantId == restaurantId).ToList();
            var avialableTables = new List<Table>();
            foreach (Table t in allTablesForRestaurant)
            {
                var isAvialable = true;
                foreach (Order o in ordersForGivenTime)
                {
                    if (t.Id == o.TableId)
                    {
                        isAvialable = false;
                    }
                }
                if (isAvialable)
                {
                    avialableTables.Add(t);
                }
            }
            return PartialView("_GetAvialableTables", avialableTables);
        }

        DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime(((dt.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
        }
    }
}