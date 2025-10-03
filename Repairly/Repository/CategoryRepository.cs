using Repairly.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Repairly.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _connectionString;
        public CategoryRepository (IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public List<CategoryViewModel> GetAllData()
        {
            List<CategoryViewModel> cate = new();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand("SELECT id ,name FROM [AssetCategories]", conn);
            using var reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                cate.Add(new CategoryViewModel
                {
                    id = reader.GetInt32(reader.GetOrdinal("id")),
                    name = reader.GetString(reader.GetOrdinal("name")),
                });
            }
            return cate;

        }
        public bool CreateCate(CategoryViewModel data)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand("IF NOT EXISTS (SELECT 1 FROM  [AssetCategories] WHERE name = @data) BEGIN INSERT INTO AssetCategories (name) VALUES (@data) END ", conn);
            cmd.Parameters.AddWithValue("@data", data.name);

            int rowsuccess = cmd.ExecuteNonQuery();



            return rowsuccess >0;
        }

        public bool DeleteCate(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand("DELETE [AssetCategories] WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            int rowsuccess = cmd.ExecuteNonQuery();



            return rowsuccess > 0;
        }

        public CategoryViewModel getId(int id)
        {
            CategoryViewModel cate = new();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand("SELECT  id,name FROM [AssetCategories] WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                cate.id = reader.GetInt32(reader.GetOrdinal("id"));
                cate.name = reader.GetString(reader.GetOrdinal("name"));
            }

            return cate;
        }

        public bool UpdateCate(CategoryViewModel data)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand("UPDATE  [AssetCategories]  SET name = @name WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", data.id);
            cmd.Parameters.AddWithValue("@name", data.name);
            int rowsuccess = cmd.ExecuteNonQuery();



            return rowsuccess > 0;
        }
    }
}
