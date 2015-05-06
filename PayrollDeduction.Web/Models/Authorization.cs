using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PayrollDeduction.Web.Models
{
    /// <summary>
    /// Represents an authorization which is used to request a new payroll
    /// deduction account or modify the dependents on an existing one.
    /// </summary>
    public class Authorization
    {
        public Authorization()
        {
            Dependents = new List<AuthorizationDependent>();
            CreatedOn = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Column]
        public int TeamMemberId { get; set; }
        public TeamMember TeamMember { get; set; }

        [Column]
        public bool Archived { get; set; }

        [Column]
        public DateTime CreatedOn { get; set; }

        public List<AuthorizationDependent> Dependents { get; set; }

        /// <summary>
        /// Mark the authorization as Archived.
        /// </summary>
        public void Archive()
        {
            Archived = true;
        }

        /// <summary>
        /// Restore an archived authorization.
        /// </summary>
        public void Restore()
        {
            Archived = false;
        }
    }
}
