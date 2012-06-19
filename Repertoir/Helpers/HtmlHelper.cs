using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

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

    }
}