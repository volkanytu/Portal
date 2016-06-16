using GK.Library.ConfigManager.Interfaces;
using GK.Library.Data.Interfaces;
using GK.Library.Data.SqlDataLayer.Interfaces;
using GK.Library.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GK.Library.ConfigManager
{
    public class DBConfigManager : ConfigManager, IConfigManager
    {
        private IDBConfigDao _dbConfigDao;

        public DBConfigManager(IDBConfigDao dbConfigDao)
        {
            _dbConfigDao = dbConfigDao;

            base.StartConfigCache();
        }

        public string GetKeyValue(string key)
        {
            return base.GetValue(key);
        }

        public string this[string key]
        {
            get
            {
                return base.GetValue(key);
            }
        }

        public override void FillConfigValues()
        {
            ConfigCollection configCollection = _dbConfigDao.GetConfigVaribales();

            base.SetConfigValuesToCache(configCollection);

        }
    }
}
