using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Security;

namespace PayrollDeduction.Web.Models
{
    /// <summary>
    /// Represents a user of the application with access to additional
    /// team member information not avaialble to a typical UserPrincipal.
    /// </summary>
    public class PayrollPrincipal : IPrincipal
    {
        private IIdentity _identity;
        private TeamMember _teamMember;
        private string[] _roles;

        public PayrollPrincipal(IIdentity identity) 
        {
            _identity = identity;
            _teamMember = GetTeamMember();
            _roles = GetRoles();
        }

        public PayrollPrincipal(IIdentity identity, TeamMember teamMember)
        {
            _identity = identity;
            _teamMember = teamMember;
        }

        public PayrollPrincipal(IIdentity identity, TeamMember teamMember, string[] roles)
        {
            _identity = identity;
            _teamMember = teamMember;
            _roles = roles ?? new string[] {};
        }

        public IIdentity Identity
        {
            get { return _identity; }
        }

        public string TeamMemberId
        {
            get { return _teamMember.TeamMemberId; }
        }

        public string FirstName
        {
            get { return _teamMember.FirstName; }
        }

        public string LastName
        {
            get { return _teamMember.LastName; }
        }

        public string FullName
        {
            get { return _teamMember.FullName; }
        }

        public bool IsAdmin
        {
            get { return IsInRole("Admin"); }
        }

        public bool IsInRole(string role)
        {
            return _roles.Contains(role);
        }

        private string[] GetRoles()
        {
            return Roles.GetRolesForUser(Identity.Name);
        }

        private TeamMember GetTeamMember()
        {
            using (var db = new PayrollContext())
            {
                return db.TeamMembers.Single(x => x.NetworkId == Identity.Name);
            }
        }
    }
}