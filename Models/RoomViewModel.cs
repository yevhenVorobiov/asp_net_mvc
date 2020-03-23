using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Models
{
    public class RoomViewModel
    {
        public Room Room { get; set; }
        [Display(Name="Additional room features")]
        public List<FeatureCheckbox> RoomFeatures { get; set; }
        public List<RoomType> RoomTypes { get; set; }
        public int SelectedTypeId { get; set; }
    }

    public class FeatureCheckbox
    {
        public bool Checked { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
    }
}