using System.Linq;
using System.Web.Mvc;
using Common;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        private DataContext db = new DataContext();

        public ActionResult Index()
        {
            var featuredAuctions = db.Auctions.Where(x => x.IsFeatured);

            // Add the Categories to the view data for navigation, etc.
            ViewBag.Categories = db.Categories.ToArray();

            return View("Homepage", featuredAuctions);
        }
    }
}
