using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using AutoMapper;
using Common;
using Common.DataAccess;
using Website.Models;

namespace Website.Controllers
{
    public class SearchController : Controller
    {
        private readonly IRepository _repository;

        public SearchController(IRepository repository)
        {
            _repository = repository;
        }

        [Route("search")]
        public ActionResult Search(string query, long? category, int page = 0, int size = 5)
        {
            var viewModel = new AuctionsViewModel
            {
                Page = page,
                PageSize = size,
                SearchQuery = query,
            };

            IQueryable<Auction> auctions = _repository.Query<Auction>();

            if (!string.IsNullOrWhiteSpace(query))
            {
                auctions = auctions.Where(x =>
                           x.Title.ToLower().IndexOf(query.ToLower()) >= 0
                        || x.Description.ToLower().IndexOf(query.ToLower()) >= 0
                    );

                ViewBag.Query = query;
                ViewBag.Title = string.Format("Search results for '{0}'", query);
            }

            if (category != null)
            {
                var cat = _repository.Find<Category>(category);
                if (cat != null)
                {
                    auctions = auctions.Where(x => x.CategoryId == cat.Id);

                    ViewBag.SelectedCategory = cat.Id;
                    viewModel.CategoryName = cat.Name;
                }
            }

            auctions = auctions.OrderBy(x => x.EndTime).Skip(page * size).Take(size);
            viewModel.Auctions = auctions.Select(Mapper.DynamicMap<AuctionViewModel>).ToArray();

            if (Request.IsAjaxRequest())
                return Json(viewModel, JsonRequestBehavior.AllowGet);

            return View("Search", viewModel);
        }

        [GET("autocomplete")]
        public ActionResult Autocomplete(string query, long? category)
        {
            IEnumerable<Auction> auctions = _repository.Query<Auction>();

            if (category != null)
                auctions = auctions.Where(x => x.CategoryId == category.Value);

            var lowerQuery = query.ToLower();
            var titles =
                auctions.Select(x => x.Title)
                    .Where(x => x.ToLower().Contains(lowerQuery));

            return Json(titles.ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}
