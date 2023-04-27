namespace SARSTWebApplication.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Residents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Residents",
                c => new
                {
                    residentId = c.String(nullable: false, maxLength: 128),
                    firstName = c.String(),
                    lastName = c.String(),
                    dateOfBirth = c.DateTime(nullable: false),
                    sex = c.String(),
                    gender = c.String(),
                    pronouns = c.String(),
                    distinguishingFeatures = c.String(),
                    status = c.String(),
                })
                .PrimaryKey(t => t.residentId);

        }

        public override void Down()
        {
            DropTable("dbo.Residents");
        }
    }
}
