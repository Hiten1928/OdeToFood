using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using log4net;
using OdeToFood.Data;
using OdeToFood.Data.Models;

namespace OdeToFood.Controllers
{
    [Authorize]
    public class BookTableController : BaseController
    {
        readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
            if (ModelState.IsValid)
            {
                var ordersForTable = DataContext.Order.FindAll(o => o.TableId == order.TableId);
                if (ordersForTable == null)
                {
                    _logger.Error("Table id in the Order model is not valid. Orders for this table are not found");
                }
                bool isAvialable = true;
                DateTime timeFromCeil = RoundUp(order.TimeFrom, TimeSpan.FromMinutes(60));
                if (ordersForTable != null)
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
                try
                {
                    DataContext.Order.Add(order);
                }
                catch(Exception ex)
                {
                    _logger.Error("Cannot insert Order to the database. Check the connection");
                    return Content(ex.Message);
                }
                return Content("You have placed your order successfully.");
            }
            return Content("Model State is not valid.");
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
            var ordersForTable = DataContext.Order.FindAll(o => o.TableId == tableId);
            if (ordersForTable == null)
            {
                _logger.Info("Spesified table Id is not valid.");
            }
            bool isAvialable = true;
            DateTime timeFromCeil = RoundUp(time, TimeSpan.FromMinutes(60));
            if (ordersForTable != null)
                foreach (var o in ordersForTable)
                {
                    if (timeFromCeil.Hour == o.TimeFrom.Hour && timeFromCeil.Day == o.TimeFrom.Day && timeFromCeil.Month == o.TimeFrom.Month)
                    {
                        isAvialable = false;
                    }
                }
            return isAvialable;
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
            var tableOrders = DataContext.Order.FindAll(o => o.TableId == tableId);
            var context = new OdeToFoodContext();
            if (context.Tables.Find(tableId) == null)
            {
                _logger.Error("Spesified table id is not found.");
                return Content("Spesified table id is not found.");
            }
            List<DateTime> takenTimesForTheDate = new List<DateTime>();
            if (tableOrders != null)
            {
                foreach (var t in tableOrders)
                {
                    if (t.TimeFrom.Date == date.Date)
                    {
                        takenTimesForTheDate.Add(t.TimeFrom);
                    }
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

        /// <summary>
        /// Gets all tables that it avialable for specified restaurant and date
        /// </summary>
        /// <param name="time">The date for avialability checking</param>
        /// <param name="restaurantId">Restaurant Id that is being checked for avialable tables</param>
        /// <returns>Partial view and sends the list of avialable tables to it</returns>
        public ActionResult GetAvialableTables(DateTime time, int restaurantId)
        {
            var context = new OdeToFoodContext();
            var ordersForTheRestaurant =
                DataContext.Order.GetAll().Where(t => t.Table.RestaurantId == restaurantId).ToList();
            var ordersForGivenTime = new List<Order>();
            var timeCeil = RoundUp(time, TimeSpan.FromMinutes(60));
            if (ordersForTheRestaurant.Count != 0)
            {
                foreach (Order o in ordersForTheRestaurant)
                {
                    if (o.TimeFrom == timeCeil)
                    {
                        ordersForGivenTime.Add(o);
                    }
                }
            }
            var allTablesForRestaurant = context.Tables.Where(t => t.RestaurantId == restaurantId).ToList();
            var avialableTables = new List<Table>();
            if (allTablesForRestaurant.Count != 0)
            {
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
            }
            return PartialView("_GetAvialableTables", avialableTables);
        }

        /// <summary>
        /// Takes a DateTime object and an interval in minutes. Ceils the minute value to the next interval spesified by TimeSpan param
        /// </summary>
        /// <param name="dt">DateTime object that is being rounded</param>
        /// <param name="d">Interval that rounding should happen to</param>
        /// <returns>Rounded DateTime object</returns>
        DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime(((dt.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
        }
    }
}