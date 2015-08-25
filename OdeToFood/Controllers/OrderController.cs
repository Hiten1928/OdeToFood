using System;
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

//        public ActionResult Edit(int id)
//        {
//            return View();
//        }
//
//        [HttpPost]
//        public ActionResult Edit(int id, FormCollection collection)
//        {
//            try
//            {
//                // TODO: Add update logic here
//
//                return RedirectToAction("Index");
//            }
//            catch
//            {
//                return View();
//            }
//        }

        public ActionResult Delete(int id)
        {
            _dataContext.Order.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
