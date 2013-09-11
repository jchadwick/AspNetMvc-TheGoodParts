using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Common;
using Website.Models;

namespace Website.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlString AuctionLink(this HtmlHelper html, AuctionViewModel auction)
        {
            return html.ActionLink(auction.Title, "Details", "Auctions", new {auction.Id}, null);
        }

        public static IHtmlString CategoryLink(this HtmlHelper html, Category category)
        {
            return html.ActionLink(category.Name, "ByCategory", "Auctions", new { categoryKey = category.Key }, null);
        }

        public static IHtmlString ConditionSelector<T>(this HtmlHelper<T> html, 
                ItemCondition? condition = null, string name = "Condition",
                string optionLabel = null, object htmlAttributes = null
            )
        {
            var conditions = Enum.GetNames(typeof (ItemCondition));
            var selections = new SelectList(conditions, condition);

            var dropDownList = html.DropDownList(name, selections, optionLabel, htmlAttributes);
            return new HtmlString(dropDownList.ToString());
        }


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