namespace NewHotel.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    StartDate = c.DateTime(nullable: false),
                    EndDate = c.DateTime(nullable: false),
                    State = c.Int(nullable: false),
                    VisitorsCount = c.Int(nullable: false),
                    FinalPrice = c.Double(nullable: false),
                    Comment = c.String(),
                    Employee_Id = c.String(nullable: false, maxLength: 128),
                    Room_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Employee_Id, cascadeDelete: true)
                .ForeignKey("dbo.Rooms", t => t.Room_Id, cascadeDelete: true)
                .Index(t => new { t.StartDate, t.EndDate }, unique: true, name: "IX_Unique")
                .Index(t => t.Employee_Id)
                .Index(t => t.Room_Id);

            CreateTable(
                "dbo.AspNetUsers",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    FirstName = c.String(nullable: false),
                    LastName = c.String(nullable: false),
                    Email = c.String(maxLength: 256),
                    EmailConfirmed = c.Boolean(nullable: false),
                    PasswordHash = c.String(),
                    SecurityStamp = c.String(),
                    PhoneNumber = c.String(),
                    PhoneNumberConfirmed = c.Boolean(nullable: false),
                    TwoFactorEnabled = c.Boolean(nullable: false),
                    LockoutEndDateUtc = c.DateTime(),
                    LockoutEnabled = c.Boolean(nullable: false),
                    AccessFailedCount = c.Int(nullable: false),
                    UserName = c.String(nullable: false, maxLength: 256),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");

            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.String(nullable: false, maxLength: 128),
                    ClaimType = c.String(),
                    ClaimValue = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                {
                    LoginProvider = c.String(nullable: false, maxLength: 128),
                    ProviderKey = c.String(nullable: false, maxLength: 128),
                    UserId = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                {
                    UserId = c.String(nullable: false, maxLength: 128),
                    RoleId = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

            CreateTable(
                "dbo.Rooms",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Title = c.String(nullable: false),
                    Floor = c.Int(nullable: false),
                    MaxVisitorsCount = c.Int(nullable: false),
                    State = c.Int(nullable: false),
                    ImageUrl = c.String(nullable: false),
                    Type_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RoomTypes", t => t.Type_Id, cascadeDelete: true)
                .Index(t => t.Type_Id);

            CreateTable(
                "dbo.RoomFeatures",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Title = c.String(nullable: false),
                    Description = c.String(nullable: false),
                    AdditionalCost = c.Double(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.RoomTypes",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Title = c.String(nullable: false),
                    LivingRoomsCount = c.Int(nullable: false),
                    BasicRate = c.Double(nullable: false),
                    Description = c.String(nullable: false),
                    ImageUrl = c.String(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Visitors",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FirstName = c.String(nullable: false),
                    LastName = c.String(nullable: false),
                    PhoneNumber = c.String(nullable: false),
                    Email = c.String(nullable: false),
                    Citizenship = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AspNetRoles",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Name = c.String(nullable: false, maxLength: 256),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");

            CreateTable(
                "dbo.RoomFeatureRooms",
                c => new
                {
                    RoomFeature_Id = c.Int(nullable: false),
                    Room_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.RoomFeature_Id, t.Room_Id })
                .ForeignKey("dbo.RoomFeatures", t => t.RoomFeature_Id, cascadeDelete: true)
                .ForeignKey("dbo.Rooms", t => t.Room_Id, cascadeDelete: true)
                .Index(t => t.RoomFeature_Id)
                .Index(t => t.Room_Id);

            CreateTable(
                "dbo.VisitorBookings",
                c => new
                {
                    Visitor_Id = c.Int(nullable: false),
                    Booking_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.Visitor_Id, t.Booking_Id })
                .ForeignKey("dbo.Visitors", t => t.Visitor_Id, cascadeDelete: true)
                .ForeignKey("dbo.Bookings", t => t.Booking_Id, cascadeDelete: true)
                .Index(t => t.Visitor_Id)
                .Index(t => t.Booking_Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.VisitorBookings", "Booking_Id", "dbo.Bookings");
            DropForeignKey("dbo.VisitorBookings", "Visitor_Id", "dbo.Visitors");
            DropForeignKey("dbo.Bookings", "Room_Id", "dbo.Rooms");
            DropForeignKey("dbo.Rooms", "Type_Id", "dbo.RoomTypes");
            DropForeignKey("dbo.RoomFeatureRooms", "Room_Id", "dbo.Rooms");
            DropForeignKey("dbo.RoomFeatureRooms", "RoomFeature_Id", "dbo.RoomFeatures");
            DropForeignKey("dbo.Bookings", "Employee_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.VisitorBookings", new[] { "Booking_Id" });
            DropIndex("dbo.VisitorBookings", new[] { "Visitor_Id" });
            DropIndex("dbo.RoomFeatureRooms", new[] { "Room_Id" });
            DropIndex("dbo.RoomFeatureRooms", new[] { "RoomFeature_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Rooms", new[] { "Type_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Bookings", new[] { "Room_Id" });
            DropIndex("dbo.Bookings", new[] { "Employee_Id" });
            DropIndex("dbo.Bookings", "IX_Unique");
            DropTable("dbo.VisitorBookings");
            DropTable("dbo.RoomFeatureRooms");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Visitors");
            DropTable("dbo.RoomTypes");
            DropTable("dbo.RoomFeatures");
            DropTable("dbo.Rooms");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Bookings");
        }
    }
}
