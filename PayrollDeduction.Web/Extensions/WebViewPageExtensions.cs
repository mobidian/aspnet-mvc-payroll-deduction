using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayrollDeduction.Web.Models;

namespace PayrollDeduction.Web.Extensions
{
    public abstract class WebViewPage : System.Web.Mvc.WebViewPage
    {
        public new PayrollPrincipal User
        {
            get { return (PayrollPrincipal)base.User; }
        }
    }

    public abstract class WebViewPage<T> : System.Web.Mvc.WebViewPage<T>
    {
        public new PayrollPrincipal User
        {
            get { return (PayrollPrincipal)base.User; }
        }
    }
}