using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace ElkaUWP.DataLayer.Studia.Resolvers
{
    public class SimpleStrategyResolver
    {
        private readonly IUnityContainer _container;

        public SimpleStrategyResolver(IUnityContainer unityContainer) => _container = unityContainer;

        public T Resolve<T>(string namedStrategy) => _container.Resolve<T>(name: namedStrategy);
    }
}
