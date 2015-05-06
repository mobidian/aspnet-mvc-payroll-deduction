using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayrollDeduction.Web.Models;

namespace PayrollDeduction.Web.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Represents the data context used to work with the database.
        /// </summary>
        public PayrollContext DB { get; set; }

        /// <summary>
        /// Represents the current user.
        /// </summary>
        private PayrollPrincipal _user;
        public new PayrollPrincipal User
        {
            get { return _user ?? (PayrollPrincipal)base.User; }
            set { _user = value; }
        }

        /// <summary>
        /// Set DB before an action executes.
        /// </summary>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (DB == null)
                DB = new PayrollContext();
        }

        /// <summary>
        /// Dispose of DB after an action executes.
        /// </summary>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (DB != null)
                DB.Dispose();
        }
    }
}