using System.Web.Mvc;
using System.Web.Routing;
using LowercaseRoutesMVC;

namespace Repertoir
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRouteLowercase(
                name: "Id_Slug",
                url: "{controller}/{action}/{id}/{slug}",
                defaults: new { controller = "Contacts", action = "Index" }
            );

            routes.MapRouteLowercase(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Contacts", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}