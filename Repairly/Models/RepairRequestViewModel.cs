namespace Repairly.Models
{
    public class RepairRequestViewModel
    {
        public int Id { get; set; }
        public string AssetName { get; set; }
        public string RequesterName { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string StatusName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
