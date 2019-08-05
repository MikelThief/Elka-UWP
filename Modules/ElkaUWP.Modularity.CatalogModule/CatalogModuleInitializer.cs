using ElkaUWP.Infrastructure;
using ElkaUWP.Modularity.CatalogModule.ViewModels;
using ElkaUWP.Modularity.CatalogModule.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace ElkaUWP.Modularity.CatalogModule
{
    public class CatalogModuleInitializer : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<SearchUsersView, SearchUsersViewModel>(key: PageTokens.CatalogSearchUsersView);
        }

        /// <inheritdoc />
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }
    }
}
