namespace Repairly.Models
{
    public class DashboardViewModel
    {
        public List<RepairRequestViewModel> Requests { get; set; }
        public List<string> ChartLabels { get; set; }
        public List<int> ChartData { get; set; }
        public List<string> MonthsLabels { get; set; }
        public List<int> MonthsData { get; set; }
        public int Active { get; set; }
        public int Wait { get; set; }
        public int Success { get; set; }
        public int Total => Active + Wait + Success;
    }
}
