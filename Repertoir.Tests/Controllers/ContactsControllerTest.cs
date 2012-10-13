using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repertoir.Controllers;
using Repertoir.Models;

namespace Repertoir.Tests.Controllers
{
    [TestClass]
    public class ContactsControllerTest
    {
        private RepertoirContext db;

        public ContactsControllerTest()
        {
            Database.SetInitializer<RepertoirContext>(new DropCreateDatabaseAlways<RepertoirContext>());
            db = new RepertoirContext();
        }

        [TestMethod]
        public void ContactsIndex_doit_renvoyer_la_vue_par_defaut()
        {
            // Arrange
            var controller = new ContactsController(db);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsNotNull(result, "Contacts.Index() aurait dû renvoyer un ViewResult");
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName), "Contacts.Index() aurait dû utiliser la vue par défaut");
        }

        [TestMethod]
        public void ContactsIndex_renvoie_ICollection_de_ContactList_a_la_vue()
        {
            // Arrange
            var controller = new ContactsController(db);

            // Act
            var result = controller.Index();

            // Assert
            var model = result.ViewData.Model as ICollection<ContactList>;
            Assert.IsNotNull(model, "Model devrait être du type ICollection<ContactList>");
        }

        [TestMethod]
        public void ContactsTable_doit_renvoyer_la_vue_par_defaut()
        {
            // Arrange
            var controller = new ContactsController(db);

            // Act
            var result = controller.Table();

            // Assert
            Assert.IsNotNull(result, "Contacts.Table() aurait dû renvoyer un ViewResult");
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName), "Contacts.Table() aurait dû utiliser la vue par défaut");
        }

        [TestMethod]
        public void ContactsTable_renvoie_ICollection_de_ContactList_a_la_vue()
        {
            // Arrange
            var controller = new ContactsController(db);

            // Act
            var result = controller.Table();

            // Assert
            var model = result.ViewData.Model as ICollection<ContactList>;
            Assert.IsNotNull(model, "Model devrait être du type ICollection<ContactList>");
        }

        [TestMethod]
        public void ContactsNext_doit_rediriger_vers_une_action_detail()
        {
            // Arrange
            var controller = new ContactsController(db);
            var contact = InsertPerson("test", "0");

            // Act
            var result = controller.Next(contact.Contact_ID) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result, "Contacts.Next() aurait dû renvoyer un RedirectToRouteResult");
            Assert.IsNotNull(result.RouteValues["controller"], "Contacts.Next() aurait dû définir le contrôleur");
            Assert.AreEqual("Details", result.RouteValues["action"], "Contacts.Next() aurait dû rediriger vers l'action Details");
            Assert.IsNotNull(result.RouteValues["id"], "Contacts.Next() aurait dû définir 'id'");
            Assert.IsNotNull(result.RouteValues["slug"], "Contacts.Next() aurait dû définir 'slug'");
        }

        [TestMethod]
        public void ContactsNext_doit_rediriger_vers_le_contact_suivant()
        {
            // Arrange
            var controller = new ContactsController(db);
            var contact1 = InsertPerson("next1", "0");
            var contact2 = InsertCompany("next2", "9");

            // Act
            var result = controller.Next(contact1.Contact_ID) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual(contact2.Contact_ID, result.RouteValues["id"], "Contacts.Next() aurait dû renvoyer l'id du contact suivant");
        }

        [TestMethod]
        public void ContactsNext_doit_rediriger_du_dernier_vers_le_premier_contact()
        {
            // Arrange
            var controller = new ContactsController(db);
            var first = db.Contacts.Where(x => x.LastName == "aaaa").FirstOrDefault()
                        ?? InsertPerson("aaaa", "0");
            var last = db.Contacts.Where(x => x.CompanyName == "zzzz").FirstOrDefault() 
                        ?? InsertCompany("zzzz", "9");

            // Act
            var result = controller.Next(last.Contact_ID) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual(first.Contact_ID, result.RouteValues["id"], "Contacts.Next() aurait dû renvoyer l'id du premier contact");
        }

        [TestMethod]
        public void ContactsPrevious_doit_rediriger_vers_une_action_detail()
        {
            // Arrange
            var controller = new ContactsController(db);
            var contact = InsertCompany("test", "0");

            // Act
            var result = controller.Previous(contact.Contact_ID) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result, "Contacts.Previous() aurait dû renvoyer un RedirectToRouteResult");
            Assert.IsNotNull(result.RouteValues["controller"], "Contacts.Previous() aurait dû définir le contrôleur");
            Assert.AreEqual("Details", result.RouteValues["action"], "Contacts.Previous() aurait dû rediriger vers l'action Details");
            Assert.IsNotNull(result.RouteValues["id"], "Contacts.Previous() aurait dû définir 'id'");
            Assert.IsNotNull(result.RouteValues["slug"], "Contacts.Previous() aurait dû définir 'slug'");
        }

        [TestMethod]
        public void ContactsPrevious_doit_rediriger_vers_le_contact_precedant()
        {
            // Arrange
            var controller = new ContactsController(db);
            var contact1 = InsertCompany("prev1", "9");
            var contact2 = InsertPerson("prev2", "0");

            // Act
            var result = controller.Previous(contact2.Contact_ID) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual(contact1.Contact_ID, result.RouteValues["id"], "Contacts.Previous() aurait dû renvoyer l'id du contact précédant");
        }

        [TestMethod]
        public void ContactsPrevious_doit_rediriger_du_premier_vers_le_dernier_contact()
        {
            // Arrange
            var controller = new ContactsController(db);
            var first = db.Contacts.Where(x => x.LastName == "aaaa").FirstOrDefault()
                        ?? InsertPerson("aaaa", "0");
            var last = db.Contacts.Where(x => x.CompanyName == "zzzz").FirstOrDefault()
                        ?? InsertCompany("zzzz", "9");

            // Act
            var result = controller.Previous(first.Contact_ID) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual(last.Contact_ID, result.RouteValues["id"], "Contacts.Next() aurait dû renvoyer l'id du dernier contact");
        }

        [TestMethod]
        public void ContactsExport_renvoie_un_JsonResult()
        {
            // Arrange
            var controller = new ContactsController(db);

            // Act
            var result = controller.Export();

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
        }

        [TestMethod]
        public void ContactsExport_renvoie_un_JsonResult_contenant_List_FlatContact()
        {
            // Arrange
            var controller = new ContactsController(db);

            // Act
            var result = controller.Export();

            // Assert
            var model = result.Data as List<FlatContact>;
            Assert.IsNotNull(model, "Data devrait être du type List<FlatContact>");
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
