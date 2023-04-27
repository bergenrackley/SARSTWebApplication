namespace SARSTWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedStayAndUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ResidentStays", "NoteworthyEvents", c => c.String());
            AddColumn("dbo.SarstUsers", "changePassword", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SarstUsers", "changePassword");
            DropColumn("dbo.ResidentStays", "NoteworthyEvents");
        }
    }
}
