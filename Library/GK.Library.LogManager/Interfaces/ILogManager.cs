using GK.Library.Entities.CustomEntities;
using System;
namespace GK.Library.LogManager.Interface
{
    public interface ILogManager
    {
        void Log(LogEntity logEntity);
        void Log(string applicationName, string functionName, string logKey, string detail, LogEntity.EventType eventType);
    }
}
