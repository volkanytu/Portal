using GK.Library.Data.Interfaces;
using GK.Library.Entities.CustomEntities;
using GK.Library.LogManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GK.Library.LogManager
{
    public class LogFactory : ILogFactory
    {
        private Dictionary<LogEntity.LogClientType, ILog> _logDataDict = null;

        public LogFactory(Dictionary<LogEntity.LogClientType, ILog> logDataDict)
        {
            _logDataDict = logDataDict;
        }

        /// <summary>
        /// Decides which class to instantiate.
        /// </summary>
        public ILog GetLogData(LogEntity.LogClientType logClientType)
        {
            ILog logData = null;

            _logDataDict.TryGetValue(logClientType, out logData);

            return logData;
        }
    }
}
