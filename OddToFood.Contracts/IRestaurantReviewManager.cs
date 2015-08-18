using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddToFood.Contracts
{
    public interface IRestaurantReviewManager
    {
        void Create(IDbEntity review);

        IDbEntity GetEntityById(int id);

        void ValidateEntity(IDbEntity entity);

        void DeleteEntity(int id);
    }
}
