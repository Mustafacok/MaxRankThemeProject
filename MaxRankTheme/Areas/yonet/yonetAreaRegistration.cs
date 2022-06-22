using System.Web.Mvc;

namespace MaxRankTheme.Areas.yonet
{
    public class yonetAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "yonet";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "yonet",
                url: "yonet/{controller}/{action}",
                defaults: new { controller = "Login", action = "Index"}
            );

            context.MapRoute(
                "yonet_default",
                "yonet/{controller}/{action}/{id}",
                new { controller="Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}