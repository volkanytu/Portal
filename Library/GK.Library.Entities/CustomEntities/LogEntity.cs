using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GK.Library.Entities.CustomEntities
{
    public class LogEntity
    {
        public string Id { get; set; }
        public string ContextIdentifier { get; set; }
        public string ApplicationName { get; set; }
        public string FunctionName { get; set; }
        public string LogKey { get; set; }
        public string Detail { get; set; }
        public DateTime? CreatedOn { get; set; }
        public EventType? LogEventType { get; set; }
        public int? TryCount { get; set; }


        public enum EventType
        {
            Info = 100000000,
            Warning,
            Exception
        }

        public enum LogClientType
        {
            SQL,
            ELASTIC,
            FILE
        }
    }
}
