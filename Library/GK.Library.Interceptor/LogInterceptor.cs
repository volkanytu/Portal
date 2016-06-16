using Castle.DynamicProxy;
using GK.Library.LogManager.Interface;
using System;
using System.Diagnostics;

namespace GK.Library.Interceptor
{
    public class LogInterceptor : BaseLogInterceptor, IInterceptor
    {
        public LogInterceptor(ILogManager logManager)
            : base(logManager)
        {

        }

        public void Intercept(IInvocation invocation)
        {
            Stopwatch sWatch = new Stopwatch();
            sWatch.Start();

            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                base.Log(invocation, ex);
            }

            sWatch.Stop();
        }
    }
}
