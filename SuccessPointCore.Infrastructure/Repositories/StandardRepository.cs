using Dapper;
using MySqlConnector;
using SuccessPointCore.Domain.Entities;
using SuccessPointCore.Domain.Helpers;
using SuccessPointCore.Infrastructure.Interfaces;
using System.Data;

namespace SuccessPointCore.Infrastructure.Repositories
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

        public int UpsertStandard(int standardId, string standardName,bool active, int createdBy)
        {
            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_StandardID", standardId);
                    parameters.Add("p_StandardName", standardName);
                    parameters.Add("p_Active", active);
                    parameters.Add("p_CreatedBy", createdBy);

                    var result = conn.Execute("sp_SP_Standard_Upsert", param: parameters);

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


        public int RemoveStandard(int standardId)
        {
            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_StandardID", standardId);

                    var result = conn.Execute("sp_sp_Standard_Delete", param: parameters);

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
