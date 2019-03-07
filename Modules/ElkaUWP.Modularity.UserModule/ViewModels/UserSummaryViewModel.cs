using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using ElkaUWP.DataLayer.Usos.Services;
using ElkaUWP.DataLayer.Propertiary.Entities;
using System.Collections.ObjectModel;
using ElkaUWP.DataLayer.Usos.Entities;
using Nito.Mvvm;
using Prism.Navigation;
using ElkaUWP.Infrastructure.Helpers;
using Windows.ApplicationModel.Resources;

namespace ElkaUWP.Modularity.UserModule.ViewModels
{
    public class UserSummaryViewModel : BindableBase, INavigationAware
    {
        private Image _userImage;
        private UserService _userService;
        private readonly ResourceLoader _resourceLoader = ResourceLoaderHelper.GetResourceLoaderForView(loginViewType: typeof(UserModuleInitializer));

        public ObservableCollection<UserInfoElement> _userInfoElement = new ObservableCollection<UserInfoElement>();
        
        public Image UserImage { get => _userImage; private set => SetProperty(storage: ref _userImage, value: value); }
        private string _nameAndSurname;
        public string NameAndSurname { get => _nameAndSurname; set => SetProperty(storage:ref _nameAndSurname, value: value, nameof(NameAndSurname)); }
        public string _indexNo;
        public string IndexNo { get => _indexNo; set => SetProperty(storage: ref _indexNo, value: value, nameof(IndexNo)); }
        public string _email;
        public string Email { get => _email; set => SetProperty(storage: ref _email, value: value, nameof(Email)); }

        public UserSummaryViewModel(UserService userService)
        {
            _userService = userService;
            
           
        }

        public async void OnNavigatingTo(INavigationParameters parameters)
        {
            var result = await GetUserInfoAsync();

            _nameAndSurname = result.Single(x => x.Header == "FirstNameKey").Value;
            _indexNo = result.Single(x => x.Header == "StudentNumberKey").Value;
            _email = result.Single(x => x.Header == "EmailKey").Value;

            foreach(var item in result)
            {
                
                item.Header = _resourceLoader.GetString(item.Header);
                _userInfoElement.Add(item);
        
            }
            

        }

        public Task<IEnumerable<UserInfoElement>> GetUserInfoAsync()

        {
            return _userService.GetUserInformation();
            
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
          
        }
    }
   


}
