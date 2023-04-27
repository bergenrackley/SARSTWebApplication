namespace SARSTWebApplication.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ResidentUpdate : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Residents");
            AlterColumn("dbo.Residents", "residentId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Residents", "residentId");
        }

        public override void Down()
        {
            DropPrimaryKey("dbo.Residents");
            AlterColumn("dbo.Residents", "residentId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Residents", "residentId");
        }
    }
}
