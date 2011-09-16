using System;
using System.Collections.Generic;
using System.Windows.Data;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation.ExtensionManagerWindow.Converters
{
    public class StringCollectionsToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType == typeof(string))
            {
                var stringValue = value as string;
                if (stringValue != null)
                {
                    return stringValue;
                }
                var parts = (IEnumerable<string>)value;
                return String.Join(", ", parts);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}