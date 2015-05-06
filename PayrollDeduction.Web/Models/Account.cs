using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PayrollDeduction.Web.Models
{
    /// <summary>
    /// Represents a payroll deducation Account for a team member. An Account
    /// keeps track balance, payment amount, and remaining pay periods.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Construct a new account object with defaults;
        /// </summary>
        public Account()
        {
            Balance = 0.0;
            PaymentAmount = 0.0;
            PayPeriods = 0;
            CreatedOn = DateTime.Now;
            Dependents = new List<AccountDependent>();
            Transactions = new List<AccountTransaction>();
        }

        /// <summary>
        /// Creates a new instance of Account with a TeamMember and
        /// starting balance.
        /// </summary>
        public Account(TeamMember teamMember, double startingBalance = 0.0) : base()
        {
            TeamMember = teamMember;
            Adjust(startingBalance, "Starting Balance");
        }

        [Key]
        public int Id { get; set; }

        [Column]
        public int TeamMemberId { get; set; }
        public TeamMember TeamMember { get; set; }

        [Column]
        public double Balance { get; set; }

        [Column]
        public double PaymentAmount { get; set; }

        [Column]
        public int PayPeriods { get; set; }

        [Column]
        public bool Active { get; set; }

        [Column]
        public DateTime CreatedOn { get; set; }

        public List<AccountDependent> Dependents { get; set; }

        public List<AccountTransaction> Transactions { get; set; }

        /// <summary>
        /// Calculates the number of payments remaining on the account.
        /// </summary>
        public int RemainingPayments
        {
            get { return Balance > 0 ? (int)Math.Ceiling(Balance / PaymentAmount) : 0; }
        }

        /// <summary>
        /// Makes an adjustment to the account's balance by creating a new
        /// transaction for the difference between the old and new balances.
        /// </summary>
        /// <param name="newBalance">
        /// The new balance that the account will be adjusted to.
        /// </param>
        /// <param name="note">
        /// (Optional) A note to include on the transaction that will be
        /// created to adjust the balance.
        /// </param>
        public void Adjust(double newBalance, string note = "Adjustment")
        {
            if (Balance == newBalance)
                return;

            if (Balance < newBalance)
                Debit(newBalance - Balance, note);
            else
                Credit(Balance - newBalance, note);

            Refresh();
        }

        /// <summary>
        /// Refreshes the Balance, PayPeriods, and PaymentAmount properties.
        /// </summary>
        public void Refresh()
        {
            // Cache the old balance for later reference
            var oldBalance = Balance;

            // Calculate the new balance
            CalculateBalance();

            // Refresh the Payment Schedule
            CalculatePaymentSchedule(oldBalance, Balance);
        }

        /// <summary>
        /// Calculates a balance for the account by aggregating all transactions.
        /// </summary>
        public void CalculateBalance()
        {
            Balance = Transactions.OrderBy(x => x.Id).Sum(t => t.Amount);
        }

        /// <summary>
        /// Calculates a payment schedule for the account by aggregating all transactions.
        /// </summary>
        public void CalculatePaymentSchedule(double oldBalance, double newBalance)
        {
            if (newBalance <= 0)
            {
                PayPeriods = 0;
                PaymentAmount = 0.0;
            }
            else if (oldBalance >= newBalance && newBalance < PaymentAmount)
            {
                PaymentAmount = newBalance;
            }
            else if (oldBalance < newBalance && newBalance <= 300)
            {
                PaymentAmount = 12.50;
                PayPeriods = (int)Math.Ceiling((newBalance / PaymentAmount));
            }
            else if (oldBalance < newBalance && newBalance <= 999)
            {
                PayPeriods = 24;
                PaymentAmount = Math.Ceiling(newBalance / PayPeriods);
            }
            else if (oldBalance < newBalance && newBalance <= 1999)
            {
                PayPeriods = 36;
                PaymentAmount = Math.Ceiling(newBalance / PayPeriods);
            }
            else if (oldBalance < newBalance && newBalance <= 2999)
            {
                PayPeriods = 48;
                PaymentAmount = Math.Ceiling(newBalance / PayPeriods);
            }
            else if (oldBalance < newBalance && newBalance >= 3000)
            {
                PayPeriods = 72;
                PaymentAmount = Math.Ceiling(newBalance / PayPeriods);
            }
        }

        /// <summary>
        /// Creates a debit tranaction of the account of the given amount.
        /// </summary>
        /// <param name="amount">
        /// A double representing the amount of the debit.
        /// </param>
        /// <param name="note">
        /// A string representing a note for the transition.
        /// </param>
        public void Debit(double amount, string note = "Debit")
        {
            var transaction = new AccountTransaction()
            {
                Type = AccountTransactionType.Debit,
                Note = note,
                Amount = Math.Abs(amount)
            };

            Transactions = Transactions ?? new List<AccountTransaction>();
            Transactions.Add(transaction);
            Refresh();
        }

        /// <summary>
        /// Creates a credit tranaction of the account of the given amount.
        /// </summary>
        /// <param name="amount">
        /// A double representing the amount of the debit.
        /// </param>
        /// <param name="note">
        /// A string representing a note for the transition.
        /// </param>
        public void Credit(double amount, string note = "Credit")
        {
            var transaction = new AccountTransaction()
            {
                Type = AccountTransactionType.Credit,
                Note = note,
                Amount = Math.Abs(amount)
            };

            Transactions = Transactions ?? new List<AccountTransaction>();
            Transactions.Add(transaction);
            Refresh();
        }

        /// <summary>
        /// Marks an account as active.
        /// </summary>
        public void Activate()
        {
            Active = true;
        }

        /// <summary>
        /// Marks an account as inactive.
        /// </summary>
        public void Deactivate()
        {
            Active = false;
        }
    }
}