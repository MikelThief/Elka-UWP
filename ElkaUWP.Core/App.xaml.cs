using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
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
using ElkaUWP.Infrastructure.Interfaces;
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
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewTypeToViewModelTypeResolver: viewType =>
            {
                var viewName = viewType.FullName;
                viewName = viewName.Replace(oldValue: ".Views.", newValue: ".ViewModels.");
                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                var suffix = (viewName.EndsWith(value: "View") || viewName.EndsWith(value: "Page")) ? "Model" : "ViewModel";
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
                (IPlatformNavigationService) Prism.Navigation.NavigationService.Create(frame: coreFrame, Gesture.Back,
                    Gesture.Forward, Gesture.Refresh);
            // set main window as a target for navigation service and then show window (activate)
            NavigationService.SetAsWindowContent(window: Window.Current, activate: true);

            // set size for average 1920x1080 desktop. Note the size is in effective pixels
            ApplicationView.PreferredLaunchViewSize = new Size(width: 500, height: 650);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.Auto;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(minSize: new Size(width: 400, height: 500));
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
                case StartKinds.Activate:
                {
                    if (args.StartCause == StartCauses.Protocol)
                    {
                        if (args.Arguments is IProtocolActivatedEventArgs protocolArguments)
                        {
                            var usosOAuthService = Container.Resolve<IUsosOAuthService>();

                            var callbackQuery = protocolArguments.Uri.Query;

                            var responseParameters = HttpUtility.ParseQueryString(query: callbackQuery);

                            var oauthVerifier = responseParameters.Get(name: "oauth_verifier");
                            var authorizedOauthToken = responseParameters.Get(name: "oauth_token");

                            await usosOAuthService.GetAccessAsync(oauthToken: authorizedOauthToken, oauthVerifier: oauthVerifier);

                            var navigationParameters = new NavigationParameters();

                            await NavigationService.NavigateAsync(name: nameof(UsosStepView), parameters: new NavigationParameters());
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
            base.RegisterRequiredTypes(containerRegistry: containerRegistry);
        }
    }
}
