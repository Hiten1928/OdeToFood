using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Data.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public int PeopleCount { get; set; }
        public DateTime Time { get; set; }
    }
}
