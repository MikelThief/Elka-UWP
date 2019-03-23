using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Propertiary.Services;
using ElkaUWP.DataLayer.Usos.Services;
using Prism.Mvvm;
using Prism.Navigation;

namespace ElkaUWP.Modularity.GradesModule.ViewModels
{
    public class TestViewModel : BindableBase, INavigationAware
    {
        private PartialGradesService PartialGradesService;
        private INavigationService _navigationService;

        private string _pageHeader;

        public string PageHeader
        {
            get => _pageHeader;
            set => SetProperty(storage: ref _pageHeader, value: value, nameof(PageHeader));
        }

        public TestViewModel(PartialGradesService crstestsService)
        {
            PartialGradesService = crstestsService;
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

            //await CrstestsService.GetUserTestsPerSemester();

            //await CrstestsService.GetSubjectTestTree(nodeId: 61078);

            var res = await PartialGradesService.GetAsync("2018Z", "103A-CSCSN-ISA-ELAC");

            //await CrstestsService.UserPointsAsync(new List<int>()
            //{
            //    61091, 61092, 61093
            //});

            PageHeader = (string) parameters["SubjectAcronym"];
        }
    }
}
