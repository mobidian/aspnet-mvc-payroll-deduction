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

namespace PayrollDeduction.Web.Tests.Controllers
{
    [TestClass]
    public class SessionControllerTest
    {
        private SessionController _controller;

        [TestInitialize]
        public void Initialize()
        {
            var userService = new Mock<IUserService>();
            userService.Setup(x => x.Authenticate("josh", "goodpassword")).Returns(true);
            userService.Setup(x => x.Authenticate("josh", "badpassword")).Returns(false);

            _controller = new SessionController();
            _controller.DB = new PayrollContext();
            _controller.UserService = userService.Object;
        }

        [TestMethod]
        public void Create_Get_ShouldRenderDefaultView()
        {
            var result = (ViewResult)_controller.Create();
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(SessionFormModel));
            Assert.AreEqual(String.Empty, result.ViewName);
        }

        [TestMethod]
        public void Create_Post_WithUser_ShouldRedirectToAccountHome()
        {
            Mock.Get(_controller.UserService)
                .Setup(x => x.IsAdmin(It.IsAny<String>()))
                .Returns(false);

            var formModel = new SessionFormModel()
            {
                UserName = "josh",
                Password = "goodpassword"
            };

            var result = (RedirectToRouteResult)_controller.Create(formModel);
            Assert.AreEqual("Account_Home", result.RouteName);
        }

        [TestMethod]
        public void Create_Post_WithAdmin_ShouldRedirectToAdminHome()
        {
            Mock.Get(_controller.UserService)
                .Setup(x => x.IsAdmin(It.IsAny<String>()))
                .Returns(true);

            var formModel = new SessionFormModel()
            {
                UserName = "josh",
                Password = "goodpassword"
            };

            var result = (RedirectToRouteResult)_controller.Create(formModel);
            Assert.AreEqual("Admin_Home", result.RouteName);
        }

        [TestMethod]
        public void Create_Post_WithInvalidCredentials_ShouldRenderDefaultView()
        {
            var formModel = new SessionFormModel()
            {
                UserName = "josh",
                Password = "badpassword"
            };

            var result = (ViewResult)_controller.Create(formModel);
            Assert.IsFalse(_controller.ModelState.IsValid);
            Assert.AreEqual(String.Empty, result.ViewName);
        }

        [TestMethod]
        public void Delete_Post_ShouldRedirectToRoot()
        {
            var result = (RedirectToRouteResult)_controller.Delete();
            Assert.AreEqual("Root", result.RouteName);
        }
    }
}
