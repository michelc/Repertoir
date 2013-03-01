using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repertoir.Controllers;
using Repertoir.Helpers;
using Repertoir.Models;

namespace Repertoir.Tests.Controllers
{
    [TestClass]
    public class TagsControllerTest
    {
        private RepertoirContext db;

        public TagsControllerTest()
        {
            Database.SetInitializer<RepertoirContext>(new DropCreateDatabaseAlways<RepertoirContext>());
            db = new RepertoirContext();
        }

        [TestMethod]
        public void TagsIndex_doit_renvoyer_la_vue_par_defaut()
        {
            // Arrange
            var controller = new TagsController(db);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsNotNull(result, "Tags.Index() aurait dû renvoyer un ViewResult");
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName), "Tags.Index() aurait dû utiliser la vue par défaut");
        }

        [TestMethod]
        public void TagsIndex_renvoie_ICollection_de_ViewTag_a_la_vue()
        {
            // Arrange
            var controller = new TagsController(db);

            // Act
            var result = controller.Index();

            // Assert
            var model = result.ViewData.Model as ICollection<ViewTag>;
            Assert.IsNotNull(model, "Model devrait être du type ICollection<ViewTag>");
        }

        [TestMethod]
        public void TagsCreate_get_doit_renvoyer_la_vue_par_defaut()
        {
            // Arrange
            var controller = new TagsController(db);

            // Act
            var result = controller.Create();

            // Assert
            Assert.IsNotNull(result, "Tags.Create() aurait dû renvoyer un ViewResult");
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName), "Tags.Create() aurait dû utiliser la vue par défaut");
        }

        [TestMethod]
        public void TagsCreate_get_doit_renvoyer_un_objet_ViewTag_a_la_vue()
        {
            // Arrange
            var controller = new TagsController(db);

            // Act
            var result = controller.Create();

            // Assert
            var model = result.ViewData.Model as ViewTag;
            Assert.IsNotNull(model, "Tags.Create() aurait dû renvoyer un ViewTag");
        }

        [TestMethod]
        public void TagsCreate_post_doit_renvoyer_la_vue_par_defaut_quand_saisie_incorrecte()
        {
            // Arrange
            var controller = new TagsController();
            var tag = new ViewTag { Caption = "test" };
            controller.ModelState.AddModelError("global", "message");

            // Act
            var result = controller.Create(tag) as ViewResult;

            // Assert
            Assert.IsNotNull(result, "Tags.Create() aurait dû renvoyer un ViewResult");
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName), "Tags.Create() aurait dû utiliser la vue par défaut");
        }

        [TestMethod]
        public void TagsCreate_post_doit_renvoyer_le_meme_objet_ViewTag_quand_saisie_incorrecte()
        {
            // Arrange
            var controller = new TagsController(db);
            var tag = new ViewTag { Caption = "test" };
            controller.ModelState.AddModelError("global", "message");

            // Act
            var result = controller.Create(tag) as ViewResult;

            // Assert
            var model = result.ViewData.Model as ViewTag;
            Assert.IsNotNull(model, "Tags.Create() aurait dû renvoyer un ViewTag");
            Assert.AreEqual(tag.Caption, model.Caption, "Tags.Create() aurait dû renvoyer le tag saisi");
        }

        [TestMethod]
        public void TagsCreate_post_doit_enregistrer_tag_quand_saisie_correcte()
        {
            // Arrange
            var controller = new TagsController(db);
            var tag = new ViewTag { Caption = "test" };

            // Act
            var result = controller.Create(tag);

            // Assert
            var model = db.Tags.Where(x => x.Caption == tag.Caption).FirstOrDefault();
            Assert.IsNotNull(model, "Tags.Create() aurait dû enregistrer le tag");
        }

        [TestMethod]
        public void TagsCreate_post_doit_definir_message_de_succes_quand_saisie_correcte()
        {
            // Arrange
            var controller = new TagsController(db);
            var tag = new ViewTag { Caption = "test" };
            var context = new ViewContext
            {
                TempData = controller.TempData
            };
            var helper = new HtmlHelper(context, Repertoir.Tests.Helpers.Moq.GetViewDataContainer());

            // Act
            var result = controller.Create(tag);

            // Assert
            var flash = helper.Flash();
            var success = string.Format("Le tag [{0}] a été inséré", tag.Caption);
            Assert.IsNotNull(flash, "Tags.Create() aurait dû définir un message flash");
            Assert.IsTrue(flash.ToString().Contains(success), "Tags.Create() aurait dû initialiser le bon message");
        }

        [TestMethod]
        public void TagsCreate_post_doit_rediriger_vers_index_quand_saisie_correcte()
        {
            // Arrange
            var controller = new TagsController(db);
            var tag = new ViewTag { Caption = "test" };

            // Act
            var result = controller.Create(tag) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result, "Tags.Create() aurait dû renvoyer un RedirectToRouteResult");
            Assert.IsNull(result.RouteValues["controller"], "Tags.Create() aurait dû rediriger vers le contrôleur en cours");
            Assert.AreEqual("Index", result.RouteValues["action"], "Tags.Create() aurait dû rediriger vers l'action Index");
        }

        [TestMethod]
        public void TagsEdit_get_doit_renvoyer_la_vue_par_defaut()
        {
            // Arrange
            var controller = new TagsController(db);
            var tag = InsertTag("test");

            // Act
            var result = controller.Edit(tag.Tag_ID);

            // Assert
            Assert.IsNotNull(result, "Tag.Edit() aurait dû renvoyer un ViewResult");
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName), "Tag.Create() aurait dû utiliser la vue par défaut");
        }

        [TestMethod]
        public void TagsEdit_get_doit_renvoyer_un_objet_ViewTag_a_la_vue()
        {
            // Arrange
            var controller = new TagsController(db);
            var tag = InsertTag("test");

            // Act
            var result = controller.Edit(tag.Tag_ID);

            // Assert
            var model = result.ViewData.Model as ViewTag;
            Assert.IsNotNull(model, "Tag.Edit() aurait dû renvoyer un ViewTag");
        }

        [TestMethod]
        public void TagsEdit_get_doit_renvoyer_le_tag_demande_a_la_vue()
        {
            // Arrange
            var controller = new TagsController(db);
            var tag1 = InsertTag("test1");
            var tag2 = InsertTag("test2");

            // Act
            var result = controller.Edit(tag1.Tag_ID);

            // Assert
            var model = result.ViewData.Model as ViewTag;
            Assert.AreEqual(tag1.Caption, model.Caption, "Tag.Edit() aurait dû renvoyer le tag demandé");
        }

        [TestMethod]
        public void TagsEdit_post_doit_renvoyer_la_vue_par_defaut_quand_saisie_incorrecte()
        {
            // Arrange
            var controller = new TagsController();
            var tag = new ViewTag { Caption = "test" };
            controller.ModelState.AddModelError("global", "message");

            // Act
            var result = controller.Edit(tag) as ViewResult;

            // Assert
            Assert.IsNotNull(result, "Tag.Edit() aurait dû renvoyer un ViewResult");
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName), "Tag.Edit() aurait dû utiliser la vue par défaut");
        }

        [TestMethod]
        public void TagsEdit_post_doit_renvoyer_le_meme_objet_ViewTag_quand_saisie_incorrecte()
        {
            // Arrange
            var controller = new TagsController(db);
            var tag = new ViewTag { Caption = "test" };
            controller.ModelState.AddModelError("global", "message");

            // Act
            var result = controller.Edit(tag) as ViewResult;

            // Assert
            var model = result.ViewData.Model as ViewTag;
            Assert.IsNotNull(model, "Tag.Edit() aurait dû renvoyer un ViewTag");
            Assert.AreEqual(tag.Caption, model.Caption, "Tag.Edit() aurait dû renvoyer le tag saisi");
        }

        [TestMethod]
        public void TagsEdit_post_doit_enregistrer_modification_quand_saisie_correcte()
        {
            // Arrange
            var controller = new TagsController(db);
            var tag = InsertTag("test");
            var view_tag = new ViewTag
            {
                Tag_ID = tag.Tag_ID,
                Caption = "màj"
            };

            // Act
            var result = controller.Edit(view_tag);

            // Assert
            var model = db.Tags.Where(x => x.Caption == view_tag.Caption).FirstOrDefault();
            Assert.IsNotNull(model, "Tag.Edit() aurait dû mettre à jour le tag");
        }

        [TestMethod]
        public void TagsEdit_post_doit_definir_message_de_succes_quand_saisie_correcte()
        {
            // Arrange
            var controller = new TagsController(db);
            var tag = InsertTag("test");
            var context = new ViewContext
            {
                TempData = controller.TempData
            };
            var helper = new HtmlHelper(context, Repertoir.Tests.Helpers.Moq.GetViewDataContainer());

            // Act
            var view_tag = new ViewTag
            {
                Tag_ID = tag.Tag_ID,
                Caption = tag.Caption
            };
            var result = controller.Edit(view_tag);

            // Assert
            var flash = helper.Flash();
            var success = string.Format("Le tag [{0}] a été mis à jour", view_tag.Caption);
            Assert.IsNotNull(flash, "Tag.Edit() aurait dû définir un message flash");
            Assert.IsTrue(flash.ToString().Contains(success), "Tag.Edit() aurait dû initialiser le bon message");
        }

        [TestMethod]
        public void TagsEdit_post_doit_rediriger_vers_index_quand_saisie_correcte()
        {
            // Arrange
            var controller = new TagsController(db);
            var tag = InsertTag("test");

            // Act
            var view_tag = new ViewTag
            {
                Tag_ID = tag.Tag_ID,
                Caption = tag.Caption
            };
            var result = controller.Edit(view_tag) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result, "Tag.Edit() aurait dû renvoyer un RedirectToRouteResult");
            Assert.IsNull(result.RouteValues["controller"], "Tag.Edit() aurait dû rediriger vers le contrôleur en cours");
            Assert.AreEqual("Index", result.RouteValues["action"], "Tag.Edit() aurait dû rediriger vers l'action Details");
        }

        [TestMethod]
        public void TagsDelete_doit_renvoyer_la_vue_par_defaut()
        {
            // Arrange
            var controller = new TagsController(db);
            var tag = InsertTag("test");

            // Act
            var result = controller.Delete(tag.Tag_ID);

            // Assert
            Assert.IsNotNull(result, "Tags.Delete() aurait dû renvoyer un ViewResult");
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName), "Tags.Delete() aurait dû utiliser la vue par défaut");
        }

        [TestMethod]
        public void TagsDelete_doit_renvoyer_un_objet_ViewTag_a_la_vue()
        {
            // Arrange
            var controller = new TagsController(db);
            var tag = InsertTag("test");

            // Act
            var result = controller.Delete(tag.Tag_ID);

            // Assert
            var model = result.ViewData.Model as ViewTag;
            Assert.IsNotNull(model, "Tags.Delete() aurait dû renvoyer un ViewTag");
        }

        [TestMethod]
        public void TagsDelete_doit_renvoyer_le_tag_demande_a_la_vue()
        {
            // Arrange
            var controller = new TagsController(db);
            var tag1 = InsertTag("test1");
            var tag2 = InsertTag("test2");

            // Act
            var result = controller.Delete(tag1.Tag_ID);

            // Assert
            var model = result.ViewData.Model as ViewTag;
            Assert.AreEqual(tag1.Caption, model.Caption, "Tags.Delete() aurait dû renvoyer le tag demandé");
        }

        [TestMethod]
        public void TagsDeleteConfirmed_doit_supprimer_le_tag()
        {
            // Arrange
            var controller = new TagsController(db);
            var tag = InsertTag("test");

            // Act
            var result = controller.DeleteConfirmed(tag.Tag_ID);

            // Assert
            var model = db.Tags.Find(tag.Tag_ID);
            Assert.IsNull(model, "Tags.DeleteConfirmed() aurait dû supprimer le tag");
        }

        [TestMethod]
        public void TagsDeleteConfirmed_doit_definir_message_de_succes()
        {
            // Arrange
            var controller = new TagsController(db);
            var tag = InsertTag("test");
            var context = new ViewContext
            {
                TempData = controller.TempData
            };
            var helper = new HtmlHelper(context, Repertoir.Tests.Helpers.Moq.GetViewDataContainer());

            // Act
            var result = controller.DeleteConfirmed(tag.Tag_ID);

            // Assert
            var flash = helper.Flash();
            var success = string.Format("Le tag [{0}] a été supprimé", tag.Caption);
            Assert.IsNotNull(flash, "Tags.DeleteConfirmed() aurait dû définir un message flash");
            Assert.IsTrue(flash.ToString().Contains(success), "Tags.DeleteConfirmed() aurait dû initialiser le bon message");
        }

        [TestMethod]
        public void TagsDeleteConfirmed_doit_rediriger_vers_liste_des_tags()
        {
            // Arrange
            var controller = new TagsController(db);
            var tag = InsertTag("test");

            // Act
            var result = controller.DeleteConfirmed(tag.Tag_ID) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result, "Tags.DeleteConfirmed() aurait dû renvoyer un RedirectToRouteResult");
            Assert.IsNull(result.RouteValues["controller"], "Tag.DeleteConfirmed() aurait dû rediriger vers le contrôleur en cours");
            Assert.AreEqual("Index", result.RouteValues["action"], "Tags.DeleteConfirmed() aurait dû rediriger vers l'action Index");
        }

        private Tag InsertTag(string caption)
        {
            var tag = new Tag { Caption = caption };
            db.Tags.Add(tag);
            db.SaveChanges();

            return tag;
        }

    }
}
