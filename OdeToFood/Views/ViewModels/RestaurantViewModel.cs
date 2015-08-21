using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OdeToFood.Data.Models;

namespace OdeToFood.Views.ViewModels
{
    public class RestaurantViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int TableCount { get; set; }
    }
}