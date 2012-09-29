using System.Collections.Generic;
using System.Data.Entity;
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
    }
}
