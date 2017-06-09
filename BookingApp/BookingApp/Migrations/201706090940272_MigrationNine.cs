namespace BookingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationNine : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AppUsers", "FullName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AppUsers", "FullName", c => c.Int(nullable: false));
        }
    }
}
