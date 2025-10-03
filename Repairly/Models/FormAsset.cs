using System.ComponentModel.DataAnnotations;

namespace Repairly.Models
{
    public class FormAsset
    {
        public int Assets_id { get; set; }

        [Required]
        public string Asset_code { get; set; }
        [Required]
        public string Asset_name { get; set; }
        [Required]
        public int Category_id { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public DateOnly Purchase_date { get; set; }
        [Required]
        public DateOnly Warranty_date { get; set; }
    }
}
