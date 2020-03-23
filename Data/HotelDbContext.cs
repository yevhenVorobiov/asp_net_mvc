using Hotel.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Hotel.Data
{
    public class HotelDbContext : IdentityDbContext<Employee>
    {
        public HotelDbContext() : base("HotelDbContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<Booking> Bookings { get; set; }
        //public DbSet<Employee> Employees { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomFeature> RoomFeatures { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
     
        public static HotelDbContext Create()
        {
            return new HotelDbContext();
        }
    }
}
