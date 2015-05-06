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
    /// The AdminAuthorizationsController allows administrators to manage 
    /// team member accounts.
    /// </summary>
    [Authorize(Roles="Admin")]
    public class AdminAccountsController : BaseController
    {
        /// <summary>
        /// List all team member accounts
        /// </summary>
        [HttpGet]
        public ActionResult Index(int page = 0)
        {
            var accounts = DB.Accounts
                .Include(x => x.TeamMember)
                .OrderBy(x => x.TeamMember.LastName)
                .ThenBy(x => x.TeamMember.FirstName)
                .Skip(page * 50)
                .Take(50)
                .ToList();

            if (Request.IsAjaxRequest())
                return PartialView("_Accounts", accounts);
            else
                return View(accounts);
        }

        /// <summary>
        /// Search team member accounts by team member ID or last name.
        /// </summary>
        [HttpGet]
        public ActionResult Search(string query, int page = 0)
        {
            var accounts = DB.Accounts
                .Include(x => x.TeamMember)
                .Where(x => x.TeamMember.TeamMemberId == query || x.TeamMember.LastName.Contains(query))
                .OrderBy(x => x.TeamMember.LastName)
                .ThenBy(x => x.TeamMember.FirstName)
                .Skip(page * 50)
                .Take(50)
                .ToList();

            if (Request.IsAjaxRequest())
                return PartialView("_Accounts", accounts);
            else
                return View("Index", accounts);
        }

        /// <summary>
        /// Marks an account as active.
        /// </summary>
        [HttpPost]
        public ActionResult Activate(int id)
        {
            var account = DB.Accounts.Find(id);
            account.Activate();
            DB.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Marks an account as inactive.
        /// </summary>
        [HttpPost]
        public ActionResult Deactivate(int id)
        {
            var account = DB.Accounts.Find(id);
            account.Deactivate();
            DB.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}