using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Usos.Services;
using Prism.Mvvm;
using Prism.Navigation;

namespace ElkaUWP.Modularity.GradesModule.ViewModels
{
    public class TestViewModel : BindableBase, INavigationAware
    {
        private TestsService _testsService;
        private INavigationService _navigationService;

        private string _pageHeader;

        public string PageHeader
        {
            get => _pageHeader;
            set => SetProperty(storage: ref _pageHeader, value: value, nameof(PageHeader));
        }

        public TestViewModel(TestsService testsService)
        {
            _testsService = testsService;
        }
        /// <inheritdoc />
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        /// <inheritdoc />
        public void OnNavigatedTo(INavigationParameters parameters)
        {
            
        }

        /// <inheritdoc />
        public void OnNavigatingTo(INavigationParameters parameters)
        {
            _navigationService = parameters.GetNavigationService();
            PageHeader = (string) parameters["SubjectAcronym"];
        }
    }
}
