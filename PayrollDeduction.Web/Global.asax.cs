using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PayrollDeduction.Web.Models;
using System.Web.Security;

namespace PayrollDeduction.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Root",
                "",
                new { controller = "Session", action = "Create", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "Login",
                "Session/Create",
                new { controller = "Session", action = "Create", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "Logout",
                "Session/Delete",
                new { controller = "Session", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "Account_Home",
                "Account/Transactions",
                new { controller = "AccountTransactions", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                "Account_Transactions",
                "Account/Transactions/{action}/{id}",
                new { controller = "AccountTransactions", Action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                "Account_Dependents",
                "Account/Dependents/{action}/{id}",
                new { controller = "AccountDependents", Action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                "Authorizations",
                "Authorizations/{action}/{id}",
                new { controller = "Authorizations", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                "Admin_Home",
                "Admin/Accounts",
                new { controller = "AdminAccounts", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                "Admin_Dependents",
                "Admin/Accounts/{accountId}/Dependents/{action}/{id}",
                new { controller = "AdminDependents", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                "Admin_Transactions",
                "Admin/Accounts/{accountId}/Transactions/{action}/{id}",
                new { controller = "AdminTransactions", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                "Admin_Accounts",
                "Admin/Accounts/{action}/{id}",
                new { controller = "AdminAccounts", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                "Admin_Authorizations",
                "Admin/Authorizations/{action}/{id}",
                new { controller = "AdminAuthorizations", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                "Notifications",
                "Notifications/{action}/{id}",
                new { controller = "Notifications", action = "Index", id = UrlParameter.Optional });

            /*
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
            */
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User == null)
                return;

            var identity = HttpContext.Current.User.Identity;

            if (identity.IsAuthenticated)
                HttpContext.Current.User = new PayrollPrincipal(identity);
            else
                HttpContext.Current.User = new PayrollPrincipal(identity, new TeamMember(), null);
        }
    }
}