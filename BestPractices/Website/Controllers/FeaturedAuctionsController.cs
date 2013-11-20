using System.Linq;
using System.Web.Mvc;

using AttributeRouting.Web.Mvc;

using AutoMapper;

using Common.DataAccess;

using Website.Models;
using Website.Models.Auctions;

namespace Website.Controllers
{
    public class FeaturedAuctionsController : Controller
    {
        private readonly IAuctionRepository _repository;

        public FeaturedAuctionsController(IAuctionRepository repository)
        {
            _repository = repository;
        }

        // Still needs a route, even though it's impossible to navigate to "externally" -- weird, huh?
        [Route("FeaturedAuctions")]
        [ChildActionOnly]
        public ActionResult FeaturedAuctions()
        {
            var auctions = _repository.GetFeaturedAuctions();

            var viewModels = auctions.Select(Mapper.DynamicMap<AuctionViewModel>);

            return PartialView("_Featured", viewModels);
        }
    }
}