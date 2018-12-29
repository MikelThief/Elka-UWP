using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ElkaUWP.Modularity.UserModule.ViewModels
{
    public class UserSummaryViewModel : BindableBase
    {
        private Image _userImage;

        public Image UserImage { get => _userImage; private set => SetProperty(storage: ref _userImage, value: value); }
    }
}
