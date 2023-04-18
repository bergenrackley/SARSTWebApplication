namespace SARSTWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class residentstays : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ResidentStays",
                c => new
                    {
                        stayId = c.Int(nullable: false, identity: true),
                        residentId = c.String(),
                        checkinDateTime = c.DateTime(nullable: false),
                        checkoutDateTime = c.DateTime(),
                        userName = c.String(),
                    })
                .PrimaryKey(t => t.stayId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ResidentStays");
        }
    }
}
