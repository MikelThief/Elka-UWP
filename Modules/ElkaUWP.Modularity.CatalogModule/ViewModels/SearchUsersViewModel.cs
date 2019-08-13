using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.System;
using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.DataLayer.Propertiary.Services;
using ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Requests;
using ElkaUWP.DataLayer.Usos.Services;
using ElkaUWP.Infrastructure.Helpers;
using Prism.Mvvm;
using User = ElkaUWP.DataLayer.Usos.Entities.User;

namespace ElkaUWP.Modularity.CatalogModule.ViewModels
{
    public class SearchUsersViewModel : BindableBase
    {
        private readonly SearchService _searchService;
        private readonly UserService _userService;

        private readonly ResourceLoader _resourceLoader =
            ResourceLoaderHelper.GetResourceLoaderForView(viewType: typeof(CatalogModuleInitializer));

        public ObservableCollection<UserMatch> SuggestedItems;

        private StaffUser _staffUser;

        public StaffUser StaffUser
        {
            get => _staffUser;
            set => SetProperty(storage: ref _staffUser, value: value, propertyName: nameof(StaffUser));
        }

        public SearchUsersViewModel(SearchService searchService, UserService userService)
        {
            _searchService = searchService;
            _userService = userService;
            SuggestedItems = new ObservableCollection<UserMatch>();
        }

        public async Task SearchUsersAsync(string query)
        {
            var result = await _searchService.SearchUsersAsync(query: query).ConfigureAwait(continueOnCapturedContext: true);
            SuggestedItems.Clear();

            if (result.IsSuccess && result.Value.HasValue)
            {
                foreach (var userMatch in result.Value.Value)
                {
                    SuggestedItems.Add(item: userMatch);
                }
            }
            else
            {
                var emptyMatch = new UserMatch(firstName: string.Empty, lastName: string.Empty,
                    title: string.Empty, employmentPosition: string.Empty,
                    id: -1, htmlMatchedName: _resourceLoader.GetString(resource: "SearchUsers_NoResultsMessage"));

                SuggestedItems.Add(item: emptyMatch);
            }
        }

        public async Task GetStaffUserDetails(int userId)
        {
            var result = await _userService.GetStaffUserAsync(userId: userId);

            if (result.IsSuccess)
            {
                StaffUser = result.Value;
            }
            else
            {
                // raise notification
            }
        }
    }
}
