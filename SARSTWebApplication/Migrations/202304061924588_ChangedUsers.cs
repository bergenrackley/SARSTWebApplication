namespace SARSTWebApplication.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ChangedUsers : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RegistrationRequests", "userRole", c => c.Int());
            AlterColumn("dbo.SarstUsers", "userRole", c => c.Int());
        }

        public override void Down()
        {
            AlterColumn("dbo.SarstUsers", "userRole", c => c.String());
            AlterColumn("dbo.RegistrationRequests", "userRole", c => c.String());
        }
    }
}
