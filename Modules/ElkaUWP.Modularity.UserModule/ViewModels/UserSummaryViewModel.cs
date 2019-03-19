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

        public ObservableCollection<UserInfoElement> UserInfoElement = new ObservableCollection<UserInfoElement>();
        
        public Image UserImage { get => _userImage; private set => SetProperty(storage: ref _userImage, value: value); }
        private string _nameAndSurname;
        public string NameAndSurname { get => _nameAndSurname;
            set => SetProperty(storage:ref _nameAndSurname, value: value, nameof(NameAndSurname)); }
        public string _indexNo;
        public string IndexNo { get => _indexNo; set => SetProperty(storage: ref _indexNo, value: value, nameof(IndexNo)); }
        public string _email;
        public string Email { get => _email; set => SetProperty(storage: ref _email, value: value, nameof(Email)); }
        public Uri PhotoUri { get => _photoUri; set => SetProperty(storage: ref _photoUri, value: value, nameof(PhotoUri)); }
        private Uri _photoUri;
        public UserSummaryViewModel(UserService userService)
        {
            _userService = userService;
            
           
        }

        public async void OnNavigatingTo(INavigationParameters parameters)
        {
            var result = await _userService.GetUserInformation();
            
            NameAndSurname = result.Single(x => x.Header == "FirstNameKey").Value + " " + result.Single(x => x.Header == "MiddleNameKey").Value+ " " + result.Single(x => x.Header == "LastNameKey").Value;
            IndexNo = "Index no:" + result.Single(x => x.Header == "StudentNumberKey").Value;
            Email = "Email: " + result.Single(x => x.Header == "EmailKey").Value;
            PhotoUri = new Uri(result.Single(x => x.Header == "PhotoUrlsKey")?.Value);

            foreach(var item in result)
            {
                
                item.Header = _resourceLoader.GetString(item.Header);
                
                UserInfoElement.Add(item);
        
            }
            

        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
          
        }
    }
   


}
