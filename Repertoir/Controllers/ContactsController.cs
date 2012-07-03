using System.Linq;
using System.Web.Mvc;
using Repertoir.Models;

namespace Repertoir.Controllers
{ 
    public class ContactsController : Controller
    {
        private RepertoirContext db = new RepertoirContext();

        //
        // GET: /Contacts/

        public ViewResult Index()
        {
            var contacts = (from c in db.Contacts
                            orderby c.LastName ascending, c.FirstName ascending
                            select new ContactList
                            {
                                Contact_ID = c.Contact_ID,
                                DisplayName = c.DisplayName,
                                Phone1 = c.Phone1,
                                Email = c.Email,
                                ControllerName = c.LastName == "*" ? "Companies" : "People"
                            }).ToList();

            return View(contacts);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}