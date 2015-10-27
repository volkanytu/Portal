using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel.Description;
using System.Net;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk;

namespace GK.Library.Utility
{
    public class MSCRM
    {
        public static IOrganizationService GetOrgService(bool admin = false, string callerId = null, string organization = null)
        {
            ClientCredentials credential = new ClientCredentials();

            if (Globals.OrganizationServiceUrl.Contains("https"))
            {
                credential.Windows.ClientCredential = admin ? new NetworkCredential(Globals.AdminUserName, Globals.AdminPassword, Globals.DomainName) : CredentialCache.DefaultNetworkCredentials;
                credential.UserName.UserName = Globals.DomainName + @"\" + Globals.AdminUserName;
                credential.UserName.Password = Globals.AdminPassword;
            }
            else
            {
                credential.Windows.ClientCredential = admin ? new NetworkCredential(Globals.AdminUserName, Globals.AdminPassword, Globals.DomainName) : CredentialCache.DefaultNetworkCredentials;
            }


            OrganizationServiceProxy orgServiceProxy = new OrganizationServiceProxy(new Uri(Globals.OrganizationServiceUrl), null, credential, null);
            if (!string.IsNullOrEmpty(callerId))
            {
                orgServiceProxy.CallerId = new Guid(callerId);
            }
            return orgServiceProxy;

            ////credential.Windows.ClientCredential = admin ? new NetworkCredential(Globals.AdminUserName, Globals.AdminPassword, Globals.DomainName) : CredentialCache.DefaultNetworkCredentials;
            ////credential.UserName.UserName = Globals.DomainName + @"\" + Globals.AdminUserName;
            ////credential.UserName.Password = Globals.AdminPassword;
            ////OrganizationServiceProxy orgServiceProxy = new OrganizationServiceProxy(new Uri(Globals.OrganizationServiceUrl), null, credential, null);
            ////if (!string.IsNullOrEmpty(callerId))
            ////{
            ////    orgServiceProxy.CallerId = new Guid(callerId);
            ////}
            ////return orgServiceProxy;

        }

        private static IOrganizationService adminService = null;
        private static readonly object lockthread = new object();

        public static IOrganizationService AdminOrgService
        {
            get
            {
                lock (lockthread)
                {
                    if (adminService == null)
                    {
                        adminService = GetOrgService(true);
                    }
                    return adminService;
                }
            }
        }
    }
}
