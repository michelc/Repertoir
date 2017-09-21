using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Repertoir.Helpers;
using Repertoir.Models;

namespace Repertoir.Controllers
{
    public class TagsController : Controller
    {
        private RepertoirContext db;

        public TagsController() { db = new RepertoirContext(); }
        public TagsController(RepertoirContext context) { db = context; }

        //
        // GET: /Tags/

        public ViewResult Index()
        {
            var tags = db.Tags.OrderBy(t => t.Caption);
            var model = AutoMap.Map<IList<ViewTag>>(tags);

            return View(model);
        }

        //
        // GET: /Tags/Create

        public ViewResult Create()
        {
            var model = new ViewTag();

            return View(model);
        }

        //
        // POST: /Tags/Create

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(ViewTag model)
        {
            if (ModelState.IsValid)
            {
                model.Caption = model.Caption.Unaccentize().SimplifySpaces().Replace(" ", "-");
                if (db.Tags.Any(t => t.Caption.ToLower() == model.Caption.ToLower())) ModelState.AddModelError("Caption", "Ce tag existe déjà");
            }

            if (ModelState.IsValid)
            {
                var tag = new Tag();
                tag.Caption = model.Caption;
                db.Tags.Add(tag);
                db.SaveChanges();

                this.Flash("Le tag [{0}] a été inséré", tag.Caption);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        //
        // GET: /Tags/Edit/5

        public ViewResult Edit(int id)
        {
            var tag = db.Tags.Find(id);
            var model = AutoMap.Map<ViewTag>(tag);

            return View(model);
        }

        //
        // POST: /Tags/Edit/5

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(ViewTag model)
        {
            if (ModelState.IsValid)
            {
                model.Caption = model.Caption.Unaccentize().SimplifySpaces().Replace(" ", "-");
                if (db.Tags.Any(t => (t.Caption.ToLower() == model.Caption.ToLower())
                                  && (t.Tag_ID != model.Tag_ID))) ModelState.AddModelError("Caption", "Ce tag existe déjà");
            }

            if (ModelState.IsValid)
            {
                var tag = db.Tags.Find(model.Tag_ID);
                tag.Caption = model.Caption;
                db.Entry(tag).State = EntityState.Modified;
                db.SaveChanges();

                this.Flash("Le tag [{0}] a été mis à jour", tag.Caption);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        //
        // GET: /Tags/Delete/5

        public ViewResult Delete(int id)
        {
            var tag = db.Tags.Find(id);
            var model = AutoMap.Map<ViewTag>(tag);

            return View(model);
        }

        //
        // POST: /Tags/Delete/5

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var tag = db.Tags.Find(id);
            db.Tags.Remove(tag);
            db.SaveChanges();

            this.Flash("Le tag [{0}] a été supprimé", tag.Caption);
            return RedirectToAction("Index");
        }

        //
        // GET: /Tags/Replace/5

        public ViewResult Replace(int id)
        {
            var tag = db.Tags.Find(id);
            var model = AutoMap.Map<ReplaceTag>(tag);

            model.Tags = ListTags(model.Tag_ID);
            return View(model);
        }

        //
        // POST: /Tags/Replace/5

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Replace(ReplaceTag model)
        {
            if (ModelState.IsValid)
            {
                var source = db.Tags.Find(model.Tag_ID);
                var destination = db.Tags.Find(model.Other_ID);

                // Recopie les contacts liés au tag source
                foreach (var c in source.Contacts)
                {
                    // Sur le tag destination
                    destination.Contacts.Add(c);
                }

                // Supprime le tag source
                db.Tags.Remove(source);

                db.SaveChanges();

                this.Flash("Le tag [{0}] a été remplacé par [{1}]", source.Caption, destination.Caption);
                return RedirectToAction("Index");
            }

            model.Tags = ListTags(model.Tag_ID);
            return View(model);
        }

        protected SelectList ListTags(int Tag_ID)
        {
            return new SelectList(db.Tags.Where(x => x.Tag_ID != Tag_ID).OrderBy(x => x.Caption), "Tag_ID", "Caption", "");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
