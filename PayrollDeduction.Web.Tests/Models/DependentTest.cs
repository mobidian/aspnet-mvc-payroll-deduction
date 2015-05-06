using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayrollDeduction.Web.Models;

namespace PayrollDeduction.Web.Tests.Models
{
    [TestClass]
    public class DependentTest
    {
        [TestMethod]
        public void FullName_ShouldGetFirstAndLastName()
        {
            var dependent = new Dependent()
            {
                FirstName = "John",
                LastName = "Smith"
            };

            Assert.AreEqual("John Smith", dependent.FullName);
        }
    }
}