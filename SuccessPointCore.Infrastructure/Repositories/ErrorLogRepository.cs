﻿using Dapper;
using MySqlConnector;
using SuccessPointCore.Domain.Entities;
using SuccessPointCore.Domain.Helpers;
using SuccessPointCore.Infrastructure.Interfaces;
using System.Data;

namespace SuccessPointCore.Infrastructure.Repositories
{
    public class ErrorLogRepository : IErrorLogRepository
    {
        public bool AddError(CreateErrorLog errorData)
        {
            using (IDbConnection conn = new MySqlConnection(AppConfigHelper.ConnectionString))
            {
                try
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_ErrorMessage", errorData.ErrorMesage);
                    parameters.Add("p_StackTrace", errorData.StackTrace);
                    parameters.Add("p_UserID", errorData.UserID);

                    return conn.Execute("sp_ErrorLog_Insert", param: parameters) > 0;
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
