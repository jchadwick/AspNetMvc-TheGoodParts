using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Common.DataAccess;
using Website.Models;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuctionRepository _repository;

        public HomeController(IAuctionRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            return View("Homepage");
        }

        [ChildActionOnly]
        public ActionResult Featured()
        {
            var auctions = _repository.GetFeaturedAuctions();

            var viewModels = auctions.Select(Mapper.DynamicMap<AuctionViewModel>);

            return PartialView("_Featured", viewModels);
        }
    }
}
