using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Castle.Windsor;
using OddToFood.Contracts;
using OdeToFood.Data;
using OdeToFood.Data.Models;
using OdeToFood.Views.ViewModels;
using AutoMapper;

namespace OdeToFood.Controllers
{
    public class RestaurantReviewController : BaseController
    {
        readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RestaurantReviewController(DataContext dataContext) : base (dataContext)
        {
            
        }

        public ActionResult Index()
        {
            List<RestaurantReviewViewModel> reviewViewModels = new List<RestaurantReviewViewModel>();
            List<RestaurantReview> reviews = new List<RestaurantReview>();
            try
            {
                reviews = DataContext.RestaurantReview.GetAll().ToList();
            }
            catch (Exception ex)
            {
                _logger.Error("Problem occured while getting reviews from the database.");
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

        public ActionResult Details(int? id)
        {
            RestaurantReview review;
            try
            {
                review = DataContext.RestaurantReview.Get(id.Value);
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot get the review specified by id.");
                return Content("Sorry. Error occued. Review detail cannot be displayed.");
            }
            Mapper.CreateMap<RestaurantReview, RestaurantReviewViewModel>();
            RestaurantReviewViewModel reviewViewModel = Mapper.Map<RestaurantReviewViewModel>(review);
            return View(reviewViewModel);
        }

        public ActionResult Create()
        {
            RestaurantReviewViewModel viewModel = new RestaurantReviewViewModel()
            {
                Restaurants = DataContext.Restaurant.GetAll().ToList()
            };

            return View(viewModel);
        }

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
                _logger.Error("Exception occured while adding new instance of RestaurantReview to the database.");
                return Content("Sorry. Error occured. Review hasn't been saved.");
            }

            return View(restaurantReview);
        }

        public ActionResult Edit(int? id)
        {
            RestaurantReview review;
            try
            {
                review = DataContext.RestaurantReview.Get(id.Value);
                if (review == null)
                {
                    return Content("Specified restaurant Id is not valid.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured while getting restaurant review by id.");
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
                _logger.Error("Exception occured while getting restaurants for RestaurantReview.");
                return Content("Sorry. Error occured. Selected review cannot be edited.");
            }
            return View(reviewViewModel);
        }

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
                    _logger.Error("Error occured while updating restaurant review: " + ex.Message);
                }
                return RedirectToAction("Index");
            }
            restaurantReview.Restaurants = DataContext.Restaurant.GetAll().ToList();

            return View(restaurantReview);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                DataContext.RestaurantReview.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.Error("Exception occured while deleting RestaurantReview.");
                return Content("Sorry. Error occured. Cannot delete the review.");
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
