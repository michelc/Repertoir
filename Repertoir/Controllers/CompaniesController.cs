using System.Data;
using System.Web.Mvc;
using Repertoir.Models;

namespace Repertoir.Controllers
{
    public class CompaniesController : Controller
    {
        private RepertoirContext db = new RepertoirContext();

        //
        // GET: /Companies/Details/5

        public ViewResult Details(int id)
        {
            Contact company = db.Contacts.Find(id);

            return View(company);
        }

        //
        // GET: /Companies/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Companies/Create

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(Contact company)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(company);
                db.SaveChanges();

                return RedirectToAction("Details", new { Id = company.ID });
            }

            return View(company);
        }

        //
        // GET: /Companies/Edit/5

        public ActionResult Edit(int id)
        {
            Contact company = db.Contacts.Find(id);

            return View(company);
        }

        //
        // POST: /Companies/Edit/5

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(Contact company)
        {
            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", new { Id = company.ID });
            }
            return View(company);
        }

        //
        // GET: /Companies/Delete/5

        public ActionResult Delete(int id)
        {
            Contact company = db.Contacts.Find(id);

            return View(company);
        }

        //
        // POST: /Companies/Delete/5

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact company = db.Contacts.Find(id);
            db.Contacts.Remove(company);
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