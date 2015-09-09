using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OdeToFood
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.Add(new Route("Account/Login", new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(new { controller = "Account", action = "Login" }),
                DataTokens = new RouteValueDictionary(new { scheme = "https" })
            });
            

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapPageRoute("Angular", "angular/{*anything}", "~/angular/index.html");
        }
    }
}
