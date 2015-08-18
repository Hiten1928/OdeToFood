using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OdeToFood.Contracts;

namespace OdeToFood.Core
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