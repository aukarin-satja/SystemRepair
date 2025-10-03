using Repairly.Repository;

namespace Repairly.Models
{
    public class ReportViewModel
    {
     
        public int id { get; set; }
        public string asset_name { get; set; }
        public string brand { get; set; }
        public string model { get; set; }
        public string category { get; set; }
        public string description { get; set; }
        public string status { get; set; }
    
        
        public DateTime create_at { get; set; }
        public Dictionary<string, int> StatusSummary { get; set; }
        public Dictionary<string, int> CategorySummary { get; set; }
        public Dictionary<string, int> BrandSummary { get; set; }


    }
}
