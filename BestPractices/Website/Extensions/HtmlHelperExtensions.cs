using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Common;

namespace Website.Extensions
{
    public static class HtmlHelperExtensions
    {

        /// <summary>
        /// Depends on Categories populated by <see cref="Website.Filters.CategoriesActionFilter"/>
        /// </summary>
        public static IHtmlString CategorySelector<T>(this HtmlHelper<T> html, 
                long? selectedCategory = null, string name = "CategoryId", 
                string optionLabel = null, object htmlAttributes = null
            )
        {
            var categories = html.ViewBag.Categories as IEnumerable<Category> ?? Enumerable.Empty<Category>();
            var selections = new SelectList(categories, "Id", "Name", selectedCategory);

            var dropDownList = html.DropDownList(name, selections, optionLabel, htmlAttributes);

            return new HtmlString(dropDownList.ToString());
        }
    }
}