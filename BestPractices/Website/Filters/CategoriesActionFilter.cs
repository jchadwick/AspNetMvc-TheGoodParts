using System.Linq;
using System.Web.Mvc;
using Common;
using Common.DataAccess;
using Munq.MVC3;

namespace Website.Filters
{
    public class CategoriesActionFilter : ActionFilterAttribute
    {
        private readonly IRepository _repository;

        // If MVC *really* supported IoC out of the box, I wouldn't have to do this!
        public CategoriesActionFilter()
            : this(MunqDependencyResolver.Container.Resolve<IRepository>())
        {
        }

        public CategoriesActionFilter(IRepository repository)
        {
            _repository = repository;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var categories = _repository.Query<Category>().ToArray();
            filterContext.Controller.ViewBag.Categories = categories;
        }
    }
}