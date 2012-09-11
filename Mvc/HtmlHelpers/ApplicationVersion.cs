using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Customer.Project.Mvc.HtmlHelpers
{
    public static class ApplicationVersionHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="html">extends HtmlHelper</param>
        /// <returns>Html to render the menu</returns>
        public static MvcHtmlString ApplicationVersion(this HtmlHelper html)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            return MvcHtmlString.Create(assembly.GetName().Version.ToString());
        }
    }
}