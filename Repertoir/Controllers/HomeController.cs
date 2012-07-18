using System.Web.Mvc;

namespace Repertoir.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            ViewBag.Title = "Répertoir";

            return View();
        }
    }
}
