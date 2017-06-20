namespace BookingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllowedToAccomm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "Allow", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppUsers", "Allow");
        }
    }
}
