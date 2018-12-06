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
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ElkaUWP.Core.ViewModels;
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
        /// <see cref="NavigationService"/> for navigating between main app views and login view
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
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewTypeToViewModelTypeResolver: viewType =>
            {
                var viewName = viewType.FullName;
                viewName = viewName.Replace(oldValue: ".Views.", newValue: ".ViewModels.");
                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                var suffix = viewName.EndsWith(value: "View") ? "Model" : "ViewModel";
                var viewModelName = string.Format(provider: CultureInfo.InvariantCulture, format: "{0}{1}, {2}", arg0: viewName, arg1: suffix, arg2: viewAssemblyName);
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
            moduleCatalog.AddModule(new ModuleInfo()
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
                (IPlatformNavigationService) Prism.Navigation.NavigationService.Create(frame: coreFrame, Gesture.Back,
                    Gesture.Forward, Gesture.Refresh);
            // set main window as a target for navigation service and then show window (activate)
            NavigationService.SetAsWindowContent(window: Window.Current, activate: true);
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
        /// </summary>
        /// <param name="args"></param>
        public override void OnStart(StartArgs args)
        {
            if (args.StartKind == StartKinds.Launch)
            {
                NavigationService.NavigateAsync(name: nameof(LoginView));
            }
            else
            {
                // TODO
            }

            ApplicationViewTitleBar formattableTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            formattableTitleBar.ButtonBackgroundColor = Colors.Transparent;
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
        }

        /// <summary>
        /// Register types without application cannot perform startup here.
        /// </summary>
        /// <param name="containerRegistry">Container against which registrations should be performed</param>
        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            // don't forget there is no logger yet
            Debug.WriteLine($"{nameof(PrismApplicationBase)}.{nameof(RegisterRequiredTypes)}()");

            // required for view-models

            containerRegistry.Register<INavigationService, NavigationService>(name: NavigationServiceParameterName);

            // register container itself
            containerRegistry.RegisterInstance<IContainerExtension>(instance: (IContainerExtension) Container);

            // standard prism services
            containerRegistry.RegisterSingleton<ILoggerFacade, DebugLogger>();
            containerRegistry.RegisterSingleton<IEventAggregator, EventAggregator>();
            containerRegistry.RegisterSingleton<IModuleInitializer, ModuleInitializer>();
            containerRegistry.RegisterSingleton<IModuleCatalog, ModuleCatalog>();
            containerRegistry.RegisterSingleton<IModuleManager, ModuleManager>();


        }

        /*
        [Windows.UI.Xaml.Data.Bindable]
        
        public sealed partial class App : PrismUnityApplication
        {
            public App()
            {
                InitializeComponent();
            }
    
            protected override void ConfigureContainer()
            {
                // register a singleton using Container.RegisterType<IInterface, Type>(new ContainerControlledLifetimeManager());
                base.ConfigureContainer();
                Container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));
            }
    
            protected override async Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
            {
                await LaunchApplicationAsync(PageTokens.LoginView, null);
            }
    
            private async Task LaunchApplicationAsync(string page, object launchParam)
            {
                NavigationService.Navigate(page, launchParam);
                Window.Current.Activate();
                await Task.CompletedTask;
            }
    
            protected override async Task OnActivateApplicationAsync(IActivatedEventArgs args)
            {
                await Task.CompletedTask;
            }
    
            protected override async Task OnInitializeAsync(IActivatedEventArgs args)
            {
                // We are remapping the default ViewNamePage and ViewNamePageViewModel naming to ViewNamePage and ViewNameViewModel to
                // gain better code reuse with other frameworks and pages within Windows Template Studio
                ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
                {
                    var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "ElkaUWP.Core.ViewModels.{0}ViewModel, ElkaUWP.Core", viewType.Name.Substring(0, viewType.Name.Length - 4));
                    return Type.GetType(viewModelTypeName);
                });
                await base.OnInitializeAsync(args);
            }
    
            public void SetNavigationFrame(Frame frame)
            {
                var sessionStateService = Container.Resolve<ISessionStateService>();
                CreateNavigationService(new FrameFacadeAdapter(frame), sessionStateService);
            }
    
            protected override UIElement CreateShell(Frame rootFrame)
            {
                var shell = Container.Resolve<ShellPage>();
                shell.SetRootFrame(rootFrame);
                return shell;
            }
    
            protected override IDeviceGestureService OnCreateDeviceGestureService()
            {
                var service = base.OnCreateDeviceGestureService();
                service.UseTitleBarBackButton = false;
                return service;
            }
        }
        */
    }
}
