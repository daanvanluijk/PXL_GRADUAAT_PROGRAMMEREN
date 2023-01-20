using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMProject.Converters
{
    public class BoolToBackgroundColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value == true ? typeof(Colors).GetField((string)parameter).GetValue(null) : Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value != true ? typeof(Colors).GetField((string)parameter).GetValue(null) : Colors.Transparent;
        }
    }
}
