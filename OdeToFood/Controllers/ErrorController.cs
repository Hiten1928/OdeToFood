using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OdeToFood.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult HttpError(string message)
        {
            object str = message;
            return View(str);
        }
    }
}