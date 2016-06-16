using GK.Library.Constants.SqlQueries;
using GK.Library.Data.Interfaces;
using GK.Library.Data.SqlDataLayer.Interfaces;
using GK.Library.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GK.Library.Data.SqlDataLayer
{
    public class SqlLogDao : ILog
    {
        private ISqlAccess _sqlAccess;
        private string _applicationName;

        public SqlLogDao(ISqlAccess sqlAccess, string applicationName)
        {
            _sqlAccess = sqlAccess;
            _applicationName = applicationName;
        }

        public void Log(LogEntity logEntity)
        {

            SqlParameter[] parameters = new SqlParameter[] 
            { 
                new SqlParameter("@ApplicationName", _applicationName),
                new SqlParameter("@FunctionName", logEntity.FunctionName),
                new SqlParameter("@EventType", (int)logEntity.LogEventType),
                new SqlParameter("@Detail", logEntity.Detail)
            };

            _sqlAccess.ExecuteNonQuery(LogQueries.INSERT_LOG, parameters);
        }
    }
}
