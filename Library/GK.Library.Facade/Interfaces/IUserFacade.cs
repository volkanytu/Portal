using GK.Library.Entities.CrmEntities;
using GK.Library.Entities.CustomEntities;
using System;
namespace GK.Library.Facade.Interfaces
{
   public interface IUserFacade
    {
        ResponseContainer<SessionData> LoginUser(User userData);
    }
}
