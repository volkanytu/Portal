using Castle.DynamicProxy;
using GK.Library.Entities.CustomEntities;
using System;
using System.Diagnostics;

namespace GK.Library.Interceptor
{
    public class CustomExceptionInterceptor : IInterceptor
    {
        public CustomExceptionInterceptor()
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
            catch (CustomException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                string functionName = string.Format("{0}_{1}", invocation.TargetType.Name, invocation.Method.Name);
                string logKey = ex.GetType().Name;

                ValidationItem validationItem = new ValidationItem(logKey);
                validationItem.ErrorMessage = ex.Message;

                throw new CustomException(validationItem);
            }

            sWatch.Stop();
        }
    }
}
