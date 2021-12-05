using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Warehouse.ClientApp.Converters
{
    [ValueConversion(typeof(bool), typeof(Brush))]
    public class IsErrorBooleanToContextualColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
            {
                throw new InvalidOperationException("The target must be a System.Windows.Media.Brush");
            }
            bool b = (bool)value;

            return new SolidColorBrush(b ? Colors.DarkRed : Colors.DarkGreen);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
