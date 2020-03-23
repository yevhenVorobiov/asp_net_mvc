using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Models
{
    public class Room
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [Range(1, 20)]
        public int Floor { get; set; }
        [Display(Name = "Room Type")]
        [Required]
        public RoomType Type { get; set; }
        [Display(Name = "Maximum visitors")]
        [Required]
        [Range(1, 5)]
        public int MaxVisitorsCount { get; set; }
        public List<RoomFeature> Features { get; set; }
        [Display(Name = "Image URL")]
        [Required]
        public string ImageUrl { get; set; }
    }
}
