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
            var result = controller.Index();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName));
        }

    }
}
