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
                                ID = c.ID,
                                LastName = c.LastName,
                                FirstName = c.FirstName,
                                Phone1 = c.Phone1,
                                Email = c.Email
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