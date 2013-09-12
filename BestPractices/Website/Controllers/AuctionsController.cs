using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using AutoMapper;
using Common;
using Common.DataAccess;
using Website.Extensions;
using Website.Models;

namespace Website.Controllers
{
    public class AuctionsController : Controller
    {
        private readonly IRepository _repository;

        public AuctionsController(IRepository repository)
        {
            _repository = repository;
        }

        [GET("categories/{categoryKey}")]
        public ActionResult ByCategory(string categoryKey)
        {
            var category = _repository.Query<Category>().FirstOrDefault(x => x.Key == categoryKey);
            
            if (category == null)
                return HttpNotFound();

            var auctions = _repository.Query<Auction>().Where(x => x.CategoryId == category.Id);

            ViewBag.Title = category.Name;
            ViewBag.SelectedCategory = category.Id;

            var viewModel = new AuctionsViewModel
            {
                Auctions = auctions.Select(Mapper.DynamicMap<AuctionViewModel>).ToArray(),
                CategoryName = category.Name,
            };

            return View("Auctions", viewModel);
        }

        [GET("auctions")]
        public ActionResult Index(string query, long? category, int page = 0, int size = 10)
        {
            var viewModel = new AuctionsViewModel {
                    Page = page,
                    PageSize = size,
                    SearchQuery = query,
                };

            IQueryable<Auction> auctions = _repository.Query<Auction>();

            if(!string.IsNullOrWhiteSpace(query))
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
                if(cat != null)
                {
                    auctions = auctions.Where(x => x.CategoryId == cat.Id);

                    ViewBag.SelectedCategory = cat.Id;
                    viewModel.CategoryName = cat.Name;
                }
            }

            auctions = auctions.OrderBy(x => x.EndTime).Skip(page*size).Take(size);
            viewModel.Auctions = auctions.Select(Mapper.DynamicMap<AuctionViewModel>).ToArray();

            return View("Auctions", viewModel);
        }

        [Authorize]
        [POST("auctions/{id}/bids")]
        public ActionResult Bid(int id, decimal amount)
        {
            var auction = _repository.Find<Auction>(id);

            if (auction == null)
                return HttpNotFound("Auction not found");

            if(auction.CurrentPrice >= amount)
            {
                TempData.ErrorMessage(
                        "Your bid of {0:c} isn't higher than the current bid ({1:c}). Try again!",
                        amount, auction.CurrentPrice);

                return RedirectToAction("Details", new { id = auction.Id });
            }

            auction.PlaceBid(User.Identity.Name, amount);
            _repository.SaveChanges();

            TempData.SuccessMessage(
                "Congratulations - you're the highest bidder at {0:c}!", 
                auction.CurrentPrice);

            return RedirectToAction("Details", new {id});
        }

        [GET("auctions/{id}")]
        public ActionResult Details(int id)
        {
            var auction = _repository.Find<Auction>(id);

            if (auction == null)
                return HttpNotFound("Auction not found");

            var viewModel = Mapper.DynamicMap<AuctionViewModel>(auction);
            
            return View("Details", viewModel);
        }

        [GET("auctions/{id}/history")]
        public ActionResult History(int id)
        {
            return View("History");
        }

        [GET("sellers/{id}")]
        public ActionResult Seller(string id)
        {
            return View("Seller");
        }
    }
}