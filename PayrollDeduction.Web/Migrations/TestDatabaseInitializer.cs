using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PayrollDeduction.Web.Models;

namespace PayrollDeduction.Web.Migrations
{
    /// <summary>
    /// Seeds the database with data that is used in the test environments.
    /// </summary>
    public class TestDatabaseInitializer : DropCreateDatabaseAlways<PayrollContext>
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
                },
                new TeamMember() 
                { 
                    TeamMemberId = "john", 
                    NetworkId = "john", 
                    FirstName = "John", 
                    LastName = "Doe", 
                    CostCenter = "2000", 
                    JobCode = "2000", 
                    HiredOn = DateTime.Now 
                },
                new TeamMember() 
                { 
                    TeamMemberId = "jane", 
                    NetworkId = "jane", 
                    FirstName = "Jane", 
                    LastName = "Smith", 
                    CostCenter = "3000", 
                    JobCode = "3000", 
                    HiredOn = DateTime.Now 
                },
                new TeamMember() 
                { 
                    TeamMemberId = "jackie", 
                    NetworkId = "jackie", 
                    FirstName = "Jackie", 
                    LastName = "Smith", 
                    CostCenter = "4000", 
                    JobCode = "4000", 
                    HiredOn = DateTime.Now 
                },
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
                },
                new Authorization() 
                { 
                    TeamMember = teamMembers.ElementAt(1), 
                    Archived = true, 
                    Dependents = new List<AuthorizationDependent>()
                    {
                        new AuthorizationDependent() 
                        { 
                            FirstName = "John", 
                            LastName = "Doe, Jr.", 
                            BirthDate = new DateTime(2010, 1, 1) 
                        }
                    }
                },
                new Authorization() 
                { 
                    TeamMember = teamMembers.ElementAt(2), 
                    Archived = true
                },
                new Authorization() 
                { 
                    TeamMember = teamMembers.ElementAt(3), 
                    Archived = false
                }
            };

            var accounts = new List<Account>()
            {
                // Account 1: no dependents, active
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
                },

                // Account 2: no transactions, active
                new Account() 
                { 
                    TeamMember = teamMembers.ElementAt(1), 
                    Active = true, 
                    Dependents = new List<AccountDependent>()
                    {
                        new AccountDependent() 
                        { 
                            FirstName = "John", 
                            LastName = "Doe, Jr.", 
                            BirthDate = new DateTime(2010, 1, 1)
                        }
                    }
                },

                // Account 3: no dependents, active
                new Account() 
                { 
                    TeamMember = teamMembers.ElementAt(2), 
                    Active = true, 
                    Transactions = new List<AccountTransaction>()
                    {
                        new AccountTransaction() 
                        { 
                            Type = AccountTransactionType.Debit, 
                            Amount = 1000.0, 
                            Note = "Starting Balance" 
                        }
                    }
                },
            };

            teamMembers.ForEach(x => context.TeamMembers.Add(x));
            authorizations.ForEach(x => context.Authorizations.Add(x));
            accounts.ForEach(x => { x.Refresh(); context.Accounts.Add(x); });
        }
    }
}