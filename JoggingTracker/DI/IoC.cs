using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;
using StructureMap;
using JoggingTracker.Db;
using JoggingTracker.Db.Repository;

namespace JoggingTracker.DI {
    public class IoC {
        private static readonly Lazy<IContainer> _container = new Lazy<IContainer>(Init);

        private static IContainer Init() {
            var container = new Container(cfg => {
                cfg.Scan(scan => {
                    scan.TheCallingAssembly();
                    scan.LookForRegistries();
                    scan.WithDefaultConventions();
                });
                
                cfg.For<IUnitOfWork>().Use<UnitOfWork>();
                cfg.For<ISimpleUserManager>().Use<SimpleUserManager>();
                cfg.For<IUserRepository>().Use<UserRepository>();
                cfg.For<IAuthenticationManager>().Use(() => HttpContext.Current.GetOwinContext().Authentication);

                //cfg.For<IAuthenticator>().Use<PhoeixApiAuthenticator>();
                //cfg.For<IMediator>().Use<Mediator>();
                //cfg.For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
                //cfg.For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            });

            return container;
        }

        public static IContainer Container {
            get {
                return _container.Value;
            }
        }
    }
}