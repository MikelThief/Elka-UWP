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

namespace ElkaUWP.Modularity.UserModule.ViewModels
{
    public class UserSummaryViewModel : BindableBase, INavigationAware
    {
        private Image _userImage;
        private UserService _userService;
        public ObservableCollection<USOSUserInfo> _userInfo = new ObservableCollection<USOSUserInfo>();
        public Image UserImage { get => _userImage; private set => SetProperty(storage: ref _userImage, value: value); }
        public string FirstName;
        public string LastName;
        public string Email;
        public long ID;
        public string Sex;
        public string StudentNumber;
        public PhotoUrls Photo;
        public string NameAndSurname;

        public UserSummaryViewModel(UserService userService)
        {
            _userService = userService;
            
           
        }

        public async void OnNavigatingTo(INavigationParameters parameters)
        {
            await GetUserInfo();
        }

            public async Task GetUserInfo()
        {
            var result = await _userService.GetUserInformation();
            




        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
          
        }
    }
   


}
