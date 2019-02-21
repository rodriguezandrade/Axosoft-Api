using System.Web.Mvc;
using System.Web.Routing;

namespace SS.Mvc.AxosoftApi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                    name: "Templates",
                    url: "templates/{feature}/{view}",
                    defaults: new { controller = "Templates", action = "Index" }
                );



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}