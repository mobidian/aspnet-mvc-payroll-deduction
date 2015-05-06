using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PayrollDeduction.Web.ViewModels
{
    public class AuthorizationFormModel
    {
        public AuthorizationFormModel()
        {
            Dependents = new List<DependentFormModel>();
        }

        [Required]
        [Display(Name="Team Member ID")]
        public string TeamMemberId { get; set; }

        public List<DependentFormModel> Dependents { get; set; }
    }

    public class DependentFormModel
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name="First Name")]
        public string FirstName { get; set; }

        [Display(Name="Last Name")]
        public string LastName { get; set; }

        [Display(Name="Date of Birth")]
        public DateTime? BirthDate { get; set; }
    }
}