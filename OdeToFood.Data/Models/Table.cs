using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Data.Models
{
    public class Table
    {
        [Key]
        public int Id { get; set; }
        public bool IsFree { get; set; }
    }
}
