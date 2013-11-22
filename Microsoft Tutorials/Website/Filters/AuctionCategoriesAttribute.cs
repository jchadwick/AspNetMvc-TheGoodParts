using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Caching;
using System.Web.Mvc;
using Common;

namespace Website
{
    public class AuctionCategoriesAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var viewBag = filterContext.Controller.ViewBag;
            var cache = filterContext.HttpContext.Cache;

            var categories = cache["Categories"] as IEnumerable<Category>;

            if (categories == null)
            {
                categories = new DataContext().Categories.ToArray();

                cache.Add(
                    "Categories", categories, null, 
                    Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(60),
                    CacheItemPriority.AboveNormal, null);
            }

            viewBag.Categories = categories;
        }
    }
}