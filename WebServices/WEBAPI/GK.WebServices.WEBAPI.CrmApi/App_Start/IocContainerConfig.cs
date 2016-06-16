using Autofac;
using Autofac.Integration.WebApi;
using Autofac.Extras.DynamicProxy2;
using System.Web;
using System.Web.Mvc;
using GK.Library.IocManager;
using System.Web.Http;
using System.Reflection;

namespace GK.WebServices.WEBAPI.CrmApi
{
    public class IocContainerConfig
    {
        private static string APPLICATION_NAME = "CRM_API";

        public static void BuildIocContainer()
        {
            var builder = new ContainerBuilder();

            builder = IocContainerBuilder.GetCrmApiIocContainer(builder, APPLICATION_NAME);

            builder = IocContainerBuilder.GetInterceptorIocContainer(builder);

            var config = GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
