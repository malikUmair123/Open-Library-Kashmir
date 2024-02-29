using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Open_Library_Kashmir
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Enabling attribute routing 
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "ImportCSV",
                url: "Books/Import",
                defaults: new { controller = "Books", action = "Import" }
            );


            //route for Account pages register and login
            routes.MapRoute(
                name: "Account",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints: new { controller = "Account" }
            );

            //route for Manage pages
            routes.MapRoute(
                name: "Manage",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints: new { controller = "Manage" }
            );

            //route for all pages
            routes.MapRoute(
                name: "Default",
                url: "{action}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
