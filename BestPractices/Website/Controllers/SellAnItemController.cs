using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using AutoMapper;
using Common;
using Common.DataAccess;
using Common.Util;
using Website.Extensions;
using Website.Models;
using Website.Models.Auctions;

namespace Website.Controllers
{
    [Authorize]
    public class SellAnItemController : Controller
    {
        public static readonly string AuctionImagesFolder = "~/Content/auction-images";

        private readonly IAuctionRepository _repository;

        public SellAnItemController(IAuctionRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("sell")]
        public ActionResult Sell()
        {
            return View("Sell", new ListItemRequest());
        }

        [HttpPost]
        [Route("sell")]
        public ActionResult Sell(ListItemRequest request)
        {
            if (ModelState.IsValid)
            {
                var auction = Mapper.DynamicMap<Auction>(request);

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

            return View("Sell", request);
        }
    }
}