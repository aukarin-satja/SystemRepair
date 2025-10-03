namespace Repairly.Models
{
    public class AssetsPageViewModel
    {
        public List<AssetsViewModel> Assets { get; set; }
        
        public int CurrentPage { get; set; } 
        public int PageSize { get; set; }
        public int TotalRecord { get; set; }
        public int TotalPage => (int)Math.Ceiling((double)TotalRecord / PageSize);
        public FormAsset FormData { get; set; }
    }
}
