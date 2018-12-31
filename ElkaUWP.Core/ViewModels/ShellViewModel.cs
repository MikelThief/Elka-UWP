using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using ElkaUWP.Core.Helpers;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ElkaUWP.Infrastructure;
using WinUI = Microsoft.UI.Xaml.Controls;

namespace ElkaUWP.Core.ViewModels
{
    public class ShellViewModel : BindableBase, INavigatedAware
    {
        public INavigationService _internalNavigationService;
        public INavigationService _externalNavigationService;

        public ShellViewModel()
        {

        }

        public void Initialize()
        {

        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }


        public void OnNavigatedTo(INavigationParameters parameters)
        {
            _externalNavigationService = parameters.GetNavigationService();
        }
    }
}
