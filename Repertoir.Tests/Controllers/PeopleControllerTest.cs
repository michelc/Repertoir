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
    public class PeopleControllerTest
    {
        private RepertoirContext db;

        public PeopleControllerTest()
        {
            Database.SetInitializer<RepertoirContext>(new DropCreateDatabaseAlways<RepertoirContext>());
            db = new RepertoirContext();
            AutoMapperConfiguration.Configure();
        }

        [TestMethod]
        public void PeopleDetails_doit_renvoyer_la_vue_par_defaut()
        {
            // Arrange
            var controller = new PeopleController(db);
            var contact = InsertPerson("test", "0");

            // Act
            var result = controller.Details(contact.Contact_ID);

            // Assert
            Assert.IsNotNull(result, "People.Details() aurait dû renvoyer un ViewResult");
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName), "People.Details() aurait dû utiliser la vue par défaut");
        }

        [TestMethod]
        public void PeopleDetails_doit_renvoyer_un_objet_ViewPerson_a_la_vue()
        {
            // Arrange
            var controller = new PeopleController(db);
            var contact = InsertPerson("test", "0");

            // Act
            var result = controller.Details(contact.Contact_ID);

            // Assert
            var model = result.ViewData.Model as ViewPerson;
            Assert.IsNotNull(model, "People.Details() aurait dû renvoyer un ViewPerson");
        }

        [TestMethod]
        public void PeopleDetails_doit_renvoyer_le_contact_demande_a_la_vue()
        {
            // Arrange
            var controller = new PeopleController(db);
            var contact1 = InsertPerson("test1", "1");
            var contact2 = InsertPerson("test2", "2");

            // Act
            var result = controller.Details(contact1.Contact_ID);

            // Assert
            var model = result.ViewData.Model as ViewPerson;
            Assert.AreEqual(contact1.FirstName, model.FirstName, "People.Details() aurait dû renvoyer le contact demandé");
            Assert.AreEqual(contact1.Phone1, model.Phone1, "People.Details() aurait dû renvoyer le contact demandé");
        }

        [TestMethod]
        public void PeopleDisplay_doit_renvoyer_la_vue_par_defaut()
        {
            // Arrange
            var controller = new PeopleController(db);
            var contact = InsertPerson("test", "0");

            // Act
            var result = controller.Display(contact.Contact_ID);

            // Assert
            Assert.IsNotNull(result, "People.Display() aurait dû renvoyer un ViewResult");
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName), "People.Display() aurait dû utiliser la vue par défaut");
        }

        [TestMethod]
        public void PeopleDisplay_doit_renvoyer_un_objet_ViewPerson_a_la_vue()
        {
            // Arrange
            var controller = new PeopleController(db);
            var contact = InsertPerson("test", "0");

            // Act
            var result = controller.Display(contact.Contact_ID);

            // Assert
            var model = result.ViewData.Model as ViewPerson;
            Assert.IsNotNull(model, "People.Display() aurait dû renvoyer un ViewPerson");
        }

        [TestMethod]
        public void PeopleDisplay_doit_renvoyer_le_contact_demande_a_la_vue()
        {
            // Arrange
            var controller = new PeopleController(db);
            var contact1 = InsertPerson("test1", "1");
            var contact2 = InsertPerson("test2", "2");

            // Act
            var result = controller.Display(contact1.Contact_ID);

            // Assert
            var model = result.ViewData.Model as ViewPerson;
            Assert.AreEqual(contact1.FirstName, model.FirstName, "People.Display() aurait dû renvoyer le contact demandé");
            Assert.AreEqual(contact1.Phone1, model.Phone1, "People.Display() aurait dû renvoyer le contact demandé");
        }

        [TestMethod]
        public void PeopleCreate_get_doit_renvoyer_la_vue_par_defaut()
        {
            // Arrange
            var controller = new PeopleController(db);

            // Act
            var result = controller.Create();

            // Assert
            Assert.IsNotNull(result, "People.Create() aurait dû renvoyer un ViewResult");
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName), "People.Create() aurait dû utiliser la vue par défaut");
        }

        [TestMethod]
        public void PeopleCreate_get_doit_renvoyer_un_objet_ViewPerson_a_la_vue()
        {
            // Arrange
            var controller = new PeopleController(db);

            // Act
            var result = controller.Create();

            // Assert
            var model = result.ViewData.Model as ViewPerson;
            Assert.IsNotNull(model, "People.Create() aurait dû renvoyer un ViewPerson");
        }

        [TestMethod]
        public void PeopleCreate_get_doit_initialiser_la_liste_des_societes()
        {
            // Arrange
            var controller = new PeopleController(db);
            InsertCompany("soc", "9");

            // Act
            var result = controller.Create();

            // Assert
            var model = result.ViewData.Model as ViewPerson;
            Assert.IsNotNull(model.Companies, "People.Create() aurait dû initialiser Model.Companies");
            var count = model.Companies.Count();
            Assert.IsTrue(count > 0, "People.Create() aurait dû remplir Model.Companies");
            var check = model.Companies.Where(x => x.Text == "soc").Count();
            Assert.IsTrue(check > 0, "People.Create() aurait dû remplir Model.Companies avec 'soc'");
        }

        [TestMethod]
        public void PeopleCreate_get_doit_initialiser_la_societe_parente_si_elle_est_renseignee()
        {
            // Arrange
            var controller = new PeopleController(db);
            var contact = InsertCompany("soc", "9");

            // Act
            var result = controller.Create(contact.Contact_ID);

            // Assert
            var model = result.ViewData.Model as ViewPerson;
            Assert.AreEqual(contact.Contact_ID, model.Company_ID, "People.Create() aurait dû initialiser Model.Company_ID avec société en paramètre");
        }

        [TestMethod]
        public void PeopleCreate_post_doit_renvoyer_la_vue_par_defaut_quand_saisie_incorrecte()
        {
            // Arrange
            var controller = new PeopleController();
            var person = new ViewPerson
            {
                LastName = "test",
                Phone1 = "0"
            };
            controller.ModelState.AddModelError("global", "message");

            // Act
            var result = controller.Create(person) as ViewResult;

            // Assert
            Assert.IsNotNull(result, "People.Create() aurait dû renvoyer un ViewResult");
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName), "People.Create() aurait dû utiliser la vue par défaut");
        }

        [TestMethod]
        public void PeopleCreate_post_doit_initialiser_la_liste_des_societes_quand_saisie_incorrecte()
        {
            // Arrange
            var controller = new PeopleController(db);
            var person = new ViewPerson
            {
                LastName = "test",
                Phone1 = "0"
            };
            controller.ModelState.AddModelError("global", "message");

            // Act
            var result = controller.Create(person) as ViewResult;

            // Assert
            var model = result.ViewData.Model as ViewPerson;
            Assert.IsNotNull(model.Companies, "People.Create() aurait dû initialiser Model.Companies");
        }

        [TestMethod]
        public void PeopleCreate_post_doit_renvoyer_le_meme_objet_ViewPerson_quand_saisie_incorrecte()
        {
            // Arrange
            var controller = new PeopleController(db);
            var person = new ViewPerson
            {
                LastName = "test",
                Phone1 = "0"
            };
            controller.ModelState.AddModelError("global", "message");

            // Act
            var result = controller.Create(person) as ViewResult;

            // Assert
            var model = result.ViewData.Model as ViewPerson;
            Assert.IsNotNull(model, "People.Create() aurait dû renvoyer un ViewPerson");
            Assert.AreEqual(person.LastName, model.LastName, "People.Create() aurait dû renvoyer le contact saisi");
            Assert.AreEqual(person.Phone1, model.Phone1, "People.Create() aurait dû renvoyer le contact saisi");
        }

        [TestMethod]
        public void PeopleCreate_post_doit_enregistrer_contact_quand_saisie_correcte()
        {
            // Arrange
            var controller = new PeopleController(db);
            var person = new ViewPerson
            {
                LastName = "test" + System.DateTime.Now.Ticks.ToString(),
                Phone1 = "0"
            };

            // Act
            var result = controller.Create(person);

            // Assert
            var contact = db.Contacts.Where(x => x.LastName == person.LastName).FirstOrDefault();
            Assert.IsNotNull(contact, "People.Create() aurait dû enregistrer le contact");
        }

        [TestMethod]
        public void PeopleCreate_post_doit_definir_message_de_succes_quand_saisie_correcte()
        {
            // Arrange
            var controller = new PeopleController(db);
            var person = new ViewPerson
            {
                LastName = "test",
                Phone1 = "0"
            };
            var context = new ViewContext
            {
                TempData = controller.TempData
            };
            var helper = new HtmlHelper(context, Repertoir.Tests.Helpers.Moq.GetViewDataContainer());

            // Act
            var result = controller.Create(person);

            // Assert
            var flash = helper.Flash();
            Assert.IsNotNull(flash, "People.Create() aurait dû définir un message flash");
            Assert.IsTrue(flash.ToString().Contains("La fiche de test a été insérée"), "People.Create() aurait dû initialiser le bon message");
        }

        [TestMethod]
        public void PeopleCreate_post_doit_rediriger_vers_details_quand_saisie_correcte()
        {
            // Arrange
            var controller = new PeopleController(db);
            var person = new ViewPerson
            {
                LastName = "test",
                Phone1 = "0"
            };

            // Act
            var result = controller.Create(person) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result, "People.Create() aurait dû renvoyer un RedirectToRouteResult");
            Assert.IsNull(result.RouteValues["controller"], "People.Create() aurait dû rediriger vers le contrôleur en cours");
            Assert.AreEqual("Details", result.RouteValues["action"], "People.Create() aurait dû rediriger vers l'action Details");
            Assert.IsNotNull(result.RouteValues["id"], "People.Create() aurait dû définir 'id'");
            Assert.IsNotNull(result.RouteValues["slug"], "People.Create() aurait dû définir 'slug'");
        }

        [TestMethod]
        public void PeopleEdit_get_doit_renvoyer_la_vue_par_defaut()
        {
            // Arrange
            var controller = new PeopleController(db);
            var contact = InsertPerson("test", "0");

            // Act
            var result = controller.Edit(contact.Contact_ID);

            // Assert
            Assert.IsNotNull(result, "People.Edit() aurait dû renvoyer un ViewResult");
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName), "People.Create() aurait dû utiliser la vue par défaut");
        }

        [TestMethod]
        public void PeopleEdit_get_doit_renvoyer_un_objet_ViewPerson_a_la_vue()
        {
            // Arrange
            var controller = new PeopleController(db);
            var contact = InsertPerson("test", "0");

            // Act
            var result = controller.Edit(contact.Contact_ID);

            // Assert
            var model = result.ViewData.Model as ViewPerson;
            Assert.IsNotNull(model, "People.Edit() aurait dû renvoyer un ViewPerson");
        }

        [TestMethod]
        public void PeopleEdit_get_doit_renvoyer_le_contact_demande_a_la_vue()
        {
            // Arrange
            var controller = new PeopleController(db);
            var contact1 = InsertPerson("test1", "1");
            var contact2 = InsertPerson("test2", "2");

            // Act
            var result = controller.Edit(contact1.Contact_ID);

            // Assert
            var model = result.ViewData.Model as ViewPerson;
            Assert.AreEqual(contact1.FirstName, model.FirstName, "People.Edit() aurait dû renvoyer le contact demandé");
            Assert.AreEqual(contact1.Phone1, model.Phone1, "People.Edit() aurait dû renvoyer le contact demandé");
        }

        [TestMethod]
        public void PeopleEdit_get_doit_initialiser_la_liste_des_societes()
        {
            // Arrange
            var controller = new PeopleController(db);
            InsertCompany("soc", "9");
            var contact = InsertPerson("test", "0");

            // Act
            var result = controller.Edit(contact.Contact_ID);

            // Assert
            var model = result.ViewData.Model as ViewPerson;
            Assert.IsNotNull(model.Companies, "People.Edit() aurait dû initialiser Model.Companies");
            var count = model.Companies.Count();
            Assert.IsTrue(count > 0, "People.Edit() aurait dû remplir Model.Companies");
            var check = model.Companies.Where(x => x.Text == "soc").Count();
            Assert.IsTrue(check > 0, "People.Edit() aurait dû remplir Model.Companies avec 'soc'");
        }

        [TestMethod]
        public void PeopleEdit_post_doit_renvoyer_la_vue_par_defaut_quand_saisie_incorrecte()
        {
            // Arrange
            var controller = new PeopleController();
            var person = new ViewPerson
            {
                LastName = "test",
                Phone1 = "0"
            };
            controller.ModelState.AddModelError("global", "message");

            // Act
            var result = controller.Edit(person) as ViewResult;

            // Assert
            Assert.IsNotNull(result, "People.Edit() aurait dû renvoyer un ViewResult");
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName), "People.Edit() aurait dû utiliser la vue par défaut");
        }

        [TestMethod]
        public void PeopleEdit_post_doit_initialiser_la_liste_des_societes_quand_saisie_incorrecte()
        {
            // Arrange
            var controller = new PeopleController(db);
            var person = new ViewPerson
            {
                LastName = "test",
                Phone1 = "0"
            };
            controller.ModelState.AddModelError("global", "message");

            // Act
            var result = controller.Edit(person) as ViewResult;

            // Assert
            var model = result.ViewData.Model as ViewPerson;
            Assert.IsNotNull(model.Companies, "People.Edit() aurait dû initialiser Model.Companies");
        }

        [TestMethod]
        public void PeopleEdit_post_doit_renvoyer_le_meme_objet_ViewPerson_quand_saisie_incorrecte()
        {
            // Arrange
            var controller = new PeopleController(db);
            var person = new ViewPerson
            {
                LastName = "test",
                Phone1 = "0"
            };
            controller.ModelState.AddModelError("global", "message");

            // Act
            var result = controller.Edit(person) as ViewResult;

            // Assert
            var model = result.ViewData.Model as ViewPerson;
            Assert.IsNotNull(model, "People.Edit() aurait dû renvoyer un ViewPerson");
            Assert.AreEqual(person.LastName, model.LastName, "People.Edit() aurait dû renvoyer le contact saisi");
            Assert.AreEqual(person.Phone1, model.Phone1, "People.Edit() aurait dû renvoyer le contact saisi");
        }

        [TestMethod]
        public void PeopleEdit_post_doit_enregistrer_modification_quand_saisie_correcte()
        {
            // Arrange
            var controller = new PeopleController(db);
            var contact = InsertPerson("test", "0");
            contact.LastName = "maj";

            // Act
            var result = controller.Edit(contact.To_ViewPerson());

            // Assert
            var updated_contact = db.Contacts.Where(x => x.LastName == contact.LastName).FirstOrDefault();
            Assert.IsNotNull(updated_contact, "People.Edit() aurait dû mettre à jour le contact");
        }

        [TestMethod]
        public void PeopleEdit_post_doit_definir_message_de_succes_quand_saisie_correcte()
        {
            // Arrange
            var controller = new PeopleController(db);
            var contact = InsertPerson("test", "0");
            var context = new ViewContext
            {
                TempData = controller.TempData
            };
            var helper = new HtmlHelper(context, Repertoir.Tests.Helpers.Moq.GetViewDataContainer());

            // Act
            var result = controller.Edit(contact.To_ViewPerson());

            // Assert
            var flash = helper.Flash();
            Assert.IsNotNull(flash, "People.Edit() aurait dû définir un message flash");
            Assert.IsTrue(flash.ToString().Contains("La fiche de test a été mise à jour"), "People.Edit() aurait dû initialiser le bon message");
        }

        [TestMethod]
        public void PeopleEdit_post_doit_rediriger_vers_details_quand_saisie_correcte()
        {
            // Arrange
            var controller = new PeopleController(db);
            var contact = InsertPerson("test", "0");

            // Act
            var result = controller.Edit(contact.To_ViewPerson()) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result, "People.Edit() aurait dû renvoyer un RedirectToRouteResult");
            Assert.IsNull(result.RouteValues["controller"], "People.Edit() aurait dû rediriger vers le contrôleur en cours");
            Assert.AreEqual("Details", result.RouteValues["action"], "People.Edit() aurait dû rediriger vers l'action Details");
            Assert.IsNotNull(result.RouteValues["id"], "People.Edit() aurait dû définir 'id'");
            Assert.IsNotNull(result.RouteValues["slug"], "People.Edit() aurait dû définir 'slug'");
        }

        [TestMethod]
        public void PeopleDelete_doit_renvoyer_la_vue_par_defaut()
        {
            // Arrange
            var controller = new PeopleController(db);
            var person = InsertPerson("test", "0");

            // Act
            var result = controller.Delete(person.Contact_ID);

            // Assert
            Assert.IsNotNull(result, "People.Delete() aurait dû renvoyer un ViewResult");
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName), "People.Delete() aurait dû utiliser la vue par défaut");
        }

        [TestMethod]
        public void PeopleDelete_doit_renvoyer_un_objet_ViewPerson_a_la_vue()
        {
            // Arrange
            var controller = new PeopleController(db);
            var person = InsertPerson("test", "0");

            // Act
            var result = controller.Delete(person.Contact_ID);

            // Assert
            var model = result.ViewData.Model as ViewPerson;
            Assert.IsNotNull(model, "People.Delete() aurait dû renvoyer un ViewPerson");
        }

        [TestMethod]
        public void PeopleDelete_doit_renvoyer_le_contact_demande_a_la_vue()
        {
            // Arrange
            var controller = new PeopleController(db);
            var person1 = InsertPerson("test1", "1");
            var person2 = InsertPerson("test2", "2");

            // Act
            var result = controller.Delete(person1.Contact_ID);

            // Assert
            var model = result.ViewData.Model as ViewPerson;
            Assert.AreEqual(person1.FirstName, model.FirstName, "People.Delete() aurait dû renvoyer le contact demandé");
            Assert.AreEqual(person1.Phone1, model.Phone1, "People.Delete() aurait dû renvoyer le contact demandé");
        }

        [TestMethod]
        public void PeopleDeleteConfirmed_doit_supprimer_le_contact()
        {
            // Arrange
            var controller = new PeopleController(db);
            var person = InsertPerson("test", "0");

            // Act
            var result = controller.DeleteConfirmed(person.Contact_ID);

            // Assert
            var contact = db.Contacts.Find(person.Contact_ID);
            Assert.IsNull(contact, "People.DeleteConfirmed() aurait dû supprimer le contact");
        }

        [TestMethod]
        public void PeopleDeleteConfirmed_doit_definir_message_de_succes()
        {
            // Arrange
            var controller = new PeopleController(db);
            var person = InsertPerson("test", "0");
            var context = new ViewContext
            {
                TempData = controller.TempData
            };
            var helper = new HtmlHelper(context, Repertoir.Tests.Helpers.Moq.GetViewDataContainer());

            // Act
            var result = controller.DeleteConfirmed(person.Contact_ID);

            // Assert
            var flash = helper.Flash();
            Assert.IsNotNull(flash, "People.DeleteConfirmed() aurait dû définir un message flash");
            Assert.IsTrue(flash.ToString().Contains("La fiche de test a été supprimée"), "People.DeleteConfirmed() aurait dû initialiser le bon message");
        }

        [TestMethod]
        public void PeopleDeleteConfirmed_doit_rediriger_vers_liste_des_contacts()
        {
            // Arrange
            var controller = new PeopleController(db);
            var person = InsertPerson("test", "0");

            // Act
            var result = controller.DeleteConfirmed(person.Contact_ID) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result, "People.DeleteConfirmed() aurait dû renvoyer un RedirectToRouteResult");
            Assert.AreEqual("Contacts", result.RouteValues["controller"], "People.DeleteConfirmed() aurait dû rediriger vers le contrôleur Contacts");
            Assert.AreEqual("Index", result.RouteValues["action"], "People.DeleteConfirmed() aurait dû rediriger vers l'action Index");
        }

        private Contact InsertPerson (string name, string phone)
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

        private Contact InsertCompany (string name, string phone)
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
