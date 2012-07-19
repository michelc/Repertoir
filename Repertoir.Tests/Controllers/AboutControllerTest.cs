using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repertoir.Controllers;

namespace Repertoir.Tests.Controllers
{
    [TestClass]
    public class AboutControllerTest
    {
        [TestMethod]
        public void AboutIndex_doit_renvoyer_la_vue_par_defaut()
        {
            // Arrange
            var controller = new AboutController();

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName));
        }

        [TestMethod]
        public void AboutIndex_doit_definir_le_bon_titre_dans_ViewBag()
        {
            // Arrange
            var controller = new AboutController();

            // Act
            var result = controller.Index();

            // Assert
            Assert.AreEqual("Répertoir", result.ViewBag.Title);
        }
    }
}
