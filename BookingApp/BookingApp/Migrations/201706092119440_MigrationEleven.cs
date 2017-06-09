namespace BookingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationEleven : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppUsers", "UserName");
        }
    }
}
