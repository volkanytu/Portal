using Autofac;
using GK.Library.Data.SqlDataLayer;
using GK.Library.Data.SqlDataLayer.Interfaces;
using Globals = GK.Library.Utility.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK.Library.Data.ElasticDataLayer.Interfaces;
using GK.Library.Data.ElasticDataLayer;
using GK.Library.LogManager.Interfaces;
using GK.Library.LogManager;
using GK.Library.Data.Interfaces;
using GK.Library.Entities.CustomEntities;
using GK.Library.LogManager.Interface;
using GK.Library.Business;
using GK.Library.Entities.CrmEntities;
using GK.Library.Interceptor;
using Castle.DynamicProxy;
using Autofac.Extras.DynamicProxy2;
using GK.Library.Business.Interfaces;

namespace GK.Library.IocManager
{
    public class IocContainerBuilder
    {
        private const string SQL_ACCESS_CRM = "SQL_ACCESS_CRM";
        private const string ELASTIC_LOG = "ELASTIC_LOG";
        private const string SQL_LOG = "SQL_LOG";

        public static ContainerBuilder GetInterceptorIocContainer(ContainerBuilder builder)
        {
            builder.Register(c => new LogInterceptor(c.Resolve<ILogManager>()));
            builder.Register(c => new CustomExceptionInterceptor());

            return builder;
        }

        public static ContainerBuilder GetCrmApiIocContainer(ContainerBuilder builder, string applicationName)
        {
            #region | DB ACCESS && LOG |

            builder.Register<ISqlAccess>(c => new SqlAccess(Globals.ConnectionString)).Named<ISqlAccess>(SQL_ACCESS_CRM)
                .InstancePerRequest();
            builder.Register<IElasticAccess>(c => new ElasticAccess(Globals.ElasticServerUrl, Globals.ElasticLogIndexName))
                .InstancePerRequest();

            builder.Register<ILogKeyClients>(c => new LogKeyClients())
                .InstancePerRequest();

            builder.Register<ILog>(c => new SqlLogDao(c.ResolveNamed<ISqlAccess>(SQL_ACCESS_CRM), applicationName)).Named<ILog>(SQL_LOG)
                .InstancePerRequest();
            builder.Register<ILog>(c => new ElasticLogDao(c.Resolve<IElasticAccess>(), applicationName)).Named<ILog>(ELASTIC_LOG)
                .InstancePerRequest();

            builder.Register<Dictionary<LogEntity.LogClientType, ILog>>(c => new Dictionary<LogEntity.LogClientType, ILog>()
            {
                  {LogEntity.LogClientType.SQL, c.ResolveNamed<ILog>(SQL_LOG)},
                  {LogEntity.LogClientType.ELASTIC, c.ResolveNamed<ILog>(ELASTIC_LOG)},
            
            }).InstancePerRequest();


            builder.Register<ILogFactory>(c => new LogFactory(c.Resolve<Dictionary<LogEntity.LogClientType, ILog>>()))
                .InstancePerRequest();

            builder.Register<ILogManager>(c => new Logger(c.Resolve<ILogFactory>(), c.Resolve<ILogKeyClients>()))
                .InstancePerRequest();

            builder.Register<IMsCrmAccess>(c => new MsCrmAccess("", ""))
                .InstancePerRequest();

            #endregion

            #region | DATA LIBRARY |

            builder.Register<IBaseDao<Portal>>(c => new PortalDao(c.Resolve<IMsCrmAccess>()
              , c.ResolveNamed<ISqlAccess>(SQL_ACCESS_CRM)))
           .InstancePerRequest()
           .EnableInterfaceInterceptors()
           .InterceptedBy(typeof(CustomExceptionInterceptor));

            builder.Register<IBaseDao<User>>(c => new UserDao(c.Resolve<IMsCrmAccess>()
              , c.ResolveNamed<ISqlAccess>(SQL_ACCESS_CRM)))
            .InstancePerRequest()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(CustomExceptionInterceptor));

            builder.Register<IUserDao>(c => new UserDao(c.Resolve<IMsCrmAccess>()
              , c.ResolveNamed<ISqlAccess>(SQL_ACCESS_CRM)))
            .InstancePerRequest()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(CustomExceptionInterceptor));

            builder.Register<IBaseDao<SessionData>>(c => new SessionDao(c.Resolve<IMsCrmAccess>()
              , c.ResolveNamed<ISqlAccess>(SQL_ACCESS_CRM)))
            .InstancePerRequest()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(CustomExceptionInterceptor));

            //builder.RegisterType<BaseDao<SessionData>>()
            //    .EnableClassInterceptors()
            //    .InterceptedBy(typeof(CustomExceptionInterceptor));

            //builder.RegisterType<BaseDao<User>>()
            //    .EnableClassInterceptors()
            //    .InterceptedBy(typeof(CustomExceptionInterceptor));

            //builder.RegisterType<BaseDao<Portal>>()
            //    .EnableClassInterceptors()
            //    .InterceptedBy(typeof(CustomExceptionInterceptor));

            #endregion

            #region | BUSINESS LIBRARY |

            builder.Register<IBaseBusiness<Portal>>(c => new PortalBusiness(c.Resolve<IBaseDao<Portal>>()))
            .InstancePerRequest()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(CustomExceptionInterceptor));

            builder.Register<IBaseBusiness<User>>(c => new UserBusiness(c.Resolve<IBaseDao<User>>()
                , c.Resolve<BaseDao<SessionData>>()
                , c.Resolve<IUserDao>()))
            .InstancePerRequest()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(CustomExceptionInterceptor));

            builder.Register<IUserBusiness>(c => new UserBusiness(c.Resolve<IBaseDao<User>>()
                , c.Resolve<IBaseDao<SessionData>>()
                , c.Resolve<IUserDao>()))
            .InstancePerRequest()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(CustomExceptionInterceptor));

            #endregion

            #region | FACADE LIBRARY |

            #endregion


            return builder;
        }
    }
}
