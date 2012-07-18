using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repertoir.Controllers;

namespace Repertoir.Tests.Controllers
{
    [TestClass]
    public class ContactsControllerTest
    {
        [TestMethod]
        public void ContactsIndex_doit_renvoyer_la_vue_par_defaut()
        {
            // Arrange
            var controller = new ContactsController();

            // Act
            // var result = controller.Index();
            ViewResult result = null;

            // Assert
            Assert.Inconclusive("Mise en quarantaine de Contacts.Index() car il passe par la bdd");
            Assert.IsNotNull(result);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName));
        }

    }
}
