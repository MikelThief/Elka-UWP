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
        private PostalAddressesService _postalAddressesService;
        private readonly ResourceLoader _resourceLoader = ResourceLoaderHelper.GetResourceLoaderForView(loginViewType: typeof(UserModuleInitializer));

        public ObservableCollection<UserInfoElement> UserInfoElement = new ObservableCollection<UserInfoElement>();
        public ObservableCollection<PostalAddressInfoElement> PostalAddressInfoElement = new ObservableCollection<PostalAddressInfoElement>();

        
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
        public UserSummaryViewModel(UserService userService, PostalAddressesService postalAddressesService)
        {
            _userService = userService;
            _postalAddressesService = postalAddressesService;
           
        }

        public async void OnNavigatingTo(INavigationParameters parameters)
        {
            var result = await _userService.GetUserInformation();

            var localSettingsContainer = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettingsContainer.Values["USOSid"] = result.Single(x => x.Header == "IdKey").Value.ToString();
            if (result.Single(x => x.Header == "MiddleNameKey").Value == null || result.Single(x => x.Header == "MiddleNameKey").Value == "")
            {
                NameAndSurname = result.Single(x => x.Header == "FirstNameKey").Value + " " + result.Single(x => x.Header == "LastNameKey").Value;
            }
            else
            {
                NameAndSurname = result.Single(x => x.Header == "FirstNameKey").Value + " " + result.Single(x => x.Header == "MiddleNameKey").Value + " " + result.Single(x => x.Header == "LastNameKey").Value;
            }
           
            IndexNo = "Index no: " + result.Single(x => x.Header == "StudentNumberKey").Value;
            Email = "Email: " + result.Single(x => x.Header == "EmailKey").Value;
            PhotoUri = new Uri(result.Single(x => x.Header == "PhotoUrlsKey")?.Value);

            //Sex
            if(result.Single(x => x.Header=="SexKey").Value=="F")
            {
                result.Single(x => x.Header == "SexKey").Value = "Female";
            }
            if (result.Single(x => x.Header == "SexKey").Value == "M")
            {
                result.Single(x => x.Header == "SexKey").Value = "Male";
            }

            //Student_status

            if (result.Single(x => x.Header == "StudentStatusKey").Value == "0")
            {
                result.Single(x => x.Header == "StudentStatusKey").Value = "Not a student";
            }
            if (result.Single(x => x.Header == "StudentStatusKey").Value == "1")
            {
                result.Single(x => x.Header == "StudentStatusKey").Value = "Inactive student";
            }
            if (result.Single(x => x.Header == "StudentStatusKey").Value == "2")
            {
                result.Single(x => x.Header == "StudentStatusKey").Value = "Active student";
            }



            foreach (var item in result.Where(x => x.Header!="PhotoUrlsKey" && x.Value!=null && x.Value!="" && x.Header!="FirstNameKey" && x.Header!="LastNameKey" && x.Header!="EmailKey" && x.Header!="StudentNumberKey"))
            {
                   
                    item.Header = _resourceLoader.GetString(item.Header);
                    UserInfoElement.Add(item);
                
            }

            var addresses = await  _postalAddressesService.GetUserAddresses();
            //adding postal addresses
            foreach(var item in addresses)
            {
                item.Type = _resourceLoader.GetString(item.Type);
                UserInfoElement ui = new UserInfoElement();
                ui.Header = item.Type;
                ui.Value = item.Address;
                UserInfoElement.Add(ui);
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
