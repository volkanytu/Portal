using GK.Library.Entities.CrmEntities;
using GK.Library.Entities.CustomEntities;
using System;
namespace GK.Library.Business.Interfaces
{
    public interface IUserBusiness
    {
        ResponseContainer<SessionData> LoginUser(User userData);
    }
}
