using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Utility
{
    public class Globals
    {
        public static string DomainName
        {
            get { return RegistryHelper.Get.Value("DomainName"); }
        }

        public static string AdminName
        {
            get { return RegistryHelper.Get.Value("AdminName"); }
        }

        public static string AdminUserName
        {
            get { return RegistryHelper.Get.Value("AdminUserName"); }
        }

        public static string AdminPassword
        {
            get { return RegistryHelper.Get.Value("AdminPassword"); }
        }

        public static string AdminId
        {
            get { return RegistryHelper.Get.Value("AdminId"); }
        }

        public static string CrmUrl
        {
            get { return RegistryHelper.Get.Value("CrmUrl"); }
        }

        public static string OrganizationName
        {
            get { return RegistryHelper.Get.Value("OrganizationName"); }
        }

        public static string OrganizationServiceUrl
        {
            get { return RegistryHelper.Get.Value("OrganizationServiceUrl"); }
        }

        public Guid AdministratorId
        {
            get { return new Guid(RegistryHelper.Get.Value("AdminId")); }
        }

        public static string DatabaseName
        {
            get { return RegistryHelper.Get.Value("DatabaseName"); }
        }

        public static string ConnectionString
        {
            get { return RegistryHelper.Get.Value("ConnectionString"); }
        }

        public static string AttachmentFolder
        {
            get { return RegistryHelper.Get.Value("AttachmentFolder"); }
        }

        public static string CrmAttachmentFolder
        {
            get { return RegistryHelper.Get.Value("CrmAttachmentFolder"); }
        }

        public static int InterlinkCatalogId
        {
            get { return Convert.ToInt32(RegistryHelper.Get.Value("InterlinkCatalogId")); }
        }

        public static int InterlinkPartnerId
        {
            get { return Convert.ToInt32(RegistryHelper.Get.Value("InterlinkPartnerId")); }
        }

        public static string FileLogPath
        {
            get { return RegistryHelper.Get.Value("FileLogPath"); }
        }

        public static string SmsConfigurationDoluHayatId
        {
            get { return RegistryHelper.Get.Value("SmsConfigurationDoluHayatId"); }
        }

        public static string DefaultPortalId
        {
            get { return RegistryHelper.Get.Value("DefaultPortalId"); }
        }

        public static string IsSendMailActive
        {
            get { return RegistryHelper.Get.Value("IsSendMailActive"); }
        }

        public static string ElasticServerUrl
        {
            get { return RegistryHelper.Get.Value("ElasticServerUrl"); }
        }

        public static string ElasticLogIndexName
        {
            get { return RegistryHelper.Get.Value("ElasticLogIndexName"); }
        }       
        

    }
}
