using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using ElkaUWP.Modularity.LoginModule.ViewModels;
using Prism.Mvvm;

namespace ElkaUWP.Modularity.LoginModule.Views
{
    public partial class WelcomeView : Page
    {
        private WelcomeViewModel ViewModel => DataContext as WelcomeViewModel;

        public WelcomeView()
        {
            InitializeComponent();
            ViewModelLocator.SetAutowireViewModel(obj: this, value: true);
        }
    }
}
