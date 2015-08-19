﻿using OddToFood.Contracts;
using OdeToFood.Data.Models;
using OdeToFood.Data.Repositories;

namespace OdeToFood.Data
{
    public class DataContext 
    {
        public RestaurantReviewRepository RestaurantReview { get; set; }

        public RestaurantRepository Restaurant { get; set; }

        public DataContext(RestaurantRepository restaurantRepository, RestaurantReviewRepository restaurantReviewRepository)
        {
            Restaurant = restaurantRepository;
            RestaurantReview = restaurantReviewRepository;
        }
//        public void Create(IDbEntity review)
//        {
//            _repository.Add((Restaurant)review);
//        }
//
//        public IDbEntity GetEntityById(int id)
//        {
//            Restaurant entity = (Restaurant)_repository.Get(id);
//            return entity;
//        }
//
//        public void ValidateEntity(IDbEntity entity)
//        {
//            Restaurant restaurant = entity as Restaurant;
//            _repository.Update(restaurant, restaurant.Id);
//        }
//
//        public void DeleteEntity(int id)
//        {
//            _repository.Delete(id);
//        }
    }
}