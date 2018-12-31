using Windows.UI.Xaml.Controls;
using ElkaUWP.Modularity.LoginModule.ViewModels;
using Prism.Mvvm;

namespace ElkaUWP.Modularity.LoginModule.Views
{
    public partial class LoginView : Page
    {
        private LoginViewModel ViewModel => DataContext as LoginViewModel;

        public LoginView()
        {
            InitializeComponent();
            ViewModelLocator.SetAutowireViewModel(obj: this, value: true);
        }
    }
}
