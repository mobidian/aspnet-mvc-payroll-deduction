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
    public class AccountTransactionsControllerTest
    {
        private AccountTransactionsController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _controller = new AccountTransactionsController();
            _controller.ControllerContext = Mock.Of<ControllerContext>();
            _controller.DB = new PayrollContext();
            _controller.User = FakePrincipal.Create("josh");
        }

        [TestMethod]
        public void Index_Get_ShouldListTransactionsForCurrentUser()
        {
            Mock.Get(_controller.ControllerContext)
                .Setup(x => x.HttpContext.Request["X-Requested-With"])
                .Returns("");

            var result = (ViewResult)_controller.Index();
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Account));

            var account = (Account)result.ViewData.Model;
            Assert.AreEqual(1, account.Transactions.Count());
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void Index_Get_WithAjax_ShouldListTransactionsForCurrentUser()
        {
            Mock.Get(_controller.ControllerContext)
                .Setup(x => x.HttpContext.Request["X-Requested-With"])
                .Returns("XMLHttpRequest");

            var result = (PartialViewResult)_controller.Index();
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

            var result = (ViewResult)_controller.Index(page: 100);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Account));

            var account = (Account)result.ViewData.Model;
            Assert.IsFalse(account.Transactions.Any());
            Assert.AreEqual("Empty", result.ViewName);
        }
    }
}