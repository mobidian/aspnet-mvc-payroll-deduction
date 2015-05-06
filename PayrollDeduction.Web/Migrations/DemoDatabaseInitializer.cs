using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PayrollDeduction.Web.Models;

namespace PayrollDeduction.Web.Migrations
{
    /// <summary>
    /// Seeds the database with demo data.
    /// </summary>
    public class DemoDatabaseInitializer : DropCreateDatabaseIfModelChanges<PayrollContext>
    {
        protected override void Seed(PayrollContext context)
        {
            var teamMembers = new List<TeamMember>()
            {
                new TeamMember() 
                { 
                    TeamMemberId = "josh", 
                    NetworkId = "josh", 
                    FirstName = "Josh", 
                    LastName = "O'Rourke", 
                    CostCenter = "1000", 
                    JobCode = "1000", 
                    HiredOn = DateTime.Now 
                }
            };

            var authorizations = new List<Authorization>()
            {
                new Authorization() 
                { 
                    TeamMember = teamMembers.ElementAt(0), 
                    Archived = true, 
                    Dependents = new List<AuthorizationDependent>()
                    {
                        new AuthorizationDependent() 
                        { 
                            FirstName = "Josh", 
                            LastName = "O'Rourke, Jr.", 
                            BirthDate = new DateTime(2010, 1, 1)
                        }
                    }
                }
            };

            var accounts = new List<Account>()
            {
                new Account() 
                { 
                    TeamMember = teamMembers.ElementAt(0), 
                    Active = true, 
                    Dependents = new List<AccountDependent>()
                    {
                        new AccountDependent() 
                        { 
                            FirstName = "Josh", 
                            LastName = "O'Rourke, Jr.", 
                            BirthDate = new DateTime(2010, 1, 1)
                        }
                    },
                    Transactions = new List<AccountTransaction>()
                    {
                        new AccountTransaction() 
                        { 
                            Type = AccountTransactionType.Debit, 
                            Amount = 1000.0, 
                            Note = "Starting Balance" 
                        }
                    }
                }
            };

            teamMembers.ForEach(x => context.TeamMembers.Add(x));
            authorizations.ForEach(x => context.Authorizations.Add(x));
            accounts.ForEach(x => { x.Refresh(); context.Accounts.Add(x); });
        }
    }
}