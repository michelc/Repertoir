using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Repertoir.Helpers;
using Repertoir.Models;

namespace Repertoir.Controllers
{
    public class PeopleController : Controller
    {
        private RepertoirContext db;

        public PeopleController() { db = new RepertoirContext(); }
        public PeopleController(RepertoirContext context) { db = context; }

        //
        // GET: /People/Details/5

        public ViewResult Details(int id)
        {
            var contact = db.Contacts.Find(id);
            var person = AutoMap.Map<ViewPerson>(contact);

            return View(person);
        }

        //
        // GET: /People/Display/5

        public ViewResult Display(int id)
        {
            var contact = db.Contacts.Find(id);
            var person = AutoMap.Map<ViewPerson>(contact);

            return View(person);
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
            var person = AutoMap.Map<ViewPerson>(contact);

            person.AvailableTags = ListTags(person.Tags_IDs);
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
                if (person.Tags_IDs != null)
                {
                    contact.Tags = (from t in db.Tags
                                    where person.Tags_IDs.Contains(t.Tag_ID)
                                    select t).ToList();
                }
                db.Contacts.Add(contact);
                db.SaveChanges();

                this.Flash("La fiche de {0} a été insérée", contact.DisplayName);
                return RedirectToAction("Details", new { id = contact.Contact_ID, slug = contact.Slug });
            }

            person.AvailableTags = ListTags(person.Tags_IDs);
            person.Companies = ListCompanies(person.Company_ID);
            return View(person);
        }

        //
        // GET: /People/Edit/5

        public ViewResult Edit(int id)
        {
            var contact = db.Contacts.Find(id);
            var person = AutoMap.Map<ViewPerson>(contact);

            person.AvailableTags = ListTags(person.Tags_IDs);
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
                if (contact.Tags != null) contact.Tags.Clear();
                if (person.Tags_IDs != null)
                {
                    contact.Tags = (from t in db.Tags
                                    where person.Tags_IDs.Contains(t.Tag_ID)
                                    select t).ToList();
                }
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();

                this.Flash("La fiche de {0} a été mise à jour", contact.DisplayName);
                return RedirectToAction("Details", new { id = contact.Contact_ID, slug = contact.Slug });
            }

            person.AvailableTags = ListTags(person.Tags_IDs);
            person.Companies = ListCompanies(person.Company_ID);
            return View(person);
        }

        //
        // GET: /People/Delete/5

        public ViewResult Delete(int id)
        {
            var contact = db.Contacts.Find(id);
            var person = AutoMap.Map<ViewPerson>(contact);

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

            this.Flash("La fiche de {0} a été supprimée", contact.DisplayName);
            return RedirectToAction("Index", "Contacts");
        }

        protected SelectList ListCompanies(int? Selected_ID)
        {
            return new SelectList(db.Contacts.Where(x => x.IsCompany == true).OrderBy(x => x.DisplayName), "Contact_ID", "DisplayName", Selected_ID);
        }

        protected MultiSelectList ListTags(ICollection<int> Selected_IDs)
        {
            return new MultiSelectList(db.Tags.OrderBy(x => x.Caption), "Tag_ID", "Caption", Selected_IDs);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}