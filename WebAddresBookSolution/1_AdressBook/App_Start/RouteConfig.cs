using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace _1_AdressBook
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}",
                defaults: new { controller = "Person", action = "Index", page = 1}
            );
            routes.MapRoute(
                name: "WithPara",
                url: "{controller}/{action}/{page}",
                defaults: new { controller = "Person", action = "Index", page = UrlParameter.Optional }
            );
        }
    }
}
