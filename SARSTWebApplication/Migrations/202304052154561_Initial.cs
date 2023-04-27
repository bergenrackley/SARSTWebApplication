namespace SARSTWebApplication.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RegistrationRequests",
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
                "dbo.ServicesOffered",
                c => new
                {
                    serviceName = c.String(nullable: false, maxLength: 128),
                    startDate = c.DateTime(),
                    endDate = c.DateTime(),
                })
                .PrimaryKey(t => t.serviceName);

        }

        public override void Down()
        {
            DropTable("dbo.ServicesOffered");
            DropTable("dbo.SarstUsers");
            DropTable("dbo.RegistrationRequests");
        }
    }
}
