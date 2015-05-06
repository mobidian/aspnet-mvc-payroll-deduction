using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayrollDeduction.Web.Models
{
    /// <summary>
    /// Demo only - used as simple storage for application users.
    /// </summary>
    public class UserStore
    {
        public static IEnumerable<User> All()
        {
            yield return new User { Username = "josh", Password = "demo", IsAdmin = true };
            yield return new User { Username = "john", Password = "demo", IsAdmin = false };
            yield return new User { Username = "jane", Password = "demo", IsAdmin = false };
            yield return new User { Username = "jackie", Password = "demo", IsAdmin = false };
        }
    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }
}