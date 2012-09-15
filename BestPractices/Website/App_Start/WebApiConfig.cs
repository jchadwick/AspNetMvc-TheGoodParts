using System.Web.Http;

namespace Website
{
    public class WebApiConfig
    {
        public void Execute()
        {
            Register(GlobalConfiguration.Configuration);
        }

        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
