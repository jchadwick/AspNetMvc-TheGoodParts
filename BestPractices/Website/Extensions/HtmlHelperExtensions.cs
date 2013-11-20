using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Common;
using Website.Models;
using Website.Models.Auctions;

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
            return html.DropDownList(name, selections, optionLabel, htmlAttributes);
        }


        public static IHtmlString CategorySelector<T>(this HtmlHelper<T> html, 
                long? selectedCategory = null, string name = null,
                string optionLabel = null, object htmlAttributes = null
            )
        {
            return html.Action("CategorySelector", "CategorySelector", 
                new {selectedCategory, name, optionLabel, htmlAttributes});
        }
    }
}