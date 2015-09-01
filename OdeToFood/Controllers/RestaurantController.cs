using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Castle.Windsor;
using OddToFood.Contracts;
using OdeToFood.Data;
using OdeToFood.Data.Models;
using OdeToFood.Views.ViewModels;

namespace OdeToFood.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly DataContext _dataContext;
        readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RestaurantController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public ActionResult Index()
        {
            List<Restaurant> restaurantList = new List<Restaurant>();
            try
            {
                restaurantList = _dataContext.Restaurant.GetAll().ToList();
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot connect to the database.");
                return Content("Sorry. Restaurants cannot be displayed.");
            }
            return View(restaurantList);
        }

        public ActionResult Details(int? id)
        {
            Restaurant restaurant = new Restaurant();
            try
            {
                restaurant = _dataContext.Restaurant.Get(id.Value);
                if (restaurant == null) return Content("Spesified restaurant Id is not valid.");
            }
            catch (Exception ex)
            {
                _logger.Error("Exception occured while selecting data from the database.");
                return Content("Sorry. Restaurant details cannot be displayed.");
            }
            return View(restaurant);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RestaurantViewModel restaurantViewModel)
        {
            if (ModelState.IsValid)
            {
                Mapper.CreateMap<RestaurantViewModel, Restaurant>();
                Restaurant restaurant = Mapper.Map<Restaurant>(restaurantViewModel);
                List<Table> tablesInTheRestaurant = new List<Table>();
                for (int i = 0; i < restaurantViewModel.TableCount; i++)
                {
                    tablesInTheRestaurant.Add(new Table(){TableNumber = i+1});
                }
                restaurant.Tables = tablesInTheRestaurant;
                try
                {
                    _dataContext.Restaurant.Add(restaurant);
                }
                catch (Exception ex)
                {
                    _logger.Error("Exception occured while inserting data to the Restaurants table.");
                    return Content("An error occured. Restaurant hasn't been saved.");
                }
                return RedirectToAction("Index");
            }
            return View(restaurantViewModel);

        }

        public ActionResult Edit(int? id)
        {
            Restaurant restaurant = new Restaurant();
            try
            {
                restaurant = _dataContext.Restaurant.Get(id.Value);
                if (restaurant == null)
                {
                    return Content("Specified Restaurant Id is not valid");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("The problem occured while connecting to the database");
                return Content("Sorry. Restaurant cannot be found.");
            }
            return View(restaurant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _dataContext.Restaurant.Update(restaurant, restaurant.Id);
                }
                catch (Exception ex)
                {
                    _logger.Error("Problem occured while updating Restaurant in the database.");
                    return Content("Sorry. An error occured. Restaurant hasn't been updated.");
                }
                return RedirectToAction("Index");
            }
            return View(restaurant);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                _dataContext.Restaurant.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.Error("An error occured while deleting restaurant from the database.");
                return Content("Sorry. Error occured. Can't delete the restaurant.");
            }
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
