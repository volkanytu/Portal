using System.Data;
using System.Data.SqlClient;
using GK.Library.Data.SqlDataLayer.Interfaces;

namespace GK.Library.Data.SqlDataLayer
{
    public class SqlAccess : ISqlAccess
    {
        private readonly string _connectionString;
        private const int SQL_COMMAND_TIME_OUT = 60 * 3600;

        public SqlAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable GetDataTable(string query)
        {
            return GetDataTable(query, new SqlParameter[0]);
        }

        public DataTable GetDataTable(string query, params SqlParameter[] parameters)
        {
            return GetDataTable(query, CommandType.Text, parameters);
        }

        public DataTable GetDataTableSP(string spName, params SqlParameter[] parameters)
        {
            return GetDataTable(spName, CommandType.StoredProcedure, parameters);
        }

        private DataTable GetDataTable(string text, CommandType commandType, params SqlParameter[] parameters)
        {
            DataTable resultSet = new DataTable();

            using (SqlConnection sqlConn = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(text, sqlConn))
                {
                    sqlCmd.CommandTimeout = SQL_COMMAND_TIME_OUT;
                    sqlCmd.CommandType = commandType;
                    sqlCmd.Parameters.AddRange(parameters);

                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCmd))
                    {
                        sqlDataAdapter.SelectCommand = sqlCmd;
                        sqlDataAdapter.Fill(resultSet);
                    }
                }
            }

            return resultSet;
        }

        public DataSet ExecuteDataSet(string query)
        {
            DataSet resultSet = new DataSet();

            using (SqlConnection sqlConn = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlConn))
                {
                    sqlCmd.CommandTimeout = SQL_COMMAND_TIME_OUT;

                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCmd))
                    {
                        sqlDataAdapter.SelectCommand = sqlCmd;
                        sqlDataAdapter.Fill(resultSet);
                    }
                }
            }

            return resultSet;
        }

        public int ExecuteNonQuery(string query)
        {
            return ExecuteNonQuery(query, new SqlParameter[0]);
        }

        public int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection sqlConn = new SqlConnection(_connectionString))
            {
                sqlConn.Open();

                using (SqlCommand sqlCmd = new SqlCommand(query, sqlConn))
                {
                    sqlCmd.CommandTimeout = SQL_COMMAND_TIME_OUT;
                    sqlCmd.Parameters.AddRange(parameters);
                    return sqlCmd.ExecuteNonQuery();
                }
            }
        }

        public object ExecuteScalar(string query)
        {
            return ExecuteScalar(query, new SqlParameter[0]);
        }

        public object ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            object result = null;

            using (SqlConnection sqlConn = new SqlConnection(_connectionString))
            {
                sqlConn.Open();

                using (SqlCommand sqlCmd = new SqlCommand(query, sqlConn))
                {
                    sqlCmd.CommandTimeout = SQL_COMMAND_TIME_OUT;
                    sqlCmd.Parameters.AddRange(parameters);
                    result = sqlCmd.ExecuteScalar();
                }
            }

            return result;
        }

        public void Dispose()
        {

        }
    }
}
