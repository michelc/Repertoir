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
            Assert.IsNotNull(model, "Model devrait être du type ViewPerson");
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
            Assert.AreEqual(contact1.DisplayName, model.DisplayName, "Model aurait dû correspondre au contact demandé");
            Assert.AreEqual(contact1.Phone1, model.Phone1, "Model aurait dû correspondre au contact demandé");
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
            Assert.IsNotNull(model, "Model devrait être du type ViewPerson");
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
            InsertCompany("soc", "9");

            // Act
            var result = controller.Create();

            // Assert
            var model = result.ViewData.Model as ViewPerson;
            Assert.IsNotNull(model.Companies, "Model.Companies devrait être initialisée");
            var count = model.Companies.Count();
            Assert.IsTrue(count > 0, "Model.Companies devrait contenir des sociétés");
            var check = model.Companies.Where(x => x.Text == "soc").Count();
            Assert.IsTrue(check > 0, "Model.Companies devrait contenir 'soc'");
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
            Assert.AreEqual(contact.Contact_ID, model.Company_ID, "Model.Company_ID aurait dû correspondre à la société en paramètre");
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
            Assert.IsNotNull(model.Companies, "Model.Companies devrait être initialisée");
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
            Assert.IsNotNull(model, "Model devrait être du type ViewPerson");
            Assert.AreEqual(person.LastName, model.LastName, "Model aurait dû correspondre à la saisie");
            Assert.AreEqual(person.Phone1, model.Phone1, "Model aurait dû correspondre à la saisie");
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
            Assert.IsNotNull(result, "People.Create() aurait dû renvoyer un ViewResult");
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
            Assert.IsNotNull(model, "Model devrait être du type ViewPerson");
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
            Assert.AreEqual(contact1.DisplayName, model.DisplayName, "Model aurait dû correspondre au contact demandé");
            Assert.AreEqual(contact1.Phone1, model.Phone1, "Model aurait dû correspondre au contact demandé");
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
            Assert.IsNotNull(model.Companies, "Model.Companies devrait être initialisée");
            var count = model.Companies.Count();
            Assert.IsTrue(count > 0, "Model.Companies devrait contenir des sociétés");
            var check = model.Companies.Where(x => x.Text == "soc").Count();
            Assert.IsTrue(check > 0, "Model.Companies devrait contenir 'soc'");
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
            Assert.IsNotNull(model.Companies, "Model.Companies devrait être initialisée");
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
            Assert.IsNotNull(model, "Model devrait être du type ViewPerson");
            Assert.AreEqual(person.LastName, model.LastName, "Model aurait dû correspondre à la saisie");
            Assert.AreEqual(person.Phone1, model.Phone1, "Model aurait dû correspondre à la saisie");
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
