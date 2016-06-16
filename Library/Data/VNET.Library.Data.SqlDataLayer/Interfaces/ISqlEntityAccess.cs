using Microsoft.Xrm.Sdk;
using System;
namespace GK.Library.Data.SqlDataLayer.Interfaces
{
    public interface ISqlEntityAccess
    {
        object Create(Entity entity, string PKName);
        void Delete(string entityName, Guid id, string PKName);
        void Update(Entity entity, string PKName);
        ISqlAccess SqlAccess { get; }
    }
}
