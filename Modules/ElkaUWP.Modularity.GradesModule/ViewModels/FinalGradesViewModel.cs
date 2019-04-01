using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using ColorCode.Common;
using ElkaUWP.DataLayer.Propertiary.Converters;
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
        private PartialGradesService _partialGradesService;
        private INavigationService _navigationService;

        public ObservableCollection<SubjectApproach> SubjectApproaches = new ObservableCollection<SubjectApproach>();

        private PartialGradesTree _partialGradesTree;

        public PartialGradesTree PartialGradesTree
        {
            get => _partialGradesTree;
            set => SetProperty(storage: ref _partialGradesTree, value: value, propertyName: nameof(PartialGradesTree));
        }

        private SubjectApproach _selectedSubjectApproach;

        public SubjectApproach SelectedSubjectApproach
        {
            get => _selectedSubjectApproach;
            set => SetProperty(storage: ref _selectedSubjectApproach, value: value, propertyName: nameof(SelectedSubjectApproach));
        }

        public DelegateCommand<SubjectApproach> ShowTestsCommand { get; private set; }

        public FinalGradesViewModel(FinalGradesService finalGradesService, PartialGradesService partialGradesService)
        {
            _finalGradesService = finalGradesService;
            _partialGradesService = partialGradesService;

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
        }

        private async Task LoadDataAsync()
        {
            var subjectApproaches = await _finalGradesService.GetAllAsync();

            // remove artificial, useless subjects
            var filteredSubjectApproaches =
                subjectApproaches.Where(subjectApproach =>
                    !subjectApproach.Acronym.All(c => c >= '0' && c <= '9') 
                    && subjectApproach.Acronym.Length >= 3).ToList();


            var inProgressApproaches =
                filteredSubjectApproaches.Where(predicate: x => x.SemesterLiteral == FindHighestSemesterLiteral(approachesList: filteredSubjectApproaches)).ToList();

            var finishedApproaches = filteredSubjectApproaches.Where(predicate: x => x.SemesterLiteral != FindHighestSemesterLiteral(approachesList: filteredSubjectApproaches)).ToList();
            finishedApproaches.SortStable(comparison: (x, y) => string.Compare(strA: x.Acronym, strB: y.Acronym,
                comparisonType: StringComparison.CurrentCultureIgnoreCase));

            foreach (var inProgressApproach in inProgressApproaches)
            {
                SubjectApproaches.Add(item: inProgressApproach);
            }

            foreach (var finishedApproach in finishedApproaches)
            {
                SubjectApproaches.Add(item: finishedApproach);
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

        public async Task LoadPartialGrades()
        {
            PartialGradesTree = await _partialGradesService.GetAsync(semesterLiteral: SelectedSubjectApproach.SemesterLiteral,
                subjectId: SelectedSubjectApproach.Id);
        }
    }
}
