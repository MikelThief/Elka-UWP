using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace ElkaUWP.Modularity.GradesModule.Models.Converters
{
    class FinishedSubjectPassedStatusToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool castedValue)
            {
                return castedValue ? new SolidColorBrush(Color.FromArgb(100, 32, 223, 32)) : new SolidColorBrush(Color.FromArgb(100, 255, 0, 44));
            }
            else throw new ArgumentException(message: nameof(FinishedSubjectPassedStatusToBrushConverter) +
                                                      "doesn't support type" + targetType.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
