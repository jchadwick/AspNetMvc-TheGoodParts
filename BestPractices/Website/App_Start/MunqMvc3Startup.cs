using System.Data.Entity;
using System.Web.Mvc;
using Common.DataAccess;
using Munq;
using Munq.MVC3;

[assembly: WebActivator.PreApplicationStartMethod(
	typeof(Website.App_Start.MunqMvc3Startup), "PreStart")]

namespace Website.App_Start {
	public static class MunqMvc3Startup {
		public static void PreStart() {
			DependencyResolver.SetResolver(new MunqDependencyResolver());

			 var ioc = MunqDependencyResolver.Container;

			 ioc.Register<DbContext, DataContext>().AsRequestSingleton();
             ioc.Register<IAuctionRepository, AuctionRepository>().AsRequestSingleton();
             ioc.Register<ICategoryRepository, CategoryRepository>().AsRequestSingleton();
		}
	}
}
 

