﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
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
            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));

            bool isRequired = false;
            if (metadata.ContainerType != null)
            {
                isRequired = metadata.ContainerType.GetProperty(metadata.PropertyName)
                                     .GetCustomAttributes(typeof(RequiredAttribute), false)
                                     .Length == 1;
            }

            if (isRequired)
            {
                tag.AddCssClass("is_required");
                tag.InnerHtml = String.Format("{0}<span>*</span>", labelText);
            }
            else
            {
                tag.SetInnerText(labelText);
            }

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        public static HtmlString ActionCancel(this HtmlHelper helper, int id = 0)
        {
            MvcHtmlString html = null;

            if (id != 0)
            {
                html = helper.ActionLink("Annuler", "Details", new { id = id });
            }
            else
            {
                html = helper.ActionLink("Annuler", "Index", "Contacts");
            }

            return html;
        }

        public static HtmlString ActionCrud(this HtmlHelper helper, int id = 0)
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
            if (id != 0)
            {
                // Alors, il faut générer les autres liens CRUD

                // Si on n'est pas sur la page Details
                html += " / ";
                if (current_action != "details")
                {
                    // Alors, il faut un lien vers la page Details
                    html += helper.ActionLink("Afficher", "Details", new { id = id }).ToString();
                }
                else
                {
                    // Sinon, il n'y a pas besoin d'un lien vers la page Details
                    html += "Afficher";
                }

                // Si on n'est pas sur la page Edit
                html += " / ";
                if (current_action != "edit")
                {
                    // Alors, il faut un lien vers la page Edit
                    html += helper.ActionLink("Modifier", "Edit", new { id = id }).ToString();
                }
                else
                {
                    // Sinon, il n'y a pas besoin d'un lien vers la page Edit
                    html += "Modifier";
                }

                // Si on n'est pas sur la page Delete
                html += " / ";
                if (current_action != "delete")
                {
                    // Alors, il faut un lien vers la page Delete
                    html += helper.ActionLink("Supprimer", "Delete", new { id = id }).ToString();
                }
                else
                {
                    // Sinon, il n'y a pas besoin d'un lien vers la page Delete
                    html += "Supprimer";
                }

            }

            return new HtmlString(html);
        }
    }
}