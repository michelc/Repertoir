using System.Data;
using System.Linq;
using System.Web.Mvc;
using Repertoir.Models; 

namespace Repertoir.Controllers
{
    public class PeopleController : Controller
    {
        private RepertoirContext db = new RepertoirContext();

        //
        // GET: /People/Details/5

        public ViewResult Details(int id)
        {
            var contact = db.Contacts.Find(id);
            var person = contact.To_ViewPerson();

            return View(person);
        }

        //
        // GET: /People/Create

        public ActionResult Create(int ParentID = 0)
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

                return RedirectToAction("Details", new { Id = contact.Contact_ID });
            }

            person.Companies = ListCompanies(person.Company_ID);
            return View(person);
        }

        //
        // GET: /People/Edit/5

        public ActionResult Edit(int id)
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

                return RedirectToAction("Details", new { Id = contact.Contact_ID });
            }

            person.Companies = ListCompanies(person.Company_ID);
            return View(person);
        }

        //
        // GET: /People/Delete/5

        public ActionResult Delete(int id)
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