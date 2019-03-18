using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorCode.Common;
using ElkaUWP.DataLayer.Propertiary.Converters;
using ElkaUWP.DataLayer.Propertiary.Converters.EntityToEntity;
using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.DataLayer.Usos.Services;
using ElkaUWP.Infrastructure;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Nito.Mvvm;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace ElkaUWP.Modularity.GradesModule.ViewModels
{
    public class GradesViewModel : BindableBase, INavigationAware
    {
        private GradesService _gradesService;
        private INavigationService _navigationService;

        public ObservableCollection<SubjectApproach> InProgressSubjectApproaches = new ObservableCollection<SubjectApproach>();
        public ObservableCollection<SubjectApproach> FinishedSubjectApproaches = new ObservableCollection<SubjectApproach>();

        public DelegateCommand<SubjectApproach> ShowTestsCommand { get; private set; }

        public GradesViewModel(GradesService gradesService)
        {
            _gradesService = gradesService;

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

            var coursesPerSemesterTask = _gradesService.GetUserCoursesPerSemesterAsync();
            var gradedSemestersTask = _gradesService.GetUserGradedSemestersAsync();

            var converter = new UsosSubjectEntitiesToPropertiarySubjectEntitiesConverter();

            converter.Convert(gradedSubjectsPerSemesterDictionary: await gradedSemestersTask,
                coursesPerSemesterDictionary: await coursesPerSemesterTask);

            foreach (var inProgressSubjectApproach in converter.InProgressSubjects)
            {
                // discarding subjects which acronyms are below 3 letters or consists of numbers only (unnecessary *ghost* subjects)
                if (inProgressSubjectApproach.Id.All(c => c >= '0' && c <= '9') || inProgressSubjectApproach.Id.Length < 3)
                    continue;
                InProgressSubjectApproaches.Add(item: inProgressSubjectApproach);
            }
            foreach (var finishedSubjectApproach in converter.FinishedSubjects)
            {
                // discarding subjects which acronyms are below 3 letters or consists of numbers only (unnecessary *ghost* subjects)
                if (finishedSubjectApproach.Acronym.All(c => c >= '0' && c <= '9') || finishedSubjectApproach.Acronym.Length < 3)
                    continue;
                FinishedSubjectApproaches.Add(item: finishedSubjectApproach);
            }
            converter.Flush();

            FinishedSubjectApproaches.SortStable(comparison: (x, y) => string.Compare(strA: x.Acronym, strB: y.Acronym,
                comparisonType: StringComparison.CurrentCultureIgnoreCase));
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
    }
}
