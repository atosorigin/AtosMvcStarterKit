using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Customer.Project.Mvc.HtmlHelpers
{
    public static class UrlHelperExtensions
    {
        public static string Image(this UrlHelper helper, string fileName)
        {
            return helper.Content("~/Images/" + fileName);
        }

        public static string Home(this UrlHelper helper)
        {
            return helper.Content("~/");
        }

        public static string About(this UrlHelper helper)
        {
            return helper.Content("~/Home/About");
        }
        
        public static string Contact(this UrlHelper helper)
        {
            return helper.Content("~/Home/Contact");
        }

        public static string Admin(this UrlHelper helper)
        {
            return helper.Content("~/Admin");
        }

        public static string ActionResultMessageExample(this UrlHelper helper)
        {
            return helper.Content("~/Home/ActionResultMessageExample");
        }

        public static string ErrorHandlingExample(this UrlHelper helper)
        {
            return helper.Content("~/Home/ErrorHandlingExample");
        }

    }
}