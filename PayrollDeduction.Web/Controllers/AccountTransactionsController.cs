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
    /// The AccountTransactionsController is responsible for allowing the
    /// current user to view dependents on their account.
    /// </summary>
    [Authorize]
    public class AccountTransactionsController : BaseController
    {
        /// <summary>
        /// Lists the transactions an the current user's account.
        /// </summary>
        public ActionResult Index(int page = 0)
        {
            var account = DB.Accounts
                .Include(x => x.TeamMember)
                .Single(x => x.TeamMember.TeamMemberId == User.TeamMemberId);

            // Load a paged set of transactions for account.
            DB.Entry(account)
                .Collection(x => x.Transactions)
                .Query()
                .OrderByDescending(x => x.Id)
                .Skip(page * 50)
                .Take(50)
                .Load();

            if (Request.IsAjaxRequest())
                return PartialView("_Transactions", account);
            else if (account.Transactions.Any())
                return View(account);
            else
                return View("Empty", account);
        }
    }
}