using GK.Library.Data.Interfaces;
using GK.Library.Entities.CustomEntities;
using GK.Library.LogManager.Interface;
using GK.Library.LogManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GK.Library.LogManager
{
    public class Logger : ILogManager
    {
        private ILogFactory _logFactory;
        private ILogKeyClients _logKeyClients;

        public Logger(ILogFactory logFactory, ILogKeyClients logKeyClients)
        {
            _logFactory = logFactory;
            _logKeyClients = logKeyClients;
        }

        public void Log(LogEntity logEntity)
        {
            List<LogEntity.LogClientType> logClientTypeList = _logKeyClients.GetLogKeyClientTypeList(logEntity.LogKey);

            foreach (LogEntity.LogClientType clientType in logClientTypeList)
            {
                ILog logData = _logFactory.GetLogData(clientType);

                logData.Log(logEntity);
            }
        }

        public void Log(string applicationName, string functionName, string logKey, string detail, LogEntity.EventType eventType)
        {
            LogEntity logEntity = new LogEntity()
                                        {
                                            ApplicationName = applicationName,
                                            FunctionName = functionName,
                                            LogKey = logKey,
                                            CreatedOn = DateTime.Now,
                                            Detail = detail,
                                            LogEventType = eventType
                                        };

            Log(logEntity);
        }
    }
}
