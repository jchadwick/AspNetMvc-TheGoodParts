using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Website.Models;
using TinyIoC;

namespace Website
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

//            InitializeDependencyResolver();
        }

        private static void InitializeDependencyResolver()
        {
            var container = TinyIoCContainer.Current;

            container.AutoRegister();

            container.Register<IRepository, DbContextRepository>().AsPerRequestSingleton();

            DependencyResolver.SetResolver(
                container.Resolve,
                container.ResolveAll
            );
        }
    }
}