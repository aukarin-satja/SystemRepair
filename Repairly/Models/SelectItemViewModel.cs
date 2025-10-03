namespace Repairly.Models
{
    public class SelectItemViewModel
    {

        public int id { get; set; }
        public int user_id { get; set; }
        public int category_id { get; set; }
        public string description { get; set; }
        public int status_id { get; set; }
        public DateTime creat_date { get; set; } = DateTime.Now;

        public string asset_code { get; set; }
        public string name { get; set; }
        public string brand { get; set; }
        public string model { get; set; }
        public string category { get; set; }
 
        public string user_name { get; set; }

        public string status_name { get; set; }
        public string email { get; set; }
    }
}