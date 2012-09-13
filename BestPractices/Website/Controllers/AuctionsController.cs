using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Common;
using Common.DataAccess;
using Common.Util;
using Website.Extensions;
using Website.Models;

namespace Website.Controllers
{
    public class AuctionsController : Controller
    {
        private readonly DataContext _db = new DataContext();
        public static readonly string AuctionImagesFolder = "~/Content/auction-images";

        public ActionResult ByCategory(string id)
        {
            var category = _db.Categories.FirstOrDefault(x => x.Key == id);
            
            if (category == null)
                return HttpNotFound();

            var auctions = _db.Auctions.Where(x => x.CategoryId == category.Id);

            ViewBag.Title = category.Name;
            ViewBag.SelectedCategory = category.Key;

            var viewModel = new AuctionsViewModel
            {
                Auctions = auctions.Select(Mapper.DynamicMap<AuctionViewModel>).ToArray(),
            };

            return View("Auctions", viewModel);
        }

        public ActionResult Index(string query, string category, int page=0, int size=10)
        {
            IQueryable<Auction> auctions = _db.Auctions;

            if(!string.IsNullOrWhiteSpace(query))
            {
                auctions = auctions.Where(x =>
                           x.Title.ToLower().IndexOf(query.ToLower()) >= 0
                        || x.Description.ToLower().IndexOf(query.ToLower()) >= 0
                    );

                ViewBag.Query = query;
                ViewBag.Title = string.Format("Search results for '{0}'", query);
            }

            if(!string.IsNullOrWhiteSpace(category))
            {
                var cat = _db.Categories.FirstOrDefault(x => x.Key == category);
                if(cat != null)
                {
                    auctions = auctions.Where(x => x.CategoryId == cat.Id);

                    ViewBag.SelectedCategory = cat.Key;
                }
            }

            auctions = auctions.OrderBy(x => x.EndTime).Skip(page*size).Take(size);

            var viewModel = new AuctionsViewModel {
                    Auctions = auctions.Select(Mapper.DynamicMap<AuctionViewModel>).ToArray(),
                    Page = page,
                    PageSize = size,
                    SearchQuery = query,
                };

            return View("Auctions", viewModel);
        }

        [Authorize]
        public ActionResult Bid(int id, decimal amount)
        {
            var auction = _db.Auctions.Find(id);

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
            _db.SaveChanges();

            TempData.SuccessMessage(
                "Congratulations - you're the highest bidder at {0:c}!", 
                auction.CurrentPrice);

            return RedirectToAction("Details", new {id});
        }

        public ActionResult Details(int id)
        {
            var auction = _db.Auctions.Find(id);

            if (auction == null)
                return HttpNotFound("Auction not found");

            var viewModel = Mapper.DynamicMap<AuctionViewModel>(auction);
            
            return View("Details", viewModel);
        }

        public ActionResult History(int id)
        {
            return View("History");
        }

        public ActionResult Seller(string id)
        {
            return View("Seller");
        }
    }
}