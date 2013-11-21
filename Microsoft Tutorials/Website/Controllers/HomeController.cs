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

        public ActionResult Demo()
        {
            // Set the Categories for the navigation
            var categories = db.Categories;
            ViewBag.Categories = categories;

            // Return implicitly-named view.
            // Works fine when Demo() is called directly...
            // but not when called from another action!
            return View();
        }

        public ActionResult AltDemo()
        {
            // BOOM!  "The view 'AltDemo' or its master was not found"!
            return Demo();
        }
    }
}
