using OdeToFood.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OdeToFood.Views.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public int TableNumber { get; set; }
        public int PeopleCount { get; set; }
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }

        public Restaurant Restaurant { get; set; }
    }
}