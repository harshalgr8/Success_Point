using Dapper;
using MySqlConnector;
using SucessPointCore.Domain.Entities;
using SucessPointCore.Domain.Helpers;
using SucessPointCore.Infrastructure.Interfaces;
using System.Data;

namespace SucessPointCore.Infrastructure.Repositories
{
    public class StandardRepository : IStandardRepository
    {
        public IEnumerable<Standard> GetStandardList()
        {

            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();

                    var result = conn.Query<Standard>("sp_sp_standard_GetStandardList", param: parameters);

                    return result;

                }
                catch (Exception)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    throw;
                }
            }
        }

        public int CreateStandard(string standardName, int createdBy)
        {
            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_StandardName", standardName);
                    parameters.Add("p_CreatedBy", createdBy);

                    var result = conn.Execute("sp_SP_Standard_Insert", param: parameters);

                    return result;

                }
                catch (Exception)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    throw;
                }
            }
        }


    }
}
