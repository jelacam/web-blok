namespace BookingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FourthModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Countries", "Name", c => c.String(maxLength: 40));
            AlterColumn("dbo.Countries", "Code", c => c.String(maxLength: 40));
            AlterColumn("dbo.Regions", "Name", c => c.String(maxLength: 40));
            AlterColumn("dbo.Places", "Name", c => c.String(maxLength: 40));
            AlterColumn("dbo.Accommodations", "Name", c => c.String(maxLength: 40));
            AlterColumn("dbo.Accommodations", "Description", c => c.String(maxLength: 400));
            AlterColumn("dbo.Accommodations", "Address", c => c.String(maxLength: 60));
            AlterColumn("dbo.AccommodationTypes", "Name", c => c.String(maxLength: 40));
            AlterColumn("dbo.Comments", "Text", c => c.String(maxLength: 400));
            AlterColumn("dbo.Rooms", "Description", c => c.String(maxLength: 400));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Rooms", "Description", c => c.String());
            AlterColumn("dbo.Comments", "Text", c => c.String());
            AlterColumn("dbo.AccommodationTypes", "Name", c => c.String());
            AlterColumn("dbo.Accommodations", "Address", c => c.String());
            AlterColumn("dbo.Accommodations", "Description", c => c.String());
            AlterColumn("dbo.Accommodations", "Name", c => c.String());
            AlterColumn("dbo.Places", "Name", c => c.String());
            AlterColumn("dbo.Regions", "Name", c => c.String(maxLength: 50));
            AlterColumn("dbo.Countries", "Code", c => c.String(maxLength: 50));
            AlterColumn("dbo.Countries", "Name", c => c.String(maxLength: 50));
        }
    }
}
