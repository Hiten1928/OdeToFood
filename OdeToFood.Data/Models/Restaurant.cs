using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OddToFood.Contracts;

namespace OdeToFood.Data.Models
{
    public class Restaurant : BaseEntity, IDbEntity
    {
        [Key] 
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        public virtual ICollection<Table> Tables { get; set; }
        public virtual ICollection<RestaurantReview> Reviews { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        
    }
}
