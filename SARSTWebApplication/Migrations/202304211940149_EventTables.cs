namespace SARSTWebApplication.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class EventTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DisciplinaryTracker",
                c => new
                {
                    disciplinaryEventId = c.Int(nullable: false, identity: true),
                    disciplinaryType = c.Int(),
                    dateProvided = c.DateTime(nullable: false),
                    description = c.String(),
                    residentId = c.String(),
                    userName = c.String(),
                    stayId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.disciplinaryEventId);

            CreateTable(
                "dbo.ServiceTracker",
                c => new
                {
                    serviceEventId = c.Int(nullable: false, identity: true),
                    serviceName = c.String(),
                    dateProvided = c.DateTime(nullable: false),
                    description = c.String(),
                    residentId = c.String(),
                    userName = c.String(),
                    stayId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.serviceEventId);

        }

        public override void Down()
        {
            DropTable("dbo.ServiceTracker");
            DropTable("dbo.DisciplinaryTracker");
        }
    }
}
