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
    public class RestaurantReviewController : BaseController
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(RestaurantReviewController));

        public RestaurantReviewController(DataContext dataContext) : base (dataContext)
        {
            
        }

        /// <summary>
        /// Gets all review,maps them to the review view models and returns it to the view
        /// </summary>
        /// <returns>View and sends a list of ReviewViewModels to it</returns>
        public ActionResult Index()
        {
            List<RestaurantReviewViewModel> reviewViewModels = new List<RestaurantReviewViewModel>();
            List<RestaurantReview> reviews;
            try
            {
                reviews = DataContext.RestaurantReview.GetAll().ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message + ex.StackTrace);
                return Content("Sorry. Problem occured. Cannot list reviews.");
            }
            foreach (var item in reviews)
            {
                Mapper.CreateMap<RestaurantReview, RestaurantReviewViewModel>();
                RestaurantReviewViewModel reviewViewModel = Mapper.Map<RestaurantReviewViewModel>(item);
                reviewViewModel.RestaurantFor = DataContext.Restaurant.Get(item.Id);
                reviewViewModels.Add(reviewViewModel);

            }
            return View(reviewViewModels);
        }

        /// <summary>
        /// Gets the restaurant reviews spesified by id and returns it to the view
        /// </summary>
        /// <param name="id">Id of the restaurant review to see details</param>
        /// <returns>View and sends restaurant review instance spesified by id to it</returns>
        public ActionResult Details(int id)
        {
            RestaurantReview review;
            try
            {
                review = DataContext.RestaurantReview.Get(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message + ex.StackTrace);
                return Content("Sorry. Error occued. Review detail cannot be displayed.");
            }
            Mapper.CreateMap<RestaurantReview, RestaurantReviewViewModel>();
            RestaurantReviewViewModel reviewViewModel = Mapper.Map<RestaurantReviewViewModel>(review);
            return View(reviewViewModel);
        }

        /// <summary>
        /// Creates a new instance of RestaurantRewviewViewModel and sends it to the view
        /// </summary>
        /// <returns>View and sends review view model to it</returns>
        public ActionResult Create()
        {
            RestaurantReviewViewModel viewModel = new RestaurantReviewViewModel()
            {
                Restaurants = DataContext.Restaurant.GetAll().ToList()
            };

            return View(viewModel);
        }

        /// <summary>
        /// Adds a new restaurant review to the database if the model is valid 
        /// </summary>
        /// <param name="restaurantReview">Restaurant review passed by user</param>
        /// <returns>If the restaurant review has been added to the database successfully redirects to Index action. Otherwise, returns the same view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RestaurantReviewViewModel restaurantReview)
        {
            if (ModelState.IsValid)
            {
                Mapper.CreateMap<RestaurantReviewViewModel, RestaurantReview>();
                RestaurantReview review = Mapper.Map<RestaurantReview>(restaurantReview);

                DataContext.RestaurantReview.Add(review);
                return RedirectToAction("Index");
            }
            try
            {
                restaurantReview.Restaurants = DataContext.Restaurant.GetAll().ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message + ex.StackTrace);
                return Content("Sorry. Error occured. Review hasn't been saved.");
            }

            return View(restaurantReview);
        }

        /// <summary>
        /// Gets the restaurant review spesified by id and sends it to the view
        /// </summary>
        /// <param name="id">Id of thre restaurant review to edit</param>
        /// <returns>View and sends restaurant review instance to it</returns>
        public ActionResult Edit(int id)
        {
            RestaurantReview review;
            try
            {
                review = DataContext.RestaurantReview.Get(id);
                if (review == null)
                {
                    return Content("Specified restaurant Id is not valid.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message + ex.StackTrace);
                return Content("Selected review cannot be found.");
            }
            Mapper.CreateMap<RestaurantReview, RestaurantReviewViewModel>();
            RestaurantReviewViewModel reviewViewModel = Mapper.Map<RestaurantReviewViewModel>(review);
            try
            {
                reviewViewModel.Restaurants = DataContext.Restaurant.GetAll().ToList();
                reviewViewModel.RestaurantFor = DataContext.Restaurant.Get(review.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message + ex.StackTrace);
                return Content("Sorry. Error occured. Selected review cannot be edited.");
            }
            return View(reviewViewModel);
        }

        /// <summary>
        /// Updates the restaurant review instance with new values if the model state is valid
        /// </summary>
        /// <param name="restaurantReview">Restaurant review instance posted by user</param>
        /// <returns>If the restaurant review has beed updated succesfully redirects to Index action. Otherwise, returns the view for editing</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RestaurantReviewViewModel restaurantReview)
        {
            if (ModelState.IsValid)
            {
                Mapper.CreateMap<RestaurantReviewViewModel, RestaurantReview>();
                RestaurantReview review = Mapper.Map<RestaurantReview>(restaurantReview);

                try
                {
                    DataContext.RestaurantReview.Update(review, review.Id);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message + ex.StackTrace);
                }
                return RedirectToAction("Index");
            }
            restaurantReview.Restaurants = DataContext.Restaurant.GetAll().ToList();

            return View(restaurantReview);
        }

        /// <summary>
        /// Deletes the restaurant review specified by id
        /// </summary>
        /// <param name="id">If od the restaurant review to delete</param>
        /// <returns>Redirects to Index action</returns>

        public ActionResult Delete(int id)
        {
            try
            {
                DataContext.RestaurantReview.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message + ex.StackTrace);
                return Content("Sorry. Error occured. Cannot delete the review.");
            }
            return RedirectToAction("Index");
        }
    }
}
