namespace SARSTWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedresidents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServicesOffered", "description", c => c.String());
            AlterColumn("dbo.Residents", "sex", c => c.Int());
            AlterColumn("dbo.Residents", "gender", c => c.Int());
            AlterColumn("dbo.Residents", "pronouns", c => c.Int());
            AlterColumn("dbo.Residents", "status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Residents", "status", c => c.String());
            AlterColumn("dbo.Residents", "pronouns", c => c.String());
            AlterColumn("dbo.Residents", "gender", c => c.String());
            AlterColumn("dbo.Residents", "sex", c => c.String());
            DropColumn("dbo.ServicesOffered", "description");
        }
    }
}
