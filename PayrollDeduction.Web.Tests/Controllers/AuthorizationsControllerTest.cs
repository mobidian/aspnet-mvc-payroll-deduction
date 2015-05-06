using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayrollDeduction.Web.Controllers;
using PayrollDeduction.Web.Models;
using PayrollDeduction.Web.ViewModels;
using PayrollDeduction.Web.Tests.Fakes;

namespace PayrollDeduction.Web.Tests.Controllers
{
    [TestClass]
    public class AuthorizationsControllerTest
    {
        private AuthorizationsController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _controller = new AuthorizationsController();
            _controller.DB = new PayrollContext();
            _controller.User = FakePrincipal.GetAnonymous();
        }

        [TestMethod]
        public void Create_Get_ShouldReturnDefaultView()
        {
            var result = (ViewResult)_controller.Create();
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(AuthorizationFormModel));
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void Create_Post_WithAnonymousUser_ShouldRedirectToRoot()
        {
            var viewModel = new AuthorizationFormModel() { TeamMemberId = "josh" };
            var result = (RedirectToRouteResult)_controller.Create(viewModel);
            Assert.IsNotNull(_controller.FlashSuccess());
            Assert.AreEqual("Root", result.RouteName);
        }

        [TestMethod]
        public void Create_Post_WithAuthenticatedUser_ShouldRedirectToAccountHome()
        {
            var user = FakePrincipal.Create("josh");
            _controller.User = user;

            var viewModel = new AuthorizationFormModel() { TeamMemberId = user.TeamMemberId };
            var result = (RedirectToRouteResult)_controller.Create(viewModel);
            Assert.IsNotNull(_controller.FlashSuccess());
            Assert.AreEqual("Account_Home", result.RouteName);
        }

        [TestMethod]
        public void Create_Post_WithErrors_ShouldRenderDefaultView()
        {
            _controller.ModelState.AddModelError("", "Invalid");
            var viewModel = new AuthorizationFormModel();
            var result = (ViewResult)_controller.Create(viewModel);
            Assert.IsNull(_controller.FlashSuccess());
            Assert.AreEqual("", result.ViewName);
        }
    }
}
