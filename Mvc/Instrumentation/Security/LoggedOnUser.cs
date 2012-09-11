using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Project.Mvc.Instrumentation
{
    public static class LoggedOnUser
    {
        public static string UserName
        {
            get
            {
                return (null != HttpContext.Current 
                    && null != HttpContext.Current.User
                    && !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                                ? HttpContext.Current.User.Identity.Name 
                                : "?";
            }

        }

    }
}