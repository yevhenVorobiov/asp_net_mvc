using System.ComponentModel.DataAnnotations;

namespace Hotel.Models
{
    public class RoomType
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Amount of living rooms")]
        public int LivingRoomsCount { get; set; }
        [Required]
        public double BasicRate { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ImageUrl { get; set; }
    }
}
