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
    public class CompaniesControllerTest
    {
        private RepertoirContext db;

        public CompaniesControllerTest()
        {
            Database.SetInitializer<RepertoirContext>(new DropCreateDatabaseAlways<RepertoirContext>());
            db = new RepertoirContext();
            AutoMap.Configure();
        }

        [TestMethod]
        public void CompaniesDetails_doit_renvoyer_la_vue_par_defaut()
        {
            // Arrange
            var controller = new CompaniesController(db);
            var contact = InsertCompany("test", "9");

            // Act
            var result = controller.Details(contact.Contact_ID);

            // Assert
            Assert.IsNotNull(result, "Companies.Details() aurait dû renvoyer un ViewResult");
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName), "Companies.Details() aurait dû utiliser la vue par défaut");
        }

        [TestMethod]
        public void CompaniesDetails_doit_renvoyer_un_objet_ViewCompany_a_la_vue()
        {
            // Arrange
            var controller = new CompaniesController(db);
            var contact = InsertCompany("test", "9");

            // Act
            var result = controller.Details(contact.Contact_ID);

            // Assert
            var model = result.ViewData.Model as ViewCompany;
            Assert.IsNotNull(model, "Companies.Details() aurait dû renvoyer un ViewCompany");
        }

        [TestMethod]
        public void CompaniesDetails_doit_renvoyer_le_contact_demande_a_la_vue()
        {
            // Arrange
            var controller = new CompaniesController(db);
            var contact1 = InsertCompany("test1", "1");
            var contact2 = InsertCompany("test2", "2");

            // Act
            var result = controller.Details(contact1.Contact_ID);

            // Assert
            var model = result.ViewData.Model as ViewCompany;
            Assert.AreEqual(contact1.CompanyName, model.CompanyName, "Companies.Details() aurait dû renvoyer le contact demandé");
            Assert.AreEqual(contact1.Phone1, model.Phone1, "Companies.Details() aurait dû renvoyer le contact demandé");
        }

        [TestMethod]
        public void CompaniesDetails_get_doit_initialiser_la_liste_des_personnes()
        {
            // Arrange
            var controller = new CompaniesController(db);
            var contact = InsertCompany("test", "9");
            var person = InsertPerson("pers", "0");
            person.Company_ID = contact.Contact_ID;
            db.SaveChanges();

            // Act
            var result = controller.Details(contact.Contact_ID);

            // Assert
            var model = result.ViewData.Model as ViewCompany;
            Assert.IsNotNull(model.People, "CompaniesDetails() aurait dû initialiser Model.People");
            var count = model.People.Count();
            Assert.IsTrue(count > 0, "CompaniesDetails() aurait dû remplir Model.People");
            var check = model.People.Where(x => x.DisplayName == "pers").Count();
            Assert.IsTrue(check > 0, "CompaniesDetails() aurait dû remplir Model.People avec 'pers'");
        }

        [TestMethod]
        public void CompaniesCreate_get_doit_renvoyer_la_vue_par_defaut()
        {
            // Arrange
            var controller = new CompaniesController(db);

            // Act
            var result = controller.Create();

            // Assert
            Assert.IsNotNull(result, "Companies.Create() aurait dû renvoyer un ViewResult");
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName), "Companies.Create() aurait dû utiliser la vue par défaut");
        }

        [TestMethod]
        public void CompaniesCreate_get_doit_renvoyer_un_objet_ViewCompany_a_la_vue()
        {
            // Arrange
            var controller = new CompaniesController(db);

            // Act
            var result = controller.Create();

            // Assert
            var model = result.ViewData.Model as ViewCompany;
            Assert.IsNotNull(model, "Companies.Create() aurait dû renvoyer un ViewCompany");
        }

        [TestMethod]
        public void CompaniesCreate_post_doit_renvoyer_la_vue_par_defaut_quand_saisie_incorrecte()
        {
            // Arrange
            var controller = new CompaniesController();
            var company = new ViewCompany
            {
                CompanyName = "test",
                Phone1 = "9"
            };
            controller.ModelState.AddModelError("global", "message");

            // Act
            var result = controller.Create(company) as ViewResult;

            // Assert
            Assert.IsNotNull(result, "Companies.Create() aurait dû renvoyer un ViewResult");
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName), "Companies.Create() aurait dû utiliser la vue par défaut");
        }

        [TestMethod]
        public void CompaniesCreate_post_doit_renvoyer_le_meme_objet_ViewCompany_quand_saisie_incorrecte()
        {
            // Arrange
            var controller = new CompaniesController(db);
            var company = new ViewCompany
            {
                CompanyName = "test",
                Phone1 = "9"
            };
            controller.ModelState.AddModelError("global", "message");

            // Act
            var result = controller.Create(company) as ViewResult;

            // Assert
            var model = result.ViewData.Model as ViewCompany;
            Assert.IsNotNull(model, "Companies.Create() aurait dû renvoyer un ViewCompany");
            Assert.AreEqual(company.CompanyName, model.CompanyName, "Companies.Create() aurait dû renvoyer le contact saisi");
            Assert.AreEqual(company.Phone1, model.Phone1, "Companies.Create() aurait dû renvoyer le contact saisi");
        }

        [TestMethod]
        public void CompaniesCreate_post_doit_enregistrer_contact_quand_saisie_correcte()
        {
            // Arrange
            var controller = new CompaniesController(db);
            var company = new ViewCompany
            {
                CompanyName = "test",
                Phone1 = "9"
            };

            // Act
            var result = controller.Create(company);

            // Assert
            var contact = db.Contacts.Where(x => x.CompanyName == company.CompanyName).FirstOrDefault();
            Assert.IsNotNull(contact, "Companies.Create() aurait dû enregistrer le contact");
        }

        [TestMethod]
        public void CompaniesCreate_post_doit_definir_message_de_succes_quand_saisie_correcte()
        {
            // Arrange
            var controller = new CompaniesController(db);
            var company = new ViewCompany
            {
                CompanyName = "test",
                Phone1 = "9"
            };
            var context = new ViewContext
            {
                TempData = controller.TempData
            };
            var helper = new HtmlHelper(context, Repertoir.Tests.Helpers.Moq.GetViewDataContainer());

            // Act
            var result = controller.Create(company);

            // Assert
            var flash = helper.Flash();
            Assert.IsNotNull(flash, "Companies.Create() aurait dû définir un message flash");
            Assert.IsTrue(flash.ToString().Contains("La fiche de test a été insérée"), "Companies.Create() aurait dû initialiser le bon message");
        }

        [TestMethod]
        public void CompaniesCreate_post_doit_rediriger_vers_details_quand_saisie_correcte()
        {
            // Arrange
            var controller = new CompaniesController(db);
            var company = new ViewCompany
            {
                CompanyName = "test",
                Phone1 = "9"
            };

            // Act
            var result = controller.Create(company) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result, "Companies.Create() aurait dû renvoyer un RedirectToRouteResult");
            Assert.IsNull(result.RouteValues["controller"], "Companies.Create() aurait dû rediriger vers le contrôleur en cours");
            Assert.AreEqual("Details", result.RouteValues["action"], "Companies.Create() aurait dû rediriger vers l'action Details");
            Assert.IsNotNull(result.RouteValues["id"], "Companies.Create() aurait dû définir 'id'");
            Assert.IsNotNull(result.RouteValues["slug"], "Companies.Create() aurait dû définir 'slug'");
        }

        [TestMethod]
        public void CompaniesEdit_get_doit_renvoyer_la_vue_par_defaut()
        {
            // Arrange
            var controller = new CompaniesController(db);
            var contact = InsertCompany("test", "9");

            // Act
            var result = controller.Edit(contact.Contact_ID);

            // Assert
            Assert.IsNotNull(result, "Companies.Edit() aurait dû renvoyer un ViewResult");
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName), "Companies.Create() aurait dû utiliser la vue par défaut");
        }

        [TestMethod]
        public void CompaniesEdit_get_doit_renvoyer_un_objet_ViewCompany_a_la_vue()
        {
            // Arrange
            var controller = new CompaniesController(db);
            var contact = InsertCompany("test", "9");

            // Act
            var result = controller.Edit(contact.Contact_ID);

            // Assert
            var model = result.ViewData.Model as ViewCompany;
            Assert.IsNotNull(model, "Companies.Edit() aurait dû renvoyer un ViewCompany");
        }

        [TestMethod]
        public void CompaniesEdit_get_doit_renvoyer_le_contact_demande_a_la_vue()
        {
            // Arrange
            var controller = new CompaniesController(db);
            var contact1 = InsertCompany("test1", "1");
            var contact2 = InsertCompany("test2", "2");

            // Act
            var result = controller.Edit(contact1.Contact_ID);

            // Assert
            var model = result.ViewData.Model as ViewCompany;
            Assert.AreEqual(contact1.CompanyName, model.CompanyName, "Companies.Edit() aurait dû renvoyer le contact demandé");
            Assert.AreEqual(contact1.Phone1, model.Phone1, "Companies.Edit() aurait dû renvoyer le contact demandé");
        }

        [TestMethod]
        public void CompaniesEdit_post_doit_renvoyer_la_vue_par_defaut_quand_saisie_incorrecte()
        {
            // Arrange
            var controller = new CompaniesController();
            var company = new ViewCompany
            {
                CompanyName = "test",
                Phone1 = "9"
            };
            controller.ModelState.AddModelError("global", "message");

            // Act
            var result = controller.Edit(company) as ViewResult;

            // Assert
            Assert.IsNotNull(result, "Companies.Edit() aurait dû renvoyer un ViewResult");
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName), "Companies.Edit() aurait dû utiliser la vue par défaut");
        }

        [TestMethod]
        public void CompaniesEdit_post_doit_renvoyer_le_meme_objet_ViewCompany_quand_saisie_incorrecte()
        {
            // Arrange
            var controller = new CompaniesController(db);
            var company = new ViewCompany
            {
                CompanyName = "test",
                Phone1 = "9"
            };
            controller.ModelState.AddModelError("global", "message");

            // Act
            var result = controller.Edit(company) as ViewResult;

            // Assert
            var model = result.ViewData.Model as ViewCompany;
            Assert.IsNotNull(model, "Companies.Edit() aurait dû renvoyer un ViewCompany");
            Assert.AreEqual(company.CompanyName, model.CompanyName, "Companies.Edit() aurait dû renvoyer le contact saisi");
            Assert.AreEqual(company.Phone1, model.Phone1, "Companies.Edit() aurait dû renvoyer le contact saisi");
        }

        [TestMethod]
        public void CompaniesEdit_post_doit_enregistrer_modification_quand_saisie_correcte()
        {
            // Arrange
            var controller = new CompaniesController(db);
            var contact = InsertCompany("test", "9");
            contact.CompanyName = "maj";

            // Act
            var result = controller.Edit(contact.To_ViewCompany());

            // Assert
            var updated_contact = db.Contacts.Where(x => x.CompanyName == contact.CompanyName).FirstOrDefault();
            Assert.IsNotNull(updated_contact, "Companies.Edit() aurait dû mettre à jour le contact");
        }

        [TestMethod]
        public void CompaniesEdit_post_doit_definir_message_de_succes_quand_saisie_correcte()
        {
            // Arrange
            var controller = new CompaniesController(db);
            var contact = InsertCompany("test", "9");
            var context = new ViewContext
            {
                TempData = controller.TempData
            };
            var helper = new HtmlHelper(context, Repertoir.Tests.Helpers.Moq.GetViewDataContainer());

            // Act
            var result = controller.Edit(contact.To_ViewCompany());

            // Assert
            var flash = helper.Flash();
            Assert.IsNotNull(flash, "Companies.Edit() aurait dû définir un message flash");
            Assert.IsTrue(flash.ToString().Contains("La fiche de test a été mise à jour"), "Companies.Edit() aurait dû initialiser le bon message");
        }

        [TestMethod]
        public void CompaniesEdit_post_doit_rediriger_vers_details_quand_saisie_correcte()
        {
            // Arrange
            var controller = new CompaniesController(db);
            var contact = InsertCompany("test", "9");

            // Act
            var result = controller.Edit(contact.To_ViewCompany()) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result, "Companies.Edit() aurait dû renvoyer un RedirectToRouteResult");
            Assert.IsNull(result.RouteValues["controller"], "Companies.Edit() aurait dû rediriger vers le contrôleur en cours");
            Assert.AreEqual("Details", result.RouteValues["action"], "Companies.Edit() aurait dû rediriger vers l'action Details");
            Assert.IsNotNull(result.RouteValues["id"], "Companies.Edit() aurait dû définir 'id'");
            Assert.IsNotNull(result.RouteValues["slug"], "Companies.Edit() aurait dû définir 'slug'");
        }

        [TestMethod]
        public void CompaniesDelete_doit_renvoyer_la_vue_par_defaut()
        {
            // Arrange
            var controller = new CompaniesController(db);
            var person = InsertCompany("test", "9");

            // Act
            var result = controller.Delete(person.Contact_ID);

            // Assert
            Assert.IsNotNull(result, "Companies.Delete() aurait dû renvoyer un ViewResult");
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName), "Companies.Delete() aurait dû utiliser la vue par défaut");
        }

        [TestMethod]
        public void CompaniesDelete_doit_renvoyer_un_objet_ViewCompany_a_la_vue()
        {
            // Arrange
            var controller = new CompaniesController(db);
            var person = InsertCompany("test", "9");

            // Act
            var result = controller.Delete(person.Contact_ID);

            // Assert
            var model = result.ViewData.Model as ViewCompany;
            Assert.IsNotNull(model, "Companies.Delete() aurait dû renvoyer un ViewCompany");
        }

        [TestMethod]
        public void CompaniesDelete_doit_renvoyer_le_contact_demande_a_la_vue()
        {
            // Arrange
            var controller = new CompaniesController(db);
            var company1 = InsertCompany("test1", "1");
            var company2 = InsertCompany("test2", "2");

            // Act
            var result = controller.Delete(company1.Contact_ID);

            // Assert
            var model = result.ViewData.Model as ViewCompany;
            Assert.AreEqual(company1.CompanyName, model.CompanyName, "Companies.Delete() aurait dû renvoyer le contact demandé");
            Assert.AreEqual(company1.Phone1, model.Phone1, "Companies.Delete() aurait dû renvoyer le contact demandé");
        }

        [TestMethod]
        public void CompaniesDeleteConfirmed_doit_supprimer_le_contact()
        {
            // Arrange
            var controller = new CompaniesController(db);
            var company = InsertCompany("test", "9");

            // Act
            var result = controller.DeleteConfirmed(company.Contact_ID);

            // Assert
            var contact = db.Contacts.Find(company.Contact_ID);
            Assert.IsNull(contact, "Companies.DeleteConfirmed() aurait dû supprimer le contact");
        }

        [TestMethod]
        public void CompaniesDeleteConfirmed_doit_definir_message_de_succes()
        {
            // Arrange
            var controller = new CompaniesController(db);
            var company = InsertCompany("test", "9");
            var context = new ViewContext
            {
                TempData = controller.TempData
            };
            var helper = new HtmlHelper(context, Repertoir.Tests.Helpers.Moq.GetViewDataContainer());

            // Act
            var result = controller.DeleteConfirmed(company.Contact_ID);

            // Assert
            var flash = helper.Flash();
            Assert.IsNotNull(flash, "Companies.DeleteConfirmed() aurait dû définir un message flash");
            Assert.IsTrue(flash.ToString().Contains("La fiche de test a été supprimée"), "Companies.DeleteConfirmed() aurait dû initialiser le bon message");
        }

        [TestMethod]
        public void CompaniesDeleteConfirmed_doit_rediriger_vers_liste_des_contacts()
        {
            // Arrange
            var controller = new CompaniesController(db);
            var person = InsertCompany("test", "9");

            // Act
            var result = controller.DeleteConfirmed(person.Contact_ID) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result, "Companies.DeleteConfirmed() aurait dû renvoyer un RedirectToRouteResult");
            Assert.AreEqual("Contacts", result.RouteValues["controller"], "Companies.DeleteConfirmed() aurait dû rediriger vers le contrôleur Contacts");
            Assert.AreEqual("Index", result.RouteValues["action"], "Companies.DeleteConfirmed() aurait dû rediriger vers l'action Index");
        }

        private Contact InsertPerson(string name, string phone)
        {
            var person = new ViewPerson
            {
                LastName = name,
                Phone1 = phone
            };
            var contact = new Contact().Update_With_ViewPerson(person);
            db.Contacts.Add(contact);
            db.SaveChanges();

            return contact;
        }

        private Contact InsertCompany(string name, string phone)
        {
            var company = new ViewCompany
            {
                CompanyName = name,
                Phone1 = phone
            };
            var contact = new Contact().Update_With_ViewCompany(company);
            db.Contacts.Add(contact);
            db.SaveChanges();

            return contact;
        }
    }
}
