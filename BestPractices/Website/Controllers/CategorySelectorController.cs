using System.Linq;
using System.Web.Mvc;
using Common.DataAccess;

namespace Website.Controllers
{
    public class CategorySelectorController : Controller
    {
        private readonly ICategoryRepository _repository;

        public CategorySelectorController(ICategoryRepository repository)
        {
            _repository = repository;
        }

        [ChildActionOnly]
        public ActionResult CategorySelector(
                long? selectedCategory = null, string name = null,
                string optionLabel = null, object htmlAttributes = null
            )
        {
            var categories = _repository.GetAvailableCategories().ToArray();
            var selections = new SelectList(categories, "Id", "Name", selectedCategory);

            ViewBag.Name = name ?? "CategoryId";
            ViewBag.OptionLabel = optionLabel;
            ViewBag.HtmlAttributes = htmlAttributes;

            return PartialView("CategorySelector", selections);
        }
    }
}
