using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using AutoMapper;

using Common.DataAccess;
using Website.Extensions;
using Website.Models;
using Website.Models.Auctions;

namespace Website.Controllers
{
    public class AuctionsController : Controller
    {
        private readonly IAuctionRepository _repository;

        public AuctionsController(IAuctionRepository repository)
        {
            _repository = repository;
        }

        [Authorize]
        [POST("auctions/{id}/bids")]
        public ActionResult Bid(int id, decimal amount)
        {
            var auction = _repository.Find(id);

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
            var auction = _repository.Find(id);

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
    }
}