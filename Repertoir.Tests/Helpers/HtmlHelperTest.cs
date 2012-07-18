using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repertoir.Helpers;

namespace Repertoir.Tests.Helpers
{
    [TestClass]
    public class HtmlHelperTest
    {
        private class VdcImplementation : IViewDataContainer
        {
            public ViewDataDictionary ViewData { get; set; }
        }

        [TestMethod]
        public void ActionCrud_renvoie_lien_contacts_si_pas_sur_index()
        {
            // Arrange            
            var context = new ViewContext();
            context.RouteData = new RouteData();
            context.RouteData.Values["action"] = "toto";
            context.RouteData.Values["controller"] = "tutu";
            HtmlHelper helper = new HtmlHelper(context, new VdcImplementation());

            // Act
            var link = helper.ActionCrud();

            // Assert
            Assert.IsTrue(link.ToString().Contains(">Contacts</a>"));
        }

        [TestMethod]
        public void ActionCrud_ne_renvoie_pas_lien_contacts_si_deja_sur_index()
        {
            // Arrange            
            var context = new ViewContext();
            context.RouteData = new RouteData();
            context.RouteData.Values["action"] = "index";
            context.RouteData.Values["controller"] = "tutu";
            HtmlHelper helper = new HtmlHelper(context, new VdcImplementation());

            // Act
            var link = helper.ActionCrud();

            // Assert
            Assert.IsFalse(link.ToString().Contains(">Contacts</a>"));
        }

        [TestMethod]
        public void ActionCrud_renvoie_lien_creer_si_pas_sur_create()
        {
            // Arrange            
            var context = new ViewContext();
            context.RouteData = new RouteData();
            context.RouteData.Values["action"] = "toto";
            context.RouteData.Values["controller"] = "tutu";
            HtmlHelper helper = new HtmlHelper(context, new VdcImplementation());

            // Act
            var link = helper.ActionCrud();

            // Assert
            Assert.IsTrue(HttpUtility.HtmlDecode(link.ToString()).Contains(">Créer</a>"));
        }

        [TestMethod]
        public void ActionCrud_ne_renvoie_pas_lien_creer_si_deja_sur_create()
        {
            // Arrange            
            var context = new ViewContext();
            context.RouteData = new RouteData();
            context.RouteData.Values["action"] = "create";
            context.RouteData.Values["controller"] = "tutu";
            HtmlHelper helper = new HtmlHelper(context, new VdcImplementation());

            // Act
            var link = helper.ActionCrud();

            // Assert
            Assert.IsFalse(link.ToString().Contains(">Créer</a>"));
        }

        [TestMethod]
        public void ActionCrud_ne_renvoie_pas_autres_liens_sans_id()
        {
            // Arrange            
            var context = new ViewContext();
            context.RouteData = new RouteData();
            context.RouteData.Values["action"] = "create";
            context.RouteData.Values["controller"] = "tutu";
            HtmlHelper helper = new HtmlHelper(context, new VdcImplementation());

            // Act
            var link = helper.ActionCrud();

            // Assert
            Assert.IsTrue(link.ToString().EndsWith("Créer"));
        }

        [TestMethod]
        public void ActionCrud_renvoie_autres_liens_si_id()
        {
            // Arrange            
            var context = new ViewContext();
            context.RouteData = new RouteData();
            context.RouteData.Values["id"] = "1";
            context.RouteData.Values["slug"] = "slug";
            context.RouteData.Values["action"] = "create";
            context.RouteData.Values["controller"] = "tutu";
            HtmlHelper helper = new HtmlHelper(context, new VdcImplementation());

            // Act
            var link = helper.ActionCrud();

            // Assert
            Assert.IsFalse(link.ToString().EndsWith("Créer"));
        }

        [TestMethod]
        public void ActionCrud_renvoie_lien_afficher_si_pas_sur_details()
        {
            // Arrange            
            var context = new ViewContext();
            context.RouteData = new RouteData();
            context.RouteData.Values["id"] = "1";
            context.RouteData.Values["slug"] = "slug";
            context.RouteData.Values["action"] = "toto";
            context.RouteData.Values["controller"] = "tutu";
            HtmlHelper helper = new HtmlHelper(context, new VdcImplementation());

            // Act
            var link = helper.ActionCrud();

            // Assert
            Assert.IsTrue(link.ToString().Contains(">Afficher</a>"));
        }

        [TestMethod]
        public void ActionCrud_ne_renvoie_pas_lien_afficher_si_deja_sur_details()
        {
            // Arrange            
            var context = new ViewContext();
            context.RouteData = new RouteData();
            context.RouteData.Values["id"] = "1";
            context.RouteData.Values["slug"] = "slug";
            context.RouteData.Values["action"] = "details";
            context.RouteData.Values["controller"] = "tutu";
            HtmlHelper helper = new HtmlHelper(context, new VdcImplementation());

            // Act
            var link = helper.ActionCrud();

            // Assert
            Assert.IsFalse(link.ToString().Contains(">Afficher</a>"));
        }

        [TestMethod]
        public void ActionCrud_renvoie_lien_modifier_si_pas_sur_edit()
        {
            // Arrange            
            var context = new ViewContext();
            context.RouteData = new RouteData();
            context.RouteData.Values["id"] = "1";
            context.RouteData.Values["slug"] = "slug";
            context.RouteData.Values["action"] = "toto";
            context.RouteData.Values["controller"] = "tutu";
            HtmlHelper helper = new HtmlHelper(context, new VdcImplementation());

            // Act
            var link = helper.ActionCrud();

            // Assert
            Assert.IsTrue(link.ToString().Contains(">Modifier</a>"));
        }

        [TestMethod]
        public void ActionCrud_ne_renvoie_pas_lien_modifier_si_deja_sur_edit()
        {
            // Arrange            
            var context = new ViewContext();
            context.RouteData = new RouteData();
            context.RouteData.Values["id"] = "1";
            context.RouteData.Values["slug"] = "slug";
            context.RouteData.Values["action"] = "edit";
            context.RouteData.Values["controller"] = "tutu";
            HtmlHelper helper = new HtmlHelper(context, new VdcImplementation());

            // Act
            var link = helper.ActionCrud();

            // Assert
            Assert.IsFalse(link.ToString().Contains(">Modifier</a>"));
        }

        [TestMethod]
        public void ActionCrud_renvoie_lien_supprimer_si_pas_sur_delete()
        {
            // Arrange            
            var context = new ViewContext();
            context.RouteData = new RouteData();
            context.RouteData.Values["id"] = "1";
            context.RouteData.Values["slug"] = "slug";
            context.RouteData.Values["action"] = "toto";
            context.RouteData.Values["controller"] = "tutu";
            HtmlHelper helper = new HtmlHelper(context, new VdcImplementation());

            // Act
            var link = helper.ActionCrud();

            // Assert
            Assert.IsTrue(link.ToString().Contains(">Supprimer</a>"));
        }

        [TestMethod]
        public void ActionCrud_ne_renvoie_pas_lien_supprimer_si_deja_sur_delete()
        {
            // Arrange            
            var context = new ViewContext();
            context.RouteData = new RouteData();
            context.RouteData.Values["id"] = "1";
            context.RouteData.Values["slug"] = "slug";
            context.RouteData.Values["action"] = "delete";
            context.RouteData.Values["controller"] = "tutu";
            HtmlHelper helper = new HtmlHelper(context, new VdcImplementation());

            // Act
            var link = helper.ActionCrud();

            // Assert
            Assert.IsFalse(link.ToString().Contains(">Supprimer</a>"));
        }
    }
}