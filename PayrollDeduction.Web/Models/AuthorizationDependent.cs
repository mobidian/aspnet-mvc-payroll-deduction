using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PayrollDeduction.Web.Models
{
    /// <summary>
    /// Represents a dependent (i.e. spouse or child) associated with an 
    /// Authorization.
    /// </summary>
    public class AuthorizationDependent : Dependent
    {
        [Column]
        public int? AuthorizationId { get; set; }
        public Authorization Authorization { get; set; }
    }
}