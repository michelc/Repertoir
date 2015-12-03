using System.Web.Mvc;

namespace Repertoir.Helpers
{
    public static class FlashHelper
    {
        public static void Flash(this Controller controler, object text, params object[] args)
        {
            controler.TempData["FlashKey"] = string.Format(text.ToString(), args);
        }

        public static MvcHtmlString Flash(this HtmlHelper helper)
        {
            var flash = (string)helper.ViewContext.TempData["FlashKey"];
            if (flash == null) return null;

            var tag = new TagBuilder("div");
            tag.AddCssClass("flash");
            tag.InnerHtml = flash;

            return new MvcHtmlString(tag.ToString());
        }
    }
}