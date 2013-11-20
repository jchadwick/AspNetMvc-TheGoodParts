using System.Linq;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using AutoMapper;
using Common;
using Common.DataAccess;
using Website.Models;
using Website.Models.Auctions;

namespace Website.Controllers
{
    public class SearchController : Controller
    {
        private readonly AuctionRepository _repository;

        public SearchController(AuctionRepository repository)
        {
            _repository = repository;
        }

        [GET("autocomplete")]
        public ActionResult Autocomplete(string query, long? category)
        {
            var suggestions = _repository.Autocomplete(query, category);
            return Json(suggestions.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [GET("categories/{categoryKey}")]
        public ActionResult ByCategory(string categoryKey)
        {
            Category category;

            var auctions = _repository.FindByCategoryKey(categoryKey, out category);

            if (category == null)
                return HttpNotFound();

            ViewBag.Title = category.Name;
            ViewBag.SelectedCategory = category.Id;

            var viewModel = new AuctionsViewModel
            {
                Auctions = auctions.Select(Mapper.DynamicMap<AuctionViewModel>).ToArray(),
                CategoryName = category.Name,
            };

            return View("Search", viewModel);
        }

        [Route("search")]
        public ActionResult Search(string query, long? category, int page = 0, int size = 5)
        {
            Category selectedCategory;


            var auctions =
                _repository
                    .Search(query, category, out selectedCategory)
                    .OrderBy(x => x.EndTime)
                    .Skip(page * size)
                    .Take(size);



            var viewModel = new AuctionsViewModel
            {
                Auctions = auctions.Select(Mapper.DynamicMap<AuctionViewModel>).ToArray(),
                Page = page,
                PageSize = size,
                SearchQuery = query,
            };


            ViewBag.Title = string.Format("Search results for '{0}'", query);
            ViewBag.Query = query ?? string.Empty;


            if (selectedCategory != null)
            {
                ViewBag.SelectedCategory = selectedCategory;
                viewModel.CategoryName = selectedCategory.Name;
            }


            if (Request.IsAjaxRequest())
                return Json(viewModel, JsonRequestBehavior.AllowGet);

            return View("Search", viewModel);
        }
    }
}
