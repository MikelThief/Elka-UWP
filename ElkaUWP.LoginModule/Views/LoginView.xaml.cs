using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using ElkaUWP.LoginModule.ViewModels;
using Prism.Mvvm;

namespace ElkaUWP.LoginModule.Views
{
    public partial class LoginView : Page
    {
        private LoginViewModel ViewModel => DataContext as LoginViewModel;

        public LoginView()
        {
            InitializeComponent();
            ViewModelLocator.SetAutowireViewModel(obj: this, value: true);
            ((Storyboard)Resources["GradientAnimation"]).Begin();
        }
    }
}
