using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PayrollDeduction.Web.Models
{
    /// <summary>
    /// Represents an employee of the organization.
    /// </summary>
    public class TeamMember
    {
        [Key]
        public int Id { get; set; }

        [Column]
        public string TeamMemberId { get; set; }

        [Column]
        public string NetworkId { get; set; }

        [Column]
        public string LastName { get; set; }

        [Column]
        public string FirstName { get; set; }

        [Column]
        public string CostCenter { get; set; }

        [Column]
        public string JobCode { get; set; }

        [Column]
        public DateTime HiredOn { get; set; }

        [Column]
        public DateTime? TerminatedOn { get; set; }

        public string FullName
        {
            get { return String.Join(" ", FirstName, LastName); }
        }
    }
}
