namespace PayrollDeduction.Web.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using PayrollDeduction.Web.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<PayrollDeduction.Web.Models.PayrollContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PayrollDeduction.Web.Models.PayrollContext context)
        {
        }
    }
}
