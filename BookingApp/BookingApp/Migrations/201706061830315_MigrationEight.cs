namespace BookingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationEight : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "appUserId", "dbo.AppUsers");
            DropIndex("dbo.AspNetUsers", new[] { "appUserId" });
            DropColumn("dbo.AspNetUsers", "appUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "appUserId", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "appUserId");
            AddForeignKey("dbo.AspNetUsers", "appUserId", "dbo.AppUsers", "Id", cascadeDelete: true);
        }
    }
}
