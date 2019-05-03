using System;
using Windows.UI.Xaml.Data;

namespace ElkaUWP.Infrastructure.Converters
{
    public class EnumToBooleanConverter : IValueConverter
    {
        public Type EnumType { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter is string enumString)
            {
                if (!Enum.IsDefined(enumType: EnumType, value: value))
                {
                    throw new ArgumentException(message: "ExceptionEnumToBooleanConverterValueMustBeAnEnum");
                }

                var enumValue = Enum.Parse(enumType: EnumType, value: enumString);

                return enumValue.Equals(obj: value);
            }

            throw new ArgumentException(message: "ExceptionEnumToBooleanConverterParameterMustBeAnEnumName");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (parameter is string enumString)
            {
                return Enum.Parse(enumType: EnumType, value: enumString);
            }

            throw new ArgumentException(message: "ExceptionEnumToBooleanConverterParameterMustBeAnEnumName");
        }
    }
}
