using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MaxRankTheme
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            

            routes.MapRoute(
                name: "About",
                url: "about/{adi}",
                defaults: new { controller = "About", action = "Index", adi = UrlParameter.Optional },
                namespaces: new[] { "MaxRankTheme.Controllers" }
            );

            routes.MapRoute(
                name: "Blog",
                url: "blog/{adi}",
                defaults: new { controller = "Blog", action = "Index", adi = UrlParameter.Optional },
                namespaces: new[] { "MaxRankTheme.Controllers" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces:new[] {"MaxRankTheme.Controllers"}
            );
        }
    }
}
