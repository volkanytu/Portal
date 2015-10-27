using System;
using System.Data;
using System.Data.SqlClient;

namespace GK.Library.Utility
{

    public class SqlDataAccess
    {
        private SqlConnection conn;

        private string connectionString;

        public void openConnection(string connectionString)
        {
            try
            {              
                    this.connectionString = connectionString;
                    conn = new SqlConnection(connectionString);
                    conn.Open();              
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public ConnectionState GetConnectionState()
        {
            return conn.State;
        }

        public DataTable getDataTable(string query)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandTimeout = 60 * 3600;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                return dt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

        public DataTable getDataTable(string query, params SqlParameter[] parameters)
        {
            DataTable resultSet = new DataTable();

            using (SqlCommand cmdMSSQL = new SqlCommand(query, conn))
            {
                try
                {
                    cmdMSSQL.CommandTimeout = 60 * 3600;

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (parameters[i] != null)
                            cmdMSSQL.Parameters.Add(parameters[i]);
                    }

                    using (SqlDataAdapter daMSSQL = new SqlDataAdapter())
                    {
                        daMSSQL.SelectCommand = cmdMSSQL;

                        daMSSQL.Fill(resultSet);
                    }

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }


            return resultSet;
        }

        public DataTable getDataTableSP(string spName, params SqlParameter[] parameters)
        {
            DataTable resultSet = new DataTable();

            using (SqlCommand cmdMSSQL = new SqlCommand())
            {
                try
                {
                    cmdMSSQL.CommandTimeout = 60 * 3600;

                    cmdMSSQL.CommandType = CommandType.StoredProcedure;

                    cmdMSSQL.CommandText = spName;

                    cmdMSSQL.Connection = conn;

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (parameters[i] != null)
                            cmdMSSQL.Parameters.Add(parameters[i]);
                    }

                    using (SqlDataAdapter daMSSQL = new SqlDataAdapter(cmdMSSQL))
                    {
                        daMSSQL.SelectCommand = cmdMSSQL;

                        daMSSQL.Fill(resultSet);
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            return resultSet;
        }

        public void ExecuteNonQuery(string query)
        {

            try
            {
                using (SqlCommand cmdMSSQL = new SqlCommand(query, conn))
                {
                    cmdMSSQL.CommandTimeout = 60 * 3600;

                    cmdMSSQL.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.ConnectionString = connectionString;
                conn.Open();
                //openConnection(connectionString);
            }

            using (SqlCommand cmdMSSQL = new SqlCommand(query, conn))
            {
                try
                {
                    cmdMSSQL.CommandTimeout = 60 * 3600;

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (parameters[i] != null)
                            cmdMSSQL.Parameters.Add(parameters[i]);
                    }

                    cmdMSSQL.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public object ExecuteScalar(string query)
        {
            object result = null;

            if (conn.State != ConnectionState.Open)
            {
                conn.ConnectionString = connectionString;
                conn.Open();
                //openConnection(connectionString);
            }

            using (SqlCommand cmdMSSQL = new SqlCommand(query, conn))
            {
                try
                {
                    cmdMSSQL.CommandTimeout = 60 * 3600;

                    result = cmdMSSQL.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return result;
        }

        public object ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            object result = null;

            using (SqlCommand cmdMSSQL = new SqlCommand(query, conn))
            {
                try
                {

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        cmdMSSQL.Parameters.Add(parameters[i]);
                    }

                    cmdMSSQL.CommandTimeout = 60 * 3600;

                    result = cmdMSSQL.ExecuteScalar();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return result;
            }

        }

        public DataSet ExecuteDataSet(string query)
        {
            DataSet resultSet = new DataSet();

            using (SqlCommand cmdMSSQL = new SqlCommand(query, conn))
            {
                try
                {
                    using (SqlDataAdapter daMSSQL = new SqlDataAdapter())
                    {
                        daMSSQL.SelectCommand = cmdMSSQL;

                        daMSSQL.Fill(resultSet);
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return resultSet;
        }

        public void closeConnection()
        {
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
