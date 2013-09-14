using System.Web.Mvc;
using System.Web.Routing;

namespace Website
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // The only way (the easiest, at least) to control routing -- custom routes:
            routes.MapRoute(
                name: "Login",
                url: "login",
                defaults: new { controller = "Account", action = "Login" }
            );

            routes.MapRoute(
                name: "Auction",
                url: "auctions/{id}",
                defaults: new { controller = "Auctions", action = "Details" },
                constraints: new { id = @"\d" }
            );

            routes.MapRoute(
                name: "Autocomplete",
                url: "autocomplete",
                defaults: new { controller = "Auctions", action = "Autocomplete" }
            );


            // The default route is still here to continue to apply the standard convention
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}