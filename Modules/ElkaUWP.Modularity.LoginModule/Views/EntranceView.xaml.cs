using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using ElkaUWP.Modularity.LoginModule.ViewModels;
using Prism.Mvvm;

namespace ElkaUWP.Modularity.LoginModule.Views
{
    public partial class EntranceView : Page
    {
        private LoginViewModel ViewModel => DataContext as LoginViewModel;

        public EntranceView()
        {
            InitializeComponent();
            ViewModelLocator.SetAutowireViewModel(obj: this, value: true);

            ApplicationViewTitleBar formattableTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            formattableTitleBar.ButtonBackgroundColor = Colors.Transparent;
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
        }
    }
}
