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
        private class CaptionForViewModel
        {
            [System.ComponentModel.DataAnnotations.Required]
            public string Obligatoire { get; set; }
            public string Facultatif { get; set; }
        }

        [TestMethod]
        public void CaptionFor_renvoie_is_required_si_attribut_required()
        {
            // Arrange
            var helper = new HtmlHelper<CaptionForViewModel>(Moq.GetViewContext(), Moq.GetViewDataContainer());

            // Act
            var link = helper.CaptionFor(x => x.Obligatoire);

            // Assert
            Assert.IsTrue(link.ToString().Contains(" class=\"is_required\""));
        }

        [TestMethod]
        public void CaptionFor_renvoie_span_etoile_si_attribut_required()
        {
            // Arrange
            var helper = new HtmlHelper<CaptionForViewModel>(Moq.GetViewContext(), Moq.GetViewDataContainer());

            // Act
            var link = helper.CaptionFor(x => x.Obligatoire);

            // Assert
            Assert.IsTrue(link.ToString().Contains("<span>*</span>"));
        }

        [TestMethod]
        public void CaptionFor_ne_renvoie_pas_is_required_si_pas_attribut_required()
        {
            // Arrange
            var helper = new HtmlHelper<CaptionForViewModel>(Moq.GetViewContext(), Moq.GetViewDataContainer());

            // Act
            var link = helper.CaptionFor(x => x.Facultatif);

            // Assert
            Assert.IsFalse(link.ToString().Contains(" class=\"is_required\""));
        }

        [TestMethod]
        public void CaptionFor_ne_renvoie_pas_span_etoile_si_pas_attribut_required()
        {
            // Arrange
            var helper = new HtmlHelper<CaptionForViewModel>(Moq.GetViewContext(), Moq.GetViewDataContainer());

            // Act
            var link = helper.CaptionFor(x => x.Facultatif);

            // Assert
            Assert.IsFalse(link.ToString().Contains("<span>*</span>"));
        }

        [TestMethod]
        public void ActionCancel_renvoie_lien_index_si_id_est_null()
        {
            // Arrange
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            var helper = new HtmlHelper(Moq.GetViewContext(), Moq.GetViewDataContainer(), routes);

            // Act
            var link = helper.ActionCancel();

            // Assert
            Assert.AreEqual("<a class=\"cancel\" href=\"/\">Annuler</a>", link.ToString());
        }

        [TestMethod]
        public void ActionCancel_renvoie_lien_detail_si_id_n_est_pas_null()
        {
            // Arrange
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            var helper = new HtmlHelper(Moq.GetViewContext(), Moq.GetViewDataContainer(), routes);
            helper.ViewContext.RouteData.Values["id"] = "5";
            helper.ViewContext.RouteData.Values["slug"] = "xxxxx";

            // Act
            var link = helper.ActionCancel();

            // Assert
            Assert.IsTrue(link.ToString().Contains("/details/5/xxxxx"));
        }

        [TestMethod]
        public void ActionCancel_renvoie_lien_avec_classe_cancel()
        {
            // Arrange
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            var helper = new HtmlHelper(Moq.GetViewContext(), Moq.GetViewDataContainer(), routes);
            helper.ViewContext.RouteData.Values["id"] = "5";
            helper.ViewContext.RouteData.Values["slug"] = "xxxxx";

            // Act
            var link = helper.ActionCancel();

            // Assert
            Assert.IsTrue(link.ToString().Contains("class=\"cancel\""));
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
            RouteConfig.RegisterRoutes(routes);
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
            RouteConfig.RegisterRoutes(routes);
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

        [TestMethod]
        public void DisplayMarkdown_renvoie_vide_si_texte_est_vide()
        {
            // Arrange
            var helper = new HtmlHelper(new ViewContext(), Moq.GetViewDataContainer());

            // Act
            var html = helper.DisplayMarkdown(string.Empty).ToString();

            // Assert
            Assert.AreEqual(string.Empty, html);
        }

        [TestMethod]
        public void DisplayMarkdown_renvoie_vide_si_texte_est_nul()
        {
            // Arrange
            var helper = new HtmlHelper(new ViewContext(), Moq.GetViewDataContainer());

            // Act
            var html = helper.DisplayMarkdown(null).ToString();

            // Assert
            Assert.AreEqual(string.Empty, html);
        }

        [TestMethod]
        public void DisplayMarkdown_genere_paragraphes_si_saut_de_lignes()
        {
            // Arrange
            var helper = new HtmlHelper(new ViewContext(), Moq.GetViewDataContainer());

            // Act
            var html = helper.DisplayMarkdown("un\ndeux").ToString();

            // Assert
            Assert.AreEqual("<p>un</p>\n<p>deux</p>\n", html);
        }

        [TestMethod]
        public void DisplayMarkdown_genere_paragraphes_si_nouvelles_lignes()
        {
            // Arrange
            var helper = new HtmlHelper(new ViewContext(), Moq.GetViewDataContainer());

            // Act
            var html = helper.DisplayMarkdown("un" + System.Environment.NewLine + "deux").ToString();

            // Assert
            Assert.AreEqual("<p>un</p>\n<p>deux</p>\n", html);
        }

        [TestMethod]
        public void DisplayMarkdown_genere_liste_si_lignes_commencent_par_asterisques()
        {
            // Arrange
            var helper = new HtmlHelper(new ViewContext(), Moq.GetViewDataContainer());

            // Act
            var html = helper.DisplayMarkdown("* un\n* deux").ToString();

            // Assert
            Assert.AreEqual("<ul>\n  <li>un</li>\n  <li>deux</li>\n</ul>\n", html);
        }

        [TestMethod]
        public void DisplayMarkdown_genere_liste_seulement_si_lignes_commencent_par_asterisques()
        {
            // Arrange
            var helper = new HtmlHelper(new ViewContext(), Moq.GetViewDataContainer());

            // Act
            var html = helper.DisplayMarkdown("u*n\nde*ux").ToString();

            // Assert
            Assert.AreEqual("<p>u*n</p>\n<p>de*ux</p>\n", html);
        }

        [TestMethod]
        public void DisplayMarkdown_genere_strong_si_double_asterisques()
        {
            // Arrange
            var helper = new HtmlHelper(new ViewContext(), Moq.GetViewDataContainer());

            // Act
            var html = helper.DisplayMarkdown("un **deux** trois").ToString();

            // Assert
            Assert.AreEqual("<p>un <strong>deux</strong> trois</p>\n", html);
        }

        [TestMethod]
        public void DisplayMarkdown_genere_strong_si_double_soulignes()
        {
            // Arrange
            var helper = new HtmlHelper(new ViewContext(), Moq.GetViewDataContainer());

            // Act
            var html = helper.DisplayMarkdown("un __deux__ trois").ToString();

            // Assert
            Assert.AreEqual("<p>un <strong>deux</strong> trois</p>\n", html);
        }

        [TestMethod]
        public void DisplayMarkdown_genere_em_si_simple_asterisque()
        {
            // Arrange
            var helper = new HtmlHelper(new ViewContext(), Moq.GetViewDataContainer());

            // Act
            var html = helper.DisplayMarkdown("un *deux* trois").ToString();

            // Assert
            Assert.AreEqual("<p>un <em>deux</em> trois</p>\n", html);
        }

        [TestMethod]
        public void DisplayMarkdown_genere_em_si_simple_souligne()
        {
            // Arrange
            var helper = new HtmlHelper(new ViewContext(), Moq.GetViewDataContainer());

            // Act
            var html = helper.DisplayMarkdown("un _deux_ trois").ToString();

            // Assert
            Assert.AreEqual("<p>un <em>deux</em> trois</p>\n", html);
        }

        [TestMethod]
        public void DisplayMarkdown_genere_lien_si_url_en_http()
        {
            // Arrange
            var helper = new HtmlHelper(new ViewContext(), Moq.GetViewDataContainer());

            // Act
            var html = helper.DisplayMarkdown("un http://lien !").ToString();

            // Assert
            Assert.AreEqual("<p>un <a href=\"http://lien\">http://lien</a> !</p>\n", html);
        }

        [TestMethod]
        public void DisplayMarkdown_genere_lien_si_url_en_https()
        {
            // Arrange
            var helper = new HtmlHelper(new ViewContext(), Moq.GetViewDataContainer());

            // Act
            var html = helper.DisplayMarkdown("un https://lien !").ToString();

            // Assert
            Assert.AreEqual("<p>un <a href=\"https://lien\">https://lien</a> !</p>\n", html);
        }

        [TestMethod]
        public void DisplayMarkdown_genere_lien_mailto_si_email()
        {
            // Arrange
            var helper = new HtmlHelper(new ViewContext(), Moq.GetViewDataContainer());

            // Act
            var html = helper.DisplayMarkdown("un e@mail.com !").ToString();

            // Assert
            Assert.AreEqual("<p>un <a href=\"mailto:e@mail.com\">e@mail.com</a> !</p>\n", html);
        }

        [TestMethod]
        public void DisplayMarkdown_encode_le_html_eventuel()
        {
            // Arrange
            var helper = new HtmlHelper(new ViewContext(), Moq.GetViewDataContainer());

            // Act
            var html = helper.DisplayMarkdown("un <b>tag</b> !").ToString();

            // Assert
            Assert.AreEqual("<p>un &lt;b&gt;tag&lt;/b&gt; !</p>\n", html);
        }
    }
}