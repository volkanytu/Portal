using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace GK.WebServices.WEBAPI.CrmApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // xml response dönmesini engelliyor
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
