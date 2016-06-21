﻿using Autofac;
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
using GK.Library.Facade.Interfaces;
using GK.Library.Facade;
using GK.Library.ConfigManager.Interfaces;
using GK.Library.ConfigManager;

namespace GK.Library.IocManager
{
    public class IocContainerBuilder
    {
        private const string CONFIGDB_SQL_ACCESS = "CONFIGDB_SQL_ACCESS";
        private const string CONFIGDB_SQL_ENTITY_ACCESS = "CONFIGDB_SQL_ENTITY_ACCESS";
        private const string SQL_ACCESS_CRM = "SQL_ACCESS_CRM";
        private const string ELASTIC_LOG = "ELASTIC_LOG";
        private const string SQL_LOG = "SQL_LOG";

        public static ContainerBuilder GetIConfigs(ContainerBuilder builder)
        {
            builder.Register<ISqlAccess>(c => new SqlAccess("Data Source=84.51.31.56; Initial Catalog=DO_MSCRM; User Id=CrmSqlUser; Password=CrmSqlPass;Max Pool Size = 10000;Pooling = True"))
                .Named<ISqlAccess>(CONFIGDB_SQL_ACCESS)
                .InstancePerDependency();

            builder.Register<ISqlEntityAccess>(c => new SqlEntityAccess(c.ResolveNamed<ISqlAccess>(CONFIGDB_SQL_ACCESS)))
                .Named<ISqlEntityAccess>(CONFIGDB_SQL_ENTITY_ACCESS)
               .InstancePerDependency();

            builder.Register<IDBConfigDao>(c => new DBConfigDao(c.ResolveNamed<ISqlEntityAccess>(CONFIGDB_SQL_ENTITY_ACCESS))).InstancePerDependency();

            builder.Register<IConfigManager>(c => new DBConfigManager(c.Resolve<IDBConfigDao>())).InstancePerDependency();

            builder.Register<IConfigs>(c => new Configs(c.Resolve<IConfigManager>())).InstancePerDependency();

            //ISqlAccess sqlAccess = new SqlAccess("Data Source=84.51.31.56; Initial Catalog=DO_MSCRM; User Id=CrmSqlUser; Password=CrmSqlPass;Max Pool Size = 10000;Pooling = True");
            //ISqlEntityAccess sqlEntityAccess = new SqlEntityAccess(sqlAccess);
            //IDBConfigDao configDao = new DBConfigDao(sqlEntityAccess);
            //IConfigManager configManager = new DBConfigManager(configDao);
            //IConfigs configs = new Configs(configManager);
            return builder;
        }

        public static ContainerBuilder GetInterceptorIocContainer(ContainerBuilder builder)
        {
            builder.Register(c => new LogInterceptor(c.Resolve<ILogManager>()));
            builder.Register(c => new CustomExceptionInterceptor());

            return builder;
        }

        public static ContainerBuilder GetCrmApiIocContainer(ContainerBuilder builder, string applicationName)
        {
            #region | DB ACCESS && LOG |

            builder.Register<ISqlAccess>(c => new SqlAccess(c.Resolve<IConfigs>().CRM_DB_CONNECTION_STRING)).Named<ISqlAccess>(SQL_ACCESS_CRM)
                .InstancePerRequest();
            builder.Register<IElasticAccess>(c => new ElasticAccess(c.Resolve<IConfigs>().ELASTIC_SERVER_URL, c.Resolve<IConfigs>().ELASTIC_CRM_INDEX))
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

            builder.Register<IMsCrmAccess>(c => new MsCrmAccess(c.Resolve<IConfigs>().CRM_SVC_CONNECTION_STRING, c.Resolve<IConfigs>().CRM_SVC_IS_CONNECTION_STRING))
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

            builder.Register<IBaseDao<Annotation>>(c => new AnnotationDao(c.Resolve<IMsCrmAccess>()
              , c.ResolveNamed<ISqlAccess>(SQL_ACCESS_CRM)))
            .InstancePerRequest()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(CustomExceptionInterceptor));

            builder.Register<IAnnotationDao>(c => new AnnotationDao(c.Resolve<IMsCrmAccess>()
              , c.ResolveNamed<ISqlAccess>(SQL_ACCESS_CRM)))
            .InstancePerRequest()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(CustomExceptionInterceptor));

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

            builder.Register<IBaseBusiness<SessionData>>(c => new SessionBusiness(c.Resolve<IBaseDao<SessionData>>()))
            .InstancePerRequest()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(CustomExceptionInterceptor));

            builder.Register<IUserBusiness>(c => new UserBusiness(c.Resolve<IBaseDao<User>>()
                , c.Resolve<IBaseDao<SessionData>>()
                , c.Resolve<IUserDao>()))
            .InstancePerRequest()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(CustomExceptionInterceptor));

            builder.Register<IBaseBusiness<Annotation>>(c => new AnnotationBusiness(c.Resolve<IBaseDao<Annotation>>()
                , c.Resolve<IAnnotationDao>()))
            .InstancePerRequest()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(CustomExceptionInterceptor));

            builder.Register<IAnnotationBusiness>(c => new AnnotationBusiness(c.Resolve<IBaseDao<Annotation>>()
                , c.Resolve<IAnnotationDao>()))
            .InstancePerRequest()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(CustomExceptionInterceptor));
            #endregion

            #region | FACADE LIBRARY |
            builder.Register<IPortalFacade>(c => new PortalFacade(c.Resolve<IBaseBusiness<Portal>>()
               , c.Resolve<IAnnotationBusiness>()))
           .InstancePerRequest()
           .EnableInterfaceInterceptors()
           .InterceptedBy(typeof(CustomExceptionInterceptor));

            builder.Register<IUserFacade>(c => new UserFacade(c.Resolve<IUserBusiness>(), c.Resolve<IBaseBusiness<SessionData>>()))
            .InstancePerRequest()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(CustomExceptionInterceptor));

            #endregion


            return builder;
        }
    }
}
