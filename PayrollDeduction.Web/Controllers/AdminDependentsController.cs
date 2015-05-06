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
    /// The AdminDependentsController is responsible for allowing the
    /// administrator to manage dependents on an account.
    /// </summary>
    [Authorize(Roles="Admin")]
    public class AdminDependentsController : BaseController
    {
        /// <summary>
        /// List all dependents on a account.
        /// </summary>
        [HttpGet]
        public ActionResult Index(int accountId)
        {
            var account = DB.Accounts
                .Include(x => x.TeamMember)
                .Include(x => x.Dependents)
                .Include(x => x.Transactions)
                .Single(x => x.Id == accountId);

            if (account.Dependents.Count > 0)
                return View("Index", account);
            else
                return View("Empty", account);
        }

        /// <summary>
        /// Adds a dependent to an account.
        /// </summary>
        [HttpPost]
        public ActionResult Create(int accountId, DependentFormModel formModel)
        {
            var account = DB.Accounts
                .Include(x => x.TeamMember)
                .Include(x => x.Dependents)
                .Include(x => x.Transactions)
                .Single(x => x.Id == accountId);

            if (!ModelState.IsValid)
                return View("Index", account);

            var dependent = new AccountDependent()
            {
                Account = account,
                FirstName = formModel.FirstName,
                LastName = formModel.LastName,
                BirthDate = formModel.BirthDate.Value
            };

            account.Dependents.Add(dependent);
            DB.SaveChanges();

            return RedirectToAction("Index", new { accountId });
        }

        /// <summary>
        /// Removes a dependent from an account.
        /// </summary>
        [HttpPost]
        public ActionResult Delete(int accountId, int id)
        {
            var account = DB.Accounts
                .Include(x => x.Dependents)
                .Single(x => x.Id == accountId);

            account.Dependents.RemoveAll(x => x.Id == id);
            DB.SaveChanges();

            return RedirectToAction("Index", new { accountId });
        }
    }
}
