using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PayrollDeduction.Web.Mailers;
using PayrollDeduction.Web.Models;

namespace PayrollDeduction.Web.Controllers
{
    /// <summary>
    /// The AdminAuthorizationsController allows administrators to manage authorizations
    /// submitted by team members.
    /// </summary>
    [Authorize(Roles="Admin")]
    public class AdminAuthorizationsController : BaseController
    {
        public IAccountMailer AccountMailer { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (AccountMailer == null)
                AccountMailer = new AccountMailer();

            base.Initialize(requestContext);
        }

        /// <summary>
        /// List all authorizations
        /// </summary>
        [HttpGet]
        public ActionResult Index(int page = 0)
        {
            var authorizations = DB.Authorizations
                .Include(x => x.TeamMember)
                .Include(x => x.Dependents)
                .OrderBy(x => x.Archived)
                .ThenBy(x => x.TeamMember.LastName)
                .ThenBy(x => x.TeamMember.FirstName)
                .Skip(page * 50)
                .Take(50)
                .ToList();

            if (Request.IsAjaxRequest())
                return PartialView("_Authorizations", authorizations);
            else
                return View(authorizations);
        }

        /// <summary>
        /// Search authorizations by team member ID or last name
        /// </summary>
        [HttpGet]
        public ActionResult Search(string query, int page = 0)
        {
            var authorizations = DB.Authorizations
                .Include(x => x.TeamMember)
                .Include(x => x.Dependents)
                .Where(x => x.TeamMember.TeamMemberId == query || x.TeamMember.LastName.Contains(query))
                .OrderBy(x => x.Archived)
                .ThenBy(x => x.TeamMember.LastName)
                .ThenBy(x => x.TeamMember.FirstName)
                .Skip(page * 50)
                .Take(50)
                .ToList();

            if (Request.IsAjaxRequest())
                return PartialView("_Authorizations", authorizations);
            else
                return View("Index", authorizations);
        }

        /// <summary>
        /// Displays an authorization
        /// </summary>
        [HttpGet]
        public ActionResult Details(int id)
        {
            var authorization = DB.Authorizations
                .Include(x => x.TeamMember)
                .Include(x => x.Dependents)
                .Single(x => x.Id == id);

            return View(authorization);
        }

        /// <summary>
        /// Converts an authorization into an account.
        /// </summary>
        [HttpPost]
        public ActionResult Complete(int id)
        {
            var authorization = DB.Authorizations
                .Include(x => x.TeamMember)
                .Include(x => x.Dependents)
                .Single(x => x.Id == id);

            var account = DB.Accounts
                .Include(x => x.TeamMember)
                .Include(x => x.Dependents)
                .Where(x => x.TeamMember.Id == authorization.TeamMember.Id)
                .SingleOrDefault();

            if (account == null)
                account = new Account(authorization.TeamMember);

            // Remove existing dependents from the account
            account.Dependents.ForEach(d => account.Dependents.Remove(d));

            // Add dependents from the authorization
            account.Dependents = authorization.Dependents.ConvertAll(d => new AccountDependent()
            {
                Account = account,
                BirthDate = d.BirthDate,
                FirstName = d.FirstName,
                LastName = d.LastName
            });

            // Archive the authorization
            authorization.Archive();

            // Create a new account or update an existing one.
            if (account.Id == 0)
            {
                DB.Accounts.Add(account);
                DB.SaveChanges();
                AccountMailer.SendAccountCreated(account);
            }
            else
            {
                DB.SaveChanges();
                AccountMailer.SendDependentsAdded(account);
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Deletes an authorization
        /// </summary>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var authorization = DB.Authorizations.Single(x => x.Id == id);
            DB.Authorizations.Remove(authorization);
            DB.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
