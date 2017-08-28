using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using woontest.Models;

namespace woontest.Helpers
{
    public static class CartHelpers
    {
        public static MvcHtmlString CartCount(this HtmlHelper html, object cart)
        {
            if (cart == null)
            {
                return new MvcHtmlString("0");
            }
            return new MvcHtmlString(((Cart)cart).Lines.Count().ToString());
        }
    }
}