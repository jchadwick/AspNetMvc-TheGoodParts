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
    public class ListItemController : Controller
    {
        private readonly DataContext _db = new DataContext();
        public static readonly string AuctionImagesFolder = "~/Content/auction-images";

        
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            return View("ListItem", new ListItemRequest());
        }

        [HttpPost]
        [Authorize]
        public ActionResult Index(ListItemRequest request, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                var auction = new Auction();

                Mapper.DynamicMap(request, auction);

                if (request.Image != null)
                {
                    string imageUrl, thumbnailUrl;

                    new ImageRepository(Server.MapPath(AuctionImagesFolder), Url.Content(AuctionImagesFolder))
                        .SaveImage(request.Image.FileName, request.Image.InputStream,
                                   out imageUrl, out thumbnailUrl);

                    auction.ImageUrl = imageUrl;
                    auction.ThumbnailUrl = thumbnailUrl;
                }

                _db.Auctions.Add(auction);
                _db.SaveChanges();

                TempData.SuccessMessage("Congratulations, your item has been listed for auction!");

                return RedirectToAction("Details", "Auctions", new { id = auction.Id });
            }

            TempData.ErrorMessage("Error listing item - please make sure that you've filled in everything correctly! ");
            return Index();
        }
    }
}