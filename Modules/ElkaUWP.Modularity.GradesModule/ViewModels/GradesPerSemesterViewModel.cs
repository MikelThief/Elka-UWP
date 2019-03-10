﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Propertiary.Converters.EntityToEntity;
using ElkaUWP.DataLayer.Usos.Services;
using Prism.Mvvm;
using Prism.Navigation;

namespace ElkaUWP.Modularity.GradesModule.ViewModels
{
    public class GradesPerSemesterViewModel : BindableBase, INavigationAware
    {
        private GradesService _gradesService;

        public GradesPerSemesterViewModel(GradesService gradesService)
        {
            _gradesService = gradesService;
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
            var converter = new UsosSubjectEntitiesToPropertiarySubjectEntitiesConverter();

            converter.Convert(gradedSubjectsPerSemesterDictionary: await _gradesService.GetUserGradedSemestersAsync(),
                coursesPerSemesterDictionary: await _gradesService.GetUserCoursesPerSemesterAsync());



        }
    }
}
