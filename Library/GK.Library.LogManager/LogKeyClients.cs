using GK.Library.Entities.CustomEntities;
using GK.Library.LogManager.Interfaces;
using System.Collections.Generic;

namespace GK.Library.LogManager
{
    public class LogKeyClients : ILogKeyClients
    {
        private Dictionary<string, List<LogEntity.LogClientType>> _logKeyClientDict = null;

        public LogKeyClients()
        {
            FillLogKeyClients();
        }

        public List<LogEntity.LogClientType> GetLogKeyClientTypeList(string logKey)
        {
            List<LogEntity.LogClientType> logDataList = null;

            if (!_logKeyClientDict.TryGetValue(logKey, out logDataList))
            {
                logDataList = new List<LogEntity.LogClientType>()
                {
                    LogEntity.LogClientType.ELASTIC
                };
            }

            return logDataList;
        }

        private void FillLogKeyClients()
        {
            _logKeyClientDict = new Dictionary<string, List<LogEntity.LogClientType>>();

            _logKeyClientDict.Add("USER_NOT_FOUND", new List<LogEntity.LogClientType>() { LogEntity.LogClientType.FILE, LogEntity.LogClientType.ELASTIC });
            _logKeyClientDict.Add("VOLKAN", new List<LogEntity.LogClientType>() { LogEntity.LogClientType.FILE});
        }
    }
}
