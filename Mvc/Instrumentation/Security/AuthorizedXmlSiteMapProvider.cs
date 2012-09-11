using System.Web;

namespace Customer.Project.Mvc.Instrumentation.Security
{
    /// <summary>
    /// FAST implementation of XmlSiteMapProvider. Limits accessibility to authorized users based on roles. 
    /// Also allows wildcards for roles (*=All users, ?=Unauthenticated users only, @=authenticated users only).
    /// </summary>
    public class AuthorizedXmlSiteMapProvider : XmlSiteMapProvider
    {
        private const string AllowAll = "*";
        private const string AllowAuthenticated = "@";
        private const string AllowNotAuthenticated = "?";
        /// <summary>
        /// Retrieves a Boolean value indicating whether the specified <see cref="T:System.Web.SiteMapNode"></see> object can be viewed by the user in the specified context.
        /// </summary>
        /// <param name="context">The <see cref="T:System.Web.HttpContext"></see> that contains user information.</param>
        /// <param name="node">The <see cref="T:System.Web.SiteMapNode"></see> that is requested by the user.</param>
        /// <returns>
        /// true if security trimming is enabled and node can be viewed by the user or security trimming is not enabled; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">context is null.- or -node is null.</exception>
        public override bool IsAccessibleToUser(HttpContext context, SiteMapNode node)
        {
            return !SecurityTrimmingEnabled || ValidateNode(context, node);
        }

        /// <summary>
        /// Validates the node.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        private static bool ValidateNode(HttpContext context, SiteMapNode node)
        {
            bool allowed = false;
            if (node.Roles.Count == 0)
            {
                allowed = true;
            }
            else
            {
                foreach (string role in node.Roles)
                {
                    if (role == AllowAll)
                    {
                        allowed = true;
                        break;
                    }
                    if (role == AllowNotAuthenticated && !context.User.Identity.IsAuthenticated)
                    {
                        allowed = true;
                        break;
                    }
                    if (role == AllowAuthenticated && context.User.Identity.IsAuthenticated)
                    {
                        allowed = true;
                        break;
                    }

                    if (context.User.IsInRole(role))
                    {
                        allowed = true;
                        break;
                    }
                }
            }
            return allowed;
        }
    }
}