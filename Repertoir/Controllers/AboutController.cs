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

        public ActionResult Config()
        {
            return File(Server.MapPath("/web.config"), "application/xml");
        }
    }
}
