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
        public string nameAndSurname;
        public string indexNo;
        public string email;

        public UserSummaryViewModel(UserService userService)
        {
            _userService = userService;
            
           
        }

        public async void OnNavigatingTo(INavigationParameters parameters)
        {
            var result = await GetUserInfoAsync();
            foreach(var item in result)
            {
                
                item.Header = _resourceLoader.GetString(item.Header);
                _userInfoElement.Add(item);
                if(item.Header.Equals("First name"))
                {
                    nameAndSurname = item.Value;
                }
                if(item.Header.Equals("Last name"))
                {
                    nameAndSurname += " " +item.Value;
                }
                if(item.Header.Equals("Email"))
                {
                    email = item.Value;
                }
                if(item.Header.Equals("Student number"))
                {
                    indexNo = item.Value;
                }
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
