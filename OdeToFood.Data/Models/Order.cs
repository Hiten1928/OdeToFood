﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Data.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public int PeopleCount { get; set; }
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
        public int RestaurantId { get; set; }
    }
}
