using Microsoft.AspNetCore.Http.HttpResults;
using Repairly.Models;
using System.Data;
using System.Data.SqlClient;

namespace Repairly.Repository
{
    public class RepairRequesterRepository : IRepairRequesterRepository
    {
        private readonly string _connectionString;
        public RepairRequesterRepository(IConfiguration _config)
        {
            _connectionString = _config.GetConnectionString("DefaultConnection");

        }
       public List<RepairRequestViewModel> GetRequester()
        {
            var requester = new List<RepairRequestViewModel>();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand("GetOrderRepairs", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               

                

                requester.Add(new RepairRequestViewModel
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    AssetName = reader.GetString(reader.GetOrdinal("AssetName")),
                    RequesterName = reader.GetString(reader.GetOrdinal("RequesterName")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    StatusName = reader.GetString(reader.GetOrdinal("StatusName")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                    Type = reader.GetString(reader.GetOrdinal("Type")),
                 
                });
            }
            return requester;

        }

       
    }
}
