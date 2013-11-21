using System.Web.Mvc;

using AttributeRouting.Web.Mvc;

namespace Website.Controllers
{
    public class HomepageController : Controller
    {
        [Route("")]
        public ActionResult Homepage()
        {
            return View("Homepage");
        }
    }
}
