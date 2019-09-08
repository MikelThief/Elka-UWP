using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.Core.Views;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Anotar.NLog;
using ElkaUWP.Core.ViewModels;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Extensions;
using ElkaUWP.Infrastructure.Services;
using NLog;
using NLog.Config;
using NLog.Targets;
using Prism;
using Prism.Unity;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using ElkaUWP.Modularity.LoginModule;
using ElkaUWP.Modularity.CalendarModule;
using ElkaUWP.Modularity.MapsModule;
using ElkaUWP.DataLayer;
using ElkaUWP.DataLayer.Usos.Services;
using ElkaUWP.Infrastructure.Misc;
using ElkaUWP.Modularity.CatalogModule;
using ElkaUWP.Modularity.GradesModule;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using LogLevel = NLog.LogLevel;
using UnhandledExceptionEventArgs = Windows.UI.Xaml.UnhandledExceptionEventArgs;

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
            UnhandledException += delegate(object sender, UnhandledExceptionEventArgs args)
            {
                LogTo.FatalException(exception: args.Exception, message: "Unhandled exception occured.");
                args.Handled = false; // crashes the app after delegate finishes
                LogManager.Flush();
            };
            StartAnalytics();
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
                var viewModelName = string.Format(provider: CultureInfo.InvariantCulture, "{0}{1}, {2}", arg0: viewName, arg1: suffix, arg2: viewAssemblyName);
                return viewModelName.GetType();
            });
        }

        /// <summary>
        /// Addition of modules implementing <see cref="IModule"/> goes here.
        /// </summary>
        /// <param name="moduleCatalog"></param>
        public override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            // TODO: Loading optimization: set loading to ondemand and load all if user is already authenticated or just login module if it is fresh start.
            // Username module
            var loginModuleType = typeof(LoginModuleInitializer);
            moduleCatalog.AddModule(moduleInfo: new ModuleInfo()
            {
                ModuleName = loginModuleType.Name,
                ModuleType = loginModuleType,
                InitializationMode = InitializationMode.WhenAvailable
            });

            // DataLayer module
            var dataLayerModuleType = typeof(DataLayerInitializer);
            moduleCatalog.AddModule(moduleInfo: new ModuleInfo()
            {
                ModuleName = dataLayerModuleType.Name,
                ModuleType = dataLayerModuleType,
                InitializationMode = InitializationMode.WhenAvailable
            });

            // Calendar module
            var calendarModuleType = typeof(CalendarModuleInitializer);
            moduleCatalog.AddModule(moduleInfo: new ModuleInfo()
            {
                ModuleName = calendarModuleType.Name,
                ModuleType = calendarModuleType,
                InitializationMode = InitializationMode.WhenAvailable
            });

            // Catalog module
            var catalogModuleType = typeof(CatalogModuleInitializer);
            moduleCatalog.AddModule(moduleInfo: new ModuleInfo()
            {
                ModuleName = catalogModuleType.Name,
                ModuleType = catalogModuleType,
                InitializationMode = InitializationMode.WhenAvailable
            });

            // Grades module
            var gradesModuleType = typeof(GradesModuleInitializer);
            moduleCatalog.AddModule(moduleInfo: new ModuleInfo()
            {
                ModuleName = gradesModuleType.Name,
                ModuleType = gradesModuleType,
                InitializationMode = InitializationMode.WhenAvailable
            });
            // Maps Module
            var mapsModuleType = typeof(MapsModuleInitializer);
            moduleCatalog.AddModule(moduleInfo: new ModuleInfo()
            {
                ModuleName = mapsModuleType.Name,
                ModuleType = mapsModuleType,
                InitializationMode = InitializationMode.WhenAvailable
            });



        }

        /// <summary>
        /// Ran when application is prepared. This class member-initialization logic should go here
        /// </summary>
        public override void OnInitialized()
        {
            SetUpLogger();

            // creating the initial frame
            var coreFrame = new ThemeAwareFrame();

            // creating navigation service for this frame
            NavigationService = (Prism.Navigation.NavigationService.Create(frame: coreFrame, Gesture.Back,
                    Gesture.Forward, Gesture.Refresh) as IPlatformNavigationService);

            //// set main window as a target for navigation service and then show window (activate)
            NavigationService.SetAsWindowContent(window: Window.Current, true);


            // Set up minimum window size
            var DPI = Windows.Graphics.Display.DisplayInformation.GetForCurrentView().LogicalDpi;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(minSize: new Size(width: (400 * 96.0f / DPI), height: (640 * 96.0f / DPI)));
        }

        /// <summary>
        /// Sets up NLog instance for application
        /// </summary>
        private void SetUpLogger()
        {
            // Step 1: Create configuration object
            var config = new LoggingConfiguration();

            // Step 2: Create targets

            var fileTarget = new FileTarget()
            {
                Name = "FileLog",
                Layout = @"${date:format=HH\:mm\:ss} ${level} ${message} ${exception:format=@:innerFormat=@:maxInnerExceptionLevel=5}",
                OptimizeBufferReuse = true,
                Encoding = Encoding.UTF8,
                WriteBom = false,
                LineEnding = LineEndingMode.Default,
                FileName = Path.Combine(path1: ApplicationData.Current.LocalFolder.Path, path2: Constants.APPLICATION_LOG_FILENAME),
                OpenFileCacheTimeout = 2,
                ArchiveNumbering = ArchiveNumberingMode.Rolling,
                ArchiveAboveSize = 10240,
                MaxArchiveFiles = 10
            };

            var bufferedTargetWrapper = new NLog.Targets.Wrappers.BufferingTargetWrapper()
            {
                Name = "BufferredFileLog",
                WrappedTarget = fileTarget,
            };

            // Step 3: Add targets

            // Step 4. Define rules
            config.AddRule(minLevel: LogLevel.Warn, maxLevel: LogLevel.Fatal, target: bufferedTargetWrapper);

#if DEBUG
            config.AddRule(minLevel: LogLevel.Trace, maxLevel: LogLevel.Fatal, target: bufferedTargetWrapper);
#endif

            // Step 5. Activate the configuration
            LogManager.Configuration = config;
        }

        /// <summary>
        /// All interface-implementation bonding should lie here as well as types' registration for use in application in DI
        /// </summary>
        /// <param name="container">Container registry used to register types</param>
        public override void RegisterTypes(IContainerRegistry container)
        {
            // register types for navigation. SetAutoWireViewModel is still needed in View's code-behind.
            container.RegisterForNavigation<ShellView, ShellViewModel>(key: PageTokens.ShellViewToken);
            container.RegisterForNavigation<SettingsView, SettingsViewModel>(key: PageTokens.SettingsViewToken);

            // Register services
            container.RegisterSingleton<SecretService>();
        }

        /// <summary>
        /// Determines an action depending of the value of arguments passed while launching
        /// Executed before <see cref="OnStartAsync"/>.
        /// </summary>
        /// <param name="args"></param>
        public override void OnStart(StartArgs args)
        {
            RegisterLicences();
        }

        public override async Task OnStartAsync(StartArgs args)
        {
            await StartAppServicesAsync();

            var secretService = Container.Resolve<SecretService>();
            switch (args.StartKind)
            {
                case StartKinds.Launch:
                    if (secretService.IsContainerPresent(container: Constants.USOS_CREDENTIAL_CONTAINER_NAME))
                        await NavigationService.NavigateAsync(name: PageTokens.ShellViewToken);
                    else
                        await NavigationService.NavigateAsync(name: PageTokens.WelcomeViewToken);
                    break;
                case StartKinds.Activate:
                {
                    if (args.StartCause == StartCauses.Protocol)
                    {
                        if (args.Arguments is IProtocolActivatedEventArgs protocolArguments)
                        {
                            var usosOAuthService = Container.Resolve<LogonService>();
                            var usosHandshakeSuccess =
                                await usosOAuthService.FinishOAuthHandshakeAsync(responseQueryString: protocolArguments.Uri.Query)
                                    .ConfigureAwait(continueOnCapturedContext: true);

                            var navigationParameters = new NavigationParameters
                            {
                                {NavigationParameterKeys.IS_USOS_AUTHORIZED, usosHandshakeSuccess.IsSuccess }
                            };

                            await NavigationService.NavigateAsync(name: PageTokens.UsosLoginViewToken,
                                parameters: navigationParameters);
                        }
                    }
                }
                    break;

                case StartKinds.Prelaunch:
                    break;
                case StartKinds.Background:
                    break;
                case StartKinds.Resume:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        /// <summary>
        /// Register types without application cannot perform startup here.
        /// </summary>
        /// <param name="containerRegistry">ParametersContainer against which registrations should be performed</param>
        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            var nLogExtension = new NLogExtension
            {
                GetName = (t, n) => t.Name
            };
            containerRegistry.GetContainer().AddExtension(extension: nLogExtension);

            base.RegisterRequiredTypes(containerRegistry: containerRegistry);
        }
        /// <summary>
        /// Executes just before app is terminated
        /// </summary>
        public override void OnSuspending()
        {
            LogManager.Flush();
            base.OnSuspending();
        }

        public static void RegisterLicences()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(licenseKey: Secrets.SYNCFUSION_CONTROLS_SECRET);
        }

        public async Task StartAppServicesAsync()
        {
            await ThemeService.InitializeAsync().ConfigureAwait(continueOnCapturedContext: false);
            await ThemeService.SetRequestedThemeAsync().ConfigureAwait(continueOnCapturedContext: false);
        }

        public void StartAnalytics()
        {
            AppCenter.Start(appSecret: Secrets.APPCENTER_ANALYTICS_SECRET, services: typeof(Analytics));
        }
    }
}
