using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PayrollDeduction.Web.Controllers;
using PayrollDeduction.Web.Mailers;
using PayrollDeduction.Web.Models;
using PayrollDeduction.Web.Tests.Fakes;

namespace PayrollDeduction.Web.Tests.Controllers
{
    [TestClass]
    public class AdminAuthorizationsControllerTest
    {
        private AdminAuthorizationsController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _controller = new AdminAuthorizationsController();
            _controller.ControllerContext = Mock.Of<ControllerContext>();
            _controller.DB = new PayrollContext();
            _controller.User = FakePrincipal.Create("josh");
            _controller.AccountMailer = Mock.Of<IAccountMailer>();
        }

        [TestMethod]
        public void Index_Get_ShouldListAuthorizations()
        {
            Mock.Get(_controller.ControllerContext)
                .Setup(x => x.HttpContext.Request["X-Requested-With"])
                .Returns("");

            var result = (ViewResult)_controller.Index();
            Assert.AreEqual(4, ((IEnumerable<Authorization>)result.ViewData.Model).Count());
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void Index_Get_WithAjax_ShouldListAuthorizations()
        {
            Mock.Get(_controller.ControllerContext)
                .Setup(x => x.HttpContext.Request["X-Requested-With"])
                .Returns("XMLHttpRequest");

            var result = (PartialViewResult)_controller.Index();
            Assert.AreEqual(4, ((IEnumerable<Authorization>)result.ViewData.Model).Count());
            Assert.AreEqual("_Authorizations", result.ViewName);
        }

        [TestMethod]
        public void Search_Get_ShouldFindAuthorizations()
        {
            Mock.Get(_controller.ControllerContext)
                .Setup(x => x.HttpContext.Request["X-Requested-With"])
                .Returns("");

            var result = (ViewResult)_controller.Search("O'Rourke");
            Assert.AreEqual(1, ((IEnumerable<Authorization>)result.ViewData.Model).Count());
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void Search_Get_WithAjax_ShouldFindAuthorizations()
        {
            Mock.Get(_controller.ControllerContext)
                .Setup(x => x.HttpContext.Request["X-Requested-With"])
                .Returns("XMLHttpRequest");

            var result = (PartialViewResult)_controller.Index();
            Assert.AreEqual(4, ((IEnumerable<Authorization>)result.ViewData.Model).Count());
            Assert.AreEqual("_Authorizations", result.ViewName);
        }

        [TestMethod]
        public void Details_Get_ShouldShowAnAuthorization()
        {
            var result = (ViewResult)_controller.Details(1);
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Authorization));
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void Complete_Post_ShouldRedirectToIndex()
        {
            var result = (RedirectToRouteResult)_controller.Complete(1);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Delete_Post_ShouldRedirectToIndex()
        {
            var result = (RedirectToRouteResult)_controller.Delete(1);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }
    }
}