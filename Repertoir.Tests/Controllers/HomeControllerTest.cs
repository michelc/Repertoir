using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repertoir.Controllers;

namespace Repertoir.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.AreEqual("Répertoir", result.ViewBag.Title);
        }
    }
}
