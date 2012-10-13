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
            var contact = GetOrInsertPerson("test", "0");

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
        public void ContactsNext_doit_rediriger_vers_le_bon_controleur()
        {
            // Arrange
            var controller = new ContactsController(db);
            var contact1 = GetOrInsertPerson("next_1", "0");
            var contact2 = GetOrInsertCompany("next_2", "9");
            var contact3 = GetOrInsertPerson("next_3", "0");

            // Act
            var result1 = controller.Next(contact1.Contact_ID) as RedirectToRouteResult;
            var result2 = controller.Next(contact2.Contact_ID) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Companies", result1.RouteValues["controller"], "Contacts.Next() aurait dû rediriger vers le contrôleur Companies");
            Assert.AreEqual("People", result2.RouteValues["controller"], "Contacts.Next() aurait dû rediriger vers le contrôleur People");
        }

        [TestMethod]
        public void ContactsNext_doit_rediriger_vers_le_contact_suivant()
        {
            // Arrange
            var controller = new ContactsController(db);
            var contact1 = GetOrInsertPerson("next1", "0");
            var contact2 = GetOrInsertCompany("next2", "9");

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
            var first = GetOrInsertPerson("aaaa", "0");
            var last = GetOrInsertCompany("zzzz", "9");

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
            var contact = GetOrInsertCompany("test", "0");

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
        public void ContactsPrevious_doit_rediriger_vers_le_bon_controleur()
        {
            // Arrange
            var controller = new ContactsController(db);
            var contact1 = GetOrInsertPerson("prev_1", "0");
            var contact2 = GetOrInsertCompany("prev_2", "9");
            var contact3 = GetOrInsertCompany("prev_3", "9");

            // Act
            var result1 = controller.Previous(contact3.Contact_ID) as RedirectToRouteResult;
            var result2 = controller.Previous(contact2.Contact_ID) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Companies", result1.RouteValues["controller"], "Contacts.Previous() aurait dû rediriger vers le contrôleur Companies");
            Assert.AreEqual("People", result2.RouteValues["controller"], "Contacts.Previous() aurait dû rediriger vers le contrôleur People");
        }

        [TestMethod]
        public void ContactsPrevious_doit_rediriger_vers_le_contact_precedant()
        {
            // Arrange
            var controller = new ContactsController(db);
            var contact1 = GetOrInsertPerson("prev1", "0");
            var contact2 = GetOrInsertCompany("prev2", "9");

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
            var first = GetOrInsertPerson("aaaa", "0");
            var last = GetOrInsertCompany("zzzz", "9");

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

        private Contact GetOrInsertPerson(string name, string phone)
        {
            var get = db.Contacts.Where(x => x.LastName == name && x.Phone1 == phone && x.IsCompany == false).FirstOrDefault();
            if (get != null) return get;

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

        private Contact GetOrInsertCompany(string name, string phone)
        {
            var get = db.Contacts.Where(x => x.CompanyName == name && x.Phone1 == phone && x.IsCompany == true).FirstOrDefault();
            if (get != null) return get;

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
