namespace NewHotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate5 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Rooms", "State");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rooms", "State", c => c.Int(nullable: false));
        }
    }
}
