using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Net;
using System.ServiceModel.Description;
using System.Threading;
using GK.Library.Data.SqlDataLayer.Interfaces;

namespace GK.Library.Data.SqlDataLayer
{
    public class MsCrmAccess : IMsCrmAccess
    {
        private readonly string _adminConnectionString;
        private readonly string _integratedSecurityConnectionString;
        private ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();

        public MsCrmAccess(string adminConnectionString, string integratedSecurityConnectionString)
        {
            _adminConnectionString = adminConnectionString;
            _integratedSecurityConnectionString = integratedSecurityConnectionString;
        }

        private object servicelock = new object();

        public IOrganizationService GetAdminService()
        {
            cacheLock.EnterReadLock();

            try
            {
                CrmConnection connection = CrmConnection.Parse(_adminConnectionString);

                connection.ServiceConfigurationInstanceMode = ServiceConfigurationInstanceMode.PerRequest;
                connection.Timeout = new TimeSpan(2, 0, 0);
                connection.UserTokenExpiryWindow = new TimeSpan(0, 1, 0);

                OrganizationService service = new OrganizationService(connection);

                return service;
            }
            finally
            {
                cacheLock.ExitReadLock();
            }
        }

        public IOrganizationService GetCurrentUserService()
        {
            cacheLock.EnterReadLock();

            try
            {
                CrmConnection connection = CrmConnection.Parse(_integratedSecurityConnectionString);

                connection.ServiceConfigurationInstanceMode = ServiceConfigurationInstanceMode.PerRequest;
                connection.Timeout = new TimeSpan(2, 0, 0);
                connection.UserTokenExpiryWindow = new TimeSpan(0, 1, 0);

                OrganizationService service = new OrganizationService(connection);

                return service;
            }
            finally
            {
                cacheLock.ExitReadLock();
            }
        }

        public IOrganizationService GetBehalfOfUserService(Guid userId)
        {
            cacheLock.EnterReadLock();

            try
            {
                CrmConnection connection = CrmConnection.Parse(_adminConnectionString);

                connection.ServiceConfigurationInstanceMode = ServiceConfigurationInstanceMode.PerRequest;
                connection.Timeout = new TimeSpan(2, 0, 0);
                connection.UserTokenExpiryWindow = new TimeSpan(0, 1, 0);
                connection.CallerId = userId;

                OrganizationService service = new OrganizationService(connection);

                return service;
            }
            finally
            {
                cacheLock.ExitReadLock();
            }
        }
    }
}
