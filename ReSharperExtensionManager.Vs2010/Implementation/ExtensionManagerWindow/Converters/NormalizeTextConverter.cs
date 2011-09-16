using System;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation.ExtensionManagerWindow.Converters
{
    public class NormalizeTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var stringValue = (string)value;
            return String.IsNullOrEmpty(stringValue) ? stringValue : Regex.Replace(stringValue, @"\s+", " ");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}