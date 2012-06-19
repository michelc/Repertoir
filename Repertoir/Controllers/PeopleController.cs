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
            Contact person = db.Contacts.Find(id);

            return View(person);
        }

        //
        // GET: /People/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /People/Create

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(Contact person)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(person);
                db.SaveChanges();

                return RedirectToAction("Details", new { Id = person.ID });
            }

            return View(person);
        }

        //
        // GET: /People/Edit/5

        public ActionResult Edit(int id)
        {
            Contact person = db.Contacts.Find(id);

            return View(person);
        }

        //
        // POST: /People/Edit/5

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(Contact person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", new { Id = person.ID });
            }
            return View(person);
        }

        //
        // GET: /People/Delete/5

        public ActionResult Delete(int id)
        {
            Contact person = db.Contacts.Find(id);

            return View(person);
        }

        //
        // POST: /People/Delete/5

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact person = db.Contacts.Find(id);
            db.Contacts.Remove(person);
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