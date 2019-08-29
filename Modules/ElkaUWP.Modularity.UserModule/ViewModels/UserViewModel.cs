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
using ElkaUWP.DataLayer.Propertiary.Services;

namespace ElkaUWP.Modularity.UserModule.ViewModels
{
    public class UserViewModel : BindableBase
    {
        private readonly UserService _userService;

        private readonly ResourceLoader _resourceLoader =
            ResourceLoaderHelper.GetResourceLoaderForView(viewType: typeof(UserModuleInitializer));

        public UserViewModel(UserService userService)
        {
            _userService = userService;
        }


    }
}

