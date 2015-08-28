﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OdeToFood.Data;
using OdeToFood.Data.Models;

namespace OdeToFood.Controllers
{
    public class OrderController : Controller
    {
        private readonly DataContext _dataContext;

        public OrderController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public ActionResult Index()
        {
            List<Order> orders = _dataContext.Order.GetAll().ToList();
            return View(orders);
        }

        public ActionResult Delete(int id)
        {
            _dataContext.Order.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
