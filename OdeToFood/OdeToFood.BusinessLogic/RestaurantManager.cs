using OdeToFood.Core;
using OdeToFood.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OdeToFood.Contracts;

namespace OdeToFood.BusinessLogic
{
    public class RestaurantManager : IRestaurantManager
    {
        private readonly IRepository<Restaurant> _repository;

        public RestaurantManager(IRepository<Restaurant> repository)
        {
            _repository = repository;
        }

        public void Create(IDbEntity review)
        {
            _repository.Add((Restaurant)review);
        }

        public IDbEntity GetEntityById(int id)
        {
            Restaurant entity = (Restaurant)_repository.Get(id);
            return entity;
        }

        public void ValidateEntity(IDbEntity entity)
        {
            Restaurant restaurant = entity as Restaurant;
            _repository.Update(restaurant, restaurant.Id);
        }

        public void DeleteEntity(int id)
        {
            _repository.Delete(id);
        }
    }
}