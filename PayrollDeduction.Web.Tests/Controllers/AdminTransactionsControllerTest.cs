using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PayrollDeduction.Web.Controllers;
using PayrollDeduction.Web.Models;
using PayrollDeduction.Web.Tests.Fakes;

namespace PayrollDeduction.Web.Tests.Controllers
{
    [TestClass]
    public class AdminTransactionsControllerTest
    {
        private AdminTransactionsController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _controller = new AdminTransactionsController();
            _controller.ControllerContext = Mock.Of<ControllerContext>();
            _controller.DB = new PayrollContext();
            _controller.User = FakePrincipal.Create("josh");
        }

        [TestMethod]
        public void Index_Get_ShouldListAccountTransactions()
        {
            Mock.Get(_controller.ControllerContext)
                .Setup(x => x.HttpContext.Request["X-Requested-With"])
                .Returns("");

            var result = (ViewResult)_controller.Index(1);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Account));

            var account = (Account)result.ViewData.Model;
            Assert.AreEqual(1, account.Transactions.Count());
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void Index_Get_WithAjax_ShouldListAccountTransactions()
        {
            Mock.Get(_controller.ControllerContext)
                .Setup(x => x.HttpContext.Request["X-Requested-With"])
                .Returns("XMLHttpRequest");

            var result = (PartialViewResult)_controller.Index(1);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Account));

            var account = (Account)result.ViewData.Model;
            Assert.AreEqual(1, account.Transactions.Count());
            Assert.AreEqual("_Transactions", result.ViewName);
        }

        [TestMethod]
        public void Index_Get_WithoutTransactions_ShouldReturnEmptyView()
        {
            _controller.User = FakePrincipal.Create("john");

            Mock.Get(_controller.ControllerContext)
                .Setup(x => x.HttpContext.Request["X-Requested-With"])
                .Returns("");

            var result = (ViewResult)_controller.Index(2);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Account));

            var account = (Account)result.ViewData.Model;
            Assert.IsFalse(account.Transactions.Any());
            Assert.AreEqual("Empty", result.ViewName);
        }

        [TestMethod]
        public void Create_Post_ShouldRedirectToIndex()
        {
            var transaction = new AccountTransaction()
            {
                Type = AccountTransactionType.Debit,
                Amount = 1000.00,
                Note = "Starting Balance"
            };

            var result = (RedirectToRouteResult)_controller.Create(1, transaction);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(1, result.RouteValues["accountId"]);
        }

        [TestMethod]
        public void Create_Post_WithErrors_ShouldRenderDefaultView()
        {
            _controller.ModelState.AddModelError("", "Invalid");
            var transaction = new AccountTransaction();
            var result = (ViewResult)_controller.Create(1, transaction);
            Assert.AreEqual("Index", result.ViewName);
        }
    }
}
