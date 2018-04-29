using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Saver.Server
{
    /// <summary>
    /// Route configuration for the API level of the application
    /// </summary>
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

            //User goals routing
            //routes.MapRoute("usergoals", "api/users/{userId}/goals", new { controller = "Goals" });
            //routes.MapRoute("usergoal", "api/users/{userId}/goals/{id}", new { controller = "Goals", id = RouteParameter.Optional });
        }
    }
}
