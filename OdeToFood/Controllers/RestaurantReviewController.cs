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
        public RestaurantReviewController(DataContext dataContext) : base (dataContext)
        {
            
        }

        public ActionResult Index()
        {
            List<RestaurantReviewViewModel> reviewViewModels = new List<RestaurantReviewViewModel>();
            List<RestaurantReview> reviews = DataContext.RestaurantReview.GetAll().ToList();
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
            RestaurantReview review = DataContext.RestaurantReview.Get(id.Value);
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
            restaurantReview.Restaurants = DataContext.Restaurant.GetAll().ToList();

            return View(restaurantReview);


//            _manager.Create(restaurantReview);
//            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            RestaurantReview review = DataContext.RestaurantReview.Get(id.Value);
            Mapper.CreateMap<RestaurantReview, RestaurantReviewViewModel>();
            RestaurantReviewViewModel reviewViewModel = Mapper.Map<RestaurantReviewViewModel>(review);
            reviewViewModel.Restaurants = DataContext.Restaurant.GetAll().ToList();
            reviewViewModel.RestaurantFor = DataContext.Restaurant.Get(review.Id);
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

                DataContext.RestaurantReview.Update(review, review.Id);
                return RedirectToAction("Index");
            }
            restaurantReview.Restaurants = DataContext.Restaurant.GetAll().ToList();

            return View(restaurantReview);
        }

        public ActionResult Delete(int id)
        {
            DataContext.RestaurantReview.Delete(id);
            return RedirectToAction("Index");
        }

        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    RestaurantReview restaurantReview = db.Reviews.Find(id);
        //    if (restaurantReview == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(restaurantReview);
        //}

        //// POST: RestaurantReview/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    RestaurantReview restaurantReview = db.Reviews.Find(id);
        //    db.Reviews.Remove(restaurantReview);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
