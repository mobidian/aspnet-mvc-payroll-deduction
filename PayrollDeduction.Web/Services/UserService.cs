using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using PayrollDeduction.Web.Models;

namespace PayrollDeduction.Web.Services
{
    public interface IUserService
    {
        bool Authenticate(string username, string password);
        bool IsAdmin(string username);
        void SignIn(string username, bool createPersistentCookie);
        void SignOut();
    }

    /// <summary>
    /// Represents a service responsible for authenticating and authorizing 
    /// users.
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        /// Verifies that the given username and password are valid.
        /// </summary>
        public bool Authenticate(string username, string password)
        {
            return Membership.ValidateUser(username, password);
        }

        /// <summary>
        /// Verifies whether or ot the given user is an administrator.
        /// </summary>
        public bool IsAdmin(string username)
        {
            return Roles.IsUserInRole(username, "Admin");
        }

        /// <summary>
        /// Signs a user into the application.
        /// </summary>
        public void SignIn(string username, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(username, createPersistentCookie);
        }

        /// <summary>
        /// Signs a user out of the application.
        /// </summary>
        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}