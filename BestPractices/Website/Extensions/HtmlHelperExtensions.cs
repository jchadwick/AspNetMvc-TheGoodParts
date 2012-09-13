using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public static IHtmlString CategorySelector<T>(this HtmlHelper<T> html, string selectedCategory = null, string name = "CategoryId", bool validate = false, object htmlAttributes = null)
        {
            var categories = html.ViewBag.Categories as IEnumerable<Category> ?? Enumerable.Empty<Category>();
            var selections = new SelectList(categories, "Key", "Name", selectedCategory);

            var tag = new StringBuilder();
            tag.AppendLine(html.DropDownList(name, selections, "All Categories", htmlAttributes).ToString());
            
            if (validate)
                tag.AppendLine(html.ValidationMessage(name).ToString());

            return new HtmlString(tag.ToString());
        }

    }
}