using System.Web;
using System.Web.Mvc;
using Common;
using Common.Util;

namespace Website.Controllers
{
    public class ListItemController : Controller
    {
        public static readonly string AuctionImagesFolder = "~/Content/auction-images";

        private DataContext db = new DataContext();

        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            return View("ListItem", new Auction());
        }

        [HttpPost]
        [Authorize]
        public ActionResult Index(Auction auction, HttpPostedFileBase image)
        {
            // Try to set the username here...
            auction.SellerUsername = User.Identity.Name;

            // But it's too late - the binding errors have already happened!  
            // Remove them :(
            if (ModelState.ContainsKey("SellerUsername"))
                ModelState.Remove("SellerUsername");

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

            return Index();
        }
    }
}