using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace ElkaUWP.Infrastructure.Helpers
{
    public static class BrushFromColorHelper
    {
        private static Dictionary<string, SolidColorBrush> colors = typeof(Colors)
            .GetProperties(bindingAttr: BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where((p) => p.PropertyType == typeof(Color))
            .ToDictionary((p) => p.Name, (p) => new SolidColorBrush(color: (Color)p.GetValue(null)));

        public static SolidColorBrush GetSolidColorBrush(string colorName)
        {
            return colors[key: colorName];
        }
    }
}
