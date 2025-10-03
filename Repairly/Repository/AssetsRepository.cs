using Microsoft.AspNetCore.Mvc.Rendering;
using Repairly.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Repairly.Repository
{
    public class AssetsRepository : IAssetsRepository
    {
        private readonly string _connectionString;
        public AssetsRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public List<AssetsViewModel> GetAssets(int page ,int pagesize ,out int totalRecord)
        {

            List<AssetsViewModel> assets = new();
            totalRecord =0;
            try {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var Record = new SqlCommand("SELECT COUNT(*) FROM Assets", conn);
            totalRecord = (int)Record.ExecuteScalar();

            using var cmd = new SqlCommand("GetPagesAsset", conn);
        
            cmd.Parameters.AddWithValue("@Offset", (page - 1) * pagesize);
            cmd.Parameters.AddWithValue("@PageSize", pagesize);
                cmd.CommandType = CommandType.StoredProcedure;
                using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                assets.Add(new AssetsViewModel
                {
                    
                    Assets_id = reader.GetInt32(reader.GetOrdinal("Assets_id")),
                    Asset_code = reader.GetString(reader.GetOrdinal("Asset_code")),
                    Asset_name = reader.GetString(reader.GetOrdinal("Asset_name")),
                    Category = reader.GetString(reader.GetOrdinal("Category")),
                    Brand = reader.GetString(reader.GetOrdinal("Brand")),
                    Model = reader.GetString(reader.GetOrdinal("Model")),
                    Location = reader.GetString(reader.GetOrdinal("Location")),
                    Purchase_date = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("Purchase_date"))),
                    Warranty_date = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("Warranty_date"))),

                });
            }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return assets;
        }


        public List<SelectListItem> GetItem()
        {
            var items = new List<SelectListItem>();


        
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                using var cate = new SqlCommand("SELECT id,name FROM AssetCategories", conn);
                using var reader = cate.ExecuteReader();
                while (reader.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Value = reader.GetInt32(reader.GetOrdinal("id")).ToString(),
                        Text = reader.GetString(reader.GetOrdinal("name")),
                    });
                }


            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return (items);

        }

         public bool UpdateAsset(AssetsPageViewModel data)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand("UPDATE [Assets] SET [asset_code]= @asset_code,[name]=@name,[category_id]=@category_id,[brand]=@brand,[model]=@model," +
                "[location]=@location,[purchase_date]=@purchase_date,[warranty_date]=@warranty_date WHERE [id] = @id    ", conn);
            cmd.Parameters.AddWithValue("@id", data.FormData.Assets_id);
            cmd.Parameters.AddWithValue("@asset_code", data.FormData.Asset_code);
            cmd.Parameters.AddWithValue("@name", data.FormData.Asset_name);
            cmd.Parameters.AddWithValue("@category_id", data.FormData.Category_id);
            cmd.Parameters.AddWithValue("@brand", data.FormData.Brand);
            cmd.Parameters.AddWithValue("@model", data.FormData.Model);
            cmd.Parameters.AddWithValue("@location", data.FormData.Location);
            cmd.Parameters.AddWithValue("@purchase_date", data.FormData.Purchase_date.ToDateTime(TimeOnly.MinValue));
            cmd.Parameters.AddWithValue("@warranty_date", data.FormData.Warranty_date.ToDateTime(TimeOnly.MinValue));

            int rowsuccess = cmd.ExecuteNonQuery();
            return rowsuccess > 0;


        }
        public bool CreateAsset(AssetsPageViewModel model)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();
                using var cmd = new SqlCommand("INSERT INTO Assets (asset_code,name,category_id ,brand,model,location,purchase_date,warranty_date ) VALUES (@asset_code,@name,@category_id ,@brand,@model,@location,@purchase_date,@warranty_date )", conn);

                cmd.Parameters.AddWithValue("@asset_code", model.FormData.Asset_code);
                cmd.Parameters.AddWithValue("@name", model.FormData.Asset_name);
                cmd.Parameters.AddWithValue("@category_id", model.FormData.Category_id);
                cmd.Parameters.AddWithValue("@brand", model.FormData.Brand);
                cmd.Parameters.AddWithValue("@model", model.FormData.Model);
                cmd.Parameters.AddWithValue("@location", model.FormData.Location);
                cmd.Parameters.AddWithValue("@purchase_date", model.FormData.Purchase_date.ToDateTime(TimeOnly.MinValue));
                cmd.Parameters.AddWithValue("@warranty_date", model.FormData.Warranty_date.ToDateTime(TimeOnly.MinValue));
                int rowAffected = cmd.ExecuteNonQuery();
                return rowAffected > 0;

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public FormAsset GetAssetById(int id)
        {

            FormAsset asset = null;


            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand("SELECT id, [asset_code], [name], [category_id], [brand], [model], [location], [purchase_date], [warranty_date] FROM Assets WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
          
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    asset = new FormAsset
                    {
                        Assets_id = reader.GetInt32(0),
                        Asset_code = reader.GetString(1),
                        Asset_name = reader.GetString(2),
                        Category_id = reader.GetInt32(3),
                        Brand = reader.GetString(4),
                        Model = reader.GetString(5),
                        Location = reader.GetString(6),
                        Purchase_date = DateOnly.FromDateTime(reader.GetDateTime(7)),
                        Warranty_date = DateOnly.FromDateTime(reader.GetDateTime(8))
                    };
                }
            }



            return asset;
        }

        public bool DeleteAsset(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand("DELETE [Assets] WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            int delsuccess = cmd.ExecuteNonQuery();
            return delsuccess > 0;
        }

        public List<AssetsViewModel> SearchAsset(string data)
        {
            List<AssetsViewModel> asset = new();
            try
            {
              
            
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand("SELECT Assets.id as idAss, asset_code, Assets.name AS nameAss, ASS.name AS category, brand, model, location, purchase_date, warranty_date FROM Assets JOIN AssetCategories AS ASS ON ASS.id = Assets.category_id WHERE LOWER(asset_code) LIKE @data OR LOWER(brand) LIKE @data OR LOWER(Assets.name) LIKE @data OR LOWER(model) LIKE @data OR LOWER(location) LIKE @data", conn);

            cmd.Parameters.AddWithValue("@data", $"%{data.ToLower()}%");


            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                asset.Add(new AssetsViewModel
                {

                    Assets_id = reader.GetInt32(reader.GetOrdinal("idAss")),
                    Asset_code = reader.GetString(reader.GetOrdinal("asset_code")),
                    Asset_name = reader.GetString(reader.GetOrdinal("nameAss")),
                    Category = reader.GetString(reader.GetOrdinal("category")),
                    Brand = reader.GetString(reader.GetOrdinal("brand")),
                    Model = reader.GetString(reader.GetOrdinal("model")),
                    Location = reader.GetString(reader.GetOrdinal("location")),
                    Purchase_date = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("purchase_date"))),
                    Warranty_date = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("warranty_date"))),
                });

            }

                return asset;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return asset;
        }
    }
}








