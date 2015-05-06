// ==================================================================================
// PLEASE NOTE:
// THIS CLASS IS PROVIDED FOR DEMONSTRATION PURPOSES ONLY AND SHOULD NOT BE USED IN A
// PRODUCTION APPLICATION. 
// ==================================================================================

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web.Security;
using PayrollDeduction.Web.Models;

namespace PayrollDeduction.Web.Providers
{
    public class SimpleRoleProvider : RoleProvider
    {
        #region RoleProvider overrides
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

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
        #endregion

        public override string[] GetRolesForUser(string username)
        {
            var user = UserStore.All().SingleOrDefault(u => u.Username == username);

            if (user != null && user.IsAdmin)
                return new[] { "Admin" };

            return new string[] { };
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            if (roleName != "Admin")
                return false;

            return UserStore.All().Any(u => u.Username == username && u.IsAdmin);
        }
    }
}
