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
    /// The NotificationsController is responsible for sending email notifications
    /// to users. The intent is that the send URL will be requested periodically
    /// by a scheduled task.
    /// </summary>
    public class NotificationsController : BaseController
    {
        public IAccountMailer AccountMailer { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (AccountMailer == null)
                AccountMailer = new AccountMailer();

            base.Initialize(requestContext);
        }

        /// <summary>
        /// Sends account balances to all active account holders.
        /// </summary>
        [HttpGet]
        public void Send()
        {
            var accounts = DB.Accounts
                .Include(x => x.TeamMember)
                .Where(x => x.Active)
                .ToList();

            accounts.ForEach(x => AccountMailer.SendAccountBalance(x));
        }
    }
}