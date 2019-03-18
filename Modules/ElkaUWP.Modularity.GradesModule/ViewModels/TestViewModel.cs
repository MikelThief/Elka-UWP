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
        public async void OnNavigatingTo(INavigationParameters parameters)
        {
            _navigationService = parameters.GetNavigationService();

            //await _testsService.GetUserTestsPerSemester();

            //await _testsService.GetSubjectTestTree(nodeId: 61078);

            await _testsService.GetUserPoints(new List<int>()
            {
                61091, 61092, 61093
            });

            PageHeader = (string) parameters["SubjectAcronym"];
        }
    }
}
