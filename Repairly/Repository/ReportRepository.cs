using Microsoft.AspNetCore.Mvc.Rendering;
using Repairly.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Repairly.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly string _ConnectionString;
        public ReportRepository(IConfiguration config)
        {
            _ConnectionString = config.GetConnectionString("DefaultConnection");
        }
        public List<ReportViewModel> GetAllData()
        {
            List<ReportViewModel> data = new();

            using var conn = new SqlConnection(_ConnectionString);
            conn.Open();

            using var cmd = new SqlCommand("SELECT Re.[id] ,Assets.name as asset_name,brand,model,AC.name AS category ,[description],StatusTypes.name as status ,[created_at]  FROM [RepairRequests]" +
                "AS Re JOIN [Assets] ON Re.[asset_id] = Assets.id JOIN[AssetCategories] AS AC ON AC.id = Re.[category_id] JOIN [StatusTypes] ON [StatusTypes].id = status_id ", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new ReportViewModel
                {
                    id = reader.GetInt32(reader.GetOrdinal("id")),
                    asset_name = reader.GetString(reader.GetOrdinal("asset_name")),
                    brand = reader.GetString(reader.GetOrdinal("brand")),
                    model = reader.GetString(reader.GetOrdinal("model")),
                    category = reader.GetString(reader.GetOrdinal("category")),
                    description = reader.GetString(reader.GetOrdinal("description")),
                    status = reader.GetString(reader.GetOrdinal("status")),
                    create_at = reader.GetDateTime(reader.GetOrdinal("created_at")),
                });
                   


             }
         



            return data;
        }

        public List<SelectListItem> GetStatus()
        {

            List<SelectListItem> status = new();
            using var conn = new SqlConnection(_ConnectionString);
            conn.Open();

            using var cmd = new SqlCommand("SELECT [id],[name] FROM [StatusTypes]", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                status.Add(new SelectListItem
                {
                    Value = reader.GetInt32(reader.GetOrdinal("id")).ToString(),
                    Text = reader.GetString(reader.GetOrdinal("name")),
                });



            }
            return status;
        }



        public List<SelectListItem> GetCategory()
        {
            List<SelectListItem> cate = new();
            using var conn = new SqlConnection(_ConnectionString);
            conn.Open();

            using var cmd = new SqlCommand("SELECT [id],[name] FROM [AssetCategories]", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cate.Add(new SelectListItem
                {
                    Value = reader.GetInt32(reader.GetOrdinal("id")).ToString(),
                    Text = reader.GetString(reader.GetOrdinal("name")),
                });



            }
            return cate;

        }

        public List<ReportViewModel> Search(DateTime? startDate, DateTime? endDate, int status, int Category)
        {

            List<ReportViewModel> data = new();
            using var conn = new SqlConnection(_ConnectionString);
            conn.Open();
            var sql = new StringBuilder("SELECT Re.[id] ,Assets.name as asset_name,brand,model,AC.name AS category ,AC.id AS categoryId,[description],StatusTypes.name as status " +
                ",StatusTypes.id as status_id ,[created_at]  FROM [RepairRequests] AS Re JOIN [Assets] ON Re.[asset_id] = Assets.id JOIN[AssetCategories] AS AC ON AC.id = Re.[category_id] " +
                "JOIN [StatusTypes] ON [StatusTypes].id = status_id WHERE 1=1");



            using var cmd = new SqlCommand( );
            cmd.Connection = conn;
            if(startDate.HasValue && endDate.HasValue)
            {
                sql.Append("AND ( created_at >= @startDate AND  created_at <= @endDate )");
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);
            }
            if(Category != 0)
            {
                sql.Append("AND AC.id = @categoryId ");
                cmd.Parameters.AddWithValue("@categoryId", Category);
            }
           if(status != 0){
                sql.Append("AND( status_id = @status )");
                cmd.Parameters.AddWithValue("@status", status);
            }
            cmd.CommandText = sql.ToString();
           

  
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new ReportViewModel
                {
                    id = reader.GetInt32(reader.GetOrdinal("id")),
                    asset_name = reader.GetString(reader.GetOrdinal("asset_name")),
                    brand = reader.GetString(reader.GetOrdinal("brand")),
                    model = reader.GetString(reader.GetOrdinal("model")),
                    category = reader.GetString(reader.GetOrdinal("category")),
                    description = reader.GetString(reader.GetOrdinal("description")),
                    status = reader.GetString(reader.GetOrdinal("status")),
                    create_at = reader.GetDateTime(reader.GetOrdinal("created_at")),
                });



            }
            return data;
        }

       
    }
}
