using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Data.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Range(1,4)]
        public int PeopleCount { get; set; }
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
        public int TableId { get; set; }

        public virtual Table Table { get; set; }
    }
}
