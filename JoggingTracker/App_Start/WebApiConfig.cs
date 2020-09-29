using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using JoggingTracker.DI;

namespace JoggingTracker
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.DependencyResolver = new StructureMapDependencyResolver(IoC.Container);


            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
