using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web.Security;
using PayrollDeduction.Web.Models;
using Mvc.Mailer;

namespace PayrollDeduction.Web.Mailers
{
    public interface IAccountMailer
    {
        void SendAccountBalance(Account account);
        void SendAccountCreated(Account account);
        void SendDependentsAdded(Account account);
    }

    public class AccountMailer : MailerBase, IAccountMailer
    {
        /// <summary>
        /// Email address that will appear as the sender on notifications.
        /// </summary>
        public const string DefaultFromAddress = "noreply@example.com";

        /// <summary>
        /// Sends an email notification to the owner of a given account
        /// with information regarding account balance and payments.
        /// </summary>
        public void SendAccountBalance(Account account)
        {
            string recipient;

            if (TryGetRecipient(account, out recipient))
            {
                var msg = new MailMessage(
                    DefaultFromAddress,
                    recipient,
                    "Payroll Deduction: Account Balance",
                    String.Empty);

                ViewBag.FirstName = account.TeamMember.FirstName;
                ViewBag.Balance = account.Balance;
                ViewBag.PaymentAmount = account.PaymentAmount;
                ViewBag.RemainingPayments = account.RemainingPayments;
                PopulateBody(msg, viewName: "AccountBalance");
                msg.Send();
            }
        }

        /// <summary>
        /// Sends an email notification to the owner of a given account
        /// with information regarding account creation.
        /// </summary>
        public void SendAccountCreated(Account account)
        {
            string recipient;

            if (TryGetRecipient(account, out recipient))
            {
                var msg = new MailMessage(
                    DefaultFromAddress,
                    recipient,
                    "Payroll Deduction: Account Created",
                    String.Empty);

                ViewBag.FirstName = account.TeamMember.FirstName;
                PopulateBody(msg, viewName: "DependentsAdded");
                msg.Send();
            }
        }

        /// <summary>
        /// Sends an email notification to the owner of a given account
        /// with information regarding changes to dependents.
        /// </summary>
        public void SendDependentsAdded(Account account)
        {
            string recipient;

            if (TryGetRecipient(account, out recipient))
            {
                var msg = new MailMessage(
                    DefaultFromAddress,
                    recipient,
                    "Payroll Deduction: Dependents Added",
                    String.Empty);

                ViewBag.FirstName = account.TeamMember.FirstName;
                PopulateBody(msg, viewName: "DependentsAdded");
                msg.Send();
            }
        }

        // Try to retrieve the account owner's email address
        private bool TryGetRecipient(Account account, out string recipient)
        {
            var user = Membership.GetUser(account.TeamMember.NetworkId);
            recipient = null;

            if (user != null && !String.IsNullOrEmpty(user.Email))
                recipient = user.Email;

            return !String.IsNullOrEmpty(recipient);
        }
    }
}