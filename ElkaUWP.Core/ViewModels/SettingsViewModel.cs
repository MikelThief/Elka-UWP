using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using ElkaUWP.Infrastructure.Services;
using Microsoft.Toolkit.Uwp.Extensions;
using Prism.Commands;
using Prism.Mvvm;
using Telerik.Core;

namespace ElkaUWP.Core.ViewModels
{
    public class SettingsViewModel : BindableBase
    {
        private ElementTheme _elementTheme = ThemeService.Theme;

        public ElementTheme ElementTheme
        {
            get => _elementTheme;
            set => SetProperty(storage: ref _elementTheme, value: value, propertyName: nameof(ElementTheme));
        }

        private string _versionDescription;

        public string VersionDescription
        {
            get => _versionDescription;
            set => SetProperty(storage: ref _versionDescription, value: value, propertyName: nameof(VersionDescription));
        }

        private ICommand _switchThemeCommand;

        public ICommand SwitchThemeCommand
        {
            get
            {
                // nullable because DelegateCommand doesn't support non-nullable enum. MVVMLight RelayCommand is fine for further use.
                return _switchThemeCommand ?? (_switchThemeCommand = new DelegateCommand<ElementTheme?>(
                           async (param) =>
                           {
                               ElementTheme = param.Value;
                               await ThemeService.SetThemeAsync(theme: param.Value);
                           }));
            }
        }

        public SettingsViewModel()
        {
        }

        public async Task InitializeAsync()
        {
            VersionDescription = GetVersionDescription();
            await Task.CompletedTask;
        }

        private string GetVersionDescription()
        {
            const string appName = "AppDisplayName";
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}
