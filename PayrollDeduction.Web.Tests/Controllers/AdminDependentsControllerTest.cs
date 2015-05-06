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
    public class AdminDependentsControllerTest
    {
        private AdminDependentsController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _controller = new AdminDependentsController();
            _controller.DB = new PayrollContext();
            _controller.User = FakePrincipal.Create("josh");
        }

        [TestMethod]
        public void Index_Get_WithDependents_ShouldListDependentsForAccount()
        {
            var result = (ViewResult)_controller.Index(accountId: 2);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Account));

            var account = (Account)result.ViewData.Model;
            Assert.AreEqual(1, account.Dependents.Count());
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void Index_Get_WithoutDependents_ShouldRenderEmptyView()
        {
            var result = (ViewResult)_controller.Index(accountId: 3);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Account));

            var account = (Account)result.ViewData.Model;
            Assert.AreEqual(0, account.Dependents.Count());
            Assert.AreEqual("Empty", result.ViewName);
        }

        [TestMethod]
        public void Create_Post_WithoutErrors_ShouldRedirectToIndex()
        {
            var dependent = new DependentFormModel()
            {
                FirstName = "Christina",
                LastName = "O'Rourke",
                BirthDate = DateTime.Now
            };

            var result = (RedirectToRouteResult)_controller.Create(accountId: 1, formModel: dependent);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(1, result.RouteValues["accountId"]);
        }

        [TestMethod]
        public void Create_Post_WithErrors_ShouldRenderDefaultView()
        {
            _controller.ModelState.AddModelError("", "Invalid");
            var result = (ViewResult)_controller.Create(accountId: 1, formModel: new DependentFormModel());
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void Delete_Post_ShouldRedirectToIndex()
        {
            var result = (RedirectToRouteResult)_controller.Delete(accountId: 1, id: 1);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(1, result.RouteValues["accountId"]);
        }
    }
}
