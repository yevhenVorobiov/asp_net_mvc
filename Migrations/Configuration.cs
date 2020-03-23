namespace NewHotel.Migrations
{
    using Hotel.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using NewHotel.App_Start;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Hotel.Data.HotelDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Hotel.Data.HotelDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var roleManager = new MyRoleManager(new MyRoleStore(context));
                var adminRole = new IdentityRole("Admin");
                roleManager.Create(adminRole);
            }
            if (!context.Users.Any(u => u.Email == "admin@admin.com"))
            {
                var manager = new UserManager<Employee>(new UserStore<Employee>(context));
                var user = new Employee()
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    FirstName = "Test",
                    LastName = "Test"
                };
                manager.Create(user, "123123");
                manager.AddToRole(user.Id, "Admin");
            }
            RoomType basic = new RoomType { Id = 1, Title = "Basic", BasicRate = 10, LivingRoomsCount = 1, ImageUrl = "basic.jpg",
                Description = "Basic setup for comfortable living with all needed features." } ;
            RoomType normal = new RoomType {Id = 2, Title = "Normal", BasicRate = 15, LivingRoomsCount = 2, ImageUrl = "normal.jpg",
                Description = "Extend format for comfortable living with one additional room."};
            RoomType business = new RoomType { Id = 3, Title = "Business", BasicRate = 20, LivingRoomsCount = 3, ImageUrl = "business.jpg",
                Description = "Basic setup for comfortable living with all needed features."};
            RoomType luxury = new RoomType { Id = 4, Title = "Luxury", BasicRate = 35, LivingRoomsCount = 5, ImageUrl = "luxury.jpg",
                Description = "The best apartments with perfecrt view of the city for our best guests."};
            context.RoomTypes.AddOrUpdate(basic);
            context.RoomTypes.AddOrUpdate(normal);
            context.RoomTypes.AddOrUpdate(business);
            context.RoomTypes.AddOrUpdate(luxury);

            RoomFeature condition = new RoomFeature {Id = 1, Title = "Air condition", Description = "High quility air condition", AdditionalCost=2};
            RoomFeature miniBar = new RoomFeature { Id = 2, Title = "Minibar", Description = "Unlimited minibar", AdditionalCost = 3 };
            context.RoomFeatures.AddOrUpdate(condition);
            context.RoomFeatures.AddOrUpdate(miniBar);

            Room basicRoom = new Room { Id = 1, Floor = 2, MaxVisitorsCount = 1, Type = basic, Title = "Basic room", ImageUrl = "basic.jpg" };
            Room normalRoom = new Room { Id = 2, Floor = 2, MaxVisitorsCount = 2, Type = normal, Title = "Normal room", ImageUrl = "normal.jpg",
            Features = new System.Collections.Generic.List<RoomFeature> {condition}};
            Room businessRoom = new Room { Id = 3, Floor = 5, MaxVisitorsCount = 2, Type = business, Title = "Business room", ImageUrl = "business.jpg",
                Features = new System.Collections.Generic.List<RoomFeature> { condition, miniBar }};
            context.Rooms.AddOrUpdate(basicRoom);
            context.Rooms.AddOrUpdate(normalRoom);
            context.Rooms.AddOrUpdate(businessRoom);
        }
    }
}
