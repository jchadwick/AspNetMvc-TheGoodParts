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
            // Set the Categories for the navigation
            var categories = db.Categories;
            ViewBag.Categories = categories;

            var featuredAuctions = db.Auctions.Where(x => x.IsFeatured);

            return View("Homepage", featuredAuctions);
        }
    }
}
