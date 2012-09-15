using System.Web.Mvc;
using Website.Filters;

namespace Website
{
    public class FilterConfig
    {
        public void Execute()
        {
            RegisterGlobalFilters(GlobalFilters.Filters);
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CategoriesActionFilter());
            filters.Add(new HandleErrorAttribute());
        }
    }
}