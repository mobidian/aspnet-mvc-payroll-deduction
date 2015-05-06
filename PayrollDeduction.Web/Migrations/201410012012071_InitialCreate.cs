namespace PayrollDeduction.Web.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamMemberId = c.Int(nullable: false),
                        Balance = c.Double(nullable: false),
                        PaymentAmount = c.Double(nullable: false),
                        PayPeriods = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("TeamMembers", t => t.TeamMemberId)
                .Index(t => t.TeamMemberId);
            
            CreateTable(
                "TeamMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamMemberId = c.String(),
                        NetworkId = c.String(),
                        LastName = c.String(),
                        FirstName = c.String(),
                        CostCenter = c.String(),
                        JobCode = c.String(),
                        HiredOn = c.DateTime(nullable: false),
                        TerminatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Dependents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastName = c.String(),
                        FirstName = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        AccountId = c.Int(),
                        AuthorizationId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Accounts", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("Authorizations", t => t.AuthorizationId, cascadeDelete: true)
                .Index(t => t.AccountId)
                .Index(t => t.AuthorizationId);
            
            CreateTable(
                "AccountTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountId = c.Int(nullable: false),
                        TypeId = c.Int(nullable: false),
                        Note = c.String(nullable: false),
                        Amount = c.Double(nullable: false),
                        CreatedBy = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Accounts", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);
            
            CreateTable(
                "Authorizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamMemberId = c.Int(nullable: false),
                        Archived = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("TeamMembers", t => t.TeamMemberId)
                .Index(t => t.TeamMemberId);
            
        }
        
        public override void Down()
        {
            DropIndex("Authorizations", new[] { "TeamMemberId" });
            DropIndex("AccountTransactions", new[] { "AccountId" });
            DropIndex("Dependents", new[] { "AuthorizationId" });
            DropIndex("Dependents", new[] { "AccountId" });
            DropIndex("Accounts", new[] { "TeamMemberId" });
            DropForeignKey("Authorizations", "TeamMemberId", "TeamMembers");
            DropForeignKey("AccountTransactions", "AccountId", "Accounts");
            DropForeignKey("Dependents", "AuthorizationId", "Authorizations");
            DropForeignKey("Dependents", "AccountId", "Accounts");
            DropForeignKey("Accounts", "TeamMemberId", "TeamMembers");
            DropTable("Authorizations");
            DropTable("AccountTransactions");
            DropTable("Dependents");
            DropTable("TeamMembers");
            DropTable("Accounts");
        }
    }
}
