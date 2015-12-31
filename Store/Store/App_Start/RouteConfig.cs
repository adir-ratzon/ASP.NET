using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Store
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.config");

            routes.MapRoute(
                name: "AdminArea",
                url: "Admin",
                defaults: new { controller = "Admin", action = "AdminArea", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Logon",
                url: "login",
                defaults: new { controller = "Admin", action = "Admin", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "HomePage",
                url: "",
                defaults: new { controller = "HomePage", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
