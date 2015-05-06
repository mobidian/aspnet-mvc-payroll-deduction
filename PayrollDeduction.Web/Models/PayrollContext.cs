using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Xml.Serialization;

namespace PayrollDeduction.Web.Models
{
    public class PayrollContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Dependent> Dependents { get; set; }
        public DbSet<AccountTransaction> AccountTransactions { get; set; }
        public DbSet<Authorization> Authorizations { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Account Configuration
            modelBuilder.Entity<Account>()
                .HasRequired(x => x.TeamMember)
                .WithMany()
                .HasForeignKey(x => x.TeamMemberId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
                .HasMany(x => x.Dependents)
                .WithRequired(x => x.Account)
                .HasForeignKey(x => x.AccountId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Account>()
                .HasMany(x => x.Transactions)
                .WithRequired(x => x.Account)
                .HasForeignKey(x => x.AccountId)
                .WillCascadeOnDelete();

            // Authorization Configuration
            modelBuilder.Entity<Authorization>()
                .HasRequired(x => x.TeamMember)
                .WithMany()
                .HasForeignKey(x => x.TeamMemberId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Authorization>()
                .HasMany(x => x.Dependents)
                .WithRequired(x => x.Authorization)
                .HasForeignKey(x => x.AuthorizationId)
                .WillCascadeOnDelete();
        }
    }
}
