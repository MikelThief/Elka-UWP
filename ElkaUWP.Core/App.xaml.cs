using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Windows.ApplicationModel;
using ElkaUWP.Core.Views;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Security.Credentials;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ElkaUWP.Core.ViewModels;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Extensions;
using ElkaUWP.Infrastructure.Interfaces;
using ElkaUWP.LoginModule;
using ElkaUWP.LoginModule.ViewModels;
using ElkaUWP.LoginModule.Views;
using NLog;
using NLog.Config;
using NLog.Targets;
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
        public static INavigationService NavigationService { get; private set; }

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
            SetUpLogger();

            // creating the initial frame
            var coreFrame = new Frame();
            // creating navigation service for this frame
            NavigationService =
                (IPlatformNavigationService) Prism.Navigation.NavigationService.Create(frame: coreFrame, Gesture.Back,
                    Gesture.Forward, Gesture.Refresh);
            // set main window as a target for navigation service and then show window (activate)
            NavigationService.SetAsWindowContent(window: Window.Current, true);


            // set size for average 1920x1080 desktop. Note the size is in effective pixels

            var DPI = Windows.Graphics.Display.DisplayInformation.GetForCurrentView().LogicalDpi;
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            var desiredSize = new Size(width: (420 * 96.0f / DPI), height: (550 * 96.0f / DPI));
            ApplicationView.PreferredLaunchViewSize = desiredSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(minSize: new Size(width: (400 * 96.0f / DPI), height: (500 * 96.0f / DPI)));
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
                Layout = @"${date:format=HH\:mm\:ss} ${level} ${message} ${exception}",
                OptimizeBufferReuse = true,
                Encoding = Encoding.UTF8,
                WriteBom = false,
                LineEnding = LineEndingMode.Default,
                FileName = Path.Combine(path1: Windows.Storage.ApplicationData.Current.LocalFolder.Path, path2: Constants.APPLICATION_LOG_FILENAME),
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
            config.AddRule(minLevel: LogLevel.Info, maxLevel: LogLevel.Fatal, target: bufferedTargetWrapper);

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

                    var vault = new PasswordVault();

                    try
                    {
                        var credential =
                            vault.GetUniversitySystemCredential(systemResourceName: Constants.USOS_RESOURCE_TOKEN);
                        // TODO: Naviagte to Main App Page
                    }
                    catch (Exception)
                    {
                        await NavigationService.NavigateAsync(name: nameof(LoginView));
                    }
                    break;
                case StartKinds.Activate:
                {
                    if (args.StartCause == StartCauses.Protocol)
                    {
                        if (args.Arguments is IProtocolActivatedEventArgs protocolArguments)
                        {
                            var usosOAuthService = Container.Resolve<IUsosOAuthService>();

                            var responseParameters = HttpUtility.ParseQueryString(query: protocolArguments.Uri.Query);

                            await usosOAuthService.GetAccessAsync(authorizedRequestToken: responseParameters.Get(name: "oauth_token"), oauthVerifier: responseParameters.Get(name: "oauth_verifier"));

                            var navigationParameters = new NavigationParameters
                            {
                                { NavigationParameterKeys.IS_USOS_AUTHORIZED, true }
                            };

                                await NavigationService.NavigateAsync(name: nameof(UsosStepView), parameters: navigationParameters);
                        }
                    }

                    break;
                }
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
        /// <param name="containerRegistry">Container against which registrations should be performed</param>
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
    }
}
