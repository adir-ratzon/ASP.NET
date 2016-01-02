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
                name: "SubmitProductsValues",
                url: "Admin/SubmitProductsValues",
                defaults: new { controller = "Admin", action = "SubmitProductsValues", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EditProducts",
                url: "Admin/EditProducts",
                defaults: new { controller = "Admin", action = "EditProducts", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AddProductsForm",
                url: "Admin/AddProducts/Form",
                defaults: new { controller = "Admin", action = "AddProductsForm", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AddProducts",
                url: "Admin/AddProducts",
                defaults: new { controller = "Admin", action = "AddProducts", id = UrlParameter.Optional }
            );

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
