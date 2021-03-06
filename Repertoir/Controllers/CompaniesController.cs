﻿using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Repertoir.Helpers;
using Repertoir.Models;

namespace Repertoir.Controllers
{
    public class CompaniesController : Controller
    {
        private RepertoirContext db;

        public CompaniesController() { db = new RepertoirContext(); }
        public CompaniesController(RepertoirContext context) { db = context; }

        //
        // GET: /Companies/Details/5

        public ViewResult Details(int id)
        {
            var contact = db.Contacts.Find(id);
            var company = AutoMap.Map<ViewCompany>(contact);

            company.People = db.Contacts.List(id);
            return View(company);
        }

        //
        // GET: /Companies/Display/5

        public ViewResult Display(int id)
        {
            var contact = db.Contacts.Find(id);
            var company = AutoMap.Map<ViewCompany>(contact);

            company.People = db.Contacts.List(id);
            return View("Details", company);
        }

        //
        // GET: /Companies/Create

        public ViewResult Create()
        {
            var contact = new Contact();
            var company = AutoMap.Map<ViewCompany>(contact);

            company.AvailableTags = ListTags(company.Tags_IDs);
            return View(company);
        }

        //
        // POST: /Companies/Create

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(ViewCompany company)
        {
            if (ModelState.IsValid)
            {
                var contact = new Contact();
                contact.Update_With_ViewCompany(company);
                if (company.Tags_IDs != null)
                {
                    contact.Tags = (from t in db.Tags
                                    where company.Tags_IDs.Contains(t.Tag_ID)
                                    select t).ToList();
                }
                db.Contacts.Add(contact);
                db.SaveChanges();

                this.Flash("La fiche de {0} a été insérée", contact.DisplayName);
                return RedirectToAction("Details", new { id = contact.Contact_ID, slug = contact.Slug });
            }

            company.AvailableTags = ListTags(company.Tags_IDs);
            return View(company);
        }

        //
        // GET: /Companies/Edit/5

        public ViewResult Edit(int id)
        {
            var contact = db.Contacts.Find(id);
            var company = AutoMap.Map<ViewCompany>(contact);

            company.AvailableTags = ListTags(company.Tags_IDs);
            return View(company);
        }

        //
        // POST: /Companies/Edit/5

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(ViewCompany company)
        {
            if (ModelState.IsValid)
            {
                var contact = db.Contacts.Find(company.Contact_ID);
                contact.Update_With_ViewCompany(company);
                if (contact.Tags != null) contact.Tags.Clear();
                if (company.Tags_IDs != null)
                {
                    contact.Tags = (from t in db.Tags
                                    where company.Tags_IDs.Contains(t.Tag_ID)
                                    select t).ToList();
                }
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();

                this.Flash("La fiche de {0} a été mise à jour", contact.DisplayName);
                return RedirectToAction("Details", new { id = contact.Contact_ID, slug = contact.Slug });
            }

            company.AvailableTags = ListTags(company.Tags_IDs);
            return View(company);
        }

        //
        // GET: /Companies/Delete/5

        public ViewResult Delete(int id)
        {
            var contact = db.Contacts.Find(id);
            var company = AutoMap.Map<ViewCompany>(contact);

            return View(company);
        }

        //
        // POST: /Companies/Delete/5

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var contact = db.Contacts.Find(id);
            db.Contacts.Remove(contact);
            db.SaveChanges();

            this.Flash("La fiche de {0} a été supprimée", contact.DisplayName);
            return RedirectToAction("Index", "Contacts");
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