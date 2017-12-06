using System.Web.Mvc;

namespace Repertoir.Controllers
{
    public class AboutController : Controller
    {
        public ViewResult Index()
        {
            ViewBag.Title = "Répertoir";

            return View();
        }
    }
}
