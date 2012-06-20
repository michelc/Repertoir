using System.Data;
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

        public ActionResult Create()
        {
            var contact = new Contact();
            var person = contact.To_ViewPerson();

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

                return RedirectToAction("Details", new { Id = contact.ID });
            }

            return View(person);
        }

        //
        // GET: /People/Edit/5

        public ActionResult Edit(int id)
        {
            var contact = db.Contacts.Find(id);
            var person = contact.To_ViewPerson();

            return View(person);
        }

        //
        // POST: /People/Edit/5

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(ViewPerson person)
        {
            if (ModelState.IsValid)
            {
                var contact = db.Contacts.Find(person.ID);
                contact.Update_With_ViewPerson(person);
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", new { Id = contact.ID });
            }

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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}