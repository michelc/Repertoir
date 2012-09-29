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
    }
}
