using System;
namespace GK.Library.Data.SqlDataLayer.Interfaces
{
    public interface ISqlAccess:IDisposable
    {
        System.Data.DataSet ExecuteDataSet(string query);
        int ExecuteNonQuery(string query);
        int ExecuteNonQuery(string query, params System.Data.SqlClient.SqlParameter[] parameters);
        object ExecuteScalar(string query);
        object ExecuteScalar(string query, params System.Data.SqlClient.SqlParameter[] parameters);
        System.Data.DataTable GetDataTable(string query);
        System.Data.DataTable GetDataTable(string query, params System.Data.SqlClient.SqlParameter[] parameters);
        System.Data.DataTable GetDataTableSP(string spName, params System.Data.SqlClient.SqlParameter[] parameters);
    }
}
