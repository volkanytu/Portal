using Castle.DynamicProxy;
using GK.Library.Common;
using GK.Library.Entities.CustomEntities;
using GK.Library.LogManager.Interface;
using Globals = GK.Library.Utility.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GK.Library.Interceptor
{
    public class BaseLogInterceptor
    {
        private ILogManager _logManager;
        private const string APPLICATION_NAME = "BaseLogInterceptor";

        public BaseLogInterceptor(ILogManager logManager)
        {
            _logManager = logManager;
        }

        protected void Log(IInvocation invocation, Exception ex, string identifier = null, int? tryCount = null)
        {
            try
            {
                string logKey = ex.GetType().Name;

                if (ex is CustomException)
                {
                    logKey = ((CustomException)ex).LogKey;
                }

                LogEntity log = new LogEntity();
                log.FunctionName = string.Format("{0}_{1}", invocation.TargetType.Name, invocation.Method.Name);
                log.Detail = ex.Message;
                log.CreatedOn = DateTime.Now;
                log.LogKey = logKey;
                log.LogEventType = LogEntity.EventType.Exception;
                log.ContextIdentifier = identifier;
                log.TryCount = tryCount;

                _logManager.Log(log);
            }
            catch (Exception exc)
            {
                FileHelper.WriteToText(APPLICATION_NAME, exc.Message, @Globals.FileLogPath);

            }
        }
    }
}
