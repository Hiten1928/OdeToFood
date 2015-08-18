using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OdeToFood.Contracts
{
    public interface IRestaurantReviewManager
    {
        void Create(IDbEntity review);

        IDbEntity GetEntityById(int id);

        void ValidateEntity(IDbEntity entity);

        void DeleteEntity(int id);
    }
}