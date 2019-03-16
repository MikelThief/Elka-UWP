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
using Microsoft.Toolkit.Uwp.UI.Controls;
using Nito.Mvvm;
using Prism.Mvvm;
using Prism.Navigation;

namespace ElkaUWP.Modularity.GradesModule.ViewModels
{
    public class GradesViewModel : BindableBase, INavigationAware
    {
        private GradesService _gradesService;

        public ObservableCollection<SubjectApproach> InProgressSubjectApproaches = new ObservableCollection<SubjectApproach>();
        public ObservableCollection<SubjectApproach> FinishedSubjectApproaches = new ObservableCollection<SubjectApproach>();

        public AsyncCommand ViewTestsCommand { get; private set; }

        public GradesViewModel(GradesService gradesService)
        {
            _gradesService = gradesService;
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
    }
}
