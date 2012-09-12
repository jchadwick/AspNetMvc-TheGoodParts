using System.Linq;
using System.Web.Mvc;
using Common.DataAccess;

namespace Website.Filters
{
    public class CategoriesActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var db = new DataContext();
            var categories = db.Categories.ToArray();
            filterContext.Controller.ViewBag.Categories = categories;
        }
    }
}