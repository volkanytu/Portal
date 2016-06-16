using GK.Library.Constants.SqlQueries;
using GK.Library.Data.Interfaces;
using GK.Library.Data.SqlDataLayer.Interfaces;
using GK.Library.Entities.CustomEntities;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GK.Library.Data.SqlDataLayer
{
    public class DBConfigDao : IDBConfigDao
    {
        private ISqlEntityAccess _sqlEntityAccess;

        private string _PKName;

        public DBConfigDao(ISqlEntityAccess sqlEntityAccess)
        {
            _sqlEntityAccess = sqlEntityAccess;
        }

        public ConfigCollection GetConfigVaribales()
        {
            ConfigCollection returnValue = new ConfigCollection();

            DataTable dt = _sqlEntityAccess.SqlAccess.GetDataTable(ConfigManagerQueries.GET_CONFIG_VARS);

            returnValue.AddRange(dt.ToKeyValueList<string, string>());

            return returnValue;
        }
    }
}