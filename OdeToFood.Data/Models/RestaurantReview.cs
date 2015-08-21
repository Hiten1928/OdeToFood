using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using OddToFood.Contracts;

namespace OdeToFood.Data.Models
{
    public class RestaurantReview : BaseEntity, IDbEntity
    {
        [Key]
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Body { get; set; }
        public int RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }
    }
}