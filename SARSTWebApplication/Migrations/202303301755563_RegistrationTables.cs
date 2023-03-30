namespace SARSTWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegistrationTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SarstUsers",
                c => new
                    {
                        userName = c.String(nullable: false, maxLength: 128),
                        firstName = c.String(),
                        lastName = c.String(),
                        email = c.String(),
                        password = c.String(),
                        userRole = c.String(),
                    })
                .PrimaryKey(t => t.userName);
            
            CreateTable(
                "dbo.RegistrationRequests",
                c => new
                    {
                        userName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.userName)
                .ForeignKey("dbo.SarstUsers", t => t.userName)
                .Index(t => t.userName);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RegistrationRequests", "userName", "dbo.SarstUsers");
            DropIndex("dbo.RegistrationRequests", new[] { "userName" });
            DropTable("dbo.RegistrationRequests");
            DropTable("dbo.SarstUsers");
        }
    }
}
