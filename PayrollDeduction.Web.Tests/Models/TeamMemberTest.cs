using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayrollDeduction.Web.Models;

namespace PayrollDeduction.Web.Tests.Models
{
    [TestClass]
    public class TeamMemberTest
    {
        [TestMethod]
        public void FullName_ShouldGetFirstAndLastName()
        {
            var teamMember = new TeamMember()
            {
                FirstName = "John",
                LastName = "Smith"
            };

            Assert.AreEqual("John Smith", teamMember.FullName);
        }
    }
}