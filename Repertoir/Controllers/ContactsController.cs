using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
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
            return View(db.Contacts.ToList());
        }

        //
        // GET: /Contacts/Details/5

        public ViewResult Details(int id)
        {
            Contact contact = db.Contacts.Find(id);
            return View(contact);
        }

        //
        // GET: /Contacts/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Contacts/Create

        [HttpPost]
        public ActionResult Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(contact);
        }
        
        //
        // GET: /Contacts/Edit/5
 
        public ActionResult Edit(int id)
        {
            Contact contact = db.Contacts.Find(id);
            return View(contact);
        }

        //
        // POST: /Contacts/Edit/5

        [HttpPost]
        public ActionResult Edit(Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        //
        // GET: /Contacts/Delete/5
 
        public ActionResult Delete(int id)
        {
            Contact contact = db.Contacts.Find(id);
            return View(contact);
        }

        //
        // POST: /Contacts/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Contact contact = db.Contacts.Find(id);
            db.Contacts.Remove(contact);
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