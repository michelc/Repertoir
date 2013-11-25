using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Repertoir.Helpers
{
    public static class HtmlHelperExtension
    {
        // http://blog.orangelightning.co.uk/?p=20
        // http://weblogs.asp.net/gunnarpeipman/archive/2012/06/17/asp-net-mvc-how-to-show-asterisk-after-required-field-label.aspx

        public static MvcHtmlString CaptionFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }

            var tag = new TagBuilder("label");
            tag.Attributes.Add("for", TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));
            tag.SetInnerText(labelText);

            if (metadata.ContainerType != null)
            {
                bool isRequired = metadata.ContainerType.GetProperty(metadata.PropertyName)
                                          .GetCustomAttributes(typeof(RequiredAttribute), false)
                                          .Length == 1;
                if (isRequired)
                {
                    tag.AddCssClass("is_required");
                    tag.InnerHtml += "<span>*</span>";
                }
            }

            return new MvcHtmlString(tag.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString ActionCancel(this HtmlHelper helper)
        {
            MvcHtmlString html = null;

            var id = helper.ViewContext.RouteData.Values["id"];
            if (id != null)
            {
                var slug = helper.ViewContext.RouteData.Values["slug"];
                html = helper.ActionLink("Annuler", "Details", new { id = id.ToString(), slug = slug.ToString() }, new { @class = "cancel" });
            }
            else
            {
                html = helper.ActionLink("Annuler", "Index", "Contacts", null, new { @class = "cancel" });
            }

            return html;
        }

        public static MvcHtmlString ActionCrud(this HtmlHelper helper, object linkValues = null)
        {
            var current_controller = helper.ViewContext.RouteData.Values["controller"].ToString().ToLower();
            var current_action = helper.ViewContext.RouteData.Values["action"].ToString().ToLower();

            var html = "";
            var index_title = current_controller == "tags" ? "Tags" : "Contacts";

            // Si on n'est pas sur la page Index
            if (current_action != "index")
            {
                // Alors, il faut un lien vers la page Index
                var index_controller = current_controller == "tags" ? "Tags" : "Contacts";
                html += helper.ActionLink(index_title, "Index", index_controller).ToString();
            }
            else
            {
                // Sinon, il n'y a pas besoin d'un lien vers la page Index
                html += index_title;
            }

            // Si on n'est pas sur la page Create, alors il faut faire un lien vers la page Create
            html += " / ";
            if (current_action != "create")
            {
                // Si on est sur l'index des contacts
                if (current_controller == "contacts")
                {
                    // Alors, le lien Create doit se faire sur les personnes
                    html += helper.ActionLink("Créer", "Create", "People").ToString();
                }
                else
                {
                    // Sinon, le lien Create doit se faire sur le controlleur en cours
                    html += helper.ActionLink("Créer", "Create").ToString();
                }
            }
            else
            {
                // Sinon, il n'y a pas besoin d'un lien vers la page Create
                html += "Créer";
            }

            // Si on a un identifiant de fiche
            var id = helper.ViewContext.RouteData.Values["id"];
            if (id != null)
            {
                // Alors, il faut générer les autres liens CRUD
                var crud = new Dictionary<string, string>
                {
                    { "details", "Afficher" },
                    { "edit", "Modifier" },
                    { "delete", "Supprimer" }
                };
                if (linkValues != null)
                {
                    var linkProperties = TypeDescriptor.GetProperties(linkValues);
                    foreach (PropertyDescriptor property in linkProperties)
                    {
                        crud.Add(property.Name.ToLowerInvariant(), property.GetValue(linkValues).ToString());
                    }
                }
                // (mais pas de fiche détail pour les tags)
                if (current_controller == "tags") crud.Remove("details");

                var slug = helper.ViewContext.RouteData.Values["slug"];
                foreach (var action in crud)
                {
                    // Si on n'est pas sur la page Action
                    html += " / ";
                    if (current_action != action.Key)
                    {
                        // Alors, il faut un lien vers la page Action
                        if (slug != null)
                        {
                            html += helper.ActionLink(action.Value, action.Key, new { id = id.ToString(), slug = slug.ToString() }).ToString();
                        }
                        else
                        {
                            html += helper.ActionLink(action.Value, action.Key, new { id }).ToString();
                        }
                    }
                    else
                    {
                        // Sinon, il n'y a pas besoin d'un lien vers la page Action
                        html += action.Value;
                    }
                }

            }

            return new MvcHtmlString(html);
        }

        public static MvcHtmlString ContactCss(this HtmlHelper helper, bool IsCompany, string Civility)
        {
            string cssClass = string.Empty;

            if (IsCompany)
            {
                cssClass = "is_company";
            }
            else if (Civility == "M.")
            {
                cssClass = "is_man";
            }
            else if (Civility == "Mme")
            {
                cssClass = "is_woman";
            }

            return new MvcHtmlString(cssClass);
        }

        public static MvcHtmlString DisplayMarkdown(this HtmlHelper htmlHelper, string text)
        {
            // Cas le plus simple
            if (string.IsNullOrEmpty(text)) return MvcHtmlString.Empty;

            // Converti le HTML éventuel en chaine codée en HTML
            text = System.Web.HttpUtility.HtmlEncode(text);

            // Découpe le texte en paragraphes
            text = text.Replace("\r\n", "\n").Replace("\r", "\n");
            var paragraphes = text.Split('\n');

            // Expressions régulières pour formattage "markdown" très basique
            var strong = new Regex(@"(\*\*|__) ((.|\n)*?) (\*\*|__)", RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);
            var em = new Regex(@"(\*|_) ((.|\n)*?) (\*|_)", RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);
            var link = new Regex("(https?://[^ ]+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var email = new Regex(@"([a-zA-Z_0-9.-]+\@[a-zA-Z_0-9.-]+\.\w+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // Converti chaque paragraphe séparément
            text = "";
            bool list = false;
            foreach (var p in paragraphes)
            {
                // Remplace tous les espaces multiples par un seul espace
                var temp = Regex.Replace(p, @"\s+", " ").Trim();

                // Traite le cas des listes à puces
                if ((temp.StartsWith("* ")) || (temp.StartsWith("+ ")) || (temp.StartsWith("- ")))
                {
                    if (!list)
                    {
                        // Démarre une nouvelle liste
                        list = true;
                        text += "<ul>\n";
                    }
                    temp = temp.Substring(1).Trim();
                }
                else if (list)
                {
                    // Termine la liste en cours
                    list = false;
                    text += "</ul>\n";
                }

                // Gère le gras, l'italique, les liens et les adresses méls
                temp = strong.Replace(temp, "<strong>$2</strong>"); // $2 car $1 contient ** ou __
                temp = em.Replace(temp, "<em>$2</em>");
                temp = link.Replace(temp, "<a href=\"$1\">$1</a>");
                temp = email.Replace(temp, "<a href=\"mailto:$1\">$1</a>");

                // Concatène le paragraphe html
                if (list)
                {
                    text += "  <li>" + temp + "</li>\n";
                }
                else
                {
                    text += "<p>" + temp + "</p>\n";
                }
            }

            // Termine la liste en cours si nécessaire
            if (list)
            {
                list = false;
                text += "</ul>\n";
            }

            return new MvcHtmlString(text);
        }
    }
}