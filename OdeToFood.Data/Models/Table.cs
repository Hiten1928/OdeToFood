using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Data.Models
{
    public class Table
    {
        [Key]
        public int Id { get; set; }
        public int TableNumber { get; set; }
        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
