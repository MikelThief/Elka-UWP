using Prism.Mvvm;
using Prism.Navigation;

namespace ElkaUWP.Core.ViewModels
{
    public class TestViewModel : BindableBase, INavigationAware
    {
        public INavigationService _nav;
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
            _nav = parameters.GetNavigationService();
        }
    }
}
