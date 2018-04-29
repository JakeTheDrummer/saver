using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Saver.Server
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            //Map the user goals route
            routes.MapRoute(
                name: "GoalsRoute",
                url: "{user}/{userID}/{controller}/{action}",
                defaults: new { controller = "Goals", action = "Get", id = UrlParameter.Optional }
            );

        }
    }
}
