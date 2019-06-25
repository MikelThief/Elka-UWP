using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;

namespace ElkaUWP.Infrastructure.Helpers
{
    public class TitleBarHelper : INotifyPropertyChanged
    {
        private static TitleBarHelper _instance = new TitleBarHelper();
        private static CoreApplicationViewTitleBar _coreTitleBar;
        private Thickness _titlePosition;
        private Visibility _titleVisibility;

        /// <summary>
        /// Initializes a new instance of the <see cref="TitleBarHelper"/> class.
        /// </summary>
        public TitleBarHelper()
        {
            _coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            _coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
            _titlePosition = CalculateTilebarOffset(leftPosition: _coreTitleBar.SystemOverlayLeftInset, height: _coreTitleBar.Height);
            _titleVisibility = Visibility.Visible;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static TitleBarHelper Instance => _instance;

        public CoreApplicationViewTitleBar TitleBar => _coreTitleBar;

        public Thickness TitlePosition
        {
            get => _titlePosition;

            set
            {
                if (value.Left != _titlePosition.Left || value.Top != _titlePosition.Top)
                {
                    _titlePosition = value;
                    PropertyChanged?.Invoke(sender: this, e: new PropertyChangedEventArgs(propertyName: nameof(TitlePosition)));
                }
            }
        }

        public Visibility TitleVisibility
        {
            get => _titleVisibility;

            set
            {
                _titleVisibility = value;
                PropertyChanged?.Invoke(sender: this, e: new PropertyChangedEventArgs(propertyName: nameof(TitleVisibility)));
            }
        }

        public void ExitFullscreen()
        {
            TitleVisibility = Visibility.Visible;
        }

        public void GoFullscreen()
        {
            TitleVisibility = Visibility.Collapsed;
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            TitlePosition = CalculateTilebarOffset(leftPosition: _coreTitleBar.SystemOverlayLeftInset, height: _coreTitleBar.Height);
        }

        private Thickness CalculateTilebarOffset(double leftPosition, double height)
        {
            // top position should be 6 pixels for a 32 pixel high titlebar hence scale by actual height
            var correctHeight = height / 32 * 6;

            return new Thickness(left: leftPosition + 12, top: correctHeight, 0, 0);
        }
    }
}
