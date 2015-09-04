using System;
using System.ComponentModel.DataAnnotations;

namespace OdeToFood.Data.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Range(1,6)]
        public int PeopleCount { get; set; }
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
        public int TableId { get; set; }

        public virtual Table Table { get; set; }
    }
}
