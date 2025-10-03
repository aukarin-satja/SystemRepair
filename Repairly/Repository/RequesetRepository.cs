using Microsoft.AspNetCore.Mvc.Rendering;
using Repairly.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Repairly.Repository
{
    public class RequesetRepository : IRequestRepository
    {
        private readonly string _connectionString;
        public RequesetRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }
        public List<RequestViewModel> GetAllData(){

            List<RequestViewModel> Requests = new();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand("SELECT Re.[id] ,Assets.name as name,brand,model,[first_name]+' '+[last_name] AS username ,AC.name AS category ," +
                "[description],StatusTypes.name as status ,[created_at] FROM [RepairRequests] AS Re JOIN [Assets] ON Re.[asset_id] = Assets.id JOIN [Users] ON " +
                "[Users].id = user_id JOIN [AssetCategories] AS AC ON AC.id = Re.[category_id] JOIN [StatusTypes] ON [StatusTypes].id = status_id ORDER BY created_at DESC", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Requests.Add(new RequestViewModel
                {
                    id = reader.GetInt32(reader.GetOrdinal("id")),
                    asset_name = reader.GetString(reader.GetOrdinal("name")),
                    user_name = reader.GetString(reader.GetOrdinal("username")),
                    category_name = reader.GetString(reader.GetOrdinal("category")),
                    description = reader.GetString(reader.GetOrdinal("description")),
                    status_name = reader.GetString(reader.GetOrdinal("status")),
                    created_at = reader.GetDateTime(reader.GetOrdinal("created_at")),
                    brand = reader.GetString(reader.GetOrdinal("brand")),
                    model = reader.GetString(reader.GetOrdinal("model")),
                });
            }
            return Requests;
        }

        public List<SelectItemViewModel> SearchAsset(string data)
        {
            List<SelectItemViewModel> asset = new();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
             
            using var cmd = new SqlCommand("SELECT Assets.id AS id ,asset_code ,brand ,model ,Assets.name AS name ,category_id,AC.name AS category FROM  [Assets] JOIN [AssetCategories] AS AC ON [category_id] = AC.id  WHERE asset_code LIKE @data ", conn);
            cmd.Parameters.AddWithValue("@data", $"%{data}%");
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                asset.Add(new SelectItemViewModel
                {
                    id = reader.GetInt32(reader.GetOrdinal("id")),
                    asset_code = reader.GetString(reader.GetOrdinal("asset_code")),
                    brand = reader.GetString(reader.GetOrdinal("brand")),
                    model = reader.GetString(reader.GetOrdinal("model")),
                    name = reader.GetString(reader.GetOrdinal("name")),
                    category_id = reader.GetInt32(reader.GetOrdinal("category_id")),
                    category = reader.GetString(reader.GetOrdinal("category")),
                });
            }
            return asset;

        }

        public List<SelectItemViewModel> GetUser(string data)
        {
            List<SelectItemViewModel> user = new();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand("SELECT [id],[first_name]+' '+[last_name] AS name ,[email] FROM [Users] WHERE first_name LIKE @data OR last_name LIKE @data ", conn);
            cmd.Parameters.AddWithValue("@data", $"%{data}%");
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                user.Add(new SelectItemViewModel
                {
                    user_id = reader.GetInt32(reader.GetOrdinal("id")),
                    user_name = reader.GetString(reader.GetOrdinal("name")),
                    email = reader.GetString(reader.GetOrdinal("email")),
      
                });
            }
            return user;
        } 


        public bool CreatRequest (SelectItemViewModel data)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand("INSERT INTO [RepairRequests] ([asset_id],[user_id],[category_id],[description],[status_id],[created_at]) " +
                "VALUES (@asset_id ,@user_id,@category_id,@description,@status_id,@created_at)", conn);
  
            cmd.Parameters.AddWithValue("@asset_id", data.id);
            cmd.Parameters.AddWithValue("@user_id", data.user_id);
            cmd.Parameters.AddWithValue("@category_id", data.category_id);
            cmd.Parameters.AddWithValue("@description", data.description);
            cmd.Parameters.AddWithValue("@status_id", data.status_id);
            cmd.Parameters.AddWithValue("@created_at", data.creat_date);

            int rowsuccess = cmd.ExecuteNonQuery();
            return rowsuccess > 0;
        }

        public bool DelRequest(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand("DELETE [RepairRequests] WHERE [id] = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            int rowsuccess = cmd.ExecuteNonQuery();
            return rowsuccess > 0;
        }

        public List<SelectListItem> GetStatus()
        {
            List<SelectListItem> status = new();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cate = new SqlCommand("SELECT id,name FROM [StatusTypes]", conn);
            using var reader = cate.ExecuteReader();
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
            List<SelectListItem> status = new();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cate = new SqlCommand("SELECT id,name FROM [AssetCategories]", conn);
            using var reader = cate.ExecuteReader();
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


        public SelectItemViewModel GetDetail(int id)
        {
            SelectItemViewModel detail = new();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand("SELECT RQ.[id] AS id,[asset_id],[Assets].[name] AS name,[first_name]+' '+[last_name] AS user_name,email ,brand,model,AC.[name] AS category,[description],STP.[id] AS status_id,[created_at] FROM [RepairRequests] AS RQ " +
                "JOIN [Assets] ON  [Assets].id = [asset_id] JOIN  [Users] ON [Users].[id] =[user_id] JOIN [AssetCategories] AS AC ON AC.[id]=RQ.[category_id] JOIN [StatusTypes] AS STP ON STP.[id] =[status_id] WHERE RQ.[id]=@id ", conn);

            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {



                        detail.id = reader.GetInt32(reader.GetOrdinal("id"));
                        detail.name = reader.GetString(reader.GetOrdinal("name"));
                        detail.user_name = reader.GetString(reader.GetOrdinal("username"));
                        detail.email = reader.GetString(reader.GetOrdinal("email"));
                        detail.category = reader.GetString(reader.GetOrdinal("category"));
                        detail.description = reader.GetString(reader.GetOrdinal("description"));
                        detail.status_id = reader.GetInt32(reader.GetOrdinal("status_id"));
                        detail.brand = reader.GetString(reader.GetOrdinal("brand"));
                        detail.model = reader.GetString(reader.GetOrdinal("model"));

            }

                
            return detail;
        }

        public bool UpdateRequest(SelectItemViewModel data)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand("UPDATE [RepairRequests] SET description =@description ,status_id =@status_id WHERE id= @id", conn);

            cmd.Parameters.AddWithValue("@id", data.id);
            cmd.Parameters.AddWithValue("@description", data.description);
            cmd.Parameters.AddWithValue("@status_id", data.status_id);

            int rowsuccess = cmd.ExecuteNonQuery();
            return rowsuccess > 0;
        }

        public List<RequestViewModel> SearchRequest(string keyword, int status, int category)
        {
            List<RequestViewModel> Requests = new();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var sql = new StringBuilder("SELECT Re.[id], Assets.name AS name, brand, model,[Users].[first_name] + ' ' + [Users].[last_name] AS username," +
                "AC.name AS category, [description], StatusTypes.name AS status, [created_at] FROM [RepairRequests] AS Re JOIN [Assets] ON Re.[asset_id] = Assets.id " +
                "JOIN [Users] ON [Users].id = Re.user_id JOIN [AssetCategories] AS AC ON AC.id = Re.category_id JOIN [StatusTypes] ON StatusTypes.id = Re.status_id WHERE 1=1"
            );

            var cmd = new SqlCommand();
            cmd.Connection = conn;

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sql.Append("AND ( Assets.name LIKE @keyword OR ([Users].[first_name] + ' ' + [Users].[last_name]) LIKE @keyword OR description LIKE @keyword OR brand" +
                    " LIKE @keyword OR model LIKE @keyword)"
                );
                cmd.Parameters.AddWithValue("@keyword", $"%{keyword}%");
            }

            if (status != 0)
            {
                sql.Append(" AND StatusTypes.id = @status ");
                cmd.Parameters.AddWithValue("@status", status);
            }


            if (category != 0)
            {
                sql.Append(" AND AC.id = @category ");
                cmd.Parameters.AddWithValue("@category", category);
            }
            cmd.CommandText = sql.ToString();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Requests.Add(new RequestViewModel
                {
                    id = reader.GetInt32(reader.GetOrdinal("id")),
                    asset_name = reader.GetString(reader.GetOrdinal("name")),
                    user_name = reader.GetString(reader.GetOrdinal("username")),
                    category_name = reader.GetString(reader.GetOrdinal("category")),
                    description = reader.GetString(reader.GetOrdinal("description")),
                    status_name = reader.GetString(reader.GetOrdinal("status")),
                    created_at = reader.GetDateTime(reader.GetOrdinal("created_at")),
                    brand = reader.GetString(reader.GetOrdinal("brand")),
                    model = reader.GetString(reader.GetOrdinal("model")),
                });
            }


            return Requests;
        }
    }
}



