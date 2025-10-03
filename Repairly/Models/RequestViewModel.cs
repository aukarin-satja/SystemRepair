namespace Repairly.Models
{
    public class RequestViewModel
    {

        public int id { get; set; }
        public int asset_id { get; set; }
        public string asset_name { get; set; }
        public int user_id { get; set; }
        public string user_name { get; set; }
        public int category_id { get; set; }
        public string category_name { get; set; }
        public string description { get; set; }
        public int status_id { get; set; }
        public string status_name { get; set; }
        public DateTime created_at { get; set; }
        public string brand { get; set; }
        public string model { get; set; }

    }
}
