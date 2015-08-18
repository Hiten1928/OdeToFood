﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OdeToFood.Contracts;

namespace OdeToFood.Core
{
    public class RestaurantReview : BaseEntity, IDbEntity
    {
        [Key]
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Body { get; set; }
        public int RestaurantId { get; set; }
    }
}