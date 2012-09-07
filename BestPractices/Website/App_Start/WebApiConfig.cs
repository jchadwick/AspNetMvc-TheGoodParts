using System.Web.Http;
using Common.Util;

namespace Website
{
    public class WebApiConfig : IBootstrapperTask
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
