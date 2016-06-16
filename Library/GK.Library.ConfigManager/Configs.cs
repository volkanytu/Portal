using GK.Library.ConfigManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GK.Library.ConfigManager
{
    public class Configs : IConfigs
    {
        private IConfigManager _configManager;

        public Configs(IConfigManager configManager)
        {
            _configManager = configManager;
        }

        public string DOMAIN_NAME { get { return _configManager.GetKeyValue("DOMAIN_NAME"); } }
        public string CRM_ADMIN_NAME { get { return _configManager.GetKeyValue("CRM_ADMIN_NAME"); } }
        public string CRM_ADMIN_USER_NAME { get { return _configManager.GetKeyValue("CRM_ADMIN_USER_NAME"); } }
        public string CRM_ADMIN_PASSWORD { get { return _configManager.GetKeyValue("CRM_ADMIN_PASSWORD"); } }
        public string CRM_ADMIN_ID { get { return _configManager.GetKeyValue("CRM_ADMIN_ID"); } }
        public string CRM_URL { get { return _configManager.GetKeyValue("CRM_URL"); } }
        public string CRM_ORGANIZATION_NAME { get { return _configManager.GetKeyValue("CRM_ORGANIZATION_NAME"); } }
        public string CRM_ORGANIZATION_SERVICE_URL { get { return _configManager.GetKeyValue("CRM_ORGANIZATION_SERVICE_URL"); } }
        public string CRM_DATABASE_NAME { get { return _configManager.GetKeyValue("CRM_DATABASE_NAME"); } }
        public string CRM_CONNECTION_STRING { get { return _configManager.GetKeyValue("CRM_CONNECTION_STRING"); } }

    }
}
