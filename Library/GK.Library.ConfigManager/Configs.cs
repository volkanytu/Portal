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

        public string CRM_ADMIN_ID { get { return _configManager.GetKeyValue("CRM_ADMIN_ID"); } }
        public string CRM_ADMIN_NAME { get { return _configManager.GetKeyValue("CRM_ADMIN_NAME"); } }
        public string CRM_ADMIN_PASSWORD { get { return _configManager.GetKeyValue("CRM_ADMIN_PASSWORD"); } }
        public string CRM_ADMIN_USER_NAME { get { return _configManager.GetKeyValue("CRM_ADMIN_USER_NAME"); } }
        public string CRM_DB_CONNECTION_STRING { get { return _configManager.GetKeyValue("CRM_DB_CONNECTION_STRING"); } }
        public string CRM_SVC_CONNECTION_STRING { get { return _configManager.GetKeyValue("CRM_SVC_CONNECTION_STRING"); } }
        public string CRM_SVC_IS_CONNECTION_STRING { get { return _configManager.GetKeyValue("CRM_SVC_IS_CONNECTION_STRING"); } }
        public string CRM_URL { get { return _configManager.GetKeyValue("CRM_URL"); } }
        public string CRM_DATABASE_NAME { get { return _configManager.GetKeyValue("CRM_DATABASE_NAME"); } }
        public string PORTAL_ID { get { return _configManager.GetKeyValue("PORTAL_ID"); } }
        public string DOMAIN_NAME { get { return _configManager.GetKeyValue("DOMAIN_NAME"); } }
        public string FILE_LOG_PATH { get { return _configManager.GetKeyValue("FILE_LOG_PATH"); } }
        public string IS_SEND_MAIL_ACTIVE { get { return _configManager.GetKeyValue("IS_SEND_MAIL_ACTIVE"); } }
        public string CRM_ORGANIZATION_NAME { get { return _configManager.GetKeyValue("CRM_ORGANIZATION_NAME"); } }
        public string CRM_ORGANIZATION_SERVICE_URL { get { return _configManager.GetKeyValue("CRM_ORGANIZATION_SERVICE_URL"); } }
        public string ELASTIC_SERVER_URL { get { return _configManager.GetKeyValue("ELASTIC_SERVER_URL"); } }
        public string ELASTIC_CRM_INDEX { get { return _configManager.GetKeyValue("ELASTIC_CRM_INDEX"); } }        
    }
}
