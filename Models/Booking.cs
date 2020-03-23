using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Models
{
    public class Booking
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        [Required]
        [Index("IX_Unique", 1, IsUnique = true)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [Required]
        [Index("IX_Unique", 2, IsUnique = true)]
        public DateTime EndDate { get; set; }
        [Required]
        [Index("IX_Unique", 3, IsUnique = true)]
        public Room Room { get; set; }
        public Employee Employee { get; set; }
        [Required]
        public BookingState State { get; set; }
        [Required]
        public int VisitorsCount { get; set; }
        [Required]
        public double FinalPrice { get; set; }
        [Required]
        public Visitor Visitor { get; set; }
        public string Comment { get; set; }
    }
}
