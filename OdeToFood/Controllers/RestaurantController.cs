using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using AutoMapper;
using log4net;
using OdeToFood.Data;
using OdeToFood.Data.Models;
using OdeToFood.Views.ViewModels;

namespace OdeToFood.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly DataContext _dataContext;
        readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public RestaurantController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        /// <summary>
        /// Gets all the Restaurants and sends it to the view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            List<Restaurant> restaurantList;
            try
            {
                restaurantList = _dataContext.Restaurant.GetAll().ToList();
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot connect to the database. Exception: " + ex.Message);
                return Content("Sorry. Restaurants cannot be displayed.");
            }
            return View(restaurantList);
        }

        /// <summary>
        /// Gets the restaurants spesified by id and returns it to the view
        /// </summary>
        /// <param name="id">Id of the restaurant to see details</param>
        /// <returns>View and sends restaurant instance spesified by id to it</returns>
        public ActionResult Details(int id)
        {
            Restaurant restaurant;
            try
            {
                restaurant = _dataContext.Restaurant.Get(id);
            }
            catch (Exception ex)
            {
                _logger.Error("Exception occured while selecting data from the database. Exception: " + ex.Message);
                return Content("Sorry. Restaurant details cannot be displayed.");
            }
            return View(restaurant);
        }

        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Adds a new restaurant to the database if the model is valid 
        /// </summary>
        /// <param name="restaurantViewModel">Restaurant view model passed by user</param>
        /// <returns>If the restaurant has been added to the database successfully redirects to Index action. Otherwise, returns the same view.</returns>
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
                    _logger.Error("Exception occured while inserting data to the Restaurants table. Exception: " + ex.Message);
                    return Content("An error occured. Restaurant hasn't been saved.");
                }
                return RedirectToAction("Index");
            }
            return View(restaurantViewModel);

        }

        /// <summary>
        /// Gets the restaurant spesified by id and sends it to the view
        /// </summary>
        /// <param name="id">Id of thre restaurant to edit</param>
        /// <returns>View and rends restaurant to it</returns>
        public ActionResult Edit(int id)
        {
            Restaurant restaurant;
            try
            {
                restaurant = _dataContext.Restaurant.Get(id);
                if (restaurant == null)
                {
                    return Content("Specified Restaurant Id is not valid");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("The problem occured while connecting to the database. Exception: " + ex.Message);
                return Content("Sorry. Restaurant cannot be found.");
            }
            return View(restaurant);
        }

        /// <summary>
        /// Updates the restaurant instance with new values if the model state is valid
        /// </summary>
        /// <param name="restaurant">Restaurant instance posted by user</param>
        /// <returns>If the restaurant has beed updated succesfully redirects to Index action. Otherwise, returns the view for editing</returns>
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
                    _logger.Error("Problem occured while updating Restaurant in the database. Exception: " + ex.Message);
                    return Content("Sorry. An error occured. Restaurant hasn't been updated.");
                }
                return RedirectToAction("Index");
            }
            return View(restaurant);
        }

        /// <summary>
        /// Deletes the restaurant specified by id
        /// </summary>
        /// <param name="id">If od the restaurant to delete</param>
        /// <returns>Redirects to Index action</returns>
        public ActionResult Delete(int id)
        {
            try
            {
                _dataContext.Restaurant.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.Error("An error occured while deleting restaurant from the database. Exception: "+ex.Message);
                return Content("Sorry. Error occured. Can't delete the restaurant.");
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            OdeToFoodContext db = new OdeToFoodContext();
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
