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

namespace PayrollDeduction.Web.Tests.Controllers
{
    [TestClass]
    public class NotificationsControllerTest
    {
        private NotificationsController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _controller = new NotificationsController();
            _controller.ControllerContext = Mock.Of<ControllerContext>();
            _controller.DB = new PayrollContext();
            _controller.AccountMailer = Mock.Of<IAccountMailer>();
        }

        [TestMethod]
        public void Send_Get_ShouldDeliverEmailNotifications()
        {
            _controller.Send();

            Mock.Get<IAccountMailer>(_controller.AccountMailer)
                .Verify(x => x.SendAccountBalance(It.IsAny<Account>()));
        }
    }
}