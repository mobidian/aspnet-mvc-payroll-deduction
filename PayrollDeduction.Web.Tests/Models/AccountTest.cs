using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayrollDeduction.Web.Models;

namespace PayrollDeduction.Web.Tests.Models
{
    [TestClass]
    public class AccountTest
    {
        [TestMethod]
        public void Constructor_ShouldCreateAccountWithDefaults()
        {
            var account = new Account();
            Assert.AreEqual(0.0, account.Balance);
            Assert.AreEqual(0.0, account.PaymentAmount);
            Assert.AreEqual(0, account.PayPeriods);
            Assert.AreEqual(DateTime.Today, account.CreatedOn.Date);
            Assert.IsFalse(account.Dependents.Any());
            Assert.IsFalse(account.Transactions.Any());
        }

        [TestMethod]
        public void Constructor_ShouldCreateAccountWithTeamMemberAndBalance()
        {
            var teamMember = new TeamMember()
            {
                TeamMemberId = "josh"
            };

            var account = new Account(teamMember, 1000.00);
            Assert.AreEqual("josh", teamMember.TeamMemberId);
            Assert.AreEqual(1000.00, account.Balance);
            Assert.AreEqual(1, account.Transactions.Count);
        }

        [TestMethod]
        public void Adjust_WithLargerBalance_ShouldIncreaseBalance()
        {
            var account = new Account(null, 1000.00);
            account.Adjust(1500.00);
            Assert.AreEqual(2, account.Transactions.Count);
            Assert.AreEqual(1500.00, account.Balance);
        }

        [TestMethod]
        public void Adjust_WithSmallerBalance_ShouldDecreaseBalance()
        {
            var account = new Account(null, 1000.00);
            account.Adjust(500.00);
            Assert.AreEqual(2, account.Transactions.Count);
            Assert.AreEqual(500.00, account.Balance);
        }

        [TestMethod]
        public void Adjust_WithSameBalance_ShouldDoNothing()
        {
            var account = new Account(null, 1000.00);
            account.Adjust(1000.00);
            Assert.AreEqual(1, account.Transactions.Count);
            Assert.AreEqual(1000.00, account.Balance);
        }

        [TestMethod]
        public void Refresh_ShouldSetBalanceAndPaymentSchedule()
        {
            var account = new Account(null);
            Assert.AreEqual(0, account.Balance);
            Assert.AreEqual(0, account.PaymentAmount);
            Assert.AreEqual(0, account.PayPeriods);
            account.Debit(100.00);
            account.Refresh();
            Assert.AreEqual(100.00, account.Balance);
            Assert.AreEqual(12.50, account.PaymentAmount);
            Assert.AreEqual(8, account.PayPeriods);
            Assert.AreEqual(8, account.RemainingPayments);
        }

        [TestMethod]
        public void CalculateBalance_ShouldSumTransactionsAndSetBalance()
        {
            var account = new Account(null, 1000.00);
            account.Debit(100.00);
            account.CalculateBalance();
            Assert.AreEqual(1100.00, account.Balance);
        }

        [TestMethod]
        public void CalculatePaymentSchedule_ShouldSetPayPeriodsAndPaymentAmount()
        {
            var tests = new[] {
                new { Balance = 0.00, PaymentAmount = 0.00, PayPeriods = 0, RemainingPayments = 0 },
                new { Balance = 100.00, PaymentAmount = 12.5, PayPeriods = 8, RemainingPayments = 8 },
                new { Balance = 500.00, PaymentAmount = 21.0, PayPeriods = 24, RemainingPayments = 24 },
                new { Balance = 1500.00, PaymentAmount = 42.0, PayPeriods = 36, RemainingPayments = 36 },
                new { Balance = 2500.00, PaymentAmount = 53.0, PayPeriods = 48, RemainingPayments = 48 },
                new { Balance = 3500.00, PaymentAmount = 49.0, PayPeriods = 72, RemainingPayments = 72 }
            };

            foreach (var test in tests)
            {
                var account = new Account(null, test.Balance);
                account.CalculatePaymentSchedule(0.00, test.Balance);
                Assert.AreEqual(test.Balance, account.Balance);
                Assert.AreEqual(test.PaymentAmount, account.PaymentAmount);
                Assert.AreEqual(test.PayPeriods, account.PayPeriods);
                Assert.AreEqual(test.RemainingPayments, account.RemainingPayments);
            }
        }

        [TestMethod]
        public void Activate_ShouldMakeAnAccountActive()
        {
            var account = new Account() { Active = false };
            account.Activate();
            Assert.IsTrue(account.Active);
        }

        [TestMethod]
        public void Deactivate_ShouldMakeAnAccountInactive()
        {
            var account = new Account() { Active = true };
            account.Deactivate();
            Assert.IsFalse(account.Active);
        }
    }
}