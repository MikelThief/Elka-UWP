using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography.Core;
using ColorCode.Common;
using ElkaUWP.DataLayer.Propertiary.Converters;
using ElkaUWP.DataLayer.Propertiary.Converters.EntityToEntity;
using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.DataLayer.Propertiary.Services;
using ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Services;
using ElkaUWP.Infrastructure;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Nito.Mvvm;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace ElkaUWP.Modularity.GradesModule.ViewModels
{
    public class FinalGradesViewModel : BindableBase, INavigationAware
    {
        private FinalGradesService _finalGradesService;
        private INavigationService _navigationService;

        public ObservableCollection<SubjectApproach> InProgressSubjectApproaches = new ObservableCollection<SubjectApproach>();
        public ObservableCollection<SubjectApproach> FinishedSubjectApproaches = new ObservableCollection<SubjectApproach>();

        public DelegateCommand<SubjectApproach> ShowTestsCommand { get; private set; }

        public FinalGradesViewModel(FinalGradesService finalGradesService)
        {
            _finalGradesService = finalGradesService;

            ShowTestsCommand = new DelegateCommand<SubjectApproach>(executeMethod: ShowTests);
        }
        /// <inheritdoc />
        public async void OnNavigatedFrom(INavigationParameters parameters)
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

            await LoadDataAsync();

            FinishedSubjectApproaches.SortStable(comparison: (x, y) => string.Compare(strA: x.Acronym, strB: y.Acronym,
                comparisonType: StringComparison.CurrentCultureIgnoreCase));
        }

        private async Task LoadDataAsync()
        {
            var subjectApproaches = await _finalGradesService.GetAllAsync();

            // remove artificial, useless subjects
            var filteredSubjectApproaches = 
                subjectApproaches.Where(subjectApproach =>
                    !subjectApproach.Acronym.All(c => c >= '0' && c <= '9') 
                    && subjectApproach.Acronym.Length >= 3).ToList();

            var highestSemesterLiteral = FindHighestSemesterLiteral(approachesList: filteredSubjectApproaches);

            foreach (var subjectApproach in filteredSubjectApproaches)
            {
                if (subjectApproach.SemesterLiteral == highestSemesterLiteral)
                    InProgressSubjectApproaches.Add(item: subjectApproach);
                else
                    FinishedSubjectApproaches.Add(item: subjectApproach);
            }
        }

        private async void ShowTests(SubjectApproach subjectApproach)
        {
            var parameters = new NavigationParameters()
            {
                { "SubjectSemesterLiteral", subjectApproach.SemesterLiteral },
                { "SubjectId", subjectApproach.Id },
                { "SubjectAcronym", subjectApproach.Acronym }
            };

            await _navigationService.NavigateAsync(name: PageTokens.GradesModuleTestViewToken, parameters: parameters).ConfigureAwait(continueOnCapturedContext: false);
        }

        private string FindHighestSemesterLiteral(IEnumerable<SubjectApproach> approachesList)
        {
            var semesterLiteralAndShortConverter = new SemesterLiteralAndShortConverter();
            short highestSemester = default;
            foreach (var approach in approachesList)
            {
                var literalAsShort = semesterLiteralAndShortConverter.LiteralToShort(semesterLiteral: approach.SemesterLiteral);
                if (literalAsShort > highestSemester)
                    highestSemester = literalAsShort;
            }

            return semesterLiteralAndShortConverter.ShortToString(semesterShort: highestSemester);
        }
    }
}
