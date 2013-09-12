using System.Linq;
using System.Web.Mvc;
using Common.DataAccess;
using Munq.MVC3;

namespace Website.Filters
{
    public class CategoriesActionFilter : ActionFilterAttribute
    {
        private readonly ICategoryRepository _repository;

        // If MVC *really* supported IoC out of the box, I wouldn't have to do this!
        public CategoriesActionFilter()
            : this(MunqDependencyResolver.Container.Resolve<ICategoryRepository>())
        {
        }

        public CategoriesActionFilter(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var categories = _repository.GetAvailableCategories().ToArray();
            filterContext.Controller.ViewBag.Categories = categories;
        }
    }
}