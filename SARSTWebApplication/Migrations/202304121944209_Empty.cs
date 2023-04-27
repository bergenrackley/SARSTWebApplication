namespace SARSTWebApplication.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Empty : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Residents");
            AlterColumn("dbo.Residents", "residentId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Residents", "dateOfBirth", c => c.DateTime());
            AddPrimaryKey("dbo.Residents", "residentId");
        }

        public override void Down()
        {
            DropPrimaryKey("dbo.Residents");
            AlterColumn("dbo.Residents", "dateOfBirth", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Residents", "residentId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Residents", "residentId");
        }
    }
}
