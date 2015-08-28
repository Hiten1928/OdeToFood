using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OdeToFood.Views.ViewModels
{
    public class TableViewModel
    {
        public int RestaurantId { get; set; }
        public int TableNumber { get; set; }
        public ICollection<DateTime> AvialableTimes { get; set; } 
    }
}