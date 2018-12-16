using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ElkaUWP.Core.Views;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ElkaUWP.Core.ViewModels;
using ElkaUWP.Infrastructure;
using ElkaUWP.LoginModule;
using ElkaUWP.LoginModule.ViewModels;
using ElkaUWP.LoginModule.Views;
using Prism;
using Prism.Events;
using Prism.Unity;
using Prism.Ioc;
using Prism.Logging;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;

namespace ElkaUWP.Core
{
    [Windows.UI.Xaml.Data.Bindable]
    public sealed partial class App : PrismApplication
    {
        /// <summary>
        /// <see cref="NavigationService"/> for navigating between main app views and login views
        /// </summary>
        public static IPlatformNavigationService NavigationService { get; private set; }

        /// <summary>
        /// Entry point to the application
        /// </summary
        public App()
        {
            InitializeComponent();
        }

        public override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewType =>
            {
                var viewName = viewType.FullName;
                viewName = viewName.Replace(".Views.", ".ViewModels.");
                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                var suffix = (viewName.EndsWith("View") || viewName.EndsWith("Page")) ? "Model" : "ViewModel";
                var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}{1}, {2}", viewName, suffix, viewAssemblyName);
                return viewModelName.GetType();
            });
        }

        /// <summary>
        /// Addition of modules implementing <see cref="IModule"/> goes here.
        /// </summary>
        /// <param name="moduleCatalog"></param>
        public override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            // Login module
            var loginModuleType = typeof(LoginModuleInitializer);
            moduleCatalog.AddModule(moduleInfo: new ModuleInfo()
            {
                ModuleName = loginModuleType.Name,
                ModuleType = loginModuleType,
                InitializationMode = InitializationMode.WhenAvailable
            });
        }

        /// <summary>
        /// Ran when application is prepared. This class member-initialization logic should go here
        /// </summary>
        public override void OnInitialized()
        {
            // creating the initial frame
            var coreFrame = new Frame();
            // creating navigation service for this frame
            NavigationService =
                (IPlatformNavigationService) Prism.Navigation.NavigationService.Create(coreFrame, Gesture.Back,
                    Gesture.Forward, Gesture.Refresh);
            // set main window as a target for navigation service and then show window (activate)
            NavigationService.SetAsWindowContent(Window.Current, true);

            // set size for average 1920x1080 desktop. Note the size is in effective pixels
            ApplicationView.PreferredLaunchViewSize = new Size(500, 650);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.Auto;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(minSize: new Size(400, 500));
        }

        /// <summary>
        /// All interface-implementation bonding should lie here as well as types' registration for use in application in DI
        /// </summary>
        /// <param name="container">Container registry used to register types</param>
        public override void RegisterTypes(IContainerRegistry container)
        {

        }

        /// <summary>
        /// Determines an action depending of the value of arguments passed while launching
        /// Executed before <see cref="OnStartAsync"/>.
        /// </summary>
        /// <param name="args"></param>
        public override void OnStart(StartArgs args)
        {
            ApplicationViewTitleBar formattableTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            formattableTitleBar.ButtonBackgroundColor = Colors.Transparent;
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
        }

        public override async Task OnStartAsync(StartArgs args)
        {
            switch (args.StartKind)
            {
                case StartKinds.Launch:
                    await NavigationService.NavigateAsync(name: nameof(LoginView));
                    break;
                default:
                {
                    if (args.StartCause == StartCauses.Protocol)
                    {
                        if (args.Arguments is IProtocolActivatedEventArgs protocolArguments)
                        {
                            var navigationParameters = new NavigationParameters();

                            navigationParameters.Add(Constants.USOS_API_AUTH_TOKEN, "");

                            await NavigationService.NavigateAsync(name: nameof(LoginView), parameters: new NavigationParameters());
                        }
                    }
                    else
                    {
                        // TODO
                    }

                    break;
                }
            }
        }
        /// <summary>
        /// Register types without application cannot perform startup here.
        /// </summary>
        /// <param name="containerRegistry">Container against which registrations should be performed</param>
        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);
        }
    }
}
