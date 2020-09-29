using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

namespace JoggingTracker.DI {
    public class StructureMapDependencyResolver : IDependencyResolver {
        private readonly IContainer _container;

        public StructureMapDependencyResolver(IContainer container) {
            _container = container;
        }

        public IDependencyScope BeginScope() {
            IContainer childContainer = _container.GetNestedContainer();
            //var childContainerConfigurator = childContainer.GetInstance<IChildContainerConfigurator>();
            return new StructureMapDependencyResolver(childContainer);
        }

        public void Dispose() {
            _container.Dispose();
        }

        public object GetService(Type serviceType) {
            return serviceType.IsAbstract || serviceType.IsInterface
                ? _container.TryGetInstance(serviceType)
                : _container.GetInstance(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType) {
            return _container.GetAllInstances(serviceType).Cast<object>();
        }
    }
}