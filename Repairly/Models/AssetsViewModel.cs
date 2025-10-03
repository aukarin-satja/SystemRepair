namespace Repairly.Models
{
    public class AssetsViewModel
    {

                public int Assets_id { get; set; }
                public string Asset_code { get; set; }
                public string Asset_name { get; set; }
                public string Category { get; set; }
                public string Brand { get; set; }
                public string Model { get; set; }
                public string Location { get; set; }
                public DateOnly Purchase_date { get; set; }
                public DateOnly Warranty_date { get; set; }
            }
}
