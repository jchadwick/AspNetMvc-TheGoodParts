using System.Web.Mvc;

using AttributeRouting.Web.Mvc;

namespace Website.Controllers
{
    public class SellersController : Controller
    {
        [GET("sellers/{id}")]
        public ActionResult Seller(string id)
        {
            return View("Seller");
        }
    }
}
