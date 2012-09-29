using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Repertoir.Helpers;
using Repertoir.Models;

namespace Repertoir.Controllers
{ 
    public class ContactsController : Controller
    {
        private RepertoirContext db  { get; set; }

        public ContactsController() { db = new RepertoirContext(); }
        public ContactsController(RepertoirContext context) { db = context; }

        //
        // GET: /Contacts/

        public ViewResult Index()
        {
            var contacts = db.Contacts.To_ContactList();

            return View(contacts);
        }

        //
        // GET: /Contacts/Table

        public ViewResult Table()
        {
            var contacts = db.Contacts.To_ContactList();

            return View(contacts);
        }

        //
        // GET: /Contacts/Export

        public JsonResult Export()
        {
            var contacts = (from c in db.Contacts
                            orderby c.IsCompany descending, c.DisplayName ascending
                            select new FlatContact
                            {
                                Slug = c.Slug,
                                DisplayName = c.DisplayName,
                                IsCompany = c.IsCompany,
                                CompanyName = c.Company_ID.HasValue ? c.Company.CompanyName : c.CompanyName,
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

            return Json(contacts, "application/json", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Contacts/Import
        public ActionResult Import()
        {
            var file = Server.MapPath("~/Scripts/sample_db.js");
            var json = System.IO.File.ReadAllText(file);

            var serializer = new JavaScriptSerializer();
            var contacts = serializer.Deserialize<List<FlatContact>>(json);

            // Vide la table des contacts actuels
            db.Database.ExecuteSqlCommand("DELETE FROM Contacts WHERE Company_ID IS NOT NULL");
            db.Database.ExecuteSqlCommand("DELETE FROM Contacts WHERE Company_ID IS NULL");
            // Réinitialise la numérotation automatique
            try
            {
                // SQL Server CE
                db.Database.ExecuteSqlCommand("ALTER TABLE Contacts ALTER COLUMN Contact_ID IDENTITY (1,1)");
            }
            catch { }
            try
            {
                // SQL Server pas CE
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE Contacts");
            }
            catch { }

            // Enregistre les sociétés
            foreach (var c in contacts)
            {
                if (c.IsCompany)
                {
                    var contact = new Contact();
                    contact.Update_With_FlatContact(c);

                    db.Contacts.Add(contact);
                }
            }
            db.SaveChanges();

            // Enregistre les personnes
            foreach (var c in contacts)
            {
                if (!c.IsCompany)
                {
                    var contact = new Contact();
                    contact.Update_With_FlatContact(c);

                    if (!string.IsNullOrEmpty(contact.CompanyName))
                    {
                        var slug = contact.CompanyName.Slugify();
                        var company = db.Contacts.Where(s => s.Slug == slug).FirstOrDefault();
                        if (company != null)
                        {
                            contact.Company_ID = company.Contact_ID;
                        }
                    }

                    db.Contacts.Add(contact);
                }
            }
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}