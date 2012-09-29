using System.Data.Entity;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repertoir.Controllers;
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
        }

        [TestMethod]
        public void PeopleDetails_doit_renvoyer_la_vue_par_defaut()
        {
            // Arrange
            var controller = new PeopleController(db);
            var contact = new Contact
            {
                DisplayName = "test",
                Phone1 = "0"
            };
            db.Contacts.Add(contact);
            db.SaveChanges();

            // Act
            var result = controller.Details(contact.Contact_ID);

            // Assert
            Assert.IsNotNull(result, "People.Details() aurait dû renvoyer un ViewResult");
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName), "People.Details() aurait dû utiliser la vue par défaut");
        }

        [TestMethod]
        public void PeopleDetails_renvoie_un_objet_ViewPerson_la_vue()
        {
            // Arrange
            var controller = new PeopleController(db);
            var contact = new Contact
            {
                DisplayName = "test",
                Phone1 = "0"
            };
            db.Contacts.Add(contact);
            db.SaveChanges();

            // Act
            var result = controller.Details(contact.Contact_ID);

            // Assert
            var model = result.ViewData.Model as ViewPerson;
            Assert.IsNotNull(model, "Model devrait être du type ViewPerson");
        }

        [TestMethod]
        public void PeopleDetails_renvoie_le_contact_demande_a_la_vue()
        {
            // Arrange
            var controller = new PeopleController(db);
            var contact1 = new Contact { DisplayName = "test1", Phone1 = "1" };
            db.Contacts.Add(contact1);
            var contact2 = new Contact { DisplayName = "test2", Phone1 = "2" };
            db.Contacts.Add(contact2);
            db.SaveChanges();

            // Act
            var result = controller.Details(contact1.Contact_ID);

            // Assert
            var model = result.ViewData.Model as ViewPerson;
            Assert.AreEqual(contact1.DisplayName, model.DisplayName, "Model aurait dû correspondre au contact demandé");
            Assert.AreEqual(contact1.Phone1, model.Phone1, "Model aurait dû correspondre au contact demandé");
        }

        [TestMethod]
        public void PeopleDisplay_doit_renvoyer_la_vue_par_defaut()
        {
            // Arrange
            var controller = new PeopleController(db);
            var contact = new Contact
            {
                DisplayName = "test",
                Phone1 = "0"
            };
            db.Contacts.Add(contact);
            db.SaveChanges();

            // Act
            var result = controller.Display(contact.Contact_ID);

            // Assert
            Assert.IsNotNull(result, "People.Display() aurait dû renvoyer un ViewResult");
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName), "People.Display() aurait dû utiliser la vue par défaut");
        }

        [TestMethod]
        public void PeopleDisplay_renvoie_un_objet_ViewPerson_la_vue()
        {
            // Arrange
            var controller = new PeopleController(db);
            var contact = new Contact
            {
                DisplayName = "test",
                Phone1 = "0"
            };
            db.Contacts.Add(contact);
            db.SaveChanges();

            // Act
            var result = controller.Display(contact.Contact_ID);

            // Assert
            var model = result.ViewData.Model as ViewPerson;
            Assert.IsNotNull(model, "Model devrait être du type ViewPerson");
        }

        [TestMethod]
        public void PeopleDisplay_renvoie_le_contact_demande_a_la_vue()
        {
            // Arrange
            var controller = new PeopleController(db);
            var contact1 = new Contact { DisplayName = "test1", Phone1 = "1" };
            db.Contacts.Add(contact1);
            var contact2 = new Contact { DisplayName = "test2", Phone1 = "2" };
            db.Contacts.Add(contact2);
            db.SaveChanges();

            // Act
            var result = controller.Display(contact1.Contact_ID);

            // Assert
            var model = result.ViewData.Model as ViewPerson;
            Assert.AreEqual(contact1.DisplayName, model.DisplayName, "Model aurait dû correspondre au contact demandé");
            Assert.AreEqual(contact1.Phone1, model.Phone1, "Model aurait dû correspondre au contact demandé");
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
            Assert.IsNotNull(model, "Model devrait être du type ViewPerson");
        }

        [TestMethod]
        public void PeopleCreate_get_doit_initialiser_la_liste_des_societes()
        {
            // Arrange
            var controller = new PeopleController(db);

            // Act
            var result = controller.Create();

            // Assert
            var model = result.ViewData.Model as ViewPerson;
            Assert.IsNotNull(model.Companies, "Model.Companies devrait être initialisée");
        }

        [TestMethod]
        public void PeopleCreate_get_doit_initialiser_la_societe_parente_si_elle_est_renseignee()
        {
            // Arrange
            var controller = new PeopleController(db);
            var societe = new Contact
            {
                DisplayName = "soc",
                Phone1 = "9",
                IsCompany = true
            };
            db.Contacts.Add(societe);
            db.SaveChanges();

            // Act
            var result = controller.Create(societe.Contact_ID);

            // Assert
            var model = result.ViewData.Model as ViewPerson;
            Assert.AreEqual(societe.Contact_ID, model.Company_ID, "Model.Company_ID aurait dû correspondre à la société en paramètre");
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
        public void PeopleCreate_post_doit_renvoyer_les_erreurs_quand_saisie_incorrecte()
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
            Assert.IsTrue(result.ViewData.ModelState.Count > 0, "People.Create() aurait dû renvoyer des erreurs dans ModelState");
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
            Assert.IsNotNull(model.Companies, "Model.Companies devrait être initialisée");
        }

        [TestMethod]
        public void PeopleCreate_post_renvoie_le_meme_objet_ViewPerson_a_la_vue_quand_saisie_incorrecte()
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
            Assert.IsNotNull(model, "Model devrait être du type ViewPerson");
            Assert.AreEqual(person.LastName, model.LastName, "Model aurait dû correspondre à la saisie");
            Assert.AreEqual(person.Phone1, model.Phone1, "Model aurait dû correspondre à la saisie");
        }
    }
}
