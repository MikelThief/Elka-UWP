using Unity;

namespace ElkaUWP.Infrastructure.Resolvers
{
    public class SimpleStrategyResolver
    {
        private readonly IUnityContainer _container;

        public SimpleStrategyResolver(IUnityContainer unityContainer) => _container = unityContainer;

        public T Resolve<T>(string namedStrategy) => _container.Resolve<T>(name: namedStrategy);
    }
}
