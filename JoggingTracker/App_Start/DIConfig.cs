using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StructureMap;
using JoggingTracker.App_Start;
using JoggingTracker.Db;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(DIConfig), "Register")]
namespace JoggingTracker.App_Start {
    public class DIConfig {

        public static void Register() {
            var container = new Container(_ => {
                _.For<IUnitOfWork>().Use<UnitOfWork>();
                _.For<ISimpleUserManager>().Use<SimpleUserManager>();
            });

            //Register for MVC
            DependencyResolver.SetResolver(new StructureMapDependencyResolve(container));

            //Register for Web API
            GlobalConfiguration.Configuration.DependencyResolver = new StructureMapDependencyResolver(container);
        }
    }
}