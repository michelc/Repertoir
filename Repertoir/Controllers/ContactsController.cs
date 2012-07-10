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
                            orderby c.DisplayName ascending
                            select new ContactList
                            {
                                Contact_ID = c.Contact_ID,
                                DisplayName = c.DisplayName,
                                Phone1 = c.Phone1,
                                Email = c.Email,
                                Civility = c.Civility,
                                IsCompany = c.IsCompany,
                                Slug = c.Slug,
                                ControllerName = c.IsCompany ? "Companies" : "People"
                            }).ToList();

            return View(contacts);
        }

        //
        // GET: /Contacts/Export

        public JsonResult Export()
        {
            var contacts = (from c in db.Contacts
                            orderby c.IsCompany descending, c.DisplayName ascending
                            select new
                            {
                                Slug = c.Slug,
                                DisplayName = c.DisplayName,
                                IsCompany = c.IsCompany,
                                CompanyName = c.Company_ID.HasValue ? c.Company.CompanyName : "",
                                Civility = c.Civility,
                                LastName = c.LastName,
                                FirstName = c.FirstName,
                                Title = c.Title,
                                Phone1 = c.Phone1,
                                Phone2 = c.Phone2,
                                Fax = c.Fax,
                                Email = c.Email,
                                Url = c.Url,
                                AddressLine1 = c.AddressLine1,
                                AddressLine2 = c.AddressLine2,
                                PostalCode = c.PostalCode,
                                Municipality = c.Municipality,
                                Country = c.Country,
                                Notes = c.Notes
                            }).ToList();

            return Json(contacts, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}