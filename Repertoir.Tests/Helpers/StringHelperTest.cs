using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repertoir.Controllers;
using Repertoir.Helpers;

namespace Repertoir.Tests.Helpers
{
    [TestClass]
    public class StringHelperTest
    {
        [TestMethod]
        public void Unaccentize_supprime_les_accents_des_minuscules()
        {
            // Arrange
            var avec = "éèçàùôï";

            // Act
            var sans = avec.Unaccentize();

            // Assert
            Assert.AreEqual("eecauoi", sans);
        }

        [TestMethod]
        public void Unaccentize_supprime_les_accents_des_majuscules()
        {
            // Arrange
            var avec = "ÉÈÇÀÙÔÏ";

            // Act
            var sans = avec.Unaccentize();

            // Assert
            Assert.AreEqual("EECAUOI", sans);
        }

        [TestMethod]
        public void Slugify_transforme_les_majuscules_en_minuscules()
        {
            // Arrange
            var avant = "AaÉé";

            // Act
            var apres = avant.Slugify();

            // Assert
            Assert.AreEqual("aaee", apres);
        }

        [TestMethod]
        public void Slugify_remplaces_tous_les_espaces_par_un_tiret()
        {
            // Arrange
            var avant = "a z\te\nr";

            // Act
            var apres = avant.Slugify();

            // Assert
            Assert.AreEqual("a-z-e-r", apres);
        }

        [TestMethod]
        public void Slugify_remplaces_les_espaces_consecutifs_par_un_seul_tiret()
        {
            // Arrange
            var avant = "a    z  \te \nr";

            // Act
            var apres = avant.Slugify();

            // Assert
            Assert.AreEqual("a-z-e-r", apres);
        }

        [TestMethod]
        public void Slugify_remplaces_toutes_les_ponctuations_par_un_tiret()
        {
            // Arrange
            var avant = "a,z?e;r.t:u!i";

            // Act
            var apres = avant.Slugify();

            // Assert
            Assert.AreEqual("a-z-e-r-t-u-i", apres);
        }

        [TestMethod]
        public void Slugify_supprime_si_ni_lettre_ni_chiffre_ni_ponctuation()
        {
            // Arrange
            var avant = "a~z";

            // Act
            var apres = avant.Slugify();

            // Assert
            Assert.AreEqual("az", apres);
        }

        [TestMethod]
        public void Slugify_supprime_les_tirets_autour()
        {
            // Arrange
            var avant = "-azerty-";

            // Act
            var apres = avant.Slugify();

            // Assert
            Assert.AreEqual("azerty", apres);
        }
    }
}