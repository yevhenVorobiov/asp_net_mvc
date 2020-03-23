namespace NewHotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VisitorBookings", "Visitor_Id", "dbo.Visitors");
            DropForeignKey("dbo.VisitorBookings", "Booking_Id", "dbo.Bookings");
            DropIndex("dbo.VisitorBookings", new[] { "Visitor_Id" });
            DropIndex("dbo.VisitorBookings", new[] { "Booking_Id" });
            AddColumn("dbo.Bookings", "Visitor_Id", c => c.Int());
            CreateIndex("dbo.Bookings", "Visitor_Id");
            AddForeignKey("dbo.Bookings", "Visitor_Id", "dbo.Visitors", "Id");
            DropColumn("dbo.Visitors", "Citizenship");
            DropTable("dbo.VisitorBookings");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.VisitorBookings",
                c => new
                    {
                        Visitor_Id = c.Int(nullable: false),
                        Booking_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Visitor_Id, t.Booking_Id });
            
            AddColumn("dbo.Visitors", "Citizenship", c => c.String());
            DropForeignKey("dbo.Bookings", "Visitor_Id", "dbo.Visitors");
            DropIndex("dbo.Bookings", new[] { "Visitor_Id" });
            DropColumn("dbo.Bookings", "Visitor_Id");
            CreateIndex("dbo.VisitorBookings", "Booking_Id");
            CreateIndex("dbo.VisitorBookings", "Visitor_Id");
            AddForeignKey("dbo.VisitorBookings", "Booking_Id", "dbo.Bookings", "Id", cascadeDelete: true);
            AddForeignKey("dbo.VisitorBookings", "Visitor_Id", "dbo.Visitors", "Id", cascadeDelete: true);
        }
    }
}
