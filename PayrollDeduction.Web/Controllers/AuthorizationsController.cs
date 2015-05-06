using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayrollDeduction.Web.Models;
using PayrollDeduction.Web.ViewModels;

namespace PayrollDeduction.Web.Controllers
{
    /// <summary>
    /// The AuthorizationsController allows a user to complete
    /// an authorization form to create a new account or update
    /// an existing one.
    /// </summary>
    public class AuthorizationsController : BaseController
    {
        /// <summary>
        /// Returns the form to create and authorization.
        /// </summary>
        public ActionResult Create()
        {
            var formModel = new AuthorizationFormModel();

            if (User.Identity.IsAuthenticated)
            {
                var account = DB.Accounts
                    .Include(x => x.TeamMember)
                    .Include(x => x.Dependents)
                    .Single(x => x.TeamMember.TeamMemberId == User.TeamMemberId);

                formModel.TeamMemberId = User.TeamMemberId;

                formModel.Dependents = account.Dependents.ConvertAll(d => new DependentFormModel()
                {
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    BirthDate = d.BirthDate
                });
            }

            for (var i = 0; i < 6; i++)
                formModel.Dependents.Add(new DependentFormModel());

            return View(formModel);
        }

        /// <summary>
        /// Creates an authorization for the current user.
        /// </summary>
        [HttpPost]
        public ActionResult Create(AuthorizationFormModel formModel)
        {
            if (!ModelState.IsValid)
                return View(formModel);

            var returnTo = (User.Identity.IsAuthenticated ? "Account_Home" : "Root");
            var teamMemberId = (User.Identity.IsAuthenticated ? User.TeamMemberId : formModel.TeamMemberId);
            var teamMember = DB.TeamMembers.Single(x => x.TeamMemberId == teamMemberId);

            var authorization = new Authorization()
            {
                TeamMember = teamMember
            };

            authorization.Dependents = formModel.Dependents
                .Where(x => !String.IsNullOrEmpty(x.FirstName))
                .Where(x => !String.IsNullOrEmpty(x.LastName))
                .Where(x => x.BirthDate.HasValue)
                .Select(x => new AuthorizationDependent()
                {
                    Authorization = authorization,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    BirthDate = x.BirthDate.Value
                }).ToList();

            DB.Authorizations.Add(authorization);
            DB.SaveChanges();

            this.FlashSuccess(@"Your authorization was successfully saved. You will be notified
                                via email when it is approved.");

            return RedirectToRoute(returnTo);
        }
    }
}
