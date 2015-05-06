using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayrollDeduction.Web.Models;

namespace PayrollDeduction.Web.Controllers
{
    /// <summary>
    /// The AccountDependentsController is responsible for allowing the
    /// current user to view dependents on their account.
    /// </summary>
    [Authorize]
    public class AccountDependentsController : BaseController
    {
        /// <summary>
        /// Lists the dependents an the current user's account.
        /// </summary>
        public ActionResult Index()
        {
            var account = DB.Accounts
                .Include(x => x.TeamMember)
                .Include(x => x.Dependents)
                .Single(x => x.TeamMember.TeamMemberId == User.TeamMemberId);

            if (account.Dependents.Any())
                return View("Index", account);
            else
                return View("Empty", account);
        }
    }
}