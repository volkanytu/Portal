using GK.Library.Entities.CustomEntities;
using System;
using System.Collections.Generic;

namespace GK.Library.LogManager.Interfaces
{
    public interface ILogKeyClients
    {
        List<LogEntity.LogClientType> GetLogKeyClientTypeList(string logKey);
    }
}
