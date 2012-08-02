using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repertoir.Controllers;
using Repertoir.Helpers;

namespace Repertoir.Tests.Helpers
{
    [TestClass]
    public class FlashHelperTest
    {
        [TestMethod]
        public void Flash_enregistre_le_texte_dans_TempData()
        {
            // Arrange
            var controller = new AboutController();

            // Act
            controller.Flash("Raton-laveur");

            // Assert
            Assert.IsTrue(controller.TempData.ContainsValue("Raton-laveur"));
        }

        [TestMethod]
        public void Flash_renvoie_null_quand_pas_de_message()
        {
            // Arrange            
            var controller = new AboutController();
            var context = new ViewContext
            {
                TempData = controller.TempData
            };
            var helper = new HtmlHelper(context, Moq.GetViewDataContainer());

            // Act
            var flash = helper.Flash();

            // Assert
            Assert.AreEqual(null, flash);
        }

        [TestMethod]
        public void Flash_renvoie_le_message_quand_il_existe()
        {
            // Arrange            
            var controller = new AboutController();
            controller.Flash("Raton-laveur");
            var context = new ViewContext
            {
                TempData = controller.TempData
            };
            var helper = new HtmlHelper(context, Moq.GetViewDataContainer());

            // Act
            var flash = helper.Flash();

            // Assert
            Assert.IsTrue(flash.ToString().Contains("Raton-laveur"));
        }
    }
}