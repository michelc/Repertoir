using System.Web.Mvc;

namespace Repertoir.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Répertoir";

            return View();
        }
    }
}
