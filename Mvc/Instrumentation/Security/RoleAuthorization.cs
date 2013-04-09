using System;
using System.Web;
using System.Web.Mvc;
using AtosOrigin.NetLibrary.Components.Core;

namespace Customer.Project.Mvc.Instrumentation.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class RoleAuthorizationAttribute : AuthorizeAttribute
    {
        public SystemRoles[] SystemRoles { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            Check.Require(string.IsNullOrEmpty(Roles), "Roles property not supported");
            Check.Require(string.IsNullOrEmpty(Users), "Users property not supported");

            bool isUserInRole = false;
            foreach (SystemRoles role in this.SystemRoles)
            {
                isUserInRole |= System.Web.Security.Roles.IsUserInRole(role.ToString());
            }
            return isUserInRole;
            //return base.AuthorizeCore(httpContext);
        }

        protected override void HandleUnauthorizedRequest(System.Web.Mvc.AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                filterContext.Result = new System.Web.Mvc.HttpStatusCodeResult(403);
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}