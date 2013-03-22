using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
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
            var model = Mapper.Map<IList<ViewTag>>(tags);

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
                var tag = new Tag();
                tag.Caption = model.Caption;
                db.Tags.Add(tag);
                db.SaveChanges();

                this.Flash(string.Format("Le tag [{0}] a été inséré", tag.Caption));
                return RedirectToAction("Index");
            }

            return View(model);
        }

        //
        // GET: /Tags/Edit/5

        public ViewResult Edit(int id)
        {
            var tag = db.Tags.Find(id);
            var model = Mapper.Map<ViewTag>(tag);

            return View(model);
        }

        //
        // POST: /Tags/Edit/5

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(ViewTag model)
        {
            if (ModelState.IsValid)
            {
                var tag = db.Tags.Find(model.Tag_ID);
                tag.Caption = model.Caption;
                db.Entry(tag).State = EntityState.Modified;
                db.SaveChanges();

                this.Flash(string.Format("Le tag [{0}] a été mis à jour", tag.Caption));
                return RedirectToAction("Index");
            }

            return View(model);
        }

        //
        // GET: /Tags/Delete/5

        public ViewResult Delete(int id)
        {
            var tag = db.Tags.Find(id);
            var model = Mapper.Map<ViewTag>(tag);

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

            this.Flash(string.Format("Le tag [{0}] a été supprimé", tag.Caption));
            return RedirectToAction("Index");
        }
    }
}
