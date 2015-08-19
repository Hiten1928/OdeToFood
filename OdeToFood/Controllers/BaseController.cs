using System.Web.Mvc;
using OdeToFood.Data;

namespace OdeToFood.Controllers
{
    public class BaseController : Controller
    {
        protected DataContext DataContext { get; private set; }

        public BaseController(DataContext dataContext)
        {
            DataContext = dataContext;
        }

    }
}