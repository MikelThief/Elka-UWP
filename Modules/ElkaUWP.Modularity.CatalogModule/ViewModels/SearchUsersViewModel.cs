using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Usos.Requests;
using ElkaUWP.DataLayer.Usos.Services;
using Prism.Mvvm;

namespace ElkaUWP.Modularity.CatalogModule.ViewModels
{
    public class SearchUsersViewModel : BindableBase
    {
        private readonly UsersService _usersSerice;

        public ObservableCollection<string> SuggestedItems;
        public SearchUsersViewModel(UsersService usersSerice)
        {
            _usersSerice = usersSerice;
        }

        public async Task SearchUsersAsync(string query, UserCategory category)
        {
            var result = await _usersSerice.Search2Async(query: query, userCategory: UserCategory.CurrentStaff);

            if (result.IsSuccess && result.Value.HasValue)
            {

            }


        }
    }
}
