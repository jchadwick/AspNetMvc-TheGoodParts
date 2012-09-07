using System.Web.Mvc;
using Common.Util;

namespace Website
{
    public class FilterConfig : IBootstrapperTask
    {
        public void Execute()
        {
            RegisterGlobalFilters(GlobalFilters.Filters);
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}