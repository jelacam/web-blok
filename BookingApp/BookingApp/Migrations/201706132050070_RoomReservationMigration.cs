namespace BookingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoomReservationMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RoomReservations", "StartDate", c => c.String());
            AlterColumn("dbo.RoomReservations", "EndDate", c => c.String());
            //AlterColumn("dbo.RoomReservations", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            DropColumn("dbo.RoomReservations", "Timestamp", null);
            AddColumn("dbo.RoomReservations", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RoomReservations", "Timestamp", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RoomReservations", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RoomReservations", "StartDate", c => c.DateTime(nullable: false));
        }
    }
}
