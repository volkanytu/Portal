using System;
namespace GK.Library.ConfigManager.Interfaces
{
    public interface IConfigs
    {
        string DOMAIN_NAME { get; }
        string CRM_ADMIN_NAME { get; }
        string CRM_ADMIN_USER_NAME { get; }
        string CRM_ADMIN_PASSWORD { get; }
        string CRM_ADMIN_ID { get; }
        string CRM_URL { get; }
        string CRM_ORGANIZATION_NAME { get; }
        string CRM_ORGANIZATION_SERVICE_URL { get; }
        string CRM_DATABASE_NAME { get; }
        string CRM_CONNECTION_STRING { get; }
    }
}
