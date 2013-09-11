using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
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
        public static readonly string AuctionImagesFolder = "~/Content/auction-images";

        private readonly IRepository _repository;

        public ListItemController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Authorize]
        [GET("sell")]
        public ActionResult Index()
        {
            return View("ListItem", new ListItemRequest());
        }

        [HttpPost]
        [Authorize]
        [POST("sell")]
        public ActionResult Index(ListItemRequest request)
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

                _repository.Add(auction);
                _repository.SaveChanges();

                TempData.SuccessMessage("Congratulations, your item has been listed for auction!");

                return RedirectToAction("Details", "Auctions", new { id = auction.Id });
            }

            TempData.ErrorMessage("Error listing item - please make sure that you've filled in everything correctly! ");
            return Index();
        }
    }
}