using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Open_Library_Kashmir.CustomHelpers
{
    public static class CustomHelpers
    {
        public static IHtmlString Image(this HtmlHelper htmlHelper, string src, string alt)
        {
            TagBuilder imgTag = new TagBuilder("img");
            imgTag.Attributes.Add("src", VirtualPathUtility.ToAbsolute(src));
            imgTag.Attributes.Add("alt", alt);

            return new MvcHtmlString(imgTag.ToString(TagRenderMode.SelfClosing));

        }
    }
}