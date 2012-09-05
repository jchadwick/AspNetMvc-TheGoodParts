using System.Linq;
using System.Web.Mvc;
using Website.Models;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly AuctionContext _db = new AuctionContext();

        public ActionResult Index()
        {
            ViewBag.FeaturedAuctions = _db.Auctions.Where(x => x.IsFeatured);

            return View("Homepage");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
