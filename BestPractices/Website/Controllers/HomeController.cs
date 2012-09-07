using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Website.Models;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repository = new DbContextRepository(new AuctionContext());

        public ActionResult Index()
        {
            return View("Homepage");
        }

        [ChildActionOnly]
        public ActionResult Featured()
        {
            var auctions = _repository.Query<Auction>().Where(x => x.IsFeatured);

            // Use LINQ + Mapping library (AutoMap) to populate the view model; i.e.:
            // foreach(var auction in auctions) { 
            //      viewModels.Add(Mapper.Map<AuctionViewModel>(auction); 
            // }
            var viewModels = auctions.Select(Mapper.DynamicMap<AuctionViewModel>);

            return View("_Featured", viewModels);
        }
    }
}
