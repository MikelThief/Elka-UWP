using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Propertiary;
using ElkaUWP.DataLayer.Usos.Requests;
using ElkaUWP.DataLayer.Usos.Services;
using Prism.Ioc;
using Prism.Modularity;

namespace ElkaUWP.DataLayer
{
    public class DataLayerInitializer : IModule
    {
        /// <inheritdoc />
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register USOS request wrappers
            containerRegistry.RegisterSingleton<CrstestsNodeRequestWrapper>();
            containerRegistry.RegisterSingleton<CrstestsParticipantRequestWrapper>();
            containerRegistry.RegisterSingleton<CrstestsUserPointsRequestWrapper>();
            containerRegistry.RegisterSingleton<ExamrepUser2RequestWrapper>();
            containerRegistry.RegisterSingleton<GradesTerms2RequestWrapper>();
            containerRegistry.RegisterSingleton<TermsSearchRequestWrapper>();
            containerRegistry.RegisterSingleton<CoursesUserRequestWrapper>();
            containerRegistry.RegisterSingleton<StudentTimeTableRequestWrapper>();
            containerRegistry.RegisterSingleton<BuildingIndexRequestWrapper>();
            containerRegistry.RegisterSingleton<UpcomingICalRequestWrapper>();
            containerRegistry.RegisterSingleton<UpcomingWebCalFeedRequestWrapper>();
            containerRegistry.RegisterSingleton<UserInfoRequestWrapper>();

            // Register services
            containerRegistry.RegisterSingleton<CoursesService>();
            containerRegistry.RegisterSingleton<CrstestsService>();
            containerRegistry.RegisterSingleton<ExamrepService>();
            containerRegistry.RegisterSingleton<GradesService>();
            containerRegistry.RegisterSingleton<TermsService>();
            containerRegistry.RegisterSingleton<TimeTableService>();
            containerRegistry.RegisterSingleton<UserService>();
        }

        /// <inheritdoc />
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }
    }
}
