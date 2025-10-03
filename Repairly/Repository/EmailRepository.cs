using Repairly.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Repairly.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly string _connectionString;

        public EmailRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }
        public List<EmailViewModel> GetAllData()
        {
            List<EmailViewModel> statusSuccess = new();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand("SELECT Re.[id] ,Assets.name as asset_name,brand,model,[first_name]+' '+[last_name] AS username ,AC.name AS category ," +
                "[description],StatusTypes.name as status ,[created_at] ,email FROM [RepairRequests] AS Re JOIN [Assets] ON Re.[asset_id] = Assets.id JOIN [Users] ON" +
                "[Users].id = user_id JOIN [AssetCategories] AS AC ON AC.id = Re.[category_id] JOIN [StatusTypes] ON [StatusTypes].id = status_id WHERE status_id = 2", conn);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                statusSuccess.Add(new EmailViewModel
                {
                    id = reader.GetInt32(reader.GetOrdinal("id")),
                    asset_name = reader.GetString(reader.GetOrdinal("asset_name")),
                    brand = reader.GetString(reader.GetOrdinal("brand")),
                    model = reader.GetString(reader.GetOrdinal("model")),
                    username = reader.GetString(reader.GetOrdinal("username")),
                    category = reader.GetString(reader.GetOrdinal("category")),
                    description = reader.GetString(reader.GetOrdinal("description")),
                    email = reader.GetString(reader.GetOrdinal("email")),
                    status = reader.GetString(reader.GetOrdinal("status")),

                });
            }
            return statusSuccess;

        }
    }
}
