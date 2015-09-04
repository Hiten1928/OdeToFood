using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using AutoMapper.Internal;
using log4net;
using Microsoft.Ajax.Utilities;
using OdeToFood.Data;
using OdeToFood.Data.Models;

namespace OdeToFood.Controllers
{
    [Authorize]
    public class BookTableController : BaseController
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(BookTableController));

        public BookTableController(DataContext dataContext)
            : base(dataContext)
        {
        }

        /// <summary>
        /// Gets a restaurant spesified by id
        /// </summary>
        /// <param name="id">Id of the restaurant</param>
        /// <returns>View and sends restaurant instance to it</returns>
        public ActionResult Index(int id)
        {
            TempData["RestaurantId"] = id;
            Restaurant restaurant = DataContext.Restaurant.Get(id);
            if (restaurant == null)
            {
                _logger.Error("Spesified restaurant Id is not valid.");
                return RedirectToAction("Index", "Restaurant");
            }
            return View(restaurant);
        }

        /// <summary>
        /// Sends a partial view to create a new Order
        /// </summary>
        /// <param name="id">Id of the table that is being booked</param>
        /// <param name="restaurantId">Id of the restautrant table belongs to</param>
        /// <returns>Partial view and sends an empty Order instance to it</returns>
        [HttpGet]
        public ActionResult PlaceOrder(int id, int restaurantId)
        {
            Order order = new Order();
            order.TimeFrom = DateTime.Now;
            order.TimeTo = DateTime.Now;
            order.TableId = id;
            order.PeopleCount = 2;
            return PartialView("_PlaceOrder", order);
        }

        /// <summary>
        /// Checks whether the table is avialable in the spesified time and saves the order to the database 
        /// </summary>
        /// <param name="order">Newly created Order instance</param>
        /// <returns>Content about Order state</returns>
        [HttpPost]
        public ActionResult PlaceOrder(Order order)
        {
            if (ModelState.IsValid && IsAvialable(order.TableId, order.TimeFrom))
            {
                order.TimeFrom = RoundUp(order.TimeFrom, TimeSpan.FromMinutes(60));
                order.TimeTo = order.TimeFrom.AddHours(1);
                DataContext.Order.Add(order);
                DataContext.Order.Add(order);
                return Content("You have placed your order successfully.");
            }
            return Content("Check the details!");
        }

        /// <summary>
        /// Checks whether table is avialable for specified time
        /// </summary>
        /// <param name="tableId">Id of the table that is being chked for avialability</param>
        /// <param name="time">Time to check for avialablity</param>
        /// <returns>True or false depending on table avialability</returns>
        [HttpPost]
        public bool IsTableAvialable(int tableId, DateTime time)
        {
            return IsAvialable(tableId, time);
        }

        /// <summary>
        /// Gets all times for specified date that is avialable for the table. Time interval is 1h.
        /// </summary>
        /// <param name="tableId">Id of the table that is being checked for avialability</param>
        /// <param name="date">The date for table avialability</param>
        /// <returns>Partial view and sends the list of DateTime instances taht is avialable for specified table and date</returns>
        [HttpPost]
        public ActionResult GetAvialableTimes(int tableId, DateTime date)
        {
            List<DateTime> times = new List<DateTime>();
            for (var i = 0; i < 24; i++)
            {
                var avialableTime = new DateTime(date.Year, date.Month, date.Day, i, date.Minute, date.Second);
                if (DataContext.Order.GetAll().Any(o=>o.TimeFrom == avialableTime) ) continue;
                times.Add(avialableTime);
            }
            return PartialView("_ViewFreeTime", times);
        }

        /// <summary>
        /// Gets all tables that it avialable for specified restaurant and date
        /// </summary>
        /// <param name="time">The date for avialability checking</param>
        /// <param name="restaurantId">Restaurant Id that is being checked for avialable tables</param>
        /// <returns>Partial view and sends the list of avialable tables to it</returns>
        public ActionResult GetAvialableTables(DateTime time, int restaurantId)
        {
            var tables = DataContext.Restaurant.Get(restaurantId).Tables;
            var timeCeil = RoundUp(time, TimeSpan.FromMinutes(60));
            List<Table> avialableTables = new List<Table>();
            tables.Each(t =>
            {
                if (DataContext.Order.FindAll(o => o.TableId == t.Id).All(od => od.TimeFrom != timeCeil))
                {
                    avialableTables.Add(t);
                }
            });
            return PartialView("_GetAvialableTables", avialableTables);
        }

        /// <summary>
        /// Takes a DateTime object and an interval in minutes. Ceils the minute value to the next interval spesified by TimeSpan param
        /// </summary>
        /// <param name="dateTime">DateTime object that is being rounded</param>
        /// <param name="interval">Interval that rounding should happen to</param>
        /// <returns>Rounded DateTime object</returns>
        DateTime RoundUp(DateTime dateTime, TimeSpan interval)
        {
            return new DateTime(((dateTime.Ticks + interval.Ticks - 1) / interval.Ticks) * interval.Ticks);
        }

        /// <summary>
        /// Helper method that checks whether the specified table is avialable at the time
        /// </summary>
        /// <param name="tableId">Id of the table that is being checked</param>
        /// <param name="time">Time for table avialability</param>
        /// <returns>True or false depending on table avialability</returns>
        bool IsAvialable(int tableId, DateTime time)
        {
            DateTime timeFromCeil = RoundUp(time, TimeSpan.FromMinutes(60));
            bool isTableTaken = false;
            try
            {
                isTableTaken = DataContext.Order.FindAll(o => o.TableId == tableId)
                    .Any(o => o.TimeFrom == timeFromCeil);
            }
            catch (Exception ex)
            {
                _logger.Error("Error while running query. Exception: " + ex.Message);
            }
            return !isTableTaken;
        }
    }
}