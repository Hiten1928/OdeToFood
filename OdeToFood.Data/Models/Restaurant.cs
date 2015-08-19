using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public virtual ICollection<RestaurantReview> Reviews { get; set; }
    }
}
