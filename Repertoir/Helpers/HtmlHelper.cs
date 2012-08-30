﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
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

        public static MvcHtmlString ActionCrud(this HtmlHelper helper)
        {
            var current_action = helper.ViewContext.RouteData.Values["action"].ToString().ToLower();

            var html = "";

            // Si on n'est pas sur la page Index
            if (current_action != "index")
            {
                // Alors, il faut un lien vers la page Index
                html += helper.ActionLink("Contacts", "Index", "Contacts").ToString();
            }
            else
            {
                // Sinon, il n'y a pas besoin d'un lien vers la page Index
                html += "Contacts";
            }

            // Si on n'est pas sur la page Create
            html += " / ";
            if (current_action != "create")
            {
                // Alors, il faut un lien vers la page Create
                var current_controller = helper.ViewContext.RouteData.Values["controller"].ToString().ToLower();
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

                var slug = helper.ViewContext.RouteData.Values["slug"];
                foreach (var action in crud)
                {
                    // Si on n'est pas sur la page Action
                    html += " / ";
                    if (current_action != action.Key)
                    {
                        // Alors, il faut un lien vers la page Action
                        html += helper.ActionLink(action.Value, action.Key, new { id = id.ToString(), slug = slug.ToString() }).ToString();
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
    }
}