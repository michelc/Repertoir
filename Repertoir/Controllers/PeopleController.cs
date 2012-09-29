using System.Data;
using System.Linq;
using System.Web.Mvc;
using Repertoir.Helpers;
using Repertoir.Models; 

namespace Repertoir.Controllers
{
    public class PeopleController : Controller
    {
        private RepertoirContext db = new RepertoirContext();

        public PeopleController() { db = new RepertoirContext(); }
        public PeopleController(RepertoirContext context) { db = context; }

        //
        // GET: /People/Details/5

        public ViewResult Details(int id)
        {
            var contact = db.Contacts.Find(id);
            var person = contact.To_ViewPerson();

            return View(person);
        }

        //
        // GET: /People/Display/5

        public ViewResult Display(int id)
        {
            var contact = db.Contacts.Find(id);
            var person = contact.To_ViewPerson();

            return View(person);
        }

        //
        // GET: /People/Next/5

        public RedirectResult Next(int id)
        {
            var contact = db.Contacts.Find(id);

            var next = (from c in db.Contacts
                        where (c.DisplayName.CompareTo(contact.DisplayName) > 0)
                        orderby c.DisplayName ascending
                        select new
                        {
                            c.Contact_ID,
                            c.Slug,
                            ControllerName = c.IsCompany ? "Companies" : "People"
                        }).FirstOrDefault();

            if (next == null)
            {
                next = (from c in db.Contacts
                        orderby c.DisplayName ascending
                        select new
                        {
                            c.Contact_ID,
                            c.Slug,
                            ControllerName = c.IsCompany ? "Companies" : "People"
                        }).FirstOrDefault();
            }

            return Redirect(Url.Action("Details", next.ControllerName, new { id = next.Contact_ID, slug = next.Slug }));
        }

        //
        // GET: /People/Previous/5

        public RedirectResult Previous(int id)
        {
            var contact = db.Contacts.Find(id);

            var next = (from c in db.Contacts
                        where (c.DisplayName.CompareTo(contact.DisplayName) < 0)
                        orderby c.DisplayName descending
                        select new
                        {
                            c.Contact_ID,
                            c.Slug,
                            ControllerName = c.IsCompany ? "Companies" : "People"
                        }).FirstOrDefault();

            if (next == null)
            {
                next = (from c in db.Contacts
                        orderby c.DisplayName descending
                        select new
                        {
                            c.Contact_ID,
                            c.Slug,
                            ControllerName = c.IsCompany ? "Companies" : "People"
                        }).FirstOrDefault();
            }

            return Redirect(Url.Action("Details", next.ControllerName, new { id = next.Contact_ID, slug = next.Slug }));
        }

        //
        // GET: /People/Create

        public ViewResult Create(int ParentID = 0)
        {
            var contact = new Contact();
            if (ParentID != 0)
            {
                contact.Company_ID = ParentID;
                contact.Company = db.Contacts.Find(ParentID);
            }
            var person = contact.To_ViewPerson();

            person.Companies = ListCompanies(person.Company_ID);
            return View(person);
        }

        //
        // POST: /People/Create

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(ViewPerson person)
        {
            if (ModelState.IsValid)
            {
                var contact = new Contact();
                contact.Update_With_ViewPerson(person);
                db.Contacts.Add(contact);
                db.SaveChanges();

                this.Flash(string.Format("La fiche de {0} a été insérée", contact.DisplayName));
                return RedirectToAction("Details", new { id = contact.Contact_ID, slug = contact.Slug });
            }

            person.Companies = ListCompanies(person.Company_ID);
            return View(person);
        }

        //
        // GET: /People/Edit/5

        public ViewResult Edit(int id)
        {
            var contact = db.Contacts.Find(id);
            var person = contact.To_ViewPerson();

            person.Companies = ListCompanies(person.Company_ID);
            return View(person);
        }

        //
        // POST: /People/Edit/5

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(ViewPerson person)
        {
            if (ModelState.IsValid)
            {
                var contact = db.Contacts.Find(person.Contact_ID);
                contact.Update_With_ViewPerson(person);
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();

                this.Flash(string.Format("La fiche de {0} a été mise à jour", contact.DisplayName));
                return RedirectToAction("Details", new { id = contact.Contact_ID, slug = contact.Slug });
            }

            person.Companies = ListCompanies(person.Company_ID);
            return View(person);
        }

        //
        // GET: /People/Delete/5

        public ViewResult Delete(int id)
        {
            var contact = db.Contacts.Find(id);
            var person = contact.To_ViewPerson();

            return View(person);
        }

        //
        // POST: /People/Delete/5

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var contact = db.Contacts.Find(id);
            db.Contacts.Remove(contact);
            db.SaveChanges();

            this.Flash(string.Format("La fiche de {0} a été supprimée", contact.DisplayName));
            return RedirectToAction("Index", "Contacts");
        }

        protected SelectList ListCompanies(int? Company_ID)
        {
            return new SelectList(db.Contacts.Where(x => x.IsCompany == true).OrderBy(x => x.DisplayName), "Contact_ID", "DisplayName", Company_ID);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}