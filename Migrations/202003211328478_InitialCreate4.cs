namespace NewHotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bookings", "Employee_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Bookings", new[] { "Employee_Id" });
            AlterColumn("dbo.Bookings", "Employee_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Bookings", "Employee_Id");
            AddForeignKey("dbo.Bookings", "Employee_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "Employee_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Bookings", new[] { "Employee_Id" });
            AlterColumn("dbo.Bookings", "Employee_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Bookings", "Employee_Id");
            AddForeignKey("dbo.Bookings", "Employee_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
