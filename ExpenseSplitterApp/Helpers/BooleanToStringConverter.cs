using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSplitterApp.Helpers
{

    public class BooleanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Return "Should Receive" for false, and "Received" for true.
            return value is bool boolValue && boolValue ? "⇨" : "⇦" ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // In case you need to convert back, we'll return true or false based on the string.
            return value is string str && str == "⇦";
        }
    }
}
