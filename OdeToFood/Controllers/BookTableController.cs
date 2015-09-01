using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using OdeToFood.Data;
using OdeToFood.Data.Models;

namespace OdeToFood.Controllers
{
    [Authorize]
    public class BookTableController : BaseController
    {
        readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BookTableController(DataContext dataContext)
            : base(dataContext)
        {
        }

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

        DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime(((dt.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
        }
    }
}