using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OdeToFood.Core;
using OdeToFood.Data;
using OdeToFood.Contracts;

namespace OdeToFood.BusinessLogic
{
    public class RestaurantReviewManager : IRestaurantReviewManager
    {
        private readonly IRepository<RestaurantReview> _repository;

        public RestaurantReviewManager(IRepository<RestaurantReview> repository)
        {
            _repository = repository;
        }

        public void Create(IDbEntity review)
        {
            _repository.Add((RestaurantReview)review);
        }

        public IDbEntity GetEntityById(int id)
        {
            RestaurantReview entity = (RestaurantReview)_repository.Get(id);
            return entity;
        }

        public void ValidateEntity(IDbEntity entity)
        {
            RestaurantReview restaurant = entity as RestaurantReview;
            _repository.Update(restaurant, restaurant.Id);
        }

        public void DeleteEntity(int id) {
            _repository.Delete(id);
        }
    }
}