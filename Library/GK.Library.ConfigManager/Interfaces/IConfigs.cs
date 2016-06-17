using System;
namespace GK.Library.ConfigManager.Interfaces
{
    public interface IConfigs
    {
        string CRM_ADMIN_ID { get; }
        string CRM_ADMIN_NAME { get; }
        string CRM_ADMIN_PASSWORD { get; }
        string CRM_ADMIN_USER_NAME { get; }
        string CRM_DB_CONNECTION_STRING { get; }
        string CRM_SVC_CONNECTION_STRING { get; }
        string CRM_SVC_IS_CONNECTION_STRING { get; }
        string CRM_URL { get; }
        string CRM_DATABASE_NAME { get; }
        string PORTAL_ID { get; }
        string DOMAIN_NAME { get; }
        string FILE_LOG_PATH { get; }
        string IS_SEND_MAIL_ACTIVE { get; }
        string CRM_ORGANIZATION_NAME { get; }
        string CRM_ORGANIZATION_SERVICE_URL { get; }
        string ELASTIC_SERVER_URL { get; }
        string ELASTIC_CRM_INDEX { get; }
    }
}
