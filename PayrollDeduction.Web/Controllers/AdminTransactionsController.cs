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
    /// The AdminTransactionsController is responsible for allowing the
    /// administrator to manage transactions on an account.
    /// </summary>
    [Authorize(Roles="Admin")]
    public class AdminTransactionsController : BaseController
    {
        /// <summary>
        /// List all transactions on an account.
        /// </summary>
        [HttpGet]
        public ActionResult Index(int accountId, int page = 0)
        {
            var account = DB.Accounts.Find(accountId);

            DB.Entry(account)
                .Reference(x => x.TeamMember)
                .Load();

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
                return View("Index", account);
            else
                return View("Empty", account);
        }

        /// <summary>
        /// Creates a transaction on an account.
        /// </summary>
        [HttpPost]
        public ActionResult Create(int accountId, AccountTransaction transaction)
        {
            var account = DB.Accounts
                .Include(x => x.TeamMember)
                .Include(x => x.Dependents)
                .Include(x => x.Transactions)
                .Single(x => x.Id == accountId);

            if (!ModelState.IsValid)
                return View("Index", account);

            if (transaction.Type == AccountTransactionType.Debit)
                account.Debit(transaction.Amount, transaction.Note);
            else
                account.Credit(transaction.Amount, transaction.Note);

            DB.SaveChanges();
            return RedirectToAction("Index", new { accountId });
        }
    }
}