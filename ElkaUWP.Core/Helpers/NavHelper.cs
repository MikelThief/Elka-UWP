using System;

using Microsoft.UI.Xaml.Controls;

using Windows.UI.Xaml;

namespace ElkaUWP.Core.Helpers
{
    public class NavHelper
    {
        public static string GetNavigateTo(NavigationViewItem item)
        {
            return (string)item.GetValue(dp: NavigateToProperty);
        }

        public static void SetNavigateTo(NavigationViewItem item, string value)
        {
            item.SetValue(dp: NavigateToProperty, value: value);
        }

        public static readonly DependencyProperty NavigateToProperty =
            DependencyProperty.RegisterAttached("NavigateTo", propertyType: typeof(string), ownerType: typeof(NavHelper), defaultMetadata: new PropertyMetadata(null));
    }
}
