using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using ElkaUWP.LoginModule.ViewModels;

namespace ElkaUWP.LoginModule.Views
{
    public sealed partial class LoginView : Page
    {
        private LoginViewModel ViewModel => DataContext as LoginViewModel;

        public LoginView()
        {
            InitializeComponent();
            ((Storyboard)Resources["GradientAnimation"]).Begin();
        }
    }
}
