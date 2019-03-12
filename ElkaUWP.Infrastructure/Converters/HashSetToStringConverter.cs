using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ElkaUWP.Infrastructure.Converters
{
    public class HashSetToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var castedValue = 
                (HashSet<string>) value ?? 
                throw new ArgumentException(message: nameof(HashSetToStringConverter) + "encountered an error", paramName: nameof(value));

            string returnedValue = default;

            return string.Join(separator: ", ", castedValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) { throw new NotImplementedException(); }
    }
}
