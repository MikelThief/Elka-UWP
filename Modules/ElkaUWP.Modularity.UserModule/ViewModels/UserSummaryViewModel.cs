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

namespace ElkaUWP.Modularity.UserModule.ViewModels
{
    public class UserSummaryViewModel : BindableBase
    {
        private Image _userImage;
        private UserService _userService;
        public ObservableCollection<UserInfoElement> _userInfo = new ObservableCollection<UserInfoElement>();
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
            GetUserInfo();
           
        }

        public async Task GetUserInfo()
        {
            var result = await _userService.GetUserInformation();
            FirstName = result.FirstName+ " " + result.MiddleName;
            LastName = result.LastName;
            Email = result.Email;
            ID = result.Id;
            Sex = result.Sex;
            StudentNumber = result.StudentNumber;
            Photo = result.PhotoUrls;
            NameAndSurname = result.FirstName + " " + result.MiddleName + " " + result.LastName;
            
                       
        }


    }
   


}
