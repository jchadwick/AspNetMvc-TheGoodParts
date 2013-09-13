using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Common.Util;

namespace Website.Controllers
{
    public class AuctionsController : Controller
    {
        private DataContext db = new DataContext();
        private const string AuctionImagesFolder = "~/Content/auction-images";

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

                [HttpGet]
        [Authorize]
        public ActionResult ListItem()
        {
            // Set the Categories for the navigation
            var categories = db.Categories;
            ViewBag.Categories = categories;

            return View(new Auction());
        }

        [HttpPost]
        [Authorize]
        public ActionResult ListItem(Auction auction, HttpPostedFileBase image, int duration)
        {
            // Try to set the username here...
            auction.SellerUsername = User.Identity.Name;

            // But it's too late - the binding errors have already happened!  
            // Remove them :(
            if (ModelState.ContainsKey("SellerUsername"))
                ModelState.Remove("SellerUsername");

            // And we never set the EndTime because we used "duration" instead...
            if (ModelState.ContainsKey("EndTime"))
                ModelState.Remove("EndTime");

            if (duration < 3)
            {
                ModelState.AddModelError("Duration", "Auction must last at least 3 days");
            }
            else
            {
                auction.EndTime = auction.StartTime.AddDays(duration);
            }

            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    string imageUrl, thumbnailUrl;

                    new ImageRepository(Server.MapPath(AuctionImagesFolder), Url.Content(AuctionImagesFolder))
                        .SaveImage(image.FileName, image.InputStream,
                                   out imageUrl, out thumbnailUrl);

                    auction.ImageUrl = imageUrl;
                    auction.ThumbnailUrl = thumbnailUrl;
                }

                db.Auctions.Add(auction);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Congratulations, your item has been listed for auction!";

                return RedirectToAction("Details", "Auctions", new { id = auction.Id });
            }

            TempData["ErrorMessage"] = "Error listing item - please make sure that you've filled in everything correctly!";

            return View(auction);
        }
    }

}