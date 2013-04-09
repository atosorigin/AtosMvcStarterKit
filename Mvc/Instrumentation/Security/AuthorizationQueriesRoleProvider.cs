using System;
using System.Web.Providers;
using System.Web.Security;

namespace Customer.Project.Mvc.Instrumentation.Security
{
    public class AuthorizationQueriesRoleProvider : RoleProvider
    {
        public AuthorizationQueriesRoleProvider()
        {
        }
        public override string[] GetRolesForUser(string username)
        {
            // Todo: retrieve your custom roles here. Example:
            //string[] roles = MvcApplication.UserQueries.GetRolesForUser(username);
            return new string[] { };
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            // Todo: retrieve your custom roles here. Example:
            //return MvcApplication.UserQueries.IsInRole(username, roleName);
            return false;
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}