using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Model;

namespace WPF_PC.Converters {
    class LocationToStringConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if(value == null || ((List<Location>)value).Count == 0) { return string.Empty; }
            
            List<Location> locations = (List<Location>)value;
            string result = locations[0].ID;
            for(int i = 1; i < locations.Count; i++) {
                result += ", " + locations[i].ID;
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
