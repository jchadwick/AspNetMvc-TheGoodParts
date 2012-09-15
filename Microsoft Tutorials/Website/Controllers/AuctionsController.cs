using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Common;

namespace Website.Controllers
{
    public class AuctionsController : Controller
    {
        public static readonly string AuctionImagesFolder = "~/Content/auction-images";

        private DataContext db = new DataContext();

        public ActionResult ByCategory(string id)
        {
            // Set the Categories for the navigation
            var categories = db.Categories;
            ViewBag.Categories = categories;

            var category = categories.FirstOrDefault(x => x.Key == id);
            
            if (category == null)
                return HttpNotFound();

            var auctions = db.Auctions.Where(x => x.CategoryId == category.Id);

            ViewBag.Title = category.Name;
            ViewBag.SelectedCategory = category.Id;
            ViewBag.CategoryName = category.Name;

            return View("Auctions", auctions.ToArray());
        }

        public ActionResult Index(string query, long? category, int page=0, int size=10)
        {
            // Set the Categories for the navigation
            var categories = db.Categories;
            ViewBag.Categories = categories;

            IQueryable<Auction> auctions = db.Auctions;

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
                var cat = categories.Find(category);
                if(cat != null)
                {
                    auctions = auctions.Where(x => x.CategoryId == cat.Id);

                    ViewBag.SelectedCategory = cat.Id;
                    ViewBag.CategoryName = cat.Name;
                }
            }

            auctions = auctions.OrderBy(x => x.EndTime).Skip(page*size).Take(size);

            ViewBag.SearchQuery = query;

            return View("Auctions", auctions.ToArray());
        }

        [Authorize]
        public ActionResult Bid(int id, decimal amount)
        {
            var auction = db.Auctions.Find(id);

            if (auction == null)
                return HttpNotFound("Auction not found");

            if(auction.CurrentPrice >= amount)
            {
                TempData["ErrorMessage"] = 
                    string.Format(
                        "Your bid of {0:c} isn't higher than the current bid ({1:c}). Try again!",
                        amount, auction.CurrentPrice);

                return RedirectToAction("Details", new { id = auction.Id });
            }

            auction.PlaceBid(User.Identity.Name, amount);
            db.SaveChanges();

            TempData["SuccessMessage"] =
                string.Format(
                    "Congratulations - you're the highest bidder at {0:c}!", 
                    auction.CurrentPrice);

            return RedirectToAction("Details", new {id});
        }

        public ActionResult Details(int id)
        {
            // Set the Categories for the navigation
            var categories = db.Categories;
            ViewBag.Categories = categories;

            var auction = db.Auctions.Find(id);

            if (auction == null)
                return HttpNotFound("Auction not found");

            return View("Details", auction);
        }

        public ActionResult History(int id)
        {
            return View("History");
        }

        public ActionResult Seller(string id)
        {
            return View("Seller");
        }

        public ActionResult Autocomplete(string query, long? category)
        {
            IEnumerable<Auction> auctions = db.Auctions;

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