using Microsoft.Xrm.Sdk;
using System;
namespace GK.Library.Data.SqlDataLayer.Interfaces
{
    public interface IMsCrmAccess
    {
        IOrganizationService GetAdminService();
        IOrganizationService GetCurrentUserService();
        IOrganizationService GetBehalfOfUserService(Guid userId);
    }
}
