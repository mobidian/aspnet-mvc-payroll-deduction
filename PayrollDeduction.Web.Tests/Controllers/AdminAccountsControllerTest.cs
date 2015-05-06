using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PayrollDeduction.Web.Controllers;
using PayrollDeduction.Web.Models;
using PayrollDeduction.Web.ViewModels;
using PayrollDeduction.Web.Tests.Fakes;

namespace PayrollDeduction.Web.Tests.Controllers
{
    [TestClass]
    public class AdminAccountsControllerTest
    {
        private AdminAccountsController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _controller = new AdminAccountsController();
            _controller.ControllerContext = Mock.Of<ControllerContext>();
            _controller.DB = new PayrollContext();
            _controller.User = FakePrincipal.Create("josh");
        }

        [TestMethod]
        public void Index_Get_ShouldListAccounts()
        {
            Mock.Get(_controller.ControllerContext)
                .Setup(x => x.HttpContext.Request["X-Requested-With"])
                .Returns("");

            var result = (ViewResult)_controller.Index();
            Assert.AreEqual(3, ((IEnumerable<Account>)result.ViewData.Model).Count());
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void Index_Get_WithAjax_ShouldListAccounts()
        {
            Mock.Get(_controller.ControllerContext)
                .Setup(x => x.HttpContext.Request["X-Requested-With"])
                .Returns("XMLHttpRequest");

            var result = (PartialViewResult)_controller.Index();
            Assert.AreEqual(3, ((IEnumerable<Account>)result.ViewData.Model).Count());
            Assert.AreEqual("_Accounts", result.ViewName);
        }

        [TestMethod]
        public void Search_Get_ShouldSearchAccounts()
        {
            Mock.Get(_controller.ControllerContext)
                .Setup(x => x.HttpContext.Request["X-Requested-With"])
                .Returns("");

            var result = (ViewResult)_controller.Search("O'Rourke");
            Assert.AreEqual(1, ((IEnumerable<Account>)result.ViewData.Model).Count());
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void Search_Get_WithAjax_ShouldSearchAccounts()
        {
            Mock.Get(_controller.ControllerContext)
                .Setup(x => x.HttpContext.Request["X-Requested-With"])
                .Returns("XMLHttpRequest");

            var result = (PartialViewResult)_controller.Search("O'Rourke");
            Assert.AreEqual(1, ((IEnumerable<Account>)result.ViewData.Model).Count());
            Assert.AreEqual("_Accounts", result.ViewName);
        }

        [TestMethod]
        public void Activate_Post_ShouldMarkAccountAsActiveAndRedirectToIndex()
        {
            Mock.Get(_controller.ControllerContext)
                .Setup(x => x.HttpContext.Request["X-Requested-With"])
                .Returns("XMLHttpRequest");

            var result = (RedirectToRouteResult)_controller.Activate(1);
            Assert.AreEqual("Index", result.RouteValues["action"]);

            var account = _controller.DB.Accounts.Find(1);
            Assert.IsTrue(account.Active);
        }

        [TestMethod]
        public void Deactivate_Post_ShouldMarkAccountAsInactiveAndRedirectToIndex()
        {
            Mock.Get(_controller.ControllerContext)
                .Setup(x => x.HttpContext.Request["X-Requested-With"])
                .Returns("XMLHttpRequest");

            var result = (RedirectToRouteResult)_controller.Deactivate(1);
            Assert.AreEqual("Index", result.RouteValues["action"]);

            var account = _controller.DB.Accounts.Find(1);
            Assert.IsFalse(account.Active);
        }
    }
}
