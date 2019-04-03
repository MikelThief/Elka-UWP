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
using ElkaUWP.DataLayer.Propertiary;

namespace ElkaUWP.Modularity.UserModule.ViewModels
{
    public class UserSummaryViewModel : BindableBase, INavigationAware
    {
        private UserService _userService;
        private readonly ResourceLoader _resourceLoader = ResourceLoaderHelper.GetResourceLoaderForView(loginViewType: typeof(UserModuleInitializer));

        private Uri _photoUri;
        public Uri PhotoUri { get => _photoUri; private set => SetProperty(storage: ref _photoUri, value: value, propertyName: nameof(PhotoUri)); }

        public string FullName => FirstName + " " + LastName;

        private string _firstName;

        public string FirstName
        {
            get => _firstName;
            set
            {
                SetProperty(storage: ref _firstName, value: value, propertyName: nameof(FirstName));
                RaisePropertyChanged(propertyName: nameof(FullName));
            }
        }

        private string _lastName;

        public string LastName
        {
            get => _lastName;
            set
            {
                SetProperty(storage: ref _lastName, value: value, propertyName: nameof(LastName));
                RaisePropertyChanged(propertyName: nameof(FullName));
            }
        }

        private string _indexNumber;

        public string IndexNumber
        {
            get => _indexNumber;
            set => SetProperty(storage: ref _indexNumber, value: value, propertyName: nameof(IndexNumber));
        }

        private string _email;

        public string Email
        {
            get => _email;
            set => SetProperty(storage: ref _email, value: value, propertyName: nameof(Email));
        }

        public ObservableCollection<UserInfoElement> UserAttributes = new ObservableCollection<UserInfoElement>();


        public UserSummaryViewModel(UserService userService)
        {
            _userService = userService;
        }

        public async void OnNavigatingTo(INavigationParameters parameters)
        {
            var result = await _userService.GetAsync();

            foreach (var element in result)
            {
                var tempElement = new UserInfoElement();

                switch (element.Header)
                {
                    case "EmailKey":
                        Email = element.Value;
                        break;
                    case "FirstNameKey":
                        FirstName = element.Value;
                        break;
                    case "IdKey":
                        tempElement.Header = _resourceLoader.GetString(resource: "IdKey");
                        tempElement.Value = element.Value;
                        UserAttributes.Add(item: tempElement);
                        break;
                    case "LastNameKey":
                        LastName = element.Value;
                        break;
                    case "MiddleNamesKey":
                        tempElement.Header = _resourceLoader.GetString(resource: "MiddleNamesKey");
                        tempElement.Value = element.Value;
                        if (!string.IsNullOrEmpty(value: element.Value))
                            UserAttributes.Add(item: tempElement);
                        break;
                    case "PeselKey":
                        tempElement.Header = _resourceLoader.GetString(resource: "PeselKey");
                        tempElement.Value = element.Value;
                        if(!string.IsNullOrEmpty(value: element.Value))
                            UserAttributes.Add(item: tempElement);
                        break;
                    case "IndexNumberKey":
                        IndexNumber = element.Value;
                        break;
                    case "SexKey":
                        tempElement.Header = _resourceLoader.GetString(resource: "SexKey");
                        tempElement.Value = element.Value;
                        UserAttributes.Add(item: tempElement);
                        break;
                    case "PrimaryAddressKey":
                        tempElement.Header = _resourceLoader.GetString(resource: "PrimaryAddressKey");
                        tempElement.Value = element.Value;
                        UserAttributes.Add(item: tempElement);
                        break;
                    case "CorrespondenceAddressKey":
                        tempElement.Header = _resourceLoader.GetString(resource: "CorrespondenceAddressKey");
                        tempElement.Value = element.Value;
                        UserAttributes.Add(item: tempElement);
                        break;
                    case "ResidenceAddressKey":
                        tempElement.Header = _resourceLoader.GetString(resource: "ResidenceAddressKey");
                        tempElement.Value = element.Value;
                        UserAttributes.Add(item: tempElement);
                        break;
                    case "OtherAddressKey":
                        tempElement.Header = _resourceLoader.GetString(resource: "OtherAddressKey");
                        tempElement.Value = element.Value;
                        UserAttributes.Add(item: tempElement);
                        break;
                    case "StudentStatusKey":
                        tempElement.Header = _resourceLoader.GetString(resource: "StudentStatusKey");
                        tempElement.Value = _resourceLoader.GetString(resource: element.Value);
                        UserAttributes.Add(item: tempElement);
                        break;
                    case "PhotoUriKey":
                        PhotoUri = new Uri(uriString: element.Value);
                        break;
                }
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
