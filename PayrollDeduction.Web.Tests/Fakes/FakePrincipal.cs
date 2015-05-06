using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using Moq;
using PayrollDeduction.Web.Models;

namespace PayrollDeduction.Web.Tests.Fakes
{
    /// <summary>
    /// Creates a fake Payroll Principal for use while testing.
    /// </summary>
    public class FakePrincipal
    {
        /// <summary>
        /// Creates and anonymous user (i.e. not authenticated).
        /// </summary>
        public static PayrollPrincipal GetAnonymous()
        {
            var identity = new Mock<IIdentity>();
            identity.Setup(x => x.IsAuthenticated).Returns(false);
            identity.Setup(x => x.Name).Returns("Anonymous");
            identity.Setup(x => x.AuthenticationType).Returns("None");

            return new PayrollPrincipal(identity.Object, new TeamMember(), null);
        }

        /// <summary>
        /// Creates user and associates it with a TeamMember.
        /// </summary>
        public static PayrollPrincipal Create(string username, bool isAdmin = false)
        {
            var teamMember = new TeamMember()
            {
                NetworkId = username,
                TeamMemberId = username,
                FirstName = "Test",
                LastName = "Member",
                CostCenter = "1000",
                JobCode = "1000",
                HiredOn = new DateTime(2010, 1, 1),
            };

            var identity = new GenericIdentity(username);
            var roles = (isAdmin ? new[] { "Admin" } : new string[] {});
            return new PayrollPrincipal(identity, teamMember, roles);
        }
    }
}
