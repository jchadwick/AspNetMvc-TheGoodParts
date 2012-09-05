using TinyIoC;
using Website.Models;

namespace Website
{
    public class DependencyInjectionConfig
    {
        public static void Initialize()
        {
            var container = TinyIoCContainer.Current;
            
            // Reduce config by automatically registering everything
            container.AutoRegister();

            // Override the registration for AuctionContext 
            // so there is only one per request
            container.Register<AuctionContext>().AsPerRequestSingleton();

            // Replace ASP.NET MVC's resolver
            System.Web.Mvc.DependencyResolver.SetResolver(
                    container.Resolve, 
                    container.ResolveAll
                );
        }
    }
}