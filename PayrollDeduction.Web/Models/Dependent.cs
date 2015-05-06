using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PayrollDeduction.Web.Models
{
    /// <summary>
    /// Represents a dependent (i.e. spouse or child) associated with an 
    /// Account or Authorization.
    /// </summary>
    public class Dependent
    {
        public Dependent()
        {
            CreatedOn = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Column]
        public string LastName { get; set; }

        [Column]
        public string FirstName { get; set; }

        [Column]
        public DateTime BirthDate { get; set; }

        [Column]
        public DateTime CreatedOn { get; set; }

        public string FullName
        {
            get { return String.Join(" ", FirstName, LastName); }
        }
    }
}