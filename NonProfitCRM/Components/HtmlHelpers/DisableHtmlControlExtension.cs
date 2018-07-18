using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NonProfitCRM.Components.HtmlHelpers
{
    public static class DisableHtmlControlExtension
    {
        public static MvcHtmlString DisableIf(this MvcHtmlString htmlString, bool expression)
        {
            if (expression)
            {
                var html = htmlString.ToString();
                const string disabled = "\"disabled\"";
                html = html.Insert(html.IndexOf(">",
                  StringComparison.Ordinal), " disabled= " + disabled);
                return new MvcHtmlString(html);
            }
            return htmlString;
        }
    }
}