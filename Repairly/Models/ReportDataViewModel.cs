namespace Repairly.Models
{
    public class ReportDataViewModel
    {
      
        public List<ReportViewModel> Data { get; set; } = new();
        public Dictionary<string, int> StatusSummary { get; set; } = new();
        public Dictionary<string, int> CategorySummary { get; set; } = new();
        public Dictionary<string, int> BrandSummary { get; set; } = new();

    }
}
