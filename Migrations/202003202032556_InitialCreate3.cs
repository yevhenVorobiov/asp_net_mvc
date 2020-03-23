namespace NewHotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bookings", "Visitor_Id", "dbo.Visitors");
            DropIndex("dbo.Bookings", new[] { "Visitor_Id" });
            AlterColumn("dbo.Bookings", "Visitor_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Bookings", "Visitor_Id");
            AddForeignKey("dbo.Bookings", "Visitor_Id", "dbo.Visitors", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "Visitor_Id", "dbo.Visitors");
            DropIndex("dbo.Bookings", new[] { "Visitor_Id" });
            AlterColumn("dbo.Bookings", "Visitor_Id", c => c.Int());
            CreateIndex("dbo.Bookings", "Visitor_Id");
            AddForeignKey("dbo.Bookings", "Visitor_Id", "dbo.Visitors", "Id");
        }
    }
}
