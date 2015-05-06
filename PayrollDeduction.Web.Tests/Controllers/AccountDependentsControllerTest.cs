using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PayrollDeduction.Web.Controllers;
using PayrollDeduction.Web.Models;
using PayrollDeduction.Web.Services;
using PayrollDeduction.Web.ViewModels;
using PayrollDeduction.Web.Tests.Fakes;

namespace PayrollDeduction.Web.Tests.Controllers
{
    [TestClass]
    public class AccountDependentsControllerTest
    {
        private AccountDependentsController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _controller = new AccountDependentsController();
            _controller.DB = new PayrollContext();
        }

        [TestMethod]
        public void Index_Get_ShouldListDependentsForCurrentUser()
        {
            _controller.User = FakePrincipal.Create("josh");
            var result = (ViewResult)_controller.Index();
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Account));

            var account = (Account)result.ViewData.Model;
            Assert.AreEqual(1, account.Dependents.Count());
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void Index_Get_WithoutDependents_ShouldReturnEmptyView()
        {
            _controller.User = FakePrincipal.Create("jane");
            var result = (ViewResult)_controller.Index();
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Account));

            var account = (Account)result.ViewData.Model;
            Assert.AreEqual(0, account.Dependents.Count());
            Assert.AreEqual("Empty", result.ViewName);
        }
    }
}
