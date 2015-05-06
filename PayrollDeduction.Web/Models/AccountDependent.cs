using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PayrollDeduction.Web.Models
{
    /// <summary>
    /// Represents a dependent (i.e. spouse or child) associated with an 
    /// Account.
    /// </summary>
    public class AccountDependent : Dependent
    {
        [Column]
        public int? AccountId { get; set; }
        public Account Account { get; set; }
    }
}