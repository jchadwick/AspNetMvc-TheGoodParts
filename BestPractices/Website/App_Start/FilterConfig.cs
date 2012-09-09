﻿using System.Web.Mvc;
using Common.Util;
using Website.ActionFilters;

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
            filters.Add(new CategoriesActionFilter());
            filters.Add(new HandleErrorAttribute());
        }
    }
}