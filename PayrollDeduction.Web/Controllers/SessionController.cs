using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using PayrollDeduction.Web.Services;
using PayrollDeduction.Web.ViewModels;

namespace PayrollDeduction.Web.Controllers
{
    /// <summary>
    /// The SessionsController is responsible for signing users in and
    /// out of the application.
    /// </summary>
    public class SessionController : BaseController
    {
        public IUserService UserService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (UserService == null)
                UserService = new UserService();

            base.Initialize(requestContext);
        }

        /// <summary>
        /// Returns the Login form.
        /// </summary>
        [HttpGet]
        public ActionResult Create()
        {
            var formModel = new SessionFormModel();
            return View(formModel);
        }

        /// <summary>
        /// Attempts to sign a user in to the application.
        /// </summary>
        [HttpPost]
        public ActionResult Create(SessionFormModel formModel)
        {
            if (!ModelState.IsValid)
                return View(formModel);

            var authenticated = UserService.Authenticate(formModel.UserName, formModel.Password);

            if (!authenticated)
            {
                ModelState.AddModelError("", "The username or password provided is invalid.");
                return View(formModel);
            }

            var account = DB.Accounts
                .Include("TeamMember")
                .SingleOrDefault(x => x.TeamMember.NetworkId == formModel.UserName);

            if (account == null)
            {
                ModelState.AddModelError("", "An account was not found for the username provided.");
                return View(formModel);
            }

            UserService.SignIn(formModel.UserName, createPersistentCookie: true);

            // Redirect admins to Admin_Home and non-admins to Account_Home
            if (UserService.IsAdmin(formModel.UserName))
                return RedirectToRoute("Admin_Home");
            else
                return RedirectToRoute("Account_Home");
        }

        /// <summary>
        /// Signs the current user out of the application.
        /// </summary>
        [HttpGet]
        public ActionResult Delete()
        {
            UserService.SignOut();
            return RedirectToRoute("Root");
        }
    }
}
