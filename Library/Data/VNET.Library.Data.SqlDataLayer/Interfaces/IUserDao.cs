using GK.Library.Entities.CrmEntities;
using System;
namespace GK.Library.Data.SqlDataLayer.Interfaces
{
    public interface IUserDao
    {
        User GetByName(string name);
    }
}
