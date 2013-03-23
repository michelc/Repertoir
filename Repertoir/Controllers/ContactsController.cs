using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AutoMapper;
using Repertoir.Helpers;
using Repertoir.Models;

namespace Repertoir.Controllers
{ 
    public class ContactsController : Controller
    {
        private RepertoirContext db;

        public ContactsController() { db = new RepertoirContext(); }
        public ContactsController(RepertoirContext context) { db = context; }

        //
        // GET: /Contacts/

        public ViewResult Index()
        {
            var contacts = db.Contacts.List();

            return View(contacts);
        }

        //
        // GET: /Contacts/Table

        public ViewResult Table()
        {
            var contacts = db.Contacts.List();

            return View(contacts);
        }

        //
        // GET: /Contacts/Next/5

        public RedirectToRouteResult Next(int id)
        {
            var contact = db.Contacts.Find(id);

            var query = (from c in db.Contacts
                         orderby c.DisplayName ascending
                         select new
                         {
                             c.DisplayName,
                             c.Contact_ID,
                             c.Slug,
                             ControllerName = c.IsCompany ? "Companies" : "People"
                         });

            var next = (from c in query
                        where (c.DisplayName.CompareTo(contact.DisplayName) > 0)
                        select c).FirstOrDefault()
                        ??
                        (from c in query
                        select c).FirstOrDefault();

            return RedirectToAction("Details", next.ControllerName, new { id = next.Contact_ID, slug = next.Slug });
        }

        //
        // GET: /Contacts/Previous/5

        public RedirectToRouteResult Previous(int id)
        {
            var contact = db.Contacts.Find(id);

            var query = (from c in db.Contacts
                         orderby c.DisplayName descending
                         select new
                         {
                             c.DisplayName,
                             c.Contact_ID,
                             c.Slug,
                             ControllerName = c.IsCompany ? "Companies" : "People"
                         });

            var prev = (from c in query
                        where (c.DisplayName.CompareTo(contact.DisplayName) < 0)
                        select c).FirstOrDefault()
                        ??
                        (from c in query
                         select c).FirstOrDefault();

            return RedirectToAction("Details", prev.ControllerName, new { id = prev.Contact_ID, slug = prev.Slug });
        }

        //
        // GET: /Contacts/Export

        public JsonResult Export()
        {
            var contacts = db.Contacts.OrderByDescending(c => c.IsCompany).ThenBy(c => c.DisplayName);
            var model = Mapper.Map<IList<FlatContact>>(contacts);

            return Json(model, "application/json", JsonRequestBehavior.AllowGet);
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
            db.Database.ExecuteSqlCommand("DELETE FROM Contacts_Tags");
            db.Database.ExecuteSqlCommand("DELETE FROM Tags");
            // Réinitialise la numérotation automatique
            try
            {
                // SQL Server CE
                db.Database.ExecuteSqlCommand("ALTER TABLE Contacts ALTER COLUMN Contact_ID IDENTITY (1,1)");
                db.Database.ExecuteSqlCommand("ALTER TABLE Tags ALTER COLUMN Tag_ID IDENTITY (1,1)");
            }
            catch { }
            try
            {
                // SQL Server pas CE
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE Contacts");
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE Tags");
            }
            catch { }

            // Enregistre les tags
            var tags = new string[0];
            foreach (var c in contacts)
            {
                if (!string.IsNullOrEmpty(c.Tags))
                {
                    tags = tags.Concat(c.Tags.Split(',')).Distinct().ToArray();
                }
            }
            foreach (var t in tags.OrderBy(t => t))
            {
                var tag = new Tag();
                tag.Caption = t;
                db.Tags.Add(tag);
            }
            db.SaveChanges();

            // Enregistre les sociétés
            foreach (var c in contacts)
            {
                if (c.IsCompany)
                {
                    var contact = new Contact();
                    contact.Update_With_FlatContact(c);

                    if (!string.IsNullOrEmpty(c.Tags))
                    {
                        tags = c.Tags.Split(',');
                        contact.Tags = (from t in db.Tags
                                        where tags.Contains(t.Caption)
                                        select t).ToList();
                    }

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

                    if (!string.IsNullOrEmpty(c.Tags))
                    {
                        tags = c.Tags.Split(',');
                        contact.Tags = (from t in db.Tags
                                        where tags.Contains(t.Caption)
                                        select t).ToList();
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