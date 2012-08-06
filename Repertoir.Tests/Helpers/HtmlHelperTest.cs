using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repertoir.Helpers;
using Repertoir.Models;

namespace Repertoir.Tests.Helpers
{
    [TestClass]
    public class HtmlHelperTest
    {
        [TestMethod]
        public void CaptionFor_renvoie_is_required_si_attribut_required()
        {
            // Arrange
            var helper = new HtmlHelper<ViewPerson>(Moq.GetViewContext(), Moq.GetViewDataContainer());

            // Act
            var link = helper.CaptionFor(x => x.LastName);

            // Assert
            Assert.IsTrue(link.ToString().Contains(" class=\"is_required\""));
        }

        [TestMethod]
        public void CaptionFor_renvoie_span_etoile_si_attribut_required()
        {
            // Arrange
            var helper = new HtmlHelper<ViewPerson>(Moq.GetViewContext(), Moq.GetViewDataContainer());

            // Act
            var link = helper.CaptionFor(x => x.LastName);

            // Assert
            Assert.IsTrue(link.ToString().Contains("<span>*</span>"));
        }

        [TestMethod]
        public void CaptionFor_ne_renvoie_pas_is_required_si_pas_attribut_required()
        {
            // Arrange
            var helper = new HtmlHelper<ViewPerson>(Moq.GetViewContext(), Moq.GetViewDataContainer());

            // Act
            var link = helper.CaptionFor(x => x.FirstName);

            // Assert
            Assert.IsFalse(link.ToString().Contains(" class=\"is_required\""));
        }

        [TestMethod]
        public void CaptionFor_ne_renvoie_pas_span_etoile_si_pas_attribut_required()
        {
            // Arrange
            var helper = new HtmlHelper<ViewPerson>(Moq.GetViewContext(), Moq.GetViewDataContainer());

            // Act
            var link = helper.CaptionFor(x => x.FirstName);

            // Assert
            Assert.IsFalse(link.ToString().Contains("<span>*</span>"));
        }

        [TestMethod]
        public void ActionCancel_renvoie_lien_index_si_id_est_null()
        {
            // Arrange
            var routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);
            var helper = new HtmlHelper(Moq.GetViewContext(), Moq.GetViewDataContainer(), routes);

            // Act
            var link = helper.ActionCancel();

            // Assert
            Assert.AreEqual("<a href=\"/\">Annuler</a>", link.ToString());
        }

        [TestMethod]
        public void ActionCancel_renvoie_lien_detail_si_id_n_est_pas_null()
        {
            // Arrange
            var routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);
            var helper = new HtmlHelper(Moq.GetViewContext(), Moq.GetViewDataContainer(), routes);
            helper.ViewContext.RouteData.Values["id"] = "5";
            helper.ViewContext.RouteData.Values["slug"] = "xxxxx";

            // Act
            var link = helper.ActionCancel();

            // Assert
            Assert.IsTrue(link.ToString().Contains("/details/5/xxxxx"));
        }

        [TestMethod]
        public void ActionCrud_renvoie_lien_contacts_si_pas_sur_index()
        {
            // Arrange
            var context = new ViewContext();
            context.RouteData = new RouteData();
            context.RouteData.Values["action"] = "toto";
            context.RouteData.Values["controller"] = "tutu";
            var helper = new HtmlHelper(context, Moq.GetViewDataContainer());

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
            var helper = new HtmlHelper(context, Moq.GetViewDataContainer());

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
            var helper = new HtmlHelper(context, Moq.GetViewDataContainer());

            // Act
            var link = helper.ActionCrud();

            // Assert
            Assert.IsTrue(HttpUtility.HtmlDecode(link.ToString()).Contains(">Créer</a>"));
        }

        [TestMethod]
        public void ActionCrud_renvoie_lien_creer_people_si_sur_contacts()
        {
            // Arrange
            var routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);
            var helper = new HtmlHelper(Moq.GetViewContext(), Moq.GetViewDataContainer(), routes);
            helper.ViewContext.RouteData.Values["action"] = "toto";
            helper.ViewContext.RouteData.Values["controller"] = "contacts";

            // Act
            var link = helper.ActionCrud();

            // Assert
            Assert.IsTrue(link.ToString().Contains("/people/create"));
        }

        [TestMethod]
        public void ActionCrud_renvoie_lien_creer_xxxxx_si_pas_sur_contacts()
        {
            // Arrange
            var routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);
            var helper = new HtmlHelper(Moq.GetViewContext(), Moq.GetViewDataContainer(), routes);
            helper.ViewContext.RouteData.Values["action"] = "toto";
            helper.ViewContext.RouteData.Values["controller"] = "xxxxx";

            // Act
            var link = helper.ActionCrud();

            // Assert
            Assert.IsTrue(link.ToString().Contains("/xxxxx/create"));
        }

        [TestMethod]
        public void ActionCrud_ne_renvoie_pas_lien_creer_si_deja_sur_create()
        {
            // Arrange
            var context = new ViewContext();
            context.RouteData = new RouteData();
            context.RouteData.Values["action"] = "create";
            context.RouteData.Values["controller"] = "tutu";
            var helper = new HtmlHelper(context, Moq.GetViewDataContainer());

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
            var helper = new HtmlHelper(context, Moq.GetViewDataContainer());

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
            var helper = new HtmlHelper(context, Moq.GetViewDataContainer());

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
            var helper = new HtmlHelper(context, Moq.GetViewDataContainer());

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
            var helper = new HtmlHelper(context, Moq.GetViewDataContainer());

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
            var helper = new HtmlHelper(context, Moq.GetViewDataContainer());

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
            var helper = new HtmlHelper(context, Moq.GetViewDataContainer());

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
            var helper = new HtmlHelper(context, Moq.GetViewDataContainer());

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
            var helper = new HtmlHelper(context, Moq.GetViewDataContainer());

            // Act
            var link = helper.ActionCrud();

            // Assert
            Assert.IsFalse(link.ToString().Contains(">Supprimer</a>"));
        }

        [TestMethod]
        public void ContactCss_renvoie_is_company_pour_les_societes()
        {
            // Arrange
            var helper = new HtmlHelper(new ViewContext(), Moq.GetViewDataContainer());

            // Act
            var css = helper.ContactCss(true, null);

            // Assert
            Assert.AreEqual("is_company", css.ToString());
        }

        [TestMethod]
        public void ContactCss_renvoie_is_man_pour_les_monsieurs()
        {
            // Arrange
            var helper = new HtmlHelper(new ViewContext(), Moq.GetViewDataContainer());

            // Act
            var css = helper.ContactCss(false, "M.");

            // Assert
            Assert.AreEqual("is_man", css.ToString());
        }

        [TestMethod]
        public void ContactCss_renvoie_is_woman_pour_les_madames()
        {
            // Arrange
            var helper = new HtmlHelper(new ViewContext(), Moq.GetViewDataContainer());

            // Act
            var css = helper.ContactCss(false, "Mme");

            // Assert
            Assert.AreEqual("is_woman", css.ToString());
        }

        [TestMethod]
        public void ContactCss_renvoie_vide_pour_les_personnes_sans_civilite()
        {
            // Arrange
            var helper = new HtmlHelper(new ViewContext(), Moq.GetViewDataContainer());

            // Act
            var css = helper.ContactCss(false, null);

            // Assert
            Assert.AreEqual(string.Empty, css.ToString());
        }
    }
}